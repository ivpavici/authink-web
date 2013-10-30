using System;
using System.Web;
using System.Collections.Generic;

using ent = Authink.Core.Domain.Entities;

namespace Authink.Core.Model.Queries
{
    public interface IUserQueries
    {
        ent::User.ShortDetails GetSingle_whereUsername           (string userName                 );
        ent::User.ShortDetails GetSingle_whereUsernameAndPassword(string username, string password);
        ent::User.ShortDetails GetSingle_whereEmail              (string email                    );
    }
    public interface IChildQueries
    {
        IReadOnlyList<ent::Child.ShortDetails> GetAll_paged   (bool showHidden, int page  );
        IReadOnlyList<ent::Child.ShortDetails> GetAll_forUser (bool showHidden, int userId);

        ent::Child.ShortDetails GetSingle_whereId(int id);
        ent::Child.ShortDetails GetSingle_whereUsername(string username);
        ent::Child.ShortDetails GetSingle_whereUsernameAndPassword(string username, string password);
    }
    public interface ITaskQueries
    {
        IReadOnlyList<ent::Task.ShortDetails> GetAll_shortDetails_whereTestId(int testId);
       
        ent::Task.LongDetails GetSingle_longDetails_whereId(int id);
    }
    public interface ITestQueries
    {
        IReadOnlyList<ent::Test.ShortDetails> GetAll_forChild(bool showHidden, int childId);

        ent::Test.ShortDetails GetTest_thatContainsTask       (int taskId);
        ent::Test.ShortDetails GetSingle_ShortDetails_WhereId (int id);
        ent::Test.LongDetails  GetSingle_LongDetails_WhereId  (int id);
    }
    public interface IPictureQueries
    {
        IReadOnlyList<ent::Picture>         GetAll_forTaskGameplay  (int  taskId    );
        IReadOnlyList<ent::Color.Details>   GetAll_colorsForPicture (int  pictureId );

        ent::Picture         GetSingle_whereId       (int id     );
        ent::Color.Details   GetSingle_color_WhereId (int colorId);
    }
    public interface ISoundQueries
    {
        ent::Sound.Details                GetSingle_byTaskId(int taskId);

        ent::Sound.Details GetSingle_whereId    (int id       );
        ent::Sound.Details GetSingle_forPicture (int pictureId);
    }
    public interface IStatisticsQueries
    {
        ent::Statistics.Meta GetStatistics_Meta_ForTest(int testId);
        ent::Statistics.Meta GetStatistics_Meta_ForTask(int taskId);
    }
}
namespace Authink.Core.Model.Commands
{
    public interface IUserCommands
    {
        void Create(string username, string password, string firstname, string lastname, string email                       );
    }
    public interface ITokenCommands
    {
        string Create_pwdReset(int userId  );
        bool   Use_pwdReset   (string value);
    }
    public interface IChildCommands
    {
        int  Create(string firstname, string lastname                                  );
        void Update(int id, string firstname, string lastname);
        void Delete(int id);
        void UpdatePicture(int id, string profilePictureUrl);
    }
    public interface ITaskCommands
    {
        int  Create            (string description, string name, int userId, string type, int difficulty, string profilePictureUrl, ent::Sound.Details voiceCommand);
        void Update            (int id, string description, string name);
        void Delete            (int id);

        void ToggleAttachToTest(int taskId,int testId);
    }
    public interface ITestCommands
    {
        int  Create (string name, string shortDescription, string longDescription, int userId, int childId);
        void Update (int id, string name, string shortDescription, string longDescription                 );
        void Delete (int id);

        void ToggleAttachToChild(int childId, int testId);
    }
    public interface IPictureCommands
    {
        int  Create (string url, string theme, bool? isAnswer);
        void Update (int id, string url                      );
        void Delete (int id);

        void AttachToTask (int taskId, int pictureId);

        int  Create_color (string value, int pictureId, bool isCorrect);
        void Update_color (int id, string value                       );

        void AttachColorToPicture(int pictureId, int colorId);
    }
    public interface ISoundCommands
    {
        int  Create (string url, string title,string type                        );
        void Update (int id, string url, bool isHidden, string title, string type);
        void Delete (int id );

        void AttachSoundToPicture(int soundId, int pictureId                );
        void AttachSoundToTask   (int soundId, int taskId                   );
    }
    public interface IStatisticsCommands
    {
        void Update_ForTask(int taskId, int sucessfullClicksCount, int errorClicksCount, string timeRun, DateTime date);
    }
}
namespace Authink.Core.Model.Services
{
    public interface IFileSystemUtilities
    {
        byte[] Transform_HttpPostedFileBase_Into_Bytes   (HttpPostedFileBase file                               );
        string Build_SavePath_And_CreateFolderIfNecessary(int relatedId, string defaultSavePath, string fileName);
        void CreateFolderForPictureIfNecessary(int relatedId, string defaultSavePath);
    }
    public interface IPictureServices
    {
        string SaveToFileSystem(string pictureName, byte[] pictureContent, int relatedId, string baseSavePath, string resizeQueryString);
        void   SaveToFileSystem(byte[] pictureContent, string savePath, string resizeQueryString);
        void ResizePicture(string pictureUrl, string resizeQuerystring);
    }
    public interface ISoundServices
    {
        string SaveToFileSystem(string soundName, byte[] soundContent, int relatedId, string baseSavePath);
        void   SaveToFileSystem(byte[] soundContent, string savePath);
    }
    public interface ILoginServices
    {
        ent::User.ShortDetails GetSignedInUser();

        bool Login  (string username, string password   );
        void SignIn (string username, bool isSessionLong);
        void SignOut();
    }
    public interface IUserAccessRights
    {
        bool CanReadTask  (int taskId);
        bool CanCreateTask(int testId);
        bool CanEditTask  (int taskId);

        bool CanReadTest  (int testId );
        bool CanCreateTest(int childId);
        bool CanEditTest  (int testId );

        bool CanEditPicture(int pictureId);
        bool CanEditSound  (int soundId  );

        bool CanReadChild(int childId);
        bool CanEditChild(int childId);
    }
}


