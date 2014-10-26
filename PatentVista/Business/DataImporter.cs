using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MySql.Data.MySqlClient;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace PatentVista.Business
{
    public class DataImporter
    {
        public static void Import()
        {

            var contentService = ApplicationContext.Current.Services.ContentService;

            const string conString = "server=127.0.0.1;uid=root; pwd=smeerkaas;database=patentvista_db;";

            MySqlConnection con = new MySqlConnection(conString);
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM landen2");

            cmd.Connection = con;

            con.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            var landenPaginaDutch = GetRootItemDescendant("Homepage NL", "Landen");
            var landenPaginaEnglish = GetRootItemDescendant("Homepage EN", "Countries");
            var landgegevens = GetRootItemByName("Landgegevens");
            
            while (reader.Read())
            {
                var land_nr = reader.GetInt32("land_nr");

                // Taalspecificiek
                var opmerkingen = reader.GetString("opmerkingen");
                var remarks = reader.GetString("remarks");
                var landnaam_nl = reader.GetString("landnaam_nl");
                var landnaam_en = reader.GetString("landnaam_en");

                // Onspecifiek
                var eerste_indiening = reader.GetInt32("premier_depot");
                var online_indienen = reader.GetInt32("online_filing");
                var extra_claimkosten_vanaf_claim_10 = reader.GetInt32("claimkosten");
                var nieuwheidsonderzoek_nationaal = reader.GetInt32("searchfee");
                var nieuwheidsonderzoek_internationaal = reader.GetInt32("searchfee_INT");
                var examination = reader.GetInt32("examination");
                var kosten_verlening = reader.GetInt32("_grant");
                var publicatiekosten_verlening = reader.GetInt32("publ_grant");

                //var claimnummer = reader.GetString("claimnummer");
                //var herstel = reader.GetInt32("herstel");

                // GetOrCreate land Nederlands
                var landNL = GetRootItemDescendant("Homepage NL", HttpUtility.HtmlDecode(landnaam_nl));
                if (landNL == null)
                {
                    landNL = contentService.CreateContent(HttpUtility.HtmlDecode(landnaam_nl), landenPaginaDutch, "Land");
                }
                landNL.SetValue("opmerkingen", HttpUtility.HtmlDecode(opmerkingen));
                contentService.SaveAndPublish(landNL);

                // GetOrCreate land Engels
                var landEN = GetRootItemDescendant("Homepage EN", HttpUtility.HtmlDecode(landnaam_en));
                if (landEN == null)
                {
                    landEN = contentService.CreateContent(HttpUtility.HtmlDecode(landnaam_en), landenPaginaEnglish, "Land");
                }
                landEN.SetValue("opmerkingen", HttpUtility.HtmlDecode(remarks));
                contentService.SaveAndPublish(landEN);

                // Fetch landendata from old db
                var con2 = new MySqlConnection(conString);
                MySqlCommand taksenCommand = new MySqlCommand("SELECT * FROM taksen2 WHERE land_nr = @LandNr", con2);
                con2.Open();
                
                taksenCommand.Parameters.AddWithValue("@LandNr", land_nr);


                // GetOrCreate kostenland and set data from old db
                var kostenland = GetRootItemDescendant("Landgegevens", landnaam_nl);
                if (kostenland == null)
                {
                    kostenland = contentService.CreateContent(landnaam_nl, landgegevens, "Landkosten");
                }

                using (MySqlDataReader mySqlDataReader = taksenCommand.ExecuteReader())
                {
                    // Set taksen values in original currency
                    while (mySqlDataReader.Read())
                    {
                        var taksejaar = mySqlDataReader.GetInt32("taksejaar");
                        var bedrag = mySqlDataReader.GetString("bedrag");
                        kostenland.SetValue(String.Format("jaar{0}", taksejaar), bedrag);
                        
                    }
                }

                // Set additional costs
                kostenland.SetValue("onlineIndienen", online_indienen);
                kostenland.SetValue("eersteIndiening", eerste_indiening);
                kostenland.SetValue("extraClaimkostenVanafClaim10", extra_claimkosten_vanaf_claim_10);
                kostenland.SetValue("nieuwheidsonderzoekNationaalType", nieuwheidsonderzoek_nationaal);
                kostenland.SetValue("nieuwheidsonderzoekInternationaalType", nieuwheidsonderzoek_internationaal);
                kostenland.SetValue("examination", examination);
                kostenland.SetValue("kostenVerlening", kosten_verlening);
                kostenland.SetValue("publicatiekostenVerlening", publicatiekosten_verlening);

                contentService.SaveAndPublish(kostenland);
            }

            contentService.PublishWithChildren(landgegevens);
        }


        public static IContent GetRootItemDescendant(String rootItemName, String descendantName)
        {
            var contentService = ApplicationContext.Current.Services.ContentService;
            var parent = contentService.GetRootContent().FirstOrDefault(x => x.Name.Equals(rootItemName));
            if (parent == null)
            {
                throw new System.Exception(String.Format("Root item '{0}' not found", rootItemName));
            }
            var firstOrDefault = parent.Descendants().FirstOrDefault(x => x.Name.Equals(descendantName));
            return firstOrDefault;
        }

        public static IContent GetRootItemByName(String itemName)
        {
            var contentService = ApplicationContext.Current.Services.ContentService;
            var firstOrDefault = contentService.GetRootContent().FirstOrDefault(x => x.Name.Equals(itemName));
            return firstOrDefault;
        }
    }
}