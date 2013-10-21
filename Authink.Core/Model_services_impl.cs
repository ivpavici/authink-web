using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;

using Authink.Core.Model.Queries;
using ImageResizer;

using ent      = Authink.Core.Domain.Entities;
using buru     = Authink.Core.Domain.Rules;
using database = Authink.Data;

namespace Authink.Core.Model.Services.Impl
{
    public class FileSystemUtilities : IFileSystemUtilities
    {
        public byte[] Transform_HttpPostedFileBase_Into_Bytes(HttpPostedFileBase file)
        {
            var buffer = new byte[file.ContentLength];
            file.InputStream.Read
            (
                buffer: buffer,
                offset: 0,
                count:  file.ContentLength
            );

            return buffer;
        }
        public string Build_SavePath_And_CreateFolderIfNecessary(int relatedId, string defaultSavePath, string fileName)
        {
            var folderPath = string.Format("{0}{1}{2}", AppDomain.CurrentDomain.BaseDirectory, defaultSavePath, relatedId);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var uniqueName = Guid.NewGuid();
            var extenstion = Path.GetExtension(fileName);

            return string.Format("{0}{1}/{2}{3}", defaultSavePath, relatedId, uniqueName, extenstion);
        }
    }
    public class PictureServicesImpl :  IPictureServices
    {
        public PictureServicesImpl
        (
            IFileSystemUtilities fileSystemUtilities
        )
        {
            this.fileSystemUtilities = fileSystemUtilities;
        }

        private readonly IFileSystemUtilities fileSystemUtilities;

        public string SaveToFileSystem(string pictureName,byte[] pictureContent, int relatedId, string baseSavePath, string resizeQueryString)
        {
            var savePath = fileSystemUtilities.Build_SavePath_And_CreateFolderIfNecessary
            (
                relatedId:       relatedId,
                defaultSavePath: baseSavePath,
                fileName:        pictureName
            );

            var resizedPicture = ResizePicture
            (
                pictureData:       pictureContent,
                resizeQuerystring: resizeQueryString
            );

            File.WriteAllBytes
            (
                path:  Path.Combine(HttpContext.Current.Server.MapPath("~/"), savePath),
                bytes: resizedPicture
            );

            return savePath;
        }

        public void SaveToFileSystem(byte[] pictureContent, string savePath, string resizeQueryString)
        {
            var resizedPicture = ResizePicture
            (
                pictureData:       pictureContent,
                resizeQuerystring: resizeQueryString
            );

            File.WriteAllBytes
            (
                path:  Path.Combine(HttpContext.Current.Server.MapPath("~/"), savePath),
                bytes: resizedPicture
            );
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
    public class SoundServicesImpl : FileSystemUtilities, ISoundServices
    {
        public SoundServicesImpl
        (
             IFileSystemUtilities fileSystemUtilities
        )
        {
            this.fileSystemUtilities = fileSystemUtilities;
        }

        private readonly IFileSystemUtilities fileSystemUtilities;

        public string SaveToFileSystem(string soundName, byte[] soundContent, int relatedId, string baseSavePath)
        {
            var savePath = fileSystemUtilities.Build_SavePath_And_CreateFolderIfNecessary
            (
                relatedId:       relatedId,
                defaultSavePath: baseSavePath,
                fileName:        soundName
            );

            File.WriteAllBytes
            (
                path:  Path.Combine(HttpContext.Current.Server.MapPath("~/"), savePath),
                bytes: soundContent
            );

            return savePath;
        }
        public void   SaveToFileSystem(byte[] soundContent, string savePath)
        {
            File.WriteAllBytes
            (
                path:  Path.Combine(HttpContext.Current.Server.MapPath("~/"), savePath),
                bytes: soundContent
            );
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