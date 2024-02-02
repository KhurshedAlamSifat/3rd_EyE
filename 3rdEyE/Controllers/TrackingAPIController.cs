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
using System.Data.SqlClient;

namespace _3rdEyE.Controllers
{
    public class TrackingAPIController : BaseAPIController
    {
        //public JsonResult GetVehicleTracking(Guid PK_User)
        //{
        //    List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
        //    DataTable dataTable = new DataTable();
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter adpt = new SqlDataAdapter();
        //    cmd.Connection = (SqlConnection)db.Database.Connection;
        //    string query = "";
        //    query = "EXEC ReportMobile_GetVehicleTracking '" + PK_User + "'";
        //    cmd.CommandText = query;
        //    adpt.SelectCommand = cmd;
        //    dataTable.Reset();
        //    adpt.Fill(dataTable);
        //    dictioneryList.AddRange(GetTableRows(dataTable));
        //    return Json(dictioneryList, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetVehicleTrackingNear(Guid PK_User, string Latitude, string Longitude)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)db.Database.Connection;
            string query = "";
            query = "EXEC ReportMobile_GetVehicleTracking_Near '" + PK_User + "', '" + Latitude.Split('.')[0] + "', '" + Longitude.Split('.')[0] + "'";

            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            return Json(dictioneryList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVehicleTrackingFar(Guid PK_User, string Latitude, string Longitude)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)db.Database.Connection;
            string query = "";
            query = "EXEC ReportMobile_GetVehicleTracking_Far '" + PK_User + "', '" + Latitude.Split('.')[0] + "', '" + Longitude.Split('.')[0] + "'";

            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            return Json(dictioneryList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehicleTracking_Single(Guid PK_Vehicle, DateTime? PreviousUpdateTime)
        {
            if (PreviousUpdateTime == null)
            {
                PreviousUpdateTime = DateTime.Now.AddMinutes(-5);
            }
            Dictionary<string, string> dictionery = new Dictionary<string, string>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)db.Database.Connection;
            string query = "";
            query = "EXEC Tracking_GetLastDeviceData'" + PK_Vehicle + "'";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictionery = GetTableRows(dataTable).FirstOrDefault();

            //query = "EXEC Tracking_GetNextDeviceData '" + PK_Vehicle + "', '" + PreviousUpdateTime.ToString() + "'";
            //cmd.CommandText = query;
            //adpt.SelectCommand = cmd;
            //dataTable.Reset();
            //adpt.Fill(dataTable);
            //dictionery = GetTableRows(dataTable).FirstOrDefault();
            //if (dictionery == null)
            //{
            //    query = "EXEC Tracking_GetLastDeviceData'" + PK_Vehicle + "'";
            //    cmd.CommandText = query;
            //    adpt.SelectCommand = cmd;
            //    dataTable.Reset();
            //    adpt.Fill(dataTable);
            //    dictionery = GetTableRows(dataTable).FirstOrDefault();
            //}
            return Json(dictionery, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVehicleDetailData(Guid PK_Vehicle)
        {
            //Dictionary<string, object> data = new Dictionary<string, object>();
            var vehicle = db.Vehicles.Where(m => m.PK_Vehicle == PK_Vehicle).FirstOrDefault();

            //#BasicInformatoin
            var BasicInfo = new
            {
                PRG_Type = vehicle.Depo.PRG_Type,
                DepotCategory = vehicle.Depo.Category,
                Depot = vehicle.Depo.Name,
                DepoGroup = vehicle.DepoGroup != null ? vehicle.DepoGroup.Name : "",
                ContactNumber = vehicle.Internal_VehicleContactNumber
            };

            //#RequisitionTrip
            var RequisitionTripInfo = new Object();
            if (vehicle.FK_RequisitionTrip_Last != null)
            {
                var requisitionTrip = db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == vehicle.FK_RequisitionTrip_Last).FirstOrDefault();
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
                        ToLocationLatLong = new { requisitionTrip.Requisition.Location1.Latitude, requisitionTrip.Requisition.Location1.Longitude }
                    };
                }
            }

            //#ParkingInOutInfo
            var ParkingInOutInfo = new object();
            if (vehicle.FK_ParkingInOut_Last != null)
            {
                var parkingInOut = db.ParkingInOuts.Where(m => m.PK_ParkingInOut == vehicle.FK_ParkingInOut_Last && m.Out_IssueDateTime == null).FirstOrDefault();
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
        //public JsonResult GetData_Single_firstTime(Guid PK_Vehicle)
        //{
        //    Dictionary<string, string> dictionery = new Dictionary<string, string>();
        //    DataTable dataTable = new DataTable();
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter adpt = new SqlDataAdapter();
        //    cmd.Connection = (SqlConnection)db.Database.Connection;
        //    string query = "";
        //    query = "EXEC Tracking_GetNextDeviceData '" + PK_Vehicle + "', '" + DateTime.Now.AddMinutes(-2).ToString() + "'";
        //    cmd.CommandText = query;
        //    adpt.SelectCommand = cmd;
        //    dataTable.Reset();
        //    adpt.Fill(dataTable);
        //    dictionery = GetTableRows(dataTable).FirstOrDefault();
        //    if (dictionery == null)
        //    {
        //        query = "EXEC Tracking_GetLastDeviceData'" + PK_Vehicle + "'";
        //        cmd.CommandText = query;
        //        adpt.SelectCommand = cmd;
        //        dataTable.Reset();
        //        adpt.Fill(dataTable);
        //        dictionery = GetTableRows(dataTable).FirstOrDefault();
        //    }
        //    return Json(dictionery, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetData_Single(Guid PK_Vehicle)
        //{
        //    Dictionary<string, string> dictionery = new Dictionary<string, string>();
        //    DataTable dataTable = new DataTable();
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter adpt = new SqlDataAdapter();
        //    cmd.Connection = (SqlConnection)db.Database.Connection;
        //    string query = "";
        //    query = "EXEC Report_GetVehicleTracking_Single '" + PK_Vehicle + "'";
        //    cmd.CommandText = query;
        //    adpt.SelectCommand = cmd;
        //    dataTable.Reset();
        //    adpt.Fill(dataTable);
        //    dictionery = GetTableRows(dataTable).FirstOrDefault();
        //    return Json(dictionery, JsonRequestBehavior.AllowGet);
        //}

    }
}