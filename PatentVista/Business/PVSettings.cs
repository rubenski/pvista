using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco;
using umbraco.NodeFactory;

namespace PatentVista.Business
{
    public class PvSettings
    {
        public static string Get(string name)
        {
            var settings = PvNodes.GetSettings();

            var property = settings.Properties.First(x => x.Alias == name);

            if (property == null)
            {
                throw new ArgumentException(String.Format("Setting with name '{0}' does not exist", name));
            }

            return property.Value.ToString();
        }
    }
}