using System.Collections.Generic;
using System.Web.Http;
using Authink.Core.Model.Commands;
using Authink.Core.Model.Queries;
using Authink.Core.Model.Services;
using Authink.Web.Controllers.TestsApi.Model;
using ent = Authink.Core.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using NLog;

namespace Authink.Web.Controllers
{
    public class TestsApiController : ApiController
    {
        public TestsApiController
        (
            ITestQueries      testQueries,
            ITestCommands     testCommands,
            ILoginServices    loginServices,
            IUserAccessRights userAccessRights,

            Logger logger
        )
        {
            this.testQueries      = testQueries;
            this.testCommands     = testCommands;
            this.loginServices    = loginServices;
            this.userAccessRights = userAccessRights;

            this.logger = logger;
        }

        private readonly ITestQueries      testQueries;
        private readonly ITestCommands     testCommands;
        private readonly ILoginServices    loginServices;
        private readonly IUserAccessRights userAccessRights;

        private Logger logger;

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

            if(!ModelState.IsValid)
            {
                throw new UnauthorizedAccessException();
            }

            try
            {
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
            catch (Exception ex)
            {
                logger.Error("There was an error creating test", ex);
                throw;
            }
        }

        [HttpPost]
        public System.Web.Mvc.HttpStatusCodeResult Edit(UpdateModel model)
        {
            if (!userAccessRights.CanEditTest(model.Id))
            {
                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.ExpectationFailed);
            }

            if (!ModelState.IsValid)
            {
                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.ExpectationFailed);
            }

            try
            {
                testCommands.Update
                (
                    id:               model.Id,
                    name:             model.Name,
                    shortDescription: model.ShortDescription,
                    longDescription:  model.LongDescription
                );
                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error("There was an error editing test with Id = {0}", model.Id, ex);
                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        public System.Web.Mvc.HttpStatusCodeResult Delete(int testId)
        {
            if (!userAccessRights.CanEditTest(testId))
            {
                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.ExpectationFailed);
            }

            try
            {
                testCommands.Delete(testId);
                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error("There was an error deleting  test with Id = {0}", testId, ex);
                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}

namespace Authink.Web.Controllers.TestsApi.Model
{
    public class CreateModel
    {
        [Required]
        public string Name             { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string LongDescription  { get; set; }
        public int    ChildId          { get; set; }
    }

    public class UpdateModel
    {
        public int    Id               { get; set; }
        [Required]
        public string Name             { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string LongDescription  { get; set; }
    }
}
