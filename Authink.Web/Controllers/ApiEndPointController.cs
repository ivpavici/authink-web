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
    public class ApiEndPointController : ApiController
    {
        public ApiEndPointController
        (
            IStatisticsCommands statisticsCommands
        )
        {
            apiAdapter              = new AuthinkApiAdapter();
            this.statisticsCommands = statisticsCommands;

        }

        private readonly AuthinkApiAdapter   apiAdapter;
        private readonly IStatisticsCommands statisticsCommands;

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

        [HttpPost] public HttpResponseMessage Save_Meta_Statistics_forTask(apiEnt::Task.Statistics.Meta taskRunDetails)
        {
            var authencationToken = Request.Headers.GetValues("AuthenticationToken").Single();
            if (authencationToken != buru::Api.StatisticsToken)
            {
                throw new Exception();
            }

            statisticsCommands.Update_ForTask
            (
                taskId:                taskRunDetails.TaskId,
                sucessfullClicksCount: taskRunDetails.SucessfullClicksCount,
                errorClicksCount:      taskRunDetails.ErrorClicksCount,
                timeRun:               taskRunDetails.TimeRun,
                date:                  DateTime.UtcNow
           );

           return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
