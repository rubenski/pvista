using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace PatentVista.Models
{
    public class ArticleCategory
    {
        private readonly IList<IPublishedContent> _items = new List<IPublishedContent>();
        public String Name { get; set; }

        public IList<IPublishedContent> Items
        {
            get { return _items; }
        }

        public void AddItem(IPublishedContent item)
        {
            _items.Add(item);    
        }
    }
}