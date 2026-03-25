using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace MojiiBackend.Infrastructure.Emailing;

public class EmailService(IConfiguration _config)
{
    public async Task SendConfirmationCodeAsync(string toEmail, string userName, string code)
    {
        // 1. Extraction des variables depuis le appsettings.json
        var settings = _config.GetSection("BrevoSettings");
        
        string host = settings["Host"]!;
        int port = int.Parse(settings["Port"]!);
        string username = settings["Username"]!;
        string password = settings["Password"]!;
        string senderEmail = settings["SenderEmail"]!;
        string senderName = settings["SenderName"]!;

        // 2. Construction de l'objet Email
        var email = new MimeMessage();

        // Expéditeur
        email.From.Add(new MailboxAddress(senderName, senderEmail));

        // Destinataire
        email.To.Add(new MailboxAddress(userName, toEmail));

        email.Subject = "Ton code de confirmation Mojii";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; border: 1px solid #ddd; padding: 20px;'>
                    <h2 style='color: #4A90E2;'>Bienvenue sur Mojii !</h2>
                    <p>Bonjour <strong>{userName}</strong>,</p>
                    <p>Voici ton code de confirmation pour finaliser ton inscription :</p>
                    <div style='background: #f0f0f0; padding: 10px; text-align: center; font-size: 24px; font-weight: bold;'>
                        {code}
                    </div>
                    <p>Ce code expire dans 15 minutes.</p>
                </div>"
        };
        email.Body = bodyBuilder.ToMessageBody();

        // 3. Envoi via MailKit
        using var client = new SmtpClient();
        try
        {
            // On utilise les variables extraites plus haut
            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(email);
        }
        catch (Exception ex)
        {
            // Log l'erreur pour ton projet scolaire
            Console.WriteLine($"[SMTP ERROR] : {ex.Message}");
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}