using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatentVista.Business.Exception
{
    public class MissingValueException : System.Exception
    {
        public MissingValueException(string message) : base(message)
        {
 
        }
    }
}