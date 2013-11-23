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

namespace Authink.Core.Model.Services.Impl
{
    public class BlobPictureServiceImpl : IPictureServices
    {
        public BlobPictureServiceImpl()
        {
            this.cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AuthinkBlobStorage"].ConnectionString);
            this.blobClient          = cloudStorageAccount.CreateCloudBlobClient();
            this.cloudBlobContainer  = blobClient.GetContainerReference("content");
        }
        private CloudStorageAccount cloudStorageAccount;
        private CloudBlobClient     blobClient;
        private CloudBlobContainer  cloudBlobContainer;
        public string Save(string pictureName, byte[] pictureContent, int relatedId, string baseSavePath, string resizeQueryString)
        {
            var resizedPicture = ResizePicture(pictureContent, resizeQueryString);
            var blobSavePath   = string.Format("{0}/{1}/{2}", baseSavePath, relatedId,pictureName);
            var blockBlob      = cloudBlobContainer.GetBlockBlobReference(blobSavePath);

            using (var stream = new MemoryStream(resizedPicture))
            {
                blockBlob.UploadFromStream(stream);
            }

            return blockBlob.Uri.ToString();
        }

        public void Save(byte[] pictureContent, string savePath, string resizeQueryString)
        {
            throw new NotImplementedException();
        }

        private byte[] ResizePicture(byte[] pictureData, string resizeQuerystring)
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
    public class BlocSoundServicesImpl : ISoundServices
    {
        public BlocSoundServicesImpl()
        {
            this.cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AuthinkBlobStorage"].ConnectionString);
            this.blobClient          = cloudStorageAccount.CreateCloudBlobClient();
            this.cloudBlobContainer  = blobClient.GetContainerReference("content");
        }

        private CloudStorageAccount cloudStorageAccount;
        private CloudBlobClient     blobClient;
        private CloudBlobContainer  cloudBlobContainer;
        public string Save(string soundName, byte[] soundContent, int relatedId, string baseSavePath)
        {
            var blobSavePath = string.Format("{0}/{1}/{2}", baseSavePath, relatedId, soundName);
            var blockBlob    = cloudBlobContainer.GetBlockBlobReference(blobSavePath);

            blockBlob.Properties.ContentType = MimeMapping.GetMimeMapping(soundName);

            using (var stream = new MemoryStream(soundContent))
            {
                blockBlob.UploadFromStream(stream);
            }

            return blockBlob.Uri.ToString();
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
            ILoginServices loginServices
            )
        {
            this.loginServices = loginServices;
        }

        private readonly ILoginServices loginServices;

        public bool CanCreateTask(int testId)
        {
            var currentUser = loginServices.GetSignedInUser();
            if(currentUser == null)
            {
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