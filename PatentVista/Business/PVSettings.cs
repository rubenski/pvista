using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PatentVista.Business.Exception;
using Umbraco.Core.Models;
using umbraco;


namespace PatentVista.Business
{
    public class PvSettings
    {
        public static string Get(string name)
        {
            var settings = PvNodes.GetSettings();

            Property property = null;
            try
            {
                property = settings.Properties.First(x => x.Alias == name);
            }
            catch (InvalidOperationException e)
            {
                throw new ArgumentException(string.Format("Web.Config setting '{0}' does not exist!", name), e);
            }
           

            return property.Value.ToString();
        }
    }
}