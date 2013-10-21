using System.Web.Http;
using Authink.Core.Model.Commands;
using Authink.Core.Model.Services;
using Authink.Web.Controllers.UserApi.Model;
using ent = Authink.Core.Domain.Entities;

namespace Authink.Web.Controllers
{
    public class UserApiController : ApiController
    {
        public UserApiController
        (
            ILoginServices loginServices,
            IUserCommands userCommands
        )
        {
            this.loginServices = loginServices;
            this.userCommands  = userCommands;
        }

        private readonly ILoginServices loginServices;
        private readonly IUserCommands  userCommands;

        [HttpGet]
        public ent::User.ShortDetails TryGetSignedInUser()
        {
            return loginServices.GetSignedInUser();
        }

        [HttpPost]
        public object Login(LoginModel model)
        {
            if(loginServices.Login(model.Username, model.Password))
            {
                loginServices.SignIn(model.Username, true);
                return new { isSuccessful= true};
            }

            return new { isSuccessful = false };
        }

        [HttpPost]
        public object SignUp(SignUpModel model)
        {
            //dodat provjere

            userCommands.Create
            (
                firstname: model.Firstname,
                lastname:  model.Lastname,
                email:     model.Email,
                username:  model.Username,
                password:  model.Password
            );

            return new { isSuccessful = true };
        }

        [HttpGet]
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
