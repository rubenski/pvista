using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatentVista.Business.Mail
{
    public class EmailAddress
    {
        public String Name { get; set; }
        public String Email { get; set; }

        public EmailAddress(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}