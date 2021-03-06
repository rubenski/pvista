﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatentVista.Models
{
    public class ContactFormModel
    {
        private string _message;
        public String RealName { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String RealEmail { get; set; }
        
        public String Message
        {
            get { return _message; }
            set
            {
                _message = value.Replace("\n", "<br />");
            }
        }

        public String Subject { get; set; }
    }
}