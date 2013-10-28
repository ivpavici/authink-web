using System.Web.Http;

using Authink.Core.Model.Queries;
using ent = Authink.Core.Domain.Entities;

namespace Authink.Web.Controllers
{
    public class TasksApiController : ApiController
    {
        public TasksApiController
        (
            ITaskQueries taskQueries
        )
        {
            this.taskQueries = taskQueries;
        }

        private readonly ITaskQueries taskQueries;

        public ent::Task.LongDetails GetSingle_whereId(int taskId)
        {
            return taskQueries.GetSingle_longDetails_whereId(taskId);
        }
    }
}
