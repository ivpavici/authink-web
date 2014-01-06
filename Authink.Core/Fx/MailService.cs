using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Authink.Core.Fx
{
    public class MailService
    {
        public void Send(string to, string from, string body, string subject)
        {
            var emailMessage = new MailMessage { Body = body, Subject = subject, IsBodyHtml = true };

            emailMessage.From = new MailAddress(from);
            emailMessage.To.Add(to);

            var smtpClient = new SmtpClient
            {
                Host = "mail2.cloud.hr",
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("password@authink.eu", "O8QKw1[w1rQiX2O8%VYa@")
            };
            try
            {
                smtpClient.Send(emailMessage);
            }
            catch (SmtpException ex)
            {
                throw;
            }
        }
    }
}
