using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Authink.Core.Fx
{
    public class Localization : ActionFilterAttribute
    {
        public string DefaultLanguage { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var language = (string)filterContext.RouteData.Values["lang"] ?? this.DefaultLanguage;
            var culture  = new CultureInfo(language);

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture  = culture;
        }
    }
}
