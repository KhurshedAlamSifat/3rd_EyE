using _3rdEyE.ManagingTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class DashBoardsController : BaseController
    {
        public ActionResult ChartJS()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }

        public ActionResult Morris()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }

        public ActionResult Flot()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }

        public ActionResult InLineChart()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }
    }
}