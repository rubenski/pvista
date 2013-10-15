using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco;
using Umbraco.Core;
using Umbraco.Core.Models;
using umbraco.NodeFactory;

namespace PatentVista.Business
{
    public class PvNodes
    {
       
        private PvNodes() {}

        public static Node GetRoot()
        {
            return new Node(-1);
        }

        public static IContent GetSettings()
        {
            var contentService = ApplicationContext.Current.Services.ContentService;
            IContent settings = contentService.GetRootContent().First(x => x.ContentType.Alias.Equals("Settings"));
            return settings;
        }
    }
}