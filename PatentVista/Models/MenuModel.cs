using System.Collections.Generic;
using PatentVista.Business;
using Umbraco.Core.Models;


namespace PatentVista.Models
{
    public class MenuModel
    {
        private Dictionary<int, ArticleCategory> _categoryMap = new Dictionary<int, ArticleCategory>();
        public IPublishedContent ContactPage { get; set; }
        public IPublishedContent CostPage { get; set; }

        public List<ArticleCategory> GetCategories()
        {
            return new List<ArticleCategory>(_categoryMap.Values);
        }

        public void AddArticle(IPublishedContent article)
        {
            IPublishedContent rubriek = UmbracoFieldHelper.GetLinkedPage(article, "rubriek");

            ArticleCategory cat;
            _categoryMap.TryGetValue(rubriek.Id, out cat);
            if (cat == null)
            {
                cat = new ArticleCategory {Name = rubriek.GetProperty("naam").Value.ToString()};
                _categoryMap.Add(rubriek.Id, cat);
            }

            cat.AddItem(article);

        }
    }


}