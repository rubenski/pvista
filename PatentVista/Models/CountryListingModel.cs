using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PatentVista.Business;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using umbraco;

namespace PatentVista.Models
{
    public class CountryListingModel
    {
        private readonly IList<Continent> _continents = new List<Continent>();
        // private readonly IContentService _contentService = ApplicationContext.Current.Services.ContentService;
        
        
        public void AddCountry(IPublishedContent country)
        {
            var continentProperty = country.GetProperty("continent");

            if (continentProperty != null)
            {
                var continentId = continentProperty.Value.ToString();

                Continent continent = null;
                foreach (var existingContinents in _continents)
                {
                    if (existingContinents.Id.Equals(continentId))
                    {
                        continent = existingContinents;
                    }  
                }
                if (continent == null)
                {
                    string continentName = library.GetPreValueAsString((Convert.ToInt32(continentId)));
                    continent = new Continent(continentName, continentId);
                    _continents.Add(continent);
                }

                continent.AddCountryLink(new LinkModel(country.Name, country.Url));

            }
        }

        public IList<Continent> GetContinents()
        {
            return _continents;
        } 
    }
}