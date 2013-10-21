using System;
using System.Collections.Generic;

using Authink.Core.Model.Queries;
using Authink.Web.Models.Statistics.JsonModels;

using ent = Authink.Core.Domain.Entities;

namespace Authink.Web.Models.Statistics
{
    public class GetForTaskModel
    {
        public GetForTaskModel
        (
            IStatisticsQueries statisticsQueries
        )
        {
            this.statisticsQueries = statisticsQueries;
        }

        private readonly IStatisticsQueries statisticsQueries;

        public int          TaskId             { get; set; }
        public List<int>    CorrectAnswerCount { get; set; }
        public List<int>    WrongAnswerCout    { get; set; }
        public List<string> RunTimes           { get; set; }

        public List<string> Categories { get; set; }
        public List<GetForTaskJsonModel> Series { get; set; }

        public string Categories_inJson { get; set; }
        public string Series_inJson { get; set; }

        private Lazy<ent::Statistics.Meta> _taskStatistics;

        public ent::Statistics.Meta TaskStatistics
        {
            get
            {
                if (_taskStatistics == null)
                {
                    _taskStatistics = new Lazy<ent::Statistics.Meta>(() => statisticsQueries.GetStatistics_Meta_ForTask(this.TaskId));
                }
                return _taskStatistics.Value;
            }
        }
    }
}

namespace Authink.Web.Models.Statistics.JsonModels
{
    public class GetForTaskJsonModel
    {
        public string name { get; set; }
        public List<int> data { get; set; }
    }

}