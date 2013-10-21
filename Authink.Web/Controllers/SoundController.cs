using System;
using System.Web;
using System.Web.Mvc;

using Authink.Core.Model.Commands;
using Authink.Core.Model.Services;
using Authink.Web.Models.Sound;

using buru = Authink.Core.Domain.Rules;
using core = Authink.Core.Domain.Entities;

namespace Authink.Web.Controllers
{
    public class SoundController : Controller
    {
        public SoundController
        (
            ISoundCommands       soundCommands,
            ISoundServices       soundServices,
            IFileSystemUtilities fileSystemUtilities,
            IUserAccessRights    userAccessRights,

            Func<CreateModel> createModelFactory,
            Func<EditModel>   editModelFactory 
        )
        {
            this.soundCommands       = soundCommands;
            this.soundServices       = soundServices;
            this.fileSystemUtilities = fileSystemUtilities;
            this.userAccessRights    = userAccessRights;

            this.createModelFactory = createModelFactory;
            this.editModelFactory   = editModelFactory;
        }

        private readonly ISoundCommands       soundCommands;
        private readonly ISoundServices       soundServices;
        private readonly IFileSystemUtilities fileSystemUtilities;
        private readonly IUserAccessRights    userAccessRights;

        private readonly Func<CreateModel> createModelFactory;
        private readonly Func<EditModel>   editModelFactory;

        [HttpGet]  public ActionResult Create(int taskId)
        {
            if(!userAccessRights.CanEditTask(taskId))
            {
                return new HttpNotFoundResult();
            }

            var model    = createModelFactory();
            model.TaskId = taskId;

            return PartialView
            (
                viewName:"_Create",
                model:model
            );
        }
        [HttpPost] public ActionResult Create(CreateModel model, HttpPostedFileBase sound)
        {
            if(sound == null || !sound.ContentType.Contains("audio"))
            {
                model.ErrorMessage = "File missing or wrong format";
                return PartialView
                (
                    viewName:"_Create",
                    model:model
                );
            }

            var soundUrl = soundServices.SaveToFileSystem
            (
                soundName:    sound.FileName,
                soundContent: fileSystemUtilities.Transform_HttpPostedFileBase_Into_Bytes(sound),
                relatedId:    model.TaskId,
                baseSavePath: buru::Sound.VoiceCommands.DefaultSavePath
            );

            var soundId = soundCommands.Create(soundUrl, sound.FileName, "ins");

            soundCommands.AttachSoundToTask(soundId, model.TaskId);
            return RedirectToRoute("Home");
        }

        [HttpGet]  public ActionResult Edit(int soundId)
        {
            if(!userAccessRights.CanEditSound(soundId))
            {
                return new HttpNotFoundResult();
            }

            var model     = editModelFactory();
            model.SoundId = soundId;

            return PartialView
            (
                viewName:"_Edit",
                model:model
            );
        }
        [HttpPost] public ActionResult Edit(EditModel model, HttpPostedFileBase sound)
        {
            if(sound == null || !sound.ContentType.Contains("audio"))
            {
                return PartialView
                (
                    viewName:"_Edit",
                    model:model
                );
            }

            var soundContent = fileSystemUtilities.Transform_HttpPostedFileBase_Into_Bytes(sound);

            soundServices.SaveToFileSystem(soundContent, model.Sound.Url);

            return RedirectToRoute("Home");
        }
    }
}
