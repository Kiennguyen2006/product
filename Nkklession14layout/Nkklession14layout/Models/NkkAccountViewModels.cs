using System.ComponentModel.DataAnnotations;

namespace Nkklession14layout.Models;

public class NkkLoginViewModel
{
    [Required, StringLength(100)]
    [Display(Name = "Tên đăng nhập hoặc email")]
    public string Identifier { get; set; } = "";

    [Required, DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = "";

    public string? ReturnUrl { get; set; }
}

public class NkkRegisterViewModel
{
    [Required, StringLength(50, MinimumLength = 3)]
    [Display(Name = "Tên đăng nhập")]
    public string UserName { get; set; } = "";

    [Required, EmailAddress, StringLength(100)]
    [Display(Name = "Email")]
    public string Email { get; set; } = "";

    [Required, StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = "";

    [Required, Compare(nameof(Password))]
    [DataType(DataType.Password)]
    [Display(Name = "Nhập lại mật khẩu")]
    public string ConfirmPassword { get; set; } = "";
}

public class NkkForgotPasswordViewModel
{
    [Required, EmailAddress, StringLength(100)]
    [Display(Name = "Email đăng ký")]
    public string Email { get; set; } = "";
}

public class NkkVerifyOtpViewModel
{
    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = "";

    [Required, RegularExpression(@"^\d{6}$", ErrorMessage = "Mã xác nhận gồm 6 chữ số.")]
    [Display(Name = "Mã xác nhận")]
    public string Code { get; set; } = "";
}

public class NkkResetPasswordViewModel
{
    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = "";

    [Required]
    public string Ticket { get; set; } = "";

    [Required, StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu mới")]
    public string Password { get; set; } = "";

    [Required, Compare(nameof(Password))]
    [DataType(DataType.Password)]
    [Display(Name = "Nhập lại mật khẩu mới")]
    public string ConfirmPassword { get; set; } = "";
}

