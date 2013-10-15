using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatentVista.Business.Mail
{
    public class EmailMessage
    {
        public IList<EmailAddress> Recipients = new List<EmailAddress>();
        public EmailAddress From { get; set; }
        public String Subject { get; set; }
        public String Message { get; set; }

        public void AddRecipient(String name, String emailAddress)
        {
            Recipients.Add(new EmailAddress(name, emailAddress));
        }
    }
}