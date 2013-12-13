using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;

using Authink.Core.Model.Commands;
using Authink.Core.Model.Queries;
using Authink.Web.Controllers.TasksApi.Models;

using ent = Authink.Core.Domain.Entities;
using Authink.Core.Model.Services;
using System;
using NLog;
using System.Web;

namespace Authink.Web.Controllers
{
    public class TasksApiController : ApiController
    {
        public TasksApiController
        (
            ITaskQueries      taskQueries,
            ITaskCommands     taskCommands,
            IUserAccessRights userAccessRights,

            Logger logger
        )
        {
            this.taskQueries      = taskQueries;
            this.taskCommands     = taskCommands;
            this.userAccessRights = userAccessRights;
            this.logger           = logger;
        }

        private readonly ITaskQueries      taskQueries;
        private readonly ITaskCommands     taskCommands;
        private readonly IUserAccessRights userAccessRights;

        private Logger logger;

        public ent::Task.LongDetails GetSingle_whereId(int taskId)
        {
            if (!userAccessRights.CanReadTask(taskId))
            {
                throw new UnauthorizedAccessException();
            }

            return taskQueries.GetSingle_longDetails_whereId(taskId);
        }

        public IEnumerable<ent::Task.ShortDetails> GetAll_shortDetails_whereTestId(int testId)
        {
            if (!userAccessRights.CanReadTest(testId))
            {
                throw new UnauthorizedAccessException();
            }

            return taskQueries.GetAll_shortDetails_whereTestId(testId).ToList();
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult Update(EditTaskModel model)
        {
            if (!userAccessRights.CanEditTask(model.Id))
            {
                throw new UnauthorizedAccessException();
            }

            taskCommands.Update
            (
                id:          model.Id,
                description: model.Description,
                name:        model.Name
            );

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [System.Web.Http.HttpDelete]
        public HttpStatusCodeResult Remove(int taskId)
        {
            if(!userAccessRights.CanEditTask(taskId))
            {
                throw new UnauthorizedAccessException();
            }

            taskCommands.Delete(taskId);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}

namespace Authink.Web.Controllers.TasksApi.Models
{
    public class EditTaskModel
    {
        public int    Id          { get; set; }
        public string Name        { get; set; }
        public string Description { get; set; }
    }
}
