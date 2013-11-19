using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Authink.Core.Model.Commands;
using Authink.Data.Api.Service;

using apiEnt = Authink.Data.Api.App.Entities;
using buru   = Authink.Core.Domain.Rules;

namespace Authink.Web.Controllers
{
    public class ConsumerApiController : ApiController
    {
        public ConsumerApiController()
        {
            apiAdapter = new AuthinkApiAdapter();

        }

        private readonly AuthinkApiAdapter apiAdapter;

        [HttpPost] public HttpResponseMessage Login(apiEnt::User.Details userData)
        {
            var authencationToken = Request.Headers.GetValues("AuthenticationToken").Single();
            if (authencationToken != buru::Api.LoginToken)
            {
                throw new Exception();
            }
            return new HttpResponseMessage
            (
                statusCode: apiAdapter.Login_User(
                                                    username: userData.Username,
                                                    password: userData.Password
                                                   ) ? HttpStatusCode.OK 
                                                     : HttpStatusCode.NotFound
            );
        }

        [HttpGet] public IEnumerable<apiEnt::Child.Details> GetChildren_forUser(string user_userName)
        {
            var authencationToken = Request.Headers.GetValues("AuthenticationToken").Single();
            if (authencationToken != buru::Api.ChildrenDataToken)
            {
                throw new Exception();
            }
            return
                apiAdapter.GetChildren
                (
                    user_userName: user_userName
                );
        }
    }
}
