using _3rdEyE.ManagingTools;
using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class GPS_DeviceController : BaseController
    {
        Dictionary<string, string> GpsDeviceModelDict = new Dictionary<string, string> { { "Meitrack T1", "Meitrack T1" }, { "Zenda VT1", "Zenda VT1" }, { "Meitrack T366", "Meitrack T366" } };

        //# GPS_DeviceExisting_Create
        public ActionResult GPS_DeviceExisting_Create()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.GpsDeviceModelDict = new SelectList(GpsDeviceModelDict, "Key", "Value");
            return View();
        }
        [HttpPost]
        public ActionResult GPS_DeviceExisting_Create(GPS_DeviceExisting model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            model.GpsIMEINumber = model.GpsIMEINumber.ToUpper();
            var existing = bll.db.GPS_DeviceExisting.Where(m => m.GpsIMEINumber == model.GpsIMEINumber).FirstOrDefault();
            if (existing != null)
            {
                var message = "IMEI: " + existing.GpsIMEINumber + " Mdoel: " + existing.GpsDeviceModel + " already exist.";
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", message);
            }
            else
            {
                model.CreatedAt = DateTime.Now;
                model.FK_AppUser_CreatetBy = CurrentUser.PK_User;
                bll.db.GPS_DeviceExisting.Add(model);
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Successfully added.");
            }
            return RedirectToAction("GPS_DeviceExisting_Create");
        }
    }
}
