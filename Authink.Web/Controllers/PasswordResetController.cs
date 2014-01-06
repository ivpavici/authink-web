using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Authink.Core.Fx;
using Authink.Core.Model.Queries;
using Authink.Web.Controllers.PasswordResetToken.Models;

namespace Authink.Web.Controllers
{
    public class PasswordResetController : Controller
    {
        public PasswordResetController
        (
            IUserQueries                userQueries,
            IPasswordResetTokenCommands passwordResetTokenCommands,
            MailService                 mailService
        )
        {
            this.userQueries                = userQueries;
            this.passwordResetTokenCommands = passwordResetTokenCommands;
            this.mailService                = mailService;
        }

        private readonly IUserQueries                userQueries;
        private readonly IPasswordResetTokenCommands passwordResetTokenCommands;
        private readonly MailService                 mailService;
        [HttpGet]
        public ActionResult Create()
        { 
            return View();
        }

        [HttpPost]
        public ActionResult Create(PasswordResetTokenCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = userQueries.GetSingle_whereEmail(model.Email);
            if (currentUser == null)
            {
                ModelState.AddModelError("Email not found", "Email not found");
                return View(model);
            }

            var tokenValue = passwordResetTokenCommands.Create(currentUser.Id);

            mailService.Send(currentUser.Email, "password@authink.eu", tokenValue, "Password reset");

            return View(model);
        }
    }
}
namespace Authink.Web.Controllers.PasswordResetToken.Models
{
    public class PasswordResetTokenCreateModel
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}