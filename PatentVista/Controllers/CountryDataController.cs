﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using PatentVista.Business;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;


namespace PatentVista.Controllers
{
    public class CountryDataController : SurfaceController
    {


        //
        // GET: /CountryData/

        public ActionResult Get() 
        {
            var landIdString = Request.Params.Get("landId");
            var currency = Request.Params.Get("currency");

            if (landIdString == null)
            {
                return HttpNotFound();
            }

            int landId = Convert.ToInt32(landIdString);

            var land = Umbraco.TypedContent(landId);

            string vulkleur = PvSettings.Get("grafiekenVulkleur");
            string lijnkleur = PvSettings.Get("grafiekenLijnkleur");

            var resultData = new ResultData(vulkleur, lijnkleur);

            AddCountryToDataSet(resultData, land);

            return Json(resultData, JsonRequestBehavior.AllowGet);
        }

        private void AddCountryToDataSet(ResultData resultData, IPublishedContent land)
        {

            for (int i = 1; i <= 20; i++)
            {
                var value = land.GetProperty(string.Format("jaar{0}", i)).Value;
                int takse = Convert.ToInt32(value);

                if (takse > resultData.MaxTakse)
                {
                    resultData.MaxTakse = takse;
                }
                resultData.AddDataPoint(i, takse);
            }
        }

        public class ResultData
        {
            public int MaxTakse { get; set; }
            private readonly DataSet _dataSet;
            private readonly Dictionary<int, string> _labels = new Dictionary<int, string>();

            public ResultData(string fillColor, string lineColor)
            {
                _dataSet = new DataSet(fillColor, lineColor);
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


            public void AddDataPoint(int jaarNummer, int takse)
            {
                _labels.Add(jaarNummer, string.Format("Jaar {0}", jaarNummer));
                _dataSet.AddDataPoint(jaarNummer, takse);
            }


        }

        public class DataSet
        {

            private readonly Dictionary<int, int> _data = new Dictionary<int, int>();

 
            public string fillColor;
            public string strokeColor;

            public DataSet(string fillColor, string strokeColor)
            {
                this.fillColor = fillColor;
                this.strokeColor = strokeColor;
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
                int jaar;
                bool b = _data.TryGetValue(jaarNummer, out jaar);
                if (b)
                {
                    jaar += takse;
                }
                else
                {
                    _data.Add(jaarNummer, takse);    
                }
            }
        }
    }
}