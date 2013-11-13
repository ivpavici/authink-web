using System.Web.Mvc;
using System.Web.Routing;

namespace Authink.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("GetTemplate", "application/templates/{template}", new { controller = "Templates", action = "GetTemplate" });

            routes.MapRoute("Login", "login",  new { controller = "Shell", action = "Login"});
            routes.MapRoute("Shell", "",       new { controller = "Shell", action = "Shell"});

            routes.MapRoute("GetStatisticsForTask", "ajax/statistics/task/{taskId}", new { controller = "Statistics", action = "GetForTask", taskId = UrlParameter.Optional });
            
            routes.MapRoute("ChooseType",       "test/{testId}/tasks/create/step-1",   new { controller = "Task", action = "ChooseType"       });
            routes.MapRoute("ChooseDifficulty", "test/tasks/create/step-2/{taskType}", new { controller = "Task", action = "ChooseDifficulty" });
            routes.MapRoute("InputMetaData",    "test/tasks/create/step-4",            new { controller = "Task", action = "InputMetaData"    });
            routes.MapRoute("Feedback",         "test/tasks/create/finished",          new { controller = "Task", action = "Feedback"         });

            routes.MapRoute("Cancel", "wizard/cancel", new { controller = "Task", action = "Cancel" });

            routes.MapRoute("DeleteTask", "ajax/task/delete/{taskId}", new { controller = "Task", action = "Delete", taskId = UrlParameter.Optional});
            routes.MapRoute("ReadTask",   "task/{taskId}",             new { controller = "Task", action = "Read"                                 });

            routes.MapRoute("UploadPictures_SimpleTasksWithPictures", "tasks/create/step-3/items/difficulty-{taskDifficulty}/pictures",            new { controller = "Picture", action = "UploadPictures_SimpleTasksWithPictures" });
            routes.MapRoute("UploadPictures_DetectColors",            "tasks/create/step-3/colors/difficulty-{taskDifficulty}/pictures",           new { controller = "Picture", action = "UploadPictures_DetectColors" });
            routes.MapRoute("UploadPictures_DetectItem",              "tasks/create/step-3/same-items/difficulty-{taskDifficulty}/pictures",       new { controller = "Picture", action = "UploadPictures_DetectItem" });
            routes.MapRoute("UploadPictures_OrderBySize",             "tasks/create/step-3/order-by-size/difficulty-{taskDifficulty}/pictures",    new { controller = "Picture", action = "UploadPictures_OrderBySize" });

            routes.MapRoute("EditTask", "tasks/{taskId}/edit", new { controller = "Shell", action = "Shell", taskId = UrlParameter.Optional });
                                                                                                                                       
            routes.MapRoute("EditPicture", "ajax/picture-edit/{taskId}/{pictureId}", new { controller = "Picture", action = "Edit", pictureId = UrlParameter.Optional });

            routes.MapRoute("Picture_Edit_simple",     "pictures/edit-simple/{pictureId}",     new { controller = "Picture", action = "Edit_simple",     pictureId = UrlParameter.Optional });
            routes.MapRoute("Picture_Edit_withColors", "ajax/pictures/edit-withcolors/{pictureId}", new { controller = "Picture", action = "Edit_withColors", pictureId = UrlParameter.Optional });

            routes.MapRoute("CreateSound", "ajax/sounds/create/{taskId}", new { controller = "Sound", action = "Create", taskId  = UrlParameter.Optional });
            routes.MapRoute("EditSound",   "ajax/sounds/edit/{soundId}",  new { controller = "Sound", action = "Edit",   soundId = UrlParameter.Optional });
        }
    }
}