using System.Web.Http;
using Authink.Core.Model.Commands;
using Authink.Core.Model.Services;
using Authink.Web.Controllers.UserApi.Model;
using ent = Authink.Core.Domain.Entities;
using System.Web.Mvc;
using System.Net;
using Authink.Core.Model.Queries;

namespace Authink.Web.Controllers
{
    public class UserApiController : ApiController
    {
        public UserApiController
        (
            ILoginServices loginServices,
            IUserCommands  userCommands,
            IUserQueries   userQueries
        )
        {
            this.loginServices = loginServices;
            this.userCommands  = userCommands;
            this.userQueries   = userQueries;
        }

        private readonly ILoginServices loginServices;
        private readonly IUserCommands  userCommands;
        private readonly IUserQueries   userQueries;

        [System.Web.Http.HttpGet]
        public ent::User.ShortDetails TryGetSignedInUser()
        {
            return loginServices.GetSignedInUser();
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult Login(LoginModel model)
        {
            if(loginServices.Login(model.Username, model.Password))
            {
                loginServices.SignIn(model.Username, true);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult SignUp(SignUpModel model)
        {
            if(userQueries.GetSingle_whereEmail(model.Email) != null || userQueries.GetSingle_whereUsername(model.Username) != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
            }

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
        public string  Username  { get; set; }
        public string  Password  { get; set; }
    }

    public class SignUpModel
    {
        public string Firstname { get; set; }
        public string Lastname  { get; set; }
        public string Email     { get; set; }
        public string Username  { get; set; }
        public string Password  { get; set; }
    }
}
