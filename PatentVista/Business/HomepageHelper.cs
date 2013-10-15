using Umbraco.Core.Models;
using Umbraco.Web;

namespace PatentVista.Business
{
    public class HomepageHelper
    {
        public static bool IsHomepageOf(IPublishedContent homepage, IPublishedContent someNode)
        {
            var someNodesHomepage = someNode.AncestorOrSelf("Homepage");
            return someNodesHomepage.Id.Equals(homepage.Id);
        }
    }
}