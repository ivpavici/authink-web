using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using Authink.Core.Model.Services;
using Authink.Web.Models.Statistics;
using Authink.Web.Models.Statistics.JsonModels;

namespace Authink.Web.Controllers
{
    public class StatisticsController : Controller
    {
        public StatisticsController
        (
            IUserAccessRights userAccessRights,

            Func<GetForTaskModel> getForTaskModelFactory
        )
        {
            this.userAccessRights = userAccessRights;

            this.getForTaskModelFactory = getForTaskModelFactory;
        }

        private readonly Func<GetForTaskModel> getForTaskModelFactory;

        private readonly IUserAccessRights userAccessRights;
 
        public ActionResult GetForTask(int taskId)
        {
            if(!userAccessRights.CanReadTask(taskId))
            {
                return new HttpNotFoundResult();
            }

            var model = getForTaskModelFactory();
            model.TaskId = taskId;

            model.CorrectAnswerCount = model.TaskStatistics
                                            .SucessfullClicksCount
                                            .Split(';')
                                            .Where(part=> !string.IsNullOrEmpty(part))
                                            .Select(part => Convert.ToInt32(part))
                                            .ToList();

            model.WrongAnswerCout = model.TaskStatistics
                                         .ErrorClicksCount
                                         .Split(';')
                                         .Where(part => !string.IsNullOrEmpty(part))
                                         .Select(part => Convert.ToInt32(part))
                                         .ToList();

            model.Series     = new List<GetForTaskJsonModel>();
            model.Categories = new List<string>();

            model.Series.Add( new GetForTaskJsonModel
            {
                name = "Correct answers",
                data = model.CorrectAnswerCount
            });

            model.Series.Add ( new GetForTaskJsonModel
            {
                name = "Wrong answers",
                data = model.WrongAnswerCout
            });
            model.Categories = model.TaskStatistics
                                  .Dates
                                  .Split(';')
                                  .Where(part => !string.IsNullOrEmpty(part))
                                  .ToList();

            var serializer = new JavaScriptSerializer();

            model.Categories_inJson = serializer.Serialize(model.Categories);
            model.Series_inJson     = serializer.Serialize(model.Series);

            return PartialView
            (
                viewName: "_GetForTask",
                model:    model
            );
        }
    }
}
