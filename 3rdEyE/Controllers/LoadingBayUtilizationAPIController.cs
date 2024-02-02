using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.Models;
using _3rdEyE.BLL;
//using _3rdEyE.ManagingTools;
using System.Threading;
using System.Threading.Tasks;
using static _3rdEyE.ManagingTools.CommonClass;
using System.IO;
using System.Web.Script.Serialization;
using _3rdEyE.ManagingTools;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace _3rdEyE.Controllers
{
    public class LoadingBayUtilizationAPIController : BaseAPIController
    {
        #region LoadUnload-Start

        public class LoadUnloadStartModel
        {
            public string AppVersionCode { get; set; }
            public string DeviceId { get; set; }
            public Guid FK_StartByUser { get; set; }
            public Guid FK_Vehicle { get; set; }
            public Int64 FK_LoadingBay { get; set; }
            public Nullable<Int64> FK_ParkingInOut { get; set; }

        }
        public JsonResult FindVehicle(Guid PK_User, string RegistrationNumber)
        {
            var flag = "n/a";
            try
            {
                var CurrentUser = db.AppUsers.Where(m => m.PK_User == PK_User).FirstOrDefault();
                var VehicleList = (from vehicle in db.Vehicles.Where(m => m.RegistrationNumber.Contains(RegistrationNumber) && m.FK_LocationInOut == CurrentUser.FK_Location && m.LocationInOrOut == true 
                                   && (m.ParkingInOut != null
                                        && m.ParkingInOut.FactoryIn_IssueDateTime != null && m.ParkingInOut.FactoryOut_IssueDateTime == null
                                        && m.ParkingInOut.BayAssign_FK_LocationBuilding != null && m.ParkingInOut.BayAssign_FK_LoadingBay != null)
                                    )
                                   select new
                                   {
                                       FK_Vehicle = vehicle.PK_Vehicle,
                                       vehicle.RegistrationNumber,
                                       FK_ParkingInOut = vehicle.ParkingInOut.PK_ParkingInOut,
                                       FK_LocationBuilding = vehicle.ParkingInOut.BayAssign_FK_LocationBuilding,
                                       FK_LoadingBay = vehicle.ParkingInOut.BayAssign_FK_LoadingBay
                                   }).ToList();
                flag = "found";
                return Json(new { flag, VehicleList = VehicleList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                flag = "internal_error";
                return Json(new { flag }, JsonRequestBehavior.AllowGet);
            }
        }
        
        //#Version9/+
        public JsonResult GetLoadUnloadStart_Utilities(Guid PK_User)
        {
            var flag = "n/a";
            try
            {
                var CurrentUser = db.AppUsers.Where(m => m.PK_User == PK_User).FirstOrDefault();
                var LocationBuildingList = db.LocationBuildings.Where(m => m.IsDeleted != true && m.FK_Location == CurrentUser.FK_Location && m.PRG_Type == CurrentUser.PRG_Type).Select(m => new
                {
                    m.PK_LocationBuilding,
                    LocationBuildingName = m.Name
                }).ToList();

                var LoadingBayList = db.LoadingBays.Where(m => m.IsDeleted != true && m.LocationBuilding.FK_Location == CurrentUser.FK_Location && m.LocationBuilding.PRG_Type == CurrentUser.PRG_Type && (m.CurrentUseType == null || m.CurrentUseType == "Empty")).Select(m => new
                {
                    m.PK_LoadingBay,
                    m.FK_LocationBuilding,
                    LoadingBayName = m.Name
                }).ToList();
                flag = "found";

                return Json(new { flag, LocationBuildingList = LocationBuildingList, LoadingBayList = LoadingBayList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                flag = "internal_error";
                return Json(new { flag }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetLoadUnloadStart_Utilities_Version7(Guid PK_User)
        {
            var flag = "n/a";
            try
            {
                var CurrentUser = db.AppUsers.Where(m => m.PK_User == PK_User).FirstOrDefault();
                var LocationBuildingList = db.LocationBuildings.Where(m => m.IsDeleted != true && m.FK_Location == CurrentUser.FK_Location && m.PRG_Type == CurrentUser.PRG_Type).Select(m => new
                {
                    m.PK_LocationBuilding,
                    LocationBuildingName = m.Name
                }).ToList();

                var LoadingBayList = db.LoadingBays.Where(m => m.IsDeleted != true && m.LocationBuilding.FK_Location == CurrentUser.FK_Location && m.LocationBuilding.PRG_Type == CurrentUser.PRG_Type && (m.CurrentUseType == null || m.CurrentUseType == "Empty")).Select(m => new
                {
                    m.PK_LoadingBay,
                    m.FK_LocationBuilding,
                    LoadingBayName = m.Name
                }).ToList();
                flag = "found";

                return Json(new { flag, LocationBuildingList = LocationBuildingList, LoadingBayList = LoadingBayList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                flag = "internal_error";
                return Json(new { flag }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult StartLoading(LoadUnloadStartModel request)
        {
            var CurrentUser = db.AppUsers.Where(m => m.PK_User == request.FK_StartByUser).FirstOrDefault();

            var AppVersionCode_Current = db.AppSettings.Where(m => m.Name == "AppVersionCode_Current" && m.IsActive == true).Select(m => m.Value).FirstOrDefault();
            var AppVersionCode_OldUsable = db.AppSettings.Where(m => m.Name == "AppVersionCode_OldUsable" && m.IsActive == true).Select(m => m.Value).ToList();

            var flag = "n/a";
            var message = "";
            if (!(request.AppVersionCode == AppVersionCode_Current || AppVersionCode_OldUsable.Contains(request.AppVersionCode)))
            {
                flag = "validation_failed";
                message = "বিফল হয়েছে। সফটয়ারটি আপডেট না করলে এন্ট্রী দেওয়া যাবে না। শীঘ্রই ট্রান্সপোর্ট ডিপার্ট্মেন্টকে জানান।";
                return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var model = new LoadingBayUtilization();
                model.UseType = "Load";
                model.StartDateTime = DateTime.Now;

                //#Conversion
                model.FK_AppUser_StartedBy = request.FK_StartByUser;
                model.DeviceId_Started = request.DeviceId;
                model.FK_Vehicle = request.FK_Vehicle;
                model.FK_LoadingBay = request.FK_LoadingBay;
                model.FK_ParkingInOut = request.FK_ParkingInOut;

                //FK_Vehicle
                var vehicle = db.Vehicles.Where(m => m.PK_Vehicle == model.FK_Vehicle).FirstOrDefault();
                model.OWN_MHT_DHT = vehicle.OWN_MHT_DHT;
                if (vehicle.FK_LoadingBayUtilization_Last != null)
                {
                    flag = "validation_failed";
                    var _conflictingLoadingBay = vehicle.LoadingBayUtilization.LoadingBay;
                    message = vehicle.RegistrationNumber + " গাড়িটি বে নাম্বারঃ" + vehicle.LoadingBayUtilization.LoadingBay.Name + "-তে আছে।";
                    return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                }

                //FK_LoadingBay
                var loadingBay = db.LoadingBays.Where(m => m.PK_LoadingBay == model.FK_LoadingBay).FirstOrDefault();
                if (loadingBay.CurrentUseType == null || loadingBay.CurrentUseType == "Empty")
                {
                    var emptyLoadingBayUtilization = loadingBay.LoadingBayUtilization;
                    if (emptyLoadingBayUtilization != null)
                    {
                        emptyLoadingBayUtilization.EndDateTime = model.StartDateTime;
                        emptyLoadingBayUtilization.FK_AppUser_EndedBy = model.FK_AppUser_StartedBy;
                        try { emptyLoadingBayUtilization.StayTimeMinute = (long)model.StartDateTime.Subtract(emptyLoadingBayUtilization.StartDateTime).TotalMinutes; } catch (Exception) {/*do nothing*/}
                    }
                }
                else
                {
                    flag = "validation_failed";
                    var _conflictingLoadingBay = loadingBay.LoadingBayUtilization.Vehicle;
                    message = "বে নাম্বারঃ" + loadingBay.Name + "-তে " + loadingBay.LoadingBayUtilization.Vehicle.RegistrationNumber + " গাড়িটি আছে।";
                    return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                }

                db.LoadingBayUtilizations.Add(model);
                vehicle.FK_LoadingBayUtilization_Last = model.PK_LoadingBayUtilization;
                loadingBay.FK_LoadingBayUtilization_Last = model.PK_LoadingBayUtilization;
                loadingBay.CurrentUseType = "Load";
                db.SaveChanges();

                //ParkingInOut
                if (vehicle.FK_ParkingInOut_Last != null)
                {
                    var parkingInOut = db.ParkingInOuts.Where(m => m.PK_ParkingInOut == vehicle.FK_ParkingInOut_Last).FirstOrDefault();
                    if (parkingInOut != null)
                    {
                        parkingInOut.FK_LoadingBayUtilization = model.PK_LoadingBayUtilization;
                    }
                }

                db.SaveChanges();
                flag = "success";
                message = "এন্ট্রি সফল হয়েছে।";
                return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var error_message = e.Message;
                while (e.InnerException != null) { e = e.InnerException; error_message = e.Message; }
                db.AppErrorLogs.Add(
                    new AppErrorLog()
                    {
                        ErrorMessage = error_message,
                        ErrorTime = DateTime.Now,
                        //UserDefinedMessage = "VehicleGateNewAPI/GateIn FK_AppUser:" + model.FK_CreatedByUser
                    }
                    );
                db.SaveChanges();

                flag = "internal_error";
                message = "বিফল হয়েছে। MIS ডিপার্ট্মেন্টকে জানান(থার্ড আই সার্ভার সমস্যা)।";
                return Json(new { flag, message, error_message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult StartUnloading(LoadUnloadStartModel request)
        {
            var AppVersionCode_Current = db.AppSettings.Where(m => m.Name == "AppVersionCode_Current" && m.IsActive == true).Select(m => m.Value).FirstOrDefault();
            var AppVersionCode_OldUsable = db.AppSettings.Where(m => m.Name == "AppVersionCode_OldUsable" && m.IsActive == true).Select(m => m.Value).ToList();

            var flag = "n/a";
            var message = "";
            if (!(request.AppVersionCode == AppVersionCode_Current || AppVersionCode_OldUsable.Contains(request.AppVersionCode)))
            {
                flag = "validation_failed";
                message = "বিফল হয়েছে। সফটয়ারটি আপডেট না করলে এন্ট্রী দেওয়া যাবে না। শীঘ্রই ট্রান্সপোর্ট ডিপার্ট্মেন্টকে জানান।";
                return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var model = new LoadingBayUtilization();
                model.UseType = "Unload";
                model.StartDateTime = DateTime.Now;

                //#Conversion
                model.FK_AppUser_StartedBy = request.FK_StartByUser;
                model.DeviceId_Started = request.DeviceId;
                model.FK_Vehicle = request.FK_Vehicle;
                model.FK_LoadingBay = request.FK_LoadingBay;
                model.FK_ParkingInOut = request.FK_ParkingInOut;

                //FK_Vehicle
                var vehicle = db.Vehicles.Where(m => m.PK_Vehicle == model.FK_Vehicle).FirstOrDefault();
                model.OWN_MHT_DHT = vehicle.OWN_MHT_DHT;
                if (vehicle.FK_LoadingBayUtilization_Last != null)
                {
                    flag = "validation_failed";
                    var _conflictingLoadingBay = vehicle.LoadingBayUtilization.LoadingBay;
                    message = vehicle.RegistrationNumber + " গাড়িটি বে নাম্বারঃ" + vehicle.LoadingBayUtilization.LoadingBay.Name + "-তে আছে।";
                    return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                }

                //FK_LoadingBay
                var loadingBay = db.LoadingBays.Where(m => m.PK_LoadingBay == model.FK_LoadingBay).FirstOrDefault();
                if (loadingBay.CurrentUseType == null || loadingBay.CurrentUseType == "Empty")
                {
                    var emptyLoadingBayUtilization = loadingBay.LoadingBayUtilization;
                    if (emptyLoadingBayUtilization != null)
                    {
                        emptyLoadingBayUtilization.EndDateTime = model.StartDateTime;
                        emptyLoadingBayUtilization.FK_AppUser_EndedBy = model.FK_AppUser_StartedBy;
                        try { emptyLoadingBayUtilization.StayTimeMinute = (long)model.StartDateTime.Subtract(emptyLoadingBayUtilization.StartDateTime).TotalMinutes; } catch (Exception) {/*do nothing*/}
                    }
                }
                else
                {
                    flag = "validation_failed";
                    var _conflictingLoadingBay = loadingBay.LoadingBayUtilization.Vehicle;
                    message = "বে নাম্বারঃ" + loadingBay.Name + "-তে " + loadingBay.LoadingBayUtilization.Vehicle.RegistrationNumber + " গাড়িটি আছে।";
                    return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                }

                db.LoadingBayUtilizations.Add(model);
                vehicle.FK_LoadingBayUtilization_Last = model.PK_LoadingBayUtilization;
                loadingBay.FK_LoadingBayUtilization_Last = model.PK_LoadingBayUtilization;
                loadingBay.CurrentUseType = "Unload";
                db.SaveChanges();

                //ParkingInOut
                if (vehicle.FK_ParkingInOut_Last != null)
                {
                    var parkingInOut = db.ParkingInOuts.Where(m => m.PK_ParkingInOut == vehicle.FK_ParkingInOut_Last).FirstOrDefault();
                    if (parkingInOut != null)
                    {
                        parkingInOut.FK_LoadingBayUtilization = model.PK_LoadingBayUtilization;
                    }
                }

                db.SaveChanges();
                flag = "success";
                message = "এন্ট্রি সফল হয়েছে।";
                return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var error_message = e.Message;
                while (e.InnerException != null) { e = e.InnerException; error_message = e.Message; }
                db.AppErrorLogs.Add(
                    new AppErrorLog()
                    {
                        ErrorMessage = error_message,
                        ErrorTime = DateTime.Now,
                        //UserDefinedMessage = "VehicleGateNewAPI/GateIn FK_AppUser:" + model.FK_CreatedByUser
                    }
                    );
                db.SaveChanges();

                flag = "internal_error";
                message = "বিফল হয়েছে। MIS ডিপার্ট্মেন্টকে জানান(থার্ড আই সার্ভার সমস্যা)।";
                return Json(new { flag, message, error_message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region LoadUnload-End
        public class LoadUnloadEndModel
        {
            public string AppVersionCode { get; set; }
            public string DeviceId { get; set; }
            public Int64 PK_LoadingbayUtilization { get; set; }
            public Guid FK_EndByUser { get; set; }

        }
        public JsonResult GetVehiclesBeingUsed(Guid PK_User)
        {
            var flag = "n/a";
            try
            {
                var CurrentUser = db.AppUsers.Where(m => m.PK_User == PK_User).FirstOrDefault();
                var _VehicleList = db.LoadingBayUtilizations.Where(m => m.FK_AppUser_StartedBy == PK_User && (m.UseType == "Load" || m.UseType == "Unload") && m.EndDateTime == null).OrderBy(m => m.PK_LoadingBayUtilization).Select(m => new
                {
                    m.PK_LoadingBayUtilization,
                    m.Vehicle.RegistrationNumber,
                    LocationBuildingName = m.LoadingBay.LocationBuilding.Name,
                    LoadingBayName = m.LoadingBay.Name,
                    m.UseType,
                    m.StartDateTime
                }).OrderBy(m => m.StartDateTime).ToList();

                flag = "found";
                var VehicleList = _VehicleList.Select(m => new
                {
                    m.PK_LoadingBayUtilization,
                    m.RegistrationNumber,
                    m.LocationBuildingName,
                    m.LoadingBayName,
                    m.UseType,
                    StartDateTime = m.StartDateTime.ToString("dd-MM-yy hh:mm")
                }).ToList();
                return Json(new { flag, VehicleList = VehicleList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                flag = "internal_error";
                return Json(new { flag }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EndLoadingUnloading(LoadUnloadEndModel request)
        {
            var CurrentUser = db.AppUsers.Where(m => m.PK_User == request.FK_EndByUser).FirstOrDefault();

            var AppVersionCode_Current = db.AppSettings.Where(m => m.Name == "AppVersionCode_Current" && m.IsActive == true).Select(m => m.Value).FirstOrDefault();
            var AppVersionCode_OldUsable = db.AppSettings.Where(m => m.Name == "AppVersionCode_OldUsable" && m.IsActive == true).Select(m => m.Value).ToList();

            var flag = "n/a";
            var message = "";
            if (!(request.AppVersionCode == AppVersionCode_Current || AppVersionCode_OldUsable.Contains(request.AppVersionCode)))
            {
                flag = "validation_failed";
                message = "বিফল হয়েছে। সফটয়ারটি আপডেট না করলে এন্ট্রী দেওয়া যাবে না। শীঘ্রই ট্রান্সপোর্ট ডিপার্ট্মেন্টকে জানান।";
                return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                db.Database.CommandTimeout = 120;
                //# get current model and check
                var currentModel = db.LoadingBayUtilizations.Where(m => m.PK_LoadingBayUtilization == request.PK_LoadingbayUtilization).FirstOrDefault();
                if (currentModel.EndDateTime != null)
                {
                    flag = "validation_failed";
                    message = "বিফল হয়েছে। এই এন্ট্রী ইতিমধ্যে বন্ধ করা হয়ে গেছে।";
                    return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                }

                //# update the current model
                currentModel.EndDateTime = DateTime.Now;
                currentModel.FK_AppUser_EndedBy = CurrentUser.PK_User;
                currentModel.DeviceId_Ended = request.DeviceId;
                try { currentModel.StayTimeMinute = (long)((DateTime)currentModel.EndDateTime).Subtract(currentModel.StartDateTime).TotalMinutes; } catch (Exception) {/*do nothing*/}

                //# update the vehicle
                var vehicle = currentModel.Vehicle;
                vehicle.FK_LoadingBayUtilization_Last = null;

                //# create new model
                var newModel = new LoadingBayUtilization();
                newModel.UseType = "Empty";
                newModel.StartDateTime = (DateTime)currentModel.EndDateTime;
                newModel.FK_AppUser_StartedBy = (Guid)currentModel.FK_AppUser_EndedBy;
                newModel.FK_Vehicle = null;
                newModel.FK_LoadingBay = currentModel.FK_LoadingBay;
                newModel.FK_ParkingInOut = null;
                db.LoadingBayUtilizations.Add(newModel);

                //Update LoadingBay
                var loadingBay = currentModel.LoadingBay;
                loadingBay.FK_LoadingBayUtilization_Last = newModel.PK_LoadingBayUtilization;
                loadingBay.CurrentUseType = "Empty";

                db.SaveChanges();
                flag = "success";
                message = "এন্ট্রি সফল হয়েছে।";
                return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var error_message = e.Message;
                while (e.InnerException != null) { e = e.InnerException; error_message = e.Message; }
                db.AppErrorLogs.Add(
                    new AppErrorLog()
                    {
                        ErrorMessage = error_message,
                        ErrorTime = DateTime.Now,
                        //UserDefinedMessage = "VehicleGateNewAPI/GateIn FK_AppUser:" + model.FK_CreatedByUser
                    }
                    );
                db.SaveChanges();

                flag = "internal_error";
                message = "বিফল হয়েছে। MIS ডিপার্ট্মেন্টকে জানান(থার্ড আই সার্ভার সমস্যা)।";
                return Json(new { flag, message, error_message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        public ActionResult List()
        {
            var list = db.LoadingBayUtilizations.OrderByDescending(m => m.PK_LoadingBayUtilization).Take(20).ToList();
            return View(list);
        }
    }
}