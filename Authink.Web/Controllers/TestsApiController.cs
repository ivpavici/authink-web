using System.Collections.Generic;
using System.Web.Http;
using Authink.Core.Model.Commands;
using Authink.Core.Model.Queries;
using Authink.Core.Model.Services;
using Authink.Web.Controllers.TestsApi.Model;
using ent = Authink.Core.Domain.Entities;

namespace Authink.Web.Controllers
{
    public class TestsApiController : ApiController
    {
        public TestsApiController
        (
            ITestQueries   testQueries,
            ITestCommands  testCommands,
            ILoginServices loginServices
        )
        {
            this.testQueries   = testQueries;
            this.testCommands  = testCommands;
            this.loginServices = loginServices;
        }

        private readonly ITestQueries   testQueries;
        private readonly ITestCommands  testCommands;
        private readonly ILoginServices loginServices;

        [HttpGet]
        public IEnumerable<ent::Test.ShortDetails> GetAllTestsForChild_shortDetails(int childId)
        {
            return testQueries.GetAll_forChild(false, childId);
        }

        [HttpGet]
        public ent::Test.LongDetails GetOne_longDetails(int testId)
        {
            return testQueries.GetSingle_LongDetails_WhereId(testId);
        }

        [HttpPost]
        public object Create(CreateModel model)
        {
            var testId = testCommands.Create
            (
                name:             model.Name,
                shortDescription: model.ShortDescription,
                longDescription:  model.LongDescription,
                userId:           loginServices.GetSignedInUser().Id,
                childId:          model.ChildId
            );

            return new {testId};
        }

        [HttpPost]
        public void Edit(UpdateModel model)
        {
            testCommands.Update
            (
                id:               model.Id,
                name:             model.Name,
                shortDescription: model.ShortDescription,
                longDescription:  model.LongDescription
            );
        }

        [HttpDelete]
        public void Delete(int testId)
        {
            testCommands.Delete(testId);
        }
    }
}

namespace Authink.Web.Controllers.TestsApi.Model
{
    public class CreateModel
    {
        public string Name             { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription  { get; set; }
        public int    ChildId          { get; set; }
    }

    public class UpdateModel
    {
        public int    Id               { get; set; }
        public string Name             { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription  { get; set; }
    }
}
