using System.Linq;
using System.Web.Mvc;
using PatentVista.Models;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace PatentVista.Controllers
{
    public class NavMenuController : SurfaceController
    {
        public ActionResult Render(RenderModel some)
        {
            var currentHomePage = CurrentPage.AncestorOrSelf(1);
            var menuModel = new MenuModel();

            foreach(var artikel in currentHomePage.Descendants("Artikel").Where(x => x.IsVisible()))
            {
                menuModel.AddArticle(artikel);    
            }

            menuModel.ContactPage = currentHomePage.DescendantsOrSelf("Contactpagina").First();

            menuModel.CostPage = currentHomePage.Descendants("Landenpagina").First();

            return PartialView("NavMenu", menuModel);
        }
    }
}
