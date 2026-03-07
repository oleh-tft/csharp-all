using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace csharp_all.Services.Email
{
    internal class GmailService : IEmailService
    {
        private JsonElement? gmailSection;

        public async Task SendAsync(MailMessage mailMessage)
        {
            if (gmailSection is null)
            {
                String settingsFilename = "appsettings.json";
                if (!File.Exists(settingsFilename))
                {
                    Console.WriteLine("Не знайдено файл конфігурації. Прочитайте README");
                    return;
                }
                var settings = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(settingsFilename));
                var emailSection = settings.GetProperty("Emails");
                gmailSection = emailSection.GetProperty("Gmail");
                if (gmailSection is null)
                {
                    throw new Exception("Помилка конфігурації");
                } 
            }
            using SmtpClient smtpClient = new()
            {
                Host = gmailSection!.Value.GetProperty("Server").GetString()!,
                Port = gmailSection!.Value.GetProperty("Port").GetInt32(),
                EnableSsl = gmailSection!.Value.GetProperty("Ssl").GetBoolean(),
                Credentials = new NetworkCredential(
                        gmailSection!.Value.GetProperty("Username").GetString()!,
                        gmailSection!.Value.GetProperty("Password").GetString()!
                    )
            };
            mailMessage.From = new MailAddress(gmailSection!.Value.GetProperty("Username").GetString()!, gmailSection!.Value.GetProperty("Password").GetString()!);
            smtpClient.Send(mailMessage);
        }
    }
}
