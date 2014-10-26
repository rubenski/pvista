using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using umbraco.cms.businesslogic.web;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace PatentVista.Controllers
{
    public class LanguageAttribController : SurfaceController
    {
        //
        // GET: /LanguageAttrib/

        public String GetCurrentLanguage()
        {
            var currentHomePage = CurrentPage.AncestorOrSelf("Homepage");
            return Domain.GetDomainsById(currentHomePage.Id)[0].Language.CultureAlias;
        }

    }
}
