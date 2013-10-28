using System;
using System.Linq;
using System.Web.Security;
using System.Collections.Generic;

using database = Authink.Data;
using mappers  = Authink.Core.Model.Mappers;
using ent      = Authink.Core.Domain.Entities;

namespace Authink.Core.Model.Queries.Impl
{
    public class UserQueriesImpl:       IUserQueries      
    {
        public ent::User.ShortDetails GetSingle_whereUsername           (string userName                )
        {
            using (var db = new database::AuthinkDataModel())
            {
                return 
                    db.Users
                      .Where           (x => x.Username == userName            )
                      .Select          (mappers::User.ShortDetails.FromDatabase)
                      .SingleOrDefault (                                       );
            }
        }
        public ent::User.ShortDetails GetSingle_whereUsernameAndPassword(string username,string password)
        {
            using (var db = new database::AuthinkDataModel())
            {
                var hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
                var user           = db.Users.SingleOrDefault(x => x.Username == username && x.Password == hashedPassword);

                return user == null ? null
                                    : new ent::User.ShortDetails
                                      (
                                            id:        user.Id, 
                                            username:  user.Username, 
                                            firstname: user.FirstName,
                                            lastname:  user.LastName, 
                                            email:     user.Email
                                      );
            }
        }
        public ent::User.ShortDetails GetSingle_whereEmail              (string email                   )
        {
            using(var db= new database::AuthinkDataModel())
            {
                return
                    db.Users
                      .Where(x => x.Email == email)
                      .Select(mappers::User.ShortDetails.FromDatabase)
                      .SingleOrDefault();
            }
        }
    }
    public class ChildQueriesImpl:      IChildQueries     
    {
        public IReadOnlyList<ent::Child.ShortDetails> GetAll_paged  (bool showHidden, int page  )
        {
            throw new System.NotImplementedException();
        }
        public IReadOnlyList<ent::Child.ShortDetails> GetAll_forUser(bool showHidden, int userId)
        {
            using(var db = new database::AuthinkDataModel())
            {
                return
                    db.Users
                      .SingleOrDefault(user => user.Id == userId)
                      .Children
                      .Where (child => (!child.IsHidden || showHidden))
                      .Select(mappers::Child.ShortDetails.FromDatabase)
                      .ToList(                                        );
            }
        }

        public ent::Child.ShortDetails GetSingle_whereId                 (int id                          )
        {
            using (var db = new database::AuthinkDataModel())
            {
                return
                    db.Children
                      .Where (child => child.Id == id                 )
                      .Select(mappers::Child.ShortDetails.FromDatabase)
                      .Single();
            }
        }
        public ent::Child.ShortDetails GetSingle_whereUsername           (string username                 )
        {
            throw new System.NotImplementedException();
        }
        public ent::Child.ShortDetails GetSingle_whereUsernameAndPassword(string username, string password)
        {
            throw new System.NotImplementedException();
        }
    }
    public class TaskQueriesImpl:       ITaskQueries      
    {
        public ent::Task.LongDetails GetSingle_longDetails_whereId(int id)
        {
            using(var db = new database::AuthinkDataModel())
            {
                return
                    db.Tasks
                      .Include("Pictures")
                      .Where(task => task.Id == id)
                      .Select(mappers::Task.LongDetails.FromDatabase)
                      .Single();
            }
        }

        public IReadOnlyList<ent::Task.ShortDetails> GetAll_shortDetails_whereTestId(int testId)
        {
            using (var db = new database::AuthinkDataModel())
            {
                return
                    db.Tests
                      .Single(test => test.Id == testId)
                      .Tasks
                      .Select(mappers::Task.ShortDetails.FromDatabase)
                      .ToList();
            }
        }
    }
    public class TestQueriesImpl:       ITestQueries      
    {
        public IReadOnlyList<ent::Test.ShortDetails> GetAll_forChild(bool showHidden, int childId)
        {
            
            using(var db= new database::AuthinkDataModel())
            {
                return
                    db.Children
                      .SingleOrDefault(child => child.Id == childId)
                      .Tests
                      .Where (test => (!test.IsDeleted || showHidden))
                      .Select(mappers::Test.ShortDetails.FromDatabase)
                      .ToList(                                       );
            }
        }

        public ent::Test.ShortDetails GetTest_thatContainsTask(int taskId)
        {
            using(var db = new database::AuthinkDataModel())
            {
                return db.Tests
                         .Where (test => test.Tasks.Any(task => task.Id == taskId))
                         .Select(mappers::Test.ShortDetails.FromDatabase          )
                         .Single(                                                 );
            }
        }

        public ent::Test.ShortDetails GetSingle_ShortDetails_WhereId(int id)
        {
            using(var db = new database::AuthinkDataModel())
            {
                return
                    db.Tests
                       .Where (test=>test.Id == id  && test.IsDeleted==false)
                       .Select(mappers::Test.ShortDetails.FromDatabase      )
                       .Single(                                             );
            }
        }
        public ent::Test.LongDetails  GetSingle_LongDetails_WhereId (int id)
        {
            using (var db = new database::AuthinkDataModel())
            {
                return
                    db.Tests
                       .Include("Tasks")
                       .Where (test => test.Id == id)
                       .ToList()
                       .Select(mappers::Test.LongDetails.FromDatabase)
                       .Single();
            }
        }
    }
    public class PictureQueriesImpl:    IPictureQueries   
    {
        public IReadOnlyList<ent::Picture>       GetAll_forTaskGameplay (int taskId     )
        {
            using (var db= new database::AuthinkDataModel())
            {
                return
                    db.Tasks
                      .Single(task => task.Id == taskId)
                      .Pictures
                      .ToList(                                     )
                      .Select(mappers::Picture.Details.FromDatabase)
                      .ToList(                                     );
            }
        }
        public IReadOnlyList<ent::Color.Details> GetAll_colorsForPicture(int pictureId  )
        {
            using (var db = new database::AuthinkDataModel())
            {
                return
                    db.Pictures
                      .Single(picture => picture.Id == pictureId)
                      .Colors
                      .ToList()
                      .Select(mappers::Color.Details.FromDatabase)
                      .ToList();
            }
        }

        public ent::Picture         GetSingle_whereId       (int id     )
        {
            using(var db = new database::AuthinkDataModel())
            {
                return
                    db.Pictures
                      .Where (picture => picture.Id == id          )
                      .Select(mappers::Picture.Details.FromDatabase)
                      .Single();
            }
        }
        public ent::Color.Details   GetSingle_color_WhereId (int colorId)
        {
            throw new System.NotImplementedException();
        }
    }
    public class SoundQueriesImpl:      ISoundQueries     
    {
        public ent::Sound.Details GetSingle_byTaskId(int taskId     )
        {
            using (var db = new database::AuthinkDataModel())
            {
                return
                    db.Tasks
                        .Where(task => task.Id == taskId)
                        .Select(task => mappers::Sound.Details.FromDatabase(task.Sound))
                        .Single();

            }
        }

        public ent::Sound.Details GetSingle_whereId   (int id       )
        {
            using(var db = new database::AuthinkDataModel())
            {
                return
                    db.Sounds
                      .Where (sound => sound.Id == id            )
                      .Select(mappers::Sound.Details.FromDatabase)
                      .Single(                                   );
            }
        }
        public ent::Sound.Details GetSingle_forPicture(int pictureId)
        {
            throw new System.NotImplementedException();
        }
    }
    public class StatisticsQueriesImpl: IStatisticsQueries
    {
        public ent::Statistics.Meta GetStatistics_Meta_ForTest(int testId)
        {
            throw new NotImplementedException();
        }
        public ent::Statistics.Meta GetStatistics_Meta_ForTask(int taskId)
        {
            using (var db = new database::AuthinkDataModel())
            {
                return
                    db.Tasks
                        .Where(task => task.Id == taskId)
                        .Select(test => test.Statistics_Meta)
                        .ToList()
                        .Select(mappers::Statistics.Meta.FromDatabse)
                        .SingleOrDefault();
            }
        }
    }
}
