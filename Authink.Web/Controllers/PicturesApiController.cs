using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Authink.Core.Model.Commands;
using Authink.Core.Model.Queries;
using Authink.Core.Model.Services;
using Authink.Web.Controllers.PicturesApi.Models;
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

        [System.Web.Http.HttpGet]
        public IEnumerable<ent::Picture> GetAll_forTaskGameplay(int taskId)
        {
            if (!userAccessRights.CanEditTask(taskId))
            {
                throw new UnauthorizedAccessException();
            }

            return pictureQueries.GetAll_forTaskGameplay(taskId);
        }

        [System.Web.Http.HttpPost]
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

        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult UpdateColorsForPicture(UpdateColorsForPictureModel model)
        {
            foreach (var color in model.WrongColors)
            {
                pictureCommands.Update_color(color.Id, color.Value);
            }

            pictureCommands.Update_color(model.CorrectColor.Id, model.CorrectColor.Value);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
    public class MyMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public MyMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var uploadedFileName = headers.ContentDisposition.FileName.Replace("\"", string.Empty);

            var fileName = MakeFileNameOnUrlUnique(RootPath, uploadedFileName);

            return fileName.Replace("\"", string.Empty);
        }

        private string MakeFileNameOnUrlUnique(string rootPath, string fileName, int index = 1)
        {
            var savePath = rootPath + '/' + fileName;

            if (File.Exists(savePath))
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(savePath);
                var fileExtenstion           = Path.GetExtension(savePath);

                return MakeFileNameOnUrlUnique(rootPath, fileNameWithoutExtension + index + fileExtenstion, index + 1);
            }

            return fileName;
        }
    }
}

namespace Authink.Web.Controllers.PicturesApi.Models
{
    public class UpdateColorsForPictureModel
    {
        public List<Color> WrongColors  { get; set; }
        public Color       CorrectColor { get; set; }
    }

    public class Color
    {
        public int    Id    { get; set; }
        public string Value { get; set; }
    }
 }
