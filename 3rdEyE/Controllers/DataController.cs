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
using _3rdEyE.ManagingTools;
using OfficeOpenXml;
using System.Data.SqlClient;

namespace _3rdEyE.Controllers
{
    public class DataController : BaseController
    {

        //# Top 10 Hired Vehicle-Start
        public ActionResult RequisitionTrip_TopHired_Index(string PRG_Type, string FK_Location, string StartingDate, string EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<VM_RequisitionTrip_TopHired>();
            var query1 = bll.db.RequisitionTrip_Finished.Where(m =>
            m.IsDeleted != true
            && (m.IsParent == true || m.FK_RequisitionTrip_Finished_Parent == null)
            && m.FK_Vehicle != null // Assigned
            && m.OWN_MHT_DHT == "DHT"
            );

            //PRG_Type
            if (!string.IsNullOrEmpty(PRG_Type))
            {
                query1 = query1.Where(m => m.Requisition.PRG_Type == PRG_Type);
            }
            Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" } };
            ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", PRG_Type);

            //FK_Location
            if (!string.IsNullOrEmpty(FK_Location))
            {
                var _FK_Location = string.IsNullOrEmpty(FK_Location) ? new Guid() : Guid.Parse(FK_Location);
                query1 = query1.Where(m => m.Requisition.FK_Location_From == _FK_Location);
            }
            ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false && (m.LocationType == "Factory" || m.LocationType == "Depo")).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location);


            //StartingDate
            if (!string.IsNullOrEmpty(StartingDate))
            {
                var _StartingDate = Convert.ToDateTime(StartingDate);
                query1 = query1.Where(m => _StartingDate < m.FinalWantedAtDateTime);
            }
            ViewBag.StartingDate = StartingDate;

            //EndingDate
            if (!string.IsNullOrEmpty(EndingDate))
            {
                var _EndingDate = Convert.ToDateTime(EndingDate).AddDays(1);
                query1 = query1.Where(m => m.FinalWantedAtDateTime < _EndingDate);
            }
            ViewBag.EndingDate = EndingDate;

            if (!string.IsNullOrEmpty(StartingDate) && !string.IsNullOrEmpty(EndingDate))
            {
                list = query1.GroupBy(m => m.Vehicle.RegistrationNumber).Select(m => new { m.Key, TotalTrip = m.Count(), trips = m.Select(n => new { n.TrackingID, n.FinalWantedAtDateTime }) }).OrderByDescending(m => m.TotalTrip).Take(10).AsQueryable().Select(m => new VM_RequisitionTrip_TopHired()
                {
                    RegistrationNumber = m.Key,
                    TotalTrip = m.TotalTrip
                }).ToList();

            }

            return View(list);
        }
        public class VM_RequisitionTrip_TopHired
        {
            public string RegistrationNumber { get; set; }
            public int TotalTrip { get; set; }
        }
        //# Top 10 Hired Vehicle-End



        //# Vehicle Staying Out of Locaitons-Start
        public ActionResult TrackingOwnVehicleWaitingOutSide(string PRG_Type, string ReportDate, string FK_Depo, string RegistrationNumber, int MinimumStayMinute = 10)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var finalOutStayLocationList = new List<Report_VehicleOutOverStay>();

            var vehicles_query = bll.db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "OWN" && m.Depo.IsDeleted == false).AsQueryable();
            var vehicles = new List<Vehicle>();

            //# PRG_Type
            Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" }, { "CS", "CS" } };
            if (!string.IsNullOrEmpty(PRG_Type))
            {
                vehicles_query = vehicles_query.Where(m => m.Depo.PRG_Type == PRG_Type);
            }
            ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", PRG_Type);

            //# ReportDate
            var StartingDate = new DateTime();
            var EndingDate = new DateTime();

            if (!string.IsNullOrEmpty(ReportDate))
            {
                StartingDate = Convert.ToDateTime(ReportDate);
                ViewBag.ReportDate = ReportDate;
                EndingDate = StartingDate.AddDays(1);
            }

            //# FK_Depo
            if (!string.IsNullOrEmpty(FK_Depo))
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                vehicles_query = vehicles_query.Where(m => m.FK_Depo == _FK_Depo);
            }
            ViewBag.Depoes = new SelectList(bll.db.Depoes.AsEnumerable().Where(c => !c.Category.Contains("Physical") && c.IsDeleted != true).OrderBy(m => m.Name).Select(m => new { m.PK_Depo, Name = m.Name + " " + m.Vehicles.Count }), "PK_Depo", "Name", FK_Depo);

            //# RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                vehicles_query = vehicles_query.Where(m => m.VehicleTrackingInformation.Vehicle.RegistrationNumber == RegistrationNumber);
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //# MinimumStayMinute
            ViewBag.MinimumStayMinute = MinimumStayMinute;

            if (!string.IsNullOrEmpty(PRG_Type) && !string.IsNullOrEmpty(ReportDate))
            {
                vehicles_query = vehicles_query.Where(m => m.VehicleTrackingInformation.VehicleTracking.UpdateTime > EndingDate).AsQueryable();
                vehicles = vehicles_query.ToList();
            }
            else
            {
                //# Empty View
                return View(finalOutStayLocationList);
            }


            foreach (var vehicle in vehicles)
            {
                //System.Diagnostics.Debug.WriteLine("#Vehicle Current " + (_count++) + " " + DateTime.Now.ToString("HH:mm:ss"));
                //var vehicle = bll.db.Vehicles.Where(m =>
                //m.RegistrationNumber == "DHAKA METRO-SHA-13-0181"
                //).FirstOrDefault();


                //# Get Out Stay From Tracking
                var timeLapMinute = 5;
                double maximumDistanceVeriaton = 0.1;
                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@USER_KEY", CurrentUser.PK_User),
                new SqlParameter("@FK_Vehicle", vehicle.PK_Vehicle),
                new SqlParameter("@StartingDate", StartingDate),
                new SqlParameter("@EndingDate", EndingDate),
                new SqlParameter("@TimeLapMinute", timeLapMinute)
                };
                var vehicleHistory_Result = bll.db.Database.SqlQuery<Report_GetVehicleHistory_Result>("Report_GetVehicleHistory @USER_KEY, @FK_Vehicle, @StartingDate, @EndingDate, @TimeLapMinute", parameters).ToList();

                var outStayLocationList = new List<Report_VehicleOutOverStay>();
                Report_VehicleOutOverStay currentOutStayLocation = new Report_VehicleOutOverStay();
                bool isNewOutStayLocation = false;
                foreach (var item in vehicleHistory_Result)
                {
                    if (item.EngineStatus != "1")
                    {
                        if (isNewOutStayLocation == false)
                        {
                            isNewOutStayLocation = true;
                            currentOutStayLocation = new Report_VehicleOutOverStay();
                            currentOutStayLocation.Vehicle = vehicle;
                            currentOutStayLocation.Start_UpdateTime = item.UpdateTime;
                            currentOutStayLocation.Start_Latitude = item.Latitude;
                            currentOutStayLocation.Start_Longitude = item.Longitude;
                            currentOutStayLocation.Start_EngineStatus = item.EngineStatus;
                            currentOutStayLocation.Start_Speed = item.Speed;
                            currentOutStayLocation.Start_NearestMapLocation = item.NearestMapLocation;
                            currentOutStayLocation.Start_NearestMapLocationDistance = item.NearestMapLocationDistance;
                        }
                    }
                    else
                    {
                        if (isNewOutStayLocation == true)
                        {
                            isNewOutStayLocation = false;
                            currentOutStayLocation.Finish_UpdateTime = item.UpdateTime;
                            currentOutStayLocation.Finish_Latitude = item.Latitude;
                            currentOutStayLocation.Finish_Longitude = item.Longitude;
                            //currentOutStayLocation.Finish_EngineStatus = item.EngineStatus;
                            currentOutStayLocation.Finish_Speed = item.Speed;
                            currentOutStayLocation.Finish_NearestMapLocation = item.NearestMapLocation;
                            currentOutStayLocation.Finish_NearestMapLocationDistance = item.NearestMapLocationDistance;

                            currentOutStayLocation.StayTimeMinute = (int)(currentOutStayLocation.Finish_UpdateTime - currentOutStayLocation.Start_UpdateTime).TotalMinutes;
                            outStayLocationList.Add(currentOutStayLocation);
                        }
                    }
                }
                outStayLocationList = outStayLocationList.Where(m => m.StayTimeMinute >= MinimumStayMinute).ToList();
                outStayLocationList = outStayLocationList.Where(m => distanceInKmBetweenEarthCoordinates(m.Start_Latitude, m.Start_Longitude, m.Finish_Latitude, m.Finish_Longitude) < maximumDistanceVeriaton).ToList();



                //# Get Out Time From Gate In Out
                var vehicleInOutMenualsList = bll.db.VehicleInOutManuals.Where(m => m.FK_Vehicle == vehicle.PK_Vehicle && ((StartingDate < m.In_IssueDateTime && m.In_IssueDateTime < EndingDate) || (StartingDate < m.Out_IssueDateTime && m.Out_IssueDateTime < EndingDate))).OrderBy(m => m.In_IssueDateTime).ToList();
                var outStayTimeList = new List<DM_OutStayTime>();
                DM_OutStayTime currentOutStayTime = new DM_OutStayTime();
                bool isNewOutStayTime = false;

                var inOutEventList_Final = new List<DM_InOutEvent>();
                var inOutEvenList_In = vehicleInOutMenualsList.Select(m => new DM_InOutEvent() { InOrOut = true, Issue_DateTime = m.In_IssueDateTime }).ToList();
                var inOutEvenList_Out = vehicleInOutMenualsList.Where(m => m.Out_IssueDateTime != null).Select(m => new DM_InOutEvent() { InOrOut = false, Issue_DateTime = m.Out_IssueDateTime ?? new DateTime() }).ToList();
                inOutEventList_Final.AddRange(inOutEvenList_In);
                inOutEventList_Final.AddRange(inOutEvenList_Out);
                inOutEventList_Final = inOutEventList_Final.Where(m => m.Issue_DateTime > StartingDate && m.Issue_DateTime < EndingDate).OrderBy(m => m.Issue_DateTime).ToList();

                if (inOutEventList_Final.Any())
                {
                    for (int i = 0; i < inOutEventList_Final.Count; i++)
                    {
                        var item = inOutEventList_Final[i];
                        if (i == 0)
                        {
                            if (item.InOrOut == true)
                            {
                                currentOutStayTime = new DM_OutStayTime();
                                currentOutStayTime.Start_DateTime = StartingDate;
                                currentOutStayTime.End_DateTime = item.Issue_DateTime;
                                outStayTimeList.Add(currentOutStayTime);

                                isNewOutStayTime = false;
                            }
                            else
                            {
                                currentOutStayTime = new DM_OutStayTime();
                                currentOutStayTime.Start_DateTime = item.Issue_DateTime;

                                isNewOutStayTime = true;
                            }
                        }
                        else
                        {
                            if (item.InOrOut == true)
                            {
                                if (isNewOutStayTime == true)
                                {
                                    currentOutStayTime.End_DateTime = item.Issue_DateTime;
                                    outStayTimeList.Add(currentOutStayTime);

                                    isNewOutStayTime = false;
                                }
                            }
                            else
                            {
                                if (isNewOutStayTime == false)
                                {
                                    currentOutStayTime = new DM_OutStayTime();
                                    currentOutStayTime.Start_DateTime = item.Issue_DateTime;

                                    isNewOutStayTime = true;
                                }
                            }
                        }
                    }
                }


                // Preparing Result 
                foreach (var outStayTime in outStayTimeList)
                {
                    var res = outStayLocationList.Where(m => outStayTime.Start_DateTime < m.Start_UpdateTime && m.Finish_UpdateTime < outStayTime.End_DateTime);
                    if (res.Any())
                    {
                        finalOutStayLocationList.AddRange(res);
                    }
                }

            }
            return View(finalOutStayLocationList);
        }
        public ActionResult TrackingOwnVehicleWaitingOutSide_AutoGenerated(string PRG_Type, string ReportDate, string FK_Depo, string RegistrationNumber, int MinimumStayMinute = 10)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var finalOutStayLocationList = new List<Report_VehicleOutOverStay>();

            var _query = bll.db.Report_VehicleOutOverStay.Where(m => m.Vehicle.IsDeleted == false && m.Vehicle.OWN_MHT_DHT == "OWN" && m.Vehicle.Depo.IsDeleted == false && m.StayTimeMinute >= MinimumStayMinute).AsQueryable();
            var vehicles = new List<Vehicle>();

            //# PRG_Type
            Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" }, { "CS", "CS" } };
            if (!string.IsNullOrEmpty(PRG_Type))
            {
                _query = _query.Where(m => m.Vehicle.Depo.PRG_Type == PRG_Type);
            }
            ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", PRG_Type);

            //# ReportDate
            var StartingDate = new DateTime();
            var EndingDate = new DateTime();

            if (!string.IsNullOrEmpty(ReportDate))
            {
                StartingDate = Convert.ToDateTime(ReportDate);
                ViewBag.ReportDate = ReportDate;
                EndingDate = StartingDate.AddDays(1);
                _query = _query.Where(m => m.Start_UpdateTime >= StartingDate && m.Finish_UpdateTime <= EndingDate);
            }

            //# FK_Depo
            if (!string.IsNullOrEmpty(FK_Depo))
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                _query = _query.Where(m => m.Vehicle.FK_Depo == _FK_Depo);
            }
            ViewBag.Depoes = new SelectList(bll.db.Depoes.AsEnumerable().Where(c => !c.Category.Contains("Physical") && c.IsDeleted != true).OrderBy(m => m.Name), "PK_Depo", "Name", FK_Depo);

            //# RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                _query = _query.Where(m => m.Vehicle.RegistrationNumber == RegistrationNumber);
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //# MinimumStayMinute
            ViewBag.MinimumStayMinute = MinimumStayMinute;

            if (!string.IsNullOrEmpty(PRG_Type) && !string.IsNullOrEmpty(ReportDate))
            {
                _query = _query.Where(m => m.Vehicle.VehicleTrackingInformation.VehicleTracking.UpdateTime > EndingDate).AsQueryable();
                finalOutStayLocationList = _query.OrderBy(m => m.Vehicle.RegistrationNumber).ThenBy(m => m.Start_UpdateTime).ToList();
            }
            //# Empty View
            return View(finalOutStayLocationList);
        }

        public string TryGenerate_Report_VehicleOutOverStay()
        {
            var guid = Guid.NewGuid();
            var now = DateTime.Now;
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TryGenerate_Report_VehicleOutOverStay-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            try
            {
                //# delete report date existing data
                var min_range = DateTime.Now.AddDays(-31).Date;
                var max_range = DateTime.Now.AddDays(-1).Date;
                var existingData = bll.db.Report_VehicleOutOverStay.Where(m => m.Finish_UpdateTime < min_range || m.Start_UpdateTime > max_range).ToList();
                bll.db.Report_VehicleOutOverStay.RemoveRange(existingData);
                bll.db.SaveChanges();

                var finalOutStayLocationList = new List<Report_VehicleOutOverStay>();

                var vehicles_query = bll.db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "OWN" && m.Depo.IsDeleted == false).AsQueryable();
                var vehicles = new List<Vehicle>();

                //# PRG_Type
                //Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" }, { "CS", "CS" } };
                //if (!string.IsNullOrEmpty(PRG_Type))
                //{
                //    vehicles_query = vehicles_query.Where(m => m.Depo.PRG_Type == PRG_Type);
                //}
                //ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", PRG_Type);

                //# ReportDate
                var StartingDate = DateTime.Now.AddDays(-1).Date;
                var EndingDate = DateTime.Now.Date;

                //if (!string.IsNullOrEmpty(ReportDate))
                //{
                //    StartingDate = Convert.ToDateTime(ReportDate);
                //    ViewBag.ReportDate = ReportDate;
                //    EndingDate = StartingDate.AddDays(1);
                //}

                ////# FK_Depo
                //if (!string.IsNullOrEmpty(FK_Depo))
                //{
                //    var _FK_Depo = Guid.Parse(FK_Depo);
                //    vehicles_query = vehicles_query.Where(m => m.FK_Depo == _FK_Depo);
                //}
                //ViewBag.Depoes = new SelectList(bll.db.Depoes.AsEnumerable().Where(c => !c.Category.Contains("Physical") && c.IsDeleted != true).OrderBy(m => m.Name).Select(m => new { m.PK_Depo, Name = m.Name + " " + m.Vehicles.Count }), "PK_Depo", "Name", FK_Depo);

                ////# RegistrationNumber
                //if (!string.IsNullOrEmpty(RegistrationNumber))
                //{
                //    vehicles_query = vehicles_query.Where(m => m.VehicleTrackingInformation.Vehicle.RegistrationNumber == RegistrationNumber);
                //}
                //ViewBag.RegistrationNumber = RegistrationNumber;

                //if (!string.IsNullOrEmpty(PRG_Type) && !string.IsNullOrEmpty(ReportDate))
                //{
                //    vehicles_query = vehicles_query.Where(m => m.VehicleTrackingInformation.VehicleTracking.UpdateTime > EndingDate).AsQueryable();
                //    vehicles = vehicles_query.ToList();
                //}
                //else
                //{
                //    //# Empty View
                //    return View(finalOutStayLocationList);
                //}

                vehicles_query = vehicles_query.Where(m => m.VehicleTrackingInformation.VehicleTracking.UpdateTime > EndingDate).AsQueryable();
                vehicles = vehicles_query.ToList();

                List<Report_VehicleOutOverStay> outStayLocationList;
                foreach (var vehicle in vehicles)
                {
                    outStayLocationList = new List<Report_VehicleOutOverStay>();
                    //System.Diagnostics.Debug.WriteLine("#Vehicle Current " + (_count++) + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //var vehicle = bll.db.Vehicles.Where(m =>
                    //m.RegistrationNumber == "DHAKA METRO-SHA-13-0181"
                    //).FirstOrDefault();


                    //# Get Out Stay From Tracking
                    var timeLapMinute = 5;
                    var minimumStayMinute = 10;
                    double maximumDistanceVeriaton = 0.1;
                    SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@USER_KEY", "00000000-0000-0000-0000-000000000000"),
                new SqlParameter("@FK_Vehicle", vehicle.PK_Vehicle),
                new SqlParameter("@StartingDate", StartingDate),
                new SqlParameter("@EndingDate", EndingDate),
                new SqlParameter("@TimeLapMinute", timeLapMinute)
                };
                    var vehicleHistory_Result = bll.db.Database.SqlQuery<Report_GetVehicleHistory_Result>("Report_GetVehicleHistory @USER_KEY, @FK_Vehicle, @StartingDate, @EndingDate, @TimeLapMinute", parameters).ToList();

                    Report_VehicleOutOverStay currentOutStayLocation = new Report_VehicleOutOverStay();
                    bool isNewOutStayLocation = false;
                    foreach (var item in vehicleHistory_Result)
                    {
                        if (item.EngineStatus != "1")
                        {
                            if (isNewOutStayLocation == false)
                            {
                                isNewOutStayLocation = true;
                                currentOutStayLocation = new Report_VehicleOutOverStay();
                                currentOutStayLocation.Vehicle = vehicle;
                                currentOutStayLocation.Start_UpdateTime = item.UpdateTime;
                                currentOutStayLocation.Start_Latitude = item.Latitude;
                                currentOutStayLocation.Start_Longitude = item.Longitude;
                                currentOutStayLocation.Start_EngineStatus = item.EngineStatus;
                                currentOutStayLocation.Start_Speed = item.Speed;
                                currentOutStayLocation.Start_NearestMapLocation = item.NearestMapLocation;
                                currentOutStayLocation.Start_NearestMapLocationDistance = item.NearestMapLocationDistance;
                            }
                        }
                        else
                        {
                            if (isNewOutStayLocation == true)
                            {
                                isNewOutStayLocation = false;
                                currentOutStayLocation.Finish_UpdateTime = item.UpdateTime;
                                currentOutStayLocation.Finish_Latitude = item.Latitude;
                                currentOutStayLocation.Finish_Longitude = item.Longitude;
                                //currentOutStayLocation.Finish_EngineStatus = item.EngineStatus;
                                currentOutStayLocation.Finish_Speed = item.Speed;
                                currentOutStayLocation.Finish_NearestMapLocation = item.NearestMapLocation;
                                currentOutStayLocation.Finish_NearestMapLocationDistance = item.NearestMapLocationDistance;

                                currentOutStayLocation.StayTimeMinute = (int)(currentOutStayLocation.Finish_UpdateTime - currentOutStayLocation.Start_UpdateTime).TotalMinutes;
                                outStayLocationList.Add(currentOutStayLocation);
                            }
                        }
                    }
                    outStayLocationList = outStayLocationList.Where(m => m.StayTimeMinute >= minimumStayMinute).ToList();
                    outStayLocationList = outStayLocationList.Where(m => distanceInKmBetweenEarthCoordinates(m.Start_Latitude, m.Start_Longitude, m.Finish_Latitude, m.Finish_Longitude) < maximumDistanceVeriaton).ToList();



                    //# Get Out Time From Gate In Out
                    var vehicleInOutMenualsList = bll.db.VehicleInOutManuals.Where(m => m.FK_Vehicle == vehicle.PK_Vehicle && ((StartingDate < m.In_IssueDateTime && m.In_IssueDateTime < EndingDate) || (StartingDate < m.Out_IssueDateTime && m.Out_IssueDateTime < EndingDate))).OrderBy(m => m.In_IssueDateTime).ToList();
                    var outStayTimeList = new List<DM_OutStayTime>();
                    DM_OutStayTime currentOutStayTime = new DM_OutStayTime();
                    bool isNewOutStayTime = false;

                    var inOutEventList_Final = new List<DM_InOutEvent>();
                    var inOutEvenList_In = vehicleInOutMenualsList.Select(m => new DM_InOutEvent() { InOrOut = true, Issue_DateTime = m.In_IssueDateTime }).ToList();
                    var inOutEvenList_Out = vehicleInOutMenualsList.Where(m => m.Out_IssueDateTime != null).Select(m => new DM_InOutEvent() { InOrOut = false, Issue_DateTime = m.Out_IssueDateTime ?? new DateTime() }).ToList();
                    inOutEventList_Final.AddRange(inOutEvenList_In);
                    inOutEventList_Final.AddRange(inOutEvenList_Out);
                    inOutEventList_Final = inOutEventList_Final.Where(m => m.Issue_DateTime > StartingDate && m.Issue_DateTime < EndingDate).OrderBy(m => m.Issue_DateTime).ToList();

                    if (inOutEventList_Final.Any())
                    {
                        for (int i = 0; i < inOutEventList_Final.Count; i++)
                        {
                            var item = inOutEventList_Final[i];
                            if (i == 0)
                            {
                                if (item.InOrOut == true)
                                {
                                    currentOutStayTime = new DM_OutStayTime();
                                    currentOutStayTime.Start_DateTime = StartingDate;
                                    currentOutStayTime.End_DateTime = item.Issue_DateTime;
                                    outStayTimeList.Add(currentOutStayTime);

                                    isNewOutStayTime = false;
                                }
                                else
                                {
                                    currentOutStayTime = new DM_OutStayTime();
                                    currentOutStayTime.Start_DateTime = item.Issue_DateTime;

                                    isNewOutStayTime = true;
                                }
                            }
                            else
                            {
                                if (item.InOrOut == true)
                                {
                                    if (isNewOutStayTime == true)
                                    {
                                        currentOutStayTime.End_DateTime = item.Issue_DateTime;
                                        outStayTimeList.Add(currentOutStayTime);

                                        isNewOutStayTime = false;
                                    }
                                }
                                else
                                {
                                    if (isNewOutStayTime == false)
                                    {
                                        currentOutStayTime = new DM_OutStayTime();
                                        currentOutStayTime.Start_DateTime = item.Issue_DateTime;

                                        isNewOutStayTime = true;
                                    }
                                }
                            }
                        }
                    }


                    // Preparing Result 
                    foreach (var outStayTime in outStayTimeList)
                    {
                        var res = outStayLocationList.Where(m => outStayTime.Start_DateTime < m.Start_UpdateTime && m.Finish_UpdateTime < outStayTime.End_DateTime);
                        if (res.Any())
                        {
                            finalOutStayLocationList.AddRange(res);
                            bll.db.Report_VehicleOutOverStay.AddRange(res);
                            bll.db.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TryGenerate_Report_VehicleOutOverStay-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            return "Done";
        }

        public class DM_InOutEvent
        {
            public System.DateTime Issue_DateTime { get; set; }
            public bool InOrOut { get; set; }
        }
        public class DM_OutStayTime
        {
            public System.DateTime Start_DateTime { get; set; }
            public System.DateTime End_DateTime { get; set; }
        }
        public class VM_OutStayLocation
        {
            public Vehicle Vehicle { get; set; }
            public System.DateTime Start_UpdateTime { get; set; }
            public double Start_Latitude { get; set; }
            public double Start_Longitude { get; set; }
            public string Start_EngineStatus { get; set; }
            public double Start_Speed { get; set; }
            public string Start_NearestMapLocation { get; set; }
            public string Start_NearestMapLocationDistance { get; set; }

            public System.DateTime Finish_UpdateTime { get; set; }
            public double Finish_Latitude { get; set; }
            public double Finish_Longitude { get; set; }
            public string Finish_EngineStatus { get; set; }
            public double Finish_Speed { get; set; }
            public string Finish_NearestMapLocation { get; set; }
            public string Finish_NearestMapLocationDistance { get; set; }

            public int StayTimeMinute { get; set; }
        }
        public double distanceInKmBetweenEarthCoordinates(double lat1, double lon1, double lat2, double lon2)
        {
            var earthRadiusKm = 6371;

            var dLat = degreesToRadians(lat2 - lat1);
            var dLon = degreesToRadians(lon2 - lon1);

            lat1 = degreesToRadians(lat1);
            lat2 = degreesToRadians(lat2);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return earthRadiusKm * c;
        }
        public double degreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
        //# Vehicle Staying Out of Locaitons-End
    }
}