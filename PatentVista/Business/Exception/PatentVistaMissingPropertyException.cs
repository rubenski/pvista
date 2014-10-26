using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatentVista.Business.Exception 
{
    public class PatentVistaMissingPropertyException : System.Exception{
        public PatentVistaMissingPropertyException(string message) : base(message)
        {
        }
    }
}