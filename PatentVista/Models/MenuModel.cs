using System.Collections.Generic;
using PatentVista.Business;
using Umbraco.Core.Models;


namespace PatentVista.Models
{
    public class MenuModel
    {
        public IList<IPublishedContent> Rubrieken { get; set; }
        public IPublishedContent ContactPage { get; set; }
        public IPublishedContent CostPage { get; set; }
    }
}