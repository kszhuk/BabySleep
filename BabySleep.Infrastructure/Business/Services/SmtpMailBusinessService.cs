using BabySleep.Domain.Models;
using BabySleep.Infrastructure.Business.Interfaces;
using System.Net;
using System.Net.Mail;

namespace BabySleep.Infrastructure.Business.Services
{
    public class SmtpMailBusinessService : ISmtpMailBusinessService
    {
        public void Send(EmailMessage message)
        {
            using (var smtp = new SmtpClient())
            {
                var email = "babysleepweb@gmail.com";
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential(email, "ksktlwupanplsjvj");////"Passw0rdBabySleep",

                var body = "<p>Email From: {0} </p><p>Message:</p><p>{1}</p>";
                var msg = new MailMessage(message.Email, email, message.Subject,
                    string.Format(body, message.Email, message.Body));
                msg.IsBodyHtml = true;

                smtp.Send(msg);
            }
        }
    }
}
