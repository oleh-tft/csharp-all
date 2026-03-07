using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Services.Email
{
    internal interface IEmailService
    {
        public Task SendAsync(MailMessage message);
    }
}
