using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Configuration;

using Authink.Core.Model.Queries;
using ImageResizer;

using ent      = Authink.Core.Domain.Entities;
using buru     = Authink.Core.Domain.Rules;
using database = Authink.Data;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NLog;

namespace Authink.Core.Model.Services.Impl
{
    public class BlobPictureServiceImpl : IPictureServices
    {
        public byte[] ResizePicture(byte[] pictureData, string resizeQuerystring)
        {
            using(var outputStream = new MemoryStream())
            {
                using(var inputStream= new MemoryStream(pictureData))
                {
                    var resizeSettings = new ResizeSettings(resizeQuerystring);
                    
                    ImageBuilder.Current.Build
                    (
                        source:   inputStream,
                        dest:     outputStream,
                        settings: resizeSettings
                    );
                    return outputStream.ToArray();
                }
            }
        }
    }
    
    public class LoginServicesImpl : ILoginServices
    {
        public LoginServicesImpl
        (
            IUserQueries    userQueries,
            HttpContextBase context
        )
        {
            this.UserQueries = userQueries;
            this.Context     = context;
        }

        private readonly IUserQueries    UserQueries;
        private readonly HttpContextBase Context;

        public bool Login(string username, string password)
        {
            return 
                UserQueries.GetSingle_whereUsernameAndPassword(username: username, password: password) != null;
        }
        public ent::User.ShortDetails GetSignedInUser()
        {
            return
                !Context.Request.IsAuthenticated ? null : UserQueries.GetSingle_whereUsername(Context.User.Identity.Name);
        }
        public void SignIn(string username,bool isSessionLong)
        {
            FormsAuthentication.SetAuthCookie(userName:username, createPersistentCookie:isSessionLong);
        }
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    public class UserAccessRightsImpl : IUserAccessRights
    {
        public UserAccessRightsImpl
        (
            ILoginServices loginServices,
            Logger         logger
        )
        {
            this.loginServices = loginServices;
            this.logger        = logger;
        }

        private readonly ILoginServices loginServices;
        private Logger                  logger;

        public bool CanCreateTask(int testId)
        {
            var currentUser = loginServices.GetSignedInUser();
            if(currentUser == null)
            {
                logger.Warn("Anonymous user tried to create task in test with Id = {0} ", testId);
                return false;
            }

            using(var db = new database::AuthinkDataModel())
            {
                return
                    db.Users
                    .Single(user => user.Id == currentUser.Id)
                    .Children
                    .Any(child => child.Tests.Any(test => test.Id == testId));
            }
        }
        public bool CanReadTask  (int taskId)
        {
            var currentUser = loginServices.GetSignedInUser();
            if (currentUser == null)
            {
                logger.Warn("Anonymous user tried to access task with Id = {0} ", taskId);
                return false;
            }

            using (var db = new database::AuthinkDataModel())
            {
                return
                    db.Users
                      .Single(user => user.Id == currentUser.Id)
                      .Children
                      .Any(child => child.Tests.Any(test => test.Tasks.Any(task => task.Id == taskId)));
            }
        }
        public bool CanEditTask  (int taskId)
        {
            var currentUser = loginServices.GetSignedInUser();
            if (currentUser == null)
            {
                logger.Warn("Anonymous user tried to edit task with Id = {0}", taskId);
                return false;
            }

            using (var db = new database::AuthinkDataModel())
            {
                return
                    db.Users
                      .Single(user => user.Id == currentUser.Id)
                      .Tasks.Any(task => task.Id == taskId);
            }
        }

        public bool CanCreateTest(int childId)
        {
            var currentUser = loginServices.GetSignedInUser();
            if (currentUser == null)
            {
                logger.Warn("Anonymous user tried to add new test to child with Id = {0} ", childId);
                return false;
            }

            using (var db = new database::AuthinkDataModel())
            {
                return
                    db.Users
                      .Single(user => user.Id == currentUser.Id)
                      .Children.Any(child => child.Id == childId);
            }
        }
        public bool CanReadTest  (int testId )
        {
            var currentUser = loginServices.GetSignedInUser();
            if (currentUser == null)
            {
                logger.Warn("Anonymous user tried to access test with Id = {0} ", testId);
                return false;
            }

            using (var db = new database::AuthinkDataModel())
            {
                return
                    db.Users
                      .Single(user => user.Id == currentUser.Id)
                      .Children.Any(child => child.Tests.Any(test => test.Id == testId));
            }
        }
        public bool CanEditTest  (int testId )
        {
            var currentUser = loginServices.GetSignedInUser();
            if (currentUser == null)
            {
                logger.Warn("Anonymous user tried to edit test with Id = {0}", testId);
                return false;
            }

            using (var db = new database::AuthinkDataModel())
            {
                return
                    db.Users
                      .Single(user => user.Id == currentUser.Id)
                      .Children.Any(child => child.Tests.Any(test => test.Id == testId));
            }
        }

        public bool CanEditPicture(int pictureId)
        {
            return true;
        }
        public bool CanEditSound  (int soundId  )
        {
            return true;
        }

        public bool CanReadChild(int childId)
        {
            var currentUser = loginServices.GetSignedInUser();
            if (currentUser == null)
            {
                logger.Warn("Anonymous user tried to access child with Id = {0}", childId);
                return false;
            }

            using (var db = new database::AuthinkDataModel())
            {
                return
                    db.Users
                      .Single(user => user.Id == currentUser.Id)
                      .Children.Any(child => child.Id == childId);
            }
        }
        public bool CanEditChild(int childId)
        {
            var currentUser = loginServices.GetSignedInUser();
            if (currentUser == null)
            {
                logger.Warn("Anonymous user tried to edit child with Id = {0} ", childId);
                return false;
            }

            using (var db = new database::AuthinkDataModel())
            {
                return
                    db.Users
                      .Single(user => user.Id == currentUser.Id)
                      .Children.Any(child => child.Id == childId);
            }
        }
    }
}