using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
// TODO: externalize mail settings
using Lucene.Net.Messages;

namespace PatentVista.Business.Mail
{
    public class MailHelper
    {
        public static void SendMail(EmailMessage message)
        {
            using (var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("rubenski", "Hijiszoleuk"),
                DeliveryMethod = SmtpDeliveryMethod.Network
            })
            {
                var mail = new MailMessage();

                foreach (EmailAddress address in message.Recipients)
                {
                    mail.To.Add(address.Email);   
                }
                
                mail.From = new MailAddress(message.From.Email, message.From.Name);
                mail.Subject = message.Subject;
                mail.Body = message.Message;
                mail.IsBodyHtml = false;
                client.Send(mail);
                
            }
        }
    }
}