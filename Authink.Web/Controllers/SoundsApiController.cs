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
using Authink.Core.Fx;

namespace Authink.Web.Controllers
{
    public class SoundsApiController : ApiController
    {
        public SoundsApiController
        (
            IUserAccessRights userAccessRights,
            ISoundServices    soundServices,
            ISoundCommands    soundCommands,
            ISoundQueries     soundQueries
        )
        {
            this.userAccessRights = userAccessRights;
            this.soundServices    = soundServices;
            this.soundCommands    = soundCommands;
            this.soundQueries     = soundQueries;
        }

        private readonly IUserAccessRights userAccessRights;
        private readonly ISoundServices    soundServices;
        private readonly ISoundCommands    soundCommands;
        private readonly ISoundQueries     soundQueries;

        [System.Web.Http.HttpPost]
        public async Task<object> Task_InsertSoundForUpdate(int soundId, int taskId)
        {
            if (!userAccessRights.CanEditSound(soundId))
            {
                throw new UnauthorizedAccessException();
            }

            var memoryStreamProvider = new MultipartMemoryStreamProvider();

            await Request.Content.ReadAsMultipartAsync(memoryStreamProvider);

            var file     = memoryStreamProvider.Contents.First();
            var fileName = FileHelpers.CreateUniqueFileName(file.Headers.ContentDisposition.FileName.Trim('\"'));
            var fileData = await file.ReadAsByteArrayAsync();

            var savePath = soundServices.Save
            (
                soundName:    fileName,
                soundContent: fileData,
                relatedId:    taskId,
                baseSavePath: buru::Sound.VoiceCommands.DefaultSavePath
            );

            soundCommands.Update(soundId, savePath);

            return new { Url = savePath };
        }

        [System.Web.Http.HttpPost]
        public async Task<ent::Sound.Details> Task_InsertSound(int taskId)
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

            var savePath =  soundServices.Save
            (
                soundName:    fileName, 
                soundContent: fileData,
                relatedId:    taskId,
                baseSavePath: buru::Sound.VoiceCommands.DefaultSavePath
            );

            var soundId = soundCommands.Create(savePath, fileName, "ttl");
            soundCommands.AttachSoundToTask(soundId, taskId);
            
            return soundQueries.GetSingle_whereId(soundId);
        }
    }
}
