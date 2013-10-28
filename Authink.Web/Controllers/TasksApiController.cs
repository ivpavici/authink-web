using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;

using Authink.Core.Model.Commands;
using Authink.Core.Model.Queries;
using Authink.Web.Controllers.TasksApi.Models;

using ent = Authink.Core.Domain.Entities;

namespace Authink.Web.Controllers
{
    public class TasksApiController : ApiController
    {
        public TasksApiController
        (
            ITaskQueries  taskQueries,
            ITaskCommands taskCommands
        )
        {
            this.taskQueries  = taskQueries;
            this.taskCommands = taskCommands;
        }

        private readonly ITaskQueries  taskQueries;
        private readonly ITaskCommands taskCommands;

        public ent::Task.LongDetails GetSingle_whereId(int taskId)
        {
            return taskQueries.GetSingle_longDetails_whereId(taskId);
        }

        public IEnumerable<ent::Task.ShortDetails> GetAll_shortDetails_whereTestId(int testId)
        {
            return taskQueries.GetAll_shortDetails_whereTestId(testId).ToList();
        }

        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult Update(EditTaskModel model)
        {
            taskCommands.Update
            (
                id:          model.Id,
                description: model.Description,
                name:        model.Name
            );

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
