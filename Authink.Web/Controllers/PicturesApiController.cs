using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Linq;

using Authink.Core.Model.Commands;
using Authink.Core.Model.Queries;
using Authink.Core.Model.Services;
using Authink.Web.Controllers.PicturesApi.Models;

using buru = Authink.Core.Domain.Rules;
using ent  = Authink.Core.Domain.Entities;

using System.Threading.Tasks;
using Authink.Core.Fx;

namespace Authink.Web.Controllers
{
    public class PicturesApiController : ApiController
    {
        public PicturesApiController
        (
            IUserAccessRights    userAccessRights,
            IPictureCommands     pictureCommands,
            IPictureQueries      pictureQueries,
            IPictureServices     pictureServices,
            IChildCommands       childrenCommands
        )
        {
            this.userAccessRights    = userAccessRights;
            this.pictureCommands     = pictureCommands;
            this.pictureQueries      = pictureQueries;
            this.pictureServices     = pictureServices;
            this.childrenCommands    = childrenCommands;
        }

        private readonly IUserAccessRights    userAccessRights;
        private readonly IPictureCommands     pictureCommands;
        private readonly IPictureQueries      pictureQueries;
        private readonly IPictureServices     pictureServices;
        private readonly IChildCommands       childrenCommands;

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
        public async Task<ent::Picture> Tasks_InsertPictureForUpdate(int pictureId, int taskId)
        {
            if (!userAccessRights.CanEditTask(taskId))
            {
                throw new UnauthorizedAccessException();
            }

            var memoryStreamProvider = new MultipartMemoryStreamProvider();

            await Request.Content.ReadAsMultipartAsync(memoryStreamProvider);

            var file     = memoryStreamProvider.Contents.First();
            var fileName = FileHelpers.CreateUniqueFileName(file.Headers.ContentDisposition.FileName.Trim('\"'));
            var fileData = await file.ReadAsByteArrayAsync();

            var savePath =  pictureServices.Save
            (
                pictureName:       fileName, 
                pictureContent:    fileData,
                relatedId:         taskId,
                baseSavePath:      buru::Picture.Task.DefaultSavePath, 
                resizeQueryString: buru::Picture.Task.DefaultResizeQuerystring
            );

            pictureCommands.Update(pictureId, savePath);

            return pictureQueries.GetSingle_whereId(pictureId);
        }

        [System.Web.Http.HttpPost]
        public async Task<object> Children_InsertPictureForUpdate(int childId)
        {
            if (!userAccessRights.CanEditChild(childId))
            {
                throw new UnauthorizedAccessException();
            }

            var memoryStreamProvider = new MultipartMemoryStreamProvider();

            await Request.Content.ReadAsMultipartAsync(memoryStreamProvider);

            var file     = memoryStreamProvider.Contents.First();
            var fileName = FileHelpers.CreateUniqueFileName(file.Headers.ContentDisposition.FileName.Trim('\"'));
            var fileData = await file.ReadAsByteArrayAsync();

            var savePath = pictureServices.Save
            (
                pictureName:       fileName,
                pictureContent:    fileData,
                relatedId:         childId,
                baseSavePath:      buru::Picture.Children.DefaultSavePath,
                resizeQueryString: buru::Picture.Children.DefaultResizeQuerystring
            );

            childrenCommands.UpdatePicture(childId, savePath);

            return new { pictureUrl = savePath };
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
