using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.BLL;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;
using Microsoft.Reporting.WebForms;
using System.Data.Entity;
using System.Diagnostics;

namespace _3rdEyE.Controllers
{
    public class _RoughController : BaseController
    {
        

        public ActionResult Page_1()
        {
            return View();
        }

        public ActionResult _index()
        {
            return View();
        }


    }
}