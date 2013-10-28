using System;
using System.Collections.Generic;
using System.Linq;

using Authink.Core.Model.Queries;

using core = Authink.Core.Domain.Entities;

namespace Authink.Web.Models.Task
{
    public class ReadModel
    {
        public ReadModel
        (
            ITaskQueries      taskQueries,
            IPictureQueries   pictureQueries,
            ISoundQueries     soundQueries
        )
        {
            this.taskQueries      = taskQueries;
            this.pictureQueries   = pictureQueries;
            this.soundQueries     = soundQueries;
        }

        private readonly ITaskQueries      taskQueries;
        private readonly IPictureQueries   pictureQueries;
        private readonly ISoundQueries     soundQueries;

        public int TaskId { get; set; }

        //public core::Task.Details Task
        //{
        //    get
        //    {
        //        if (_task == null)
        //        {
        //            _task = new Lazy<core::Task.Details>(() => taskQueries.GetSingle_whereId(this.TaskId));
        //        }
        //        return _task.Value;
        //    }
        //}
        //private Lazy<core::Task.Details> _task;

        public List<core::Picture.Details> Pictures
        {
            get
            {
                if (_pictures == null)
                {
                    _pictures = new Lazy<List<core::Picture.Details>>(() => pictureQueries.GetAll_forTaskGameplay(this.TaskId).ToList());
                }
                return _pictures.Value;
            }
        }
        private Lazy<List<core::Picture.Details>> _pictures;

        public core::Sound.Details Sound
        {
            get
            {
                if (_sound == null)
                {
                    _sound = new Lazy<core::Sound.Details>(() => soundQueries.GetSingle_byTaskId(this.TaskId));
                }
                return _sound.Value;
            }
        }
        private Lazy<core::Sound.Details> _sound;
    }
}