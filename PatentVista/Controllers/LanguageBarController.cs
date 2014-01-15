using System.Collections.Generic;
using System.Web.Mvc;
using PatentVista.Models;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace PatentVista.Controllers
{
    public class LanguageBarController : SurfaceController
    {
        public ActionResult Render()
        {
            var currentHomePage = CurrentPage.AncestorOrSelf(1);
            var languageBarModel = new LanguageBarModel();

            var test = CurrentPage;

            IEnumerable<IContent> rootContent = ApplicationContext.Services.ContentService.GetRootContent();    

            IEnumerable<IPublishedContent> homepages = currentHomePage.Siblings().Where(x => x.DocumentTypeAlias.Equals("Homepage"));

            foreach (var homepage in homepages)
            {
                languageBarModel.AddBarItem(new LanguageBarItem(homepage, CurrentPage));
            }

            return PartialView("LanguageBar", languageBarModel);
        }

    }
}
