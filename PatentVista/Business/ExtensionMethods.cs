using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Umbraco.Core.Models;

namespace PatentVista.Business
{
    public static class ExtensionMethods
    {
        private static string PLACEHOLDER_ONE_NAME = "{adsense}";

        public static HtmlString Adsensed(this string text)
        {
            int adsensePos = text.IndexOf(PLACEHOLDER_ONE_NAME);
            if (adsensePos >= 0)
            {
                string adsenseCode = PvNodes.GetSettings().GetValue("inlineAdsense").ToString();
                string textBeforeAdd = text.Substring(0, adsensePos);
                string textAfterAdd = text.Substring(adsensePos + PLACEHOLDER_ONE_NAME.Length);
                return new HtmlString(textBeforeAdd + "<br/>" + adsenseCode + "<br/>" + textAfterAdd);
            }

            return new HtmlString(text);
            
        }
    }
}
