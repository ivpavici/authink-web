using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Authink.Core.Model.Commands;
using Authink.Core.Model.Services;
using Authink.Web.Models.Session;
using Authink.Web.Models.Picture;

using buru = Authink.Core.Domain.Rules;
using ent =  Authink.Core.Domain.Entities;

namespace Authink.Web.Controllers
{
    public partial class PictureController : Controller
    {
        public PictureController
        (
            IPictureCommands     pictureCommands,
            IPictureServices     pictureServices,
            IFileSystemUtilities fileSystemUtilities,
            IUserAccessRights    userAccessRights,

            Func<EditWithColorsModel> editWithColorsModelFactory,
            Func<EditModel>           editModelFactory,

            Func<UploadPictures_SimpleTasksWithPictures> uploadPictures_SimpleTasksWithPicturesModelFactory,
            Func<UploadPictures_DetectColorsModel>       uploadPictures_DetectColorsModelFactory,
            Func<UploadPictures_DetectItemModel>         uploadPictures_DetectItemModelFactory,
            Func<UploadPictures_OrderBySizeModel>        uploadPictures_OrderBySizeModelFactory,

            HttpContextBase httpContextBase
        )
        {
            this.pictureCommands     = pictureCommands;
            this.pictureServices     = pictureServices;
            this.fileSystemUtilities = fileSystemUtilities;
            this.userAccessRights    = userAccessRights;

            this.editWithColorsModelFactory = editWithColorsModelFactory;
            this.editModelFactory           = editModelFactory;

            this.uploadPictures_SimpleTasksWithPicturesModelFactory = uploadPictures_SimpleTasksWithPicturesModelFactory;
            this.uploadPictures_DetectColorsModelFactory            = uploadPictures_DetectColorsModelFactory;
            this.uploadPictures_DetectItemModelFactory              = uploadPictures_DetectItemModelFactory;
            this.uploadPictures_OrderBySizeModelFactory             = uploadPictures_OrderBySizeModelFactory;

            this.httpContextBase = httpContextBase;
        }

        private readonly IPictureCommands     pictureCommands;
        private readonly IPictureServices     pictureServices;
        private readonly IFileSystemUtilities fileSystemUtilities;
        private readonly IUserAccessRights    userAccessRights;

        private readonly Func<EditWithColorsModel> editWithColorsModelFactory;
        private readonly Func<EditModel>           editModelFactory;

        private readonly Func<UploadPictures_SimpleTasksWithPictures> uploadPictures_SimpleTasksWithPicturesModelFactory;
        private readonly Func<UploadPictures_DetectColorsModel>       uploadPictures_DetectColorsModelFactory;
        private readonly Func<UploadPictures_DetectItemModel>         uploadPictures_DetectItemModelFactory;
        private readonly Func<UploadPictures_OrderBySizeModel>        uploadPictures_OrderBySizeModelFactory;

        private readonly HttpContextBase httpContextBase;

        private static readonly IDictionary<string, string> KnownPictureEditMappings = new Dictionary<string, string>()
        {
            { buru.Task.Keys.ContinueSequence, "Picture_Edit_simple" }, { buru.Task.Keys.DetectColors,  "Picture_Edit_withColors"}, { buru.Task.Keys.DetectDifferentItems, "Picture_Edit_simple" },
            { buru.Task.Keys.OrderBySize,      "Picture_Edit_simple" }, { buru.Task.Keys.PairSameItems, "Picture_Edit_simple"    }, { buru.Task.Keys.VoiceCommands,        "Picture_Edit_simple" }
        };
    }
    public partial class PictureController
    {
        [HttpGet] public ActionResult Edit(int taskId, int? pictureId)
        {
            if(!Request.IsAjaxRequest() || !userAccessRights.CanEditPicture((int)pictureId) || !userAccessRights.CanEditTask(taskId))
            {
                return new HttpNotFoundResult();
            }

            var model    = editModelFactory();
            model.TaskId = (int)taskId;

            httpContextBase.Session["CurrentEditTaskId"] = taskId;

            return RedirectToRoute
            (
                routeName:   KnownPictureEditMappings[model.Task.Type],
                routeValues: new {pictureId = pictureId}
            );
        }

        [HttpPost] public ActionResult Edit_simple(EditSimpleModel model, HttpPostedFileBase picture)
        {
            if (!userAccessRights.CanEditTask(model.TaskId))
            {
                return new HttpNotFoundResult();
            }

            if(picture==null || !picture.ContentType.Contains("image"))
            {
                return RedirectToRoute
                (
                    "Shell"
                );
            }

            var pictureContent = fileSystemUtilities.Transform_HttpPostedFileBase_Into_Bytes(picture);

            var newSavePath = pictureServices.SaveToFileSystem(picture.FileName, pictureContent, model.TaskId, buru.Picture.Task.DefaultSavePath, buru.Picture.Task.DefaultResizeQuerystring);
            pictureCommands.Update(model.PictureId, newSavePath);

            return RedirectToRoute("Shell");
        }

        [HttpGet]  public ActionResult Edit_withColors(int pictureId)
        {
            if (!userAccessRights.CanEditPicture(pictureId))
            {
                return new HttpNotFoundResult();
            }

            var model       = editWithColorsModelFactory();
            model.PictureId = pictureId;

            model.WrongColorsToEdit = model.Colors
                                           .Where (color => !color.IsCorrect                )
                                           .Select(color => new Color(color.Id, color.Value))
                                           .ToList();

            model.CorrectColorToEdit = model.Colors
                                            .Where (color => color.IsCorrect                 )
                                            .Select(color => new Color(color.Id, color.Value))
                                            .Single();
            return PartialView
            (
                viewName: "_Edit_withColors",
                model:    model
            );
        }
        [HttpPost] public ActionResult Edit_withColors(EditWithColorsModel model, HttpPostedFileBase picture)
        {
            if (httpContextBase.Session["CurrentEditTaskId"] == null)
            {
                return RedirectToRoute("Home");
            }

            if (picture != null && !picture.ContentType.Contains("image"))
            {
                return PartialView
                (
                    viewName: "_Edit_withColors",
                    model:    model
                );
            }
            var taskId   = (int)httpContextBase.Session["CurrentEditTaskId"];
            model.TaskId = taskId;

            if(picture != null)
            {
                var pictureContent = fileSystemUtilities.Transform_HttpPostedFileBase_Into_Bytes(picture);

                if (httpContextBase.Session["CurrentEditTaskId"] == null)
                {
                    return RedirectToRoute("Home");
                }

                var newSavePath = pictureServices.SaveToFileSystem(picture.FileName, pictureContent, taskId, buru.Picture.Task.DefaultSavePath, buru.Picture.Task.DefaultResizeQuerystring);
                pictureCommands.Update(model.PictureId, newSavePath);
            }

            foreach (var color in model.WrongColorsToEdit)
            {
               pictureCommands.Update_color(color.Id, color.Value);
            }

            pictureCommands.Update_color(model.CorrectColorToEdit.Id, model.CorrectColorToEdit.Value);

            return RedirectToRoute("Home", new { test = model.Test.Id });
        }
    }

    public partial class PictureController
    {
        [HttpGet]  public ActionResult UploadPictures_SimpleTasksWithPictures(int taskDifficulty)
        {
            if (httpContextBase.Session["TaskKey"] == null)
            {
                return new HttpNotFoundResult();
            }

            var model = uploadPictures_SimpleTasksWithPicturesModelFactory();
            model.TaskDifficulty = taskDifficulty;
            model.TaskKey = httpContextBase.Session["TaskKey"].ToString();

            HttpContext.Session["TaskDifficulty"] = taskDifficulty;

            return View
            (
                model: model
            );
        }
        [HttpPost] public ActionResult UploadPictures_SimpleTasksWithPictures(UploadPictures_SimpleTasksWithPictures model, List<HttpPostedFileBase> pictures)
        {
            if (pictures.Any(picture => picture == null || !picture.ContentType.Contains("image")))
            {
                model.ErrorMessage = "Niste odabrali slike ili ste odabrali pogrešni format";
                return View(model);
            }

            if (!IsNumberOfUploadedPicturesCorrect(pictures.Count, model.NumberOfPictures))
            {
                model.ErrorMessage = GenerateTaskUploadErrorMessage(pictures.Count, model.NumberOfPictures);
                return View(model);
            }

            var uploadedPicturesData = pictures.Select(picture => new SessionPictureData
            (
                filename: picture.FileName,
                content:  fileSystemUtilities.Transform_HttpPostedFileBase_Into_Bytes(picture),
                isAnswer: false
            ))
            .ToList();

            HttpContext.Session["Pictures"] = uploadedPicturesData;

            return RedirectToRoute
            (
                routeName: "InputMetaData"
            );
        }

        [HttpGet]  public ActionResult UploadPictures_DetectColors(int taskDifficulty)
        {
            var model = uploadPictures_DetectColorsModelFactory();
            model.TaskDifficulty = taskDifficulty;

            HttpContext.Session["TaskDifficulty"] = taskDifficulty;

            return View
            (
                model: model
            );
        }
        [HttpPost] public ActionResult UploadPictures_DetectColors(UploadPictures_DetectColorsModel model, List<HttpPostedFileBase> pictures)
        {
            if (pictures.Any(picture => picture == null || !picture.ContentType.Contains("image")))
            {
                model.ErrorMessage = "Niste odabrali slike ili ste odabrali pogrešni format";
                return View(model);
            }
            if (!IsNumberOfUploadedPicturesCorrect(pictures.Count, model.NumberOfPicturesAndWrongColorsMapping.NumberOfPictures))
            {
                model.ErrorMessage = GenerateTaskUploadErrorMessage(pictures.Count, model.TaskDifficulty);
                return View(model);
            }

            HttpContext.Session["Pictures"] = pictures.Select(picture => new SessionPictureData
            (
                filename: picture.FileName,
                content:  fileSystemUtilities.Transform_HttpPostedFileBase_Into_Bytes(picture),
                isAnswer: false
            )).ToList();

            HttpContext.Session["Colors"]   = model.Colors.Select(color => new SessionColorData
            (
                correctColor: color.CorrectColor,
                wrongColors:  new List<string>(color.WrongColors)
            )).ToList();

            return RedirectToRoute
            (
                routeName: "InputMetaData"
            );
        }

        [HttpGet]  public ActionResult UploadPictures_DetectItem(int taskDifficulty)
        {
            var model = uploadPictures_DetectItemModelFactory();
            model.TaskDifficulty = taskDifficulty;

            HttpContext.Session["TaskDifficulty"] = taskDifficulty;

            return View
            (
                model: model
            );
        }
        [HttpPost] public ActionResult UploadPictures_DetectItem(UploadPictures_DetectItemModel model, List<HttpPostedFileBase> pictures, HttpPostedFileBase correctPicture)
        {
            if (correctPicture == null || !correctPicture.ContentType.Contains("image") || pictures.Any(picture => picture == null || !picture.ContentType.Contains("image")))
            {
                model.ErrorMessage = "Wrong image format";
                return View(model);
            }

            if (!IsNumberOfUploadedPicturesCorrect(pictures.Count + 1, model.TaskDifficulty))
            {
                model.ErrorMessage = GenerateTaskUploadErrorMessage(pictures.Count + 1, model.TaskDifficulty);
                return View(model);
            }

            var uploadedPicturesData = pictures.Select(picture => new SessionPictureData
            (
                filename: picture.FileName,
                content:  fileSystemUtilities.Transform_HttpPostedFileBase_Into_Bytes(picture),
                isAnswer: false
            ))
            .ToList();

            uploadedPicturesData.Add
            (
                new SessionPictureData
                (
                    filename: correctPicture.FileName,
                    content:  fileSystemUtilities.Transform_HttpPostedFileBase_Into_Bytes(correctPicture),
                    isAnswer: true
                )
            );

            HttpContext.Session["Pictures"] = uploadedPicturesData;

            return RedirectToRoute
            (
                routeName: "InputMetaData"
            );
        }

        [HttpGet]  public ActionResult UploadPictures_OrderBySize(int taskDifficulty)
        {
            var model = uploadPictures_OrderBySizeModelFactory();
            model.TaskDifficulty = taskDifficulty;

            HttpContext.Session["TaskDifficulty"] = taskDifficulty;

            return View
            (
                model: model
            );
        }
        [HttpPost] public ActionResult UploadPictures_OrderBySize(UploadPictures_OrderBySizeModel model, HttpPostedFileBase picture)
        {
            if (picture == null)
            {
                model.ErrorMessage = "Niste odabrali slike ili ste odabrali pogrešni format";
                return View(model);
            }

            var uploadedPicturesData = new SessionPictureData
            (
                filename: picture.FileName,
                content:  fileSystemUtilities.Transform_HttpPostedFileBase_Into_Bytes(picture),
                isAnswer: false
            );

            HttpContext.Session["Pictures"] = uploadedPicturesData;

            return RedirectToRoute
            (
                routeName: "InputMetaData"
            );
        }

        private static bool   IsNumberOfUploadedPicturesCorrect(int numberOfPicturesUploaded, int taskDifficulty)
        {
            return numberOfPicturesUploaded == taskDifficulty;
        }
        private static string GenerateTaskUploadErrorMessage   (int numberOfPicturesUploaded, int taskDifficulty)
        {
            return
                String.Format("Uploadali ste {0} slike, trebate uploadat {1} slike", numberOfPicturesUploaded, taskDifficulty);
        }
    }
}
