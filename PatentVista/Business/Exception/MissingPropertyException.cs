using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatentVista.Business.Exception 
{
    public class MissingPropertyException : System.Exception{
        public MissingPropertyException(string message) : base(message)
        {
        }
    }
}