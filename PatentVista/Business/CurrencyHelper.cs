using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace PatentVista.Business
{
    public class CurrencyHelper
    {
       
        public static double FindDollarRate(String currencyCode)
        {
            IContent valutaMap = PvNodes.GetRootContentNodeByName("Valuta");
            var dollarRateItem = valutaMap.Children().First(x => x.Name.Equals(currencyCode));
            string dollarValueString = dollarRateItem.GetValue("waarde").ToString();
            string replacedDollarValueString = dollarValueString.Replace(",", ".");
            double dollarRate = Convert.ToDouble(replacedDollarValueString, System.Globalization.CultureInfo.InvariantCulture);
            return dollarRate;
        }
    }
}