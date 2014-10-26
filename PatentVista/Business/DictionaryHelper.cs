using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.XPath;
using Umbraco.Core;
using Umbraco.Core.Dictionary;
using Umbraco.Core.Models;
using Umbraco.Core.ObjectResolution;
using Umbraco.Web;
using Umbraco.Web.Dictionary;
using umbraco.cms.businesslogic;

namespace PatentVista.Business
{
    public class DictionaryHelper
    {
        public static String ItemForKeyAndLanguage(String key, String languageCode)
        {
            ILanguage language = ApplicationContext.Current.Services.LocalizationService.GetAllLanguages().First(x => x.IsoCode.Equals(languageCode));
            Dictionary.DictionaryItem dictionaryItem = new Dictionary.DictionaryItem(key);
            return dictionaryItem.Value(language.Id);
        }
    }
}