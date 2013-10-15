using System;
using PatentVista.Business.Exception;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace PatentVista.Business
{
    public class UmbracoFieldHelper
    {
        public static UmbracoImage GetImage(IPublishedContent page, string field)
        {
            IPublishedContentProperty property = GetProperty(page, field);
            string value = property.Value.ToString();
            int id = Convert.ToInt32(value);
            IMedia mediaObject = ApplicationContext.Current.Services.MediaService.GetById(id);

            var image = new UmbracoImage
            {
                Url = mediaObject.Properties["umbracoFile"].Value.ToString(),
                Width = mediaObject.Properties["umbracoWidth"].Value.ToString(),
                Height = mediaObject.Properties["umbracoHeight"].Value.ToString(),
                Extension = mediaObject.Properties["umbracoExtension"].Value.ToString(),
                AltText = mediaObject.Properties["altText"].Value.ToString()
            };

            return image;
        }

        public static IPublishedContent GetLinkedPage(IPublishedContent page, string field)
        {
            IPublishedContentProperty property = GetProperty(page, field);

            if (property.ToString().Equals(""))
            {
                throw new MissingValueException(String.Format("No {0} found on item titled '{1}'.", field, page.Name));
            }

            int linkedPageId = Convert.ToInt32(property.Value);

            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var linkedPage = umbracoHelper.TypedContent(linkedPageId);

            return linkedPage;
        }

        public static int GetRequiredIntField(IPublishedContent page, string field)
        {
            IPublishedContentProperty intProperty = GetProperty(page, field);

            if (intProperty.Value.ToString().Equals(""))
            {
                throw new MissingValueException(String.Format("No value for {0} found on item titled '{1}'.", field, page.Name));
            }

            return Convert.ToInt32(intProperty.Value);
        }


        private static IPublishedContentProperty GetProperty(IPublishedContent page, string field)
        {
            if (field == null) throw new ArgumentNullException("field");
            if (page == null) throw new ArgumentNullException("page");

            var property = page.GetProperty(field);
            if (property == null)
            {
                throw new MissingPropertyException(String.Format("Missing property {0} on item titled {1}", field, page.Name));
            }

            return property;
        }
    }
}