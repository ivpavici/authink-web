using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authink.Core.Model.Commands;
using Authink.Core.Model.Services;
using Authink.Web.Models.Session;
using Authink.Web.Models.Picture;

using Resources;
using buru = Authink.Core.Domain.Rules;
using ent =  Authink.Core.Domain.Entities;

namespace Authink.Web.Controllers
{
    public partial class PictureController : Controller
    {
        public PictureController
        (
            Func<UploadPictures_SimpleTasksWithPictures> uploadPictures_SimpleTasksWithPicturesModelFactory,
            Func<UploadPictures_DetectColorsModel>       uploadPictures_DetectColorsModelFactory,
            Func<UploadPictures_DetectItemModel>         uploadPictures_DetectItemModelFactory,
            Func<UploadPictures_OrderBySizeModel>        uploadPictures_OrderBySizeModelFactory,

            HttpContextBase httpContextBase,

            IFileSystemUtilities fileSystemUtilities
        )
        {

            this.uploadPictures_SimpleTasksWithPicturesModelFactory = uploadPictures_SimpleTasksWithPicturesModelFactory;
            this.uploadPictures_DetectColorsModelFactory            = uploadPictures_DetectColorsModelFactory;
            this.uploadPictures_DetectItemModelFactory              = uploadPictures_DetectItemModelFactory;
            this.uploadPictures_OrderBySizeModelFactory             = uploadPictures_OrderBySizeModelFactory;

            this.httpContextBase      = httpContextBase;
            this.fileSystemUtilities = fileSystemUtilities;
        }

        private readonly IFileSystemUtilities fileSystemUtilities;

        private readonly Func<UploadPictures_SimpleTasksWithPictures> uploadPictures_SimpleTasksWithPicturesModelFactory;
        private readonly Func<UploadPictures_DetectColorsModel>       uploadPictures_DetectColorsModelFactory;
        private readonly Func<UploadPictures_DetectItemModel>         uploadPictures_DetectItemModelFactory;
        private readonly Func<UploadPictures_OrderBySizeModel>        uploadPictures_OrderBySizeModelFactory;

        private readonly HttpContextBase httpContextBase;
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
                model.ErrorMessage = TaskWizard.UploadPicture_ValidationMessage;
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
                model.ErrorMessage = TaskWizard.UploadPicture_ValidationMessage;
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
                model.ErrorMessage = TaskWizard.UploadPicture_ValidationMessage;
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
            if (picture == null || !picture.ContentType.Contains("image"))
            {
                model.ErrorMessage = TaskWizard.UploadPicture_ValidationMessage;
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
                String.Format(TaskWizard.UploadPicture_QuantityValidationMessage_Uploaded 
                              + " {0} "  
                              + TaskWizard.UploadPicture_QuantityValidationMessage_HaveToUpload
                              + " {1}.", numberOfPicturesUploaded, taskDifficulty);
        }
    }
}
