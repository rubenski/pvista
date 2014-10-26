using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Helpers;
using Newtonsoft.Json;
using Umbraco.Core;
using Umbraco.Core.Models;
using umbraco.NodeFactory;

namespace PatentVista.Business
{
    public class OpenExchangeRatesUpdater
    {
        public static void Update()
        {

            string url = PvSettings.Get("currencyUrl");

            var request = WebRequest.Create(url);

            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
                var currencyData = JsonConvert.DeserializeObject<CurrencyData>(text);
                UpdateUmbracoCurrencyFields(currencyData);
            }
        }

        private static void UpdateUmbracoCurrencyFields(CurrencyData data)
        {

            var contentService = ApplicationContext.Current.Services.ContentService;
            var valutamap = contentService.GetRootContent().First(x => x.ContentType.Name.Equals("Valutamap"));

            foreach (var rate in data.Rates)
            {
                var existingRates = valutamap.Children();
                IContent rateItem = existingRates.FirstOrDefault(x => x.Name.Equals(rate.Key));

                if (rateItem == null)
                {
                    rateItem = contentService.CreateContent(
                        rate.Key,
                        valutamap.Id,
                        "DollarRate");
                }

                rateItem.SetValue("waarde", rate.Value.Replace(".", ","));
                contentService.SaveAndPublish(rateItem);
            }

            contentService.PublishWithChildren(valutamap);

        }
    }

    public class CurrencyData
    {
        [DataMember(Name = "rates")]
        public IDictionary<string, string> Rates = new Dictionary<string, string>();
    }
}