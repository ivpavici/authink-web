using System;
using System.Web;
using System.Web.Mvc;

using Authink.Core.Model.Commands;
using Authink.Core.Model.Services;
using Authink.Data.ResourceFileStorage;
using Authink.Web.Models.Sound;

using buru = Authink.Core.Domain.Rules;
using core = Authink.Core.Domain.Entities;
using Authink.Core.Fx;

namespace Authink.Web.Controllers
{
    public class SoundController : Controller
    {
        public SoundController
        (
            ISoundCommands       soundCommands,
            IFileStorageAdapter  fileStorageAdapter,
            IUserAccessRights    userAccessRights,

            Func<CreateModel> createModelFactory,
            Func<EditModel>   editModelFactory 
        )
        {
            this.soundCommands      = soundCommands;
            this.userAccessRights   = userAccessRights;
            this.fileStorageAdapter = fileStorageAdapter;
            this.createModelFactory = createModelFactory;
            this.editModelFactory   = editModelFactory;
        }

        private readonly ISoundCommands       soundCommands;
        private readonly IFileStorageAdapter  fileStorageAdapter;
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

            var soundUrl =
                fileStorageAdapter.Upload(
                    new ResourceFileStorageAdapter.ResourceFile(sound.FileName,
                        FileHelpers.Transform_HttpPostedFileBase_Into_Bytes(sound)),
                    buru::Sound.VoiceCommands.DefaultSavePath);
            
            var soundId = soundCommands.Create(soundUrl.ToString(), sound.FileName, "ins");

            soundCommands.AttachSoundToTask(soundId, model.TaskId);
            return RedirectToRoute("Home");
        }
    }
}
