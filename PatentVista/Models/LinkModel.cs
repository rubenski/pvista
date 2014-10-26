using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatentVista.Models
{
    public class LinkModel
    {
        private readonly string _linkText;
        private readonly string _href;

        public LinkModel(string linkText, string href)
        {
            _linkText = linkText;
            _href = href;
        }

        public string LinkText
        {
            get { return _linkText; }
        }

        public string Href
        {
            get { return _href; }
        }
    }
}