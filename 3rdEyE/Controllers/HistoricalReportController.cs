using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.BLL;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;
using Microsoft.Reporting.WebForms;

namespace _3rdEyE.Controllers
{
    public class HistoricalReportController : BaseController
    {
        Dictionary<string, string> OWN_MHT_DHT_Dict = new Dictionary<string, string> { { "OWN", "OWN" }, { "MHT", "MHT" }, { "DHT", "DHT" } };

        #region //Halt Report
        public ActionResult HaltReport()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var vehicles = (from vt in bll.db.VehicleTrackings
                            join v in bll.db.Vehicles on vt.PK_Vehicle equals v.PK_Vehicle
                            select v).ToList();
            ViewBag.Vehicles = new SelectList(vehicles.OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
            return View();
        }
        public JsonResult GetVehicleHaltTime(string FK_Vehicles, DateTime StartingDate, DateTime EndingDate, Int32 MinimumMinute)
        {
            DateTime _StartingDate;
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> _dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 600;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            var FK_Vehicle_list = FK_Vehicles.Split(',');
            if (FK_Vehicle_list.Count() > 0)
            {
                foreach (var FK in FK_Vehicle_list)
                {
                    if (FK == "")
                    {
                        break;
                    }
                    _StartingDate = StartingDate;
                    _dictioneryList = new List<Dictionary<string, string>>();
                    while (_StartingDate <= EndingDate)
                    {
                        query = "EXEC Report_GetVehicleHaltTime '" + CurrentUser.PK_User + "', '" + FK + "', '" + _StartingDate.ToString() + "', '" + _StartingDate.AddDays(1).ToString() + "', '" + MinimumMinute + "'";
                        cmd.CommandText = query;
                        adpt.SelectCommand = cmd;
                        dataTable.Reset();
                        adpt.Fill(dataTable);
                        _dictioneryList.AddRange(GetTableRows(dataTable));
                        _dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewDate_end" }, { "DateString", _StartingDate.ToString("MMMM dd yyyy") } });
                        _StartingDate = _StartingDate.AddDays(1);
                    }
                    var _FK = Guid.Parse(FK);
                    var vehicleRegNumber = bll.db.Vehicles.Where(m => m.PK_Vehicle == _FK).Select(m => m.RegistrationNumber).FirstOrDefault();
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_start" }, { "_rowSpan", (_dictioneryList.Count + 1).ToString() }, { "NewVehicle", vehicleRegNumber } });

                    dictioneryList.AddRange(_dictioneryList);
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_end" }, { "StartingDate", StartingDate.ToString("MMMM dd yyyy") }, { "EndingDate", EndingDate.ToString("MMMM dd yyyy") } });
                }
            }
            return Json(dictioneryList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region //Vehicle History
        public ActionResult VehicleHistory()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var vehicles = (from vt in bll.db.VehicleTrackings
                            join v in bll.db.Vehicles on vt.PK_Vehicle equals v.PK_Vehicle
                            select v).ToList();
            ViewBag.Vehicles = new SelectList(vehicles.OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
            return View();
        }
        public JsonResult GetVehicleHistory(string FK_Vehicle, DateTime StartingDate, DateTime EndingDate, Int32 TimeLap)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 600;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Report_GetVehicleHistory '" + CurrentUser.PK_User + "', '" + FK_Vehicle + "', '" + StartingDate.ToString() + "', '" + EndingDate.ToString() + "', " + TimeLap;
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            return Json(dictioneryList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVehicleHistoryDetail(string FK_Vehicle, DateTime StartingDate, DateTime EndingDate)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 600;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Report_GetVehicleHistoryDetail '" + CurrentUser.PK_User + "', '" + FK_Vehicle + "', '" + StartingDate.ToString() + "', '" + EndingDate.ToString() + "', " + 0000;
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            return Json(dictioneryList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region //ConsolidatedReport
        public ActionResult ConsolidatedReport()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var vehicles = (from vt in bll.db.VehicleTrackings
                            join v in bll.db.Vehicles on vt.PK_Vehicle equals v.PK_Vehicle
                            select v).ToList();
            ViewBag.Vehicles = new SelectList(vehicles.OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
            return View();
        }
        public JsonResult GetConsolidatedReport(string FK_Vehicles, DateTime StartingDate, DateTime EndingDate)
        {
            DateTime _StartingDate;
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> _dictioneryList = new List<Dictionary<string, string>>();

            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 600;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            var FK_Vehicle_list = FK_Vehicles.Split(',');
            if (FK_Vehicle_list.Count() > 0)
            {
                foreach (var FK in FK_Vehicle_list)
                {
                    if (FK == "")
                    {
                        break;
                    }
                    _StartingDate = StartingDate;
                    _dictioneryList = new List<Dictionary<string, string>>();

                    while (_StartingDate <= EndingDate)
                    {
                        query = "EXEC Report_GetVehicleConsolidatedReport '" + FK + "', '" + _StartingDate.ToString() + "'";
                        cmd.CommandText = query;
                        adpt.SelectCommand = cmd;
                        dataTable.Reset();
                        adpt.Fill(dataTable);
                        _dictioneryList.AddRange(GetTableRows(dataTable));
                        _StartingDate = _StartingDate.AddDays(1);
                    }
                    var _FK = Guid.Parse(FK);
                    var vehicleRegNumber = bll.db.Vehicles.Where(m => m.PK_Vehicle == _FK).Select(m => m.RegistrationNumber).FirstOrDefault();
                    var vehicleMobileNumber = bll.db.Vehicles.Where(m => m.PK_Vehicle == _FK).Select(m => m.MHT_DHT_DriverContactNumber).FirstOrDefault();
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_start" }, { "_rowSpan", (_dictioneryList.Count + 1).ToString() }, { "NewVehicle", vehicleRegNumber }, { "NewVehicle_mobileNumber", vehicleMobileNumber } });

                    dictioneryList.AddRange(_dictioneryList);
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_end" }, { "StartingDate", StartingDate.ToString("MMMM dd yyyy") }, { "EndingDate", EndingDate.ToString("MMMM dd yyyy") } });

                }
            }
            return Json(dictioneryList, JsonRequestBehavior.AllowGet);
        }
        #endregion



    }
}