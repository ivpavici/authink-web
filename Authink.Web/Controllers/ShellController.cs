using Authink.Core.Fx;
using Authink.Core.Model.Services;
using System.Web.Mvc;

namespace Authink.Web.Controllers
{
    [Localization (DefaultLanguage = "en")]
    public class ShellController : Controller
    {
        public ShellController
        (
            ILoginServices loginServices
        )
        {
            this.loginServices = loginServices;
        }
        private readonly ILoginServices loginServices;

        public ActionResult Shell()
        {
            if(loginServices.GetSignedInUser() == null)
            {
                return RedirectToRoute("Login");
            }

            return View("Shell");
        }

        public ActionResult Login()
        {
            return View("Shell");
        }
    }
}
