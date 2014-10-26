using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatentVista.Business.Exception
{
    public class PatentVistaMissingValueException : System.Exception
    {
        public PatentVistaMissingValueException(string message) : base(message)
        {
 
        }
    }
}