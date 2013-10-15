using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PatentVista.Business;

namespace PatentVista.Controllers
{
    public class CurrencyUpdaterController : Controller
    {
        //
        // GET: /CurrencyUpdater/

        public ActionResult Get()
        {

            var client = new WebClient();

            Stream responseStream = client.OpenRead(string.Format(PvSettings.Get("currencyUrl")));
            using (var sr = new StreamReader(responseStream))
            {
                string str = sr.ReadToEnd();
                str.ToString();


            }
            


            return null;
        }

    }
}
