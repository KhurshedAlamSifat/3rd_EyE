using _3rdEyE.ManagingTools;
using _3rdEyE.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class VehicleGateNewController : BaseController
    {
        Dictionary<string, string> OWN_MHT_DHT_Dict = new Dictionary<string, string> { { "OWN", "OWN" }, { "MHT", "MHT" }, { "DHT", "DHT" } };
        Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" }, { "CS", "CS" } };
        Dictionary<string, string> MaxStayTimeHourDict = new Dictionary<string, string> { { "2", "2" }, { "4", "4" }, { "6", "6" }, { "8", "8" }, { "10", "10" }, { "12", "12" }, { "14", "14" }, { "16", "16" }, { "18", "18" }, { "20", "20" }, { "22", "22" }, { "24", "24" } };
        Dictionary<string, string> MinStayTimeHourDict = new Dictionary<string, string> { { "1", "1" }, { "2", "2" }, { "3", "3" }, { "4", "4" }, { "6", "6" }, { "8", "8" }, { "10", "10" } };

        static class InternalTripStatus
        {
            public const string Assigned = "Assigned";
            public const string EnteredStartingLocation = "Entered Starting Location";
            public const string StartedLoading = "Started Loading";
            public const string FinishedLoading = "Finished Loading";
            public const string StartedEmptyTrip = "Started Empty Trip";
            public const string CreatedBill = "Created Bill";
            public const string PaidBill = "Paid Bill";
            public const string LeftStartingLoaction = "Left Starting Loaction";
            public const string EnteredFinishingLocation = "Entered Finishing Location";
            public const string StartedUnloading = "Started Unloading";
            public const string FinishedUnloading = "Finished Unloading";
            public const string FinishedEmptyTrip = "Finished Empty Trip";
        }
        static class ExternalTripStatus
        {
            public const string Assigned = "Assigned";
            public const string EnteredStartingLocation = "Entered Starting Location";
            public const string LeftStartingLoaction = "Left Starting Loaction";
            public const string EnteredFinishingLocation = "Entered Finishing Location";
            public const string LeftFinishingLocation = "Left Finishing Location";
        }
        public VehicleGateNewController()
        {
            bll.db.Database.CommandTimeout = int.MaxValue;
        }
        //VehicleInOutManual_Index
        public ActionResult VehicleInOutManual_Index(string OWN_MHT_DHT, string PRG_Type, String FK_Location, string StartingDate, string EndingDate, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<vw_VehicleInOutManual> list = new List<vw_VehicleInOutManual>();

            var query = bll.db.vw_VehicleInOutManual.AsQueryable();

            //OWN_MHT_DHTList
            if (OWN_MHT_DHT == null)
            {
                //Do Nothing
            }
            else if (OWN_MHT_DHT != "all")
            {
                query = query.Where(m => m.Vehicle_OWN_MHT_DHT == OWN_MHT_DHT);
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
                query = query.Where(m => m.Depo_PRG_Type == PRG_Type);
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

                query = query.Where(m => m.FK_Location == _FK_Location);
            }
            List<SelectListItem> LocationList = new List<SelectListItem>();
            LocationList.Add(new SelectListItem() { Value = "all", Text = "All" });
            LocationList.AddRange(bll.db.Locations.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Location.ToString(), Text = m.Name }));
            ViewBag.Locations = new SelectList(LocationList.OrderBy(m => m.Text), "Value", "Text", FK_Location);

            //StartingDate EndingDate
            if (!string.IsNullOrEmpty(StartingDate) && !string.IsNullOrEmpty(EndingDate))
            {
                var _StartingDate = Convert.ToDateTime(StartingDate);
                var _EndingDate = Convert.ToDateTime(EndingDate);
                query = query.Where(m =>
                (_StartingDate < m.In_IssueDateTime && m.Out_IssueDateTime < _EndingDate) || // [↓↑]
                (m.In_IssueDateTime < _StartingDate && _StartingDate < m.Out_IssueDateTime) || // ↓[↑
                (m.In_IssueDateTime < _EndingDate && (_EndingDate < m.Out_IssueDateTime || m.Out_IssueDateTime == null)) || // [↓]↑*
                (m.In_IssueDateTime < _StartingDate && (_EndingDate < m.Out_IssueDateTime || m.Out_IssueDateTime == null))// ↓[]↑*
                );
            }
            ViewBag.StartingDate = StartingDate;
            ViewBag.EndingDate = EndingDate;

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //final
            if ((!string.IsNullOrEmpty(OWN_MHT_DHT)) || (!string.IsNullOrEmpty(PRG_Type)) || (!string.IsNullOrEmpty(FK_Location)) || (!string.IsNullOrEmpty(StartingDate)) || (!string.IsNullOrEmpty(EndingDate)) || (!string.IsNullOrEmpty(RegistrationNumber)))
            {
                list = query.OrderBy(m => m.In_IssueDateTime).ToList();
                //var endingDateLimit = DateTime.Now;
                //if (!string.IsNullOrEmpty(EndingDate))
                //{
                //    endingDateLimit = Convert.ToDateTime(EndingDate);
                //}
                var endingDateLimit = DateTime.Now;
                foreach (var item in list.Where(m => m.Out_IssueDateTime == null))
                {
                    item.InStayTimeMinute = (long)(endingDateLimit - item.In_IssueDateTime).TotalMinutes;
                    if (item.InStayTimeMinute < 0)
                    {
                        item.InStayTimeMinute = 0;
                    }
                }
            }

            return View(list);
        }

        public void VehicleInOutManual_ExcelDownload(string OWN_MHT_DHT, string PRG_Type, String FK_Location, string StartingDate, string EndingDate, String RegistrationNumber)
        {
            List<vw_VehicleInOutManual> list = new List<vw_VehicleInOutManual>();

            var query = bll.db.vw_VehicleInOutManual.AsQueryable();

            //OWN_MHT_DHTList
            if (OWN_MHT_DHT == null)
            {
                //Do Nothing
            }
            else if (OWN_MHT_DHT != "all")
            {
                query = query.Where(m => m.Vehicle_OWN_MHT_DHT == OWN_MHT_DHT);
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
                query = query.Where(m => m.Depo_PRG_Type == PRG_Type);
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

                query = query.Where(m => m.FK_Location == _FK_Location);
            }
            List<SelectListItem> LocationList = new List<SelectListItem>();
            LocationList.Add(new SelectListItem() { Value = "all", Text = "All" });
            LocationList.AddRange(bll.db.Locations.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Location.ToString(), Text = m.Name }));
            ViewBag.Locations = new SelectList(LocationList.OrderBy(m => m.Text), "Value", "Text", FK_Location);

            //StartingDate EndingDate
            if (!string.IsNullOrEmpty(StartingDate) && !string.IsNullOrEmpty(EndingDate))
            {
                var _StartingDate = Convert.ToDateTime(StartingDate);
                var _EndingDate = Convert.ToDateTime(EndingDate);
                query = query.Where(m =>
                (_StartingDate < m.In_IssueDateTime && m.Out_IssueDateTime < _EndingDate) || // [↓↑]
                (m.In_IssueDateTime < _StartingDate && _StartingDate < m.Out_IssueDateTime) || // ↓[↑
                (m.In_IssueDateTime < _EndingDate && (_EndingDate < m.Out_IssueDateTime || m.Out_IssueDateTime == null)) || // [↓]↑*
                (m.In_IssueDateTime < _StartingDate && (_EndingDate < m.Out_IssueDateTime || m.Out_IssueDateTime == null))// ↓[]↑*
                );
            }
            ViewBag.StartingDate = StartingDate;
            ViewBag.EndingDate = EndingDate;

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //final
            if ((!string.IsNullOrEmpty(OWN_MHT_DHT)) || (!string.IsNullOrEmpty(PRG_Type)) || (!string.IsNullOrEmpty(FK_Location)) || (!string.IsNullOrEmpty(StartingDate)) || (!string.IsNullOrEmpty(EndingDate)) || (!string.IsNullOrEmpty(RegistrationNumber)))
            {
                list = query.OrderBy(m => m.In_IssueDateTime).ToList();
                //var endingDateLimit = DateTime.Now;
                //if (!string.IsNullOrEmpty(EndingDate))
                //{
                //    endingDateLimit = Convert.ToDateTime(EndingDate);
                //}
                var endingDateLimit = DateTime.Now;
                foreach (var item in list.Where(m => m.Out_IssueDateTime == null))
                {
                    item.InStayTimeMinute = (long)(endingDateLimit - item.In_IssueDateTime).TotalMinutes;
                    if (item.InStayTimeMinute < 0)
                    {
                        item.InStayTimeMinute = 0;
                    }
                }
            }



            //# region Excell_File_Generate
            ExcelPackage file = new ExcelPackage();
            ExcelWorksheet sheet = file.Workbook.Worksheets.Add("Sheet 1");

            int rowNumber = 1;

            //#COMMON
            sheet.Cells["A" + rowNumber.ToString()].Value = "Vehicle";
            sheet.Cells["B" + rowNumber.ToString()].Value = "O/M/D";
            sheet.Cells["C" + rowNumber.ToString()].Value = "PRG Type";
            sheet.Cells["D" + rowNumber.ToString()].Value = "Location";
            sheet.Cells["E" + rowNumber.ToString()].Value = "User/Depot";

            //#IN Part
            sheet.Cells["F" + rowNumber.ToString()].Value = "Destination";
            sheet.Cells["G" + rowNumber.ToString()].Value = "In Gate";
            sheet.Cells["H" + rowNumber.ToString()].Value = "In Date";
            sheet.Cells["I" + rowNumber.ToString()].Value = "In Time";
            sheet.Cells["J" + rowNumber.ToString()].Value = "In Status";
            sheet.Cells["K" + rowNumber.ToString()].Value = "In Reason";
            sheet.Cells["L" + rowNumber.ToString()].Value = "In Scan";

            //#OUT Part
            sheet.Cells["M" + rowNumber.ToString()].Value = "Out Gate";
            sheet.Cells["N" + rowNumber.ToString()].Value = "Out Date";
            sheet.Cells["O" + rowNumber.ToString()].Value = "Out Time";
            sheet.Cells["P" + rowNumber.ToString()].Value = "Out Status";
            sheet.Cells["Q" + rowNumber.ToString()].Value = "Out Reason";
            sheet.Cells["R" + rowNumber.ToString()].Value = "Out Scan";

            //#STAY TIME Calculation
            sheet.Cells["S" + rowNumber.ToString()].Value = "Stay Time (Hour)";
            sheet.Cells["T" + rowNumber.ToString()].Value = "Stay Time (Min)";
            sheet.Cells["U" + rowNumber.ToString()].Value = "Depot Category";

            foreach (var item in list)
            {
                rowNumber++;
                //#COMMON
                //sheet.Cells["A" + rowNumber.ToString()].Value = "Vehicle";
                sheet.Cells["A" + rowNumber.ToString()].Value = item.RegistrationNumber;
                //sheet.Cells["B" + rowNumber.ToString()].Value = "O/M/D";
                sheet.Cells["B" + rowNumber.ToString()].Value = item.Vehicle_OWN_MHT_DHT;
                //sheet.Cells["C" + rowNumber.ToString()].Value = "PRG Type";
                sheet.Cells["C" + rowNumber.ToString()].Value = item.Vehicle_OWN_MHT_DHT != "DHT" ? item.Depo_PRG_Type : item.InGate_PRG_Type;
                //sheet.Cells["D" + rowNumber.ToString()].Value = "Location";
                sheet.Cells["D" + rowNumber.ToString()].Value = item.Location_Name;
                //sheet.Cells["E" + rowNumber.ToString()].Value = "User/Depot";
                sheet.Cells["E" + rowNumber.ToString()].Value = item.Depo_Name;

                //#IN Part
                //sheet.Cells["F" + rowNumber.ToString()].Value = "Destination";
                sheet.Cells["F" + rowNumber.ToString()].Value = item.Destination;
                //sheet.Cells["G" + rowNumber.ToString()].Value = "In Gate";
                sheet.Cells["G" + rowNumber.ToString()].Value = item.InGate_FullName;
                //sheet.Cells["H" + rowNumber.ToString()].Value = "In Date";
                sheet.Cells["H" + rowNumber.ToString()].Value = item.In_IssueDateTime.ToString("dd/MM/yyyy");
                //sheet.Cells["I" + rowNumber.ToString()].Value = "In Time";
                sheet.Cells["I" + rowNumber.ToString()].Value = item.In_IssueDateTime.ToString("H:mm");
                //sheet.Cells["J" + rowNumber.ToString()].Value = "In Status";
                sheet.Cells["J" + rowNumber.ToString()].Value = item.In_LoadOrEmpty;
                //sheet.Cells["K" + rowNumber.ToString()].Value = "In Reason";
                sheet.Cells["K" + rowNumber.ToString()].Value = item.InReason_TitleBangla;
                //sheet.Cells["L" + rowNumber.ToString()].Value = "In Scan";
                sheet.Cells["L" + rowNumber.ToString()].Value = item.In_IsScannedEntry == true ? "Scan" : "Manual";

                //#OUT Part
                //sheet.Cells["M" + rowNumber.ToString()].Value = "Out Gate";
                sheet.Cells["M" + rowNumber.ToString()].Value = item.OutGate_FullName != null ? item.OutGate_FullName : "";
                //sheet.Cells["N" + rowNumber.ToString()].Value = "Out Date";
                sheet.Cells["N" + rowNumber.ToString()].Value = item.Out_IssueDateTime != null ? item.Out_IssueDateTime.Value.ToString("dd/MM/yyyy") : "";
                //sheet.Cells["O" + rowNumber.ToString()].Value = "Out Time";
                sheet.Cells["O" + rowNumber.ToString()].Value = item.Out_IssueDateTime != null ? item.Out_IssueDateTime.Value.ToString("H:mm") : "";
                //sheet.Cells["P" + rowNumber.ToString()].Value = "Out Status";
                sheet.Cells["P" + rowNumber.ToString()].Value = item.Out_LoadOrEmpty != null ? item.Out_LoadOrEmpty : "";
                //sheet.Cells["Q" + rowNumber.ToString()].Value = "Out Reason";
                sheet.Cells["Q" + rowNumber.ToString()].Value = item.OutReason_TitleBangla != null ? item.OutReason_TitleBangla : "";
                //sheet.Cells["R" + rowNumber.ToString()].Value = "Out Scan";
                sheet.Cells["R" + rowNumber.ToString()].Value = item.Out_IsScannedEntry == true ? "Scan" : item.Out_IsScannedEntry == false ? "Manual" : "";

                //#STAY TIME Calculation
                if (item.InStayTimeMinute != null)
                {
                    var _hour = Math.Round((Double)item.InStayTimeMinute / 60, 2); //(((Int64)item.InStayTimeMinute) / 60);
                    sheet.Cells["S" + rowNumber.ToString()].Value = _hour;
                }
                else
                {
                    sheet.Cells["S" + rowNumber.ToString()].Value = "";
                }
                sheet.Cells["T" + rowNumber.ToString()].Value = item.InStayTimeMinute != null ? item.InStayTimeMinute.ToString() : "";
                sheet.Cells["U" + rowNumber.ToString()].Value = item.Depo_Category;
            }
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + "InOutReport_" + System.DateTime.Now.ToString("dd-MMM-yyyy") + ".xlsx");
            Response.BinaryWrite(file.GetAsByteArray());
            Response.End();


        }
        //VehicleInOutManual_Index2
        public ActionResult VehicleInOutManual_Index2(string StartingDate, string EndingDate, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<vw_VehicleInOutManual> list = new List<vw_VehicleInOutManual>();

            var query = bll.db.vw_VehicleInOutManual.AsQueryable();

            //StartingDate EndingDate
            if (!string.IsNullOrEmpty(StartingDate) && !string.IsNullOrEmpty(EndingDate))
            {
                var _StartingDate = Convert.ToDateTime(StartingDate);
                var _EndingDate = Convert.ToDateTime(EndingDate);
                _EndingDate = _EndingDate.AddDays(1);
                query = query.Where(m =>
                (_StartingDate < m.In_IssueDateTime && m.Out_IssueDateTime < _EndingDate) || // [↓↑]
                (m.In_IssueDateTime < _StartingDate && _StartingDate < m.Out_IssueDateTime) || // ↓[↑
                (m.In_IssueDateTime < _EndingDate && (_EndingDate < m.Out_IssueDateTime || m.Out_IssueDateTime == null)) || // [↓]↑*
                (m.In_IssueDateTime < _StartingDate && (_EndingDate < m.Out_IssueDateTime || m.Out_IssueDateTime == null))// ↓[]↑*
                );
            }
            ViewBag.StartingDate = StartingDate;
            ViewBag.EndingDate = EndingDate;

            ////StartingDate
            //var _StartingDate = Convert.ToDateTime(StartingDate);
            //query = query.Where(m => (m.In_IssueDateTime >= _StartingDate) || (m.In_IssueDateTime < _StartingDate && m.Out_IssueDateTime == null));
            //ViewBag.StartingDate = StartingDate;

            ////EndingDate
            //var _EndingDate = Convert.ToDateTime(EndingDate);
            //_EndingDate = _EndingDate.AddDays(1);
            //query = query.Where(m => m.In_IssueDateTime < _EndingDate);
            //ViewBag.EndingDate = EndingDate;

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //final
            if ((!string.IsNullOrEmpty(StartingDate)) || (!string.IsNullOrEmpty(EndingDate)) || (!string.IsNullOrEmpty(RegistrationNumber)))
            {
                list = query.OrderBy(m => m.In_IssueDateTime).ToList();

                var endingDateLimit = Convert.ToDateTime(EndingDate);
                endingDateLimit = endingDateLimit.AddDays(1);
                foreach (var item in list.Where(m => m.Out_IssueDateTime > endingDateLimit))
                {
                    item.InStayTimeMinute = (long)(endingDateLimit - item.In_IssueDateTime).TotalMinutes;
                    if (item.InStayTimeMinute < 0)
                    {
                        item.InStayTimeMinute = 0;
                    }
                }

                var now = DateTime.Now;
                foreach (var item in list.Where(m => m.Out_IssueDateTime == null))
                {
                    item.InStayTimeMinute = (long)(now - item.In_IssueDateTime).TotalMinutes;
                    if (item.InStayTimeMinute < 0)
                    {
                        item.InStayTimeMinute = 0;
                    }
                }
            }

            return View(list);
        }

        public void VehicleInOutManual_ExcelDownload2(string StartingDate, string EndingDate, String RegistrationNumber)
        {
            List<vw_VehicleInOutManual> list = new List<vw_VehicleInOutManual>();

            var query = bll.db.vw_VehicleInOutManual.AsQueryable();

            //StartingDate EndingDate
            if (!string.IsNullOrEmpty(StartingDate) && !string.IsNullOrEmpty(EndingDate))
            {
                var _StartingDate = Convert.ToDateTime(StartingDate);
                var _EndingDate = Convert.ToDateTime(EndingDate);
                _EndingDate = _EndingDate.AddDays(1);
                query = query.Where(m =>
                (_StartingDate < m.In_IssueDateTime && m.Out_IssueDateTime < _EndingDate) || // [↓↑]
                (m.In_IssueDateTime < _StartingDate && _StartingDate < m.Out_IssueDateTime) || // ↓[↑
                (m.In_IssueDateTime < _EndingDate && (_EndingDate < m.Out_IssueDateTime || m.Out_IssueDateTime == null)) || // [↓]↑*
                (m.In_IssueDateTime < _StartingDate && (_EndingDate < m.Out_IssueDateTime || m.Out_IssueDateTime == null))// ↓[]↑*
                );
            }
            ViewBag.StartingDate = StartingDate;
            ViewBag.EndingDate = EndingDate;

            ////StartingDate
            //var _StartingDate = Convert.ToDateTime(StartingDate);
            //query = query.Where(m => (m.In_IssueDateTime >= _StartingDate) || (m.In_IssueDateTime < _StartingDate && m.Out_IssueDateTime == null));
            //ViewBag.StartingDate = StartingDate;

            ////EndingDate
            //var _EndingDate = Convert.ToDateTime(EndingDate);
            //_EndingDate = _EndingDate.AddDays(1);
            //query = query.Where(m => m.In_IssueDateTime < _EndingDate);
            //ViewBag.EndingDate = EndingDate;

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //final
            if ((!string.IsNullOrEmpty(StartingDate)) || (!string.IsNullOrEmpty(EndingDate)) || (!string.IsNullOrEmpty(RegistrationNumber)))
            {
                list = query.OrderBy(m => m.In_IssueDateTime).ToList();

                var endingDateLimit = Convert.ToDateTime(EndingDate);
                endingDateLimit = endingDateLimit.AddDays(1);
                foreach (var item in list.Where(m => m.Out_IssueDateTime > endingDateLimit))
                {
                    item.InStayTimeMinute = (long)(endingDateLimit - item.In_IssueDateTime).TotalMinutes;
                    if (item.InStayTimeMinute < 0)
                    {
                        item.InStayTimeMinute = 0;
                    }
                }

                var now = DateTime.Now;
                foreach (var item in list.Where(m => m.Out_IssueDateTime == null))
                {
                    item.InStayTimeMinute = (long)(now - item.In_IssueDateTime).TotalMinutes;
                    if (item.InStayTimeMinute < 0)
                    {
                        item.InStayTimeMinute = 0;
                    }
                }
            }


            //# region Excell_File_Generate
            ExcelPackage file = new ExcelPackage();
            ExcelWorksheet sheet = file.Workbook.Worksheets.Add("Sheet 1");

            int rowNumber = 1;

            //#COMMON
            sheet.Cells["A" + rowNumber.ToString()].Value = "Vehicle";
            sheet.Cells["B" + rowNumber.ToString()].Value = "O/M/D";
            sheet.Cells["C" + rowNumber.ToString()].Value = "PRG Type";
            sheet.Cells["D" + rowNumber.ToString()].Value = "Location";
            sheet.Cells["E" + rowNumber.ToString()].Value = "User/Depot";

            //#IN Part
            sheet.Cells["F" + rowNumber.ToString()].Value = "Destination";
            sheet.Cells["G" + rowNumber.ToString()].Value = "In Gate";
            sheet.Cells["H" + rowNumber.ToString()].Value = "In Date";
            sheet.Cells["I" + rowNumber.ToString()].Value = "In Time";
            sheet.Cells["J" + rowNumber.ToString()].Value = "In Status";
            sheet.Cells["K" + rowNumber.ToString()].Value = "In Reason";
            sheet.Cells["L" + rowNumber.ToString()].Value = "In Scan";

            //#OUT Part
            sheet.Cells["M" + rowNumber.ToString()].Value = "Out Gate";
            sheet.Cells["N" + rowNumber.ToString()].Value = "Out Date";
            sheet.Cells["O" + rowNumber.ToString()].Value = "Out Time";
            sheet.Cells["P" + rowNumber.ToString()].Value = "Out Status";
            sheet.Cells["Q" + rowNumber.ToString()].Value = "Out Reason";
            sheet.Cells["R" + rowNumber.ToString()].Value = "Out Scan";

            //#STAY TIME Calculation
            sheet.Cells["S" + rowNumber.ToString()].Value = "Stay Time (hh:mm)";
            sheet.Cells["T" + rowNumber.ToString()].Value = "Stay Time (Min)";
            sheet.Cells["U" + rowNumber.ToString()].Value = "Depot Category";

            foreach (var item in list)
            {
                rowNumber++;
                //#COMMON
                //sheet.Cells["A" + rowNumber.ToString()].Value = "Vehicle";
                sheet.Cells["A" + rowNumber.ToString()].Value = item.RegistrationNumber;
                //sheet.Cells["B" + rowNumber.ToString()].Value = "O/M/D";
                sheet.Cells["B" + rowNumber.ToString()].Value = item.Vehicle_OWN_MHT_DHT;
                //sheet.Cells["C" + rowNumber.ToString()].Value = "PRG Type";
                sheet.Cells["C" + rowNumber.ToString()].Value = item.Vehicle_OWN_MHT_DHT != "DHT" ? item.Depo_PRG_Type : item.InGate_PRG_Type;
                //sheet.Cells["D" + rowNumber.ToString()].Value = "Location";
                sheet.Cells["D" + rowNumber.ToString()].Value = item.Location_Name;
                //sheet.Cells["E" + rowNumber.ToString()].Value = "User/Depot";
                sheet.Cells["E" + rowNumber.ToString()].Value = item.Depo_Name;

                //#IN Part
                //sheet.Cells["F" + rowNumber.ToString()].Value = "Destination";
                sheet.Cells["F" + rowNumber.ToString()].Value = item.Destination != null ? item.Destination : "";
                //sheet.Cells["G" + rowNumber.ToString()].Value = "In Gate";
                sheet.Cells["G" + rowNumber.ToString()].Value = item.InGate_FullName;
                //sheet.Cells["H" + rowNumber.ToString()].Value = "In Date";
                sheet.Cells["H" + rowNumber.ToString()].Value = item.In_IssueDateTime.ToString("dd/MM/yyyy");
                //sheet.Cells["I" + rowNumber.ToString()].Value = "In Time";
                sheet.Cells["I" + rowNumber.ToString()].Value = item.In_IssueDateTime.ToString("H:mm");
                //sheet.Cells["J" + rowNumber.ToString()].Value = "In Status";
                sheet.Cells["J" + rowNumber.ToString()].Value = item.In_LoadOrEmpty;
                //sheet.Cells["K" + rowNumber.ToString()].Value = "In Reason";
                sheet.Cells["K" + rowNumber.ToString()].Value = item.InReason_TitleBangla != null ? item.InReason_TitleBangla : "";
                //sheet.Cells["L" + rowNumber.ToString()].Value = "In Scan";
                sheet.Cells["L" + rowNumber.ToString()].Value = item.In_IsScannedEntry == true ? "Scan" : "Manual";

                //#OUT Part
                //sheet.Cells["M" + rowNumber.ToString()].Value = "Out Gate";
                sheet.Cells["M" + rowNumber.ToString()].Value = item.OutGate_FullName != null ? item.OutGate_FullName : "";
                //sheet.Cells["N" + rowNumber.ToString()].Value = "Out Date";
                sheet.Cells["N" + rowNumber.ToString()].Value = item.Out_IssueDateTime != null ? item.Out_IssueDateTime.Value.ToString("dd/MM/yyyy") : "";
                //sheet.Cells["O" + rowNumber.ToString()].Value = "Out Time";
                sheet.Cells["O" + rowNumber.ToString()].Value = item.Out_IssueDateTime != null ? item.Out_IssueDateTime.Value.ToString("H:mm") : "";
                //sheet.Cells["P" + rowNumber.ToString()].Value = "Out Status";
                sheet.Cells["P" + rowNumber.ToString()].Value = item.Out_LoadOrEmpty != null ? item.Out_LoadOrEmpty : "";
                //sheet.Cells["Q" + rowNumber.ToString()].Value = "Out Reason";
                sheet.Cells["Q" + rowNumber.ToString()].Value = item.OutReason_TitleBangla != null ? item.OutReason_TitleBangla : "";
                //sheet.Cells["R" + rowNumber.ToString()].Value = "Out Scan";
                sheet.Cells["R" + rowNumber.ToString()].Value = item.Out_IsScannedEntry == true ? "Scan" : "Manual";

                //#STAY TIME Calculation
                if (item.InStayTimeMinute != null)
                {
                    var _hour = Math.Round((Double)item.InStayTimeMinute / 60, 2); //(((Int64)item.InStayTimeMinute) / 60);
                    sheet.Cells["S" + rowNumber.ToString()].Value = _hour;
                }
                else
                {
                    sheet.Cells["S" + rowNumber.ToString()].Value = "";
                }
                sheet.Cells["T" + rowNumber.ToString()].Value = item.InStayTimeMinute != null ? item.InStayTimeMinute.ToString() : "";
                sheet.Cells["U" + rowNumber.ToString()].Value = item.Depo_Category;
            }
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + "StayReport_" + System.DateTime.Now.ToString("dd-MMM-yyyy") + ".xlsx");
            Response.BinaryWrite(file.GetAsByteArray());
            Response.End();


        }

        //VehicleInOutManual_Index3
        public ActionResult VehicleInOutManual_Index3(string StartingDate, string EndingDate, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<vw_VehicleInOutManual> list = new List<vw_VehicleInOutManual>();

            var query = bll.db.vw_VehicleInOutManual.AsQueryable();

            //StartingDate EndingDate
            if (!string.IsNullOrEmpty(StartingDate) && !string.IsNullOrEmpty(EndingDate))
            {
                var _StartingDate = Convert.ToDateTime(StartingDate);
                var _EndingDate = Convert.ToDateTime(EndingDate);
                _EndingDate = _EndingDate.AddDays(1);
                query = query.Where(m =>
                (_StartingDate < m.In_IssueDateTime && m.Out_IssueDateTime < _EndingDate) || // [↓↑]
                (m.In_IssueDateTime < _StartingDate && _StartingDate < m.Out_IssueDateTime) || // ↓[↑
                (m.In_IssueDateTime < _EndingDate && (_EndingDate < m.Out_IssueDateTime || m.Out_IssueDateTime == null)) || // [↓]↑*
                (m.In_IssueDateTime < _StartingDate && (_EndingDate < m.Out_IssueDateTime || m.Out_IssueDateTime == null))// ↓[]↑*
                );
            }
            ViewBag.StartingDate = StartingDate;
            ViewBag.EndingDate = EndingDate;

            ////StartingDate
            //var _StartingDate = Convert.ToDateTime(StartingDate);
            //query = query.Where(m => (m.In_IssueDateTime >= _StartingDate) || (m.In_IssueDateTime < _StartingDate && m.Out_IssueDateTime == null));
            //ViewBag.StartingDate = StartingDate;

            ////EndingDate
            //var _EndingDate = Convert.ToDateTime(EndingDate);
            //_EndingDate = _EndingDate.AddDays(1);
            //query = query.Where(m => m.In_IssueDateTime < _EndingDate);
            //ViewBag.EndingDate = EndingDate;

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //final
            if ((!string.IsNullOrEmpty(StartingDate)) || (!string.IsNullOrEmpty(EndingDate)) || (!string.IsNullOrEmpty(RegistrationNumber)))
            {
                list = query.OrderBy(m => m.In_IssueDateTime).ToList();
                var endingDateLimit = Convert.ToDateTime(EndingDate);
                endingDateLimit = endingDateLimit.AddDays(1);
                var now = DateTime.Now;

                var startingDateLimit = Convert.ToDateTime(StartingDate);
                foreach (var item in list)
                {
                    //# Limit In
                    var _In_IssueDateTime = new DateTime();
                    if (item.In_IssueDateTime < startingDateLimit)
                    {
                        _In_IssueDateTime = startingDateLimit;
                    }
                    else
                    {
                        _In_IssueDateTime = item.In_IssueDateTime;
                    }

                    //# Limit Out
                    var _Out_IssueDateTime = new DateTime();
                    if (item.Out_IssueDateTime > endingDateLimit)
                    {
                        _Out_IssueDateTime = endingDateLimit;
                    }
                    else if (item.Out_IssueDateTime == null)
                    {
                        _Out_IssueDateTime = now;
                    }
                    else
                    {
                        _Out_IssueDateTime = item.Out_IssueDateTime ?? new DateTime();
                    }

                    item.InStayTimeMinute = (long)(_Out_IssueDateTime - _In_IssueDateTime).TotalMinutes;
                    if (item.InStayTimeMinute < 0)
                    {
                        item.InStayTimeMinute = 0;
                    }
                }
            }

            return View(list);
        }

        public void VehicleInOutManual_ExcelDownload3(string StartingDate, string EndingDate, String RegistrationNumber)
        {
            List<vw_VehicleInOutManual> list = new List<vw_VehicleInOutManual>();

            var query = bll.db.vw_VehicleInOutManual.AsQueryable();

            //StartingDate EndingDate
            if (!string.IsNullOrEmpty(StartingDate) && !string.IsNullOrEmpty(EndingDate))
            {
                var _StartingDate = Convert.ToDateTime(StartingDate);
                var _EndingDate = Convert.ToDateTime(EndingDate);
                _EndingDate = _EndingDate.AddDays(1);
                query = query.Where(m =>
                (_StartingDate < m.In_IssueDateTime && m.Out_IssueDateTime < _EndingDate) || // [↓↑]
                (m.In_IssueDateTime < _StartingDate && _StartingDate < m.Out_IssueDateTime) || // ↓[↑
                (m.In_IssueDateTime < _EndingDate && (_EndingDate < m.Out_IssueDateTime || m.Out_IssueDateTime == null)) || // [↓]↑*
                (m.In_IssueDateTime < _StartingDate && (_EndingDate < m.Out_IssueDateTime || m.Out_IssueDateTime == null))// ↓[]↑*
                );
            }
            ViewBag.StartingDate = StartingDate;
            ViewBag.EndingDate = EndingDate;

            ////StartingDate
            //var _StartingDate = Convert.ToDateTime(StartingDate);
            //query = query.Where(m => (m.In_IssueDateTime >= _StartingDate) || (m.In_IssueDateTime < _StartingDate && m.Out_IssueDateTime == null));
            //ViewBag.StartingDate = StartingDate;

            ////EndingDate
            //var _EndingDate = Convert.ToDateTime(EndingDate);
            //_EndingDate = _EndingDate.AddDays(1);
            //query = query.Where(m => m.In_IssueDateTime < _EndingDate);
            //ViewBag.EndingDate = EndingDate;

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //final
            if ((!string.IsNullOrEmpty(StartingDate)) || (!string.IsNullOrEmpty(EndingDate)) || (!string.IsNullOrEmpty(RegistrationNumber)))
            {
                list = query.OrderBy(m => m.In_IssueDateTime).ToList();
                var endingDateLimit = Convert.ToDateTime(EndingDate);
                endingDateLimit = endingDateLimit.AddDays(1);
                var now = DateTime.Now;

                var startingDateLimit = Convert.ToDateTime(StartingDate);
                foreach (var item in list)
                {
                    //# Limit In
                    var _In_IssueDateTime = new DateTime();
                    if (item.In_IssueDateTime < startingDateLimit)
                    {
                        _In_IssueDateTime = startingDateLimit;
                    }
                    else
                    {
                        _In_IssueDateTime = item.In_IssueDateTime;
                    }

                    //# Limit Out
                    var _Out_IssueDateTime = new DateTime();
                    if (item.Out_IssueDateTime > endingDateLimit)
                    {
                        _Out_IssueDateTime = endingDateLimit;
                    }
                    else if (item.Out_IssueDateTime == null)
                    {
                        _Out_IssueDateTime = now;
                    }
                    else
                    {
                        _Out_IssueDateTime = item.Out_IssueDateTime ?? new DateTime();
                    }

                    item.InStayTimeMinute = (long)(_Out_IssueDateTime - _In_IssueDateTime).TotalMinutes;
                    if (item.InStayTimeMinute < 0)
                    {
                        item.InStayTimeMinute = 0;
                    }
                }
            }



            //# region Excell_File_Generate
            ExcelPackage file = new ExcelPackage();
            ExcelWorksheet sheet = file.Workbook.Worksheets.Add("Sheet 1");

            int rowNumber = 1;

            //#COMMON
            sheet.Cells["A" + rowNumber.ToString()].Value = "Vehicle";
            sheet.Cells["B" + rowNumber.ToString()].Value = "O/M/D";
            sheet.Cells["C" + rowNumber.ToString()].Value = "PRG Type";
            sheet.Cells["D" + rowNumber.ToString()].Value = "Location";
            sheet.Cells["E" + rowNumber.ToString()].Value = "User/Depot";

            //#IN Part
            sheet.Cells["F" + rowNumber.ToString()].Value = "Destination";
            sheet.Cells["G" + rowNumber.ToString()].Value = "In Gate";
            sheet.Cells["H" + rowNumber.ToString()].Value = "In Date";
            sheet.Cells["I" + rowNumber.ToString()].Value = "In Time";
            sheet.Cells["J" + rowNumber.ToString()].Value = "In Status";
            sheet.Cells["K" + rowNumber.ToString()].Value = "In Reason";
            sheet.Cells["L" + rowNumber.ToString()].Value = "In Scan";

            //#OUT Part
            sheet.Cells["M" + rowNumber.ToString()].Value = "Out Gate";
            sheet.Cells["N" + rowNumber.ToString()].Value = "Out Date";
            sheet.Cells["O" + rowNumber.ToString()].Value = "Out Time";
            sheet.Cells["P" + rowNumber.ToString()].Value = "Out Status";
            sheet.Cells["Q" + rowNumber.ToString()].Value = "Out Reason";
            sheet.Cells["R" + rowNumber.ToString()].Value = "Out Scan";

            //#STAY TIME Calculation
            sheet.Cells["S" + rowNumber.ToString()].Value = "Stay Time (hh:mm)";
            sheet.Cells["T" + rowNumber.ToString()].Value = "Stay Time (Min)";
            sheet.Cells["U" + rowNumber.ToString()].Value = "Depot Category";

            foreach (var item in list)
            {
                rowNumber++;
                //#COMMON
                //sheet.Cells["A" + rowNumber.ToString()].Value = "Vehicle";
                sheet.Cells["A" + rowNumber.ToString()].Value = item.RegistrationNumber;
                //sheet.Cells["B" + rowNumber.ToString()].Value = "O/M/D";
                sheet.Cells["B" + rowNumber.ToString()].Value = item.Vehicle_OWN_MHT_DHT;
                //sheet.Cells["C" + rowNumber.ToString()].Value = "PRG Type";
                sheet.Cells["C" + rowNumber.ToString()].Value = item.Vehicle_OWN_MHT_DHT != "DHT" ? item.Depo_PRG_Type : item.InGate_PRG_Type;
                //sheet.Cells["D" + rowNumber.ToString()].Value = "Location";
                sheet.Cells["D" + rowNumber.ToString()].Value = item.Location_Name;
                //sheet.Cells["E" + rowNumber.ToString()].Value = "User/Depot";
                sheet.Cells["E" + rowNumber.ToString()].Value = item.Depo_Name;

                //#IN Part
                //sheet.Cells["F" + rowNumber.ToString()].Value = "Destination";
                sheet.Cells["F" + rowNumber.ToString()].Value = item.Destination != null ? item.Destination : "";
                //sheet.Cells["G" + rowNumber.ToString()].Value = "In Gate";
                sheet.Cells["G" + rowNumber.ToString()].Value = item.InGate_FullName;
                //sheet.Cells["H" + rowNumber.ToString()].Value = "In Date";
                sheet.Cells["H" + rowNumber.ToString()].Value = item.In_IssueDateTime.ToString("dd/MM/yyyy");
                //sheet.Cells["I" + rowNumber.ToString()].Value = "In Time";
                sheet.Cells["I" + rowNumber.ToString()].Value = item.In_IssueDateTime.ToString("H:mm");
                //sheet.Cells["J" + rowNumber.ToString()].Value = "In Status";
                sheet.Cells["J" + rowNumber.ToString()].Value = item.In_LoadOrEmpty;
                //sheet.Cells["K" + rowNumber.ToString()].Value = "In Reason";
                sheet.Cells["K" + rowNumber.ToString()].Value = item.InReason_TitleBangla != null ? item.InReason_TitleBangla : "";
                //sheet.Cells["L" + rowNumber.ToString()].Value = "In Scan";
                sheet.Cells["L" + rowNumber.ToString()].Value = item.In_IsScannedEntry == true ? "Scan" : "Manual";

                //#OUT Part
                //sheet.Cells["M" + rowNumber.ToString()].Value = "Out Gate";
                sheet.Cells["M" + rowNumber.ToString()].Value = item.OutGate_FullName != null ? item.OutGate_FullName : "";
                //sheet.Cells["N" + rowNumber.ToString()].Value = "Out Date";
                sheet.Cells["N" + rowNumber.ToString()].Value = item.Out_IssueDateTime != null ? item.Out_IssueDateTime.Value.ToString("dd/MM/yyyy") : "";
                //sheet.Cells["O" + rowNumber.ToString()].Value = "Out Time";
                sheet.Cells["O" + rowNumber.ToString()].Value = item.Out_IssueDateTime != null ? item.Out_IssueDateTime.Value.ToString("H:mm") : "";
                //sheet.Cells["P" + rowNumber.ToString()].Value = "Out Status";
                sheet.Cells["P" + rowNumber.ToString()].Value = item.Out_LoadOrEmpty != null ? item.Out_LoadOrEmpty : "";
                //sheet.Cells["Q" + rowNumber.ToString()].Value = "Out Reason";
                sheet.Cells["Q" + rowNumber.ToString()].Value = item.OutReason_TitleBangla != null ? item.OutReason_TitleBangla : "";
                //sheet.Cells["R" + rowNumber.ToString()].Value = "Out Scan";
                sheet.Cells["R" + rowNumber.ToString()].Value = item.Out_IsScannedEntry == true ? "Scan" : "Manual";

                //#STAY TIME Calculation
                if (item.InStayTimeMinute != null)
                {
                    var _hour = Math.Round((Double)item.InStayTimeMinute / 60, 2); //(((Int64)item.InStayTimeMinute) / 60);
                    sheet.Cells["S" + rowNumber.ToString()].Value = _hour;
                }
                else
                {
                    sheet.Cells["S" + rowNumber.ToString()].Value = "";
                }
                sheet.Cells["T" + rowNumber.ToString()].Value = item.InStayTimeMinute != null ? item.InStayTimeMinute.ToString() : "";
                sheet.Cells["U" + rowNumber.ToString()].Value = item.Depo_Category;
            }
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + "StayReportDaily_" + System.DateTime.Now.ToString("dd-MMM-yyyy") + ".xlsx");
            Response.BinaryWrite(file.GetAsByteArray());
            Response.End();


        }

        //VehicleInOutManual_Index4
        public ActionResult VehicleInOutManual_Index4(string ReportDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<vw_VehicleInOutManual> list = new List<vw_VehicleInOutManual>();

            var _StartingLimit = Convert.ToDateTime(ReportDate);
            var _EndingLimit = _StartingLimit.AddDays(1);
            var query = bll.db.vw_VehicleInOutManual.AsQueryable();
            query = query.Where(m =>
                (_StartingLimit < m.In_IssueDateTime && m.Out_IssueDateTime < _EndingLimit) || // [↓↑]
                (m.In_IssueDateTime < _StartingLimit && _StartingLimit < m.Out_IssueDateTime) || // ↓[↑
                (m.In_IssueDateTime < _EndingLimit && (_EndingLimit < m.Out_IssueDateTime || m.Out_IssueDateTime == null)) || // [↓]↑*
                (m.In_IssueDateTime < _StartingLimit && (_EndingLimit < m.Out_IssueDateTime || m.Out_IssueDateTime == null))// ↓[]↑*
                );
            list = query.ToList();

            var list_still_inside = list.Where(m => m.Out_IssueDateTime == null || m.Out_IssueDateTime > _EndingLimit).ToList();

            //final
            foreach (var item in list_still_inside)
            {
                item.InStayTimeMinute = (long)(_EndingLimit - item.In_IssueDateTime).TotalMinutes;
                if (item.InStayTimeMinute < 0)
                {
                    item.InStayTimeMinute = 0;
                }
            }

            return View(list);
        }

        public void VehicleInOutManual_ExcelDownload4(string ReportDate)
        {
            List<vw_VehicleInOutManual> list = new List<vw_VehicleInOutManual>();

            var _StartingLimit = Convert.ToDateTime(ReportDate);
            var _EndingLimit = _StartingLimit.AddDays(1);
            var query = bll.db.vw_VehicleInOutManual.AsQueryable();
            query = query.Where(m =>
                (_StartingLimit < m.In_IssueDateTime && m.Out_IssueDateTime < _EndingLimit) || // [↓↑]
                (m.In_IssueDateTime < _StartingLimit && _StartingLimit < m.Out_IssueDateTime) || // ↓[↑
                (m.In_IssueDateTime < _EndingLimit && (_EndingLimit < m.Out_IssueDateTime || m.Out_IssueDateTime == null)) || // [↓]↑*
                (m.In_IssueDateTime < _StartingLimit && (_EndingLimit < m.Out_IssueDateTime || m.Out_IssueDateTime == null))// ↓[]↑*
                );
            list = query.ToList();

            var list_still_inside = list.Where(m => m.Out_IssueDateTime == null || m.Out_IssueDateTime > _EndingLimit).ToList();

            //final
            foreach (var item in list_still_inside)
            {
                item.InStayTimeMinute = (long)(_EndingLimit - item.In_IssueDateTime).TotalMinutes;
                if (item.InStayTimeMinute < 0)
                {
                    item.InStayTimeMinute = 0;
                }
            }

            //# region Excell_File_Generate
            ExcelPackage file = new ExcelPackage();
            ExcelWorksheet sheet = file.Workbook.Worksheets.Add("Sheet 1");

            int rowNumber = 1;

            //#COMMON
            sheet.Cells["A" + rowNumber.ToString()].Value = "Vehicle";
            sheet.Cells["B" + rowNumber.ToString()].Value = "O/M/D";
            sheet.Cells["C" + rowNumber.ToString()].Value = "PRG Type";
            sheet.Cells["D" + rowNumber.ToString()].Value = "Location";
            sheet.Cells["E" + rowNumber.ToString()].Value = "User/Depot";

            //#IN Part
            sheet.Cells["F" + rowNumber.ToString()].Value = "In Gate";
            sheet.Cells["G" + rowNumber.ToString()].Value = "In Date";
            sheet.Cells["H" + rowNumber.ToString()].Value = "In Time";
            sheet.Cells["I" + rowNumber.ToString()].Value = "In Status";
            sheet.Cells["J" + rowNumber.ToString()].Value = "In Reason";
            sheet.Cells["K" + rowNumber.ToString()].Value = "In Scan";

            //#OUT Part
            sheet.Cells["L" + rowNumber.ToString()].Value = "Out Gate";
            sheet.Cells["M" + rowNumber.ToString()].Value = "Out Date";
            sheet.Cells["N" + rowNumber.ToString()].Value = "Out Time";
            sheet.Cells["O" + rowNumber.ToString()].Value = "Out Status";
            sheet.Cells["P" + rowNumber.ToString()].Value = "Out Reason";
            sheet.Cells["Q" + rowNumber.ToString()].Value = "Out Scan";

            //#STAY TIME Calculation
            sheet.Cells["R" + rowNumber.ToString()].Value = "Stay Time (hh:mm)";
            sheet.Cells["S" + rowNumber.ToString()].Value = "Stay Time (Min)";
            sheet.Cells["T" + rowNumber.ToString()].Value = "Depot Category";
            sheet.Cells["U" + rowNumber.ToString()].Value = "Destination";

            foreach (var item in list)
            {
                rowNumber++;
                //#COMMON
                //sheet.Cells["A" + rowNumber.ToString()].Value = "Vehicle";
                sheet.Cells["A" + rowNumber.ToString()].Value = item.RegistrationNumber;
                //sheet.Cells["B" + rowNumber.ToString()].Value = "O/M/D";
                sheet.Cells["B" + rowNumber.ToString()].Value = item.Vehicle_OWN_MHT_DHT;
                //sheet.Cells["C" + rowNumber.ToString()].Value = "PRG Type";
                sheet.Cells["C" + rowNumber.ToString()].Value = item.Vehicle_OWN_MHT_DHT != "DHT" ? item.Depo_PRG_Type : item.InGate_PRG_Type;
                //sheet.Cells["D" + rowNumber.ToString()].Value = "Location";
                sheet.Cells["D" + rowNumber.ToString()].Value = item.Location_Name;
                //sheet.Cells["E" + rowNumber.ToString()].Value = "User/Depot";
                sheet.Cells["E" + rowNumber.ToString()].Value = item.Depo_Name;

                //#IN Part
                //sheet.Cells["F" + rowNumber.ToString()].Value = "In Gate";
                sheet.Cells["F" + rowNumber.ToString()].Value = item.InGate_FullName;
                //sheet.Cells["G" + rowNumber.ToString()].Value = "In Date";
                sheet.Cells["G" + rowNumber.ToString()].Value = item.In_IssueDateTime.ToString("dd/MM/yyyy");
                //sheet.Cells["H" + rowNumber.ToString()].Value = "In Time";
                sheet.Cells["H" + rowNumber.ToString()].Value = item.In_IssueDateTime.ToString("H:mm");
                //sheet.Cells["I" + rowNumber.ToString()].Value = "In Status";
                sheet.Cells["I" + rowNumber.ToString()].Value = item.In_LoadOrEmpty;
                //sheet.Cells["J" + rowNumber.ToString()].Value = "In Reason";
                sheet.Cells["J" + rowNumber.ToString()].Value = item.InReason_TitleBangla;
                //sheet.Cells["K" + rowNumber.ToString()].Value = "In Scan";
                sheet.Cells["K" + rowNumber.ToString()].Value = item.In_IsScannedEntry == true ? "Scan" : "Manual";

                //#OUT Part
                //sheet.Cells["L" + rowNumber.ToString()].Value = "Out Gate";
                sheet.Cells["L" + rowNumber.ToString()].Value = item.OutGate_FullName != null ? item.OutGate_FullName : "";
                //sheet.Cells["M" + rowNumber.ToString()].Value = "Out Date";
                sheet.Cells["M" + rowNumber.ToString()].Value = item.Out_IssueDateTime != null ? item.Out_IssueDateTime.Value.ToString("dd/MM/yyyy") : "";
                //sheet.Cells["N" + rowNumber.ToString()].Value = "Out Time";
                sheet.Cells["N" + rowNumber.ToString()].Value = item.Out_IssueDateTime != null ? item.Out_IssueDateTime.Value.ToString("H:mm") : "";
                //sheet.Cells["O" + rowNumber.ToString()].Value = "Out Status";
                sheet.Cells["O" + rowNumber.ToString()].Value = item.Out_LoadOrEmpty != null ? item.Out_LoadOrEmpty : "";
                //sheet.Cells["P" + rowNumber.ToString()].Value = "Out Reason";
                sheet.Cells["P" + rowNumber.ToString()].Value = item.OutReason_TitleBangla != null ? item.OutReason_TitleBangla : "";
                //sheet.Cells["Q" + rowNumber.ToString()].Value = "Out Scan";
                sheet.Cells["Q" + rowNumber.ToString()].Value = item.Out_IsScannedEntry == true ? "Scan" : "Manual";

                //#STAY TIME Calculation
                if (item.InStayTimeMinute != null)
                {
                    var _hour = Math.Round((Double)item.InStayTimeMinute / 60, 2); //(((Int64)item.InStayTimeMinute) / 60);
                    //var _min = (Int64)(item.InStayTimeMinute % 60);
                    sheet.Cells["R" + rowNumber.ToString()].Value = _hour;
                    //sheet.Cells["R" + rowNumber.ToString()].Value = (new DateTime() + TimeSpan.FromMinutes((double)item.InStayTimeMinute)).ToString("hh:mm:ss tt");
                }
                else
                {
                    sheet.Cells["R" + rowNumber.ToString()].Value = "";
                }
                //sheet.Cells["S" + rowNumber.ToString()].Value = "Stay Time (Min)";
                sheet.Cells["S" + rowNumber.ToString()].Value = item.InStayTimeMinute != null ? item.InStayTimeMinute.ToString() : "";
                sheet.Cells["T" + rowNumber.ToString()].Value = item.Depo_Category;
                sheet.Cells["U" + rowNumber.ToString()].Value = item.Destination != null ? item.Destination.ToString() : "";
            }
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + "StayReportDaily_" + System.DateTime.Now.ToString("dd-MMM-yyyy") + ".xlsx");
            Response.BinaryWrite(file.GetAsByteArray());
            Response.End();


        }

        public ActionResult EnteredVehicleIndex(String OWN_MHT_DHT, String PRG_Type, String FK_Location, String FK_AppUser_Gate, String FK_Destination, String RegistrationNumber, string MaxStayTimeHour)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<Vehicle> VehicleList = new List<Vehicle>();

            var query = bll.db.Vehicles.AsEnumerable().Where(m => m.LocationInOrOut == true);

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
                query = query.Where(m => ((m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT") && m.Depo.PRG_Type == PRG_Type) || ((m.OWN_MHT_DHT == "DHT") && m.VehicleInOutManual.AppUser.PRG_Type == PRG_Type));
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

                query = query.Where(m => m.FK_LocationInOut == _FK_Location);
            }
            List<SelectListItem> LocationList = new List<SelectListItem>();
            LocationList.Add(new SelectListItem() { Value = "all", Text = "All" });
            LocationList.AddRange(bll.db.Locations.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Location.ToString(), Text = m.Name }));
            ViewBag.Locations = new SelectList(LocationList.OrderBy(m => m.Text), "Value", "Text", FK_Location);

            //FK_AppUser_Gate
            if (FK_AppUser_Gate == null)
            {
                //Do Nothing
                List<SelectListItem> GateList = new List<SelectListItem>();
                GateList.Add(new SelectListItem() { Value = "all", Text = "All" });
                ViewBag.FK_AppUser_Gate = new SelectList(GateList.OrderBy(m => m.Text), "Value", "Text", FK_AppUser_Gate);
            }
            else if (FK_AppUser_Gate == "all")
            {
                //Do Nothing
                if (FK_Location == null)
                {
                    List<SelectListItem> GateList = new List<SelectListItem>();
                    GateList.Add(new SelectListItem() { Value = "all", Text = "All" });
                    ViewBag.FK_AppUser_Gate = new SelectList(GateList.OrderBy(m => m.Text), "Value", "Text", FK_AppUser_Gate);
                }
                else if (FK_Location != "all")
                {
                    var _FK_Location = Guid.Parse(FK_Location);
                    List<SelectListItem> GateList = new List<SelectListItem>();
                    GateList.Add(new SelectListItem() { Value = "all", Text = "All" });
                    GateList.AddRange(bll.db.AppUsers.AsEnumerable().Where(m => m.IsDeleted == false && m.AppUserType == "Internal Gate Entry Device" && m.FK_Location == _FK_Location).OrderBy(m => m.FullName).Select(m => new SelectListItem { Value = m.PK_User.ToString(), Text = m.FullName + " " + m.UniqueIDNumber }));
                    ViewBag.FK_AppUser_Gate = new SelectList(GateList.OrderBy(m => m.Text), "Value", "Text", FK_AppUser_Gate);
                }
                else // if (FK_Location == "all")
                {
                    List<SelectListItem> GateList = new List<SelectListItem>();
                    GateList.Add(new SelectListItem() { Value = "all", Text = "All" });
                    ViewBag.FK_AppUser_Gate = new SelectList(GateList.OrderBy(m => m.Text), "Value", "Text", FK_AppUser_Gate);
                }
            }
            else if (FK_AppUser_Gate != "all")
            {
                var _FK_AppUser_Gate = Guid.Parse(FK_AppUser_Gate);

                query = query.Where(m => m.VehicleInOutManual.In_FK_CreatedByUser == _FK_AppUser_Gate);
                List<SelectListItem> GateList = new List<SelectListItem>();
                GateList.Add(new SelectListItem() { Value = "all", Text = "All" });
                GateList.AddRange(bll.db.AppUsers.AsEnumerable().Where(m => m.IsDeleted == false && m.AppUserType == "Internal Gate Entry Device" && m.PK_User == _FK_AppUser_Gate).OrderBy(m => m.FullName).Select(m => new SelectListItem { Value = m.PK_User.ToString(), Text = m.FullName + " " + m.UniqueIDNumber }));
                ViewBag.FK_AppUser_Gate = new SelectList(GateList.OrderBy(m => m.Text), "Value", "Text", FK_AppUser_Gate);
            }


            //FK_Destination
            if (FK_Destination == null)
            {
                //Do Nothing
            }
            else if (FK_Destination != "all")
            {
                var _FK_Destination = Convert.ToInt64(FK_Destination);

                query = query.Where(m => m.VehicleInOutManual.FK_PRG_Type == _FK_Destination);
            }
            List<SelectListItem> DestinaitonList = new List<SelectListItem>();
            DestinaitonList.Add(new SelectListItem() { Value = "all", Text = "All" });
            DestinaitonList.AddRange(bll.db.PRG_Type.Where(m => m.IsDeleted != true && m.Show_VehicleGateInOutManual == true).Select(m => new SelectListItem { Value = m.PK_PRG_Type.ToString(), Text = m.Title }));
            ViewBag.FK_Destination = new SelectList(DestinaitonList.OrderBy(m => m.Text), "Value", "Text", FK_Destination);

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //MaxWaitingMinute
            if (!string.IsNullOrEmpty(MaxStayTimeHour))
            {
                ViewBag.MaxStayTimeHour = MaxStayTimeHour;
            }
            else
            {
                ViewBag.MaxStayTimeHour = "";
            }
            ViewBag.MaxStayTimeHourDict = new SelectList(MaxStayTimeHourDict, "Key", "Value", MaxStayTimeHour);


            //final
            if (OWN_MHT_DHT != null || PRG_Type != null || FK_Location != null || FK_AppUser_Gate != null || FK_Destination != null || RegistrationNumber != null || MaxStayTimeHour != null)
            {
                VehicleList = query.OrderBy(m => m.RegistrationNumber).ToList();
            }
            return View(VehicleList);
        }
        public ActionResult StayReport(String InsideOrOutside, String ReportDateTime, String OWN_MHT_DHT, String PRG_Type, String Depo_Category, String FK_Depo, String FK_Location, String FK_Vehicles, String FK_Destination, String MinStayTimeHour)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            var InsideOrOutsideList = new List<string>() { "Inside", "Outside" };
            ViewBag.InsideOrOutsideList = new SelectList(InsideOrOutsideList, InsideOrOutside);
            ViewBag.InsideOrOutside = InsideOrOutside;
            //#Common
            //ReportDateTime
            ViewBag.ReportDateTime = ReportDateTime;

            //OWN_MHT_DHTList
            List<SelectListItem> OWN_MHT_DHTList = new List<SelectListItem>();
            OWN_MHT_DHTList.Add(new SelectListItem() { Value = "all", Text = "All" });
            OWN_MHT_DHTList.AddRange(OWN_MHT_DHT_Dict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key, Text = m.Value }));
            ViewBag.OWN_MHT_DHT = new SelectList(OWN_MHT_DHTList.OrderBy(m => m.Text), "Value", "Text", OWN_MHT_DHT);

            //PRG_Type
            List<SelectListItem> PRG_TypeList = new List<SelectListItem>();
            PRG_TypeList.Add(new SelectListItem() { Value = "all", Text = "All" });
            PRG_TypeList.AddRange(PRG_TypesDict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key, Text = m.Value }));
            ViewBag.PRG_Type = new SelectList(PRG_TypeList.OrderBy(m => m.Text), "Value", "Text", PRG_Type);

            // #Depo_Category
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depo_Categories = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo)).Select(m => m.Category).Distinct(), Depo_Category);
            
            //FK_Location
            List<SelectListItem> LocationList = new List<SelectListItem>();
            LocationList.Add(new SelectListItem() { Value = "all", Text = "All" });
            LocationList.AddRange(bll.db.Locations.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Location.ToString(), Text = m.Name }));
            ViewBag.Locations = new SelectList(LocationList.OrderBy(m => m.Text), "Value", "Text", FK_Location);

            //FK_Depo
            if (FK_Depo == null)
            {
                //Do Nothing
                List<SelectListItem> DepoList = new List<SelectListItem>();
                DepoList.Add(new SelectListItem() { Value = "all", Text = "All" });
                ViewBag.DepoList = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text", FK_Depo);
            }
            else if (FK_Depo == "all")
            {
                //Do Nothing
                if (PRG_Type == null)
                {
                    List<SelectListItem> DepoList = new List<SelectListItem>();
                    DepoList.Add(new SelectListItem() { Value = "all", Text = "All" });
                    ViewBag.DepoList = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text", FK_Depo);
                }
                else if (PRG_Type != "all")
                {
                    List<SelectListItem> DepoList = new List<SelectListItem>();
                    DepoList.Add(new SelectListItem() { Value = "all", Text = "All" });
                    DepoList.AddRange(bll.db.Depoes.AsEnumerable().Where(m => m.IsDeleted == false && m.PRG_Type == PRG_Type).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Depo.ToString(), Text = m.Name }));
                    ViewBag.DepoList = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text", FK_Depo);
                }
                else // if (PRG_Type == "all")
                {
                    List<SelectListItem> DepoList = new List<SelectListItem>();
                    DepoList.Add(new SelectListItem() { Value = "all", Text = "All" });
                    ViewBag.DepoList = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text", FK_Depo);
                }
            }
            else if (FK_Depo != "all")
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                List<SelectListItem> DepoList = new List<SelectListItem>();
                DepoList.Add(new SelectListItem() { Value = "all", Text = "All" });
                DepoList.AddRange(bll.db.Depoes.AsEnumerable().Where(m => m.IsDeleted == false && m.PRG_Type == PRG_Type).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Depo.ToString(), Text = m.Name }));
                ViewBag.DepoList = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text", FK_Depo);
            }

            //FK_Vehicles
            if (!string.IsNullOrEmpty(FK_Vehicles))
            {
                var FK_VehicleArray = FK_Vehicles.Split(',').Select(m => Guid.Parse(m)).ToArray();
                ViewBag.Vehicles = new MultiSelectList(bll.db.Vehicles.Where(m => FK_VehicleArray.Contains(m.PK_Vehicle)), "PK_Vehicle", "RegistrationNumber", FK_VehicleArray);
            }
            else
            {
                ViewBag.Vehicles = new MultiSelectList(bll.db.Vehicles.Where(m => m.PK_Vehicle == null), "PK_Vehicle", "RegistrationNumber");
            }

            //FK_Destination
            List<SelectListItem> DestinaitonList = new List<SelectListItem>();
            DestinaitonList.Add(new SelectListItem() { Value = "all", Text = "All" });
            DestinaitonList.AddRange(bll.db.PRG_Type.Where(m => m.IsDeleted != true && m.Show_VehicleGateInOutManual == true).Select(m => new SelectListItem { Value = m.PK_PRG_Type.ToString(), Text = m.Title }));
            ViewBag.FK_Destination = new SelectList(DestinaitonList.OrderBy(m => m.Text), "Value", "Text", FK_Destination);

            //MinStayTimeHour
            if (!string.IsNullOrEmpty(MinStayTimeHour))
            {
                ViewBag.MinStayTimeHour = MinStayTimeHour;
            }
            else
            {
                ViewBag.MinStayTimeHour = "";
            }
            ViewBag.MinStayTimeHourDict = new SelectList(MinStayTimeHourDict, "Key", "Value", MinStayTimeHourDict);

            //#Inside
            DataTable insideDataTable = new DataTable();
            DataTable outsideDataTable = new DataTable();
            if (InsideOrOutside == "Inside")
            {
                string insideQuery = $"EXEC Report_VehicleInOutManual_StayReport ";

                //ReportDateTime
                if (!string.IsNullOrEmpty(ReportDateTime))
                {
                    var _ReportDateTime = Convert.ToDateTime(ReportDateTime);
                    insideQuery = insideQuery + $"@ReportDateTime = '{ReportDateTime}',";
                }

                //OWN_MHT_DHTList
                if (OWN_MHT_DHT == null)
                {
                    //Do Nothing
                }
                else if (OWN_MHT_DHT != "all")
                {
                    insideQuery = insideQuery + $"@OWN_MHT_DHT = '{OWN_MHT_DHT}',";
                }

                //PRG_Type
                if (PRG_Type == null)
                {
                    //Do Nothing
                }
                else if (PRG_Type != "all")
                {
                    insideQuery = insideQuery + $"@PRG_Type = '{PRG_Type}',";
                }

                //FK_Location
                if (FK_Location == null)
                {
                    //Do Nothing
                }
                else if (FK_Location != "all")
                {
                    var _FK_Location = Guid.Parse(FK_Location);
                    insideQuery = insideQuery + $"@FK_Location = '{FK_Location}',";
                }

                
                
                //FK_Depo
                if (FK_Depo == null)
                {
                }
                else if (FK_Depo == "all")
                {
                }
                else if (FK_Depo != "all")
                {
                    var _FK_Depo = Guid.Parse(FK_Depo);
                    insideQuery = insideQuery + $"@FK_Depo = '{FK_Depo}',";
                }

                //FK_Vehicles
                if (!string.IsNullOrEmpty(FK_Vehicles))
                {
                    insideQuery = insideQuery + $"@FK_Vehicles = '{FK_Vehicles}',";
                }

                //FK_Destination
                if (!string.IsNullOrEmpty(FK_Destination))
                {
                    insideQuery = insideQuery + $"@FK_Destination = '{FK_Destination}',";
                }

                //MinStayTimeHour
                if (!string.IsNullOrEmpty(MinStayTimeHour))
                {
                    insideQuery = insideQuery + $"@MinStayTimeHour = '{MinStayTimeHour}',";
                }

                //Group Category
                if (!string.IsNullOrEmpty(Depo_Category))
                {
                    insideQuery = insideQuery + $"@Depo_Category = '{Depo_Category}',";
                }

                //final
                if (!string.IsNullOrEmpty(ReportDateTime))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = int.MaxValue;
                    SqlDataAdapter adpt = new SqlDataAdapter();
                    cmd.Connection = (SqlConnection)bll.db.Database.Connection;
                    cmd.CommandText = insideQuery.TrimEnd(',');
                    adpt.SelectCommand = cmd;
                    adpt.Fill(insideDataTable);
                }
            }
            else if (InsideOrOutside == "Outside")
            {
                string insideQuery = $"EXEC Report_VehicleOutStayNonMoving_StayReport ";

                //ReportDateTime
                if (!string.IsNullOrEmpty(ReportDateTime))
                {
                    var _ReportDateTime = Convert.ToDateTime(ReportDateTime);
                    insideQuery = insideQuery + $"@ReportDateTime = '{ReportDateTime}',";
                }

                //OWN_MHT_DHTList
                if (OWN_MHT_DHT == null)
                {
                    //Do Nothing
                }
                else if (OWN_MHT_DHT != "all")
                {
                    insideQuery = insideQuery + $"@OWN_MHT_DHT = '{OWN_MHT_DHT}',";
                }

                //PRG_Type
                if (PRG_Type == null)
                {
                    //Do Nothing
                }
                else if (PRG_Type != "all")
                {
                    insideQuery = insideQuery + $"@PRG_Type = '{PRG_Type}',";
                }

                //FK_Depo
                if (FK_Depo == null)
                {
                }
                else if (FK_Depo == "all")
                {
                }
                else if (FK_Depo != "all")
                {
                    var _FK_Depo = Guid.Parse(FK_Depo);
                    insideQuery = insideQuery + $"@FK_Depo = '{FK_Depo}',";
                }

                //FK_Vehicles
                if (!string.IsNullOrEmpty(FK_Vehicles))
                {
                    insideQuery = insideQuery + $"@FK_Vehicles = '{FK_Vehicles}',";
                }

                //MinStayTimeHour
                if (!string.IsNullOrEmpty(MinStayTimeHour))
                {
                    insideQuery = insideQuery + $"@MinStayTimeHour = '{MinStayTimeHour}',";
                }
                //Group Category
                if (!string.IsNullOrEmpty(Depo_Category))
                {
                    insideQuery = insideQuery + $"@Depo_Category = '{Depo_Category}',";
                }

                //final
                if (!string.IsNullOrEmpty(ReportDateTime))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = int.MaxValue;
                    SqlDataAdapter adpt = new SqlDataAdapter();
                    cmd.Connection = (SqlConnection)bll.db.Database.Connection;
                    cmd.CommandText = insideQuery.TrimEnd(',');
                    adpt.SelectCommand = cmd;
                    adpt.Fill(outsideDataTable);
                }
            }
            return View(new Tuple<DataTable, DataTable>(insideDataTable, outsideDataTable));
        }
        public ActionResult HistoryReport(String InsideOrOutside, String StartingDateTime, String EndingDateTime, String OWN_MHT_DHT, String PRG_Type, String Depo_Category, String FK_Depo, String FK_Location, String FK_Vehicles, String FK_Destination, String MinStayTimeHour)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            var InsideOrOutsideList = new List<string>() { "Inside", "Outside" };
            ViewBag.InsideOrOutsideList = new SelectList(InsideOrOutsideList, InsideOrOutside);
            ViewBag.InsideOrOutside = InsideOrOutside;
            //#Common
            //StartingDateTime
            ViewBag.StartingDateTime = StartingDateTime;

            //EndingDateTime
            ViewBag.EndingDateTime = EndingDateTime;

            //OWN_MHT_DHTList
            List<SelectListItem> OWN_MHT_DHTList = new List<SelectListItem>();
            OWN_MHT_DHTList.Add(new SelectListItem() { Value = "all", Text = "All" });
            OWN_MHT_DHTList.AddRange(OWN_MHT_DHT_Dict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key, Text = m.Value }));
            ViewBag.OWN_MHT_DHT = new SelectList(OWN_MHT_DHTList.OrderBy(m => m.Text), "Value", "Text", OWN_MHT_DHT);

            //PRG_Type
            List<SelectListItem> PRG_TypeList = new List<SelectListItem>();
            PRG_TypeList.Add(new SelectListItem() { Value = "all", Text = "All" });
            PRG_TypeList.AddRange(PRG_TypesDict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key, Text = m.Value }));
            ViewBag.PRG_Type = new SelectList(PRG_TypeList.OrderBy(m => m.Text), "Value", "Text", PRG_Type);

            // #Depo_Category
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depo_Categories = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo)).Select(m => m.Category).Distinct(), Depo_Category);

            //FK_Location
            List<SelectListItem> LocationList = new List<SelectListItem>();
            LocationList.Add(new SelectListItem() { Value = "all", Text = "All" });
            LocationList.AddRange(bll.db.Locations.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Location.ToString(), Text = m.Name }));
            ViewBag.Locations = new SelectList(LocationList.OrderBy(m => m.Text), "Value", "Text", FK_Location);

            //FK_Depo
            if (FK_Depo == null)
            {
                //Do Nothing
                List<SelectListItem> DepoList = new List<SelectListItem>();
                DepoList.Add(new SelectListItem() { Value = "all", Text = "All" });
                ViewBag.DepoList = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text", FK_Depo);
            }
            else if (FK_Depo == "all")
            {
                //Do Nothing
                if (PRG_Type == null)
                {
                    List<SelectListItem> DepoList = new List<SelectListItem>();
                    DepoList.Add(new SelectListItem() { Value = "all", Text = "All" });
                    ViewBag.DepoList = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text", FK_Depo);
                }
                else if (PRG_Type != "all")
                {
                    List<SelectListItem> DepoList = new List<SelectListItem>();
                    DepoList.Add(new SelectListItem() { Value = "all", Text = "All" });
                    DepoList.AddRange(bll.db.Depoes.AsEnumerable().Where(m => m.IsDeleted == false && m.PRG_Type == PRG_Type).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Depo.ToString(), Text = m.Name }));
                    ViewBag.DepoList = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text", FK_Depo);
                }
                else // if (PRG_Type == "all")
                {
                    List<SelectListItem> DepoList = new List<SelectListItem>();
                    DepoList.Add(new SelectListItem() { Value = "all", Text = "All" });
                    ViewBag.DepoList = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text", FK_Depo);
                }
            }
            else if (FK_Depo != "all")
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                List<SelectListItem> DepoList = new List<SelectListItem>();
                DepoList.Add(new SelectListItem() { Value = "all", Text = "All" });
                DepoList.AddRange(bll.db.Depoes.AsEnumerable().Where(m => m.IsDeleted == false && m.PRG_Type == PRG_Type).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Depo.ToString(), Text = m.Name }));
                ViewBag.DepoList = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text", FK_Depo);
            }

            //FK_Vehicles
            if (!string.IsNullOrEmpty(FK_Vehicles))
            {
                var FK_VehicleArray = FK_Vehicles.Split(',').Select(m => Guid.Parse(m)).ToArray();
                ViewBag.Vehicles = new MultiSelectList(bll.db.Vehicles.Where(m => FK_VehicleArray.Contains(m.PK_Vehicle)), "PK_Vehicle", "RegistrationNumber", FK_VehicleArray);
            }
            else
            {
                ViewBag.Vehicles = new MultiSelectList(bll.db.Vehicles.Where(m => m.PK_Vehicle == null), "PK_Vehicle", "RegistrationNumber");
            }

            //FK_Destination
            List<SelectListItem> DestinaitonList = new List<SelectListItem>();
            DestinaitonList.Add(new SelectListItem() { Value = "all", Text = "All" });
            DestinaitonList.AddRange(bll.db.PRG_Type.Where(m => m.IsDeleted != true && m.Show_VehicleGateInOutManual == true).Select(m => new SelectListItem { Value = m.PK_PRG_Type.ToString(), Text = m.Title }));
            ViewBag.FK_Destination = new SelectList(DestinaitonList.OrderBy(m => m.Text), "Value", "Text", FK_Destination);


            //MinStayTimeHour
            if (!string.IsNullOrEmpty(MinStayTimeHour))
            {
                ViewBag.MinStayTimeHour = MinStayTimeHour;
            }
            else
            {
                ViewBag.MinStayTimeHour = "";
            }
            ViewBag.MinStayTimeHourDict = new SelectList(MinStayTimeHourDict, "Key", "Value", MinStayTimeHourDict);

            //#Inside
            DataTable insideDataTable = new DataTable();
            DataTable outsideDataTable = new DataTable();
            if (InsideOrOutside == "Inside")
            {
                string insideQuery = $"EXEC Report_VehicleInOutManual_HistoryReport ";

                //StartingDateTime
                if (!string.IsNullOrEmpty(StartingDateTime))
                {
                    var _StartingDateTime = Convert.ToDateTime(StartingDateTime);
                    insideQuery = insideQuery + $"@StartingDateTime = '{StartingDateTime}',";
                }

                //EndingDateTime
                if (!string.IsNullOrEmpty(EndingDateTime))
                {
                    var _EndingDateTime = Convert.ToDateTime(EndingDateTime);
                    insideQuery = insideQuery + $"@EndingDateTime = '{EndingDateTime}',";
                }

                //OWN_MHT_DHTList
                if (OWN_MHT_DHT == null)
                {
                    //Do Nothing
                }
                else if (OWN_MHT_DHT != "all")
                {
                    insideQuery = insideQuery + $"@OWN_MHT_DHT = '{OWN_MHT_DHT}',";
                }

                //PRG_Type
                if (PRG_Type == null)
                {
                    //Do Nothing
                }
                else if (PRG_Type != "all")
                {
                    insideQuery = insideQuery + $"@PRG_Type = '{PRG_Type}',";
                }

                //FK_Location
                if (FK_Location == null)
                {
                    //Do Nothing
                }
                else if (FK_Location != "all")
                {
                    var _FK_Location = Guid.Parse(FK_Location);
                    insideQuery = insideQuery + $"@FK_Location = '{FK_Location}',";
                }

                //FK_Depo
                if (FK_Depo == null)
                {
                }
                else if (FK_Depo == "all")
                {
                }
                else if (FK_Depo != "all")
                {
                    var _FK_Depo = Guid.Parse(FK_Depo);
                    insideQuery = insideQuery + $"@FK_Depo = '{FK_Depo}',";
                }

                //FK_Vehicles
                if (!string.IsNullOrEmpty(FK_Vehicles))
                {
                    insideQuery = insideQuery + $"@FK_Vehicles = '{FK_Vehicles}',";
                }

                //FK_Destination
                if (!string.IsNullOrEmpty(FK_Destination))
                {
                    insideQuery = insideQuery + $"@FK_Destination = '{FK_Destination}',";
                }

                //MinStayTimeHour
                if (!string.IsNullOrEmpty(MinStayTimeHour))
                {
                    insideQuery = insideQuery + $"@MinStayTimeHour = '{MinStayTimeHour}',";
                }

                //Group Category
                if (!string.IsNullOrEmpty(Depo_Category))
                {
                    insideQuery = insideQuery + $"@Depo_Category = '{Depo_Category}',";
                }

                //final
                if (!string.IsNullOrEmpty(StartingDateTime))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = int.MaxValue;
                    SqlDataAdapter adpt = new SqlDataAdapter();
                    cmd.Connection = (SqlConnection)bll.db.Database.Connection;
                    cmd.CommandText = insideQuery.TrimEnd(',');
                    adpt.SelectCommand = cmd;
                    adpt.Fill(insideDataTable);
                }
            }
            else if (InsideOrOutside == "Outside")
            {
                string insideQuery = $"EXEC Report_VehicleOutStayNonMoving_HistoryReport ";

                //StartingDateTime
                if (!string.IsNullOrEmpty(StartingDateTime))
                {
                    var _StartingDateTime = Convert.ToDateTime(StartingDateTime);
                    insideQuery = insideQuery + $"@StartingDateTime = '{StartingDateTime}',";
                }

                //EndingDateTime
                if (!string.IsNullOrEmpty(EndingDateTime))
                {
                    var _EndingDateTime = Convert.ToDateTime(EndingDateTime);
                    insideQuery = insideQuery + $"@EndingDateTime = '{EndingDateTime}',";
                }

                //OWN_MHT_DHTList
                if (OWN_MHT_DHT == null)
                {
                    //Do Nothing
                }
                else if (OWN_MHT_DHT != "all")
                {
                    insideQuery = insideQuery + $"@OWN_MHT_DHT = '{OWN_MHT_DHT}',";
                }

                //PRG_Type
                if (PRG_Type == null)
                {
                    //Do Nothing
                }
                else if (PRG_Type != "all")
                {
                    insideQuery = insideQuery + $"@PRG_Type = '{PRG_Type}',";
                }

                //FK_Depo
                if (FK_Depo == null)
                {
                }
                else if (FK_Depo == "all")
                {
                }
                else if (FK_Depo != "all")
                {
                    var _FK_Depo = Guid.Parse(FK_Depo);
                    insideQuery = insideQuery + $"@FK_Depo = '{FK_Depo}',";
                }

                //FK_Vehicles
                if (!string.IsNullOrEmpty(FK_Vehicles))
                {
                    insideQuery = insideQuery + $"@FK_Vehicles = '{FK_Vehicles}',";
                }

                //MinStayTimeHour
                if (!string.IsNullOrEmpty(MinStayTimeHour))
                {
                    insideQuery = insideQuery + $"@MinStayTimeHour = '{MinStayTimeHour}',";
                }

                //Group Category
                if (!string.IsNullOrEmpty(Depo_Category))
                {
                    insideQuery = insideQuery + $"@Depo_Category = '{Depo_Category}',";
                }

                //final
                if (!string.IsNullOrEmpty(StartingDateTime))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = int.MaxValue;
                    SqlDataAdapter adpt = new SqlDataAdapter();
                    cmd.Connection = (SqlConnection)bll.db.Database.Connection;
                    cmd.CommandText = insideQuery.TrimEnd(',');
                    adpt.SelectCommand = cmd;
                    adpt.Fill(outsideDataTable);
                }
            }
            return View(new Tuple<DataTable, DataTable>(insideDataTable, outsideDataTable));
        }


        public ActionResult OutTrip_Index(String FK_Depo, String RegistrationNumber, String GPNumber, string StartingDate, string EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<VehicleInOutManual> list = new List<VehicleInOutManual>();

            var query = bll.db.VehicleInOutManuals.AsQueryable().Where(m => m.InOrOut == false && m.GPNumber != null);

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.Vehicle.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //FK_Depo
            if (FK_Depo == null || FK_Depo == "null")
            {
                query.Where(m => m.Vehicle.FK_Depo == null);
            }
            else if (FK_Depo != "all")
            {
                var _FK_Depo = Guid.Parse(FK_Depo);

                query = query.Where(m => m.Vehicle.FK_Depo == _FK_Depo);
            }
            List<SelectListItem> DepoList = new List<SelectListItem>();
            DepoList.Add(new SelectListItem() { Value = "all", Text = "All" });
            DepoList.AddRange(bll.db.Depoes.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Depo.ToString(), Text = m.Name }));
            ViewBag.Depos = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text", FK_Depo);

            //StartingDate
            if (!string.IsNullOrEmpty(StartingDate))
            {
                var _StartingDate = Convert.ToDateTime(StartingDate);
                query = query.Where(m => m.Out_IssueDateTime >= _StartingDate);
            }
            ViewBag.StartingDate = StartingDate;

            //EndingDate
            if (!string.IsNullOrEmpty(EndingDate))
            {
                var _EndingDate = Convert.ToDateTime(EndingDate);
                query = query.Where(m => m.Out_IssueDateTime < _EndingDate);
            }
            ViewBag.EndingDate = EndingDate;

            //final
            if (FK_Depo != null || (!string.IsNullOrEmpty(RegistrationNumber)) || (!string.IsNullOrEmpty(GPNumber)) || StartingDate != null || EndingDate != null)
            {
                list = query.OrderBy(m => m.In_IssueDateTime).ToList();
            }

            return View(list);
        }

        public ActionResult TemporaryVehicleIndex(String FK_Location, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<TemporaryVehicle> list = new List<TemporaryVehicle>();

            var query = bll.db.TemporaryVehicles.AsEnumerable();

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //FK_Location
            if (FK_Location == null || FK_Location == "null")
            {
                query.Where(m => m.FK_Locaiton == null);
            }
            else if (FK_Location != "all")
            {
                var _FK_Location = Guid.Parse(FK_Location);

                query = query.Where(m => m.FK_Locaiton == _FK_Location);
            }
            List<SelectListItem> LocationList = new List<SelectListItem>();
            LocationList.Add(new SelectListItem() { Value = "all", Text = "All" });
            LocationList.AddRange(bll.db.Locations.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Location.ToString(), Text = m.Name }));
            ViewBag.Locations = new SelectList(LocationList.OrderBy(m => m.Text), "Value", "Text", FK_Location);

            //final
            if ((!string.IsNullOrEmpty(RegistrationNumber)) || FK_Location != null)
            {
                list = query.OrderBy(m => m.IssueDateTime).ToList();
            }

            return View(list);
        }
        public ActionResult Delete(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.TemporaryVehicles.Where(m => m.PK_TemporaryVehicle == id).FirstOrDefault();
            if (model != null)
            {
                bll.db.TemporaryVehicles.Remove(model);
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Temporary Vehicle is successfully deleted.");
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Temporary vehicle not found.");
            }
            return RedirectToAction("TemporaryVehicleIndex");
        }

        //# Services
        public string TrySendCurrentlyOverStayMail()
        {
            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendCurrentlyOverStayMail-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            try
            {
                var PRG_Type = "";
                int MaxStayTimeHour = 6;
                var Now = DateTime.Now;
                var minDataTime = Now.AddHours(-MaxStayTimeHour);

                //# PRAN-All
                PRG_Type = "PRAN";
                if (bll.db.Vehicles.Where(m => m.IsDeleted != true && m.LocationInOrOut == true && m.LocationInOutTime < minDataTime && m.Location != null && m.Location.LocationType != "Workshop" && m.VehicleInOutManual != null && m.VehicleInOutManual.FK_PRG_Type != 3)
                .Where(m => ((m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT") && m.Depo.PRG_Type == PRG_Type) || ((m.OWN_MHT_DHT == "DHT") && m.VehicleInOutManual.AppUser.PRG_Type == PRG_Type)).Any())
                {
                    var _email = "automation@mis.prangroup.com";
                    var _epass = "aaaaAAAA0000";

                    SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                    sc.EnableSsl = false;
                    sc.Credentials = new NetworkCredential(_email, _epass);
                    sc.Port = 25;

                    var Mail_Subject = "PRAN | Vehicle Inside Overstay Hourly Report (All) | " + DateTime.Now.ToString("h:mm tt");
                    var Mail_ToList = new List<string>() {
                        "piptpt8@pip.prangroup.com","dist100@prangroup.com","distdepotsch@prangroup.com","PRAN-Dist-DIC-AllRecipients@prangroup.com","PRAN-Dist-TPT-AllRecipients@prangroup.com"
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
                    url = url + @"VehicleGateNew/CurrentlyOverStayMailBodyGenerator_AllVehicles?PRG_Type=PRAN&MaxStayTimeHour=" + MaxStayTimeHour;
                    WebClient myWebClient = new WebClient();
                    byte[] myDataBuffer = myWebClient.DownloadData(url);
                    string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                    mail.Body = mailBody_HTML;
                    mail.IsBodyHtml = true;

                    //# xl
                    MemoryStream ms2 = new MemoryStream(Encoding.ASCII.GetBytes(RenderPartialViewToString_CurrentlyOverStayMailExcelGenerator_AllVehicles("CurrentlyOverStayMailExcelGenerator_AllVehicles", "PRAN", MaxStayTimeHour)));
                    mail.Attachments.Add(new Attachment(ms2, "Detail.xls", "application/vnd.ms-excel"));
                    sc.Send(mail);

                }

                //# PRAN-OWN
                if (bll.db.Vehicles.Where(m => m.IsDeleted != true && m.LocationInOrOut == true && m.LocationInOutTime < minDataTime && m.Location != null && m.Location.LocationType != "Workshop" && m.VehicleInOutManual != null && m.VehicleInOutManual.FK_PRG_Type != 3)
                .Where(m => ((m.OWN_MHT_DHT == "OWN") && m.Depo.PRG_Type == PRG_Type)).Any())
                {
                    var _email = "automation@mis.prangroup.com";
                    var _epass = "aaaaAAAA0000";

                    SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                    sc.EnableSsl = false;
                    sc.Credentials = new NetworkCredential(_email, _epass);
                    sc.Port = 25;

                    var Mail_Subject = "PRAN | Vehicle Inside Overstay Hourly Report (Own) | " + DateTime.Now.ToString("h:mm tt");
                    var Mail_ToList = new List<string>() {
                    "piptpt8@pip.prangroup.com","dist100@prangroup.com",
                    "pfgdist@prangroup.com","dist49@prangroup.com",
                    "dist44@prangroup.com", "dist46@prangroup.com",
                    "dist07@prangroup.com","dist80@prangroup.com",
                    "dist210@prangroup.com","dist322@prangroup.com",
                    "distdepotsch@prangroup.com",
                    "PRAN-Dist-TPT-AllRecipients@prangroup.com", "PRAN-Dist-DIC-AllRecipients@prangroup.com"
                    };
                    var Mail_CcList = new List<string>()
                    {
                        "automation17@mis.prangroup.com", "mis7@prangroup.com", "mis3@prangroup.com",
                    };
                    var Mail_BccList = new List<string>() {
                        "PRAN-Dist-MIS-AllRecipients@prangroup.com",
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
                    url = url + @"VehicleGateNew/CurrentlyOverStayMailBodyGenerator_OWNVehicles?PRG_Type=PRAN&MaxStayTimeHour=" + MaxStayTimeHour;
                    WebClient myWebClient = new WebClient();
                    byte[] myDataBuffer = myWebClient.DownloadData(url);
                    string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                    mail.Body = mailBody_HTML;
                    mail.IsBodyHtml = true;

                    //# xl
                    MemoryStream ms2 = new MemoryStream(Encoding.ASCII.GetBytes(RenderPartialViewToString_CurrentlyOverStayMailExcelGenerator_OWNVehicles("CurrentlyOverStayMailExcelGenerator_OWNVehicles", "PRAN", MaxStayTimeHour)));
                    mail.Attachments.Add(new Attachment(ms2, "Detail.xls", "application/vnd.ms-excel"));
                    sc.Send(mail);
                }

                //# RFL-All
                PRG_Type = "RFL";
                if (bll.db.Vehicles.Where(m => m.IsDeleted != true && m.LocationInOrOut == true && m.LocationInOutTime < minDataTime && m.Location != null && m.Location.LocationType != "Workshop" && m.VehicleInOutManual != null && m.VehicleInOutManual.FK_PRG_Type != 3)
                .Where(m => ((m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT") && m.Depo.PRG_Type == PRG_Type) || ((m.OWN_MHT_DHT == "DHT") && m.VehicleInOutManual.AppUser.PRG_Type == PRG_Type)).Any())
                {
                    var _email = "automation@mis.prangroup.com";
                    var _epass = "aaaaAAAA0000";

                    SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                    sc.EnableSsl = false;
                    sc.Credentials = new NetworkCredential(_email, _epass);
                    sc.Port = 25;

                    var Mail_Subject = "RFL | Vehicle Inside Overstay Hourly Report (All) | " + DateTime.Now.ToString("h:mm tt");
                    var Mail_ToList = new List<string>() {
                        "dist121@rflgroupbd.com",
                        "RFLDist2DepotAllTPT@rflgroupbd.com", "RFLDist2DepotAllDIC@rflgroupbd.com","RFLDist2DepotAllMIS@rflgroupbd.com",
                       "RFLDistCoreTeam@rflgroupbd.com", "dist121@rflgroupbd.com", "dist370@rflgroupbd.com", "dist371@rflgroupbd.com", "towfique@rflgroupbd.com"
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
                    url = url + @"VehicleGateNew/CurrentlyOverStayMailBodyGenerator_AllVehicles?PRG_Type=RFL&MaxStayTimeHour=" + MaxStayTimeHour;
                    WebClient myWebClient = new WebClient();
                    byte[] myDataBuffer = myWebClient.DownloadData(url);
                    string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                    mail.Body = mailBody_HTML;
                    mail.IsBodyHtml = true;

                    //# xl
                    MemoryStream ms2 = new MemoryStream(Encoding.ASCII.GetBytes(RenderPartialViewToString_CurrentlyOverStayMailExcelGenerator_AllVehicles("CurrentlyOverStayMailExcelGenerator_AllVehicles", "RFL", MaxStayTimeHour)));
                    mail.Attachments.Add(new Attachment(ms2, "Detail.xls", "application/vnd.ms-excel"));
                    sc.Send(mail);
                }

                //# RFL-OWN
                if (bll.db.Vehicles.Where(m => m.IsDeleted != true && m.LocationInOrOut == true && m.LocationInOutTime < minDataTime && m.Location != null && m.Location.LocationType != "Workshop" && m.VehicleInOutManual != null && m.VehicleInOutManual.FK_PRG_Type != 3)
                .Where(m => ((m.OWN_MHT_DHT == "OWN") && m.Depo.PRG_Type == PRG_Type)).Any())
                {
                    var _email = "automation@mis.prangroup.com";
                    var _epass = "aaaaAAAA0000";

                    SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                    sc.EnableSsl = false;
                    sc.Credentials = new NetworkCredential(_email, _epass);
                    sc.Port = 25;

                    var Mail_Subject = "RFL | Vehicle Inside Overstay Hourly Report (Own) | " + DateTime.Now.ToString("h:mm tt");
                    var Mail_ToList = new List<string>() {
                        "mis7@prangroup.com", "automation17@mis.prangroup.com", "dist121@rflgroupbd.com",
                        "RFLDist2DepotAllTPT@rflgroupbd.com", "RFLDist2DepotAllDIC@rflgroupbd.com","RFLDist2DepotAllMIS@rflgroupbd.com",
                        "RFLDistCoreTeam@rflgroupbd.com", "dist121@rflgroupbd.com", "dist370@rflgroupbd.com", "dist371@rflgroupbd.com", "towfique@rflgroupbd.com"

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
                    url = url + @"VehicleGateNew/CurrentlyOverStayMailBodyGenerator_OWNVehicles?PRG_Type=RFL&MaxStayTimeHour=" + MaxStayTimeHour;
                    WebClient myWebClient = new WebClient();
                    byte[] myDataBuffer = myWebClient.DownloadData(url);
                    string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                    mail.Body = mailBody_HTML;
                    mail.IsBodyHtml = true;

                    //# xl
                    MemoryStream ms2 = new MemoryStream(Encoding.ASCII.GetBytes(RenderPartialViewToString_CurrentlyOverStayMailExcelGenerator_OWNVehicles("CurrentlyOverStayMailExcelGenerator_OWNVehicles", "RFL", MaxStayTimeHour)));
                    mail.Attachments.Add(new Attachment(ms2, "Detail.xls", "application/vnd.ms-excel"));
                    sc.Send(mail);
                }

            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);

                bll.db.ServiceCalls.Add(
                    new ServiceCall()
                    {
                        CallingTime = DateTime.Now,
                        CallingMessage = "TrySendCurrentlyOverStayMail-Error" + guid,
                        UserDefinedMessage = errrorMessage
                    }
                 );
                bll.db.SaveChanges();
            }

            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendCurrentlyOverStayMail-End" + guid,
                      CallingTime = DateTime.Now,
                  }
                  );
            bll.db.SaveChanges();
            return "Done";
        }
        public ActionResult CurrentlyOverStayMailBodyGenerator_AllVehicles(string PRG_Type, int MaxStayTimeHour)
        {
            ViewBag.PRG_Type = PRG_Type;

            //int MaxStayTimeHour = 6;
            ViewBag.MaxStayTimeHour = MaxStayTimeHour;

            var Now = DateTime.Now;
            ViewBag.Now = Now;

            var minDataTime = Now.AddHours(-MaxStayTimeHour);


            List<Vehicle> VehicleList = new List<Vehicle>();
            if (!string.IsNullOrEmpty(PRG_Type))
            {
                VehicleList = bll.db.Vehicles.Where(m => m.IsDeleted != true && m.LocationInOrOut == true && m.LocationInOutTime < minDataTime && m.Location != null && m.Location.LocationType != "Workshop" && m.VehicleInOutManual != null && m.VehicleInOutManual.FK_PRG_Type != 3)
                .Where(m => ((m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT") && m.Depo.PRG_Type == PRG_Type) || ((m.OWN_MHT_DHT == "DHT") && m.VehicleInOutManual.AppUser.PRG_Type == PRG_Type)).ToList();
            }
            return View(VehicleList);
        }
        public string RenderPartialViewToString_CurrentlyOverStayMailExcelGenerator_AllVehicles(string viewName, string PRG_Type, int MaxStayTimeHour)
        {
            var Now = DateTime.Now;
            var minDataTime = Now.AddHours(-MaxStayTimeHour);
            List<Vehicle> VehicleList = new List<Vehicle>();
            if (!string.IsNullOrEmpty(PRG_Type))
            {
                VehicleList = bll.db.Vehicles.Where(m => m.IsDeleted != true && m.LocationInOrOut == true && m.LocationInOutTime < minDataTime && m.Location != null && m.Location.LocationType != "Workshop" && m.VehicleInOutManual != null && m.VehicleInOutManual.FK_PRG_Type != 3)
                .Where(m => ((m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT") && m.Depo.PRG_Type == PRG_Type) || ((m.OWN_MHT_DHT == "DHT") && m.VehicleInOutManual.AppUser.PRG_Type == PRG_Type)).ToList();
            }
            ViewBag.VehicleList = VehicleList;

            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                var res = sw.GetStringBuilder().ToString();
                return res;
            }
        }
        public ActionResult CurrentlyOverStayMailExcelGenerator_AllVehicles()
        {
            //ViewBag.VehicleList = VehicleList;
            return View();
        }

        public ActionResult CurrentlyOverStayMailBodyGenerator_OWNVehicles(string PRG_Type, int MaxStayTimeHour)
        {
            ViewBag.PRG_Type = PRG_Type;

            //int MaxStayTimeHour = 6;
            ViewBag.MaxStayTimeHour = MaxStayTimeHour;

            var Now = DateTime.Now;
            ViewBag.Now = Now;

            var minDataTime = Now.AddHours(-MaxStayTimeHour);

            List<Vehicle> VehicleList = new List<Vehicle>();
            if (!string.IsNullOrEmpty(PRG_Type))
            {
                VehicleList = bll.db.Vehicles.Where(m => m.IsDeleted != true && m.LocationInOrOut == true && m.LocationInOutTime < minDataTime && m.Location != null && m.Location.LocationType != "Workshop" && m.VehicleInOutManual != null && m.VehicleInOutManual.FK_PRG_Type != 3)
                .Where(m => ((m.OWN_MHT_DHT == "OWN") && m.Depo.PRG_Type == PRG_Type)).ToList();
            }
            return View(VehicleList);
        }
        public string RenderPartialViewToString_CurrentlyOverStayMailExcelGenerator_OWNVehicles(string viewName, string PRG_Type, int MaxStayTimeHour)
        {
            var Now = DateTime.Now;
            var minDataTime = Now.AddHours(-MaxStayTimeHour);
            List<Vehicle> VehicleList = new List<Vehicle>();
            if (!string.IsNullOrEmpty(PRG_Type))
            {
                VehicleList = bll.db.Vehicles.Where(m => m.IsDeleted != true && m.LocationInOrOut == true && m.LocationInOutTime < minDataTime && m.Location != null && m.Location.LocationType != "Workshop" && m.VehicleInOutManual != null && m.VehicleInOutManual.FK_PRG_Type != 3)
                .Where(m => ((m.OWN_MHT_DHT == "OWN") && m.Depo.PRG_Type == PRG_Type)).ToList();
            }
            ViewBag.VehicleList = VehicleList;

            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                var res = sw.GetStringBuilder().ToString();
                return res;
            }
        }
        public ActionResult CurrentlyOverStayMailExcelGenerator_OWNVehicles()
        {
            //ViewBag.VehicleList = VehicleList;
            return View();
        }

        public ActionResult GateInOutDashboard_NonParking()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<VehicleInOutManual>();
            var query = bll.db.VehicleInOutManuals.Where(m => m.InOrOut == true && m.In_FK_CreatedByUser == CurrentUser.PK_User).AsEnumerable();
            list = query.OrderByDescending(m => m.PK_VehicleInOutManual).ToList();
            return View(list);
        }

        public string TrySendDailyStayTimeMail()
        {
            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendCurrentlyOverStayMail-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            try
            {
                //# PRAN
                {
                    var _email = "automation@mis.prangroup.com";
                    var _epass = "aaaaAAAA0000";

                    SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                    sc.EnableSsl = false;
                    sc.Credentials = new NetworkCredential(_email, _epass);
                    sc.Port = 25;

                    var Mail_Subject = "PRAN | Vehicle Inside Stay Daily Report";
                    var Mail_ToList = new List<string>() {
                        "piptpt8@pip.prangroup.com", "dist100@prangroup.com",
                        "ed.dist@prangroup.com", "pfgdist@prangroup.com",
                        "tpt3@prangroup.com",
                        "pipmis@pip.rflgroupbd.com","mis8@prangroup.com",
                        "distdepotsch@prangroup.com",
                        "PRAN-Dist-TPT-AllRecipients@prangroup.com", "PRAN-Dist-DIC-AllRecipients@prangroup.com"
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
                    url = url + @"VehicleGateNew/DailyStayTimeMailBodyGenerator?PRG_Type=PRAN";
                    WebClient myWebClient = new WebClient();
                    byte[] myDataBuffer = myWebClient.DownloadData(url);
                    string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                    mail.Body = mailBody_HTML;
                    mail.IsBodyHtml = true;

                    //# xl
                    MemoryStream ms2 = new MemoryStream(Encoding.ASCII.GetBytes(RenderPartialViewToString_DailyStayTimeMailExcelGenerator("DailyStayTimeMailExcelGenerator", "PRAN")));
                    mail.Attachments.Add(new Attachment(ms2, "Detail.xls", "application/vnd.ms-excel"));
                    sc.Send(mail);
                }

                //# RFL
                {
                    var _email = "automation@mis.prangroup.com";
                    var _epass = "aaaaAAAA0000";

                    SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                    sc.EnableSsl = false;
                    sc.Credentials = new NetworkCredential(_email, _epass);
                    sc.Port = 25;

                    var Mail_Subject = "RFL | Vehicle Inside Stay Daily Report";
                    var Mail_ToList = new List<string>() {
                    "RFLDist2DepotAllTPT@rflgroupbd.com", "RFLDist2DepotAllDIC@rflgroupbd.com",
                    "RFLDistCoreTeam@rflgroupbd.com", "dist121@rflgroupbd.com",
                    "dist370@rflgroupbd.com", "dist371@rflgroupbd.com",
                    "towfique@rflgroupbd.com",
                    "pipmis@pip.rflgroupbd.com","mis8@prangroup.com",
                    "pipelec@pip.rflgroupbd.com","rflwatertank3@pip.rflgroupbd.com",
                    "rplprod20@pip.rflgroupbd.com","pipdplagm@pip.rflgroupbd.com",
                    "rplengr7@pip.rflgroupbd.com","rplprod38@pip.rflgroupbd.com",
                    "admin3@pip.rflgroupbd.com","admin24@pip.rflgroupbd.com",
                    "rplpvc@pip.rflgroupbd.com","DistDepotFactoryDICAll@rflgroupbd.com",
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
                    url = url + @"VehicleGateNew/DailyStayTimeMailBodyGenerator?PRG_Type=RFL";
                    WebClient myWebClient = new WebClient();
                    byte[] myDataBuffer = myWebClient.DownloadData(url);
                    string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                    mail.Body = mailBody_HTML;
                    mail.IsBodyHtml = true;

                    //# xl
                    MemoryStream ms2 = new MemoryStream(Encoding.ASCII.GetBytes(RenderPartialViewToString_DailyStayTimeMailExcelGenerator("DailyStayTimeMailExcelGenerator", "RFL")));
                    mail.Attachments.Add(new Attachment(ms2, "Detail.xls", "application/vnd.ms-excel"));
                    sc.Send(mail);
                }

            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);

                bll.db.ServiceCalls.Add(
                    new ServiceCall()
                    {
                        CallingTime = DateTime.Now,
                        CallingMessage = "TrySendCurrentlyOverStayMail-Error" + guid,
                        UserDefinedMessage = errrorMessage
                    }
                 );
                bll.db.SaveChanges();
            }

            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendCurrentlyOverStayMail-End" + guid,
                      CallingTime = DateTime.Now,
                  }
                  );
            bll.db.SaveChanges();
            return "Done";
        }
        public ActionResult DailyStayTimeMailBodyGenerator(string PRG_Type)
        {
            var Now = DateTime.Now.Date;
            ViewBag.Now = Now;

            List<VehicleInOutManual> List = new List<VehicleInOutManual>();

            var query = bll.db.VehicleInOutManuals.Where(m => m.Location.LocationType != "Workshop" && m.FK_PRG_Type != 3).AsQueryable();

            //PRG_Type
            query = query.Where(m => ((m.Vehicle.OWN_MHT_DHT == "OWN" || m.Vehicle.OWN_MHT_DHT == "MHT") && m.Vehicle.Depo.PRG_Type == PRG_Type) || (m.Vehicle.OWN_MHT_DHT == "DHT" && m.AppUser.PRG_Type == PRG_Type));

            //StartingDate
            var _StartingDate = DateTime.Now.AddDays(-1).Date;
            query = query.Where(m => (m.In_IssueDateTime >= _StartingDate) || (m.In_IssueDateTime < _StartingDate && m.Out_IssueDateTime == null));

            //EndingDate
            var _EndingDate = DateTime.Now.Date;
            query = query.Where(m => m.In_IssueDateTime < _EndingDate);

            //final
            List = query.OrderBy(m => m.In_IssueDateTime).ToList();

            var _list1 = List.Where(m => m.Vehicle.OWN_MHT_DHT == "OWN").ToList();

            foreach (var item in List.Where(m => m.Out_IssueDateTime == null || m.Out_IssueDateTime > _EndingDate))
            {
                item.InStayTimeMinute = (long)(Now - item.In_IssueDateTime).TotalMinutes;
                if (item.InStayTimeMinute < 0)
                {
                    item.InStayTimeMinute = 0;
                }
            }
            return View(List);
        }
        public string RenderPartialViewToString_DailyStayTimeMailExcelGenerator(string viewName, string PRG_Type)
        {

            var Now = DateTime.Now.Date;

            List<VehicleInOutManual> List = new List<VehicleInOutManual>();

            var query = bll.db.VehicleInOutManuals.Where(m => m.Location.LocationType != "Workshop" && m.FK_PRG_Type != 3).AsQueryable();

            //PRG_Type
            query = query.Where(m => ((m.Vehicle.OWN_MHT_DHT == "OWN" || m.Vehicle.OWN_MHT_DHT == "MHT") && m.Vehicle.Depo.PRG_Type == PRG_Type) || (m.Vehicle.OWN_MHT_DHT == "DHT" && m.AppUser.PRG_Type == PRG_Type));

            //StartingDate
            var _StartingDate = DateTime.Now.AddDays(-1).Date;
            query = query.Where(m => (m.In_IssueDateTime >= _StartingDate) || (m.In_IssueDateTime < _StartingDate && m.Out_IssueDateTime == null));

            //EndingDate
            var _EndingDate = DateTime.Now.Date;
            query = query.Where(m => m.In_IssueDateTime < _EndingDate);

            //final
            List = query.OrderBy(m => m.In_IssueDateTime).ToList();
            foreach (var item in List.Where(m => m.Out_IssueDateTime == null || m.Out_IssueDateTime > _EndingDate))
            {
                item.InStayTimeMinute = (long)(Now - item.In_IssueDateTime).TotalMinutes;
                if (item.InStayTimeMinute < 0)
                {
                    item.InStayTimeMinute = 0;
                }
            }
            ViewBag.List = List;

            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                var res = sw.GetStringBuilder().ToString();
                return res;
            }
        }
        public ActionResult DailyStayTimeMailExcelGenerator()
        {
            //var Now = DateTime.Now.Date.;

            //List<VehicleInOutManual> List = new List<VehicleInOutManual>();

            //var query = bll.db.VehicleInOutManuals.AsQueryable();

            ////PRG_Type
            //query = query.Where(m => m.AppUser.PRG_Type == "PRAN");

            ////StartingDate
            //var _StartingDate = DateTime.Now.AddDays(-1).Date.;
            //query = query.Where(m => (m.In_IssueDateTime >= _StartingDate)  || (m.In_IssueDateTime < _StartingDate && m.Out_IssueDateTime == null));

            ////EndingDate
            //var _EndingDate = DateTime.Now.Date.;
            //query = query.Where(m => m.In_IssueDateTime < _EndingDate);

            ////final
            //List = query.OrderBy(m => m.In_IssueDateTime).ToList();
            //foreach (var item in list.Where(m => m.Out_IssueDateTime == null || m.Out_IssueDateTime > endingDateLimit))
            //{
            //    item.InStayTimeMinute = (long)(Now - item.In_IssueDateTime).TotalMinutes;
            //    if (item.InStayTimeMinute < 0)
            //    {
            //        item.InStayTimeMinute = 0;
            //    }
            //}
            //ViewBag.List = List;

            return View();
        }

        public string TrySych_VehicleGateInOut_With_SCM_OracleDB()
        {
            var guid = Guid.NewGuid();
            var now = DateTime.Now;
            var sqlCommandList = new List<string>();

            //# PRAN Oracle Push:Start
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySych_VehicleGateInOut_With_SCM_OracleDB-Push-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            try
            {
                //# get list
                var finishing_limit = DateTime.Now;//# to cancel trip for wrong entry
                var starting_limit = DateTime.Now.AddHours(-6);
                var this_list = bll.db.VehicleInOutManuals.Where(m =>
                m.Location.SCM_LocationId != null
                && m.In_IssueDateTime > starting_limit
                && m.In_IssueDateTime < finishing_limit
                && m.SCM_Database_Status == null
                ).Select(m => m).ToList();

                foreach (var item in this_list)
                {
                    /*
                    insert into vehicle_info_3di (gate_inout_id,vehicle_id,vehicle_reg_number,location_id,location_name,in_time) values('1816730','5b493167-ebcc-4cdb-9f04-e285d4474d22','DHAKA METRO-U-12-1766','ace5aa77-92d1-4e3c-aa18-37a7c534e091','PIP',to_date('2021-10-18 12:43:38', 'yyyy-mm-dd hh24:mi:ss'))
                     */
                    var sqlCommand = "insert into vehicle_info_3di (gate_inout_id,vehicle_id,vehicle_reg_number,location_id,location_name,in_time) values(" +
                                    "'" + item.PK_VehicleInOutManual + "'," +
                                    "'" + item.FK_Vehicle + "'," +
                                    "'" + item.Vehicle.RegistrationNumber + "'," +
                                    "'" + item.Location.SCM_LocationId + "'," +
                                    "'" + item.Location.Name + "'," +
                                    "to_date('" + ((DateTime)item.In_IssueDateTime).ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')" +
                                    ")";
                    sqlCommandList.Add(sqlCommand);
                }

                var res = OracleGateInOut_DBHelper.DbSaveChanges(sqlCommandList);
                if (res == true)
                {
                    foreach (var item in this_list)
                    {
                        item.SCM_Database_Status = "pushed-1";
                    }
                    bll.db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                      new AppErrorLog()
                      {
                          ErrorMessage = e.Message,
                          ErrorTime = DateTime.Now,
                          UserDefinedMessage = "VehicleGateInOutNewTrySych_VehicleGateInOut_With_SCM_OracleDB-Push"
                      }
                    );
                bll.db.SaveChanges();
            }
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySych_VehicleGateInOut_With_SCM_OracleDB-Push-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            //# PRAN Oracle Push:End


            //# PRAN Oracle Pull:Start
            //bll.db.ServiceCalls.Add(
            //      new ServiceCall()
            //      {
            //          CallingMessage = "TrySych_VehicleGateInOut_With_SCM_OracleDB-Pull-Start-" + guid,
            //          CallingTime = DateTime.Now,
            //          UserDefinedMessage = ""
            //      }
            //      );
            //bll.db.SaveChanges();
            //try
            //{
            //    var query = "SELECT * FROM T_REQE_PRAN where REQE_DAT >= to_date('" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss") + "', 'YYYY-MM-DD HH24:MI:SS') and REQE_GPNO is not NULL and IS_PULLED = 'N'";
            //    DataTable res = OracleDBHelper.ExecuteSelectCommand(query);
            //    foreach (DataRow row in res.Rows)
            //    {
            //        var REQE_ID = Convert.ToInt64(row["REQE_ID"].ToString());
            //        var requisitionTrip = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == REQE_ID).FirstOrDefault();
            //        if (requisitionTrip != null)
            //        {
            //            var REQE_GPNO = row["REQE_GPNO"].ToString();
            //            if (!string.IsNullOrEmpty(REQE_GPNO))
            //            {
            //                requisitionTrip.OracleDB_GPNumber = REQE_GPNO;
            //            }
            //            var REQE_UPDAT = row["REQE_UPDAT"].ToString();
            //            if (!string.IsNullOrEmpty(REQE_UPDAT))
            //            {
            //                requisitionTrip.OracleDB_GPNumberUpdatedAt = Convert.ToDateTime(REQE_UPDAT);
            //            }
            //            var sqlCommand = "update T_REQE_PRAN set IS_PULLED = 'Y' where REQE_ID = '" + REQE_ID + "'";
            //            sqlCommandList.Add(sqlCommand);
            //        }
            //    }
            //    bll.db.SaveChanges();
            //    OracleDBHelper.DbSaveChanges(sqlCommandList);
            //}
            //catch (Exception e)
            //{
            //    bll.db.AppErrorLogs.Add(
            //          new AppErrorLog()
            //          {
            //              ErrorMessage = e.Message,
            //              ErrorTime = DateTime.Now,
            //              UserDefinedMessage = "VehicleGateInOutNewTrySych_VehicleGateInOut_With_SCM_OracleDB-Pull"
            //          }
            //        );
            //    bll.db.SaveChanges();
            //}

            //bll.db.ServiceCalls.Add(
            //      new ServiceCall()
            //      {
            //          CallingMessage = "TrySych_VehicleGateInOut_With_SCM_OracleDB-Pull-End-" + guid,
            //          CallingTime = DateTime.Now,
            //          UserDefinedMessage = ""
            //      }
            //      );
            //bll.db.SaveChanges();
            //# PRAN Oracle Pull:End
            return "Done";
        }

        //# BI Data sending
        public JsonResult VehicleInOutManual_GetDataForBI(string ReportDate)
        {
            List<VehicleInOutManual> list = new List<VehicleInOutManual>();

            var _StartingLimit = Convert.ToDateTime(ReportDate);
            var _EndingLimit = _StartingLimit.AddDays(1);
            var query = bll.db.VehicleInOutManuals.AsQueryable();
            query = query.Where(m =>
                (_StartingLimit < m.In_IssueDateTime && m.Out_IssueDateTime < _EndingLimit) || // [↓↑]
                (m.In_IssueDateTime < _StartingLimit && _StartingLimit < m.Out_IssueDateTime) || // ↓[↑
                (m.In_IssueDateTime < _EndingLimit && (_EndingLimit < m.Out_IssueDateTime || m.Out_IssueDateTime == null)) || // [↓]↑*
                (m.In_IssueDateTime < _StartingLimit && (_EndingLimit < m.Out_IssueDateTime || m.Out_IssueDateTime == null))// ↓[]↑*
                );
            list = query.ToList();

            var list_still_inside = list.Where(m => m.Out_IssueDateTime == null || m.Out_IssueDateTime > _EndingLimit).ToList();

            //final
            foreach (var item in list_still_inside)
            {
                item.InStayTimeMinute = (long)(_EndingLimit - item.In_IssueDateTime).TotalMinutes;
                if (item.InStayTimeMinute < 0)
                {
                    item.InStayTimeMinute = 0;
                }
            }

            var _list = list.Select(item => new BI_VehicleInOutManual
            {
                VehicleID = item.Vehicle.PK_Vehicle,
                Vehicle = item.Vehicle.RegistrationNumber,
                OWN_MHT_DHT = item.Vehicle.OWN_MHT_DHT,
                PRG_Type_Owner = item.Vehicle.OWN_MHT_DHT != "DHT" ? item.Vehicle.Depo.PRG_Type : item.AppUser.PRG_Type,
                Location = item.Location.Name,
                LocationId = item.Location.PK_Location,
                Destinaition = item.PRG_Type?.Title,
                User_Depot = item.Vehicle.Depo.Name,
                InGate = item.AppUser.FullName,
                InGate_PRG_Type = item.AppUser.PRG_Type,
                InGate_Type = item.AppUser.AppUserType.Contains("Parking") ? "Parking Gate" : "Nonparking Gate",
                InTime = item.In_IssueDateTime.ToString(),
                InStatus = item.In_LoadOrEmpty,
                InReasson = item.VehicleInOutManualReason.TitleBangla,
                OutGate = item.Out_FK_CreatedByUser != null ? item.AppUser1.FullName : "",
                OutTime = item.Out_IssueDateTime != null ? item.Out_IssueDateTime.ToString() : "",
                OutStatus = item.Out_LoadOrEmpty ?? "",
                OutReasson = item.Out_FK_VehicleInOutManualReason != null ? item.VehicleInOutManualReason1.TitleBangla : "",
                InStayTimeMinute = item.InStayTimeMinute,
                In_IssueDateTime = item.In_IssueDateTime,
                Out_IssueDateTime = item.Out_IssueDateTime
            }).ToList();

            foreach (var item in _list)
            {
                item.VehicleTypeId = item.OWN_MHT_DHT == "OWN" ? 1 : item.OWN_MHT_DHT == "MHT" ? 2 : 3;
                item.CompanyId = item.PRG_Type_Owner == "PRAN" ? 1 :
                    item.PRG_Type_Owner == "RFL" ? 2 :
                    item.PRG_Type_Owner == "CS" ? 3 :
                    item.PRG_Type_Owner == "N / A" ? 4 : 5;
                var _In_IssueDateTime = new DateTime();
                var _Out_IssueDateTime = new DateTime();
                if (item.In_IssueDateTime < _StartingLimit)
                {
                    _In_IssueDateTime = item.In_IssueDateTime;
                }
                else
                {
                    _In_IssueDateTime = _StartingLimit;
                }
                if (item.Out_IssueDateTime == null || item.Out_IssueDateTime > _EndingLimit)
                {
                    _Out_IssueDateTime = _EndingLimit;
                }
                else
                {
                    _Out_IssueDateTime = item.Out_IssueDateTime ?? new DateTime();
                }
                item.InStayTimeMinuteDaily = (long)(_Out_IssueDateTime - _In_IssueDateTime).TotalMinutes;
                if (item.InStayTimeMinuteDaily < 0)
                {
                    item.InStayTimeMinuteDaily = 0;
                }
            }

            JsonResult res = Json(
                _list
            , JsonRequestBehavior.AllowGet);
            res.MaxJsonLength = int.MaxValue;
            return res;
        }

    }

    internal class BI_VehicleInOutManual
    {
        public Guid VehicleID { get; set; }
        public string Vehicle { get; set; }
        public string OWN_MHT_DHT { get; set; }
        public int VehicleTypeId { get; set; }
        public string PRG_Type_Owner { get; set; }
        public string PRG_Type_App { get; set; }
        public int CompanyId { get; set; } //from PRG_Type_Owner
        public int CompanyId_App { get; set; }
        public string Location { get; set; }
        public Guid LocationId { get; set; }
        public string Destinaition { get; set; }
        public string User_Depot { get; set; }
        public string InGate { get; set; }
        public string InGate_Type { get; set; }
        public string InGate_PRG_Type { get; set; }
        public string InTime { get; set; }
        public string InStatus { get; set; }
        public string InReasson { get; set; }
        public string OutGate { get; set; }
        public string OutTime { get; set; }
        public string OutStatus { get; set; }
        public string OutReasson { get; set; }
        public long? InStayTimeMinute { get; set; }
        public DateTime In_IssueDateTime { get; set; }
        public DateTime? Out_IssueDateTime { get; set; }
        public long? InStayTimeMinuteDaily { get; set; }
    }
}