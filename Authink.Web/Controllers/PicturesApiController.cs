using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Authink.Core.Model.Commands;
using Authink.Core.Model.Queries;
using Authink.Core.Model.Services;

using ent = Authink.Core.Domain.Entities;

using System.Threading.Tasks;

namespace Authink.Web.Controllers
{
    public class PicturesApiController : ApiController
    {
        public PicturesApiController
        (
            IUserAccessRights userAccessRights,
            IPictureCommands  pictureCommands,
            IPictureQueries   pictureQueries
        )
        {
            this.userAccessRights = userAccessRights;
            this.pictureCommands  = pictureCommands;
            this.pictureQueries   = pictureQueries;
        }

        private readonly IUserAccessRights userAccessRights;
        private readonly IPictureCommands  pictureCommands;
        private readonly IPictureQueries   pictureQueries;

        [HttpGet]
        public IEnumerable<ent::Picture> GetAll_forTaskGameplay(int taskId)
        {
            if (!userAccessRights.CanEditTask(taskId))
            {
                throw new UnauthorizedAccessException();
            }

            return pictureQueries.GetAll_forTaskGameplay(taskId);
        }
        [HttpPost]
        public async Task<ent::Picture> InsertPictureForUpdate(int pictureId, int taskId)
        {
            if (!userAccessRights.CanEditTask(taskId))
            {
                throw new UnauthorizedAccessException();
            }

            var savePath           = HttpContext.Current.Server.MapPath("~/Content/Images/Tasks/" + taskId);
            var dataStreamProvider = new MyMultipartFormDataStreamProvider(savePath);


            await Request.Content.ReadAsMultipartAsync(dataStreamProvider);

            foreach (var file in dataStreamProvider.FileData)
            {
                var newPictureSavePath = file.LocalFileName.Substring(file.LocalFileName.IndexOf("Content", StringComparison.Ordinal)).Replace("\\","/");

                pictureCommands.Update(pictureId, newPictureSavePath);
            }

            return pictureQueries.GetSingle_whereId(pictureId);
        }
    }
    public class MyMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public MyMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var fileName = headers.ContentDisposition.FileName;

            return fileName.Replace("\"", string.Empty);
        }
    }
}
