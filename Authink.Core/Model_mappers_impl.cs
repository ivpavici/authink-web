using System;
using System.Collections.Generic;
using ent      = Authink.Core.Domain.Entities;
using database = Authink.Data;

namespace Authink.Core.Model.Mappers
{
    public static class Statistics
    {
        public static class Meta
        {
            public static ent::Statistics.Meta FromDatabse(database::Statistics_Meta statisticsMetaData)
            {
                return new ent.Statistics.Meta
                (
                    id:                    statisticsMetaData.Id,
                    sucessfullClicksCount: statisticsMetaData.SucessfullClicks,
                    errorClicksCount:      statisticsMetaData.ErrorClicks,
                    totalRunSummaryCsv:    statisticsMetaData.TotalRunSummary,
                    dates:                 statisticsMetaData.Dates
                );
            }
        }
    }
    public static class User   
    {
        public static class LongDetails
        {
            public static ent::User.LongDetails FromDatabase(database::User userData)
            {
                return new ent::User.LongDetails
                (
                    id:        userData.Id,
                    username:  userData.Username,
                    password:  userData.Password,
                    firstname: userData.FirstName,
                    lastname:  userData.LastName,
                    email:     userData.Email,
                    isHidden:  userData.IsHidden
                );
            }
        }
        public static class ShortDetails
        {
            public static ent::User.ShortDetails FromDatabase (database::User userData)
            {
                return new ent::User.ShortDetails
                (
                    id:        userData.Id,
                    username:  userData.Username,
                    firstname: userData.FirstName,
                    lastname:  userData.LastName,
                    email:     userData.Email
                );
            }
        }
    }
    public static class Child  
    {
        public static class LongDetails
        {
            public static ent::Child.LongDetails FromDatabase(database::Child childData)
            {
                return new ent::Child.LongDetails
                (
                     id:                     childData.Id,
                     firstname:              childData.FirstName,
                     lastname:               childData.LastName,
                     dateOfBirth:            childData.DateOfBirth,
                     notes:                  childData.Notes,
                     center:                 childData.Center,
                     descriptionOfCondition: childData.DescriptionOfCondition,
                     parentName:             childData.ParentName,
                     placeOfBirth:           childData.PlaceOfBirth,
                     isHidden:               childData.IsHidden
                );
            }
        }
        public static class ShortDetails
        {
            public static ent::Child.ShortDetails FromDatabase(database::Child childData)
            {
                return new ent::Child.ShortDetails
                (
                    id:                childData.Id,
                    firstname:         childData.FirstName,
                    lastname:          childData.LastName,
                    profilePictureUrl: childData.ProfilePictureUrl
                );
            }
        }
    }
    public static class Test   
    {
        public static class LongDetails
        {
            public static ent::Test.LongDetails FromDatabase(database::Test testData, Func<bool, int, IReadOnlyList<ent::Task.Details>> tasksProvider)
            {
                return new ent::Test.LongDetails
                (
                    id:               testData.Id,
                    name:             testData.Name,
                    shortDescription: testData.ShortDescription,
                    longDescription:  testData.LongDescription,
                    isDeleted:        testData.IsDeleted,
                    userId:           testData.UserId,

                    tasks:            tasksProvider(false, testData.Id)

                );
            }
        }
        public static class ShortDetails
        {
            public static ent::Test.ShortDetails FromDatabase(database::Test testData)
            {
                return new ent::Test.ShortDetails
                (
                    id:               testData.Id,
                    name:             testData.Name,
                    shortDescription: testData.ShortDescription
                );
            }
        }
    }
    public static class Task   
    {
        public static class Details
        {
            public static ent::Task.Details FromDatabase(database::Task taskData)
            {
                return new ent::Task.Details
                (
                    id:                taskData.Id,
                    description:       taskData.Description,
                    name:              taskData.Name,
                    userId:            taskData.UserId,
                    type:              taskData.Type,
                    isHidden:          taskData.IsHidden,
                    difficulty:        taskData.Difficulty,
                    profilePictureUrl: taskData.ProfilePictureUrl,
                    voiceCommand:      Sound.Details.FromDatabase(taskData.Sound)
                );
            }
        }
    }
    public static class Picture
    {
        public static class Details
        {
            public static ent::Picture.Details FromDatabase(database::Picture pictureData)
            {
                return new ent::Picture.Details
                (
                    id:       pictureData.Id,
                    url:      pictureData.Url,
                    theme:    pictureData.Theme,
                    isHidden: pictureData.IsHidden,
                    isAnswer: pictureData.IsAnswer,

                    sound:    Sound.Details.FromDatabase(pictureData.Sound)
                );
            }
        }
    }
    public static class Color  
    {
        public static class Details
        {
            public static ent::Color.Details FromDatabase(database::Color colorData)
            {
                return new ent::Color.Details
                (
                    id:        colorData.Id,
                    value:     colorData.Value,
                    pictureId: colorData.PictureId,
                    isCorrect: colorData.IsCorrect
                );
            }
        }
    }
    public static class Sound  
    {
        public static class Details
        {
            public static ent::Sound.Details FromDatabase(database::Sound soundData)
            {
                return soundData==null ? null : 
                    new ent::Sound.Details
                    (
                        id:       soundData.Id,
                        url:      soundData.Url,
                        isHidden: soundData.IsHidden,
                        title:    soundData.Title,
                        type:     soundData.Type
                    );
            }
        }
    }
}