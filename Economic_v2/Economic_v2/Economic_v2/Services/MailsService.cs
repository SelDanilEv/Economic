using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Economic_v2.Services
{
    public class MailsService
    {
        public static void SendEmailAsync(string emailTo, string title, string displayName, string htmlBody = "")
        {
            (new Task(() =>
            {
                string login = ConfigurationManager.AppSettings["emailLogin"];
                string password = ConfigurationManager.AppSettings["emailPass"];

                MailAddress from = new MailAddress(login, displayName);
                MailAddress to = new MailAddress(emailTo);
                MailMessage message = new MailMessage(from, to)
                {
                    Subject = title,
                    Body = htmlBody,
                    IsBodyHtml = true
                };

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(login, password),
                    EnableSsl = true,
                };

                smtp.SendMailAsync(message);
            })).Start();
        }
    }
}
