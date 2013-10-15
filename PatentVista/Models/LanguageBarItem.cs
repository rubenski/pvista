using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CookComputing.MetaWeblog;
using PatentVista.Business;
using Umbraco.Core;
using Umbraco.Core.Models;
using umbraco.MacroEngines.RazorDataTypeModels;

namespace PatentVista.Models
{
    public class LanguageBarItem
    {
        private readonly IPublishedContent _homepage;
        private readonly IPublishedContent _current;

        public IPublishedContent Homepage
        {
            get { return _homepage; }
        }

        public IPublishedContent Current
        {
            get { return _current; }
        }

        public LanguageBarItem(IPublishedContent homepage, IPublishedContent currentPage)
        {
            _homepage = homepage;
            _current = currentPage;
        }

        public UmbracoImage GetImage()
        {
            if (HomepageHelper.IsHomepageOf(_homepage, _current))
            {
                return UmbracoFieldHelper.GetImage(Homepage, "gekleurdeVlag");    
            }

            return UmbracoFieldHelper.GetImage(Homepage, "grijzeVlag"); ;
        }

        public int GetPosition()
        {
            return UmbracoFieldHelper.GetRequiredIntField(_homepage, "positieVlaggetje");
        }
    }
}