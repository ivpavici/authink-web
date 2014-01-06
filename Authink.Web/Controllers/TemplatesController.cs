using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authink.Web.Controllers
{
    public class TemplatesController : Controller
    {
        [HttpGet] public ActionResult GetTemplate(string template)
        {
            return
                  (template == "childMenu")                ? PartialView("~/Views/Templates/ChildMenu.cshtml")
                : (template == "createChild")              ? PartialView("~/Views/Templates/CreateChild.cshtml")
                : (template == "createTest")               ? PartialView("~/Views/Templates/CreateTest.cshtml")
                : (template == "editChild")                ? PartialView("~/Views/Templates/EditChild.cshtml")
                : (template == "editTask")                 ? PartialView("~/Views/Templates/EditTask.cshtml")
                : (template == "editTaskPicturesList")     ? PartialView("~/Views/Templates/EditTaskPicturesList.cshtml")
                : (template == "editTest")                 ? PartialView("~/Views/Templates/EditTest.cshtml")
                : (template == "omniBar")                  ? PartialView("~/Views/Templates/Omnibar.cshtml")
                : (template == "signUp" )                  ? PartialView("~/Views/Templates/SignUp.cshtml")
                : (template == "taskPicturesEditor")       ? PartialView("~/Views/Templates/TaskPicturesEditor.cshtml")
                : (template == "taskPreview")              ? PartialView("~/Views/Templates/TaskPreview.cshtml")
                : (template == "testPreview")              ? PartialView("~/Views/Templates/TestPreview.cshtml")
                : (template == "testsList")                ? PartialView("~/Views/Templates/TestsList.cshtml")
                : (template == "taskDeleteConfirmDialog")  ? PartialView("~/Views/Templates/TaskDelete_ConfirmationDialog.cshtml")
                : (template == "testDeleteConfirmDialog")  ? PartialView("~/Views/Templates/TestDelete_ConfirmationDialog.cshtml")
                : (template == "testEditConfirmDialog")    ? PartialView("~/Views/Templates/TestEditCancel_ConfirmationDialog.cshtml")
                : (template == "childDeleteConfirmDialog") ? PartialView("~/Views/Templates/ChildDelete_ConfirmationDialog.cshtml")
                : (template == "login")                    ? PartialView("~/Views/Templates/Login.cshtml")
                : (template == "home")                     ? PartialView("~/Views/Templates/Home.cshtml")
                : (template == "passwordreset")            ? PartialView("~/Views/Templates/PasswordReset.cshtml")
                :                                            PartialView("~/Views/Templates/TestTasksList.cshtml");
        }
    }
}
