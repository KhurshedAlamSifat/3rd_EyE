using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.Models;
using _3rdEyE.ViewModels;
using _3rdEyE.BLL;
using _3rdEyE.ManagingTools;
using System.IO;

namespace _3rdEyE.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Basic()
        {
            CreateAlertMessage(AlertMessageType.Danger, "Danger", "Danger Danger Danger Danger");
            return View();
        }

        public ActionResult BasicForm()
        {
            return View();
        }

        public ActionResult BasicList()
        {
            return View();
        }


        public ActionResult Index()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }
        public ActionResult AccessDenyed()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }

    }
}