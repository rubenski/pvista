using System;
using System.Collections.Generic;
using PatentVista.Business;
using PatentVista.Business.Exception;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using umbraco;

namespace PatentVista.Models
{
    public class CountryCost
    {
        private readonly IDictionary<int, int?> jaren = new Dictionary<int, int?>();
        private readonly string currencyCode;
        private readonly int? onlineIndienen;
        private readonly int? eersteIndiening;
        private readonly int? extraClaimkostenVanafClaim10;
        private readonly int? nieuwheidsonderzoekNationaalType;
        private readonly int? nieuwheidsonderzoekInternationaalType;
        private readonly int? examination;
        private readonly int? kostenVerlening;
        private readonly int? publicatiekostenVerlening;

        private readonly double localDollarRate;
        private readonly double goalDollarRate;

        public CountryCost(IPublishedContent costCountry, string goalCurrency)
        {

            if (costCountry == null)
            {
                throw new PatentVistaMissingValueException("Het kostenland is niet ingevuld! Waarschijnlijk ben je vergeten om dit land aan een kostenland te koppelen");
            }

            if (!costCountry.DocumentTypeAlias.Equals("Landkosten"))
            {
                throw new ArgumentException("CountryCost object must receive a Landkosten node instead of a {0} node", costCountry.DocumentTypeAlias);    
            }


            if (costCountry.GetProperty("valutacode").Value != null)
            {
                var umbHelper = new UmbracoHelper(UmbracoContext.Current);
                object valutaCode = costCountry.GetProperty("valutacode").Value;
                if (valutaCode == null || valutaCode == "")
                {
                    throw new PatentVistaMissingValueException(String.Format("De valutacode voor kostenland {0} is niet ingevuld", costCountry.Name));
                }
                IPublishedContent valutaItem = umbHelper.TypedContent(valutaCode.ToString());

                if (valutaItem == null)
                {
                    throw new PatentVistaMissingValueException(String.Format("Er hangt voor kostenland {0} misschien wat oude data in de Umbraco cache. Koppel {0} " +
                                                                             "(het kostenland, niet het land) opnieuw aan een valuta en save en publiceer {0} " +
                                                                             "  opnieuw", costCountry.Name));
                }

                currencyCode = valutaItem.Name;   
            }

            localDollarRate = CurrencyHelper.FindDollarRate(currencyCode);
            goalDollarRate = CurrencyHelper.FindDollarRate(goalCurrency);

            onlineIndienen = UmbracoFieldHelper.GetIntField(costCountry, "onlineIndienen");
            eersteIndiening = UmbracoFieldHelper.GetIntField(costCountry, "eersteIndiening");
            extraClaimkostenVanafClaim10 = UmbracoFieldHelper.GetIntField(costCountry, "extraClaimkostenVanafClaim10");
            nieuwheidsonderzoekNationaalType = UmbracoFieldHelper.GetIntField(costCountry, "nieuwheidsonderzoekNationaalType");
            nieuwheidsonderzoekInternationaalType = UmbracoFieldHelper.GetIntField(costCountry, "nieuwheidsonderzoekInternationaalType");
            examination = UmbracoFieldHelper.GetIntField(costCountry, "examination");
            kostenVerlening = UmbracoFieldHelper.GetIntField(costCountry, "kostenVerlening");
            publicatiekostenVerlening = UmbracoFieldHelper.GetIntField(costCountry, "publicatiekostenVerlening");


            for (int i = 1; i <= 20; i++)
            {
                jaren.Add(i, UmbracoFieldHelper.GetIntField(costCountry, String.Format("jaar{0}", i)));
            }

        }

        public int? GetYear(int year)
        {
            int? amount;
            jaren.TryGetValue(year, out amount);
            return ConvertToGoalRate(amount);
        }

        public IDictionary<int, int?> Jaren
        {
            get { return jaren; }
        }

        public string CurrencyCode
        {
            get { return currencyCode; }
        }

        public int? OnlineIndienen
        {
            get { return ConvertToGoalRate(onlineIndienen); }
        }

        public int? EersteIndiening
        {
            get { return ConvertToGoalRate(eersteIndiening); }
        }

        public int? ExtraClaimkostenVanafClaim10
        {
            get { return ConvertToGoalRate(extraClaimkostenVanafClaim10); }
        }

        public int? NieuwheidsonderzoekNationaalType
        {
            get { return ConvertToGoalRate(nieuwheidsonderzoekNationaalType); }
        }

        public int? NieuwheidsonderzoekInternationaalType
        {
            get { return ConvertToGoalRate(nieuwheidsonderzoekInternationaalType); }
        }

        public int? Examination
        {
            get { return ConvertToGoalRate(examination); }
        }

        public int? KostenVerlening
        {
            get { return ConvertToGoalRate(kostenVerlening); }
        }

        public int? PublicatiekostenVerlening
        {
            get { return ConvertToGoalRate(publicatiekostenVerlening); }
        }

        private int? ConvertToGoalRate(int? value)
        {
            if (value == null) return null;
            double? valueInDollars = value*(1/localDollarRate);
            return Convert.ToInt32(valueInDollars*goalDollarRate);
        }

        public TotalAndAverage GetTotalAndAverage()
        {
            var totalAndAverage = new TotalAndAverage();
            foreach (var jaar in jaren)
            {
                totalAndAverage.Total += GetYear(jaar.Key).GetValueOrDefault();    
            }

            totalAndAverage.Average = totalAndAverage.Total/jaren.Count;

            return totalAndAverage;
        }

        public class TotalAndAverage
        {
            public int Total { get; set; }
            public int Average { get; set; }
        }
    }
} 