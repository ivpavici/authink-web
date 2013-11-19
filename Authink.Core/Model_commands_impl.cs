using System;
using System.Linq;
using System.Web.Security;

using Authink.Core.Model.Services;

using ent      = Authink.Core.Domain.Entities;
using database = Authink.Data;
using rules    = Authink.Core.Domain.Rules;

namespace Authink.Core.Model.Commands.Impl
{
    public class UserCommandsImpl:    IUserCommands 
    {
        public void Create(string username, string password, string firstname, string lastname, string email)
        {
            using (var db = new database::AuthinkDataModel())
            {
                var user = new database::User 
                {
                    FirstName = firstname,
                    LastName  = lastname,
                    Username  = username,
                    Password  = FormsAuthentication.HashPasswordForStoringInConfigFile(password,"sha1"),
                    Email     = email,
                    IsHidden  = false
                };

                db.Users.Add(user);
                db.SaveChanges();
            }
        }
    }
    public class ChildCommandsImpl:   IChildCommands
    {
        public ChildCommandsImpl
        (
            ILoginServices loginServices
        )
        {
            this.loginServices = loginServices;
        }

        private readonly ILoginServices loginServices;

        public int Create      (string firstname, string lastname       )
        {
            using (var db=new database::AuthinkDataModel())
            {
                var currentUser = loginServices.GetSignedInUser();
                var parent      = db.Users.SingleOrDefault(x => x.Id == currentUser.Id);

                var child = new database::Child
                {
                    FirstName         = firstname,
                    LastName          = lastname,
                    IsHidden          = false,
                    ProfilePictureUrl = rules::Children.DefaultProfilePictureUrl,
                    Users             = { parent }

                };

                db.Children.Add(child);
                db.SaveChanges();

                return child.Id;
            }
        }
        public void Update      (int id, string firstname, string lastname)
        {
            using (var db = new database::AuthinkDataModel())
            {
                var dbChild = db.Children.Single(child=> child.Id == id);

                dbChild.FirstName         = firstname;
                dbChild.LastName          = lastname;

                db.SaveChanges();
            }
        }
        public void Delete      (int id)                                                                                                                                                                                            
        {
            using (var db = new database::AuthinkDataModel())
            {
                var dbChild = db.Children.Single(child => child.Id == id);

                dbChild.IsHidden = true;
                db.SaveChanges();
            }
        }
        public void UpdatePicture(int id, string profilePictureUrl)
        {
            using (var db = new database::AuthinkDataModel())
            {
                var dbChild = db.Children.Single(child => child.Id == id);

                dbChild.ProfilePictureUrl = profilePictureUrl;

                db.SaveChanges();
            }
        }
    }
    public class TaskCommandsImpl:        ITaskCommands      
    {
        public int  Create(string description, string name, int userId, string type, int difficulty, string profilePictureUrl, ent::Sound.Details voiceCommand)
        {
            using(var db = new database::AuthinkDataModel())
            {
                var task = new database::Task
                {
                    Name              = name,
                    Description       = description,
                    Difficulty        = difficulty,
                    IsHidden          = false,
                    ProfilePictureUrl = profilePictureUrl,
                    Type              = type,
                    UserId            = userId
                };

                if(voiceCommand != null)
                {
                    task.Sound = new database::Sound
                                     {
                                         IsHidden = false,
                                         Type     = voiceCommand.Type,
                                         Url      = voiceCommand.Url
                                     };
                }

                db.Tasks.Add(task);
                db.SaveChanges();

                return task.Id;
            }
        }
        public void Update(int id, string description, string name)
        {
            using(var db = new database::AuthinkDataModel())
            {
                var dbTask = db.Tasks.Single(task => task.Id == id);

                dbTask.Name        = name;
                dbTask.Description = description;

                db.SaveChanges();
            }
        }
        public void Delete(int id)
        {
            using(var db = new database::AuthinkDataModel())
            {
                var dbTask = db.Tasks.SingleOrDefault(task => task.Id == id);
                if (dbTask != null)
                {
                    dbTask.IsHidden = true;
                    db.SaveChanges();
                }
            }
        }
        public void ToggleAttachToTest(int taskId, int testId     )
        {
            using(var db = new database::AuthinkDataModel())
            {
                var dbTest = db.Tests.Single(test => test.Id == testId);
                var dbTask = db.Tasks.Single(task => task.Id == taskId);

                if(dbTest.Tasks.Contains(dbTask))
                {
                    dbTest.Tasks.Remove(dbTask);
                }else
                {
                    dbTest.Tasks.Add(dbTask);
                }

                db.SaveChanges();
            }
        }
    }
    public class TestCommandsImpl:        ITestCommands      
    {
        public TestCommandsImpl
        (
            ILoginServices loginServices
        )
        {
            this.loginServices = loginServices;
        }

        private readonly ILoginServices loginServices;

        public int  Create (string name, string shortDescription, string longDescription, int userId, int childId)
        {
            using(var db= new database::AuthinkDataModel())
            {
                var currentUser  = loginServices.GetSignedInUser();
                var currentChild = db.Children.SingleOrDefault(child => child.Id == childId);

                var test = new database::Test
                {
                    Name             = name,
                    ShortDescription = shortDescription,
                    LongDescription  = longDescription,
                    IsDeleted        = false,
                    UserId           = currentUser.Id,
                    Children         = {currentChild}
                };

                db.Tests.Add(test);
                db.SaveChanges();

                return test.Id;
            }
        }
        public void Update (int id, string name, string shortDescription, string longDescription                 )
        {
            using(var db= new database::AuthinkDataModel())
            {
                var dbTest = db.Tests.Single(test => test.Id == id);

                dbTest.Name             = name;
                dbTest.ShortDescription = shortDescription;
                dbTest.LongDescription  = longDescription;

                db.SaveChanges();
            }
        }
        public void Delete (int id)
        {
            using (var db = new database::AuthinkDataModel())
            {
                var dbTest = db.Tests.Single(test => test.Id == id);

                dbTest.IsDeleted = true;
                db.SaveChanges();
            }
        }

        public void ToggleAttachToChild(int childId, int testId)
        {
            throw new NotImplementedException();
        }
    }
    public class PictureCommandsImpl:     IPictureCommands   
    {
        public int  Create(string url, string theme, bool? isAnswer)
        {
            using(var db= new database::AuthinkDataModel())
            {
                var picture = new database::Picture
                {
                    Url      = url,
                    IsHidden = false,
                    IsAnswer = isAnswer,
                    Theme    = theme
                };

                db.Pictures.Add(picture);
                db.SaveChanges();

                return picture.Id;
            }
        }
        public void Update(int id, string url                      )
        {
            using (var db = new database::AuthinkDataModel())
            {
                var dbPicture = db.Pictures.Single(picture => picture.Id == id);

                dbPicture.Url = url;
                db.SaveChanges();
            }
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        public void AttachToTask(int taskId, int pictureId         )
        {
            using(var db= new database::AuthinkDataModel())
            {
                var dbTask    = db.Tasks.Single(task=> task.Id== taskId);
                var dbPicture = db.Pictures.Single(picture => picture.Id == pictureId);

                dbTask.Pictures.Add(dbPicture);
                db.SaveChanges();
            }
        }

        public int Create_color(string value, int pictureId, bool isCorrect)
        {
            using(var db = new database::AuthinkDataModel())
            {
                var color = new database::Color
                {
                    Value     = value,
                    IsCorrect = isCorrect,
                    PictureId = pictureId
                };

                db.Colors.Add(color);
                db.SaveChanges();

                return color.Id;
            }
        }
        public void Update_color(int id, string value)
        {
            using(var db = new database::AuthinkDataModel())
            {
                var dbColor = db.Colors.Single(color => color.Id == id);

                dbColor.Value = value;
                db.SaveChanges();
            }
        }
        public void AttachColorToPicture(int pictureId, int colorId       )
        {
            using(var db = new database::AuthinkDataModel())
            {
                var dbPicture = db.Pictures.Single(picture => picture.Id == pictureId);
                var dbColor   = db.Colors.Single(color => color.Id == colorId);

                dbPicture.Colors.Add(dbColor);
                db.SaveChanges();
            }
        }
    }
    public class SoundCommandsImpl:       ISoundCommands     
    {
        public int Create(string url, string title, string type)
        {
            using(var db = new database::AuthinkDataModel())
            {
                var sound = new database::Sound
                {
                    IsHidden = false,
                    Type     = type,
                    Url      = url
                };

                db.Sounds.Add(sound);
                db.SaveChanges();

                return sound.Id;
            }
        }
        public void Update(int id, string url)
        {
            using(var db = new database::AuthinkDataModel())
            {
                var dbSound = db.Sounds.Single(sound => sound.Id == id);

                dbSound.Url = url;

                db.SaveChanges();
            }
        }
        public void AttachSoundToTask   (int soundId, int taskId    )
        {
            using(var db = new database::AuthinkDataModel())
            {
                var dbTask  = db.Tasks.Single(task => task.Id == taskId);
                var dbSound = db.Sounds.Single(sound => sound.Id == soundId);

                dbTask.Sound = dbSound;
                db.SaveChanges();
            }
        }
    }
}