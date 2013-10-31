using System.Collections.Generic;
using System.Web.Http;
using Authink.Core.Model.Commands;
using Authink.Core.Model.Queries;
using Authink.Core.Model.Services;
using Authink.Web.Controllers.TestsApi.Model;
using ent = Authink.Core.Domain.Entities;
using System;

namespace Authink.Web.Controllers
{
    public class TestsApiController : ApiController
    {
        public TestsApiController
        (
            ITestQueries      testQueries,
            ITestCommands     testCommands,
            ILoginServices    loginServices,
            IUserAccessRights userAccessRights
        )
        {
            this.testQueries      = testQueries;
            this.testCommands     = testCommands;
            this.loginServices    = loginServices;
            this.userAccessRights = userAccessRights;
        }

        private readonly ITestQueries      testQueries;
        private readonly ITestCommands     testCommands;
        private readonly ILoginServices    loginServices;
        private readonly IUserAccessRights userAccessRights;

        [HttpGet]
        public IEnumerable<ent::Test.ShortDetails> GetAllTestsForChild_shortDetails(int childId)
        {
            if (!userAccessRights.CanEditChild(childId))
            {
                throw new UnauthorizedAccessException();
            }

            return testQueries.GetAll_forChild(false, childId);
        }

        [HttpGet]
        public ent::Test.LongDetails GetOne_longDetails(int testId)
        {
            if (!userAccessRights.CanReadTest(testId))
            {
                throw new UnauthorizedAccessException();
            }

            return testQueries.GetSingle_LongDetails_WhereId(testId);
        }

        [HttpPost]
        public ent::Test.ShortDetails Create(CreateModel model)
        {
            if (!userAccessRights.CanCreateTest(model.ChildId))
            {
                throw new UnauthorizedAccessException();
            }

            var testId = testCommands.Create
            (
                name:             model.Name,
                shortDescription: model.ShortDescription,
                longDescription:  model.LongDescription,
                userId:           loginServices.GetSignedInUser().Id,
                childId:          model.ChildId
            );

            return testQueries.GetSingle_ShortDetails_WhereId(testId);
        }

        [HttpPost]
        public void Edit(UpdateModel model)
        {
            if (!userAccessRights.CanEditTest(model.Id))
            {
                throw new UnauthorizedAccessException();
            }

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
            if (!userAccessRights.CanEditTest(testId))
            {
                throw new UnauthorizedAccessException();
            }

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
