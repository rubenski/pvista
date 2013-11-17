using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PatentVista.Business;
using PatentVista.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using umbraco.cms.businesslogic.web;
using Language = umbraco.cms.businesslogic.language.Language;


namespace PatentVista.Controllers
{
    public class CountryDataController : SurfaceController
    {
        //
        // GET: /CountryData/

        public ActionResult Get()
        {
            
            var kostenLandIdString = Request.Params.Get("landId");
            var currency = Request.Params.Get("currency");
            var langCode = Request.Params.Get("lang");

            int landId = Convert.ToInt32(kostenLandIdString);

            if (kostenLandIdString == null || currency == null)
            {
                return HttpNotFound();
            }

            string vulkleur = PvSettings.Get("grafiekenVulkleur");
            string lijnkleur = PvSettings.Get("grafiekenLijnkleur");

            var resultData = new ResultData(vulkleur, lijnkleur, 1.0);

            var landkosten = Umbraco.TypedContent(landId);

            AddCountryToDataSet(resultData, landkosten, currency, DictionaryHelper.ItemForKeyAndLanguage("Jaar", langCode));

            return Json(resultData, JsonRequestBehavior.AllowGet);
            
           
        }

        private void AddCountryToDataSet(ResultData resultData, IPublishedContent landkosten, string goalCurrency, String localizedYear)
        {

            var kostenland = new CountryCost(landkosten, goalCurrency);

            for (int i = 1; i <= 20; i++)
            {

                int jaarTakseInGoalCurrency = kostenland.GetYear(i) ?? default(int);

                if (jaarTakseInGoalCurrency > resultData.MaxTakse)
                {
                    resultData.MaxTakse = jaarTakseInGoalCurrency;
                }

                resultData.AddDataPoint(i, jaarTakseInGoalCurrency, localizedYear);
            }

            string stepWidthString = (resultData.MaxTakse / 10).ToString();
            int biggestInt = Convert.ToInt32(stepWidthString[0].ToString()) + 1;
            string nextBigNumberforStep = biggestInt.ToString();

            for (int i = 0; i < stepWidthString.Length - 1; i++)
            {
                nextBigNumberforStep += "0";
            }

            int nextBigNumberForStepInt = Convert.ToInt32(nextBigNumberforStep);
            resultData.StepWidth = nextBigNumberForStepInt;


        }

        public class ResultData
        {
            private readonly double _multiplier;

            public int MaxTakse     
            {
                get { return _maxTakse; }
                set
                {
                    _maxTakse = (int) Math.Round(value * _multiplier);
                }
            }

            public int StepWidth { get; set; }

            private readonly DataSet _dataSet;
            private readonly Dictionary<int, string> _labels = new Dictionary<int, string>();
            private int _maxTakse;

            public ResultData(string fillColor, string lineColor, double multiplier)
            {
                _multiplier = multiplier;
                _dataSet = new DataSet(fillColor, lineColor, multiplier);
            }


            public IList<string> labels
            {
                get
                {
                    IOrderedEnumerable<KeyValuePair<int, string>> keyValuePairs = _labels.OrderBy(x => x.Key);
                    Dictionary<int, string> dictionary = keyValuePairs.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
                    var list = new List<string>(dictionary.Values);
                    return list;
                }
            }
            

            public IList<DataSet> datasets
            {
                // Only one dataset is ever used, but still returning a list to get proper JSON output
                get { return new List<DataSet>{_dataSet}; }
            }


            public void AddDataPoint(int jaarNummer, int takse, string localizedYear)
            {
                _labels.Add(jaarNummer, string.Format("{0} {1}", localizedYear, jaarNummer));
                _dataSet.AddDataPoint(jaarNummer, takse);
            }


        }

        public class DataSet
        {

            private readonly Dictionary<int, int> _data = new Dictionary<int, int>();

 
            public string fillColor;
            public string strokeColor;
            private double _multiplier;

            public DataSet(string fillColor, string strokeColor, double multiplier)
            {
                this.fillColor = fillColor;
                this.strokeColor = strokeColor;
                _multiplier = multiplier;
            }

            public List<int> data
            {
                get
                {
                    IOrderedEnumerable<KeyValuePair<int, int>> keyValuePairs = _data.OrderBy(x => x.Key);
                    Dictionary<int, int> dictionary = keyValuePairs.ToDictionary(keyItem => keyItem.Key, valueItem => valueItem.Value);
                    var list = new List<int>(dictionary.Values);
                    return list;
                }
            } 

            public void AddDataPoint(int jaarNummer, int takse)
            {
                int takseValue = (int) Math.Round(takse*_multiplier);
                int jaar;
                bool b = _data.TryGetValue(jaarNummer, out jaar);
                if (b)
                {
                    jaar += takseValue;
                }
                else
                {
                    _data.Add(jaarNummer, takseValue);    
                }
            }
        }
    }
}
