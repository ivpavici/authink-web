using System.Web.Http;
using System.Web.Mvc;
using System.Net;
using Authink.Core.Model.Queries;
using Authink.Core.Model.Services;
using Authink.Core.Model.Commands;
using Authink.Web.Controllers.UserApi.Model;
using ent = Authink.Core.Domain.Entities;
using NLog;
using System.ComponentModel.DataAnnotations;

namespace Authink.Web.Controllers
{
    public class UserApiController : ApiController
    {
        public UserApiController
        (
            ILoginServices loginServices,
            IUserCommands  userCommands,
            IUserQueries   userQueries,
            Logger         logger
        )
        {
            this.loginServices = loginServices;
            this.userCommands  = userCommands;
            this.userQueries   = userQueries;
            this.logger        = logger;
        }

        private readonly ILoginServices loginServices;
        private readonly IUserCommands  userCommands;
        private readonly IUserQueries   userQueries;
        private Logger                  logger;

        [System.Web.Http.HttpGet]
        public ent::User.ShortDetails TryGetSignedInUser()
        {
            return loginServices.GetSignedInUser();
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult Login(LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
            }

            if(loginServices.Login(model.Username, model.Password))
            {
                loginServices.SignIn(model.Username, true);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            logger.Error("Login failed for user with username = {0} ", model.Username);
            return new HttpStatusCodeResult(HttpStatusCode.Conflict);
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult SignUp(SignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
            }

            if(userQueries.GetSingle_whereEmail(model.Email) != null || userQueries.GetSingle_whereUsername(model.Username) != null)
            {
                logger.Error("Sign up failed for user with username = {0} and email = {1} ", model.Username, model.Email);
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
            }

            try
            {
                userCommands.Create
                (
                    firstname: model.Firstname,
                    lastname:  model.Lastname,
                    email:     model.Email,
                    username:  model.Username,
                    password:  model.Password
                );

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (System.Exception ex)
            {
                logger.Error("Sign up failed", ex);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [System.Web.Http.HttpGet]
        public void SignOut()
        {
            loginServices.SignOut();
        }
    }
}

namespace Authink.Web.Controllers.UserApi.Model
{
    public class LoginModel
    {
        [Required]
        public string  Username  { get; set; }

        [Required]
        public string  Password  { get; set; }
    }

    public class SignUpModel
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname  { get; set; }

        [Required]
        public string Email     { get; set; }

        [Required]
        public string Username  { get; set; }

        [Required]
        public string Password  { get; set; }
    }
}
