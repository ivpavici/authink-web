[assembly: WebActivator.PreApplicationStartMethod(typeof(Authink.Web.App_Start.Combres), "PreStart")]
namespace Authink.Web.App_Start {
	using System.Web.Routing;
	using global::Combres;
	
    public static class Combres {
        public static void PreStart() {
            RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}