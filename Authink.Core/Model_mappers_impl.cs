using System;
using System.Collections.Generic;
using System.Linq;
using ent      = Authink.Core.Domain.Entities;
using database = Authink.Data;

namespace Authink.Core.Model.Mappers
{
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
            public static ent::Test.LongDetails FromDatabase(database::Test testData)
            {
                return new ent::Test.LongDetails
                (
                    id:               testData.Id,
                    name:             testData.Name,
                    shortDescription: testData.ShortDescription,
                    longDescription:  testData.LongDescription,
                    isDeleted:        testData.IsDeleted,
                    userId:           testData.UserId,

                    tasks:            testData.Tasks.Where(task => !task.IsHidden).Select(Task.ShortDetails.FromDatabase).ToList()

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
        public static class LongDetails
        {
            public static ent::Task.LongDetails FromDatabase(database::Task taskData)
            {
                return new ent::Task.LongDetails
                (
                    id:                taskData.Id,
                    description:       taskData.Description,
                    name:              taskData.Name,
                    userId:            taskData.UserId,
                    type:              taskData.Type,
                    isHidden:          taskData.IsHidden,
                    difficulty:        taskData.Difficulty,
                    profilePictureUrl: taskData.ProfilePictureUrl,
                    voiceCommand:      Sound.Details.FromDatabase(taskData.Sound),
                    pictures:          taskData.Pictures.Where(picture => !picture.IsHidden).Select(Picture.Details.FromDatabase).ToList()
                );
            }
        }

        public static class ShortDetails
        {
            public static ent::Task.ShortDetails FromDatabase(database::Task taskData)
            {
                return new ent::Task.ShortDetails
                (
                    id:                taskData.Id,
                    description:       taskData.Description,
                    name:              taskData.Name,
                    difficulty:        taskData.Difficulty,
                    profilePictureUrl: taskData.ProfilePictureUrl
                );
            }
        }
    }
    public static class Picture
    {
        public static class Details
        {
            public static ent::Picture FromDatabase(database::Picture pictureData)
            {
                if(pictureData.Colors.Any())
                {
                    return new ent::Picture.WithColors
                    (
                        id:  pictureData.Id,
                        url: pictureData.Url,

                        sound:        Sound.Details.FromDatabase(pictureData.Sound),
                        wrongColors:  pictureData.Colors.Where(color => !color.IsCorrect).Select(Color.Details.FromDatabase).ToList(),
                        correctColor: pictureData.Colors.Where(color => color.IsCorrect).Select(Color.Details.FromDatabase).Single()
                    );
                }
                
                return new ent::Picture.Simple
                (
                    id:       pictureData.Id,
                    url:      pictureData.Url,
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
                        type:     soundData.Type
                    );
            }
        }
    }
}