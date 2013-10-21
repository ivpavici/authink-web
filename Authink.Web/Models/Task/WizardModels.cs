using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Authink.Core.Model.Queries;
using Authink.Web.Controllers.Task.Helpers;

using ent = Authink.Core.Domain.Entities;

namespace Authink.Web.Models.Task
{
    public class ChooseDifficultyModel
    {
        public string    TaskName                  { get; set; }
        public string    UploadPicturesRouteName   { get; set; }
        public List<int> AvailableTaskDifficulties { get; set; }
    }
    public class ChooseTypeModel
    {
        public IDictionary<string, KnownTaskTypes> TaskTypes { get; set; }
    }
    public class InputMetaDataModel
    {
        [Required(ErrorMessage = "You must fill out a title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "You must fill out a description")]
        public string Description { get; set; }

        public string TaskKey { get; set; }
        public bool IsVoiceCommandRequired { get; set; }

        public string ErrorMessage { get; set; }
    }
    public class FeedbackModel
    {
        public FeedbackModel
        (
            ITestQueries testQueries
        )
        {
            this.testQueries = testQueries;
        }

        private readonly ITestQueries testQueries;

        public int TaskId { get; set; }

        public ent::Test.ShortDetails Test
        {
            get
            {
                if (_test == null)
                {
                    _test = new Lazy<ent::Test.ShortDetails>(() => testQueries.GetTest_thatContainsTask(this.TaskId));
                }
                return _test.Value;
            }
        }
        private Lazy<ent::Test.ShortDetails> _test;
    }

    
}