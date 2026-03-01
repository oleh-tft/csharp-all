using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace csharp_all.Networking
{
    internal class EmailDemo
    {
        public void Run()
        {
            String settingsFilename = "appsettings.json";
            if (!File.Exists(settingsFilename))
            {
                Console.WriteLine("Не знайдено файл конфігурації. Прочитайте README");
                return;
            }
            var settings = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(settingsFilename));
            String server, email, password;
            int port;
            bool isSsl;
            try
            {
                var emailSection = settings.GetProperty("Emails");
                var gmailSection = emailSection.GetProperty("Gmail");
                server = gmailSection.GetProperty("Server").GetString()!;
                email = gmailSection.GetProperty("Username").GetString()!;
                password = gmailSection.GetProperty("Password").GetString()!;
                port = gmailSection.GetProperty("Port").GetInt32();
                isSsl = gmailSection.GetProperty("Ssl").GetBoolean();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка визначення конфігурації: {ex.Message}");
                return;
            }

            using SmtpClient smtpClient = new()
            {
                Host = server,
                Port = port,
                EnableSsl = isSsl,
                Credentials = new NetworkCredential(email, password),
            };

            SendMail(smtpClient, "olegit101@gmail.com", "oleh369@proton.me", "Test Subject", "<html><h2 style=\"color: #00FF00;\">Вигідна пропозиція! лише <strike>0.99₴</strike> <span style=\"color: red\">3.99₴</span></h2><img style=\"width: 350px; height: 350px;\" src=\"https://www.soulfullymade.com/wp-content/uploads/2023/03/cake-brownies-recipe-square-featured.jpg\" alt=\"\"></html>");
            SendMail(smtpClient, "olegit101@gmail.com", "oleh369@proton.me", "Subject 2", "<html><h1>Wow</h1></html>");
        }

        private void SendMail(SmtpClient smtpClient, String emailFrom, String emailTo, String subject, String body)
        {
            try
            {
                MailMessage mailMessage = new()
                {
                    From = new MailAddress(emailFrom),
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body
                };
                mailMessage.To.Add(new MailAddress(emailTo));
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while sending e-mail: " + ex.Message);
            }
        }

        public void Run2()
        {
            Console.WriteLine("Робота з електронною поштою. SMTP");
            String settingsFilename = "appsettings.json";
            if (!File.Exists(settingsFilename))
            {
                Console.WriteLine("Не знайдено файл конфігурації. Прочитайте README");
                return;
            }
            var settings = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(settingsFilename));
            String server, email, password;
            int port;
            bool isSsl;
            try
            {
                var emailSection = settings.GetProperty("Emails");
                var gmailSection = emailSection.GetProperty("Gmail");
                server = gmailSection.GetProperty("Server").GetString()!;
                email = gmailSection.GetProperty("Username").GetString()!;
                password = gmailSection.GetProperty("Password").GetString()!;
                port = gmailSection.GetProperty("Port").GetInt32();
                isSsl = gmailSection.GetProperty("Ssl").GetBoolean();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка визначення конфігурації: {ex.Message}");
                return;
            }

            using SmtpClient smtpClient = new()
            {
                Host = server,
                Port = port,
                EnableSsl = isSsl,
                Credentials = new NetworkCredential(email, password),
            };

            //smtpClient.Send(email, "oleh369@proton.me", "Message from Sharp", "I am your fan!");
            //smtpClient.Send(email, "azure.spd111.od.0@ukr.net", "Message from Sharp", "I am your fan!");

            MailMessage mailMessage = new()
            {
                From = new MailAddress(email),
                IsBodyHtml = true,
                Subject = "Message from Sharp",
                Body = "<html><h2 style=\"color: #00FF00;\">Вигідна пропозиція! лише <strike>0.99₴</strike> <span style=\"color: red\">3.99₴</span></h2><img style=\"width: 350px; height: 350px;\" src=\"https://www.soulfullymade.com/wp-content/uploads/2023/03/cake-brownies-recipe-square-featured.jpg\" alt=\"\"></html>"
            };
            mailMessage.To.Add(new MailAddress("oleh369@proton.me"));
            smtpClient.Send(mailMessage);
        }
    }
}
