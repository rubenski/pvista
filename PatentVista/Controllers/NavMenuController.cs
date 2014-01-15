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
            menuModel.Rubrieken = currentHomePage.Descendants("Rubriek").Where(x => x.IsVisible()).ToList();  
            menuModel.ContactPage = currentHomePage.DescendantsOrSelf("Contactpagina").First();
            menuModel.CostPage = currentHomePage.Descendants("Landenpagina").First();
            menuModel.OtherPages = currentHomePage.Children().Where(x => x.DocumentTypeAlias.Equals("Tweekolomspagina")).ToList();
            return PartialView("NavMenu", menuModel);
        }
    }
}
