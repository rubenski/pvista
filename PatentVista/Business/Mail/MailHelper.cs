using System;
using System.Collections.Generic;
using System.Configuration;
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

            var host = Properties.Settings.Default.SmtpHost;
            var port = Properties.Settings.Default.SmtpPort;
            var user = Properties.Settings.Default.SmtpUser;
            var pass = Properties.Settings.Default.SmtpPass;

            using (var client = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                Credentials = new NetworkCredential(user, pass),
                DeliveryMethod = SmtpDeliveryMethod.Network
            })
            {
                var mail = new MailMessage();


                foreach (EmailAddress address in message.ToRecipients)
                {
                    mail.To.Add(address.Email);   
                }
                
                mail.From = new MailAddress(message.From.Email, message.From.Name);
                mail.Subject = message.Subject;
                mail.Body = message.Message;
                mail.IsBodyHtml = true;
                client.Send(mail);
                
            }
        }
    }
}