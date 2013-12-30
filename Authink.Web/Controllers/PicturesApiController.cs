using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Linq;

using Authink.Core.Model.Commands;
using Authink.Core.Model.Queries;
using Authink.Core.Model.Services;
using Authink.Data.ResourceFileStorage;
using Authink.Web.Controllers.PicturesApi.Models;

using buru = Authink.Core.Domain.Rules;
using ent  = Authink.Core.Domain.Entities;

using System.Threading.Tasks;
using Authink.Core.Fx;
using NLog;

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
            IFileStorageAdapter  fileStorageAdapter,
            IChildCommands       childrenCommands,
            Logger               logger
        )
        {
            this.userAccessRights    = userAccessRights;
            this.pictureCommands     = pictureCommands;
            this.pictureQueries      = pictureQueries;
            this.pictureServices     = pictureServices;
            this.fileStorageAdapter  = fileStorageAdapter;
            this.childrenCommands    = childrenCommands;
            this.logger              = logger;
        }

        private readonly IUserAccessRights    userAccessRights;
        private readonly IPictureCommands     pictureCommands;
        private readonly IPictureQueries      pictureQueries;
        private readonly IPictureServices     pictureServices;
        private readonly IFileStorageAdapter  fileStorageAdapter;
        private readonly IChildCommands       childrenCommands;

        private Logger logger;

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

            try
            {
                logger.Info("Updating task picture");

                var memoryStreamProvider = new MultipartMemoryStreamProvider();

                await Request.Content.ReadAsMultipartAsync(memoryStreamProvider);
                
                var file     = memoryStreamProvider.Contents.First();
                var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                var fileData = await file.ReadAsByteArrayAsync();

                var resizedPicture = pictureServices.ResizePicture(fileData, buru::Picture.Task.DefaultResizeQuerystring);

                var savePath = fileStorageAdapter.Upload(new ResourceFileStorageAdapter.ResourceFile(fileName, resizedPicture), buru::Picture.Task.DefaultSavePath);

                pictureCommands.Update(pictureId, savePath.ToString());
                
                return pictureQueries.GetSingle_whereId(pictureId);
            }
            catch (Exception ex)
            {
                logger.Error("Update failed for picture with Id = {0}", pictureId, ex);
                throw;
            }
        }

        [System.Web.Http.HttpPost]
        public async Task<object> Children_InsertPictureForUpdate(int childId)
        {
            if (!userAccessRights.CanEditChild(childId))
            {
                throw new UnauthorizedAccessException();
            }

            try
            {
                logger.Info("Updating child picture");

                var memoryStreamProvider = new MultipartMemoryStreamProvider();

                await Request.Content.ReadAsMultipartAsync(memoryStreamProvider);

                var file     = memoryStreamProvider.Contents.First();
                var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                var fileData = await file.ReadAsByteArrayAsync();

                var resizedPicture = pictureServices.ResizePicture(fileData, buru::Picture.Children.DefaultResizeQuerystring);
                var savePath       = fileStorageAdapter.Upload(new ResourceFileStorageAdapter.ResourceFile(fileName, resizedPicture), buru::Picture.Children.DefaultSavePath);

                childrenCommands.UpdatePicture(childId, savePath.ToString());
                return new { pictureUrl = savePath };
            }
            catch(Exception ex)
            {
                logger.Error("Save profile picture for child with Id = {0} failed", childId, ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult UpdateColorsForPicture(UpdateColorsForPictureModel model)
        {
            logger.Info("Updating colors for  picture");
            foreach (var color in model.WrongColors)
            {
                pictureCommands.Update_color(color.Id, color.Value);
            }
            try
            {
                pictureCommands.Update_color(model.CorrectColor.Id, model.CorrectColor.Value);

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.Error("Update colors failed", ex);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
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
