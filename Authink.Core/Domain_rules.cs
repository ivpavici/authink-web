using System.Collections.Generic;

namespace Authink.Core.Domain.Rules
{
    public static class Login
    {
        public static string LoginFailedMessage                = "Korisnik s unesenim podacima nije pronađen!";
        public static string SignUpFailedMessage_UsernameTaken = "Korisničko ime se već koristi";
        public static string SignUpFailedMessage_EmailTaken    = "Email adresa se već koristi";
    }
    public static class Children
    {
        public static string DefaultProfilePictureUrl = "Images/Children/default-profile-picture.png";
    }
    public static class Task
    {
        public static IDictionary<string,string> AvailableTaskTypes = new Dictionary<string, string>
        {
            { Keys.DetectDifferentItems, "Detect different items     "}, { Keys.DetectColors, "Detect colors" },
            { Keys.ContinueSequence,     "Continue sequence of items" }, { Keys.PairHalves,   "Pair halves"   }, { Keys.PairSameItems,"Pair same items"}, 
            { Keys.Affiliation,          "Affiliation of items"       }, { Keys.OrderBySize,  "Order by size" }, { Keys.VoiceCommands,"Voice commands" }

        };
        public static IDictionary<string, string> AvailableTaskTypesDefaultPictures = new Dictionary<string, string>
        {
            { Keys.DetectDifferentItems, "Content/Images/TaskIcons/task-detect-different-items-256x256.png" }, { Keys.DetectColors, "Content/Images/TaskIcons/task-detect-colors-256x256.png" },
            { Keys.ContinueSequence,     "Content/Images/TaskIcons/task-continue-sequence-256x256.png"      }, { Keys.PairHalves,   "Content/Images/TaskIcons/task-pair-halves-256x256.png"   }, {Keys.PairSameItems,"Content/Images/TaskIcons/task-pair-same-items-256x256.png" }, 
            { Keys.Affiliation,          "Content/Images/TaskIcons/task-affiliation-256x256.png"            }, { Keys.OrderBySize,  "Content/Images/TaskIcons/task-order-by-size-256x256.png" }, {Keys.VoiceCommands,"Content/Images/TaskIcons/task-voice-commands-256x256.png"  }
        };

        public static class Keys
        {
            public static string PairSameItems        = "#001";
            public static string DetectDifferentItems = "#002";
            public static string DetectColors         = "#003";
            public static string ContinueSequence     = "#004";
            public static string PairHalves           = "#005";
            public static string Affiliation          = "#007";
            public static string OrderBySize          = "#008";
            public static string VoiceCommands        = "#009";
        }  
    }
    public static class Picture
    {
        public static class Task
        {
            public const string DefaultSavePath          = @"Images/Tasks";
            public const string DefaultResizeQuerystring = "?W=150&Mode=Stretch&Scale=Both";
        }
        public static class Children
        {
            public const string DefaultSavePath          = @"Images/Children";
            public const string DefaultResizeQuerystring = "?W=150&Mode=Stretch&Scale=Both";
        }
    }

    public static class Sound
    {
        public static IDictionary<string, string> AvailableSoundTypes = new Dictionary<string, string>
        {
            { "ttl", "Naslov zadatka" }, { "rw","Nagrada"  }, { "sml","Klik na sliku" }, { "ins", "Upute"}
        };
        public static class VoiceCommands
        {
            public const string DefaultSavePath = @"Sounds/VoiceCommands";
        }
    }
    public static class Api
    {
        public const string LoginToken        = "doVt4I-aovZtPnjXz-D1Fi";
        public const string ChildrenDataToken = "MI4-GPHwyr-phAadk5S-e9S";
        public const string StatisticsToken   = "tbosh5qhso-Q6gnMN2jf3";
    }
}
