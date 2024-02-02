using _3rdEyE.BLL;
using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class ManagementDashBoardController : Controller
    {
        public DBEnityModelContainer db = new DBEnityModelContainer();
        public ActionResult CEO_Dashbord()
        {
            //Workshop Vehicle
            //var workshopInsideRawData = (from vehicle in db.Vehicles.Where(m => m.IsDeleted == false && m.LocationInOrOut == true && m.FK_VehicleInOutManual_Last != null)
            //                             join vehicleInOutManual in db.VehicleInOutManuals.Where(m => m.FK_PRG_Type != 3 && m.Out_IssueDateTime == null) on vehicle.FK_VehicleInOutManual_Last equals vehicleInOutManual.PK_VehicleInOutManual
            //                             select new
            //                             {
            //                                 vehicleInOutManual.In_IssueDateTime
            //                             }
            //                             ).ToList();
            //var workshopInsideSummaryData = new List<int> {
            //workshopInsideRawData.Where(m=>m.In_IssueDateTime > DateTime.Now.AddDays(-1)).Count(),//-1D
            //workshopInsideRawData.Where(m=>m.In_IssueDateTime < DateTime.Now.AddDays(-1) && m.In_IssueDateTime > DateTime.Now.AddDays(-3)).Count(),//1D-3D
            //workshopInsideRawData.Where(m=>m.In_IssueDateTime < DateTime.Now.AddDays(-3) && m.In_IssueDateTime > DateTime.Now.AddDays(-7)).Count(),//3D-7D
            //workshopInsideRawData.Where(m=>m.In_IssueDateTime < DateTime.Now.AddDays(-7) && m.In_IssueDateTime > DateTime.Now.AddDays(-30)).Count(),//7D-30D
            //workshopInsideRawData.Where(m=>m.In_IssueDateTime < DateTime.Now.AddDays(-30)).Count(),//30D+
            //};
            //ViewBag.WorkshopInsideSummaryData = string.Join(",", workshopInsideSummaryData);


            //Over Speed
            var OverSpeedSummaryData = new List<Dictionary<string, string>>();
            //var _OverSpeedSummaryData = (from vehicleTracking in db.VehicleTrackings.Where(m => m.Speed >= 60)
            //                             join vehicle in db.Vehicles.Where(m => m.IsDeleted == false) on vehicleTracking.PK_Vehicle equals vehicle.PK_Vehicle
            //                             join depo in db.Depoes.Where(m => m.IsDeleted == false) on vehicle.FK_Depo equals depo.PK_Depo
            //                             group new { depo, vehicle, vehicleTracking } by new { depo } into _group
            //                             select new
            //                             {
            //                                 DepoName = _group.Key.depo.Name,
            //                                 Count_60_to_70 = _group.Where(m => m.vehicleTracking.Speed >= 60 && m.vehicleTracking.Speed < 70).Count(),
            //                                 Count_70_to_80 = _group.Where(m => m.vehicleTracking.Speed >= 70 && m.vehicleTracking.Speed < 80).Count(),
            //                                 Count_80_to_90 = _group.Where(m => m.vehicleTracking.Speed >= 80 && m.vehicleTracking.Speed < 90).Count(),
            //                                 Count_90_to_up = _group.Where(m => m.vehicleTracking.Speed >= 90).Count()
            //                             }
            //            ).ToList();
            //foreach (var item in _OverSpeedSummaryData)
            //{
            //    OverSpeedSummaryData.Add(new Dictionary<string, string>() {
            //        {"DepoName", item.DepoName },
            //        {"Count_60_to_70", item.Count_60_to_70.ToString() },
            //        {"Count_70_to_80", item.Count_70_to_80.ToString() },
            //        {"Count_80_to_90", item.Count_80_to_90.ToString() },
            //        {"Count_90_to_up", item.Count_90_to_up.ToString() },
            //    });
            //}
            ViewBag.OverSpeedSummaryData = OverSpeedSummaryData;


            //Inside Stay Vehicle Summary
            var VehicleInsideSummaryData = new List<Dictionary<string, string>>();
            //var now = DateTime.Now;
            //var _1hourBefore = now.AddDays(-1);
            //var _3hourBefore = now.AddDays(-3);
            //var _6hourBefore = now.AddDays(-6);
            //var _10hourBefore = now.AddDays(-10);

            //var _VehicleInsideSummaryData = (from vehicle in db.Vehicles.Where(m => m.IsDeleted == false && m.LocationInOrOut == true && m.FK_VehicleInOutManual_Last != null)
            //                                 join depo in db.Depoes.Where(m => m.IsDeleted == false) on vehicle.FK_Depo equals depo.PK_Depo
            //                                 join vehicleInOutManual in db.VehicleInOutManuals.Where(m => m.FK_PRG_Type != 3 && m.Out_IssueDateTime == null) on vehicle.FK_VehicleInOutManual_Last equals vehicleInOutManual.PK_VehicleInOutManual
            //                                 group new { vehicle, depo, vehicleInOutManual } by new { depo } into _group
            //                                 select new
            //                                 {
            //                                     DepoName = _group.Key.depo.Name,
            //                                     Count_1_to_3_hours = _group.Where(m => m.vehicleInOutManual.In_IssueDateTime < _1hourBefore && m.vehicleInOutManual.In_IssueDateTime > _3hourBefore).Count(),
            //                                     Count_3_to_6_hours = _group.Where(m => m.vehicleInOutManual.In_IssueDateTime < _3hourBefore && m.vehicleInOutManual.In_IssueDateTime > _6hourBefore).Count(),
            //                                     Count_6_to_10_hours = _group.Where(m => m.vehicleInOutManual.In_IssueDateTime < _6hourBefore && m.vehicleInOutManual.In_IssueDateTime > _10hourBefore).Count(),
            //                                     Count_10_to_up_hours = _group.Where(m => m.vehicleInOutManual.In_IssueDateTime < _10hourBefore).Count()
            //                                 }
            //                          ).ToList();
            //foreach (var item in _VehicleInsideSummaryData.Where(m => m.Count_1_to_3_hours != 0 || m.Count_3_to_6_hours != 0 || m.Count_6_to_10_hours != 0 || m.Count_10_to_up_hours != 0))
            //{
            //    VehicleInsideSummaryData.Add(new Dictionary<string, string>() {
            //        {"DepoName", item.DepoName },
            //        {"Count_1_to_3_hours", item.Count_1_to_3_hours.ToString() },
            //        {"Count_3_to_6_hours", item.Count_3_to_6_hours.ToString() },
            //        {"Count_6_to_10_hours", item.Count_6_to_10_hours.ToString() },
            //        {"Count_10_to_up_hours", item.Count_10_to_up_hours.ToString() },
            //    });
            //}
            ViewBag.VehicleInsideSummaryData = VehicleInsideSummaryData;

            //Depo Wise Vehicle Info
            var OwnVehicleSummaryData = new List<Dictionary<string, string>>();
            //var _OwnVehicleSummaryData = (from depo in db.Depoes.Where(m => m.IsDeleted == false)
            //                              join vehicle in db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "OWN") on depo.PK_Depo equals vehicle.FK_Depo
            //                              group new { depo, vehicle } by new { depo } into _group
            //                              select new
            //                              {
            //                                  DepoName = _group.Key.depo.Name,
            //                                  All_Vehicle_Count = _group.Count(),
            //                                  Cargo_Truck_Count = _group.Where(m => m.vehicle.VehicleType == "CargoTruck").Count(),
            //                                  Cargo_Truck_Open_Count = _group.Where(m => m.vehicle.VehicleType == "CargoTruck-Open").Count(),
            //                                  Cargo_VAN_Count = _group.Where(m => m.vehicle.VehicleType == "CargoVAN").Count(),
            //                                  Covered_Van_Count = _group.Where(m => m.vehicle.VehicleType == "CoveredVan").Count(),
            //                                  Delivery_Van_Count = _group.Where(m => m.vehicle.VehicleType == "DeliveryVan").Count(),
            //                                  Mini_Truck_Count = _group.Where(m => m.vehicle.VehicleType == "MiniTruck").Count(),
            //                                  Open_VAN_Count = _group.Where(m => m.vehicle.VehicleType == "OpenVAN").Count(),
            //                                  Pickup_Count = _group.Where(m => m.vehicle.VehicleType == "Pickup").Count(),
            //                                  Tank_Lorry_Count = _group.Where(m => m.vehicle.VehicleType == "TankLorry").Count(),
            //                                  Tipper_Count = _group.Where(m => m.vehicle.VehicleType == "Tipper").Count(),
            //                                  Trailers_Count = _group.Where(m => m.vehicle.VehicleType == "Trailers").Count(),
            //                                  Van_Count = _group.Where(m => m.vehicle.VehicleType == "Van").Count(),
            //                              }
            //            ).OrderBy(m => m.DepoName).ToList();
            //foreach (var item in _OwnVehicleSummaryData
            //.Where(m => m.Cargo_Truck_Count != 0 || m.Cargo_Truck_Open_Count != 0 || m.Cargo_VAN_Count != 0 ||
            //m.Covered_Van_Count != 0 || m.Delivery_Van_Count != 0 || m.Mini_Truck_Count != 0 ||
            //m.Open_VAN_Count != 0 || m.Pickup_Count != 0 || m.Tank_Lorry_Count != 0 ||
            //m.Tipper_Count != 0 || m.Trailers_Count != 0 || m.Van_Count != 0)
            //    )
            //{
            //    OwnVehicleSummaryData.Add(new Dictionary<string, string>() {
            //    {"DepoName",item.DepoName},
            //    {"All_Vehicle_Count",item.All_Vehicle_Count==0?"":item.All_Vehicle_Count.ToString()},
            //    {"Cargo_Truck_Count",item.Cargo_Truck_Count==0?"":item.Cargo_Truck_Count.ToString()},
            //    {"Cargo_Truck_Open_Count",item.Cargo_Truck_Open_Count==0?"":item.Cargo_Truck_Open_Count.ToString()},
            //    {"Cargo_VAN_Count",item.Cargo_VAN_Count==0?"":item.Cargo_VAN_Count.ToString()},
            //    {"Covered_Van_Count",item.Covered_Van_Count==0?"":item.Covered_Van_Count.ToString()},
            //    {"Delivery_Van_Count",item.Delivery_Van_Count==0?"":item.Delivery_Van_Count.ToString()},
            //    {"Mini_Truck_Count",item.Mini_Truck_Count==0?"":item.Mini_Truck_Count.ToString()},
            //    {"Open_VAN_Count",item.Open_VAN_Count==0?"":item.Open_VAN_Count.ToString()},
            //    {"Pickup_Count",item.Pickup_Count==0?"":item.Pickup_Count.ToString()},
            //    {"Tank_Lorry_Count",item.Tank_Lorry_Count==0?"":item.Tank_Lorry_Count.ToString()},
            //    {"Tipper_Count",item.Tipper_Count==0?"":item.Tipper_Count.ToString()},
            //    {"Trailers_Count",item.Trailers_Count==0?"":item.Trailers_Count.ToString()},
            //    {"Van_Count",item.Van_Count==0?"":item.Van_Count.ToString()},
            //    });
            //}

            //var _staringDateTime = DateTime.Now.AddDays(-1).Date;
            //var _endingDateTime = DateTime.Now.Date;
            //var TripData = (from requisitionTrips in db.RequisitionTrips.Where(m => m.FinalWantedAtDateTime > _staringDateTime && m.FinalWantedAtDateTime < _endingDateTime)
            //                join requisition in db.Requisitions on requisitionTrips.FK_Requisition equals requisition.PK_Requisition
            //                join client in db.AppUsers on requisition.FK_AppUser_Client equals client.PK_User
            //                join depo in db.Depoes on client.FK_Depo equals depo.PK_Depo
            //                group new { depo, requisitionTrips.OWN_MHT_DHT } by new { depo } into _group
            //                select new
            //                {
            //                    DepoName = _group.Key.depo.Name,
            //                    OWN_Trip = _group.Where(m => m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "OWN").Count(),
            //                    DHT_Trip = _group.Where(m => m.OWN_MHT_DHT == "DHT").Count()
            //                }
            //                        ).OrderBy(m => m.DepoName).ToList();
            //foreach (var item in TripData)
            //{
            //    if (OwnVehicleSummaryData.Where(m => m["DepoName"] == item.DepoName).Any())
            //    {
            //        OwnVehicleSummaryData.Where(m => m["DepoName"] == item.DepoName).FirstOrDefault()["OWN_Trip"] = item.OWN_Trip.ToString();
            //        OwnVehicleSummaryData.Where(m => m["DepoName"] == item.DepoName).FirstOrDefault()["DHT_Trip"] = item.DHT_Trip.ToString();
            //    }
            //    else
            //    {
            //        OwnVehicleSummaryData.Add(new Dictionary<string, string>() {
            //        {"DepoName", item.DepoName },
            //        {"All_Vehicle_Count", "" },
            //        {"Cargo_Truck_Count", "" },
            //        {"Cargo_Truck_Open_Count", "" },
            //        {"Cargo_VAN_Count", "" },
            //        {"Covered_Van_Count", ""},
            //        {"Delivery_Van_Count", ""},
            //        {"Mini_Truck_Count", ""},
            //        {"Open_VAN_Count",""},
            //        {"Pickup_Count", ""},
            //        {"Tank_Lorry_Count", ""},
            //        {"Tipper_Count", ""},
            //        {"Trailers_Count", ""},
            //        {"Van_Count", ""},
            //        {"OWN_Trip", item.OWN_Trip==0?"": item.OWN_Trip.ToString()},

            //       {"DHT_Trip", item.DHT_Trip==0?"": item.DHT_Trip.ToString()},
            //        });
            //    }
            //}
            ViewBag.OwnVehicleSummaryData = OwnVehicleSummaryData;

            //Accident History
            var AccidentSummaryData = new List<Dictionary<string, string>>();
            //var _1MonthBefore = DateTime.Now.AddDays(-30).Date;
            //var _AccidentSummaryData = (from accident in db.Accidents.Where(m => m.OccuranceDate > _1MonthBefore)
            //                            join vehicle in db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "OWN") on accident.FK_Vehicle equals vehicle.PK_Vehicle
            //                            join depo in db.Depoes.Where(m => m.IsDeleted == false) on vehicle.FK_Depo equals depo.PK_Depo
            //                            group new { depo, vehicle } by new { depo } into _group
            //                            select new
            //                            {
            //                                DepoName = _group.Key.depo.Name,
            //                                All_Acident_Count = _group.Count(),
            //                                Cargo_Truck_Count = _group.Where(m => m.vehicle.VehicleType == "Cargo Truck").Count(),
            //                                Cargo_Truck_Open_Count = _group.Where(m => m.vehicle.VehicleType == "Cargo Truck - Open").Count(),
            //                                Cargo_VAN_Count = _group.Where(m => m.vehicle.VehicleType == "Cargo VAN").Count(),
            //                                Covered_Van_Count = _group.Where(m => m.vehicle.VehicleType == "Covered Van").Count(),
            //                                Delivery_Van_Count = _group.Where(m => m.vehicle.VehicleType == "Delivery Van").Count(),
            //                                Mini_Truck_Count = _group.Where(m => m.vehicle.VehicleType == "Mini Truck").Count(),
            //                                Open_VAN_Count = _group.Where(m => m.vehicle.VehicleType == "Open VAN").Count(),
            //                                Pickup_Count = _group.Where(m => m.vehicle.VehicleType == "Pickup").Count(),
            //                                Tank_Lorry_Count = _group.Where(m => m.vehicle.VehicleType == "Tank Lorry").Count(),
            //                                Tipper_Count = _group.Where(m => m.vehicle.VehicleType == "Tipper").Count(),
            //                                Trailers_Count = _group.Where(m => m.vehicle.VehicleType == "Trailers").Count(),
            //                                Van_Count = _group.Where(m => m.vehicle.VehicleType == "Van").Count(),
            //                            }
            //            ).OrderBy(m => m.DepoName).ToList();
            //foreach (var item in _AccidentSummaryData
            //.Where(m => m.Cargo_Truck_Count != 0 || m.Cargo_Truck_Open_Count != 0 || m.Cargo_VAN_Count != 0 ||
            //m.Covered_Van_Count != 0 || m.Delivery_Van_Count != 0 || m.Mini_Truck_Count != 0 ||
            //m.Open_VAN_Count != 0 || m.Pickup_Count != 0 || m.Tank_Lorry_Count != 0 ||
            //m.Tipper_Count != 0 || m.Trailers_Count != 0 || m.Van_Count != 0)
            //    )
            //{
            //    AccidentSummaryData.Add(new Dictionary<string, string>() {
            //    {"DepoName",item.DepoName},
            //    {"All_Acident_Count",item.All_Acident_Count==0?"":item.All_Acident_Count.ToString()},
            //    {"Cargo_Truck_Count",item.Cargo_Truck_Count==0?"":item.Cargo_Truck_Count.ToString()},
            //    {"Cargo_Truck_Open_Count",item.Cargo_Truck_Open_Count==0?"":item.Cargo_Truck_Open_Count.ToString()},
            //    {"Cargo_VAN_Count",item.Cargo_VAN_Count==0?"":item.Cargo_VAN_Count.ToString()},
            //    {"Covered_Van_Count",item.Covered_Van_Count==0?"":item.Covered_Van_Count.ToString()},
            //    {"Delivery_Van_Count",item.Delivery_Van_Count==0?"":item.Delivery_Van_Count.ToString()},
            //    {"Mini_Truck_Count",item.Mini_Truck_Count==0?"":item.Mini_Truck_Count.ToString()},
            //    {"Open_VAN_Count",item.Open_VAN_Count==0?"":item.Open_VAN_Count.ToString()},
            //    {"Pickup_Count",item.Pickup_Count==0?"":item.Pickup_Count.ToString()},
            //    {"Tank_Lorry_Count",item.Tank_Lorry_Count==0?"":item.Tank_Lorry_Count.ToString()},
            //    {"Tipper_Count",item.Tipper_Count==0?"":item.Tipper_Count.ToString()},
            //    {"Trailers_Count",item.Trailers_Count==0?"":item.Trailers_Count.ToString()},
            //    {"Van_Count",item.Van_Count==0?"":item.Van_Count.ToString()},
            //    });
            //}
            ViewBag.AccidentSummaryData = AccidentSummaryData;

            //PoliceCase History
            var PoliceCaseSummaryData = new List<Dictionary<string, string>>();
            //var _1MonthBefore = DateTime.Now.AddDays(-30).Date;
            //var _PoliceCaseSummaryData = (from policeCases in db.PoliceCases.Where(m => m.IssueDate > _1MonthBefore)
            //                              join vehicle in db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "OWN") on policeCases.FK_Vehicle equals vehicle.PK_Vehicle
            //                              join depo in db.Depoes.Where(m => m.IsDeleted == false) on vehicle.FK_Depo equals depo.PK_Depo
            //                              group new { depo, vehicle } by new { depo } into _group
            //                              select new
            //                              {
            //                                  DepoName = _group.Key.depo.Name,
            //                                  All_Acident_Count = _group.Count(),
            //                                  Cargo_Truck_Count = _group.Where(m => m.vehicle.VehicleType == "Cargo Truck").Count(),
            //                                  Cargo_Truck_Open_Count = _group.Where(m => m.vehicle.VehicleType == "Cargo Truck - Open").Count(),
            //                                  Cargo_VAN_Count = _group.Where(m => m.vehicle.VehicleType == "Cargo VAN").Count(),
            //                                  Covered_Van_Count = _group.Where(m => m.vehicle.VehicleType == "Covered Van").Count(),
            //                                  Delivery_Van_Count = _group.Where(m => m.vehicle.VehicleType == "Delivery Van").Count(),
            //                                  Mini_Truck_Count = _group.Where(m => m.vehicle.VehicleType == "Mini Truck").Count(),
            //                                  Open_VAN_Count = _group.Where(m => m.vehicle.VehicleType == "Open VAN").Count(),
            //                                  Pickup_Count = _group.Where(m => m.vehicle.VehicleType == "Pickup").Count(),
            //                                  Tank_Lorry_Count = _group.Where(m => m.vehicle.VehicleType == "Tank Lorry").Count(),
            //                                  Tipper_Count = _group.Where(m => m.vehicle.VehicleType == "Tipper").Count(),
            //                                  Trailers_Count = _group.Where(m => m.vehicle.VehicleType == "Trailers").Count(),
            //                                  Van_Count = _group.Where(m => m.vehicle.VehicleType == "Van").Count(),
            //                              }
            //            ).OrderBy(m => m.DepoName).ToList();
            //foreach (var item in _PoliceCaseSummaryData
            //.Where(m => m.Cargo_Truck_Count != 0 || m.Cargo_Truck_Open_Count != 0 || m.Cargo_VAN_Count != 0 ||
            //m.Covered_Van_Count != 0 || m.Delivery_Van_Count != 0 || m.Mini_Truck_Count != 0 ||
            //m.Open_VAN_Count != 0 || m.Pickup_Count != 0 || m.Tank_Lorry_Count != 0 ||
            //m.Tipper_Count != 0 || m.Trailers_Count != 0 || m.Van_Count != 0)
            //    )
            //{
            //    AccidentSummaryData.Add(new Dictionary<string, string>() {
            //    {"DepoName",item.DepoName},
            //    {"All_Acident_Count",item.All_Acident_Count==0?"":item.All_Acident_Count.ToString()},
            //    {"Cargo_Truck_Count",item.Cargo_Truck_Count==0?"":item.Cargo_Truck_Count.ToString()},
            //    {"Cargo_Truck_Open_Count",item.Cargo_Truck_Open_Count==0?"":item.Cargo_Truck_Open_Count.ToString()},
            //    {"Cargo_VAN_Count",item.Cargo_VAN_Count==0?"":item.Cargo_VAN_Count.ToString()},
            //    {"Covered_Van_Count",item.Covered_Van_Count==0?"":item.Covered_Van_Count.ToString()},
            //    {"Delivery_Van_Count",item.Delivery_Van_Count==0?"":item.Delivery_Van_Count.ToString()},
            //    {"Mini_Truck_Count",item.Mini_Truck_Count==0?"":item.Mini_Truck_Count.ToString()},
            //    {"Open_VAN_Count",item.Open_VAN_Count==0?"":item.Open_VAN_Count.ToString()},
            //    {"Pickup_Count",item.Pickup_Count==0?"":item.Pickup_Count.ToString()},
            //    {"Tank_Lorry_Count",item.Tank_Lorry_Count==0?"":item.Tank_Lorry_Count.ToString()},
            //    {"Tipper_Count",item.Tipper_Count==0?"":item.Tipper_Count.ToString()},
            //    {"Trailers_Count",item.Trailers_Count==0?"":item.Trailers_Count.ToString()},
            //    {"Van_Count",item.Van_Count==0?"":item.Van_Count.ToString()},
            //    });
            //}
            ViewBag.PoliceCaswSummaryData = PoliceCaseSummaryData;

            //VehicleVsTrip
            var VehicleVsTripSummaryData = new List<Dictionary<string, string>>();
            //var _staringDateTime2 = DateTime.Now.AddDays(-1).Date;
            //var _endingDateTime2 = DateTime.Now.Date;
            ////var TripData1 = (from requisitionTrips in db.RequisitionTrips.Where(m => m.FinalWantedAtDateTime > _staringDateTime2 && m.FinalWantedAtDateTime < _endingDateTime2)
            ////                 join vehicle in db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN") on requisitionTrips.FK_Vehicle equals vehicle.PK_Vehicle
            ////                 group new { vehicle, requisitionTrips } by new { vehicle } into _group
            ////                 select new
            ////                 {
            ////                     _group.Key.vehicle.RegistrationNumber,
            ////                     TripCount = _group.Count()
            ////                 }
            ////                ).GroupBy(m => m.TripCount).Select(m => new { TripCount = m.Key, VehicleCount = m.Count() }).ToList();

            //var _VehicleVsTripSummaryData = (from row in (from requisitionTrips in db.RequisitionTrips.Where(m => m.FinalWantedAtDateTime > _staringDateTime2 && m.FinalWantedAtDateTime < _endingDateTime2)
            //                                              join vehicle in db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN") on requisitionTrips.FK_Vehicle equals vehicle.PK_Vehicle
            //                                              group new { vehicle, requisitionTrips } by new { vehicle } into _group
            //                                              select new
            //                                              {
            //                                                  //_group.Key.vehicle.RegistrationNumber,
            //                                                  TripCount = _group.Count()
            //                                              })
            //                                 group new { row } by new { row.TripCount } into _group
            //                                 select new
            //                                 {
            //                                     TripCount = _group.Key.TripCount,
            //                                     VehicleCount = _group.Count()
            //                                 }
            //           ).ToList();
            //foreach (var item in _VehicleVsTripSummaryData)
            //{
            //    VehicleVsTripSummaryData.Add(
            //        new Dictionary<string, string>() {
            //            {"TripCount",item.TripCount.ToString() },
            //            {"VehicleCount",item.VehicleCount.ToString() }
            //        });
            //}
            ViewBag.VehicleVsTripSummaryData = VehicleVsTripSummaryData;
            return View();
        }
    }
}