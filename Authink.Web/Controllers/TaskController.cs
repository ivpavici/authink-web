using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

using Authink.Core.Model.Commands;
using Authink.Core.Model.Services;
using Authink.Web.Controllers.Task.Models;
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
            
            Func<ChooseTypeModel>       chooseTypeModelFactory,
            Func<ChooseDifficultyModel> chooseDifficultyModelFactory,
            Func<InputMetaDataModel>    inputMetaDataModelFactory,
            Func<FeedbackModel>         feedbackModelFactory,

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

            this.chooseTypeModelFactory       = chooseTypeModelFactory;
            this.chooseDifficultyModelFactory = chooseDifficultyModelFactory;
            this.inputMetaDataModelFactory    = inputMetaDataModelFactory;
            this.feedbackModelFactory         = feedbackModelFactory;

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
        private readonly Func<FeedbackModel>         feedbackModelFactory;

        private HttpContextBase httpContextBase;

        private static IDictionary<string, KnownTaskTypes> KnownTaskTypesMappings = new Dictionary<string, KnownTaskTypes>()
        {
            { buru::Task.Keys.VoiceCommands,        new KnownTaskTypes("Voice commands",            "voice-commands",         new List<int>{2,3,4,5,    }, "UploadPictures_DetectItem"              ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.VoiceCommands]       , "'Voice commands' lets you create a task by uploading four different pictures (one correct and three false) with one voice command that states the objective of the task.")},
            { buru::Task.Keys.DetectColors,         new KnownTaskTypes("Detect colors",             "detect-colors",          new List<int>{1,2,3,4,5,6 }, "UploadPictures_DetectColors"            ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.DetectColors]        , "'Detect colors' lets you create a task first by choosing colored pictures and then correct and incorrect colors for those pictures.")},
            { buru::Task.Keys.DetectDifferentItems, new KnownTaskTypes("Detect different items",    "detect-different-items", new List<int>{3,4,5,      }, "UploadPictures_SimpleTasksWithPictures" ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.DetectDifferentItems], "'Detect different items' lets you upload images that will shuffle through a few lines and create a task in which a child must choose a different image from a set of images.")},
            { buru::Task.Keys.PairSameItems,        new KnownTaskTypes("Pair same items",           "pair-same-items",        new List<int>{3,5,6       }, "UploadPictures_SimpleTasksWithPictures" ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.PairSameItems]       , "'Pair same items' lets you upload images and when the task is created the images will become settled in two lists. The objective is to pair items from one list to another.")},
            { buru::Task.Keys.ContinueSequence,     new KnownTaskTypes("Continue sequence of items", "continue-squence",      new List<int>{2,3,4       }, "UploadPictures_SimpleTasksWithPictures" ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.ContinueSequence]    , "'Continue sequence' lets you upload images after which a task will be generated with a sequence of items that will shuffle the correct and false images.")},
            { buru::Task.Keys.OrderBySize,          new KnownTaskTypes("Order items by size",        "order-by-size",         new List<int>{2,3,4,5,6   }, "UploadPictures_OrderBySize"             ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.OrderBySize]         , "'Order by size' lets you upload one picture. After that, that picture will be resized and the objective is to order the items by size.")},
            { buru::Task.Keys.PairHalves,           new KnownTaskTypes("Pair halves",                "pair-halves",           new List<int>{2,3,4,5,6   }, "UploadPictures_SimpleTasksWithPictures" ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.PairHalves]          , "'Pair halves' lets you upload images, after which they will be cut in half and the objective is to pair those halves.")}
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

            httpContextBase.Session["TestId"] = testId;

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
                model.TaskKey == buru::Task.Keys.VoiceCommands    || model.TaskKey == buru::Task.Keys.DetectDifferentItems ||  model.TaskKey == buru::Task.Keys.PairSameItems ||
                model.TaskKey == buru::Task.Keys.ContinueSequence || model.TaskKey == buru::Task.Keys.PairHalves
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
                    taskKey:     model.TaskKey,
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

            ResetWizardSessionState();
            return View
            (
                model:model.Test.Id
            );
        }

        [HttpGet] public ActionResult Cancel()
        {
            ResetWizardSessionState();

            return RedirectToRoutePermanent("Shell");
        }
        private void ResetWizardSessionState()
        {
            httpContextBase.Session["TestId"]            = null;
            httpContextBase.Session["Colors"]            = null;
            httpContextBase.Session["TaskKey"]           = null;
            httpContextBase.Session["Pictures"]          = null;
            httpContextBase.Session["TaskDifficulty"]    = null;
            httpContextBase.Session["ImplementedTaskId"] = null;
        }
    }
    public partial class TaskController
    {
        private int  CreateTask_SimpleTasksWithPictures(string taskKey, string title, string description, int userId,int testId)
        {
            if (httpContextBase.Session["Pictures"] == null || httpContextBase.Session["TaskDifficulty"] == null)
            {
                return 0;
            }

            var taskDifficulty = Convert.ToInt32(httpContextBase.Session["TaskDifficulty"]);
            var pictures       = httpContextBase.Session["Pictures"] as List<SessionPictureData>;

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

            httpContextBase.Session["ImplementedTaskId"] = taskId;
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
            if (httpContextBase.Session["Pictures"] == null || httpContextBase.Session["Colors"] == null || httpContextBase.Session["TaskDifficulty"] == null)
            {
                return 0;
            }

            var taskDifficulty = Convert.ToInt32(HttpContext.Session["TaskDifficulty"]);
            var pictures       = httpContextBase.Session["Pictures"] as List<SessionPictureData>;
            var colors         = httpContextBase.Session["Colors"] as List<SessionColorData>;

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
            httpContextBase.Session["ImplementedTaskId"] = taskId;

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
            if (httpContextBase.Session["Pictures"] == null || httpContextBase.Session["TaskDifficulty"] == null)
            {
                return 0;
            }

            var taskDifficulty = Convert.ToInt32(httpContextBase.Session["TaskDifficulty"]);
            var picture        = httpContextBase.Session["Pictures"] as SessionPictureData;

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
            httpContextBase.Session["ImplementedTaskId"] = taskId;

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

        private void Create_And_Attach_Color_To_Picture  (int pictureId, string value, bool isCorrect      )
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
}

namespace Authink.Web.Controllers.Task.Models
{
    using Authink.Core.Model.Queries;
    using Authink.Web.Controllers.Task.Helpers;

    using ent = Authink.Core.Domain.Entities;

    public class ChooseDifficultyModel
    {
        public string TaskName { get; set; }
        public string UploadPicturesRouteName { get; set; }
        public List<int> AvailableTaskDifficulties { get; set; }
    }

    public class ChooseTypeModel
    {
        public IDictionary<string, KnownTaskTypes> TaskTypes { get; set; }
    }

    public class InputMetaDataModel
    {
        [Required(ErrorMessage = "You must fill out a title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "You must fill out a description")]
        public string Description { get; set; }

        public string TaskKey { get; set; }
        public bool IsVoiceCommandRequired { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class FeedbackModel
    {
        public FeedbackModel
        (
            ITestQueries testQueries
        )
        {
            this.testQueries = testQueries;
        }

        private readonly ITestQueries testQueries;

        public int TaskId { get; set; }

        public ent::Test.ShortDetails Test
        {
            get
            {
                if (_test == null)
                {
                    _test = new Lazy<ent::Test.ShortDetails>(() => testQueries.GetTest_thatContainsTask(this.TaskId));
                }
                return _test.Value;
            }
        }
        private Lazy<ent::Test.ShortDetails> _test;
    }
}

namespace Authink.Web.Controllers.Task.Helpers
{
    public class KnownTaskTypes 
    {
        public KnownTaskTypes
        (
            string       name,
            string       urlFriendyName,
            List<int>    availableDifficulties,
            string       uploadPicturesRouteName,
            string       pictureUrl,
            string       taskDescription
        )
        {
            this.Name                    = name;
            this.UrlFriendyname          = urlFriendyName;
            this.AvailableDifficulties   = availableDifficulties;
            this.UploadPicturesRouteName = uploadPicturesRouteName;
            this.PictureUrl              = pictureUrl;
            this.TaskDescription         = taskDescription;
        }

        public string       Name                     { get; private set; }
        public string       UrlFriendyname           { get; private set; }
        public List<int>    AvailableDifficulties    { get; private set; }
        public string       UploadPicturesRouteName  { get; private set; }
        public string       PictureUrl               { get; private set; }
        public string       TaskDescription          { get; private set; }
    }
}
