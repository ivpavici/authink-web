using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Authink.Data.Factory;
using Authink.Web.App_Start;
using System.Threading;
using System.Globalization;

namespace Authink.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            SetTestData();
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            ModelBinders.Binders.DefaultBinder = new StructureMapModelBinder();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true;
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            var language = Request.Cookies["authink-language"] != null
                            ? Request.Cookies["authink-language"].Value
                            : "en";

            var culture  = new CultureInfo(language);

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture   = CultureInfo.CreateSpecificCulture(culture.Name);
        }
        private void SetTestData()
        {
#if DEBUG
            TestDataFactory.Fill();
#endif
        }
    }
}