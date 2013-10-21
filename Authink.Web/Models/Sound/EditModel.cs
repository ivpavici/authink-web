using System;
using Authink.Core.Model.Queries;

namespace Authink.Web.Models.Sound
{
    public class EditModel
    {
        public EditModel
        (
            ISoundQueries soundQueries
        )
        {
            this.soundQueries = soundQueries;
        }

        private readonly ISoundQueries soundQueries;

        public int SoundId { get; set; }

        public Core.Domain.Entities.Sound.Details Sound
        {
            get
            {
                if (_sound == null)
                {
                    _sound = new Lazy<Core.Domain.Entities.Sound.Details>(() => soundQueries.GetSingle_whereId(this.SoundId));
                }
                return _sound.Value;
            }
        }
        private Lazy<Core.Domain.Entities.Sound.Details> _sound;
    }
}