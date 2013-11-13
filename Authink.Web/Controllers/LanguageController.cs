using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authink.Web.Controllers
{
    public class LanguageController : Controller
    {
        public ActionResult ChangeCulture(string lang)
        {
            if(lang != "en" && lang != "hr")
            {
                return new HttpNotFoundResult();
            }

            var langCookie     = new HttpCookie("authink-language", lang);
            langCookie.Expires = DateTime.Now.AddMonths(12);  
            
            Response.AppendCookie(langCookie);

            return RedirectToRoute("Shell");
        }
    }
}
