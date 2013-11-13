using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace PatentVista.Business
{
    public class CountryRate 
    {
        public string CurrencyCode;
        public String Rate;
    }

    public class CurrencyUpdater
    {
        public static void Update()
        {
            var rates = GetRates();

            var contentService = ApplicationContext.Current.Services.ContentService;

            IContent settings = PvNodes.GetSettings();

            foreach (var rate in rates)
            {
                if (rate.CurrencyCode.Equals("USD"))
                {
                    settings.SetValue("euroDollarRate", rate.Rate);
                }  
            }

            contentService.SaveAndPublish(settings);
        }

        private static IEnumerable<CountryRate> GetRates()
        {
            var doc = new XmlDocument();
            doc.Load(PvSettings.Get("currencyUrl"));

            XDocument xDocument = XDocument.Load(PvSettings.Get("currencyUrl"));
            var rates = new List<CountryRate>();

            foreach (var element in xDocument.Descendants())
            {
                string currencyCode = "";
                string rate = "";
                bool isCurrencyElement = false;


                foreach (var xAttribute in element.Attributes())
                {
                    if (xAttribute.Name.LocalName.Equals("currency"))
                    {
                        isCurrencyElement = true;
                        currencyCode = xAttribute.Value;
                    }

                    if (xAttribute.Name.LocalName.Equals("rate"))
                    {
                        rate = xAttribute.Value;
                    }
                }

                if (isCurrencyElement)
                {
                    rates.Add(new CountryRate
                    {
                        CurrencyCode = currencyCode,
                        Rate = rate
                    });
                }
            }
            return rates;
        }
    }
}