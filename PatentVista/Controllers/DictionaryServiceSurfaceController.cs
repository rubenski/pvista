using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PatentVista.Business;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace PatentVista.Controllers
{
    [PluginController("DSC")]
    public class DictionaryServiceSurfaceController : SurfaceController
    {
        //
        // GET: /DicionaryService/
      
        public ActionResult GetValueForKey()
        {
            string key = Request.Params.Get("key");
            string language = Request.Params.Get("lang");
            string translation = DictionaryHelper.ItemForKeyAndLanguage(key, language);

            return Json(new DictionaryCallResult() {Key = key, Value = translation}, JsonRequestBehavior.AllowGet);
        }

        public class DictionaryCallResult
        {
            public String Key { get; set; }
            public String Value { get; set; }
        }

    }
}
