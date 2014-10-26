using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatentVista.Models
{
    public class Continent
    {
        private const int NumberOfColumns = 6;
        private const int MinimumLinksPerColumn = 3;

        private readonly IList<LinkModel> _countryLinks = new List<LinkModel>();
        public string Name { get; set; }
        public string Id { get; set; }

        public Continent(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public void AddCountryLink(LinkModel link)
        {
            _countryLinks.Add(link);
        }

        public IList<LinkModel> GetCountryLinks()
        {
            return _countryLinks;
        }
 
        public IList<IList<LinkModel>> GetTableRows()
        {
            var tooSmallLinksPerColumn = (int) Math.Floor((double)_countryLinks.Count()/NumberOfColumns);
            var linksPerColumn = tooSmallLinksPerColumn + 1;

            if (linksPerColumn < MinimumLinksPerColumn)
            {
                linksPerColumn = MinimumLinksPerColumn;
            }

            IList<LinkModel> sortedLinks = _countryLinks.OrderBy(x => x.LinkText).ToList();
            int NumberOfCountries = sortedLinks.Count - 1;
            
            IList<IList<LinkModel>> linkTable = new List<IList<LinkModel>>();

            for (int i = 0; i < linksPerColumn; i++)
            {
                IList<LinkModel> tableRow = new List<LinkModel>();

                for (int j = i; j <= NumberOfCountries; j += linksPerColumn)
                {

                    LinkModel countryLink = sortedLinks[j];
                    tableRow.Add(countryLink);
                }
                linkTable.Add(tableRow);
            }

            return linkTable;
        } 

    }
}