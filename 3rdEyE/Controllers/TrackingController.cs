using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.BLL;
using _3rdEyE.ManagingTools;
using _3rdEyE.Models;

namespace _3rdEyE.Controllers
{
    public class TrackingController : BaseController
    {
        DataTable dtindividual = new DataTable();

        public ActionResult TrackingDashBoard()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }
        public JsonResult GetTrackingDashBoardData()
        {
            var allVehicleList = bll.db.Vehicles.Where(m => m.IsDeleted != true && m.OWN_MHT_DHT == "OWN" && m.Depo.IsDeleted != true).ToList();

            //# Pran
            var PranVehicleList = allVehicleList.Where(m => m.Depo.PRG_Type == "PRAN").ToList();
            var PranVehicleCount = PranVehicleList.Count();
            var PranVehicleWithIMEICount = PranVehicleList.Where(m => m.GpsIMEINumber != null && m.GpsIMEINumber != "").Count();
            var PranVehicleTrackingList = PranVehicleList.Where(m => m.VehicleTrackingInformation != null && m.VehicleTrackingInformation.VehicleTracking != null).Select(m => m.VehicleTrackingInformation.VehicleTracking).ToList();
            var PranVehicleTrackingCount = PranVehicleTrackingList.Count();
            var PranVehicleTrackingConnectedCount = PranVehicleTrackingList.Where(m => m.ServerTime > DateTime.Now.AddHours(-6)).Count();
            var PranCompanyWiseDetails = PranVehicleList.GroupBy(m => m.FK_Company).Select(g => new
            {
                g.Key,
                CompanyName = g.Key == null ? "Not Assigned" : g.FirstOrDefault().Company.Name,
                VehicleCount = g.Count(),
                VehicleWithIMEICount = g.Where(n => n.GpsIMEINumber != null && n.GpsIMEINumber != "").Count(),
                VehicleTrackingCount = g.Where(n => n.VehicleTrackingInformation != null && n.VehicleTrackingInformation.VehicleTracking != null).Count(),
                VehicleTrackingConnectedCount = g.Where(n => n.VehicleTrackingInformation != null && n.VehicleTrackingInformation.VehicleTracking != null && n.VehicleTrackingInformation.VehicleTracking.ServerTime > DateTime.Now.AddHours(-6)).Count(),
            }).ToList();


            //# RFL
            var RFLVehicleList = allVehicleList.Where(m => m.Depo.PRG_Type == "RFL").ToList();
            var RFLVehicleCount = RFLVehicleList.Count();
            var RFLVehicleWithIMEICount = RFLVehicleList.Where(m => m.GpsIMEINumber != null && m.GpsIMEINumber != "").Count();
            var RFLVehicleTrackingList = RFLVehicleList.Where(m => m.VehicleTrackingInformation != null && m.VehicleTrackingInformation.VehicleTracking != null).Select(m => m.VehicleTrackingInformation.VehicleTracking).ToList();
            var RFLVehicleTrackingCount = RFLVehicleTrackingList.Count();
            var RFLVehicleTrackingConnectedCount = RFLVehicleTrackingList.Where(m => m.ServerTime > DateTime.Now.AddHours(-6)).Count();
            var RFLCompanyWiseDetails = RFLVehicleList.GroupBy(m => m.FK_Company).Select(g => new
            {
                g.Key,
                CompanyName = g.Key == null ? "Not Assigned" : g.FirstOrDefault().Company.Name,
                VehicleCount = g.Count(),
                VehicleWithIMEICount = g.Where(n => n.GpsIMEINumber != null && n.GpsIMEINumber != "").Count(),
                VehicleTrackingCount = g.Where(n => n.VehicleTrackingInformation != null && n.VehicleTrackingInformation.VehicleTracking != null).Count(),
                VehicleTrackingConnectedCount = g.Where(n => n.VehicleTrackingInformation != null && n.VehicleTrackingInformation.VehicleTracking != null && n.VehicleTrackingInformation.VehicleTracking.ServerTime > DateTime.Now.AddHours(-6)).Count(),
            }).ToList();

            return Json(
                new
                {
                    PranVehicleCount,
                    PranVehicleWithIMEICount,
                    PranVehicleTrackingCount,
                    PranVehicleTrackingConnectedCount,
                    PranCompanyWiseDetails,

                    RFLVehicleCount,
                    RFLVehicleWithIMEICount,
                    RFLVehicleTrackingCount,
                    RFLVehicleTrackingConnectedCount,
                    RFLCompanyWiseDetails,
                });
        }
        //Index all
        #region 
        //public ActionResult Index_all_live()
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    return View();
        //}
        //public ActionResult Index_all_live2()
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    return View();
        //}
        public ActionResult Index_all()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => accessibleDepoes.Contains(m.PK_Depo) && m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name");

            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => accessibleDepoes.Contains(m.FK_Depo) && m.IsDeleted == false).OrderBy(m => m.Name), "PK_DepoGroup", "Name");
            return View();
        }
        //public ActionResult Index_all_before_merge()
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    return View();
        //}
        public ActionResult Index_all2()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }

        public ActionResult Index_all3()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => accessibleDepoes.Contains(m.PK_Depo) && m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name");

            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => accessibleDepoes.Contains(m.FK_Depo) && m.IsDeleted == false).OrderBy(m => m.Name), "PK_DepoGroup", "Name");
            return View();
        }

        public ActionResult Index_all4(string LoadReport, string Depo_Category, string FK_Depo, string FK_DepoGroup, string InOut_FK_Location, string IsInWorkshop, string CurrentTripToward_FK_Location)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var vehicles = new List<VM_VehicleTracking4>();
            var query = bll.db.Vehicles.Where(m => m.IsDeleted == false && m.Depo.IsDeleted == false && m.VehicleTrackingInformation != null && (m.VehicleTrackingInformation.VehicleTracking != null || m.VehicleTrackingInformation.VehicleTrackingVT1 != null));

            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();

            // #Depo_Category
            if (!string.IsNullOrEmpty(Depo_Category))
            {
                query = query.Where(m => m.Depo.Category == Depo_Category);
            }
            ViewBag.Depo_Categories = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo)).Select(m => m.Category).Distinct(), Depo_Category);

            // #FK_Depo
            if (!string.IsNullOrEmpty(FK_Depo))
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                query = query.Where(m => m.FK_Depo == _FK_Depo);
                // #FK_DepoGroup
                if (!string.IsNullOrEmpty(FK_DepoGroup))
                {
                    var _FK_DepoGroup = Guid.Parse(FK_DepoGroup);
                    query = query.Where(m => m.FK_DepoGroup == _FK_DepoGroup);
                }
                ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && m.FK_Depo == _FK_Depo).OrderBy(m => m.Name), "PK_DepoGroup", "Name", FK_DepoGroup);
            }
            else
            {
                query = query.Where(m => accessibleDepoes.Contains(m.FK_Depo));
                ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && m.PK_DepoGroup == null).OrderBy(m => m.Name), "PK_DepoGroup", "Name");
            }
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo)).OrderBy(m => m.Name), "PK_Depo", "Name", FK_Depo);

            // #InOut_FK_Location
            var InOut_LocationDict = new Dictionary<string, string>();
            InOut_LocationDict.Add("all_in", "All Inside");
            InOut_LocationDict.Add("all_out", "All Outside");

            var InOut_LocationList = bll.db.Locations.Where(m => m.IsDeleted == false && (m.LocationType == "Depo" || m.LocationType == "Factory" || m.LocationType == "Workshop"))
                   .Select(m => new { m.PK_Location, m.Name }).OrderBy(m => m.Name).ToList();
            foreach (var item in InOut_LocationList)
            {
                InOut_LocationDict.Add(item.PK_Location.ToString(), item.Name);
            }

            if (!string.IsNullOrEmpty(InOut_FK_Location))
            {
                if (InOut_FK_Location == "all_in")
                {
                    query = query.Where(m => m.LocationInOrOut == true);
                }
                else if (InOut_FK_Location == "all_out")
                {
                    query = query.Where(m => m.LocationInOrOut == false);
                }
                else
                {
                    var _FK_Location = Guid.Parse(InOut_FK_Location);
                    query = query.Where(m => m.LocationInOrOut == true && m.FK_LocationInOut == _FK_Location);
                }
            }
            ViewBag.InOut_Locations = new SelectList(InOut_LocationDict, "Key", "Value", InOut_FK_Location);

            // #IsInWorkshop
            var IsInWorkshopDict = new Dictionary<string, string>() { { "yes", "In Workshop" }, { "no", "Out Workshop" } };
            if (!string.IsNullOrEmpty(IsInWorkshop))
            {
                if (IsInWorkshop == "yes")
                {
                    //query = query.Where(m => m.VehicleInOutManual != null && m.VehicleInOutManual.InOrOut == true && ((m.VehicleInOutManual.Location.LocationType == "Workshop") || (m.VehicleInOutManual.FK_PRG_Type == 3)));
                    query = query.Where(m => m.VehicleInOutManual != null && m.VehicleInOutManual.InOrOut == true && m.VehicleInOutManual.FK_PRG_Type == 3);
                }
                else if (IsInWorkshop == "no")
                {
                    query = query.Where(m => m.VehicleInOutManual == null || m.VehicleInOutManual.InOrOut == false || m.VehicleInOutManual.FK_PRG_Type != 3);
                }
            }
            ViewBag.IsInWorkshop = new SelectList(IsInWorkshopDict, "Key", "Value", IsInWorkshop);

            // #CurrentTripToward_FK_Location
            if (!string.IsNullOrEmpty(CurrentTripToward_FK_Location))
            {
                var _FK_Location = Guid.Parse(CurrentTripToward_FK_Location);
                query = query.Where(m => m.RequisitionTrip != null && m.RequisitionTrip.Requisition.FK_Location_To == _FK_Location);
            }
            ViewBag.CurrentTripToward_Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", CurrentTripToward_FK_Location);

            if (!string.IsNullOrEmpty(LoadReport))
            {
                vehicles = query.Select(m => new VM_VehicleTracking4()
                {
                    PK_Vehicle = m.PK_Vehicle,
                    RegistrationNumber = m.RegistrationNumber,
                    Depo_Name = m.Depo.Name,
                    Location_InOrOut = m.LocationInOrOut,
                    Location_Name = m.Location != null ? m.Location.Name : "",
                    Location_InOrOutDateTime = m.LocationInOutTime ?? new DateTime(),
                    VehicleTracking_UpdateTime = m.VehicleTrackingInformation.VehicleTracking != null ? m.VehicleTrackingInformation.VehicleTracking.UpdateTime : m.VehicleTrackingInformation.VehicleTrackingVT1.UpdateTime
                }).OrderByDescending(m => m.VehicleTracking_UpdateTime).ToList();
            }

            Session["Index_all4_PK_VehicleList"] = vehicles.Select(m => m.PK_Vehicle).ToList();

            return View(vehicles);
        }

        public JsonResult Index_all4_GetTrackingDataFirstTime()
        {
            List<Guid> PK_Vehicles = new List<Guid>();
            if (Session["Index_all4_PK_VehicleList"] != null)
            {
                PK_Vehicles = (List<Guid>)Session["Index_all4_PK_VehicleList"];
            }
            var now = DateTime.Now;
            var _list = bll.db.VehicleTrackings.Where(m => PK_Vehicles.Contains(m.PK_Vehicle))
                .Select(m => new
                {
                    m.PK_Vehicle,
                    m.VehicleTrackingInformation.Vehicle.RegistrationNumber,
                    m.Latitude,
                    m.Longitude,
                    m.UpdateTime,
                    m.ServerTime,
                    m.EngineStatus,
                    m.Speed,
                    m.VehicleTrackingInformation.GpsIMEINumber
                }).ToList();
            _list.AddRange(bll.db.VehicleTrackingVT1.Where(m => PK_Vehicles.Contains(m.PK_Vehicle))
                .Select(m => new
                {
                    m.PK_Vehicle,
                    m.VehicleTrackingInformation.Vehicle.RegistrationNumber,
                    m.Latitude,
                    m.Longitude,
                    m.UpdateTime,
                    m.ServerTime,
                    m.EngineStatus,
                    m.Speed,
                    m.VehicleTrackingInformation.GpsIMEINumber
                }).ToList());

            var list = _list.Select(m => new
            {
                m.PK_Vehicle,
                m.RegistrationNumber,
                m.Latitude,
                m.Longitude,
                UpdateTime = m.UpdateTime.ToString("dd'/'MM'/'yy hh:mm tt"),
                ServerTime = m.ServerTime.ToString("dd'/'MM'/'yy hh:mm tt"),
                m.EngineStatus,
                m.Speed,
                Status = (m.GpsIMEINumber == null) ? -3 :
                ((now - m.UpdateTime).TotalMinutes > 720) ? -2 :
                ((now - m.UpdateTime).TotalMinutes > 15) ? -1 :
                (m.Speed > 0) ? 2 :
                (m.EngineStatus == "1") ? 1 : 0
            }).ToList();
            var json_data = Json(list, JsonRequestBehavior.AllowGet);
            json_data.MaxJsonLength = int.MaxValue;
            return json_data;
        }

        public JsonResult Index_all4_GetTrackingDataCyclicTime()
        {
            List<Guid> PK_Vehicles = new List<Guid>();
            if (Session["Index_all4_PK_VehicleList"] != null)
            {
                PK_Vehicles = (List<Guid>)Session["Index_all4_PK_VehicleList"];
            }
            var now = DateTime.Now;
            var _list = bll.db.VehicleTrackings.Where(m => PK_Vehicles.Contains(m.PK_Vehicle))
                .Select(m => new
                {
                    m.PK_Vehicle,
                    m.VehicleTrackingInformation.Vehicle.RegistrationNumber,
                    m.Latitude,
                    m.Longitude,
                    m.UpdateTime,
                    m.ServerTime,
                    m.EngineStatus,
                    m.Speed,
                    m.VehicleTrackingInformation.GpsIMEINumber
                }).ToList();
            _list.AddRange(bll.db.VehicleTrackingVT1.Where(m => PK_Vehicles.Contains(m.PK_Vehicle))
                .Select(m => new
                {
                    m.PK_Vehicle,
                    m.VehicleTrackingInformation.Vehicle.RegistrationNumber,
                    m.Latitude,
                    m.Longitude,
                    m.UpdateTime,
                    m.ServerTime,
                    m.EngineStatus,
                    m.Speed,
                    m.VehicleTrackingInformation.GpsIMEINumber
                }).ToList());
            var list = _list.Select(m => new
            {
                m.PK_Vehicle,
                m.RegistrationNumber,
                m.Latitude,
                m.Longitude,
                UpdateTime = m.UpdateTime.ToString("dd'/'MM'/'yy hh:mm tt"),
                ServerTime = m.ServerTime.ToString("dd'/'MM'/'yy hh:mm tt"),
                m.EngineStatus,
                m.Speed,
                Status = (m.GpsIMEINumber == null) ? -3 :
                ((now - m.UpdateTime).TotalMinutes > 720) ? -2 :
                ((now - m.UpdateTime).TotalMinutes > 15) ? -1 :
                (m.Speed > 0) ? 2 :
                (m.EngineStatus == "1") ? 1 : 0
            }).ToList();
            var json_data = Json(list, JsonRequestBehavior.AllowGet);
            json_data.MaxJsonLength = int.MaxValue;
            return json_data;
        }

        public JsonResult Index_all4_GetVehicleDetailData(Guid PK_Vehicle)
        {
            //Dictionary<string, object> data = new Dictionary<string, object>();
            var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == PK_Vehicle).FirstOrDefault();

            //#BasicInformatoin
            var BasicInfo = new
            {
                PRG_Type = vehicle.Depo.PRG_Type,
                DepotCategory = vehicle.Depo.Category,
                Depot = vehicle.Depo.Name,
                DepoGroup = vehicle.DepoGroup?.Name + " " + vehicle.DepoGroup?.ContactNumber,
                ContactNumber = vehicle.Internal_VehicleContactNumber,
                ImageLink = vehicle.ImageLocation != null ? vehicle.ImageLocation : "/Content/Images/vehicle_photo_common.jpeg"
            };

            //#TrackingInformatoin
            var now = DateTime.Now;
            var disconnectedMinute = 720;
            var timeLapsedMinute = 5;
            var TrackingInfo = new Object();
            var tracking = bll.db.VehicleTrackings.Where(m => m.PK_Vehicle == vehicle.PK_Vehicle).Select(m => new
            {
                m.Latitude,
                m.Longitude,
                m.UpdateTime,
                m.Speed,
                m.Temperature,
                m.EngineStatus
            }).FirstOrDefault();
            if (tracking == null)
            {
                tracking = bll.db.VehicleTrackingVT1.Where(m => m.PK_Vehicle == vehicle.PK_Vehicle).Select(m => new
                {
                    m.Latitude,
                    m.Longitude,
                    m.UpdateTime,
                    m.Speed,
                    m.Temperature,
                    m.EngineStatus
                }).FirstOrDefault();
            }
            var delayMinute = (now - tracking.UpdateTime).TotalMinutes;
            var _MapLocation = "";


            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            var query = $"EXEC Report_GetMapLocattion @Latitude= {tracking.Latitude}, @Longitude={tracking.Longitude}";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            cmd.Connection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    _MapLocation = String.Format("{0}", reader["MapLocation"]);
                }
            }
            cmd.Connection.Close();
            if (tracking != null)
            {
                TrackingInfo = new
                {
                    GpsDeviceModel = vehicle.GpsDeviceModel,
                    UpdateTime = tracking.UpdateTime.ToString(),
                    MapLocation = _MapLocation,
                    tracking.Speed,
                    Temperature = vehicle.Internal_ShowTemperature == true ? tracking.Temperature.ToString() : "N/A",
                    Status = (delayMinute > disconnectedMinute) ? "Disconnected" :
                    (delayMinute > timeLapsedMinute) ? "Time Lapsed" :
                    tracking.Speed > 0 ? "Running" :
                    tracking.EngineStatus == "1" ? "Stand By" : "Egnition Off",
                };
            }

            //#RequisitionTrip
            var RequisitionTripInfo = new Object();
            if (vehicle.FK_RequisitionTrip_Last != null)
            {
                var requisitionTrip = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == vehicle.FK_RequisitionTrip_Last).FirstOrDefault();
                if (requisitionTrip != null)
                {
                    RequisitionTripInfo = new
                    {
                        requisitionTrip.TrackingID,
                        FromLocation = requisitionTrip.Requisition.Location.Name,
                        ToLocation = requisitionTrip.Requisition.Location1.Name,
                        StartedAt = requisitionTrip.StartedAt != null ? requisitionTrip.StartedAt.Value.ToString() : "",
                        TentativeFinishingDateTime = requisitionTrip.TentativeFinishingDateTime != null ? requisitionTrip.TentativeFinishingDateTime.Value.ToString() : "",
                        FinishedAt = requisitionTrip.FinishedAt != null ? requisitionTrip.StartedAt.Value.ToString() : "",
                        Status = requisitionTrip.StatusText,
                        ToLocationLatLong = new
                        {
                            requisitionTrip.Requisition.Location1.Latitude,
                            requisitionTrip.Requisition.Location1.Longitude
                        }
                    };
                }
            }

            //#ParkingInOutInfo
            var ParkingInOutInfo = new object();
            if (vehicle.FK_ParkingInOut_Last != null)
            {
                var parkingInOut = bll.db.ParkingInOuts.Where(m => m.PK_ParkingInOut == vehicle.FK_ParkingInOut_Last && m.Out_IssueDateTime == null).FirstOrDefault();
                if (parkingInOut != null)
                {
                    ParkingInOutInfo = new
                    {
                        ParkingName = parkingInOut.AppUser.FullName,
                        ParkingInTime = parkingInOut.In_IssueDateTime.ToString()
                    };
                }
            }

            return Json(new
            {
                BasicInfo = new
                {
                    DisplayName = "Basic Information",
                    Data = BasicInfo
                },
                TrackingInfo = new
                {
                    DisplayName = "Tracking Information",
                    Data = TrackingInfo
                },
                RequisitionTripInfo = new
                {
                    DisplayName = "Trip Information",
                    Data = RequisitionTripInfo
                },
                ParkingInOutInfo = new
                {
                    DisplayName = "Parking Information",
                    Data = ParkingInOutInfo
                }
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index_all5(string LoadReport, string Depo_Category, string FK_Depo, string FK_DepoGroup, string InOut_FK_Location, string IsInWorkshop, string CurrentTripToward_FK_Location)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var vehicles = new List<VM_VehicleTracking4>();
            var query = bll.db.Vehicles.Where(m => m.IsDeleted == false && m.Depo.IsDeleted == false && m.VehicleTrackingInformation != null && (m.VehicleTrackingInformation.VehicleTracking != null || m.VehicleTrackingInformation.VehicleTrackingVT1 != null));

            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();

            // #Depo_Category
            if (!string.IsNullOrEmpty(Depo_Category))
            {
                query = query.Where(m => m.Depo.Category == Depo_Category);
            }
            ViewBag.Depo_Categories = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo)).Select(m => m.Category).Distinct(), Depo_Category);

            // #FK_Depo
            if (!string.IsNullOrEmpty(FK_Depo))
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                query = query.Where(m => m.FK_Depo == _FK_Depo);
                // #FK_DepoGroup
                if (!string.IsNullOrEmpty(FK_DepoGroup))
                {
                    var _FK_DepoGroup = Guid.Parse(FK_DepoGroup);
                    query = query.Where(m => m.FK_DepoGroup == _FK_DepoGroup);
                }
                ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && m.FK_Depo == _FK_Depo).OrderBy(m => m.Name), "PK_DepoGroup", "Name", FK_DepoGroup);
            }
            else
            {
                query = query.Where(m => accessibleDepoes.Contains(m.FK_Depo));
                ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && m.PK_DepoGroup == null).OrderBy(m => m.Name), "PK_DepoGroup", "Name");
            }
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo)).OrderBy(m => m.Name), "PK_Depo", "Name", FK_Depo);

            // #InOut_FK_Location
            var InOut_LocationDict = new Dictionary<string, string>();
            InOut_LocationDict.Add("all_in", "All Inside");
            InOut_LocationDict.Add("all_out", "All Outside");

            var InOut_LocationList = bll.db.Locations.Where(m => m.IsDeleted == false && (m.LocationType == "Depo" || m.LocationType == "Factory" || m.LocationType == "Workshop"))
                   .Select(m => new { m.PK_Location, m.Name }).OrderBy(m => m.Name).ToList();
            foreach (var item in InOut_LocationList)
            {
                InOut_LocationDict.Add(item.PK_Location.ToString(), item.Name);
            }

            if (!string.IsNullOrEmpty(InOut_FK_Location))
            {
                if (InOut_FK_Location == "all_in")
                {
                    query = query.Where(m => m.LocationInOrOut == true);
                }
                else if (InOut_FK_Location == "all_out")
                {
                    query = query.Where(m => m.LocationInOrOut == false);
                }
                else
                {
                    var _FK_Location = Guid.Parse(InOut_FK_Location);
                    query = query.Where(m => m.LocationInOrOut == true && m.FK_LocationInOut == _FK_Location);
                }
            }
            ViewBag.InOut_Locations = new SelectList(InOut_LocationDict, "Key", "Value", InOut_FK_Location);

            // #IsInWorkshop
            var IsInWorkshopDict = new Dictionary<string, string>() { { "yes", "In Workshop" }, { "no", "Out Workshop" } };
            if (!string.IsNullOrEmpty(IsInWorkshop))
            {
                if (IsInWorkshop == "yes")
                {
                    //query = query.Where(m => m.VehicleInOutManual != null && m.VehicleInOutManual.InOrOut == true && ((m.VehicleInOutManual.Location.LocationType == "Workshop") || (m.VehicleInOutManual.FK_PRG_Type == 3)));
                    query = query.Where(m => m.VehicleInOutManual != null && m.VehicleInOutManual.InOrOut == true && m.VehicleInOutManual.FK_PRG_Type == 3);
                }
                else if (IsInWorkshop == "no")
                {
                    query = query.Where(m => m.VehicleInOutManual == null || m.VehicleInOutManual.InOrOut == false || m.VehicleInOutManual.FK_PRG_Type != 3);
                }
            }
            ViewBag.IsInWorkshop = new SelectList(IsInWorkshopDict, "Key", "Value", IsInWorkshop);

            // #CurrentTripToward_FK_Location
            if (!string.IsNullOrEmpty(CurrentTripToward_FK_Location))
            {
                var _FK_Location = Guid.Parse(CurrentTripToward_FK_Location);
                query = query.Where(m => m.RequisitionTrip != null && m.RequisitionTrip.Requisition.FK_Location_To == _FK_Location);
            }
            ViewBag.CurrentTripToward_Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", CurrentTripToward_FK_Location);

            if (!string.IsNullOrEmpty(LoadReport))
            {
                vehicles = query.Select(m => new VM_VehicleTracking4()
                {
                    PK_Vehicle = m.PK_Vehicle,
                    RegistrationNumber = m.RegistrationNumber,
                    Depo_Name = m.Depo.Name,
                    Location_InOrOut = m.LocationInOrOut,
                    Location_Name = m.Location != null ? m.Location.Name : "",
                    Location_InOrOutDateTime = m.LocationInOutTime ?? new DateTime(),
                    VehicleTracking_UpdateTime = m.VehicleTrackingInformation.VehicleTracking != null ? m.VehicleTrackingInformation.VehicleTracking.UpdateTime : m.VehicleTrackingInformation.VehicleTrackingVT1.UpdateTime
                }).OrderByDescending(m => m.VehicleTracking_UpdateTime).ToList();
            }

            Session["Index_all5_PK_VehicleList"] = vehicles.Select(m => m.PK_Vehicle).ToList();

            return View(vehicles);
        }
        public JsonResult Index_all5_GetTrackingDataFirstTime()
        {
            List<Guid> PK_Vehicles = new List<Guid>();
            if (Session["Index_all5_PK_VehicleList"] != null)
            {
                PK_Vehicles = (List<Guid>)Session["Index_all5_PK_VehicleList"];
            }
            var now = DateTime.Now;
            var _list = bll.db.VehicleTrackings.Where(m => PK_Vehicles.Contains(m.PK_Vehicle))
                .Select(m => new
                {
                    m.PK_Vehicle,
                    m.VehicleTrackingInformation.Vehicle.RegistrationNumber,
                    m.Latitude,
                    m.Longitude,
                    m.UpdateTime,
                    m.ServerTime,
                    m.EngineStatus,
                    m.Speed,
                    m.VehicleTrackingInformation.GpsIMEINumber
                }).ToList();
            _list.AddRange(bll.db.VehicleTrackingVT1.Where(m => PK_Vehicles.Contains(m.PK_Vehicle))
                .Select(m => new
                {
                    m.PK_Vehicle,
                    m.VehicleTrackingInformation.Vehicle.RegistrationNumber,
                    m.Latitude,
                    m.Longitude,
                    m.UpdateTime,
                    m.ServerTime,
                    m.EngineStatus,
                    m.Speed,
                    m.VehicleTrackingInformation.GpsIMEINumber
                }).ToList());

            var list = _list.Select(m => new
            {
                m.PK_Vehicle,
                m.RegistrationNumber,
                m.Latitude,
                m.Longitude,
                UpdateTime = m.UpdateTime.ToString("dd'/'MM'/'yy hh:mm tt"),
                ServerTime = m.ServerTime.ToString("dd'/'MM'/'yy hh:mm tt"),
                m.EngineStatus,
                m.Speed,
                Status = (m.GpsIMEINumber == null) ? -3 :
                ((now - m.UpdateTime).TotalMinutes > 720) ? -2 :
                ((now - m.UpdateTime).TotalMinutes > 15) ? -1 :
                (m.Speed > 0) ? 2 :
                (m.EngineStatus == "1") ? 1 : 0
            }).ToList();
            var json_data = Json(list, JsonRequestBehavior.AllowGet);
            json_data.MaxJsonLength = int.MaxValue;
            return json_data;
        }

        public JsonResult Index_all5_GetTrackingDataCyclicTime()
        {
            List<Guid> PK_Vehicles = new List<Guid>();
            if (Session["Index_all5_PK_VehicleList"] != null)
            {
                PK_Vehicles = (List<Guid>)Session["Index_all5_PK_VehicleList"];
            }
            var now = DateTime.Now;
            var _list = bll.db.VehicleTrackings.Where(m => PK_Vehicles.Contains(m.PK_Vehicle))
                .Select(m => new
                {
                    m.PK_Vehicle,
                    m.VehicleTrackingInformation.Vehicle.RegistrationNumber,
                    m.Latitude,
                    m.Longitude,
                    m.UpdateTime,
                    m.ServerTime,
                    m.EngineStatus,
                    m.Speed,
                    m.VehicleTrackingInformation.GpsIMEINumber
                }).ToList();
            _list.AddRange(bll.db.VehicleTrackingVT1.Where(m => PK_Vehicles.Contains(m.PK_Vehicle))
                .Select(m => new
                {
                    m.PK_Vehicle,
                    m.VehicleTrackingInformation.Vehicle.RegistrationNumber,
                    m.Latitude,
                    m.Longitude,
                    m.UpdateTime,
                    m.ServerTime,
                    m.EngineStatus,
                    m.Speed,
                    m.VehicleTrackingInformation.GpsIMEINumber
                }).ToList());
            var list = _list.Select(m => new
            {
                m.PK_Vehicle,
                m.RegistrationNumber,
                m.Latitude,
                m.Longitude,
                UpdateTime = m.UpdateTime.ToString("dd'/'MM'/'yy hh:mm tt"),
                ServerTime = m.ServerTime.ToString("dd'/'MM'/'yy hh:mm tt"),
                m.EngineStatus,
                m.Speed,
                Status = (m.GpsIMEINumber == null) ? -3 :
                ((now - m.UpdateTime).TotalMinutes > 720) ? -2 :
                ((now - m.UpdateTime).TotalMinutes > 15) ? -1 :
                (m.Speed > 0) ? 2 :
                (m.EngineStatus == "1") ? 1 : 0
            }).ToList();
            var json_data = Json(list, JsonRequestBehavior.AllowGet);
            json_data.MaxJsonLength = int.MaxValue;
            return json_data;
        }

        public JsonResult Index_all5_GetVehicleDetailData(Guid PK_Vehicle)
        {
            //Dictionary<string, object> data = new Dictionary<string, object>();
            var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == PK_Vehicle).FirstOrDefault();

            //#BasicInformatoin
            var BasicInfo = new
            {
                PRG_Type = vehicle.Depo.PRG_Type,
                DepotCategory = vehicle.Depo.Category,
                Depot = vehicle.Depo.Name,
                DepoGroup = vehicle.DepoGroup?.Name + " " + vehicle.DepoGroup?.ContactNumber,
                ContactNumber = vehicle.Internal_VehicleContactNumber,
                ImageLink = vehicle.ImageLocation != null ? vehicle.ImageLocation : "/Content/Images/vehicle_photo_common.jpeg"
            };

            //#TrackingInformatoin
            var now = DateTime.Now;
            var disconnectedMinute = 720;
            var timeLapsedMinute = 5;
            var TrackingInfo = new Object();
            var tracking = bll.db.VehicleTrackings.Where(m => m.PK_Vehicle == vehicle.PK_Vehicle).Select(m => new
            {
                m.Latitude,
                m.Longitude,
                m.UpdateTime,
                m.Speed,
                m.Temperature,
                m.EngineStatus
            }).FirstOrDefault();
            if (tracking == null)
            {
                tracking = bll.db.VehicleTrackingVT1.Where(m => m.PK_Vehicle == vehicle.PK_Vehicle).Select(m => new
                {
                    m.Latitude,
                    m.Longitude,
                    m.UpdateTime,
                    m.Speed,
                    m.Temperature,
                    m.EngineStatus
                }).FirstOrDefault();
            }
            var delayMinute = (now - tracking.UpdateTime).TotalMinutes;
            var _MapLocation = "";


            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            var query = $"EXEC Report_GetMapLocattion @Latitude= {tracking.Latitude}, @Longitude={tracking.Longitude}";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            cmd.Connection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    _MapLocation = String.Format("{0}", reader["MapLocation"]);
                }
            }
            cmd.Connection.Close();
            if (tracking != null)
            {
                TrackingInfo = new
                {
                    GpsDeviceModel = vehicle.GpsDeviceModel,
                    UpdateTime = tracking.UpdateTime.ToString(),
                    MapLocation = _MapLocation,
                    tracking.Speed,
                    Temperature = vehicle.Internal_ShowTemperature == true ? tracking.Temperature.ToString() : "N/A",
                    Status = (delayMinute > disconnectedMinute) ? "Disconnected" :
                    (delayMinute > timeLapsedMinute) ? "Time Lapsed" :
                    tracking.Speed > 0 ? "Running" :
                    tracking.EngineStatus == "1" ? "Stand By" : "Egnition Off",
                };
            }

            //#RequisitionTrip
            var RequisitionTripInfo = new Object();
            if (vehicle.FK_RequisitionTrip_Last != null)
            {
                var requisitionTrip = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == vehicle.FK_RequisitionTrip_Last).FirstOrDefault();
                if (requisitionTrip != null)
                {
                    RequisitionTripInfo = new
                    {
                        requisitionTrip.TrackingID,
                        FromLocation = requisitionTrip.Requisition.Location.Name,
                        ToLocation = requisitionTrip.Requisition.Location1.Name,
                        StartedAt = requisitionTrip.StartedAt != null ? requisitionTrip.StartedAt.Value.ToString() : "",
                        TentativeFinishingDateTime = requisitionTrip.TentativeFinishingDateTime != null ? requisitionTrip.TentativeFinishingDateTime.Value.ToString() : "",
                        FinishedAt = requisitionTrip.FinishedAt != null ? requisitionTrip.StartedAt.Value.ToString() : "",
                        Status = requisitionTrip.StatusText,
                        ToLocationLatLong = new
                        {
                            requisitionTrip.Requisition.Location1.Latitude,
                            requisitionTrip.Requisition.Location1.Longitude
                        }
                    };
                }
            }

            //#ParkingInOutInfo
            var ParkingInOutInfo = new object();
            if (vehicle.FK_ParkingInOut_Last != null)
            {
                var parkingInOut = bll.db.ParkingInOuts.Where(m => m.PK_ParkingInOut == vehicle.FK_ParkingInOut_Last && m.Out_IssueDateTime == null).FirstOrDefault();
                if (parkingInOut != null)
                {
                    ParkingInOutInfo = new
                    {
                        ParkingName = parkingInOut.AppUser.FullName,
                        ParkingInTime = parkingInOut.In_IssueDateTime.ToString()
                    };
                }
            }

            return Json(new
            {
                BasicInfo = new
                {
                    DisplayName = "Basic Information",
                    Data = BasicInfo
                },
                TrackingInfo = new
                {
                    DisplayName = "Tracking Information",
                    Data = TrackingInfo
                },
                RequisitionTripInfo = new
                {
                    DisplayName = "Trip Information",
                    Data = RequisitionTripInfo
                },
                ParkingInOutInfo = new
                {
                    DisplayName = "Parking Information",
                    Data = ParkingInOutInfo
                }
            }, JsonRequestBehavior.AllowGet);
        }

        public class VM_VehicleTracking4
        {
            public Guid PK_Vehicle { get; set; }
            public string RegistrationNumber { get; set; }
            public string Depo_Name { get; set; }
            public Nullable<bool> Location_InOrOut { get; set; }
            public string Location_Name { get; set; }
            public DateTime VehicleTracking_UpdateTime { get; set; }
            public Nullable<DateTime> Location_InOrOutDateTime { get; set; }
        }


        public JsonResult GetData()
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Report_GetVehicleTracking '" + CurrentUser.PK_User + "'";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            //dictioneryList.MaxJsonLength = int.MaxValue;
            var json_data = Json(dictioneryList, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }
        public JsonResult _GetData()
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Report_GetVehicleTracking '" + CurrentUser.PK_User + "'";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            var json_data = Json(dictioneryList, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }
        public JsonResult GetData2()
        {
            DateTime now = DateTime.Now;
            Random random = new Random();
            var list = (from vt in bll.db.VehicleTrackings.AsEnumerable()
                        join v in bll.db.Vehicles on vt.PK_Vehicle equals v.PK_Vehicle
                        select new
                        {
                            v.PK_Vehicle,
                            v.RegistrationNumber,
                            v.MHT_DHT_DriverContactNumber,
                            Latitude = NextDouble(random, 23.0, 23.5),
                            Longitude = NextDouble(random, 88.0, 89.0),
                            vt.EngineStatus,
                            vt.Course,
                            vt.Temperature,
                            vt.Fuel,
                            vt.Speed,
                            vt.Distance,
                            UpdateTime = vt.UpdateTime.ToString("dd'-'MM'-'yyyy' 'h':'mm' 'tt"),
                            //Status = now.Subtract(vt.UpdateTime).TotalMinutes > 30 ? -1 : vt.Distance != 0 ? 1 : 0
                            Status = now.Subtract(vt.UpdateTime).TotalMinutes > 30 ? -1 : vt.Speed != 0 ? 2 : vt.EngineStatus == "1" ? 1 : 0,
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion


        //index by group
        #region
        public ActionResult Index_ByGroup()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => accessibleDepoes.Contains(m.FK_Depo) && m.IsDeleted == false).OrderBy(m => m.Name), "PK_DepoGroup", "Name");
            return View();
        }
        public ActionResult Index_ByGroup2()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_DepoGroup", "Name");
            return View();
        }
        public JsonResult GetDataByGroup(Guid FK_DepoGroup)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Report_GetVehicleTrackingByDepoGroup '" + FK_DepoGroup + "'";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            var json_data = Json(dictioneryList, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }
        public JsonResult _GetDataByGroup(Guid FK_DepoGroup)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Report_GetVehicleTrackingByDepoGroup '" + FK_DepoGroup + "'";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            var json_data = Json(dictioneryList, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }

        #endregion

        //index by depo
        #region
        public ActionResult Index_ByDepo()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => accessibleDepoes.Contains(m.PK_Depo) && m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name");
            return View();
        }
        public JsonResult GetDataByDepo(Guid FK_Depo)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Report_GetVehicleTrackingByDepo '" + FK_Depo + "'";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            var json_data = Json(dictioneryList, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }
        public JsonResult _GetDataByDepo(Guid FK_Depo)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Report_GetVehicleTrackingByDepo '" + FK_Depo + "'";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            var json_data = Json(dictioneryList, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }

        #endregion

        //single vehicle
        #region
        public ActionResult Index_single_live(Guid PK_Vehicle)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var vehicle = bll.db.Vehicles.Where(v => v.PK_Vehicle == PK_Vehicle).FirstOrDefault();
            ViewBag.vehicle = vehicle;
            return View();
        }
        public ActionResult Index_single_live2(Guid PK_Vehicle)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var vehicle = bll.db.Vehicles.Where(v => v.PK_Vehicle == PK_Vehicle).FirstOrDefault();
            ViewBag.vehicle = vehicle;
            return View();
        }
        public JsonResult GetData_Single_firstTime(Guid PK_Vehicle)
        {
            Dictionary<string, string> dictionery = new Dictionary<string, string>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            //query = "EXEC Tracking_GetNextDeviceData '" + PK_Vehicle + "', '" + DateTime.Now.AddMinutes(-2).ToString() + "'";
            //query = "EXEC Tracking_GetNextDeviceData '" + PK_Vehicle + "', '" + DateTime.Now.ToString() + "'";
            //cmd.CommandText = query;
            //adpt.SelectCommand = cmd;
            //dataTable.Reset();
            //adpt.Fill(dataTable);
            //dictionery = GetTableRows(dataTable).FirstOrDefault();
            //if (dictionery == null)
            //{
            query = "EXEC Tracking_GetLastDeviceData'" + PK_Vehicle + "'";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictionery = GetTableRows(dataTable).FirstOrDefault();
            //}
            return Json(dictionery, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetData_Single(Guid PK_Vehicle, DateTime PreviousUpdateTime)
        {
            Dictionary<string, string> dictionery = new Dictionary<string, string>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Tracking_GetNextDeviceData'" + PK_Vehicle + "','" + PreviousUpdateTime + "'";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictionery = GetTableRows(dataTable).FirstOrDefault();
            if (dictionery != null)
            {
                return Json(dictionery, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("NotFound", JsonRequestBehavior.AllowGet);
            }
        }
        public static float NextDouble(Random random, double minValue, double maxValue)
        {
            return (float)(random.NextDouble() * (maxValue - minValue) + minValue);
        }
        #endregion

        //# services
        #region Mail Out Over stay Vehicle
        public string TrySendOverStayOutSideAlertEmail()
        {
            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendOverStayOutSideAlertEmail-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();

            try
            {
                var _email = "automation@mis.prangroup.com";
                var _epass = "aaaaAAAA0000";

                SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                sc.EnableSsl = false;
                sc.Credentials = new NetworkCredential(_email, _epass);
                sc.Port = 25;

                var _serverTime = DateTime.Now.AddMinutes(-5);
                var _Location_ChangedAt = DateTime.Now.AddMinutes(-65);
                var PRG_Type = "";
                var list = new List<Vehicle>();
                //# PRAN 
                PRG_Type = "PRAN";
                list = (from vt in bll.db.VehicleTrackings.Where(m => m.EngineStatus == "0" && m.Location_ChangedAt < _Location_ChangedAt && m.ServerTime > _serverTime)
                        join v in bll.db.Vehicles.Where(m => m.Depo.PRG_Type == PRG_Type && m.GpsIMEINumber != null && m.LocationInOrOut == false) on vt.PK_Vehicle equals v.PK_Vehicle
                        select new { vt.Location_ChangedAt, v.LocationInOutTime, vehicle = v }).Where(m => m.Location_ChangedAt > m.LocationInOutTime).Select(m => m.vehicle).ToList();
                if (list.Count > 0)
                {
                    var Mail_Subject = "PRAN | Vehicle Outside Overstay Report (Own) | " + DateTime.Now.ToString("h:mm tt");
                    var Mail_ToList = new List<string>() {
                    "piptpt8@pip.prangroup.com","dist100@prangroup.com",
                    "pfgdist@prangroup.com","dist49@prangroup.com",
                    "dist44@prangroup.com", "dist46@prangroup.com",
                    "dist07@prangroup.com","dist80@prangroup.com",
                    "dist210@prangroup.com","dist322@prangroup.com",
                    "pipmis@pip.rflgroupbd.com","mis8@prangroup.com",
                    "distdepotsch@prangroup.com",
                    "PRAN-Dist-TPT-AllRecipients@prangroup.com", "PRAN-Dist-DIC-AllRecipients@prangroup.com",
                    };
                    var Mail_CcList = new List<string>()
                    {
                        "automation17@mis.prangroup.com", "mis7@prangroup.com", "mis3@prangroup.com",
                    };
                    var Mail_BccList = new List<string>()
                    {
                        //"PRAN-Dist-MIS-AllRecipients@prangroup.com",
                    };
                    MailMessage mail = new MailMessage();
                    foreach (var to in Mail_ToList)
                    {
                        mail.To.Add(to);
                    }
                    foreach (var to in Mail_CcList)
                    {
                        mail.CC.Add(to);
                    }
                    foreach (var to in Mail_BccList)
                    {
                        mail.Bcc.Add(to);
                    }

                    mail.From = new MailAddress(_email);
                    mail.Subject = Mail_Subject;
                    string url = "";
#if DEBUG       
                    url = ConfigurationManager.AppSettings["DEBUG_DOMAIN"];
#else
url = ConfigurationManager.AppSettings["LIVE_DOMAIN"];
#endif
                    url = url + @"Tracking/OverStayOutSideBodyGenerator?PRG_Type=" + PRG_Type;
                    WebClient myWebClient = new WebClient();
                    byte[] myDataBuffer = myWebClient.DownloadData(url);
                    string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                    mail.Body = mailBody_HTML;
                    mail.IsBodyHtml = true;

                    sc.Send(mail);
                }

                //# RFL
                PRG_Type = "RFL";
                list = (from vt in bll.db.VehicleTrackings.Where(m => m.EngineStatus == "0" && m.Location_ChangedAt < _Location_ChangedAt && m.ServerTime > _serverTime)
                        join v in bll.db.Vehicles.Where(m => m.Depo.PRG_Type == PRG_Type && m.GpsIMEINumber != null && m.LocationInOrOut == false) on vt.PK_Vehicle equals v.PK_Vehicle
                        select new { vt.Location_ChangedAt, v.LocationInOutTime, vehicle = v }).Where(m => m.Location_ChangedAt > m.LocationInOutTime).Select(m => m.vehicle).ToList();
                if (list.Count > 0)
                {
                    var Mail_Subject = "RFL | Vehicle Outside Overstay Report (Own) | " + DateTime.Now.ToString("h:mm tt");
                    var Mail_ToList = new List<string>() {
                        "dist121@rflgroupbd.com",
                        "RFLDist2DepotAllTPT@rflgroupbd.com", "RFLDist2DepotAllDIC@rflgroupbd.com","RFLDist2DepotAllMIS@rflgroupbd.com",
                       "RFLDistCoreTeam@rflgroupbd.com", "dist121@rflgroupbd.com", "dist370@rflgroupbd.com", "dist371@rflgroupbd.com", "towfique@rflgroupbd.com",
                       "pipmis@pip.rflgroupbd.com","mis8@prangroup.com"
                    };
                    var Mail_CcList = new List<string>()
                    {
                        "automation17@mis.prangroup.com", "mis7@prangroup.com", "mis3@prangroup.com",
                    };
                    MailMessage mail = new MailMessage();
                    foreach (var to in Mail_ToList)
                    {
                        mail.To.Add(to);
                    }
                    foreach (var to in Mail_CcList)
                    {
                        mail.CC.Add(to);
                    }
                    mail.From = new MailAddress(_email);
                    mail.Subject = Mail_Subject;
                    string url = "";
#if DEBUG       
                    url = ConfigurationManager.AppSettings["DEBUG_DOMAIN"];
#else
url = ConfigurationManager.AppSettings["LIVE_DOMAIN"];
#endif
                    url = url + @"Tracking/OverStayOutSideBodyGenerator?PRG_Type=" + PRG_Type;
                    WebClient myWebClient = new WebClient();
                    byte[] myDataBuffer = myWebClient.DownloadData(url);
                    string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                    mail.Body = mailBody_HTML;
                    mail.IsBodyHtml = true;

                    sc.Send(mail);
                }
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                      new AppErrorLog()
                      {
                          ErrorMessage = e.Message,
                          ErrorTime = DateTime.Now,
                          UserDefinedMessage = "Tracking/TrySendOverStayOutSideAlertEmail"
                      }
                    );
                bll.db.SaveChanges();
            }
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendOverStayOutSideAlertEmail-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            return "Sent ";
        }
        public ActionResult OverStayOutSideBodyGenerator(string PRG_Type)
        {
            var _serverTime = DateTime.Now.AddMinutes(-5);
            var _Location_ChangedAt = DateTime.Now.AddMinutes(-65);
            var list = (from vt in bll.db.VehicleTrackings.Where(m => m.EngineStatus == "0" && m.Location_ChangedAt < _Location_ChangedAt && m.ServerTime > _serverTime)
                        join v in bll.db.Vehicles.Where(m => m.Depo.PRG_Type == PRG_Type && m.GpsIMEINumber != null && m.LocationInOrOut == false) on vt.PK_Vehicle equals v.PK_Vehicle
                        select new { vt.Location_ChangedAt, v.LocationInOutTime, vehicle = v }).Where(m => m.Location_ChangedAt > m.LocationInOutTime).Select(m => m.vehicle).ToList();

            ViewBag.Message_1 = "These " + list.Count + " vehicles are waiting more than 30 minutes outside group locations. Please, take N/A.";
            return View(list);
        }

        public string TrySendOverStayOutSideAlertEmailToDepoGroupManager()
        {
            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendOverStayOutSideAlertEmailToDepoGroupManager-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();

            try
            {
                var _email = "automation@mis.prangroup.com";
                var _epass = "aaaaAAAA0000";

                SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                sc.EnableSsl = false;
                sc.Credentials = new NetworkCredential(_email, _epass);
                sc.Port = 25;

                var _serverTime = DateTime.Now.AddMinutes(-5);
                var _Location_ChangedAt = DateTime.Now.AddMinutes(-65);
                var list = (from vt in bll.db.VehicleTrackings.Where(m => m.EngineStatus == "0" && m.Location_ChangedAt < _Location_ChangedAt && m.ServerTime > _serverTime)
                            join v in bll.db.Vehicles.Where(m => m.GpsIMEINumber != null && m.LocationInOrOut == false) on vt.PK_Vehicle equals v.PK_Vehicle
                            join d in bll.db.Depoes.Where(m => m.IsDeleted == false) on v.FK_Depo equals d.PK_Depo
                            join dg in bll.db.DepoGroups.Where(m => m.IsDeleted == false && m.ConcernMailAddress != null) on v.FK_DepoGroup equals dg.PK_DepoGroup
                            select new { vt.Location_ChangedAt, v.LocationInOutTime, depo = d, vehicle = v, vehicleTracking = vt, depoGroup = dg }).Where(m => m.Location_ChangedAt > m.LocationInOutTime).ToList();
                if (list.Count > 0)
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(_email);
                    mail.Subject = "Vehicle Outside Overstay Report (Own) | " + DateTime.Now.ToString("h:mm tt");
                    foreach (var group_item in list.GroupBy(m => m.depoGroup))
                    {
                        mail.To.Clear();
                        mail.CC.Clear();
                        mail.Bcc.Clear();
                        if (String.IsNullOrEmpty(group_item.Key.ConcernMailAddress))
                        {
                            continue;
                        }
                        mail.To.Add(group_item.Key.ConcernMailAddress);
                        if (group_item.Key.Depo.PRG_Type == "PRAN")
                        {
                            mail.Bcc.Add("automation17@mis.prangroup.com");
                            mail.Bcc.Add("dist100@prangroup.com");
                        }
                        var mailBody_HTML = "<!DOCTYPE html>";
                        mailBody_HTML += @"
                        <html>
                        <head>
                            <style>
                                .class_table {
                                    font-family: Arial, Helvetica, sans-serif;
                                    border-collapse: collapse;
                                    width: 100%;
                                }

                                    .class_table td, .class_table th {
                                        border: 1px solid #ddd;
                                        padding: 8px;
                                    }

                                    .class_table tr:nth-child(even) {
                                        background-color: #f2f2f2;
                                    }

                                    .class_table tr:hover {
                                        background-color: #ddd;
                                    }

                                    .class_table th {
                                        padding-top: 12px;
                                        padding-bottom: 12px;
                                        text-align: left;
                                        background-color: #04AA6D;
                                        color: white;
                                    }
                            </style>
                        </head>
                        <body width='100px'>";
                        mailBody_HTML += @"
                            <h2>Dear Concern</h2>
                            <p style='font-size:x-large;'>Below vehicles are waitng outside more than standard time.</p>
                            <p style='font-size:x-large;'>Please take N/A.</p>";

                        mailBody_HTML += @"
                            <table class='class_table'>
                                        <tr>
                                            <th>SL</th>
                                            <th>User Group</th>
                                            <th>Vehicle</th>
                                            <th>Waiting From</th>
                                            <th>Waiting Time</th>
                                            <th>Location</th>
                                            <th>Lat/Long</th>
                                        </tr>";
                        int i = 0;
                        foreach (var item in group_item.ToList())
                        {
                            i++;
                            mailBody_HTML = mailBody_HTML +
                                        "<tr>" +
                                            "<td>" + i + "</td>" +
                                            "<td>" + item.depo.Name + "</td>" +
                                            "<td>" + item.vehicle.RegistrationNumber + "</td>" +
                                            "<td>" + String.Format("{0:dd/MM/yyyy H:mm}", item.vehicleTracking.Location_ChangedAt) + "</td>" +
                                            "<td>" + GetTimeDiffrence_HourMinute(item.vehicleTracking.Location_ChangedAt) + "</td>" +
                                            "<td><a href='http://3rdeye.prangroup.com:7698/Report/ShowMapWithMarker?latitude=" + item.vehicleTracking.Latitude + "&longitude=" + item.vehicleTracking.Longitude + "&target='_blank'>map</a>" + "</td>" +
                                            "<td>" + item.vehicleTracking.Latitude + "," + item.vehicleTracking.Longitude + "</td>" +
                                        "</tr>";
                        }
                        mailBody_HTML += @"
                            </table>";
                        mailBody_HTML += @"
                    </body>
                    </html>";
                        mail.Body = mailBody_HTML;
                        mail.IsBodyHtml = true;
                        sc.Send(mail);
                    }
                }
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                      new AppErrorLog()
                      {
                          ErrorMessage = e.Message,
                          ErrorTime = DateTime.Now,
                          UserDefinedMessage = "Tracking/TrySendOverStayOutSideAlertEmailToDepoGroupManager"
                      }
                    );
                bll.db.SaveChanges();
            }
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendOverStayOutSideAlertEmailToDepoGroupManager2-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            return "Sent ";
        }
        string GetTimeDiffrence_HourMinute(DateTime? dateTime)
        {
            var totalMinute = (long)(DateTime.Now - (dateTime ?? DateTime.Now)).TotalMinutes;
            var hour = (int)(totalMinute / 60);
            var minute = (int)(totalMinute % 60);
            return hour + " hour(s) " + minute + " minute(s)";
        }
        #endregion

        #region Mail Disconnected Device 
        public string TrySendDisconnectedDeviceAlertEmail()
        {
            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendDisconnectedDeviceAlertEmail-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();

            try
            {
                var _email = "automation@mis.prangroup.com";
                var _epass = "aaaaAAAA0000";

                SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                sc.EnableSsl = false;
                sc.Credentials = new NetworkCredential(_email, _epass);
                sc.Port = 25;

                var limit = DateTime.Now.Date.AddDays(-7);
                var PRG_Type = "";
                var list = new List<Vehicle>();
                //# PRAN 
                PRG_Type = "PRAN";
                list = (from vt in bll.db.VehicleTrackings.Where(m => m.UpdateTime < limit)
                        join v in bll.db.Vehicles.Where(m => m.Depo.PRG_Type == PRG_Type && m.GpsIMEINumber != null) on vt.PK_Vehicle equals v.PK_Vehicle
                        select v).ToList();
                if (list.Count > 0)
                {
                    var Mail_Subject = "3rd EyE: Daily Disconnected VTS Report " + PRG_Type;
                    var Mail_ToList = new List<string>() {
                    "dist100@prangroup.com","dist98@prangroup.com","dist99@prangroup.com"
                    };
                    var Mail_CcList = new List<string>() { "automation17@mis.prangroup.com", "mis7@prangroup.com" };
                    MailMessage mail = new MailMessage();
                    foreach (var to in Mail_ToList)
                    {
                        mail.To.Add(to);
                    }
                    foreach (var to in Mail_CcList)
                    {
                        mail.CC.Add(to);
                    }

                    mail.From = new MailAddress(_email);
                    mail.Subject = Mail_Subject;
                    string url = "";
#if DEBUG       
                    url = ConfigurationManager.AppSettings["DEBUG_DOMAIN"];
#else
url = ConfigurationManager.AppSettings["LIVE_DOMAIN"];
#endif
                    url = url + @"Tracking/DisconnectedDeviceEmailBodyGenerator?PRG_Type=" + PRG_Type;
                    WebClient myWebClient = new WebClient();
                    byte[] myDataBuffer = myWebClient.DownloadData(url);
                    string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                    mail.Body = mailBody_HTML;
                    mail.IsBodyHtml = true;

                    sc.Send(mail);
                }

                //# RFL
                PRG_Type = "RFL";
                list = (from vt in bll.db.VehicleTrackings.Where(m => m.UpdateTime < limit)
                        join v in bll.db.Vehicles.Where(m => m.Depo.PRG_Type == PRG_Type && m.GpsIMEINumber != null) on vt.PK_Vehicle equals v.PK_Vehicle
                        select v).ToList();
                if (list.Count > 0)
                {
                    var Mail_Subject = "3rd EyE: Daily Disconnected VTS Report " + PRG_Type;
                    var Mail_ToList = new List<string>() {
                    "dist121@prangroup.com"
                    };
                    var Mail_CcList = new List<string>() { "automation17@mis.prangroup.com", "mis7@prangroup.com" };
                    MailMessage mail = new MailMessage();
                    foreach (var to in Mail_ToList)
                    {
                        mail.To.Add(to);
                    }
                    foreach (var to in Mail_CcList)
                    {
                        mail.CC.Add(to);
                    }
                    mail.From = new MailAddress(_email);
                    mail.Subject = Mail_Subject;
                    string url = "";
#if DEBUG       
                    url = ConfigurationManager.AppSettings["DEBUG_DOMAIN"];
#else
url = ConfigurationManager.AppSettings["LIVE_DOMAIN"];
#endif
                    url = url + @"Tracking/DisconnectedDeviceEmailBodyGenerator?PRG_Type=" + PRG_Type;
                    WebClient myWebClient = new WebClient();
                    byte[] myDataBuffer = myWebClient.DownloadData(url);
                    string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                    mail.Body = mailBody_HTML;
                    mail.IsBodyHtml = true;

                    sc.Send(mail);
                }
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                      new AppErrorLog()
                      {
                          ErrorMessage = e.Message,
                          ErrorTime = DateTime.Now,
                          UserDefinedMessage = "Tracking/TrySendDisconnectedDeviceAlertEmail"
                      }
                    );
                bll.db.SaveChanges();
            }
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendDisconnectedDeviceAlertEmail-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            return "Sent ";
        }
        public ActionResult DisconnectedDeviceEmailBodyGenerator(string PRG_Type)
        {
            var limit = DateTime.Now.Date.AddDays(-7);
            var list = (from vt in bll.db.VehicleTrackings.Where(m => m.UpdateTime < limit)
                        join v in bll.db.Vehicles.Where(m => m.Depo.PRG_Type == PRG_Type && m.GpsIMEINumber != null) on vt.PK_Vehicle equals v.PK_Vehicle
                        select v).ToList();
            ViewBag.Message_1 = "These " + list.Count + " VTS Devices are disconnected more than 7 days(last). Please, take N/A.";
            return View(list);
        }
        #endregion


    }
}