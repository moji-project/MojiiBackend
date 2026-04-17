using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace MojiiBackend.Infrastructure.Emailing;

public class EmailService(IConfiguration config)
{
    public async Task SendConfirmationCodeAsync(string toEmail, string userName, string code)
    {
        var (host, port, username, password, senderEmail, senderName) = GetSettings();

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(senderName, senderEmail));
        email.To.Add(new MailboxAddress(userName, toEmail));
        email.Subject = "Ton code de confirmation Mojii";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; border: 1px solid #ddd; padding: 20px;'>
                    <h2 style='color: #4A90E2;'>Bienvenue sur Mojii !</h2>
                    <p>Bonjour <strong>{WebUtility.HtmlEncode(userName)}</strong>,</p>
                    <p>Voici ton code de confirmation pour finaliser ton inscription :</p>
                    <div style='background: #f0f0f0; padding: 10px; text-align: center; font-size: 24px; font-weight: bold;'>
                        {WebUtility.HtmlEncode(code)}
                    </div>
                    <p>Ce code expire dans 15 minutes.</p>
                </div>"
        };

        email.Body = bodyBuilder.ToMessageBody();
        await SendEmailAsync(email, host, port, username, password);
    }

    public async Task SendReportNotificationAsync(
        string toEmail,
        int reportId,
        int reporterUserId,
        string? reporterEmail,
        string? reporterFirstName,
        string? reporterLastName,
        string reason,
        string status,
        int? targetUserId,
        int? targetPostId,
        int? targetCommentId,
        string? comment)
    {
        var (host, port, username, password, senderEmail, senderName) = GetSettings();

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(senderName, senderEmail));
        email.To.Add(new MailboxAddress("Moderation", toEmail));
        email.Subject = $"[Mojii] Nouveau signalement #{reportId}";

        var safeComment = string.IsNullOrWhiteSpace(comment)
            ? "(Aucun commentaire)"
            : WebUtility.HtmlEncode(comment);
        var safeReporterEmail = string.IsNullOrWhiteSpace(reporterEmail) ? "(inconnu)" : WebUtility.HtmlEncode(reporterEmail);
        var safeReporterFirstName = string.IsNullOrWhiteSpace(reporterFirstName) ? "-" : WebUtility.HtmlEncode(reporterFirstName);
        var safeReporterLastName = string.IsNullOrWhiteSpace(reporterLastName) ? "-" : WebUtility.HtmlEncode(reporterLastName);

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; border: 1px solid #ddd; padding: 20px;'>
                    <h2 style='color: #E67E22;'>Nouveau signalement recu</h2>
                    <p><strong>Report ID:</strong> {reportId}</p>
                    <p><strong>ReporterUserId:</strong> {reporterUserId}</p>
                    <p><strong>Reporter email:</strong> {safeReporterEmail}</p>
                    <p><strong>Reporter prenom:</strong> {safeReporterFirstName}</p>
                    <p><strong>Reporter nom:</strong> {safeReporterLastName}</p>
                    <p><strong>Reason:</strong> {WebUtility.HtmlEncode(reason)}</p>
                    <p><strong>Status:</strong> {WebUtility.HtmlEncode(status)}</p>
                    <p><strong>TargetUserId:</strong> {(targetUserId?.ToString() ?? "null")}</p>
                    <p><strong>TargetPostId:</strong> {(targetPostId?.ToString() ?? "null")}</p>
                    <p><strong>TargetCommentId:</strong> {(targetCommentId?.ToString() ?? "null")}</p>
                    <p><strong>Comment:</strong> {safeComment}</p>
                </div>"
        };

        email.Body = bodyBuilder.ToMessageBody();
        await SendEmailAsync(email, host, port, username, password);
    }

    public async Task SendFeedbackEmailAsync(
        string toEmail,
        string subject,
        string message,
        int senderUserId,
        string? senderEmail,
        string? senderFirstName,
        string? senderLastName)
    {
        var (host, port, username, password, senderSystemEmail, senderSystemName) = GetSettings();

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(senderSystemName, senderSystemEmail));
        email.To.Add(new MailboxAddress("Feedback", toEmail));

        var safeSubject = WebUtility.HtmlEncode(subject);
        var safeMessage = WebUtility.HtmlEncode(message).Replace("\n", "<br />");
        var safeSenderEmail = string.IsNullOrWhiteSpace(senderEmail) ? "(inconnu)" : WebUtility.HtmlEncode(senderEmail);
        var safeSenderFirstName = string.IsNullOrWhiteSpace(senderFirstName) ? "-" : WebUtility.HtmlEncode(senderFirstName);
        var safeSenderLastName = string.IsNullOrWhiteSpace(senderLastName) ? "-" : WebUtility.HtmlEncode(senderLastName);

        email.Subject = $"[Mojii][Feedback] {subject}";

        if (!string.IsNullOrWhiteSpace(senderEmail))
        {
            email.ReplyTo.Add(new MailboxAddress(
                $"{senderFirstName ?? string.Empty} {senderLastName ?? string.Empty}".Trim(),
                senderEmail));
        }

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; border: 1px solid #ddd; padding: 20px;'>
                    <h2 style='color: #2E86C1;'>Nouveau feedback utilisateur</h2>
                    <p><strong>Sujet:</strong> {safeSubject}</p>
                    <p><strong>Message:</strong><br />{safeMessage}</p>
                    <hr />
                    <p><strong>SenderUserId:</strong> {senderUserId}</p>
                    <p><strong>Sender email:</strong> {safeSenderEmail}</p>
                    <p><strong>Sender prenom:</strong> {safeSenderFirstName}</p>
                    <p><strong>Sender nom:</strong> {safeSenderLastName}</p>
                </div>"
        };

        email.Body = bodyBuilder.ToMessageBody();
        await SendEmailAsync(email, host, port, username, password);
    }

    private (string host, int port, string username, string password, string senderEmail, string senderName) GetSettings()
    {
        var settings = config.GetSection("BrevoSettings");

        var host = settings["Host"] ?? throw new InvalidOperationException("BrevoSettings:Host is missing.");
        var portValue = settings["Port"] ?? throw new InvalidOperationException("BrevoSettings:Port is missing.");
        var username = settings["Username"] ?? throw new InvalidOperationException("BrevoSettings:Username is missing.");
        var password = settings["Password"] ?? throw new InvalidOperationException("BrevoSettings:Password is missing.");
        var senderEmail = settings["SenderEmail"] ?? throw new InvalidOperationException("BrevoSettings:SenderEmail is missing.");
        var senderName = settings["SenderName"] ?? "Mojii";

        if (!int.TryParse(portValue, out var port))
            throw new InvalidOperationException("BrevoSettings:Port must be a valid integer.");

        return (host, port, username, password, senderEmail, senderName);
    }

    private static async Task SendEmailAsync(
        MimeMessage email,
        string host,
        int port,
        string username,
        string password)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(email);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SMTP ERROR] : {ex.Message}");
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}
