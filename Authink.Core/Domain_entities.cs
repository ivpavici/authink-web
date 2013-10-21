using System.Collections.Generic;

namespace Authink.Core.Domain.Entities
{
    using System;

    public abstract class User      
    {
        public class LongDetails 
        {
            public LongDetails
            (
                int    id,
                string username,
                string password,
                string firstname,
                string lastname,
                string email,
                bool   isHidden
            )
            {
                this.Id        = id;
                this.Username  = username;
                this.Password  = password;
                this.Firstname = firstname;
                this.Lastname  = lastname;
                this.Email     = email;
                this.IsHidden  = isHidden;
            }

            public int    Id        { get; private set; }
            public string Username  { get; private set; }
            public string Password  { get; private set; }
            public string Firstname { get; private set; }
            public string Lastname  { get; private set; }
            public string Email     { get; private set; }
            public bool   IsHidden  { get; private set; }
        }
        public class ShortDetails
        {
            public ShortDetails
            (
                int    id,
                string username,
                string firstname,
                string lastname,
                string email
            )
            {
                this.Id        = id;
                this.Username  = username;
                this.Firstname = firstname;
                this.Lastname  = lastname;
                this.Email     = email;
            }

            public int    Id        { get; private set; }
            public string Username  { get; private set; }
            public string Firstname { get; private set; }
            public string Lastname  { get; private set; }
            public string Email     { get; private set; }
        }
    }
    public abstract class Child     
    {
        public class LongDetails
        {
            public LongDetails
            (
                int      id,
                string   firstname,
                string   lastname,
                string   dateOfBirth,
                string   notes,
                string   center,
                string   descriptionOfCondition,
                string   parentName,
                string   placeOfBirth,
                bool     isHidden
            )
            {
                this.Id                     = id;
                this.Firstname              = firstname;
                this.Lastname               = lastname;
                this.DateOfBirth            = dateOfBirth;
                this.Notes                  = notes;
                this.Center                 = center;
                this.DescriptionOfCondition = descriptionOfCondition;
                this.ParentName             = parentName;
                this.PlaceOfBirth           = placeOfBirth;
                this.IsHidden               = isHidden;
            }

            public int      Id                     { get; private set; }
            public string   Firstname              { get; private set; }
            public string   Lastname               { get; private set; }
            public string   DateOfBirth            { get; private set; }
            public string   Notes                  { get; private set; }
            public string   Center                 { get; private set; }
            public string   DescriptionOfCondition { get; private set; }
            public string   PlaceOfBirth           { get; private set; }
            public string   ParentName             { get; private set; }
            public bool     IsHidden               { get; private set; }
        }
        public class ShortDetails
        {
            public ShortDetails
            (
                int    id,
                string firstname,
                string lastname,
                string profilePictureUrl
            )
            {
                this.Id                = id;
                this.Firstname         = firstname;
                this.Lastname          = lastname;
                this.ProfilePictureUrl = profilePictureUrl;
            }

            public int    Id                { get; private set; }
            public string Firstname         { get; private set; }
            public string Lastname          { get; private set; }
            public string ProfilePictureUrl { get; private set; }
        }
    }
    public abstract class Task      
    {
        public class Details
        {
            public Details
            (
                int    id,
                string description,
                string name,
                int    userId,
                string type,
                bool   isHidden,
                int    difficulty,
                string profilePictureUrl,

                Sound.Details voiceCommand
            )
            {
                this.Id                = id;
                this.Description       = description;
                this.Name              = name;
                this.UserId            = userId;
                this.Type              = type;
                this.IsHidden          = isHidden;
                this.Difficulty        = difficulty;
                this.ProfilePictureUrl = profilePictureUrl;

                this.VoiceCommand = voiceCommand;
            }

            public int    Id                { get; private set; }
            public string Description       { get; private set; }
            public string Name              { get; private set; }
            public int    UserId            { get; private set; }
            public string Type              { get; private set; }
            public bool   IsHidden          { get; private set; }
            public int    Difficulty        { get; private set; }
            public string ProfilePictureUrl { get; private set; }

            public Sound.Details VoiceCommand { get; private set; }
        }
       
    }
    public abstract class Test      
    {
        public class LongDetails 
        {
            public LongDetails
            (
                int    id,
                string name,
                string shortDescription,
                string longDescription,
                bool   isDeleted,
                int    userId,

                IReadOnlyList<Task.Details> tasks
            )
            {
                this.Id               = id;
                this.Name             = name;
                this.ShortDescription = shortDescription;
                this.LongDescription  = longDescription;
                this.IsDeleted        = isDeleted;
                this.UserId           = userId;
                this.Tasks            = tasks;
            }

            public int    Id               { get; private set; }
            public string Name             { get; private set; }
            public string ShortDescription { get; private set; }
            public string LongDescription  { get; private set; }
            public bool   IsDeleted        { get; private set; }
            public int    UserId           { get; private set; }

            public IReadOnlyList<Task.Details> Tasks { get; private set; }
        }
        public class ShortDetails
        {
            public ShortDetails
            (
                int    id,
                string name,
                string shortDescription
            )
            {
                this.Id               = id;
                this.Name             = name;
                this.ShortDescription = shortDescription;
            }

            public int    Id               { get; private set; }
            public string Name             { get; private set; }
            public string ShortDescription { get; private set; }
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
                int    pictureId,
                bool   isCorrect
            )
            {
                this.Id        = id;
                this.Value     = value;
                this.PictureId = pictureId;
                this.IsCorrect = isCorrect;
            }

            public int    Id        { get; private set; }
            public string Value     { get; private set; }
            public int    PictureId { get; private set; }
            public bool   IsCorrect { get; private set; }
        }
        
    }
    public abstract class Picture   
    {
        public class Details
        {
            public Details
            (
                int     id,
                string  url,
                string  theme,
                bool    isHidden,
                bool?   isAnswer,

                Sound.Details   sound
            )
            {
                this.Id       = id;
                this.Url      = url;
                this.Theme    = theme;
                this.Sound    = sound;
                this.IsAnswer = isAnswer;
                this.IsHidden = isHidden;
            }

            public int     Id       { get; private set; }
            public string  Url      { get; private set; }
            public string  Theme    { get; private set; }
            public int?    SoundId  { get; private set; }
            public bool    IsHidden { get; private set; }
            public bool?   IsAnswer { get; private set; }

            public Sound.Details Sound { get; private set; }
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
                bool   isHidden,
                string title,
                string type
            )
            {
                this.Id       = id;
                this.Url      = url;
                this.IsHidden = isHidden;
                this.Title    = title;
                this.Type     = type;
            }

            public int    Id       { get; private set; }
            public string Url      { get; private set; }
            public bool   IsHidden { get; private set; }
            public string Title    { get; private set; }
            public string Type     { get; private set; }
        }
    }
    public abstract class Statistics
    {
        public class Meta
        {
            public Meta
            (
                int    id,
                string sucessfullClicksCount,
                string errorClicksCount,
                string totalRunSummaryCsv,
                string dates
            )
            {
                this.Id                    = id;
                this.SucessfullClicksCount = sucessfullClicksCount;
                this.ErrorClicksCount      = errorClicksCount;
                this.TotalRunSummaryCsv    = totalRunSummaryCsv;
                this.Dates = dates;
            }

            public int    Id                    { get; private set; }
            public string SucessfullClicksCount { get; private set; }
            public string ErrorClicksCount      { get; private set; }
            public string TotalRunSummaryCsv    { get; private set; }
            public string Dates                 { get; private set; }       
        }
    }
 }
