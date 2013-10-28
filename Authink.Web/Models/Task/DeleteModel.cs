using System;

using Authink.Core.Model.Queries;

using ent = Authink.Core.Domain.Entities;

namespace Authink.Web.Models.Task
{
    public class DeleteModel
    {
        public DeleteModel
        (
            ITaskQueries taskQueries
        )
        {
            this.taskQueries = taskQueries;
        }

        private readonly ITaskQueries taskQueries;

        public int TaskId { get; set; }

        //public ent::Task.Details Task
        //{
        //    get
        //    {
        //        if (_task == null)
        //        {
        //            _task = new Lazy<ent::Task.Details>(() => taskQueries.GetSingle_whereId(this.TaskId));
        //        }
        //        return _task.Value;
        //    }
        //}
        //private Lazy<ent::Task.Details> _task;
    }
}