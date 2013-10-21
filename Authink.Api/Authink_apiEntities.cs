using System.Collections.Generic;

namespace Authink.Data.Api.App.Entities
{
    public abstract class User
    {
        public class Details
        {
            public Details
            (
                string username,
                string password
            )
            {
                this.Username = username;
                this.Password = password;
            }

            public string Username { get; private set; }
            public string Password { get; private set; }
        }
    }
    public abstract class Child
    {
        public class Details
        {
            public Details
            (
                int    id,
                string firstName,
                string lastName,
                string profilePictureUrl,

                IReadOnlyList<Test.Details> tests 
            )
            {
                this.Id                = id;
                this.Firstname         = firstName;
                this.Lastname          = lastName;
                this.ProfilePictureUrl = profilePictureUrl;
                this.Tests             = tests;

            }

            public int    Id                { get; private set; }     
            public string Firstname         { get; private set; }
            public string Lastname          { get; private set; }
            public string ProfilePictureUrl { get; private set; }

            public IReadOnlyList<Test.Details> Tests { get; private set; }
        }
    }
    public abstract class Test
    {
        public class Details
        {
            public Details
            (
                int    id,
                string name,
                string shortDescription,
                string longDescription,

                IReadOnlyList<Task.Details> tasks
            )
            {
                this.Id               = id;
                this.Name             = name;
                this.ShortDescription = shortDescription;
                this.LongDescription  = longDescription;
                this.Tasks            = tasks;
            }

            public int    Id               { get; private set; }
            public string Name             { get; private set; }
            public string ShortDescription { get; private set; }
            public string LongDescription  { get; private set; }

            public IReadOnlyList<Task.Details> Tasks { get; private set; }
        }
    }
    public abstract class Task
    {
        public class Details
        {
            public Details
            (
                int     id,
                string  description,
                string  name,
                string  type,
                int     difficulty,
                string  profilePictureUrl,

                IReadOnlyList<Picture.Details> pictures,

                Sound.Details sound
            )
            {
                this.Id                = id;
                this.Description       = description;
                this.Name              = name;
                this.Type              = type;
                this.Difficulty        = difficulty;
                this.ProfilePictureUrl = profilePictureUrl;
                this.Pictures          = pictures;
                this.Sound             = sound;
            }

            public int    Id                { get; private set; }
            public string Description       { get; private set; }
            public string Name              { get; private set; }
            public string Type              { get; private set; }
            public int    Difficulty        { get; private set; }
            public string ProfilePictureUrl { get; private set; }

            public IReadOnlyList<Picture.Details> Pictures { get; private set; }

            public Sound.Details Sound  { get; private set; } 
        }
        public abstract class Statistics
        {
            public class Meta
            {
                public Meta
                (
                    int    taskId,
                    int    sucessfullClicksCount,
                    int    errorClicksCount,
                    string timeRun
                )
                {
                    this.TaskId                = taskId;
                    this.SucessfullClicksCount = sucessfullClicksCount;
                    this.ErrorClicksCount      = errorClicksCount;
                    this.TimeRun               = timeRun;
                }

                public int    TaskId                { get; private set; }
                public int    SucessfullClicksCount { get; private set; }
                public int    ErrorClicksCount      { get; private set; }
                public string TimeRun               { get; private set; }
            }
        }
    }
    
    public abstract class Picture
    {
        public class Details
        {
            public Details
            (
                int    id,
                string url,
                bool?  isAnswer,

                IReadOnlyList<Color.Details> colors,

                Sound.Details                sound
            )
            {
                this.Id       = id;
                this.Url      = url;
                this.IsAnswer = isAnswer;
                this.Colors   = colors;
                this.Sound    = sound;
            }

            public int    Id       { get; private set; }
            public string Url      { get; private set; }
            public bool?  IsAnswer { get; private set; }

            public IReadOnlyList<Color.Details> Colors { get; private set; }
            public Sound.Details                Sound  { get; private set; }
        }
    }
    public abstract class Color
    {
        public class Details
        {
            public Details
            (
                int    id,
                string value,
                bool   isCorrect
            )
            {
                this.Id        = id;
                this.Value     = value;
                this.IsCorrect = isCorrect;
            }

            public int    Id        { get; private set; }
            public string Value     { get; private set; }
            public bool   IsCorrect { get; private set; }
        }
    }
    public abstract class Sound
    {
        public class Details
        {
            public Details
            (
                int    id,
                string url,
                string type
            )
            {
                this.Id   = id;
                this.Url  = url;
                this.Type = type;
            }

            public int    Id   { get; private set; }
            public string Url  { get; private set; }
            public string Type { get; private set; }
        }
    }
}
