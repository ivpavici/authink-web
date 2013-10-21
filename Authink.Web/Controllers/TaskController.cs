using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

using Authink.Core.Model.Commands;
using Authink.Core.Model.Services;
using Authink.Web.Models.Task;
using Authink.Web.Models.Session;

using ent = Authink.Core.Domain.Entities;
using buru = Authink.Core.Domain.Rules;

namespace Authink.Web.Controllers
{
    using Authink.Web.Controllers.Task.Helpers;

    [Authorize]
    public partial class TaskController: Controller
    {
        public TaskController
        (
            ITaskCommands        taskCommands,
            IPictureCommands     pictureCommands,
            ISoundCommands       soundCommands,
            IPictureServices     pictureServices,
            IFileSystemUtilities fileSystemUtilities,
            ILoginServices       loginServices,
            ISoundServices       soundServices,
            IUserAccessRights    userAccessRights,
            
            Func<ChooseTypeModel>                        chooseTypeModelFactory,
            Func<ChooseDifficultyModel>                  chooseDifficultyModelFactory,
            Func<InputMetaDataModel>                     inputMetaDataModelFactory,
            Func<DeleteModel>                            deleteModelFactory,
            Func<ReadModel>                              readModelFactory,
            Func<FeedbackModel>                          feedbackModelFactory,

            Func<EditModel>                        editModelFactory,
            Func<EditSimpleTasksWithPicturesModel> editSimpleTasksWithPicturesModelFactory, 
            Func<EditDetectColorsModel>            editDetectColorsModelFactory,
            Func<EditDetectItemModel>              editDetectItemModelFactory,

            HttpContextBase   httpContextBase
        )
        {
            this.taskCommands        = taskCommands;
            this.pictureCommands     = pictureCommands;
            this.soundCommands       = soundCommands;
            this.pictureServices     = pictureServices;
            this.fileSystemUtilities = fileSystemUtilities;
            this.loginServices       = loginServices;
            this.soundServices       = soundServices;
            this.userAccessRights    = userAccessRights;

            this.chooseTypeModelFactory                             = chooseTypeModelFactory;
            this.chooseDifficultyModelFactory                       = chooseDifficultyModelFactory;
            this.inputMetaDataModelFactory                          = inputMetaDataModelFactory;
            this.deleteModelFactory                                 = deleteModelFactory;
            this.readModelFactory                                   = readModelFactory;
            this.feedbackModelFactory                               = feedbackModelFactory;

            this.editModelFactory                        = editModelFactory;
            this.editSimpleTasksWithPicturesModelFactory = editSimpleTasksWithPicturesModelFactory;
            this.editDetectColorsModelFactory            = editDetectColorsModelFactory;
            this.editDetectItemModelFactory              = editDetectItemModelFactory;

            this.httpContextBase = httpContextBase;
        }

        private readonly ITaskCommands        taskCommands;
        private readonly IPictureCommands     pictureCommands;
        private readonly ISoundCommands       soundCommands;
        private readonly IPictureServices     pictureServices;
        private readonly IFileSystemUtilities fileSystemUtilities;
        private readonly ILoginServices       loginServices;
        private readonly ISoundServices       soundServices;
        private readonly IUserAccessRights    userAccessRights;

        private readonly Func<ChooseTypeModel>       chooseTypeModelFactory;
        private readonly Func<ChooseDifficultyModel> chooseDifficultyModelFactory;
        private readonly Func<InputMetaDataModel>    inputMetaDataModelFactory;
        private readonly Func<DeleteModel>           deleteModelFactory;
        private readonly Func<ReadModel>             readModelFactory;
        private readonly Func<FeedbackModel>         feedbackModelFactory;

        private readonly Func<EditModel>                        editModelFactory;
        private readonly Func<EditSimpleTasksWithPicturesModel> editSimpleTasksWithPicturesModelFactory;
        private readonly Func<EditDetectColorsModel>            editDetectColorsModelFactory;
        private readonly Func<EditDetectItemModel>              editDetectItemModelFactory;

        private HttpContextBase httpContextBase;

        private static IDictionary<string, KnownTaskTypes> KnownTaskTypesMappings = new Dictionary<string, KnownTaskTypes>()
        {
            { buru::Task.Keys.VoiceCommands,        new KnownTaskTypes("Voice commands",            "voice-commands",         new List<int>{2,3,4,5,    }, "UploadPictures_DetectItem"              ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.VoiceCommands])},
            { buru::Task.Keys.DetectColors,         new KnownTaskTypes("Detect colors",             "detect-colors",          new List<int>{1,2,3,4,5,6 }, "UploadPictures_DetectColors"            ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.DetectColors])},
            { buru::Task.Keys.DetectDifferentItems, new KnownTaskTypes("Detect different items",    "detect-different-items", new List<int>{3,4,5,      }, "UploadPictures_SimpleTasksWithPictures" ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.DetectDifferentItems])},
            { buru::Task.Keys.PairSameItems,        new KnownTaskTypes("Pair same items",           "pair-same-items",        new List<int>{3,5,6       }, "UploadPictures_SimpleTasksWithPictures" ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.PairSameItems])},
            { buru::Task.Keys.ContinueSequence,     new KnownTaskTypes("Continue sequence of items", "continue-squence",      new List<int>{2,3,4       }, "UploadPictures_SimpleTasksWithPictures" ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.ContinueSequence])},
            { buru::Task.Keys.OrderBySize,          new KnownTaskTypes("Order items by size",        "order-by-size",         new List<int>{2,3,4,5,6   }, "UploadPictures_OrderBySize"             ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.OrderBySize])}
        };
        private static IDictionary<string, string> KnownTaskEditMappings = new Dictionary<string, string>()
        {
            { buru::Task.Keys.ContinueSequence, "Edit_SimpleTasksWithPictures" }, { buru::Task.Keys.DetectColors,  "Edit_DetectColors"            }, { buru::Task.Keys.DetectDifferentItems, "Edit_SimpleTasksWithPictures" },
            { buru::Task.Keys.OrderBySize,      "Edit_SimpleTasksWithPictures" }, { buru::Task.Keys.PairSameItems, "Edit_SimpleTasksWithPictures" }, { buru::Task.Keys.VoiceCommands,        "Edit_DetectItem"              }
        };
    }
    public partial class TaskController
    {
        [HttpGet]  public ActionResult ChooseType      (int testId              )
        {
            if (!userAccessRights.CanCreateTask(testId))
            {
                return new HttpNotFoundResult();
            }

            var model       = chooseTypeModelFactory();
            model.TaskTypes = KnownTaskTypesMappings;

            HttpContext.Session["TestId"] = testId;

            return View
            (
                model: model
            );
        }
        [HttpGet]  public ActionResult ChooseDifficulty(string taskType         )
        {
            var model = chooseDifficultyModelFactory();

            var taskKey = KnownTaskTypesMappings.Single(pair => pair.Value.UrlFriendyname == taskType)
                                                .Key;
                            
            model.TaskName                  = KnownTaskTypesMappings[taskKey].Name;
            model.AvailableTaskDifficulties = KnownTaskTypesMappings[taskKey].AvailableDifficulties;
            model.UploadPicturesRouteName   = KnownTaskTypesMappings[taskKey].UploadPicturesRouteName;

            httpContextBase.Session["TaskKey"] = taskKey;

            return View
            (
                model:model
            );
        }
        [HttpGet ] public ActionResult InputMetaData   (                        )
        {
            if (httpContextBase.Session["TaskKey"] == null)
            {
                return new HttpNotFoundResult();
            }

            var model = inputMetaDataModelFactory();

            model.TaskKey                = httpContextBase.Session["TaskKey"].ToString();
            model.IsVoiceCommandRequired = model.TaskKey == buru::Task.Keys.VoiceCommands;

            return View
            (
                model:model
            );
        }
        [HttpPost] public ActionResult InputMetaData   (InputMetaDataModel model, HttpPostedFileBase sound)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            if(httpContextBase.Session["TestId"] == null)
            {
                return new HttpNotFoundResult();
            }

            if(sound == null && model.IsVoiceCommandRequired)
            {
                model.ErrorMessage = "Voice command is required for this type of task";
                return View(model);
            }

            if(sound != null && !sound.ContentType.Contains("audio"))
            {
                return View(model);
            }

            var testId    = Convert.ToInt32(httpContextBase.Session["TestId"]);
            var newTaskId = 0;

            if 
            (
                model.TaskKey == buru::Task.Keys.VoiceCommands    ||  model.TaskKey == buru::Task.Keys.DetectDifferentItems ||  model.TaskKey == buru::Task.Keys.PairSameItems ||
                model.TaskKey == buru::Task.Keys.ContinueSequence
            ) 
            {
                newTaskId = CreateTask_SimpleTasksWithPictures
                (
                    taskKey:     model.TaskKey,
                    title:       model.Title,
                    description: model.Description,
                    userId:      loginServices.GetSignedInUser().Id,
                    testId:      testId
                );
            }else if( model.TaskKey == buru::Task.Keys.DetectColors)
            {
                newTaskId = CreateTask_DetectColors
                (
                    taskKey:     model.TaskKey,
                    title:       model.Title,
                    description: model.Description,
                    userId:      loginServices.GetSignedInUser().Id,
                    testId:      testId
                );
            }else if ( model.TaskKey==buru::Task.Keys.OrderBySize)
            {
                newTaskId = CreateTask_OrderBySize
                (
                    taskKey:      model.TaskKey,
                    title:       model.Title,
                    description: model.Description,
                    userId:      loginServices.GetSignedInUser().Id,
                    testId:      testId
                );
            }

            if(sound != null && newTaskId != 0)
            {
                var soundUrl = soundServices.SaveToFileSystem
                (
                    soundName:    sound.FileName,
                    soundContent: fileSystemUtilities.Transform_HttpPostedFileBase_Into_Bytes(sound),
                    relatedId:    newTaskId,
                    baseSavePath: buru::Sound.VoiceCommands.DefaultSavePath
                );

                var soundId = soundCommands.Create(soundUrl, sound.FileName, "ins");

                soundCommands.AttachSoundToTask(soundId, newTaskId);
            }

            return RedirectToRoute
            (
                routeName:"Feedback"
            );
        }
        [HttpGet ] public ActionResult Feedback        (                        )
        {
            if(HttpContext.Session["ImplementedTaskId"]==null)
            {
                return new HttpNotFoundResult();
            }

            var model    = feedbackModelFactory();
            var taskId   = Convert.ToInt32(HttpContext.Session["ImplementedTaskId"]);
            model.TaskId = taskId;

            HttpContext.Session["ImplementedTaskId"] = null;
            return View
            (
                model:model.Test.Id
            );
        }
    }
    public partial class TaskController
    {
        [HttpGet]
        public ActionResult Edit(string taskId)
        {
            if (!Request.IsAjaxRequest())
            {
                return new HttpNotFoundResult();
            }

            var model = editModelFactory();
            model.TaskId = taskId;

            return RedirectToRoute
            (
                routeName: KnownTaskEditMappings[model.Task.Type],
                routeValues: new { taskId = Convert.ToInt32(taskId) }
            );
        }

        [HttpGet]
        public ActionResult Edit_SimpleTasksWithPictures(int taskId)
        {
            if (!userAccessRights.CanEditTask(taskId))
            {
                return new HttpNotFoundResult();
            }

            var model = editSimpleTasksWithPicturesModelFactory();
            model.Id = taskId;

            model.Name = model.Task.Name;
            model.Description = model.Task.Description;

            return PartialView
            (
                model: model,
                viewName: "_Edit_SimpleTasksWithPictures"
            );
        }
        [HttpPost]
        public ActionResult Edit_SimpleTasksWithPictures(EditSimpleTasksWithPicturesModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView
                (
                    model: model,
                    viewName: "_Edit_SimpleTasksWithPictures"
                );
            }

            taskCommands.Update
            (
                id: model.Id,
                description: model.Description,
                name: model.Name
            );

            return JavaScript("location.reload()");
        }

        [HttpGet]
        public ActionResult Edit_DetectColors(int taskId)
        {
            if (!userAccessRights.CanEditTask(taskId))
            {
                return new HttpNotFoundResult();
            }

            var model = editDetectColorsModelFactory();
            model.TaskId = taskId;
            model.Name = model.Task.Name;
            model.Description = model.Task.Description;

            var pas = model.PicturesWithColors.ToList();
            return PartialView
            (
                viewName: "_Edit_DetectColors",
                model: model
            );
        }
        [HttpPost]
        public ActionResult Edit_DetectColors(EditDetectColorsModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView
                (
                    model: model,
                    viewName: "_Edit_DetectColors"
                );
            }

            taskCommands.Update
            (
                id: model.TaskId,
                description: model.Description,
                name: model.Name
            );

            return JavaScript("location.reload()");
        }

        [HttpGet]
        public ActionResult Edit_DetectItem(int taskId)
        {
            if (!userAccessRights.CanEditTask(taskId))
            {
                return new HttpNotFoundResult();
            }

            var model = editDetectItemModelFactory();
            model.TaskId = taskId;
            model.Name = model.Task.Name;
            model.Description = model.Task.Description;

            return PartialView
            (
                model: model,
                viewName: "_Edit_DetectItem"
            );
        }
        [HttpPost]
        public ActionResult Edit_DetectItem(EditDetectItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView
                (
                    model: model,
                    viewName: "_Edit_DetectItem"
                );
            }

            taskCommands.Update
            (
                id: model.TaskId,
                description: model.Description,
                name: model.Name
            );

            return JavaScript("location.reload()");
        }
    }
    public partial class TaskController
    {
        private int  CreateTask_SimpleTasksWithPictures(string taskKey, string title, string description, int userId,int testId)
        {
            if(HttpContext.Session["Pictures"]==null || HttpContext.Session["TaskDifficulty"]==null)
            {
                return 0;
            }

            var taskDifficulty = Convert.ToInt32(HttpContext.Session["TaskDifficulty"]);
            var pictures       = HttpContext.Session["Pictures"] as List<SessionPictureData>;


            string soundSavePath="";
            var taskId = taskCommands.Create
            (
                description:       description,
                name:              title,
                userId:            userId,
                type:              taskKey,
                difficulty:        taskDifficulty,
                profilePictureUrl: buru::Task.AvailableTaskTypesDefaultPictures[taskKey],

                voiceCommand: null
            );

            HttpContext.Session["ImplementedTaskId"] = taskId;
            foreach (var pictureData in pictures)
            {
                var savePath  = pictureServices.SaveToFileSystem(pictureData.FileName, pictureData.Content, taskId, buru::Picture.Task.DefaultSavePath, buru::Picture.Task.DefaultResizeQuerystring);
                var pictureId = pictureCommands.Create
                (
                    url:      savePath,
                    theme:    "",
                    isAnswer: pictureData.IsAnswer
                );

                pictureCommands.AttachToTask(taskId, pictureId);
            }

            taskCommands.ToggleAttachToTest(taskId,testId);

            return taskId;
        }
        private int  CreateTask_DetectColors           (string taskKey, string title, string description, int userId,int testId)
        {
            if(HttpContext.Session["Pictures"] == null || HttpContext.Session["Colors"] == null || HttpContext.Session["TaskDifficulty"] == null)
            {
                return 0;
            }

            var taskDifficulty = Convert.ToInt32(HttpContext.Session["TaskDifficulty"]);
            var pictures       = HttpContext.Session["Pictures"] as List<SessionPictureData>;
            var colors         = HttpContext.Session["Colors"] as List<SessionColorData>;

            var taskId = taskCommands.Create
            (
                description:       description,
                name:              title,
                userId:            userId,
                type:              taskKey,
                difficulty:        taskDifficulty,
                profilePictureUrl: buru::Task.AvailableTaskTypesDefaultPictures[taskKey],
                voiceCommand:      null
            );
            HttpContext.Session["ImplementedTaskId"] = taskId;

            foreach (var pictureData in pictures)
            {
                var savePath             = pictureServices.SaveToFileSystem(pictureData.FileName, pictureData.Content, taskId, buru::Picture.Task.DefaultSavePath, buru::Picture.Task.DefaultResizeQuerystring);
                var picturePosition      = pictures.IndexOf(pictureData);
                var colorsForThisPicture = colors[picturePosition];

                var pictureId = pictureCommands.Create
                (
                    url:      savePath,
                    theme:    "",
                    isAnswer: null
                );

                pictureCommands.AttachToTask (taskId,pictureId);

                Create_And_Attach_Color_To_Picture
                (
                    pictureId: pictureId, 
                    value:     colorsForThisPicture.CorrectColor, 
                    isCorrect: true
                );

                foreach (var wrongColor in colorsForThisPicture.WrongColors)
                {
                    Create_And_Attach_Color_To_Picture
                    (
                        pictureId: pictureId,
                        value:     wrongColor,
                        isCorrect: false
                    );
                }
            }

            taskCommands.ToggleAttachToTest(taskId, testId);

            return taskId;
        }
        private int  CreateTask_OrderBySize            (string taskKey, string title, string description, int userId,int testId)
        {
            if (HttpContext.Session["Pictures"] == null || HttpContext.Session["TaskDifficulty"] == null)
            {
                return 0;
            }

            var taskDifficulty = Convert.ToInt32(HttpContext.Session["TaskDifficulty"]);
            var picture        = HttpContext.Session["Pictures"] as SessionPictureData;

            var taskId = taskCommands.Create
            (
                description:       description,
                name:              title,
                userId:            userId,
                type:              taskKey,
                difficulty:        taskDifficulty,
                profilePictureUrl: buru::Task.AvailableTaskTypesDefaultPictures[taskKey],

                voiceCommand: null
            );
            HttpContext.Session["ImplementedTaskId"] = taskId;

            var savePath  = pictureServices.SaveToFileSystem(picture.FileName, picture.Content, taskId, buru::Picture.Task.DefaultSavePath, buru::Picture.Task.DefaultResizeQuerystring);
            var pictureId = pictureCommands.Create
            (
                url:      savePath,
                theme:    "",
                isAnswer: picture.IsAnswer
            );

            pictureCommands.AttachToTask(taskId, pictureId);

            taskCommands.ToggleAttachToTest(taskId, testId);

            return taskId;
        }

        private void          Create_And_Attach_Color_To_Picture  (int pictureId, string value, bool isCorrect      )
        {
            var colorId = pictureCommands.Create_color
            (
                value:     value,
                pictureId: pictureId,
                isCorrect: isCorrect
            );
            
            pictureCommands.AttachColorToPicture(pictureId, colorId);
        }
    }

    public partial class TaskController
    {
        [HttpGet] public ActionResult        Read  (int taskId)
        {
            if (!userAccessRights.CanReadTask(taskId))
            {
                return new HttpNotFoundResult();
            }

            var model    = readModelFactory();
            model.TaskId = taskId;

            return PartialView(model);
        }
        [HttpGet] public HttpResponseMessage Delete(int taskId)
        {
            if (!userAccessRights.CanReadTask(taskId))
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            var model    = deleteModelFactory();
            model.TaskId = taskId;
            
            if(model.Task != null)
            {
                taskCommands.Delete(taskId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}

namespace Authink.Web.Controllers.Task.Helpers
{
    public class KnownTaskTypes 
    {
        public KnownTaskTypes
        (
            string    name,
            string    urlFriendyName,
            List<int> availableDifficulties,
            string    uploadPicturesRouteName,
            string    pictureUrl
        )
        {
            this.Name                    = name;
            this.UrlFriendyname          = urlFriendyName;
            this.AvailableDifficulties   = availableDifficulties;
            this.UploadPicturesRouteName = uploadPicturesRouteName;
            this.PictureUrl              = pictureUrl;
        }

        public string    Name                     { get; private set; }
        public string    UrlFriendyname           { get; private set; }
        public List<int> AvailableDifficulties    { get; private set; }
        public string    UploadPicturesRouteName  { get; private set; }
        public string    PictureUrl               { get; private set; }
    }
}
