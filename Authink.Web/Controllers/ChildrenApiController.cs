using System.Collections.Generic;
using System.Web.Http;
using Authink.Core.Model.Commands;
using Authink.Core.Model.Queries;
using Authink.Core.Model.Services;
using Authink.Web.Controllers.ChildrenApi.Model;
using ent = Authink.Core.Domain.Entities;

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
            IPictureServices     pictureServices
        )
        {
            this.loginServices       = loginServices;
            this.childQueries        = childQueries;
            this.childCommands       = childCommands;
            this.fileSystemUtilities = fileSystemUtilities;
            this.pictureServices     = pictureServices;
        }

        private readonly ILoginServices       loginServices;
        private readonly IChildQueries        childQueries;
        private readonly IChildCommands       childCommands;
        private readonly IFileSystemUtilities fileSystemUtilities;
        private readonly IPictureServices     pictureServices;
        
        [HttpGet]
        public IEnumerable<ent::Child.ShortDetails> GetAllForUser_shortDetails()
        {
            return childQueries.GetAll_forUser(false, loginServices.GetSignedInUser().Id);
        }

        [HttpGet]
        public ent::Child.ShortDetails GetOne_shortDetails(int childId)
        {
            return childQueries.GetSingle_whereId(childId);
        }

        [HttpPost]
        public object Create(CreateModel model)
        {
            var childId = childCommands.Create
            (
                firstname: model.FirstName,
                lastname:  model.LastName
            );

            return new {childId};
        }

        [HttpPost]
        public void Edit(EditModel model)
        {
            //if (picture != null)
            //{
            //    var pictureContent = fileSystemUtilities.Transform_HttpPostedFileBase_Into_Bytes(picture);
            //    var newProfilePictureUrl = pictureServices.SaveToFileSystem(picture.FileName, pictureContent, model.Child.Id, buru::Picture.Children.DefaultSavePath, buru::Picture.Children.DefaultResizeQuerystring);
            //    childCommands.Update
            //    (
            //        id: model.ChildId,
            //        firstname: model.Firstname,
            //        lastname: model.Lastname,
            //        profilePictureUrl: newProfilePictureUrl
            //    );
            //}
            //else
            //{
                childCommands.Update
                (
                    id:                model.ChildId,
                    firstname:         model.Firstname,
                    lastname:          model.Lastname,
                    profilePictureUrl: model.ProfilePictureUrl
                );
            //}
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
