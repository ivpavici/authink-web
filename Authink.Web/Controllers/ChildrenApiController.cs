using System.Collections.Generic;
using System.Web.Http;
using Authink.Core.Model.Commands;
using Authink.Core.Model.Queries;
using Authink.Core.Model.Services;
using Authink.Web.Controllers.ChildrenApi.Model;
using ent = Authink.Core.Domain.Entities;
using System;

namespace Authink.Web.Controllers
{
    public class ChildrenApiController : ApiController
    {
        public ChildrenApiController
        (
            ILoginServices       loginServices,
            IChildQueries        childQueries,
            IChildCommands       childCommands,
            IFileSystemUtilities fileSystemUtilities,
            IPictureServices     pictureServices,
            IUserAccessRights    userAccessRights
        )
        {
            this.loginServices       = loginServices;
            this.childQueries        = childQueries;
            this.childCommands       = childCommands;
            this.fileSystemUtilities = fileSystemUtilities;
            this.pictureServices     = pictureServices;
            this.userAccessRights    = userAccessRights;
        }

        private readonly ILoginServices       loginServices;
        private readonly IChildQueries        childQueries;
        private readonly IChildCommands       childCommands;
        private readonly IFileSystemUtilities fileSystemUtilities;
        private readonly IPictureServices     pictureServices;
        private readonly IUserAccessRights    userAccessRights;
        
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

            var childId = childCommands.Create
            (
                firstname: model.FirstName,
                lastname:  model.LastName
            );

            return childQueries.GetSingle_whereId(childId);
        }

        [HttpPost]
        public void Edit(EditModel model)
        {
            if (!userAccessRights.CanEditChild(model.ChildId))
            {
                throw new UnauthorizedAccessException();
            }

            childCommands.Update
            (
                id:                model.ChildId,
                firstname:         model.Firstname,
                lastname:          model.Lastname
            );
        }
    }
}

namespace Authink.Web.Controllers.ChildrenApi.Model
{
    public class CreateModel
    {
        public string FirstName { get; set; }
        public string LastName  { get; set; }
    }

    public class EditModel
    {
        public int    ChildId           { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Firstname         { get; set; }
        public string Lastname          { get; set; }
    }
}
