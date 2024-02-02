using _3rdEyE.ManagingTools;
using _3rdEyE.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class LoadingBayUtilizationController : BaseController
    {
        Dictionary<string, string> OWN_MHT_DHT_Dict = new Dictionary<string, string> { { "OWN", "OWN" }, { "MHT", "MHT" }, { "DHT", "DHT" } };
        Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" }, { "CS", "CS" } };
        Dictionary<string, string> MaxStayTimeHourDict = new Dictionary<string, string> { { "2", "2" }, { "4", "4" }, { "6", "6" }, { "8", "8" }, { "10", "10" }, { "12", "12" }, { "14", "14" }, { "16", "16" }, { "18", "18" }, { "20", "20" }, { "22", "22" }, { "24", "24" } };
        //VehicleInOutManual_Index
        public ActionResult LoadingBayUtilization_Index(string OWN_MHT_DHT, string PRG_Type, String FK_Location, String FK_LocationBuilding, string StartingDate, string EndingDate, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<LoadingBayUtilization> list = new List<LoadingBayUtilization>();

            var query = bll.db.LoadingBayUtilizations.Where(m => m.UseType != "Empty").AsQueryable();

            //OWN_MHT_DHTList
            if (OWN_MHT_DHT == null)
            {
                //Do Nothing
            }
            else if (OWN_MHT_DHT != "all")
            {
                query = query.Where(m => m.OWN_MHT_DHT == OWN_MHT_DHT);
            }
            List<SelectListItem> OWN_MHT_DHTList = new List<SelectListItem>();
            OWN_MHT_DHTList.Add(new SelectListItem() { Value = "all", Text = "All" });
            OWN_MHT_DHTList.AddRange(OWN_MHT_DHT_Dict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key, Text = m.Value }));
            ViewBag.OWN_MHT_DHT = new SelectList(OWN_MHT_DHTList.OrderBy(m => m.Text), "Value", "Text", OWN_MHT_DHT);

            //PRG_Type
            if (PRG_Type == null)
            {
                //Do Nothing
            }
            else if (PRG_Type != "all")
            {
                query = query.Where(m => m.LoadingBay.LocationBuilding.PRG_Type == PRG_Type);
            }
            List<SelectListItem> PRG_TypeList = new List<SelectListItem>();
            PRG_TypeList.Add(new SelectListItem() { Value = "all", Text = "All" });
            PRG_TypeList.AddRange(PRG_TypesDict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key, Text = m.Value }));
            ViewBag.PRG_Type = new SelectList(PRG_TypeList.OrderBy(m => m.Text), "Value", "Text", PRG_Type);

            //FK_Location
            if (FK_Location == null)
            {
                //Do Nothing
            }
            else if (FK_Location != "all")
            {
                var _FK_Location = Guid.Parse(FK_Location);

                query = query.Where(m => m.LoadingBay.LocationBuilding.FK_Location == _FK_Location);
            }
            List<SelectListItem> LocationList = new List<SelectListItem>();
            LocationList.Add(new SelectListItem() { Value = "all", Text = "All" });
            LocationList.AddRange(bll.db.Locations.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Location.ToString(), Text = m.Name }));
            ViewBag.Locations = new SelectList(LocationList.OrderBy(m => m.Text), "Value", "Text", FK_Location);

            //FK_LocationBuilding
            if (FK_LocationBuilding == null)
            {
                //Do Nothing
            }
            else if (FK_LocationBuilding != "all")
            {
                var _FK_LocationBuilding = Convert.ToInt64(FK_LocationBuilding);

                query = query.Where(m => m.LoadingBay.FK_LocationBuilding == _FK_LocationBuilding);
            }
            List<SelectListItem> LocationBuildingList = new List<SelectListItem>();
            LocationBuildingList.Add(new SelectListItem() { Value = "all", Text = "All" });
            LocationBuildingList.AddRange(bll.db.LocationBuildings.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_LocationBuilding.ToString(), Text = m.Name }));
            ViewBag.LocationBuildings = new SelectList(LocationBuildingList.OrderBy(m => m.Text), "Value", "Text", FK_LocationBuilding);

            //StartingDate EndingDate
            if (!string.IsNullOrEmpty(StartingDate) && !string.IsNullOrEmpty(EndingDate))
            {
                var _StartingDate = Convert.ToDateTime(StartingDate);
                var _EndingDate = Convert.ToDateTime(EndingDate);
                query = query.Where(m =>
                (_StartingDate < m.StartDateTime && m.EndDateTime < _EndingDate) || // [↓↑]
                (m.StartDateTime < _StartingDate && _StartingDate < m.EndDateTime) || // ↓[↑
                (m.StartDateTime < _EndingDate && (_EndingDate < m.EndDateTime || m.EndDateTime == null)) || // [↓]↑*
                (m.StartDateTime < _StartingDate && (_EndingDate < m.EndDateTime || m.EndDateTime == null))// ↓[]↑*
                );
            }
            ViewBag.StartingDate = StartingDate;
            ViewBag.EndingDate = EndingDate;


            ////StartingDate
            //if (!string.IsNullOrEmpty(StartingDate))
            //{
            //    var _StartingDate = Convert.ToDateTime(StartingDate);
            //    query = query.Where(m => (m.In_IssueDateTime >= _StartingDate) || (m.In_IssueDateTime < _StartingDate && m.Out_IssueDateTime == null));
            //}
            //ViewBag.StartingDate = StartingDate;

            ////EndingDate
            //if (!string.IsNullOrEmpty(EndingDate))
            //{
            //    var _EndingDate = Convert.ToDateTime(EndingDate);
            //    query = query.Where(m => m.In_IssueDateTime < _EndingDate);
            //}
            //ViewBag.EndingDate = EndingDate;

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.Vehicle.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //final
            if ((!string.IsNullOrEmpty(StartingDate) && !string.IsNullOrEmpty(EndingDate)) || !string.IsNullOrEmpty(RegistrationNumber))
            {
                list = query.OrderBy(m => m.PK_LoadingBayUtilization).ToList();
                //var endingDateLimit = DateTime.Now;
                //if (!string.IsNullOrEmpty(EndingDate))
                //{
                //    endingDateLimit = Convert.ToDateTime(EndingDate);
                //}
                var endingDateLimit = DateTime.Now;
                foreach (var item in list.Where(m => m.EndDateTime == null))
                {
                    item.StayTimeMinute = (long)(endingDateLimit - item.StartDateTime).TotalMinutes;
                    if (item.StayTimeMinute < 0)
                    {
                        item.StayTimeMinute = 0;
                    }
                }
            }

            return View(list);
        }
        public JsonResult GetLoadUnloadStart_Utilities()
        {
            var VehicleList = (from vehicle in bll.db.Vehicles.Where(m => m.FK_LocationInOut == CurrentUser.FK_Location && m.LocationInOrOut == true && m.VehicleInOutManual.AppUser.AppUserType == "Internal Gate Entry Device" && m.FK_LoadingBayUtilization_Last == null)
                               select new
                               {
                                   FK_Vehicle = vehicle.PK_Vehicle,
                                   vehicle.RegistrationNumber,
                                   FK_ParkingInOut = vehicle.FK_ParkingInOut_Last,
                                   FK_LocationBuilding = vehicle.ParkingInOut != null ? (vehicle.ParkingInOut.BayAssign_FK_LocationBuilding != null ? vehicle.ParkingInOut.BayAssign_FK_LocationBuilding : 0) : 0,
                                   FK_LoadingBay = vehicle.ParkingInOut != null ? (vehicle.ParkingInOut.BayAssign_FK_LoadingBay != null ? vehicle.ParkingInOut.BayAssign_FK_LoadingBay : 0) : 0,
                               }).ToList();
            var LocationBuildingList = bll.db.LocationBuildings.Where(m => m.IsDeleted != true && m.FK_Location == CurrentUser.FK_Location && m.PRG_Type == CurrentUser.PRG_Type).Select(m => new
            {
                m.PK_LocationBuilding,
                LocationBuildingName = m.Name
            }).ToList();
            var LoadingBayList = bll.db.LoadingBays.Where(m => m.IsDeleted != true && m.LocationBuilding.FK_Location == CurrentUser.FK_Location && m.LocationBuilding.PRG_Type == CurrentUser.PRG_Type && (m.CurrentUseType == null || m.CurrentUseType == "Empty")).Select(m => new
            {
                m.PK_LoadingBay,
                m.FK_LocationBuilding,
                LoadingBayName = m.Name
            }).ToList();
            return Json(new { VehicleList = VehicleList, LocationBuildingList = LocationBuildingList, LoadingBayList = LoadingBayList }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StartLoadingUnloading()
        {
            return View();
        }
        [HttpPost]
        public ActionResult StartLoadingUnloading(LoadingBayUtilization model)
        {
            var message = "";
            model.StartDateTime = DateTime.Now;

            //#Conversion
            model.FK_AppUser_StartedBy = CurrentUser.PK_User;
            model.DeviceId_Started = "Web";
            //model.FK_LoadingBay = request.FK_LoadingBay;
            model.FK_ParkingInOut = bll.db.Vehicles.Where(m => m.PK_Vehicle == model.FK_Vehicle).FirstOrDefault().FK_ParkingInOut_Last;

            //FK_Vehicle
            var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == model.FK_Vehicle).FirstOrDefault();
            model.OWN_MHT_DHT = vehicle.OWN_MHT_DHT;
            if (vehicle.FK_LoadingBayUtilization_Last != null)
            {
                //flag = "validation_failed";
                var _conflictingLoadingBay = vehicle.LoadingBayUtilization.LoadingBay;
                message = vehicle.RegistrationNumber + " গাড়িটি বে নাম্বারঃ" + vehicle.LoadingBayUtilization.LoadingBay.Name + "-তে আছে।";
                CreateAlertMessage(AlertMessageType.Warning, "Warning", message);
                return RedirectToAction("StartLoadingUnloading");
            }

            //FK_LoadingBay
            var loadingBay = bll.db.LoadingBays.Where(m => m.PK_LoadingBay == model.FK_LoadingBay).FirstOrDefault();
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
                //flag = "validation_failed";
                var _conflictingLoadingBay = loadingBay.LoadingBayUtilization.Vehicle;
                message = "বে নাম্বারঃ" + loadingBay.Name + "-তে " + loadingBay.LoadingBayUtilization.Vehicle.RegistrationNumber + " গাড়িটি আছে।";
                CreateAlertMessage(AlertMessageType.Warning, "Warning", message);
                return RedirectToAction("StartLoadingUnloading");
            }

            bll.db.LoadingBayUtilizations.Add(model);
            vehicle.FK_LoadingBayUtilization_Last = model.PK_LoadingBayUtilization;
            loadingBay.FK_LoadingBayUtilization_Last = model.PK_LoadingBayUtilization;
            loadingBay.CurrentUseType = model.UseType;
            bll.db.SaveChanges();

            //ParkingInOut
            if (vehicle.FK_ParkingInOut_Last != null)
            {
                var parkingInOut = bll.db.ParkingInOuts.Where(m => m.PK_ParkingInOut == vehicle.FK_ParkingInOut_Last).FirstOrDefault();
                if (parkingInOut != null)
                {
                    parkingInOut.FK_LoadingBayUtilization = model.PK_LoadingBayUtilization;
                }
            }

            bll.db.SaveChanges();
            //flag = "success";
            message = "এন্ট্রি সফল হয়েছে।";
            CreateAlertMessage(AlertMessageType.Success, "Success", message);
            return RedirectToAction("CurrentlyBusyLoadingBays_CurrentUser");
        }

        public ActionResult CurrentlyBusyLoadingBays_CurrentUser()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = bll.db.LoadingBayUtilizations.Where(m => m.FK_AppUser_StartedBy == CurrentUser.PK_User && (m.UseType == "Load" || m.UseType == "Unload") && m.EndDateTime == null).ToList();
            return View(list);
        }

        public ActionResult CurrentlyBusyLoadingBays(string OWN_MHT_DHT, string PRG_Type, String FK_Location, String FK_LocationBuilding, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<LoadingBayUtilization> list = new List<LoadingBayUtilization>();

            var query = bll.db.LoadingBayUtilizations.Where(m => m.UseType != "Empty" && m.EndDateTime == null).AsQueryable();

            //OWN_MHT_DHTList
            if (OWN_MHT_DHT == null)
            {
                //Do Nothing
            }
            else if (OWN_MHT_DHT != "all")
            {
                query = query.Where(m => m.OWN_MHT_DHT == OWN_MHT_DHT);
            }
            List<SelectListItem> OWN_MHT_DHTList = new List<SelectListItem>();
            OWN_MHT_DHTList.Add(new SelectListItem() { Value = "all", Text = "All" });
            OWN_MHT_DHTList.AddRange(OWN_MHT_DHT_Dict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key, Text = m.Value }));
            ViewBag.OWN_MHT_DHT = new SelectList(OWN_MHT_DHTList.OrderBy(m => m.Text), "Value", "Text", OWN_MHT_DHT);

            //PRG_Type
            if (PRG_Type == null)
            {
                //Do Nothing
            }
            else if (PRG_Type != "all")
            {
                query = query.Where(m => m.LoadingBay.LocationBuilding.PRG_Type == PRG_Type);
            }
            List<SelectListItem> PRG_TypeList = new List<SelectListItem>();
            PRG_TypeList.Add(new SelectListItem() { Value = "all", Text = "All" });
            PRG_TypeList.AddRange(PRG_TypesDict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key, Text = m.Value }));
            ViewBag.PRG_Type = new SelectList(PRG_TypeList.OrderBy(m => m.Text), "Value", "Text", PRG_Type);

            //FK_Location
            if (FK_Location == null)
            {
                //Do Nothing
            }
            else if (FK_Location != "all")
            {
                var _FK_Location = Guid.Parse(FK_Location);

                query = query.Where(m => m.LoadingBay.LocationBuilding.FK_Location == _FK_Location);
            }
            List<SelectListItem> LocationList = new List<SelectListItem>();
            LocationList.Add(new SelectListItem() { Value = "all", Text = "All" });
            LocationList.AddRange(bll.db.Locations.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Location.ToString(), Text = m.Name }));
            ViewBag.Locations = new SelectList(LocationList.OrderBy(m => m.Text), "Value", "Text", FK_Location);

            //FK_LocationBuilding
            if (FK_LocationBuilding == null)
            {
                //Do Nothing
            }
            else if (FK_LocationBuilding != "all")
            {
                var _FK_LocationBuilding = Convert.ToInt64(FK_LocationBuilding);

                query = query.Where(m => m.LoadingBay.FK_LocationBuilding == _FK_LocationBuilding);
            }
            List<SelectListItem> LocationBuildingList = new List<SelectListItem>();
            LocationBuildingList.Add(new SelectListItem() { Value = "all", Text = "All" });
            LocationBuildingList.AddRange(bll.db.LocationBuildings.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_LocationBuilding.ToString(), Text = m.Name }));
            ViewBag.LocationBuildings = new SelectList(LocationBuildingList.OrderBy(m => m.Text), "Value", "Text", FK_LocationBuilding);



            ////StartingDate
            //if (!string.IsNullOrEmpty(StartingDate))
            //{
            //    var _StartingDate = Convert.ToDateTime(StartingDate);
            //    query = query.Where(m => (m.In_IssueDateTime >= _StartingDate) || (m.In_IssueDateTime < _StartingDate && m.Out_IssueDateTime == null));
            //}
            //ViewBag.StartingDate = StartingDate;

            ////EndingDate
            //if (!string.IsNullOrEmpty(EndingDate))
            //{
            //    var _EndingDate = Convert.ToDateTime(EndingDate);
            //    query = query.Where(m => m.In_IssueDateTime < _EndingDate);
            //}
            //ViewBag.EndingDate = EndingDate;

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.Vehicle.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;
           
            //var endingDateLimit = DateTime.Now;
            if (!string.IsNullOrEmpty(OWN_MHT_DHT) || !string.IsNullOrEmpty(PRG_Type) || !string.IsNullOrEmpty(FK_Location) || !string.IsNullOrEmpty(FK_LocationBuilding) || !string.IsNullOrEmpty(RegistrationNumber))
            {
                list = query.OrderBy(m => m.PK_LoadingBayUtilization).ToList();
            }
            var endingDateLimit = DateTime.Now;
            foreach (var item in list.Where(m => m.EndDateTime == null))
            {
                item.StayTimeMinute = (long)(endingDateLimit - item.StartDateTime).TotalMinutes;
                if (item.StayTimeMinute < 0)
                {
                    item.StayTimeMinute = 0;
                }
            }
            return View(list);
        }

        public ActionResult EndLoadingUnloading(Int64 PK_LoadingbayUtilization)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var message = "";
            //# get current model and check
            var currentModel = bll.db.LoadingBayUtilizations.Where(m => m.PK_LoadingBayUtilization == PK_LoadingbayUtilization).FirstOrDefault();
            if (currentModel.EndDateTime != null)
            {
                //flag = "validation_failed";
                message = "বিফল হয়েছে। এই এন্ট্রী ইতিমধ্যে বন্ধ করা হয়ে গেছে।";
                CreateAlertMessage(AlertMessageType.Warning, "Warning", message);
            }

            //# update the current model
            currentModel.EndDateTime = DateTime.Now;
            currentModel.FK_AppUser_EndedBy = CurrentUser.PK_User;
            currentModel.DeviceId_Ended = "Web";
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
            bll.db.LoadingBayUtilizations.Add(newModel);

            //Update LoadingBay
            var loadingBay = currentModel.LoadingBay;
            loadingBay.FK_LoadingBayUtilization_Last = newModel.PK_LoadingBayUtilization;
            loadingBay.CurrentUseType = "Empty";

            bll.db.SaveChanges();
            //flag = "success";
            message = "এন্ট্রি সফল হয়েছে।";
            CreateAlertMessage(AlertMessageType.Success, "Success", message);
            return RedirectToAction("CurrentlyBusyLoadingBays_CurrentUser");
        }
    }

}