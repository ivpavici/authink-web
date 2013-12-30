using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Authink.Core.Model.Commands;
using Authink.Core.Model.Services;
using Authink.Data.ResourceFileStorage;
using Authink.Web.Controllers.Task.Models;
using Authink.Web.Models.Session;

using ent = Authink.Core.Domain.Entities;
using buru = Authink.Core.Domain.Rules;

namespace Authink.Web.Controllers
{
    using Authink.Web.Controllers.Task.Helpers;
    using Authink.Core.Fx;
    using NLog;

    [Authorize]
    public partial class TaskController: Controller
    {
        public TaskController
        (
            ITaskCommands        taskCommands,
            IPictureCommands     pictureCommands,
            ISoundCommands       soundCommands,
            IPictureServices     pictureServices,
            ILoginServices       loginServices,
            IFileStorageAdapter  fileStorageAdapter,
            IUserAccessRights    userAccessRights,
            
            Func<ChooseTypeModel>       chooseTypeModelFactory,
            Func<ChooseDifficultyModel> chooseDifficultyModelFactory,
            Func<InputMetaDataModel>    inputMetaDataModelFactory,
            Func<FeedbackModel>         feedbackModelFactory,

            HttpContextBase   httpContextBase,
            Logger            logger
        )
        {
            this.taskCommands        = taskCommands;
            this.pictureCommands     = pictureCommands;
            this.fileStorageAdapter  = fileStorageAdapter;
            this.pictureServices     = pictureServices;
            this.loginServices       = loginServices;
            this.userAccessRights    = userAccessRights;
            this.soundCommands       = soundCommands;
            this.chooseTypeModelFactory       = chooseTypeModelFactory;
            this.chooseDifficultyModelFactory = chooseDifficultyModelFactory;
            this.inputMetaDataModelFactory    = inputMetaDataModelFactory;
            this.feedbackModelFactory         = feedbackModelFactory;

            this.httpContextBase = httpContextBase;
            this.logger          = logger;
        }

        private readonly ITaskCommands        taskCommands;
        private readonly IPictureCommands     pictureCommands;
        private readonly IFileStorageAdapter  fileStorageAdapter;
        private readonly IPictureServices     pictureServices;
        private readonly ILoginServices       loginServices;
        private readonly ISoundCommands       soundCommands;
        private readonly IUserAccessRights    userAccessRights;

        private readonly Func<ChooseTypeModel>       chooseTypeModelFactory;
        private readonly Func<ChooseDifficultyModel> chooseDifficultyModelFactory;
        private readonly Func<InputMetaDataModel>    inputMetaDataModelFactory;
        private readonly Func<FeedbackModel>         feedbackModelFactory;

        private HttpContextBase httpContextBase;
        private Logger          logger;

        private static IDictionary<string, KnownTaskTypes> KnownTaskTypesMappings = new Dictionary<string, KnownTaskTypes>()
        {
            { buru::Task.Keys.VoiceCommands,        new KnownTaskTypes("Voice commands",            "voice-commands",         new List<int>{2,3,4,5,    }, "UploadPictures_DetectItem"              ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.VoiceCommands])},
            { buru::Task.Keys.DetectColors,         new KnownTaskTypes("Detect colors",             "detect-colors",          new List<int>{1,2,3,4,5,6 }, "UploadPictures_DetectColors"            ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.DetectColors])},
            { buru::Task.Keys.DetectDifferentItems, new KnownTaskTypes("Detect different items",    "detect-different-items", new List<int>{3,4,5,      }, "UploadPictures_SimpleTasksWithPictures" ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.DetectDifferentItems])},
            { buru::Task.Keys.PairSameItems,        new KnownTaskTypes("Pair same items",           "pair-same-items",        new List<int>{3,5,6       }, "UploadPictures_SimpleTasksWithPictures" ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.PairSameItems])},
            { buru::Task.Keys.ContinueSequence,     new KnownTaskTypes("Continue sequence of items", "continue-squence",      new List<int>{2,3,4       }, "UploadPictures_SimpleTasksWithPictures" ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.ContinueSequence])},
            { buru::Task.Keys.OrderBySize,          new KnownTaskTypes("Order items by size",        "order-by-size",         new List<int>{2,3,4,5,6   }, "UploadPictures_OrderBySize"             ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.OrderBySize])},
            { buru::Task.Keys.PairHalves,           new KnownTaskTypes("Pair halves",                "pair-halves",           new List<int>{2,3,4,5,6   }, "UploadPictures_SimpleTasksWithPictures" ,buru::Task.AvailableTaskTypesDefaultPictures[buru::Task.Keys.PairHalves])}
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

            logger.Info("User {0} requested choose type in task wizard", httpContextBase.User.Identity.Name);

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
            logger.Info("User {0} requested choose difficulty for task type {1}", httpContextBase.User.Identity.Name, taskType);

            var model = chooseDifficultyModelFactory();

            var taskKey = KnownTaskTypesMappings.Single(pair => pair.Value.UrlFriendyname == taskType)
                                                .Key;
                            
            model.TaskName                  = KnownTaskTypesMappings[taskKey].Name;
            model.AvailableTaskDifficulties = KnownTaskTypesMappings[taskKey].AvailableDifficulties;
            model.UploadPicturesRouteName   = KnownTaskTypesMappings[taskKey].UploadPicturesRouteName;

            httpContextBase.Session["TaskKey"] = taskKey;

            logger.Info("Formed model for choose difficulty: task name = {0}", model.TaskName);

            return View
            (
                model:model
            );
        }
        [HttpGet ] public ActionResult InputMetaData   (                        )
        {
            if (httpContextBase.Session["TaskKey"] == null)
            {
                logger.Warn("TaskKey session variable expired for user {0} ", httpContextBase.User.Identity.Name);
                return RedirectToRoute("Shell");
            }

            logger.Info("User {0} requested general task data inputs", httpContextBase.User.Identity.Name);

            var model = inputMetaDataModelFactory();

            model.TaskKey                = httpContextBase.Session["TaskKey"].ToString();
            model.IsVoiceCommandRequired = model.TaskKey == buru::Task.Keys.VoiceCommands;

            logger.Info("Formed model for choose difficulty: task key = {0} and voiceCommandRequired = {1}", model.TaskKey, model.IsVoiceCommandRequired);
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
                logger.Warn("TestId session variable expired for user {0}", httpContextBase.User.Identity.Name);
                return RedirectToRoute("Shell");
            }

            if(sound == null && model.IsVoiceCommandRequired)
            {
                logger.Info("Voice command is required but didn't get one here.");

                model.ErrorMessage = "Voice command is required for this type of task";
                return View(model);
            }

            if(sound != null && !sound.ContentType.Contains("audio"))
            {
                return View(model);
            }

            var testId    = Convert.ToInt32(httpContextBase.Session["TestId"]);
            var newTaskId = 0;

            try
            {
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
            }
            catch (Exception ex)
            {
                logger.Error("Creation of task failed for user {0}",httpContextBase.User.Identity.Name, ex);
                return View(model);
            }

            try
            {
                if(sound != null && newTaskId != 0)
                {
                    var soundUrl = fileStorageAdapter.Upload
                    (
                        resourceFile:   new ResourceFileStorageAdapter.ResourceFile(sound.FileName, FileHelpers.Transform_HttpPostedFileBase_Into_Bytes(sound)),
                        baseFoldername: buru::Sound.VoiceCommands.DefaultSavePath 
                    );
                   
                    var soundId = soundCommands.Create(soundUrl.ToString(), sound.FileName, "ins");

                    soundCommands.AttachSoundToTask(soundId, newTaskId);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Creation of sound failed for user {0}", httpContextBase.User.Identity.Name, ex);
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
                logger.Warn("ImplementedTaskId session variable expired for user {0}", httpContextBase.User.Identity.Name);
                return RedirectToRoute("Shell");
            }

            var model    = feedbackModelFactory();
            var taskId   = Convert.ToInt32(HttpContext.Session["ImplementedTaskId"]);
            model.TaskId = taskId;

            logger.Info("{0} successfully created task with Id = {1} ", httpContextBase.User.Identity.Name, taskId);

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
                logger.Warn("Simple task with pictures creation: Pictures or TaskDifficulty session variables expired for user {0}", httpContextBase.User.Identity.Name);
                return 0;
            }

            logger.Info("Creating simple task with pictures. Parameters: taskKey = {0},title = {1},description = {2}, userId = {3}, testId = {4}",taskKey, title, description, userId, testId );

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
                try
                {
                    logger.Info("Uploading picture. Parameters: fileName = {0}, content = {1}, taskId = {2}", pictureData.FileName, pictureData.Content, taskId);

                    var resizedPicture = pictureServices.ResizePicture(pictureData.Content, buru::Picture.Task.DefaultResizeQuerystring);

                    var savePath = fileStorageAdapter.Upload
                    (
                        resourceFile:   new ResourceFileStorageAdapter.ResourceFile(pictureData.FileName, resizedPicture),
                        baseFoldername: buru::Picture.Task.DefaultSavePath
                    );

                    var pictureId = pictureCommands.Create
                    (
                        url:      savePath.ToString(),
                        theme:    "",
                        isAnswer: pictureData.IsAnswer
                    );

                    pictureCommands.AttachToTask(taskId, pictureId);
                }
                catch (Exception ex)
                {
                    logger.Error("Failed to upload picture", ex);
                    throw;
                }
            }

            taskCommands.ToggleAttachToTest(taskId,testId);

            return taskId;
        }
        private int  CreateTask_DetectColors           (string taskKey, string title, string description, int userId,int testId)
        {
            if (httpContextBase.Session["Pictures"] == null || httpContextBase.Session["Colors"] == null || httpContextBase.Session["TaskDifficulty"] == null)
            {
                logger.Warn("Detect colors task creation: Pictures, Colors or TaskDifficulty session variables expired for user {0}", httpContextBase.User.Identity.Name);
                return 0;
            }

            logger.Info("Creating task with colors. Parameters: taskKey = {0},title = {1},description = {2}, userId = {3}, testId = {4}", taskKey, title, description, userId, testId);

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
                try
                {
                    logger.Info("Uploading picture. Parameters: fileName = {0}, content = {1}, taskId = {2}", pictureData.FileName, pictureData.Content, taskId);

                    var resizedPicture = pictureServices.ResizePicture(pictureData.Content, buru::Picture.Task.DefaultResizeQuerystring);

                    var savePath = fileStorageAdapter.Upload
                    (
                        resourceFile:   new ResourceFileStorageAdapter.ResourceFile(pictureData.FileName, resizedPicture),
                        baseFoldername: buru::Picture.Task.DefaultSavePath
                    );

                    var picturePosition      = pictures.IndexOf(pictureData);
                    var colorsForThisPicture = colors[picturePosition];

                    var pictureId = pictureCommands.Create
                    (
                        url:      savePath.ToString(),
                        theme:    "",
                        isAnswer: null
                    );

                    pictureCommands.AttachToTask (taskId,pictureId);

                    try
                    {
                        logger.Info("Uploaded picture. Attaching colors to picture. Parameters: pictureId = {0}, correctColor = {1}", pictureId, colorsForThisPicture.CorrectColor);

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
                    catch (Exception ex)
                    {
                        logger.Error("Failed to attach colors to picture", ex);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Failed to upload pictures and attach colors", ex);
                    throw;
                }
            }

            taskCommands.ToggleAttachToTest(taskId, testId);

            return taskId;
        }
        private int  CreateTask_OrderBySize            (string taskKey, string title, string description, int userId,int testId)
        {
            if (httpContextBase.Session["Pictures"] == null || httpContextBase.Session["TaskDifficulty"] == null)
            {
                logger.Warn("Order by size task creation: Picture or TaskDifficulty session variables expired for user {0} ", httpContextBase.User.Identity.Name);
                return 0;
            }

            logger.Info("Creating task order by size. Parameters: taskKey = {0},title = {1},description = {2}, userId = {3}, testId = {4}", taskKey, title, description, userId, testId);

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

            try
            {
                logger.Info("Uploading picture. Parameters: fileName = {0}, content = {1}, taskId = {2}", picture.FileName, picture.Content, taskId);

                var resizedPicture = pictureServices.ResizePicture(picture.Content, buru::Picture.Task.DefaultResizeQuerystring);

                var savePath = fileStorageAdapter.Upload
                (
                    resourceFile:   new ResourceFileStorageAdapter.ResourceFile(picture.FileName, resizedPicture),
                    baseFoldername: buru::Picture.Task.DefaultSavePath
                );

                var pictureId = pictureCommands.Create
                (
                    url:      savePath.ToString(),
                    theme:    "",
                    isAnswer: picture.IsAnswer
                );

                pictureCommands.AttachToTask(taskId, pictureId);
            }
            catch (Exception ex)
            {
                logger.Error("Failed to upload pictures", ex);
                throw;
            }
           
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
