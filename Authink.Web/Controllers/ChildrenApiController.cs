using System.Collections.Generic;
using System.Web.Http;
using Authink.Core.Model.Commands;
using Authink.Core.Model.Queries;
using Authink.Core.Model.Services;
using Authink.Web.Controllers.ChildrenApi.Model;
using ent = Authink.Core.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using NLog;

namespace Authink.Web.Controllers
{
    public class ChildrenApiController : ApiController
    {
        public ChildrenApiController
        (
            ILoginServices       loginServices,
            IChildQueries        childQueries,
            IChildCommands       childCommands,
            IPictureServices     pictureServices,
            IUserAccessRights    userAccessRights,

            Logger logger
        )
        {
            this.loginServices       = loginServices;
            this.childQueries        = childQueries;
            this.childCommands       = childCommands;
            this.pictureServices     = pictureServices;
            this.userAccessRights    = userAccessRights;

            this.logger = logger;
        }

        private readonly ILoginServices       loginServices;
        private readonly IChildQueries        childQueries;
        private readonly IChildCommands       childCommands;
        private readonly IPictureServices     pictureServices;
        private readonly IUserAccessRights    userAccessRights;

        private Logger logger;
        
        [HttpGet]
        public IEnumerable<ent::Child.ShortDetails> GetAllForUser_shortDetails()
        {
            return childQueries.GetAll_forUser(false, loginServices.GetSignedInUser().Id);
        }

        [HttpGet]
        public ent::Child.ShortDetails GetOne_shortDetails(int childId)
        {
            if(!userAccessRights.CanReadChild(childId))
            {
                throw new UnauthorizedAccessException();
            }

            return childQueries.GetSingle_whereId(childId);
        }

        [HttpPost]
        public ent::Child.ShortDetails Create(CreateModel model)
        {
            if (loginServices.GetSignedInUser() == null)
            {
                throw new UnauthorizedAccessException();
            }

            if(!ModelState.IsValid)
            {
                throw new Exception();
            }

            try
            {
                var childId = childCommands.Create
                (
                    firstname: model.FirstName,
                    lastname:  model.LastName
                );

                return childQueries.GetSingle_whereId(childId);
            }
            catch (Exception ex)
            {
                logger.Error("Failed to create new child", ex);
                throw;
            }
        }

        [HttpPost]
        public System.Web.Mvc.HttpStatusCodeResult Edit(EditModel model)
        {
            if (!userAccessRights.CanEditChild(model.ChildId))
            {
                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.ExpectationFailed);
            }

            try
            {
                childCommands.Update
                (
                    id:                model.ChildId,
                    firstname:         model.Firstname,
                    lastname:          model.Lastname
                );

                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error("Failed to edit child with Id = {0}", model.ChildId, ex);
                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        public System.Web.Mvc.HttpStatusCodeResult Remove(int childId)
        {
            if (!userAccessRights.CanEditChild(childId))
            {
                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.ExpectationFailed);
            }

            try
            {
                childCommands.Delete(id: childId);

                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error("Failed to remove child with Id = {0}", childId, ex);

                return new System.Web.Mvc.HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}

namespace Authink.Web.Controllers.ChildrenApi.Model
{
    public class CreateModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName  { get; set; }
    }

    public class EditModel
    {
        public int    ChildId           { get; set; }
        public string ProfilePictureUrl { get; set; }
        [Required]
        public string Firstname         { get; set; }
        [Required]
        public string Lastname          { get; set; }
    }
}
