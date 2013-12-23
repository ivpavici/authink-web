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
using NLog;

namespace Authink.Web.Controllers
{
    public class ConsumerApiController : ApiController
    {
        public ConsumerApiController
        (
            AuthinkApiAdapter apiAdapter,
            Logger            logger
        )
        {
            this.apiAdapter = apiAdapter;
            this.logger     = logger;
        }

        private readonly AuthinkApiAdapter apiAdapter;
        private Logger                     logger;

        [HttpPost] public HttpResponseMessage Login(apiEnt::User.Details userData)
        {
            var authencationToken = Request.Headers.GetValues("AuthenticationToken").Single();
            if (authencationToken != buru::Api.LoginToken)
            {
                logger.Error("ConsumerApi: Wrong login authentication token: {0}", authencationToken);
                throw new Exception();
            }

            try 
	        {	        
	        	var statusCode = apiAdapter.Login_User(
                                                        username: userData.Username,
                                                        password: userData.Password
                                                      ) ? HttpStatusCode.OK 
                                                        : HttpStatusCode.NotFound;

                logger.Error("ConsumerApi: User loged in app: username: {0}", userData.Username);
                return new HttpResponseMessage(statusCode);
	        }
	        catch (Exception ex)
	        {
	        	logger.Error("ConsumerApi: login failed", ex);
	        	return new HttpResponseMessage(HttpStatusCode.NotFound);
	        }
        }

        [HttpGet] public IEnumerable<apiEnt::Child.Details> GetChildren_forUser(string user_userName)
        {
            var authencationToken = Request.Headers.GetValues("AuthenticationToken").Single();
            if (authencationToken != buru::Api.ChildrenDataToken)
            {
                logger.Error("ConsumerApi: Wrong login authentication token:{0}", authencationToken);
                throw new Exception();
            }

            try
            {
                var children = apiAdapter.GetChildren(user_userName: user_userName);
                return children;
            }
            catch (Exception ex)
            {
                logger.Error("ConsumerApi: failed to deliver children for user with username = {0} ", user_userName, ex);

                return new List<apiEnt::Child.Details>();
            }
        }
    }
}
