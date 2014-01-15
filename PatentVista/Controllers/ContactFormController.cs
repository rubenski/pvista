using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using PatentVista.Business;
using PatentVista.Business.Mail;
using PatentVista.Models;
using Umbraco.Web.Mvc;
namespace PatentVista.Controllers
{
    public class ContactFormController : SurfaceController
    {

        [HttpPost]
        [ActionName("Submit")]
        public ActionResult SubmitIt(ContactFormModel model)
        {

            // Deze twee velden zijn verborgen in het formulier m.b.v. Javascript. Als ze ingevuld
            // zijn is het een bot die troep verstuurt. We doen dan dus niets.
            if (model.Name != null || model.Email != null)
            {
                return RedirectToCurrentUmbracoPage();
            } 

            Validate(model);

            if (ModelState.IsValid)
            {
                var message = new EmailMessage();
                message.AddToRecipient(Properties.Settings.Default.DefaultRecipientName, Properties.Settings.Default.DefaultRecipientAddress);
                message.From = new EmailAddress(model.RealName, model.RealEmail);
                message.Message = model.Message;
                message.Subject = model.Subject;
                MailHelper.SendMail(message);
                return new RedirectResult(string.Format("{0}?thanks=1", CurrentPage.Url));
            }

            return CurrentUmbracoPage();
            
        }

        private void Validate(ContactFormModel model)
        {
            // Validate Email
            bool emailError = false;
            if (model.RealEmail == null)
            {
                emailError = true;
            }
            else
            {
                Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match emailMatch = emailRegex.Match(model.RealEmail);
                if (!emailMatch.Success)
                {
                    emailError = true;
                }    
            }

            if (emailError)
            {
                string fouteEmail = Umbraco.GetDictionaryValue("Fout emailadres");
                ModelState.AddModelError("RealEmail", fouteEmail);    
            }
            
            // Validate Name
            bool nameError = false;
            if (model.RealName == null)
            {
                nameError = true;
            }
            else
            {
                Regex nameRegex = new Regex(@"^[A-Za-z0-9!)(/#.,';-?\ ]{2,35}$");
                Match nameMatch = nameRegex.Match(model.RealName);
                if (!nameMatch.Success)
                {
                    nameError = true;
                }    
            }

            if (nameError)
            {
                string fouteNaam = Umbraco.GetDictionaryValue("Foute naam");
                ModelState.AddModelError("RealName", fouteNaam);    
            }
            

            // Validate Subject
            bool subjectError = false;
            if (model.Subject == null)
            {
                subjectError = true;
            }
            else
            {
                Regex subjectRegex = new Regex(@"^[A-Za-z0-9!)(/#.,';-?\ ]{2,35}$");
                Match subjectMatch = subjectRegex.Match(model.Subject);
                if (!subjectMatch.Success)
                {
                    subjectError = true;
                }   
            }

            if (subjectError)
            {
                string foutOnderwerp = Umbraco.GetDictionaryValue("Fout onderwerp");
                ModelState.AddModelError("Subject", foutOnderwerp);
            }
            
            // Validate Message
            bool messageError = false;
            if (model.Message == null)
            {
                messageError = true;
            }
            else
            {
                Regex messageRegex = new Regex(@"[A-Za-z0-9 \\,><\.]{5,500}");
                Match messageMatch = messageRegex.Match(model.Message);
                if (!messageMatch.Success)
                {
                    messageError = true;
                }    
            }

            if (messageError)
            {
                string foutBericht = Umbraco.GetDictionaryValue("Fout bericht");
                ModelState.AddModelError("Message", foutBericht);
            }
        }
    }
}


