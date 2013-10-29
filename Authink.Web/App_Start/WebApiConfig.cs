using System.Web.Http;

namespace Authink.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("UserLogin", "api/login/user", new { controller = "ApiEndPoint", action = "Login" });

            config.Routes.MapHttpRoute("GetChildren_forUser", "api/users/children/{user_userName}", new { controller = "ApiEndPoint", action = "GetChildren_forUser" });

            config.Routes.MapHttpRoute("Save_Meta_Statistics_forTask", "api/statistics/task", new { controller = "ApiEndPoint", action = "Save_Meta_Statistics_forTask" });
            
            config.Routes.MapHttpRoute("TestsApi_GetAllTestsForChild_shortDetails", "api/children/{childId}/tests", new { controller = "TestsApi", action = "GetAllTestsForChild_shortDetails" });
            config.Routes.MapHttpRoute("TestsApi_Create", "api/tests/create",                                       new { controller = "TestsApi", action = "Create"                           });
            config.Routes.MapHttpRoute("TestsApi_Edit", "api/tests/edit",                                           new { controller = "TestsApi", action = "Edit"                             });
            config.Routes.MapHttpRoute("TestsApi_Delete", "api/tests/delete/{testId}",                              new { controller = "TestsApi", action = "Delete"                           });
            config.Routes.MapHttpRoute("TestsApi_GetOne_longDetails", "api/tests/{testId}",                         new { controller = "TestsApi", action = "GetOne_longDetails"               });
            
            config.Routes.MapHttpRoute("PicturesApi_InsertPictureForUpdate", "api/pictures/update/{pictureId}/{taskId}", new { controller = "PicturesApi", action = "InsertPictureForUpdate" });
            config.Routes.MapHttpRoute("PicturesApi_GetAll_forTaskGameplay", "api/task/{taskId}/pictures",               new { controller = "PicturesApi", action = "GetAll_forTaskGameplay" });

            config.Routes.MapHttpRoute("TasksApi_GetSingle_whereId", "api/tasks/{taskId}", new { controller = "TasksApi", action = "GetSingle_whereId" });
            config.Routes.MapHttpRoute("TasksApi_GetAll_shortDetails_whereTestId", "api/test/{testId}/tasks", new { controller = "TasksApi", action = "GetAll_shortDetails_whereTestId" });
            config.Routes.MapHttpRoute("TasksApi_Update", "api/task/update", new { controller = "TasksApi", action = "Update" });

            config.Routes.MapHttpRoute("ChildrenApi_Create", "api/children/create",                 new { controller = "ChildrenApi", action = "Create" });
            config.Routes.MapHttpRoute("ChildrenApi_Edit", "api/children/edit",                     new { controller = "ChildrenApi", action = "Edit"   });
            config.Routes.MapHttpRoute("ChildrenApi_GetOne_shortDetails", "api/children/{childId}", new { controller = "ChildrenApi", action = "GetOne_shortDetails"        });
            config.Routes.MapHttpRoute("ChildrenApi_GetAllForUser_shortDetails", "api/children",    new { controller = "ChildrenApi", action = "GetAllForUser_shortDetails" });

            config.Routes.MapHttpRoute("UserApi_Login", "api/user/login",      new { controller = "UserApi", action = "Login"   });
            config.Routes.MapHttpRoute("UserApi_SignUp", "api/user/sign-up",   new { controller = "UserApi", action = "SignUp"  });
            config.Routes.MapHttpRoute("UserApi_SignOut", "api/user/sign-out", new { controller = "UserApi", action = "SignOut" });
            
            config.Routes.MapHttpRoute("UserApi_TryGetSignedInUser", "api/user", new { controller = "UserApi", action = "TryGetSignedInUser" });
        }
    }
}
