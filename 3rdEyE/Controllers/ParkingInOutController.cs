using _3rdEyE.ManagingTools;
using _3rdEyE.Models;
using _3rdEyE.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static _3rdEyE.Controllers.VehicleGateNewAPIController;

namespace _3rdEyE.Controllers
{
    public class ParkingInOutController : BaseController
    {
        List<int> BayNames = Enumerable.Range(1, 30).ToList(); //new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,  };
        Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" } };

        public ActionResult ParkingInOutDashboard_Parking()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<vw_ParkingInOut_Detail>();
            var query = bll.db.vw_ParkingInOut_Detail.Where(m => m.In_FK_CreatedByUser == CurrentUser.PK_User && m.Out_IssueDateTime == null).AsEnumerable();
            list = query.ToList();
            return View(list);
        }
        public ActionResult ParkingInOutDashboard_Parking2()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var _PK_Location = CurrentUser.FK_Location;//PIP
            var _PK_OutSideBuildings = bll.db.LocationBuildings.Where(m => m.Name.Contains("Out Side")).Select(m => m.PK_LocationBuilding).ToList();
            //# Building Data-Start
            var locationBuildingList = bll.db.LocationBuildings.Where(m => m.IsDeleted != true && m.FK_Location == _PK_Location).Select(m => new LocationBuildingUtilizaiton()
            {
                PK_LocationBuilding = m.PK_LocationBuilding,
                PRG_Type = m.PRG_Type,
                LocationBuildingName = m.Name,
                GraceVehicleCount = m.GraceVehicleCount ?? 0,
                TotalBayCount = m.LoadingBays.Where(n => n.IsDeleted != true).Count(),
                BusyBayCount = m.LoadingBays.Where(n => n.IsDeleted != true && n.LoadingBayUtilization != null && n.LoadingBayUtilization.FK_Vehicle != null).Count(),
            }).ToList();

            var lowerLimit = DateTime.Now.AddDays(-3);
            //Guid _PK_GateUser = CurrentUser.PK_User;//PIP - Parking MP Land
            //var parkingInOutDetailList_SelfLocation = bll.db.vw_ParkingInOut_Detail.Where(m => m.In_FK_CreatedByUser == _PK_GateUser && lowerLimit < m.In_IssueDateTime).ToList();
            var parkingInOutDetailList_SelfLocation = bll.db.vw_ParkingInOut_Detail.Where(m =>
                 m.FK_Location == _PK_Location
                && (
                    (m.Out_IssueDateTime == null)
                    || (m.BayAssign_FK_LocationBuilding != null && _PK_OutSideBuildings.Contains((long)m.BayAssign_FK_LocationBuilding) && m.In_IssueDateTime > lowerLimit)
                    || (m.BayAssign_FK_LocationBuilding != null && !_PK_OutSideBuildings.Contains((long)m.BayAssign_FK_LocationBuilding) && m.FactoryOut_IssueDateTime == null)
                )
            ).ToList();

            foreach (var item in locationBuildingList)
            {
                item.VehicleOnGoingCount = parkingInOutDetailList_SelfLocation
                    .Where(m => m.BayAssign_FK_LocationBuilding == item.PK_LocationBuilding
                        && m.Out_IssueDateTime != null
                        && m.FactoryIn_IssueDateTime == null
                ).Count();
#if DEBUG

#endif
                item.StillInsideVehicleCount = parkingInOutDetailList_SelfLocation.Where(m => m.BayAssign_FK_LocationBuilding == item.PK_LocationBuilding && m.FactoryIn_IssueDateTime != null && m.FactoryOut_IssueDateTime == null).Count();
                if (_PK_OutSideBuildings.Contains(item.PK_LocationBuilding) || (item.TotalBayCount + item.GraceVehicleCount) > (item.VehicleOnGoingCount + item.StillInsideVehicleCount))
                {
                    item.TakeMoreVehicle = true;
                }
            }
            //# Building Data-End
            //Guid _PK_GateUser = CurrentUser.PK_User;//PIP - Parking MP Land
            var parkingInOutDetailList_SelfUser = parkingInOutDetailList_SelfLocation.Where(m => m.In_FK_CreatedByUser == CurrentUser.PK_User && (
                    (m.Out_IssueDateTime == null)
                    || (m.BayAssign_FK_LocationBuilding != null && _PK_OutSideBuildings.Contains((long)m.BayAssign_FK_LocationBuilding) && m.In_IssueDateTime > lowerLimit)
                    || (m.BayAssign_FK_LocationBuilding != null && !_PK_OutSideBuildings.Contains((long)m.BayAssign_FK_LocationBuilding) && m.FactoryOut_IssueDateTime == null)
                )).ToList();
            return View(new Tuple<List<LocationBuildingUtilizaiton>, List<vw_ParkingInOut_Detail>>(locationBuildingList, parkingInOutDetailList_SelfUser));
        }

        public ActionResult ParkingInOutDashboard_Parking3()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var _PK_Location = CurrentUser.FK_Location;//PIP
            var _PK_OutSideBuildings = bll.db.LocationBuildings.Where(m => m.Name.Contains("Out Side")).Select(m => m.PK_LocationBuilding).ToList();
            //# Building Data-Start
            var locationBuildingList = bll.db.LocationBuildings.Where(m => m.IsDeleted != true && m.FK_Location == _PK_Location).Select(m => new LocationBuildingUtilizaiton()
            {
                PK_LocationBuilding = m.PK_LocationBuilding,
                PRG_Type = m.PRG_Type,
                LocationBuildingName = m.Name,
                GraceVehicleCount = m.GraceVehicleCount ?? 0,
                TotalBayCount = m.LoadingBays.Where(n => n.IsDeleted != true).Count(),
                BusyBayCount = m.LoadingBays.Where(n => n.IsDeleted != true && n.LoadingBayUtilization != null && n.LoadingBayUtilization.FK_Vehicle != null).Count(),
            }).ToList();

            var lowerLimit = DateTime.Now.AddDays(-3);
            //Guid _PK_GateUser = CurrentUser.PK_User;//PIP - Parking MP Land
            //var parkingInOutDetailList_SelfLocation = bll.db.vw_ParkingInOut_Detail.Where(m => m.In_FK_CreatedByUser == _PK_GateUser && lowerLimit < m.In_IssueDateTime).ToList();
            var parkingInOutDetailList_SelfLocation = bll.db.vw_ParkingInOut_Detail.Where(m =>
                 m.FK_Location == _PK_Location
                && (
                    (m.Out_IssueDateTime == null)
                    || (m.BayAssign_FK_LocationBuilding != null && _PK_OutSideBuildings.Contains((long)m.BayAssign_FK_LocationBuilding) && m.In_IssueDateTime > lowerLimit)
                    || (m.BayAssign_FK_LocationBuilding != null && !_PK_OutSideBuildings.Contains((long)m.BayAssign_FK_LocationBuilding) && m.FactoryOut_IssueDateTime == null)
                )
            ).ToList();

            foreach (var item in locationBuildingList)
            {
                item.VehicleOnGoingCount = parkingInOutDetailList_SelfLocation
                    .Where(m => m.BayAssign_FK_LocationBuilding == item.PK_LocationBuilding
                        && m.Out_IssueDateTime != null
                        && m.FactoryIn_IssueDateTime == null
                ).Count();
#if DEBUG

#endif
                item.StillInsideVehicleCount = parkingInOutDetailList_SelfLocation.Where(m => m.BayAssign_FK_LocationBuilding == item.PK_LocationBuilding && m.FactoryIn_IssueDateTime != null && m.FactoryOut_IssueDateTime == null).Count();
                if (_PK_OutSideBuildings.Contains(item.PK_LocationBuilding) || (item.TotalBayCount + item.GraceVehicleCount) > (item.VehicleOnGoingCount + item.StillInsideVehicleCount))
                {
                    item.TakeMoreVehicle = true;
                }
            }
            //# Building Data-End
            //Guid _PK_GateUser = CurrentUser.PK_User;//PIP - Parking MP Land
            var parkingInOutDetailList_SelfUser = parkingInOutDetailList_SelfLocation.Where(m => m.In_FK_CreatedByUser == CurrentUser.PK_User && (
                    (m.Out_IssueDateTime == null)
                    || (m.BayAssign_FK_LocationBuilding != null && _PK_OutSideBuildings.Contains((long)m.BayAssign_FK_LocationBuilding) && m.In_IssueDateTime > lowerLimit)
                    || (m.BayAssign_FK_LocationBuilding != null && !_PK_OutSideBuildings.Contains((long)m.BayAssign_FK_LocationBuilding) && m.FactoryOut_IssueDateTime == null)
                )).ToList();
            return View(new Tuple<List<LocationBuildingUtilizaiton>, List<vw_ParkingInOut_Detail>>(locationBuildingList, parkingInOutDetailList_SelfUser));
        }

        public class LocationBuildingUtilizaiton
        {
            public Int64 PK_LocationBuilding { get; set; }
            public string PRG_Type { get; set; }
            public string LocationBuildingName { get; set; }
            public int GraceVehicleCount { get; set; }
            public int TotalBayCount { get; set; }
            public int BusyBayCount { get; set; }
            public int StillInsideVehicleCount { get; set; }
            public int VehicleOnGoingCount { get; set; }
            public bool TakeMoreVehicle = false;
        }

        public ActionResult ParkingEntryReport(String PRG_Type, String FK_Location, String FK_LocationDepartment, string FK_LocationBuilding, String FK_Depo, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var _PK_Location = string.IsNullOrEmpty(FK_Location) ? CurrentUser.FK_Location : Guid.Parse(FK_Location);
            var _PK_OutSideBuildings = bll.db.LocationBuildings.Where(m => m.Name.Contains("Out Side")).Select(m => m.PK_LocationBuilding).ToList();
            //# Building Data-Start
            var locationBuildingList = bll.db.LocationBuildings.Where(m => m.IsDeleted != true && m.FK_Location == _PK_Location).Select(m => new LocationBuildingUtilizaiton()
            {
                PK_LocationBuilding = m.PK_LocationBuilding,
                PRG_Type = m.PRG_Type,
                LocationBuildingName = m.Name,
                GraceVehicleCount = m.GraceVehicleCount ?? 0,
                TotalBayCount = m.LoadingBays.Where(n => n.IsDeleted != true).Count(),
                BusyBayCount = m.LoadingBays.Where(n => n.IsDeleted != true && n.LoadingBayUtilization != null && n.LoadingBayUtilization.FK_Vehicle != null).Count(),
            }).ToList();

            var lowerLimit = DateTime.Now.AddDays(-3);
            //Guid _PK_GateUser = Guid.Parse("7383D883-6C15-4A13-A318-80CC8CAA16A4");//PIP - Parking MP Land
            var parkingInOutDetailList_SelfLocation = bll.db.vw_ParkingInOut_Detail.Where(m =>
                 m.FK_Location == _PK_Location
                && (
                    (m.Out_IssueDateTime == null)
                    || (m.BayAssign_FK_LocationBuilding != null && _PK_OutSideBuildings.Contains((long)m.BayAssign_FK_LocationBuilding) && m.In_IssueDateTime > lowerLimit)
                    || (m.BayAssign_FK_LocationBuilding != null && !_PK_OutSideBuildings.Contains((long)m.BayAssign_FK_LocationBuilding) && m.FactoryOut_IssueDateTime == null)
                )
            ).ToList();

            foreach (var item in locationBuildingList)
            {
                item.VehicleOnGoingCount = parkingInOutDetailList_SelfLocation
                    .Where(m => m.BayAssign_FK_LocationBuilding == item.PK_LocationBuilding
                        && m.Out_IssueDateTime != null
                        && m.FactoryIn_IssueDateTime == null
                ).Count();
                item.StillInsideVehicleCount = parkingInOutDetailList_SelfLocation.Where(m => m.BayAssign_FK_LocationBuilding == item.PK_LocationBuilding && m.FactoryIn_IssueDateTime != null && m.FactoryOut_IssueDateTime == null).Count();
                if (_PK_OutSideBuildings.Contains(item.PK_LocationBuilding) || (item.TotalBayCount + item.GraceVehicleCount) > (item.VehicleOnGoingCount + item.StillInsideVehicleCount))
                {
                    item.TakeMoreVehicle = true;
                }
            }
            //# Building Data-End

            var list = new List<vw_ParkingInOut_Detail>();
            var query = bll.db.vw_ParkingInOut_Detail.Where(m =>
                 m.FK_Location == _PK_Location
                && (
                    (m.Out_IssueDateTime == null)
                    || (m.BayAssign_FK_LocationBuilding != null && _PK_OutSideBuildings.Contains((long)m.BayAssign_FK_LocationBuilding) && m.In_IssueDateTime > lowerLimit)
                    || (m.BayAssign_FK_LocationBuilding != null && !_PK_OutSideBuildings.Contains((long)m.BayAssign_FK_LocationBuilding) && m.FactoryOut_IssueDateTime == null)
                )
            ).AsEnumerable();

            //FK_Location
            //if (FK_Location != null && FK_Location != "null")
            //{
            //    var _FK_Location = Guid.Parse(FK_Location);
            //    query = query.Where(m => m.FK_Location == _FK_Location);
            //}
            List<SelectListItem> LocationList = new List<SelectListItem>();
            LocationList.Add(new SelectListItem() { Value = "null", Text = "-" });
            LocationList.AddRange(bll.db.AppUsers.AsEnumerable().Where(n => n.IsDeleted != true && n.AppUserType == "Internal Parking Entry Device").Select(m => m.Location).Distinct().Where(m => m.IsDeleted != true).Select(m => new SelectListItem { Value = m.PK_Location.ToString(), Text = m.Name }));
            ViewBag.Locations = new SelectList(LocationList.OrderBy(m => m.Text), "Value", "Text", _PK_Location);


            //FK_LocationDepartment
            if (FK_LocationDepartment != null && FK_LocationDepartment != "null")
            {
                var _FK_LocationDepartment = Convert.ToInt64(FK_LocationDepartment);
                query = query.Where(m => m.BayAssign_FK_LocationDepartment == _FK_LocationDepartment || m.Requisition_PK_LocationDepartment == _FK_LocationDepartment);
                //var _FK_Location = Guid.Parse(FK_Location);

                List<SelectListItem> LocationDepartmentList = new List<SelectListItem>();
                LocationDepartmentList.Add(new SelectListItem() { Value = "null", Text = "-" });
                LocationDepartmentList.AddRange(bll.db.LocationDepartments.AsEnumerable().Where(n => n.FK_Location == _PK_Location).Select(m => new SelectListItem { Value = m.PK_LocationDepartment.ToString(), Text = m.DepartmentCode }));
                ViewBag.LocationDepartments = new SelectList(LocationDepartmentList.OrderBy(m => m.Text), "Value", "Text", FK_LocationDepartment);
            }
            else
            {
                List<SelectListItem> LocationDepartmentList = new List<SelectListItem>();
                LocationDepartmentList.Add(new SelectListItem() { Value = "null", Text = "-" });
                LocationDepartmentList.AddRange(bll.db.LocationDepartments.AsEnumerable().Where(n => n.PK_LocationDepartment == null).Select(m => new SelectListItem { Value = m.PK_LocationDepartment.ToString(), Text = m.DepartmentCode }));
                ViewBag.LocationDepartments = new SelectList(LocationDepartmentList.OrderBy(m => m.Text), "Value", "Text", FK_LocationDepartment);
            }
            //FK_LocationBuilding
            if (FK_LocationBuilding != null && FK_LocationBuilding != "null")
            {
                var _FK_LocationBuilding = Convert.ToInt64(FK_LocationBuilding);
                query = query.Where(m => m.BayAssign_FK_LocationBuilding == _FK_LocationBuilding);
                //var _FK_Location = Guid.Parse(FK_Location);

                List<SelectListItem> LocationBuildingList = new List<SelectListItem>();
                LocationBuildingList.Add(new SelectListItem() { Value = "null", Text = "-" });
                LocationBuildingList.AddRange(bll.db.LocationBuildings.AsEnumerable().Where(n => n.FK_Location == _PK_Location).Select(m => new SelectListItem { Value = m.PK_LocationBuilding.ToString(), Text = m.Name }));
                ViewBag.LocationBuildings = new SelectList(LocationBuildingList.OrderBy(m => m.Text), "Value", "Text", FK_LocationBuilding);
            }
            else
            {
                List<SelectListItem> LocationBuildingList = new List<SelectListItem>();
                LocationBuildingList.Add(new SelectListItem() { Value = "null", Text = "-" });
                LocationBuildingList.AddRange(bll.db.LocationBuildings.AsEnumerable().Where(n => n.PK_LocationBuilding == null).Select(m => new SelectListItem { Value = m.PK_LocationBuilding.ToString(), Text = m.Name }));
                ViewBag.LocationBuildings = new SelectList(LocationBuildingList.OrderBy(m => m.Text), "Value", "Text", FK_LocationBuilding);
            }


            //PRG_Type
            if (PRG_Type != null && PRG_Type != "null")
            {
                query = query.Where(m => m.PRG_Type == PRG_Type);
            }
            List<SelectListItem> PRG_TypeList = new List<SelectListItem>();
            PRG_TypeList.Add(new SelectListItem() { Value = "null", Text = "-" });
            PRG_TypeList.AddRange(PRG_TypesDict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key.ToString(), Text = m.Value }));
            ViewBag.PRG_Type = new SelectList(PRG_TypeList.OrderBy(m => m.Text), "Value", "Text", PRG_Type);

            //FK_Depo
            if (FK_Depo != null && FK_Depo != "null")
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                query = query.Where(m => m.FK_Depo == _FK_Depo);
            }
            List<SelectListItem> DepoList = new List<SelectListItem>();
            DepoList.Add(new SelectListItem() { Value = "null", Text = "-" });
            DepoList.AddRange(bll.db.Depoes.AsEnumerable().Where(n => n.IsDeleted != true).Select(m => new SelectListItem { Value = m.PK_Depo.ToString(), Text = m.Name }));
            ViewBag.Depoes = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text", FK_Depo);

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //final
            if (PRG_Type != null || FK_Location != null || RegistrationNumber != null)
            {
                list = query.OrderBy(m => m.In_IssueDateTime).ToList();
            }
            //return View(list);
            return View(new Tuple<List<LocationBuildingUtilizaiton>, List<vw_ParkingInOut_Detail>>(locationBuildingList, list));
        }
        public ActionResult ParkingInOutHistory(string StartingDate, string EndingDate, String PRG_Type, String FK_Location, String FK_Depo, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<vw_ParkingInOut>();

            var query = bll.db.vw_ParkingInOut.AsEnumerable();

            //FK_Location
            if (!string.IsNullOrEmpty(FK_Location))
            {
                var _FK_Location = Guid.Parse(FK_Location);
                query = query.Where(m => m.FK_Location == _FK_Location);
            }
            List<SelectListItem> LocationList = new List<SelectListItem>();
            LocationList.AddRange(bll.db.AppUsers.AsEnumerable().Where(n => n.IsDeleted != true && n.AppUserType == "Internal Parking Entry Device").Select(m => m.Location).Distinct().Where(m => m.IsDeleted != true).Select(m => new SelectListItem { Value = m.PK_Location.ToString(), Text = m.Name }));
            ViewBag.Locations = new SelectList(LocationList.OrderBy(m => m.Text), "Value", "Text", FK_Location);

            //StartingDate
            if (!string.IsNullOrEmpty(StartingDate))
            {
                var _StartingDate = Convert.ToDateTime(StartingDate);
                query = query.Where(m => m.In_IssueDateTime >= _StartingDate);
            }

            //EndingDate
            if (!string.IsNullOrEmpty(EndingDate))
            {
                var _EndingDate = Convert.ToDateTime(EndingDate);
                query = query.Where(m => m.In_IssueDateTime <= _EndingDate);
            }

            //PRG_Type
            if (!string.IsNullOrEmpty(PRG_Type))
            {
                query = query.Where(m => m.PRG_Type == PRG_Type);
            }
            List<SelectListItem> PRG_TypeList = new List<SelectListItem>();
            PRG_TypeList.AddRange(PRG_TypesDict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key.ToString(), Text = m.Value }));
            ViewBag.PRG_Type = new SelectList(PRG_TypeList.OrderBy(m => m.Text), "Value", "Text", PRG_Type);

            //FK_Depo
            if (!string.IsNullOrEmpty(FK_Depo))
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                query = query.Where(m => m.FK_Depo == _FK_Depo);
            }
            List<SelectListItem> DepoList = new List<SelectListItem>();
            DepoList.AddRange(bll.db.Depoes.AsEnumerable().Where(n => n.IsDeleted != true).Select(m => new SelectListItem { Value = m.PK_Depo.ToString(), Text = m.Name }));
            ViewBag.Depoes = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text", FK_Depo);

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //final
            if (StartingDate != null || EndingDate != null || PRG_Type != null || FK_Location != null || FK_Depo != null || RegistrationNumber != null)
            {
                list = query.OrderBy(m => m.In_IssueDateTime).ToList();
            }
            return View(list);
        }

        public ActionResult SetBayAssignTime(Int64 PK_ParkingInOut)
        {
            var model = bll.db.ParkingInOuts.Where(m => m.PK_ParkingInOut == PK_ParkingInOut).FirstOrDefault();
            ViewBag.LocationBuildings = new SelectList(bll.db.LocationBuildings.Where(m => m.PRG_Type == CurrentUser.PRG_Type && m.FK_Location == model.FK_Location), "PK_LocationBuilding", "Name");


            if (model.FK_RequisitionTrip != null)
            {
                var requisitionTrip = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == model.FK_RequisitionTrip).FirstOrDefault();
                if (requisitionTrip != null && requisitionTrip.Requisition.FK_Location_From == model.FK_Location)
                {
                    ViewBag.LocationDepartments = new SelectList(bll.db.LocationDepartments.Where(m => m.PRG_Type == CurrentUser.PRG_Type && m.FK_Location == model.FK_Location), "PK_LocationDepartment", "DepartmentName", requisitionTrip.Requisition.FK_LocationDepartment_From);
                }
                else if (requisitionTrip != null && requisitionTrip.Requisition.FK_Location_To == model.FK_Location)
                {
                    ViewBag.LocationDepartments = new SelectList(bll.db.LocationDepartments.Where(m => m.PRG_Type == CurrentUser.PRG_Type && m.FK_Location == model.FK_Location), "PK_LocationDepartment", "DepartmentName", requisitionTrip.Requisition.FK_LocationDepartment_To);
                }
                else
                {
                    ViewBag.LocationDepartments = new SelectList(bll.db.LocationDepartments.Where(m => m.PRG_Type == CurrentUser.PRG_Type && m.FK_Location == model.FK_Location), "PK_LocationDepartment", "DepartmentName");
                }
            }
            else
            {
                ViewBag.LocationDepartments = new SelectList(bll.db.LocationDepartments.Where(m => m.PRG_Type == CurrentUser.PRG_Type && m.FK_Location == model.FK_Location), "PK_LocationDepartment", "DepartmentName");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult SetBayAssignTime(FormCollection form)
        {
            var _pk = Convert.ToInt64(form["PK_ParkingInOut"]);
            var model = bll.db.ParkingInOuts.Where(m => m.PK_ParkingInOut == _pk).FirstOrDefault();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                if (model.BayAssign_IssueDateTime == null)
                {
                    if (!string.IsNullOrEmpty(form["BayAssign_FK_LocationBuilding"]))
                    {
                        model.BayAssign_FK_LocationBuilding = Convert.ToInt64(form["BayAssign_FK_LocationBuilding"]);
                    }
                    if (!string.IsNullOrEmpty(form["BayAssign_FK_LoadingBay"]))
                    {
                        model.BayAssign_FK_LoadingBay = Convert.ToInt64(form["BayAssign_FK_LoadingBay"]);
                    }

                    if (!string.IsNullOrEmpty(form["BayAssign_FK_LocationDepartment"]))
                    {
                        model.BayAssign_FK_LocationDepartment = Convert.ToInt64(form["BayAssign_FK_LocationDepartment"]);
                    }
                    model.BayAssign_IssueDateTime = DateTime.ParseExact(form["BayAssign_IssueDateTime"], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);

                    model.BayAssign_CreatedDateTime = DateTime.Now;
                    model.BayAssign_FK_CreatedByUser = CurrentUser.PK_User;
                    bll.db.SaveChanges();

                    CreateAlertMessage(AlertMessageType.Success, "Success", "Assigning bay is successfully done.");
                    return RedirectToAction("ParkingEntryReport");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Bay is already assigned.");
                    return RedirectToAction("ParkingEntryReport");
                }
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return View(model);
        }


        public ActionResult ChangeBayAssignTime(Int64 PK_ParkingInOut)
        {
            var model = bll.db.ParkingInOuts.Where(m => m.PK_ParkingInOut == PK_ParkingInOut).FirstOrDefault();
            if (model.FactoryIn_IssueDateTime != null)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Vehicle has already entered factory.");
                return RedirectToAction("ParkingEntryReport");
            }
            ViewBag.LocationBuildings = new SelectList(bll.db.LocationBuildings.Where(m => m.PRG_Type == CurrentUser.PRG_Type && m.FK_Location == model.FK_Location), "PK_LocationBuilding", "Name", model.BayAssign_FK_LocationBuilding);
            ViewBag.LoadingBays = new SelectList(bll.db.LoadingBays.Where(m => m.FK_LocationBuilding == model.BayAssign_FK_LocationBuilding), "PK_LoadingBay", "Name", model.BayAssign_FK_LoadingBay);
            ViewBag.LocationDepartments = new SelectList(bll.db.LocationDepartments.Where(m => m.PRG_Type == CurrentUser.PRG_Type && m.FK_Location == model.FK_Location), "PK_LocationDepartment", "DepartmentName", model.BayAssign_FK_LocationDepartment);
            return View(model);
        }
        [HttpPost]
        public ActionResult ChangeBayAssignTime(FormCollection form)
        {
            var _pk = Convert.ToInt64(form["PK_ParkingInOut"]);
            var model = bll.db.ParkingInOuts.Where(m => m.PK_ParkingInOut == _pk).FirstOrDefault();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                if (model.FactoryIn_IssueDateTime == null)
                {
                    if (!string.IsNullOrEmpty(form["BayAssign_FK_LocationBuilding"]))
                    {
                        model.BayAssign_FK_LocationBuilding = Convert.ToInt64(form["BayAssign_FK_LocationBuilding"]);
                    }
                    if (!string.IsNullOrEmpty(form["BayAssign_FK_LoadingBay"]))
                    {
                        model.BayAssign_FK_LoadingBay = Convert.ToInt64(form["BayAssign_FK_LoadingBay"]);
                    }
                    else
                    {
                        model.BayAssign_FK_LoadingBay = null;
                    }

                    if (!string.IsNullOrEmpty(form["BayAssign_FK_LocationDepartment"]))
                    {
                        model.BayAssign_FK_LocationDepartment = Convert.ToInt64(form["BayAssign_FK_LocationDepartment"]);
                    }
                    model.BayAssign_IssueDateTime = DateTime.ParseExact(form["BayAssign_IssueDateTime"], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);

                    model.BayAssign_UpdateDateTime = DateTime.Now;
                    model.BayAssign_FK_UpdatedByUser = CurrentUser.PK_User;
                    bll.db.SaveChanges();

                    CreateAlertMessage(AlertMessageType.Success, "Success", "Assigning bay is successfully done.");
                    return RedirectToAction("ParkingEntryReport");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Vehicle has already left parking.");
                    return RedirectToAction("ParkingEntryReport");
                }
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return View(model);
        }
        public ActionResult ChangeBayAssignTimeForRassign(Int64 PK_ParkingInOut)
        {
            var model = bll.db.ParkingInOuts.Where(m => m.PK_ParkingInOut == PK_ParkingInOut).FirstOrDefault();
            if (model.FactoryOut_IssueDateTime != null)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Vehicle has already exit from factory.");
                return RedirectToAction("ParkingEntryReport");
            }
            ViewBag.LocationBuildings = new SelectList(bll.db.LocationBuildings.Where(m => m.PRG_Type == CurrentUser.PRG_Type && m.FK_Location == model.FK_Location), "PK_LocationBuilding", "Name", model.BayAssign_FK_LocationBuilding);
            ViewBag.LoadingBays = new SelectList(bll.db.LoadingBays.Where(m => m.FK_LocationBuilding == model.BayAssign_FK_LocationBuilding), "PK_LoadingBay", "Name", model.BayAssign_FK_LoadingBay);
            ViewBag.LocationDepartments = new SelectList(bll.db.LocationDepartments.Where(m => m.PRG_Type == CurrentUser.PRG_Type && m.FK_Location == model.FK_Location), "PK_LocationDepartment", "DepartmentName", model.BayAssign_FK_LocationDepartment);
            return View(model);
        }
        [HttpPost]
        public ActionResult ChangeBayAssignTimeForRassign(FormCollection form)
        {
            var _pk = Convert.ToInt64(form["PK_ParkingInOut"]);
            var model = bll.db.ParkingInOuts.Where(m => m.PK_ParkingInOut == _pk).FirstOrDefault();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                if (model.FactoryIn_IssueDateTime != null && model.FactoryOut_IssueDateTime == null)
                {
                    if (!string.IsNullOrEmpty(form["BayAssign_FK_LocationBuilding"]))
                    {
                        model.BayAssign_FK_LocationBuilding = Convert.ToInt64(form["BayAssign_FK_LocationBuilding"]);
                    }
                    if (!string.IsNullOrEmpty(form["BayAssign_FK_LoadingBay"]))
                    {
                        model.BayAssign_FK_LoadingBay = Convert.ToInt64(form["BayAssign_FK_LoadingBay"]);
                    }
                    else
                    {
                        model.BayAssign_FK_LoadingBay = null;
                    }

                    if (!string.IsNullOrEmpty(form["BayAssign_FK_LocationDepartment"]))
                    {
                        model.BayAssign_FK_LocationDepartment = Convert.ToInt64(form["BayAssign_FK_LocationDepartment"]);
                    }
                    model.BayAssign_IssueDateTime = DateTime.ParseExact(form["BayAssign_IssueDateTime"], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);

                    model.BayAssign_UpdateDateTime = DateTime.Now;
                    model.BayAssign_FK_UpdatedByUser = CurrentUser.PK_User;

                    model.FK_LoadingBayUtilization = null;
                    bll.db.SaveChanges();

                    CreateAlertMessage(AlertMessageType.Success, "Success", "Re-assigning bay is successfully done.");
                    return RedirectToAction("ParkingEntryReport");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Vehicle has already left parking.");
                    return RedirectToAction("ParkingEntryReport");
                }
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return View(model);
        }

        public ActionResult ParkingInOutDashboard_NonParking()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var startingLimit = DateTime.Now.AddDays(-1);
            var list = new List<vw_ParkingInOut_Detail>();
            var query = bll.db.vw_ParkingInOut_Detail.Where(m => m.FK_Location == CurrentUser.FK_Location && m.In_IssueDateTime > startingLimit).AsEnumerable();
            list = query.ToList();
            return View(list);
        }

        public ActionResult GateOutFromParking(Int64 PK_ParkingInOut, String StaffId, String Note = "")
        {
            var _parkingInOut = bll.db.ParkingInOuts.Where(m => m.PK_ParkingInOut == PK_ParkingInOut).FirstOrDefault();
            var _vehicle = _parkingInOut.Vehicle;
            var _vehicleInOutManual = _vehicle.VehicleInOutManual;

            GateInOutModel gateOutModel = new GateInOutModel();
            gateOutModel.FK_CreatedByUser = CurrentUser.PK_User.ToString();
            gateOutModel.RegistrationNumber = _vehicle.RegistrationNumber;
            gateOutModel.IsFormatedRegistrationNumber = "yes";

            //# SAME AS VehicleGateNewAPIController.GateOut(GateInOutModel gateOutModel);
            {
                var _PK_User = Guid.Parse(gateOutModel.FK_CreatedByUser);
                var gateUser = bll.db.AppUsers.Where(m => m.PK_User == _PK_User).FirstOrDefault();
                //gateUser.AppVersionCode = gateOutModel.AppVersionCode;

                bll.db.SaveChanges();
                var AppVersionCode_Current = bll.db.AppSettings.Where(m => m.Name == "AppVersionCode_Current" && m.IsActive == true).Select(m => m.Value).FirstOrDefault();
                var AppVersionCode_OldUsable = bll.db.AppSettings.Where(m => m.Name == "AppVersionCode_OldUsable" && m.IsActive == true).Select(m => m.Value).ToList();

                var flag = "n/a";
                var message = "";
                //var version_message = "";
                //if (!(gateOutModel.AppVersionCode == AppVersionCode_Current || AppVersionCode_OldUsable.Contains(gateOutModel.AppVersionCode)))
                //{
                //    flag = "appversion_not_usable";
                //    message = "বিফল হয়েছে। সফটয়ারটি আপডেট না করলে এন্ট্রী দেওয়া যাবে না। শীঘ্রই ট্রান্সপোর্ট ডিপার্ট্মেন্টকে জানান।";
                //    //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                //}
                try
                {
                    //gateOutModel.RegistrationNumber = gateOutModel.RegistrationNumber.ToUpper();

                    //# Conversion
                    //TemporaryVehicle temporaryVehicle = new TemporaryVehicle();
                    //temporaryVehicle.RegistrationNumber = _vehicle.RegistrationNumber;//gateOutModel.IsFormatedRegistrationNumber == "yes" ? gateOutModel.RegistrationNumber : FormatRegistrationNumber(gateOutModel.RegistrationNumber);
                    //temporaryVehicle.FK_VehicleInOutManualReason = _vehicleInOutManual.In_FK_VehicleInOutManualReason;//Convert.ToInt64(gateOutModel.FK_VehicleInOutManualReason);
                    //temporaryVehicle.LoadOrEmpty = _vehicleInOutManual.In_LoadOrEmpty;//gateOutModel.LoadOrEmpty.Split('/')[1];
                    //temporaryVehicle.IssueDateTime = DateTime.Now;
                    //temporaryVehicle.IsScannedEntry = false;//gateOutModel.IsFormatedRegistrationNumber == "yes" ? true : false;
                    //temporaryVehicle.FK_CreatedByLocationGate = Guid.Parse(gateOutModel.FK_CreatedByUser);
                    //temporaryVehicle.GPNumber = gateOutModel.GPNumber;

                    var RegistrationNumber = _vehicle.RegistrationNumber;
                    var vehicle = _vehicle;//bll.db.Vehicles.Where(m => m.RegistrationNumber == RegistrationNumber).FirstOrDefault();
                    //temporaryVehicle.FK_Locaiton = gateUser.FK_Location;

                    //if (vehicle == null)
                    //{
                    //    var temppraryVehicle = bll.db.TemporaryVehicles.Where(m => m.RegistrationNumber == temporaryVehicle.RegistrationNumber).FirstOrDefault();
                    //    if (temppraryVehicle != null)
                    //    {
                    //        flag = "vehicle_not_confirmed";
                    //        message = "বিফল হয়েছে। শীঘ্রই ট্রান্সপোর্ট ডিপার্ট্মেন্টকে গাড়ির তথ্য হালনাগাদ করতে বলুন।";
                    //        //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                    //    }
                    //    else
                    //    {
                    //        flag = "vehicle_not_found";
                    //        message = "বিফল হয়েছে। গাড়ি খুজে পাওয়া যায়নি।";
                    //        //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                    //    }
                    //}
                    if (vehicle.LocationInOrOut == false || vehicle.FK_LocationInOut != gateUser.FK_Location)
                    {
                        if (vehicle.LocationInOrOut == true && vehicle.FK_LocationInOut != null)
                        {
                            flag = "vehicle_is_in_another_dipo";
                            message = "বিফল হয়েছে। গাড়িটি " + bll.db.Locations.Where(m => m.PK_Location == vehicle.FK_LocationInOut).FirstOrDefault().Name + " লোকেশনের ভিতরে রয়েছে।";
                            //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            flag = "vehicle_not_in_this_dipo";
                            message = "বিফল হয়েছে। গাড়ি এই লোকেশনের বাহিরে।";
                            //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    ////#GPNumber checking#
                    //var tripWithGpNumber = bll.db.RequisitionTrips.Where(m => m.OracleDB_GPNumber == temporaryVehicle.GPNumber && m.IsGatePassUsed == true).FirstOrDefault();
                    //if (gateOutModel.GPNumber != "Not Given" && tripWithGpNumber != null)
                    //{
                    //    flag = "gp_alrady_used";
                    //    message = "বিফল হয়েছে। জিপি নাম্বার: " + temporaryVehicle.GPNumber + " ইতিমধ্যে ব্যাবহার হয়েছে।";
                    //}
                    ////#GPNumber Checking*
                    //if (gateOutModel.GPNumber != "Not Given")
                    //{
                    //    var usedGPVehicleInOutManual = bll.db.VehicleInOutManuals.Where(m => m.InOrOut == false && m.GPNumber == gateOutModel.GPNumber).FirstOrDefault();
                    //    if (usedGPVehicleInOutManual != null)
                    //    {
                    //        flag = "gp_number_already_used";
                    //        message = "বিফল হয়েছে। জিপি নাম্বারটি পূর্বে ব্যবহৃত।";
                    //        //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                    //    }
                    //    try
                    //    {
                    //        var res = "";
                    //        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://172.17.107.254:808/api/dgx/");
                    //        httpWebRequest.ContentType = "application/json";
                    //        httpWebRequest.Method = "POST";

                    //        httpWebRequest.Headers["ss"] = "DGTX";
                    //        httpWebRequest.Headers["yy"] = "HJDyh876Yhd765JHdgeoOUE765487sf543GDJksn";

                    //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    //        {
                    //            string json = new JavaScriptSerializer().Serialize(new
                    //            {
                    //                dgno = gateOutModel.GPNumber
                    //            });

                    //            streamWriter.Write(json);
                    //        }

                    //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    //        {
                    //            res = streamReader.ReadToEnd();
                    //        }
                    //        dynamic data_json = JObject.Parse(res);

                    //        if (data_json.Status == "N") // else data_json.Status = "Y"
                    //        {
                    //            flag = "gp_not_found";
                    //            message = "বিফল হয়েছে। জিপি নাম্বারটি সঠিক নয়।";
                    //            //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                    //        }
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        var error_message = e.Message;
                    //        while (e.InnerException != null) { e = e.InnerException; error_message = e.Message; }
                    //        bll.db.AppErrorLogs.Add(
                    //            new AppErrorLog()
                    //            {
                    //                ErrorMessage = error_message,
                    //                ErrorTime = DateTime.Now,
                    //                UserDefinedMessage = "VehicleGateNewAPI/GateIn FK_AppUser:" + gateOutModel.FK_CreatedByUser
                    //            }
                    //            );
                    //        bll.db.SaveChanges();

                    //        flag = "gp_checking_error";
                    //        message = "বিফল হয়েছে। MIS ডিপার্ট্মেন্টকে জানান(জিপি সার্ভার সমস্যা)।";
                    //        //return Json(new
                    //        {
                    //            flag,
                    //            message,
                    //            error_message
                    //        }, JsonRequestBehavior.AllowGet);
                    //    }

                    //}
                    //#GPNumber checking*

                    DataTable dataTable = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = int.MaxValue;
                    SqlDataAdapter adpt = new SqlDataAdapter();
                    cmd.Connection = (SqlConnection)bll.db.Database.Connection;
                    string query = $"EXEC VehicleInOutManual_Out " +
                    $"@DevelopersNote = 'ParkingInOutController/GateOutFromParking_by_SP/VehicleInOutManual-Out'," +
                    $"@FK_Vehicle = '{vehicle.PK_Vehicle}'," +
                    $"@InOrOut = '0'," +
                    $"@Out_FK_CreatedByUser = '{gateOutModel.FK_CreatedByUser}'," +
                    $"@Out_LoadOrEmpty = '{_vehicleInOutManual.In_LoadOrEmpty}'," +
                    $"@Out_FK_VehicleInOutManualReason = '{_vehicleInOutManual.In_FK_VehicleInOutManualReason}'," +
                    $"@Out_IssueDateTime = '{DateTime.Now}'," +
                    $"@Out_IsScannedEntry = '0'," +
                    $"@GPNumber = '{gateOutModel.GPNumber}'," +
                    $"@Out_DeviceId = 'Web',";
                    if (!string.IsNullOrEmpty(StaffId))
                    {
                        query = query + $"@Out_DriverStaffId = '{StaffId}',";
                    }
                    query = query + $"@Out_Note = '{Note}';";
                    cmd.CommandText = query;
                    adpt.SelectCommand = cmd;
                    adpt.Fill(dataTable);
                    if (dataTable.Rows[0]["return_status"].ToString() == "OK")
                    {
                        if (gateUser.AppUserType == "Internal Gate Entry Device")
                        {
                            try
                            {
                                new RequisitionController().RequisitionTrip_StartMulti(vehicle.PK_Vehicle, gateUser.FK_Location ?? new Guid(), gateUser.PK_User);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        flag = "success";
                        message = "বাহির হওয়ার এন্ট্রি সফল হয়েছে।";
                        //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        flag = "error";
                        message = "বিফল হয়েছে। " + dataTable.Rows[0]["return_status"].ToString() + "  " + dataTable.Rows[0]["return_mesage"].ToString();
                        //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    var error_message = e.Message;
                    while (e.InnerException != null) { e = e.InnerException; error_message = e.Message; }
                    bll.db.AppErrorLogs.Add(
                        new AppErrorLog()
                        {
                            ErrorMessage = error_message,
                            ErrorTime = DateTime.Now,
                            UserDefinedMessage = "VehicleGateNewAPI/GateIn FK_AppUser:" + gateOutModel.FK_CreatedByUser
                        }
                        );
                    bll.db.SaveChanges();

                    flag = "internal_error";
                    message = "বিফল হয়েছে। MIS ডিপার্ট্মেন্টকে জানান(থার্ড আই সার্ভার সমস্যা)।";
                    //return Json(new { flag, message, error_message }, JsonRequestBehavior.AllowGet);
                }
                if (flag == "success")
                {
                    CreateAlertMessage(AlertMessageType.Success, "Success", message);
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", message);
                }
            }
            if (string.IsNullOrEmpty(StaffId))
            {
                return RedirectToAction("ParkingInOutDashboard_Parking2");
            }
            else
            {
                return RedirectToAction("ParkingInOutDashboard_Parking3");
            }
        }

        public ActionResult GateOutFromParking_Old(Int64 PK_ParkingInOut, String StaffId, String Note = "")
        {
            var _parkingInOut = bll.db.ParkingInOuts.Where(m => m.PK_ParkingInOut == PK_ParkingInOut).FirstOrDefault();
            var _vehicle = _parkingInOut.Vehicle;
            var _vehicleInOutManual = _vehicle.VehicleInOutManual;

            GateInOutModel gateOutModel = new GateInOutModel();
            gateOutModel.FK_CreatedByUser = CurrentUser.PK_User.ToString();
            gateOutModel.RegistrationNumber = _vehicle.RegistrationNumber;
            gateOutModel.IsFormatedRegistrationNumber = "yes";

            //# SAME AS VehicleGateNewAPIController.GateOut(GateInOutModel gateOutModel);
            {
                var _PK_User = Guid.Parse(gateOutModel.FK_CreatedByUser);
                var gateUser = bll.db.AppUsers.Where(m => m.PK_User == _PK_User).FirstOrDefault();
                //gateUser.AppVersionCode = gateOutModel.AppVersionCode;

                bll.db.SaveChanges();
                var AppVersionCode_Current = bll.db.AppSettings.Where(m => m.Name == "AppVersionCode_Current" && m.IsActive == true).Select(m => m.Value).FirstOrDefault();
                var AppVersionCode_OldUsable = bll.db.AppSettings.Where(m => m.Name == "AppVersionCode_OldUsable" && m.IsActive == true).Select(m => m.Value).ToList();

                var flag = "n/a";
                var message = "";
                //var version_message = "";
                //if (!(gateOutModel.AppVersionCode == AppVersionCode_Current || AppVersionCode_OldUsable.Contains(gateOutModel.AppVersionCode)))
                //{
                //    flag = "appversion_not_usable";
                //    message = "বিফল হয়েছে। সফটয়ারটি আপডেট না করলে এন্ট্রী দেওয়া যাবে না। শীঘ্রই ট্রান্সপোর্ট ডিপার্ট্মেন্টকে জানান।";
                //    //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                //}
                try
                {
                    //gateOutModel.RegistrationNumber = gateOutModel.RegistrationNumber.ToUpper();

                    //# Conversion
                    //TemporaryVehicle temporaryVehicle = new TemporaryVehicle();
                    //temporaryVehicle.RegistrationNumber = _vehicle.RegistrationNumber;//gateOutModel.IsFormatedRegistrationNumber == "yes" ? gateOutModel.RegistrationNumber : FormatRegistrationNumber(gateOutModel.RegistrationNumber);
                    //temporaryVehicle.FK_VehicleInOutManualReason = _vehicleInOutManual.In_FK_VehicleInOutManualReason;//Convert.ToInt64(gateOutModel.FK_VehicleInOutManualReason);
                    //temporaryVehicle.LoadOrEmpty = _vehicleInOutManual.In_LoadOrEmpty;//gateOutModel.LoadOrEmpty.Split('/')[1];
                    //temporaryVehicle.IssueDateTime = DateTime.Now;
                    //temporaryVehicle.IsScannedEntry = false;//gateOutModel.IsFormatedRegistrationNumber == "yes" ? true : false;
                    //temporaryVehicle.FK_CreatedByLocationGate = Guid.Parse(gateOutModel.FK_CreatedByUser);
                    //temporaryVehicle.GPNumber = gateOutModel.GPNumber;

                    var RegistrationNumber = _vehicle.RegistrationNumber;
                    var vehicle = _vehicle;//bll.db.Vehicles.Where(m => m.RegistrationNumber == RegistrationNumber).FirstOrDefault();
                    //temporaryVehicle.FK_Locaiton = gateUser.FK_Location;

                    //if (vehicle == null)
                    //{
                    //    var temppraryVehicle = bll.db.TemporaryVehicles.Where(m => m.RegistrationNumber == temporaryVehicle.RegistrationNumber).FirstOrDefault();
                    //    if (temppraryVehicle != null)
                    //    {
                    //        flag = "vehicle_not_confirmed";
                    //        message = "বিফল হয়েছে। শীঘ্রই ট্রান্সপোর্ট ডিপার্ট্মেন্টকে গাড়ির তথ্য হালনাগাদ করতে বলুন।";
                    //        //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                    //    }
                    //    else
                    //    {
                    //        flag = "vehicle_not_found";
                    //        message = "বিফল হয়েছে। গাড়ি খুজে পাওয়া যায়নি।";
                    //        //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                    //    }
                    //}
                    if (vehicle.LocationInOrOut == false || vehicle.FK_LocationInOut != gateUser.FK_Location)
                    {
                        if (vehicle.LocationInOrOut == true && vehicle.FK_LocationInOut != null)
                        {
                            flag = "vehicle_is_in_another_dipo";
                            message = "বিফল হয়েছে। গাড়িটি " + bll.db.Locations.Where(m => m.PK_Location == vehicle.FK_LocationInOut).FirstOrDefault().Name + " লোকেশনের ভিতরে রয়েছে।";
                            //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            flag = "vehicle_not_in_this_dipo";
                            message = "বিফল হয়েছে। গাড়ি এই লোকেশনের বাহিরে।";
                            //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    ////#GPNumber checking#
                    //var tripWithGpNumber = bll.db.RequisitionTrips.Where(m => m.OracleDB_GPNumber == temporaryVehicle.GPNumber && m.IsGatePassUsed == true).FirstOrDefault();
                    //if (gateOutModel.GPNumber != "Not Given" && tripWithGpNumber != null)
                    //{
                    //    flag = "gp_alrady_used";
                    //    message = "বিফল হয়েছে। জিপি নাম্বার: " + temporaryVehicle.GPNumber + " ইতিমধ্যে ব্যাবহার হয়েছে।";
                    //}
                    ////#GPNumber Checking*
                    //if (gateOutModel.GPNumber != "Not Given")
                    //{
                    //    var usedGPVehicleInOutManual = bll.db.VehicleInOutManuals.Where(m => m.InOrOut == false && m.GPNumber == gateOutModel.GPNumber).FirstOrDefault();
                    //    if (usedGPVehicleInOutManual != null)
                    //    {
                    //        flag = "gp_number_already_used";
                    //        message = "বিফল হয়েছে। জিপি নাম্বারটি পূর্বে ব্যবহৃত।";
                    //        //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                    //    }
                    //    try
                    //    {
                    //        var res = "";
                    //        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://172.17.107.254:808/api/dgx/");
                    //        httpWebRequest.ContentType = "application/json";
                    //        httpWebRequest.Method = "POST";

                    //        httpWebRequest.Headers["ss"] = "DGTX";
                    //        httpWebRequest.Headers["yy"] = "HJDyh876Yhd765JHdgeoOUE765487sf543GDJksn";

                    //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    //        {
                    //            string json = new JavaScriptSerializer().Serialize(new
                    //            {
                    //                dgno = gateOutModel.GPNumber
                    //            });

                    //            streamWriter.Write(json);
                    //        }

                    //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    //        {
                    //            res = streamReader.ReadToEnd();
                    //        }
                    //        dynamic data_json = JObject.Parse(res);

                    //        if (data_json.Status == "N") // else data_json.Status = "Y"
                    //        {
                    //            flag = "gp_not_found";
                    //            message = "বিফল হয়েছে। জিপি নাম্বারটি সঠিক নয়।";
                    //            //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                    //        }
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        var error_message = e.Message;
                    //        while (e.InnerException != null) { e = e.InnerException; error_message = e.Message; }
                    //        bll.db.AppErrorLogs.Add(
                    //            new AppErrorLog()
                    //            {
                    //                ErrorMessage = error_message,
                    //                ErrorTime = DateTime.Now,
                    //                UserDefinedMessage = "VehicleGateNewAPI/GateIn FK_AppUser:" + gateOutModel.FK_CreatedByUser
                    //            }
                    //            );
                    //        bll.db.SaveChanges();

                    //        flag = "gp_checking_error";
                    //        message = "বিফল হয়েছে। MIS ডিপার্ট্মেন্টকে জানান(জিপি সার্ভার সমস্যা)।";
                    //        //return Json(new
                    //        {
                    //            flag,
                    //            message,
                    //            error_message
                    //        }, JsonRequestBehavior.AllowGet);
                    //    }

                    //}
                    //#GPNumber checking*

                    var vehicleInOutManual = _vehicleInOutManual;//bll.db.VehicleInOutManuals.Where(m => m.PK_VehicleInOutManual == vehicle.FK_VehicleInOutManual_Last).FirstOrDefault();
                    vehicleInOutManual.DevelopersNote = "ParkingInOutController/GateOutFromParking/VehicleInOutManual-Out";
                    vehicleInOutManual.Out_LoadOrEmpty = _vehicleInOutManual.In_LoadOrEmpty;
                    vehicleInOutManual.Out_FK_VehicleInOutManualReason = _vehicleInOutManual.In_FK_VehicleInOutManualReason;
                    vehicleInOutManual.Out_IssueDateTime = DateTime.Now;
                    vehicleInOutManual.Out_IsScannedEntry = false;
                    //vehicleInOutManual.GPNumber = temporaryVehicle.GPNumber;


                    vehicleInOutManual.InOrOut = false;

                    DateTimeOffset _In_IssueDateTime = vehicleInOutManual.In_IssueDateTime;
                    DateTimeOffset _Out_IssueDateTime = (DateTimeOffset)vehicleInOutManual.Out_IssueDateTime;
                    vehicleInOutManual.InStayTimeMinute = (long)_Out_IssueDateTime.Subtract(_In_IssueDateTime).TotalMinutes;

                    if (vehicle.DepoStayMaximumTimeMinute != null)
                    {
                        vehicleInOutManual.OverStayTimeMinute = vehicleInOutManual.InStayTimeMinute - vehicle.DepoStayMaximumTimeMinute;
                    }
                    vehicleInOutManual.Out_FK_CreatedByUser = gateUser.PK_User;
                    vehicleInOutManual.Out_DeviceId = "Web";
                    bll.db.SaveChanges();

                    vehicle.LocationInOutTime = vehicleInOutManual.Out_IssueDateTime;
                    vehicle.LocationInOrOut = false;
                    vehicle.LocationInOut_Load_Unload_Workshop = vehicleInOutManual.Out_LoadOrEmpty;
                    bll.db.SaveChanges();

                    if (gateUser.AppUserType == "Internal Gate Entry Device")
                    {
                        try
                        {
                            new RequisitionController().RequisitionTrip_StartMulti(vehicle.PK_Vehicle, gateUser.FK_Location ?? new Guid(), gateUser.PK_User);
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            if (vehicle.FK_ParkingInOut_Last != null)
                            {
                                //# when leaving through non-parking gate from parking
                                var parkingInOut = _parkingInOut;//bll.db.ParkingInOuts.Where(m => m.PK_ParkingInOut == vehicle.FK_ParkingInOut_Last).FirstOrDefault();
                                if (parkingInOut != null && parkingInOut.Out_IssueDateTime == null)
                                {
                                    parkingInOut.Out_IssueDateTime = vehicleInOutManual.Out_IssueDateTime;
                                    parkingInOut.Out_FK_CreatedByUser = gateUser.PK_User;
                                    bll.db.SaveChanges();
                                }
                                else if (parkingInOut != null && parkingInOut.FactoryOut_IssueDateTime == null)
                                {
                                    parkingInOut.FactoryOut_IssueDateTime = vehicleInOutManual.Out_IssueDateTime;
                                    parkingInOut.FactoryOut_FK_CreatedByUser = gateUser.PK_User;
                                    vehicle.FK_ParkingInOut_Last = null;
                                    bll.db.SaveChanges();
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else if (gateUser.AppUserType == "Internal Parking Entry Device")
                    {
                        try
                        {
                            if (vehicle.FK_ParkingInOut_Last != null)
                            {
                                var parkingInOut = _parkingInOut;//bll.db.ParkingInOuts.Where(m => m.PK_ParkingInOut == vehicle.FK_ParkingInOut_Last).FirstOrDefault();
                                if (parkingInOut != null && parkingInOut.Out_IssueDateTime == null)
                                {
                                    parkingInOut.Out_IssueDateTime = vehicleInOutManual.Out_IssueDateTime;
                                    parkingInOut.Out_FK_CreatedByUser = gateUser.PK_User;
                                    if (!string.IsNullOrEmpty(StaffId))
                                    {
                                        parkingInOut.Out_DriverStaffId = StaffId;
                                        if (!string.IsNullOrEmpty(Note))
                                        {
                                            parkingInOut.Out_Note = Note;
                                        }
                                    }
                                    bll.db.SaveChanges();
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    flag = "success";
                    message = "বাহির হওয়ার এন্ট্রি সফল হয়েছে।";
                    //return Json(new { flag, message }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    var error_message = e.Message;
                    while (e.InnerException != null) { e = e.InnerException; error_message = e.Message; }
                    bll.db.AppErrorLogs.Add(
                        new AppErrorLog()
                        {
                            ErrorMessage = error_message,
                            ErrorTime = DateTime.Now,
                            UserDefinedMessage = "VehicleGateNewAPI/GateIn FK_AppUser:" + gateOutModel.FK_CreatedByUser
                        }
                        );
                    bll.db.SaveChanges();

                    flag = "internal_error";
                    message = "বিফল হয়েছে। MIS ডিপার্ট্মেন্টকে জানান(থার্ড আই সার্ভার সমস্যা)।";
                    //return Json(new { flag, message, error_message }, JsonRequestBehavior.AllowGet);
                }
                if (flag == "success")
                {
                    CreateAlertMessage(AlertMessageType.Success, "Success", message);
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", message);
                }
            }
            if (string.IsNullOrEmpty(StaffId))
            {
                return RedirectToAction("ParkingInOutDashboard_Parking2");
            }
            else
            {
                return RedirectToAction("ParkingInOutDashboard_Parking3");
            }
        }

        public ActionResult TodayParkingVehicle()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var startingLimit = DateTime.Now.AddHours(-12);
            var list = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.FinalWantedAtDateTime > startingLimit && m.OWN_MHT_DHT == "DHT" && m.ManualParkingEntryTime == null && m.StatusText == "Assigned" && m.Requisition.FK_Location_From == CurrentUser.FK_Location).ToList();
            return View(list);
        }
        public ActionResult ConfirmManualParkingEntry(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == id).FirstOrDefault();
            if (model.ManualParkingEntryTime == null)
            {
                model.ManualParkingEntryTime = DateTime.Now;
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Successfully parking entry confirmed.");
                return RedirectToAction("TodayParkingVehicle");
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Parking entry confirmation is already done by other gate.");
                return RedirectToAction("TodayParkingVehicle");
            }
        }

        //# API
        public string SetCurrentFingerGivenDriver(String PK_User, String StaffID)
        {
            try
            {
                var Category = "Parking Out From web";
                var model = new MiscellaneousData();
                model.Category = Category;
                model.CreatedBy = Guid.Parse(PK_User);
                model.CreatedAt = DateTime.Now;
                model.Key_1 = PK_User;
                model.Data_1 = StaffID;
                bll.db.MiscellaneousDatas.Add(model);
                bll.db.SaveChanges();
                return "OK";
            }
            catch (Exception e)
            {

                var errorMessage = "";
                while (e != null)
                {
                    errorMessage = errorMessage + " | " + e.Message;
                    e = e.InnerException;
                }
                return "ERROR on ParkingInOut/SetCurrentFingerGivenDriver Error Message: " + errorMessage;
            }
        }
        public ActionResult GetCurrentFingerGivenDriver()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Json(new { status = "FAIL", message = "Please Logout and Login." }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var Category = "Parking Out From web";
                var PK_User = CurrentUser.PK_User.ToString();
                var list = bll.db.MiscellaneousDatas.Where(m => m.Category == Category && m.Key_1 == PK_User).ToList();
                var data = list.LastOrDefault();
                bll.db.MiscellaneousDatas.RemoveRange(list);
                bll.db.SaveChanges();
                if (data != null)
                {
                    try
                    {
                        var options = new RestClientOptions("http://hris.prangroup.com:8696")
                        {
                            MaxTimeout = -1,
                        };
                        var client = new RestClient(options);
                        var request = new RestRequest("/StaffInformation/getStaffImg", Method.Post);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", "Basic YXV0aDoxMlByYW5AMTIzNDU2JA==");
                        Dictionary<string, string> data_dict = new Dictionary<string, string>();
                        data_dict.Add("staffId", data.Data_1);
                        data_dict.Add("sK_KEY", "tU2QLneUPnHZH5jq");
                        request.AddBody(data_dict);
                        RestResponse response = client.Execute(request);
                        var res_stirng = response.Content.ToString();
                        res_stirng = res_stirng.Replace("\\", "");
                        res_stirng = res_stirng.Replace("\"[", "[").Replace("]\"", "]");
                        res_stirng = res_stirng.Substring(1, res_stirng.Length - 2);
                        var deserializedObject = JsonConvert.DeserializeObject<VM_HRISImageAPIData>(res_stirng);
                        if (deserializedObject.staff_personal_details.Any())
                        {
                            return Json(new { status = "SUCCESS", staff_Id = data.Data_1, employee_name = deserializedObject.staff_personal_details.FirstOrDefault().employee_name, image_url = deserializedObject.staff_personal_details.FirstOrDefault().imageurl }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { status = "SUCCESS", staff_Id = data.Data_1 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception e)
                    {
                        return Json(new { status = "SERVER ERROR", message = "Can not connect HRIS for staff image. Please contact MIS." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = "FAIL", message = "No driver found from finger punch. Give driver's finget punch again." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { status = "SERVER ERROR", message = "Server error. Please contact MIS." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}