using System.Collections.Generic;
using System.Linq;

namespace PatentVista.Models
{
    public class LanguageBarModel
    {

        private IList<LanguageBarItem> _barItems = new List<LanguageBarItem>();

        public IEnumerable<LanguageBarItem> BarItems
        {
            get
            {
                return from i in _barItems orderby i.GetPosition() select i;
            }
        }

        public void AddBarItem(LanguageBarItem item)
        {
            _barItems.Add(item);    
        }
    }
}