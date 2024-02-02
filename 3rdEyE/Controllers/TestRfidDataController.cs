using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class TestRfidDataController : Controller
    {
        public DBEnityModelContainer db = new DBEnityModelContainer();

        //http://localhost:1025/TestRfidData/Push?DeviceId=0&RegistrationNumber=DHAKA%20METRO-DA-11-6705&InOrOut=true
        public JsonResult Push(int DeviceId, string RegistrationNumber, bool InOrOut)
        {
            try
            {
                var model = new TestRfidData();
                model.DeviceId = DeviceId;
                model.RegistrationNumber = RegistrationNumber;
                model.InOrOut = InOrOut;
                model.CreatedAt = DateTime.Now;
                db.TestRfidDatas.Add(model);
                db.SaveChanges();

                return Json(new { status = "OK", DeviceId, RegistrationNumber, InOrOut }, JsonRequestBehavior.AllowGet);
                //{"status":"OK","DeviceId":0,"RegistrationNumber":"DHAKA METRO-DA-11-6705","InOrOut":true}
            }
            catch (Exception e)
            {
                return Json(new { status = "Err", DeviceId, RegistrationNumber, InOrOut }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Pull(int count = 100)
        {
            var list = db.TestRfidDatas.OrderByDescending(m => m.PK_TestRfidData).Take(count).ToList();
            return View(list);
        }

    }
}