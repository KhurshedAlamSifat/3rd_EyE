using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class DownloadController : Controller
    {
        // GET: Download
        public ActionResult Index()
        {
            return View();
        }
        public FileResult T366()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath("/__DD_Images/T366.rar")));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "T366.rar");
        }
    }
    

}