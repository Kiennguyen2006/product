using System.Net;
using System.Net.Mail;

namespace Nkklession14layout.Services;

public interface NkkIEmailSender
{
    bool NkkIsConfigured { get; }
    Task NkkSendPasswordResetCodeAsync(string email, string code, CancellationToken cancellationToken);
}

public sealed class NkkSmtpEmailSender(IConfiguration configuration) : NkkIEmailSender
{
    private string? NkkHost => configuration["Email:SmtpHost"];
    private string? NkkFrom => configuration["Email:From"];

    public bool NkkIsConfigured =>
        !string.IsNullOrWhiteSpace(NkkHost) &&
        !string.IsNullOrWhiteSpace(NkkFrom);

    public async Task NkkSendPasswordResetCodeAsync(string email, string code, CancellationToken cancellationToken)
    {
        if (!NkkIsConfigured)
        {
            throw new InvalidOperationException("Email SMTP settings are not configured.");
        }

        using var message = new MailMessage(NkkFrom!, email)
        {
            Subject = "Mã xác nhận đặt lại mật khẩu",
            Body = $"Mã xác nhận của bạn là {code}. Mã có hiệu lực trong 5 phút.",
            IsBodyHtml = false
        };

        using var client = new SmtpClient(NkkHost!, int.TryParse(configuration["Email:SmtpPort"], out var port) ? port : 587)
        {
            EnableSsl = configuration.GetValue("Email:EnableSsl", true)
        };

        var userName = configuration["Email:UserName"];
        var password = configuration["Email:Password"];
        if (!string.IsNullOrWhiteSpace(userName))
        {
            client.Credentials = new NetworkCredential(userName, password);
        }

        await client.SendMailAsync(message, cancellationToken);
    }
}
