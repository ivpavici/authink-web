using System.Linq;

using ent       = Authink.Data.Api.App.Entities;
using database  = Authink.Data;

namespace Authink.Api.Mappers
{
    public static class Child
    {
        public static ent::Child.Details FromDatabaseData(database::Child childData)
        {
            return new ent::Child.Details
             (
                id:                childData.Id,
                firstName:         childData.FirstName,
                lastName:          childData.LastName,
                profilePictureUrl: childData.ProfilePictureUrl,
                tests:             childData.Tests
                                            .Where(test => test.IsDeleted == false)
                                            .Select(Test.FromDatabaseData)
                                            .ToList(                     )
             );
        }
    }
    public static class Test   
    {
        public static ent::Test.Details FromDatabaseData(database::Test testData)
        {
            return new ent::Test.Details
            (
                id:               testData.Id,
                name:             testData.Name,
                shortDescription: testData.ShortDescription,
                longDescription:  testData.LongDescription,

                tasks: testData.Tasks
                               .Where (task=>task.IsHidden == false)
                               .Select(Task.FromDatabaseData       )
                               .ToList(                            )
            );
        }
    }
    public static class Task   
    {
        public static ent::Task.Details FromDatabaseData(database::Task taskData)
        {
            return new ent::Task.Details
            (
                id:                taskData.Id,
                description:       taskData.Description,
                name:              taskData.Name,
                type:              taskData.Type,
                difficulty:        taskData.Difficulty,
                profilePictureUrl: taskData.ProfilePictureUrl,

                pictures: taskData.Pictures
                                  .ToList(                        )
                                  .Select(Picture.FromDatabaseData)
                                  .ToList(                        ),

                sound:   Sound.FromDatabaseData(taskData.Sound)
            );
        }
    }
    public static class Picture
    {
        public static ent::Picture.Details FromDatabaseData(database::Picture pictureData)
        {
            return new ent::Picture.Details
                (
                id:       pictureData.Id,
                url:      pictureData.Url,
                isAnswer: pictureData.IsAnswer,
                sound:    Sound.FromDatabaseData(pictureData.Sound),

                colors:   pictureData.Colors
                                     .ToList(                      )
                                     .Select(Color.FromDatabaseData)
                                     .ToList(                      )
                );
        }
    }
    public static class Sound  
    {
        public static ent::Sound.Details FromDatabaseData(database::Sound soundData)
        {
            return soundData == null ? null :
                   new ent::Sound.Details
                   (
                       id:   soundData.Id,
                       url:  soundData.Url,
                       type: soundData.Type
                   );
        }
    }
    public static class Color  
    {
        public static ent::Color.Details FromDatabaseData(database::Color colorData)
        {
            return colorData == null ? null: 
                   new ent::Color.Details
                   (
                       id:        colorData.Id,
                       value:     colorData.Value,
                       isCorrect: colorData.IsCorrect
                   );
        }
    }
}
