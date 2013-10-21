using System.Collections.Generic;

namespace Authink.Web.Models.Session
{
    public class SessionPictureData
    {
        public SessionPictureData
        (
            string filename,
            byte[] content,
            bool?  isAnswer
        )
        {
            this.FileName = filename;
            this.Content  = content;
            this.IsAnswer = isAnswer;
        }

        public string FileName { get; private set; }
        public byte[] Content  { get; private set; }
        public bool?  IsAnswer { get; private set; }
    }
    public class SessionColorData
    {
        public SessionColorData
        (
            string correctColor,
            List<string> wrongColors 
        )
        {
            this.CorrectColor = correctColor;
            this.WrongColors  = wrongColors;
        }
        public string CorrectColor { get; private set; }

        public List<string> WrongColors { get; private set; }
    }
    public class SessionSoundData
    {
        public SessionSoundData
        (
            string filename,
            byte[] content
        )
        {
            this.FileName = filename;
            this.Content  = content;
        }

        public string FileName { get; private set; }
        public byte[] Content  { get; private set; }
    }
}