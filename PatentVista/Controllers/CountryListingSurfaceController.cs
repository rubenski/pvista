using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PatentVista.Models;
using Umbraco.Web.Mvc;

namespace PatentVista.Controllers
{
    [PluginController("CLC")]
    public class CountryListingSurfaceController : SurfaceController
    {
  
        public ActionResult Index()
        {
            var listing = new CountryListingModel();

            var countries = CurrentPage.Children;

            foreach (var country in countries)
            {
                listing.AddCountry(country);
            }

            return PartialView("CountryListing", listing);
        }

    }
}
