using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PatentVista.Business;
using PatentVista.Models;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using umbraco.cms.businesslogic.web;

namespace PatentVista.Controllers
{
    public class ExtraCostController : SurfaceController
    {

        public ActionResult Index()
        {

            var currency = Request.Params.Get("currency");

            if (String.IsNullOrEmpty(currency))
            {
                currency = "EUR";
            }
                
            var kostenLandId = CurrentPage.GetPropertyValue("kostenland");
            var kostenLandItem = Umbraco.TypedContent(kostenLandId);

            var kostenland = new CountryCost(kostenLandItem, currency);

            return PartialView("ExtraCost", kostenland);
        }

    }
}
