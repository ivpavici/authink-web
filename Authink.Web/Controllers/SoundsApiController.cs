using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using Authink.Core.Model.Services;
using Authink.Core.Model.Commands;
using Authink.Core.Model.Queries;

using buru = Authink.Core.Domain.Rules;
using ent = Authink.Core.Domain.Entities;

namespace Authink.Web.Controllers
{
    public class SoundsApiController : ApiController
    {
        public SoundsApiController
        (
            IUserAccessRights    userAccessRights,
            IFileSystemUtilities fileSystemUtilities,
            ISoundCommands       soundCommands,
            ISoundQueries        soundQueries
        )
        {
            this.userAccessRights    = userAccessRights;
            this.fileSystemUtilities = fileSystemUtilities;
            this.soundCommands       = soundCommands;
            this.soundQueries        = soundQueries;
        }

        private readonly IUserAccessRights    userAccessRights;
        private readonly IFileSystemUtilities fileSystemUtilities;
        private readonly ISoundCommands       soundCommands;
        private readonly ISoundQueries        soundQueries;

        [System.Web.Http.HttpPost]
        public async Task<object> Task_InsertSoundForUpdate(int soundId, int taskId)
        {
            if (!userAccessRights.CanEditSound(soundId))
            {
                throw new UnauthorizedAccessException();
            }

            var soundSavePathRoot = HttpContext.Current.Server.MapPath("~/" + buru::Sound.VoiceCommands.DefaultSavePath);

            fileSystemUtilities.CreateFolderIfNecessary(taskId, buru::Sound.VoiceCommands.DefaultSavePath);

            var savePath           = soundSavePathRoot + taskId;
            var dataStreamProvider = new MyMultipartFormDataStreamProvider(savePath);

            await Request.Content.ReadAsMultipartAsync(dataStreamProvider);

            var file             = dataStreamProvider.FileData.First();
            var newSoundSavePath = file.LocalFileName.Substring(file.LocalFileName.IndexOf("Content", StringComparison.Ordinal)).Replace("\\", "/");

            soundCommands.Update(soundId, newSoundSavePath);

            return new { Url = newSoundSavePath };
        }

        [System.Web.Http.HttpPost]
        public async Task<ent::Sound.Details> Task_InsertSound(int taskId)
        {
            if (!userAccessRights.CanEditTask(taskId))
            {
                throw new UnauthorizedAccessException();
            }

            var soundSavePathRoot = HttpContext.Current.Server.MapPath("~/" + buru::Sound.VoiceCommands.DefaultSavePath);

            fileSystemUtilities.CreateFolderIfNecessary(taskId, buru::Sound.VoiceCommands.DefaultSavePath);

            var savePath           = soundSavePathRoot + taskId;
            var dataStreamProvider = new MyMultipartFormDataStreamProvider(savePath);

            await Request.Content.ReadAsMultipartAsync(dataStreamProvider);

            var file = dataStreamProvider.FileData.First();
            var newSoundSavePath = file.LocalFileName.Substring(file.LocalFileName.IndexOf("Content", StringComparison.Ordinal)).Replace("\\", "/");

            var soundId = soundCommands.Create(newSoundSavePath, file.LocalFileName, "ttl");
            soundCommands.AttachSoundToTask(soundId, taskId);

            return soundQueries.GetSingle_whereId(soundId);
        }
    }
}
