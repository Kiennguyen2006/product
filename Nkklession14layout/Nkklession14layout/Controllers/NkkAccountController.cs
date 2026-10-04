using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Nkklession14layout.Data;
using Nkklession14layout.Models;
using Nkklession14layout.Services;

namespace Nkklession14layout.Controllers;

[AllowAnonymous]
public class NkkAccountController(
    NkkApplicationDbContext db,
    PasswordHasher<NkkTaiKhoanModel> passwordHasher,
    IMemoryCache cache,
    NkkIEmailSender emailSender,
    IWebHostEnvironment environment,
    ILogger<NkkAccountController> logger) : Controller
{
    private static readonly TimeSpan OtpLifetime = TimeSpan.FromMinutes(5);
    private const int MaximumOtpAttempts = 5;

    [HttpGet]
    public IActionResult NkkLogin(string? returnUrl = null) =>
        View(new NkkLoginViewModel { ReturnUrl = returnUrl });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NkkLogin(NkkLoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var identifier = model.Identifier.Trim();
        var account = await db.TaiKhoans.FirstOrDefaultAsync(x =>
            x.TenDangNhap == identifier || x.Email == identifier);
        if (account is null || !NkkVerifyPassword(account, model.Password))
        {
            ModelState.AddModelError(string.Empty, "Tên đăng nhập/email hoặc mật khẩu không chính xác.");
            return View(model);
        }

        if (!NkkIsIdentityHash(account.MatKhau))
        {
            account.MatKhau = passwordHasher.HashPassword(account, model.Password);
            await db.SaveChangesAsync();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, account.MaTaiKhoan),
            new Claim(ClaimTypes.Name, account.TenDangNhap),
            new Claim(ClaimTypes.Role, account.PhanQuyen)
        };
        var identity = new ClaimsIdentity(claims, "ShopCookie");
        await HttpContext.SignInAsync("ShopCookie", new ClaimsPrincipal(identity));

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return LocalRedirect(model.ReturnUrl);
        }

        return RedirectToAction("NkkIndex", "NkkShop");
    }

    [HttpGet]
    public IActionResult NkkRegister() => View(new NkkRegisterViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NkkRegister(NkkRegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userName = model.UserName.Trim();
        var email = model.Email.Trim();
        if (await db.TaiKhoans.AnyAsync(x => x.TenDangNhap == userName))
        {
            ModelState.AddModelError(nameof(model.UserName), "Tên đăng nhập đã được sử dụng.");
        }

        if (await db.TaiKhoans.AnyAsync(x => x.Email == email))
        {
            ModelState.AddModelError(nameof(model.Email), "Email đã được sử dụng.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await using var transaction = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var accountIds = await db.TaiKhoans
            .Select(x => x.MaTaiKhoan)
            .ToListAsync();
        var lastNumber = accountIds
            .Where(x => x.StartsWith("TK", StringComparison.OrdinalIgnoreCase))
            .Select(x => int.TryParse(x.AsSpan(2), out var number) ? number : 0)
            .DefaultIfEmpty(0)
            .Max();

        var account = new NkkTaiKhoanModel
        {
            MaTaiKhoan = $"TK{lastNumber + 1:000}",
            TenDangNhap = userName,
            Email = email,
            NgayTao = DateTime.Now,
            PhanQuyen = "NhanVien"
        };
        account.MatKhau = passwordHasher.HashPassword(account, model.Password);
        db.TaiKhoans.Add(account);

        try
        {
            await db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception)
        {
            await transaction.RollbackAsync();
            logger.LogWarning(exception, "Account registration conflicted with a database constraint.");
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc email đã được sử dụng. Vui lòng kiểm tra lại.");
            return View(model);
        }

        TempData["Success"] = "Tạo tài khoản thành công. Bạn có thể đăng nhập.";
        return RedirectToAction(nameof(NkkLogin));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NkkLogout()
    {
        await HttpContext.SignOutAsync("ShopCookie");
        return RedirectToAction(nameof(NkkLogin));
    }

    [HttpGet]
    public IActionResult NkkForgotPassword() => View(new NkkForgotPasswordViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NkkForgotPassword(NkkForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var email = model.Email.Trim();
        var account = await db.TaiKhoans.FirstOrDefaultAsync(x => x.Email == email);
        var canDeliver = emailSender.NkkIsConfigured || environment.IsDevelopment();
        if (!canDeliver)
        {
            logger.LogError("Password reset was requested but SMTP is not configured.");
            ViewBag.StatusMessage = "Dịch vụ email chưa được cấu hình nên hiện không thể gửi mã xác nhận. Vui lòng liên hệ quản trị viên.";
            return View(model);
        }

        if (account is not null)
        {
            var code = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
            cache.Set(NkkOtpCacheKey(email), new NkkOtpState(account.MaTaiKhoan, NkkHashCode(code)), OtpLifetime);

            if (emailSender.NkkIsConfigured)
            {
                try
                {
                    await emailSender.NkkSendPasswordResetCodeAsync(email, code, HttpContext.RequestAborted);
                }
                catch (Exception exception) when (exception is SmtpException or InvalidOperationException)
                {
                    cache.Remove(NkkOtpCacheKey(email));
                    logger.LogError(exception, "Could not send a password reset code.");
                    ViewBag.StatusMessage = "Không thể gửi mã xác nhận lúc này. Vui lòng thử lại sau.";
                    return View(model);
                }
            }
            else
            {
                ViewBag.DevelopmentOtp = code;
                logger.LogInformation("A password reset code was generated using the development-only delivery path.");
            }
        }

        ViewBag.StatusMessage = "Nếu email tồn tại trong hệ thống, mã xác nhận đã được gửi. Mã có hiệu lực trong 5 phút.";
        return View("NkkVerifyOtp", new NkkVerifyOtpViewModel { Email = email });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult NkkVerifyOtp(NkkVerifyOtpViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var key = NkkOtpCacheKey(model.Email.Trim());
        if (!cache.TryGetValue<NkkOtpState>(key, out var state) || state is null)
        {
            ModelState.AddModelError(string.Empty, "Mã không hợp lệ hoặc đã hết hạn. Hãy yêu cầu mã mới.");
            return View(model);
        }

        state.Attempts++;
        if (state.Attempts > MaximumOtpAttempts)
        {
            cache.Remove(key);
            ModelState.AddModelError(string.Empty, "Bạn đã nhập sai quá số lần cho phép. Hãy yêu cầu mã mới.");
            return View(model);
        }

        if (!CryptographicOperations.FixedTimeEquals(
                Convert.FromHexString(state.CodeHash),
                Convert.FromHexString(NkkHashCode(model.Code))))
        {
            ModelState.AddModelError(nameof(model.Code), "Mã xác nhận không chính xác.");
            return View(model);
        }

        cache.Remove(key);
        var ticket = WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32));
        cache.Set(NkkResetCacheKey(ticket), new NkkResetState(state.AccountId, model.Email.Trim()), OtpLifetime);
        return RedirectToAction(nameof(NkkResetPassword), new { email = model.Email.Trim(), ticket });
    }

    [HttpGet]
    public IActionResult NkkResetPassword(string email, string ticket)
    {
        if (string.IsNullOrWhiteSpace(email) || !NkkIsResetTicketValid(ticket, email))
        {
            TempData["Error"] = "Yêu cầu đặt lại mật khẩu không hợp lệ hoặc đã hết hạn.";
            return RedirectToAction(nameof(NkkForgotPassword));
        }

        return View(new NkkResetPasswordViewModel { Email = email, Ticket = ticket });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NkkResetPassword(NkkResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!cache.TryGetValue<NkkResetState>(NkkResetCacheKey(model.Ticket), out var reset) ||
            reset is null ||
            !string.Equals(reset.Email, model.Email, StringComparison.OrdinalIgnoreCase))
        {
            TempData["Error"] = "Yêu cầu đặt lại mật khẩu không hợp lệ hoặc đã hết hạn.";
            return RedirectToAction(nameof(NkkForgotPassword));
        }

        var account = await db.TaiKhoans.FirstOrDefaultAsync(x => x.MaTaiKhoan == reset.AccountId);
        if (account is null)
        {
            cache.Remove(NkkResetCacheKey(model.Ticket));
            TempData["Error"] = "Tài khoản không còn tồn tại.";
            return RedirectToAction(nameof(NkkForgotPassword));
        }

        account.MatKhau = passwordHasher.HashPassword(account, model.Password);
        await db.SaveChangesAsync();
        cache.Remove(NkkResetCacheKey(model.Ticket));
        TempData["Success"] = "Đặt lại mật khẩu thành công. Bạn có thể đăng nhập bằng mật khẩu mới.";
        return RedirectToAction(nameof(NkkLogin));
    }

    private bool NkkVerifyPassword(NkkTaiKhoanModel account, string password)
    {
        if (NkkIsIdentityHash(account.MatKhau))
        {
            return passwordHasher.VerifyHashedPassword(account, account.MatKhau, password)
                != PasswordVerificationResult.Failed;
        }

        var storedBytes = Encoding.UTF8.GetBytes(account.MatKhau);
        var enteredBytes = Encoding.UTF8.GetBytes(password);
        return storedBytes.Length == enteredBytes.Length &&
            CryptographicOperations.FixedTimeEquals(storedBytes, enteredBytes);
    }

    private static bool NkkIsIdentityHash(string value) => value.StartsWith("AQAAAA", StringComparison.Ordinal);
    private static string NkkHashCode(string code) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(code)));
    private static string NkkOtpCacheKey(string email) => $"password-otp:{email.Trim().ToUpperInvariant()}";
    private static string NkkResetCacheKey(string ticket) => $"password-reset:{ticket}";

    private bool NkkIsResetTicketValid(string ticket, string email) =>
        cache.TryGetValue<NkkResetState>(NkkResetCacheKey(ticket), out var state) &&
        state is not null &&
        string.Equals(state.Email, email, StringComparison.OrdinalIgnoreCase);

    private sealed class NkkOtpState(string accountId, string codeHash)
    {
        public string AccountId { get; } = accountId;
        public string CodeHash { get; } = codeHash;
        public int Attempts { get; set; }
    }

    private sealed record NkkResetState(string AccountId, string Email);
}
