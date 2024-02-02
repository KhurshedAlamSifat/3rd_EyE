using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.Models;
using _3rdEyE.ViewModels;
using _3rdEyE.BLL;
using _3rdEyE.ManagingTools;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Net.Mail;
using Microsoft.Reporting.WebForms;
using System.Configuration;

namespace _3rdEyE.Controllers
{
    public class EventController : BaseController
    {
        static class EventStatus
        {
            public const string Created = "Created";
            public const string Issued = "Issued";
        }
        //AppUser CurrentUser = CommonClass.GetCurrentUser();
        Dictionary<string, string> OtherEventTypeDetailDict = new Dictionary<string, string> { { "Registration", "Registration" }, { "OwnershipTransfer", "Ownership Transfer" } };
        BLL_Event bll = new BLL_Event();

        public ActionResult EventDashBoard()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }
        public JsonResult GetEventDashBoardData()
        {
            object LastMonth_fitness_paper_TotalExpense;
            object LastMonth_insurance_TotalExpense;
            object LastMonth_route_permit_TotalExpense;
            object LastMonth_tax_token_TotalExpense;
            /*
            Delete  from ReadyReport where ReportName = 'LastMonthEventTotalExpense';
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1)
            select 'Event/EventDashBoard','LastMonthEventTotalExpense', Depo.PRG_Type, EventType.PK_EventType,sum(Event.TotalAmount)
            from Event 
            join EventType on EventType.PK_EventType = Event.FK_EventType
            join Vehicle on Vehicle.PK_Vehicle = Event.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            where  1=1
            and Event.CreatedAt >= '2020-10-01' and Event.CreatedAt < '2020-11-01'
            group by Depo.PRG_Type,EventType.PK_EventType
            having sum(Event.TotalAmount) > 0;
             */

            object LastYear_fitness_paper_TotalExpense;
            object LastYear_insurance_TotalExpense;
            object LastYear_route_permit_TotalExpense;
            object LastYear_tax_token_TotalExpense;
            /*
            Delete  from ReadyReport where ReportName = 'LastYearEventTotalExpense';
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1)
            select 'Event/EventDashBoard','LastYearEventTotalExpense', Depo.PRG_Type, EventType.PK_EventType,sum(Event.TotalAmount)
            from Event 
            join EventType on EventType.PK_EventType = Event.FK_EventType
            join Vehicle on Vehicle.PK_Vehicle = Event.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            where  1=1
            and Event.CreatedAt >= '2020-01-01' and Event.CreatedAt < '2021-01-01'
            group by Depo.PRG_Type,EventType.PK_EventType
            having sum(Event.TotalAmount) > 0;
             */

            object LastMonth_fitness_paper_TotalFineAmount;
            object LastMonth_insurance_TotalFineAmount;
            object LastMonth_route_permit_TotalFineAmount;
            object LastMonth_tax_token_TotalFineAmount;
            /*
            Delete  from ReadyReport where ReportName = 'LastMonthEventTotalFineAmount';
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1)
            select 'Event/EventDashBoard','LastMonthEventTotalFineAmount', Depo.PRG_Type, EventType.PK_EventType,sum(Event.FineAmount)
            from Event 
            join EventType on EventType.PK_EventType = Event.FK_EventType
            join Vehicle on Vehicle.PK_Vehicle = Event.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            where  1=1
            and Event.CreatedAt >= '2020-10-01' and Event.CreatedAt < '2020-11-01'
            group by Depo.PRG_Type,EventType.PK_EventType
            having sum(Event.FineAmount) > 0;
             */

            object LastYear_fitness_paper_TotalFineAmount;
            object LastYear_insurance_TotalFineAmount;
            object LastYear_route_permit_TotalFineAmount;
            object LastYear_tax_token_TotalFineAmount;
            /*
            Delete  from ReadyReport where ReportName = 'LastYearEventTotalFineAmount';
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1)
            select 'Event/EventDashBoard','LastYearEventTotalFineAmount', Depo.PRG_Type, EventType.PK_EventType,sum(Event.FineAmount)
            from Event 
            join EventType on EventType.PK_EventType = Event.FK_EventType
            join Vehicle on Vehicle.PK_Vehicle = Event.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            where  1=1
            and Event.CreatedAt >= '2020-01-01' and Event.CreatedAt < '2021-01-01'
            group by Depo.PRG_Type,EventType.PK_EventType
            having sum(Event.FineAmount) > 0;
             */

            object LastMonthDepoWiseEventTotalFineAmount;
            /*
            Delete  from ReadyReport where ReportName = 'LastMonthDepoWiseEventTotalFineAmount';
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1)
            select 'Event/EventDashBoard','LastMonthDepoWiseEventTotalFineAmount', Depo.PRG_Type, Depo.Name, sum(Event.FineAmount)
            from Event 
            join EventType on EventType.PK_EventType = Event.FK_EventType
            join Vehicle on Vehicle.PK_Vehicle = Event.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            where  1=1
            and Event.CreatedAt >= '2020-08-01' and Event.CreatedAt < '2020-09-01'
            group by Depo.PRG_Type,Depo.Name
            having sum(Event.FineAmount) > 0;
            */
            if (CurrentUser.PRG_Type == "ALL")
            {
                LastMonth_fitness_paper_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalExpense" && m.Key == "fitness_paper").Select(m => m.Value1).Sum();
                LastMonth_insurance_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalExpense" && m.Key == "insurance").Select(m => m.Value1).Sum();
                LastMonth_route_permit_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalExpense" && m.Key == "route_permit").Select(m => m.Value1).Sum();
                LastMonth_tax_token_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalExpense" && m.Key == "tax_token").Select(m => m.Value1).Sum();

                LastYear_fitness_paper_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalExpense" && m.Key == "fitness_paper").Select(m => m.Value1).Sum();
                LastYear_insurance_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalExpense" && m.Key == "insurance").Select(m => m.Value1).Sum();
                LastYear_route_permit_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalExpense" && m.Key == "route_permit").Select(m => m.Value1).Sum();
                LastYear_tax_token_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalExpense" && m.Key == "tax_token").Select(m => m.Value1).Sum();

                LastMonth_fitness_paper_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalFineAmount" && m.Key == "fitness_paper").Select(m => m.Value1).Sum();
                LastMonth_insurance_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalFineAmount" && m.Key == "insurance").Select(m => m.Value1).Sum();
                LastMonth_route_permit_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalFineAmount" && m.Key == "route_permit").Select(m => m.Value1).Sum();
                LastMonth_tax_token_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalFineAmount" && m.Key == "tax_token").Select(m => m.Value1).Sum();

                LastYear_fitness_paper_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalFineAmount" && m.Key == "fitness_paper").Select(m => m.Value1).Sum();
                LastYear_insurance_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalFineAmount" && m.Key == "insurance").Select(m => m.Value1).Sum();
                LastYear_route_permit_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalFineAmount" && m.Key == "route_permit").Select(m => m.Value1).Sum();
                LastYear_tax_token_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalFineAmount" && m.Key == "tax_token").Select(m => m.Value1).Sum();

                LastMonthDepoWiseEventTotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthDepoWiseEventTotalFineAmount").ToList();
            }
            else
            {
                LastMonth_fitness_paper_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalExpense" && m.Key == "fitness_paper" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
                LastMonth_insurance_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalExpense" && m.Key == "insurance" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
                LastMonth_route_permit_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalExpense" && m.Key == "route_permit" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
                LastMonth_tax_token_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalExpense" && m.Key == "tax_token" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();

                LastYear_fitness_paper_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalExpense" && m.Key == "fitness_paper" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
                LastYear_insurance_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalExpense" && m.Key == "insurance" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
                LastYear_route_permit_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalExpense" && m.Key == "route_permit" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
                LastYear_tax_token_TotalExpense = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalExpense" && m.Key == "tax_token" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();

                LastMonth_fitness_paper_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalFineAmount" && m.Key == "fitness_paper" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
                LastMonth_insurance_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalFineAmount" && m.Key == "insurance" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
                LastMonth_route_permit_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalFineAmount" && m.Key == "route_permit" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
                LastMonth_tax_token_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthEventTotalFineAmount" && m.Key == "tax_token" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();

                LastYear_fitness_paper_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalFineAmount" && m.Key == "fitness_paper" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
                LastYear_insurance_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalFineAmount" && m.Key == "insurance" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
                LastYear_route_permit_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalFineAmount" && m.Key == "route_permit" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
                LastYear_tax_token_TotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastYearEventTotalFineAmount" && m.Key == "tax_token" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();

                LastMonthDepoWiseEventTotalFineAmount = bll.db.ReadyReports.Where(m => m.ReportName == "LastMonthDepoWiseEventTotalFineAmount" && m.PRG_Type == CurrentUser.PRG_Type).ToList();
            }
            return Json(new
            {
                LastMonth_fitness_paper_TotalExpense = LastMonth_fitness_paper_TotalExpense != null ? LastMonth_fitness_paper_TotalExpense : 0,
                LastMonth_insurance_TotalExpense = LastMonth_insurance_TotalExpense != null ? LastMonth_insurance_TotalExpense : 0,
                LastMonth_route_permit_TotalExpense = LastMonth_route_permit_TotalExpense != null ? LastMonth_route_permit_TotalExpense : 0,
                LastMonth_tax_token_TotalExpense = LastMonth_tax_token_TotalExpense != null ? LastMonth_tax_token_TotalExpense : 0,

                LastYear_fitness_paper_TotalExpense = LastYear_fitness_paper_TotalExpense != null ? LastYear_fitness_paper_TotalExpense : 0,
                LastYear_insurance_TotalExpense = LastYear_insurance_TotalExpense != null ? LastYear_insurance_TotalExpense : 0,
                LastYear_route_permit_TotalExpense = LastYear_route_permit_TotalExpense != null ? LastYear_route_permit_TotalExpense : 0,
                LastYear_tax_token_TotalExpense = LastYear_tax_token_TotalExpense != null ? LastYear_tax_token_TotalExpense : 0,

                LastMonth_fitness_paper_TotalFineAmount = LastMonth_fitness_paper_TotalFineAmount != null ? LastMonth_fitness_paper_TotalFineAmount : 0,
                LastMonth_insurance_TotalFineAmount = LastMonth_insurance_TotalFineAmount != null ? LastMonth_insurance_TotalFineAmount : 0,
                LastMonth_route_permit_TotalFineAmount = LastMonth_route_permit_TotalFineAmount != null ? LastMonth_route_permit_TotalFineAmount : 0,
                LastMonth_tax_token_TotalFineAmount = LastMonth_tax_token_TotalFineAmount != null ? LastMonth_tax_token_TotalFineAmount : 0,

                LastYear_fitness_paper_TotalFineAmount = LastYear_fitness_paper_TotalFineAmount != null ? LastYear_fitness_paper_TotalFineAmount : 0,
                LastYear_insurance_TotalFineAmount = LastYear_insurance_TotalFineAmount != null ? LastYear_insurance_TotalFineAmount : 0,
                LastYear_route_permit_TotalFineAmount = LastYear_route_permit_TotalFineAmount != null ? LastYear_route_permit_TotalFineAmount : 0,
                LastYear_tax_token_TotalFineAmount = LastYear_tax_token_TotalFineAmount != null ? LastYear_tax_token_TotalFineAmount : 0,

                LastMonthDepoWiseEventTotalFineAmount
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Event_Index(string StartingDate, string EndingDate, string FK_EventType, string FK_Depo, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<Event> list = new List<Event>();

            var query = bll.db.Events.Where(c => c.IsDeleted == false).AsQueryable();

            //StartingDate EndingDate
            if (!string.IsNullOrEmpty(StartingDate) && !string.IsNullOrEmpty(EndingDate))
            {
                var _StartingDate = Convert.ToDateTime(StartingDate);
                var _EndingDate = Convert.ToDateTime(EndingDate);
                _EndingDate = _EndingDate.AddDays(1);
                query = query.Where(m => (_StartingDate <= m.IssueDate && m.IssueDate <= _EndingDate) || (_StartingDate <= m.DepositDate && m.DepositDate <= _EndingDate));
            }
            ViewBag.StartingDate = StartingDate;
            ViewBag.EndingDate = EndingDate;

            //EventTypes
            if (!string.IsNullOrEmpty(FK_EventType))
            {
                query = query.Where(m => m.FK_EventType == FK_EventType);
            }
            ViewBag.EventTypes = new SelectList(bll.db.EventTypes.Where(c => c.IsDeleted == false).OrderBy(m => m.Title), "PK_EventType", "Title", FK_EventType);

            //FK_Depo
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            if (!string.IsNullOrEmpty(FK_Depo))
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                query = query.Where(m => m.Vehicle.FK_Depo == _FK_Depo);
            }
            ViewBag.Depoes = new SelectList(bll.db.Depoes.AsEnumerable().Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo)).OrderBy(m => m.Name), "PK_Depo", "Name", FK_Depo);

            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.Vehicle.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //final
            if ((!string.IsNullOrEmpty(StartingDate)) || (!string.IsNullOrEmpty(EndingDate)) || (!string.IsNullOrEmpty(RegistrationNumber)))
            {
                list = query.OrderBy(m => m.IssueDate).ToList();
            }

            return View(list);
        }

        public ActionResult EventReportForAccountant_1(string StartingDate, string EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.StartingDate = StartingDate;
            ViewBag.EndingDate = EndingDate;
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            if (string.IsNullOrEmpty(StartingDate) || string.IsNullOrEmpty(EndingDate))
            {
                return View(dictioneryList);
            }
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = int.MaxValue;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = $"EXEC Report_GetEventReportForAccountant_1 " +
            $"@PRG_Type = '{CurrentUser.PRG_Type}'," +
            $"@StartingDate = '{StartingDate}'," +
            $"@EndingDate = '{EndingDate}';";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            adpt.Fill(dataTable);
            dataTable.Columns.Add("TotalAMount", typeof(decimal));
            foreach (DataRow item in dataTable.Rows)
            {
                item["TotalAMount"] =
                Convert.ToDecimal(String.IsNullOrEmpty(item["TaxToken_PrimaryAmount"].ToString()) ? "0" : item["TaxToken_PrimaryAmount"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["TaxToken_AdvancedIncomeTax"].ToString()) ? "0" : item["TaxToken_AdvancedIncomeTax"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["TaxToken_SupplementaryDutyAmount"].ToString()) ? "0" : item["TaxToken_SupplementaryDutyAmount"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["TaxToken_FinancialAssistanceFund"].ToString()) ? "0" : item["TaxToken_FinancialAssistanceFund"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["FitnessPaper_PrimaryAmount"].ToString()) ? "0" : item["FitnessPaper_PrimaryAmount"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["FitnessPaper_AdvancedIncomeTax"].ToString()) ? "0" : item["FitnessPaper_AdvancedIncomeTax"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["FitnessPaper_AdditionalAmount"].ToString()) ? "0" : item["FitnessPaper_AdditionalAmount"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["FitnessPaper_SupplementaryDutyAmount"].ToString()) ? "0" : item["FitnessPaper_SupplementaryDutyAmount"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["FitnessPaper_FinancialAssistanceFund"].ToString()) ? "0" : item["FitnessPaper_FinancialAssistanceFund"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["RoutePermit_PrimaryAmount"].ToString()) ? "0" : item["RoutePermit_PrimaryAmount"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["RoutePermit_AdditionalAmount"].ToString()) ? "0" : item["RoutePermit_AdditionalAmount"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["Others_Registration_PrimaryAmount"].ToString()) ? "0" : item["Others_Registration_PrimaryAmount"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["Others_Registration_OtherAmount"].ToString()) ? "0" : item["Others_Registration_OtherAmount"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["Other_OwnershipTransfer_WithdrawHirePurchaseAmount"].ToString()) ? "0" : item["Other_OwnershipTransfer_WithdrawHirePurchaseAmount"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["Other_OwnershipTransfer_OtherAmount"].ToString()) ? "0" : item["Other_OwnershipTransfer_OtherAmount"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["Other_Registration_HirePurchase"].ToString()) ? "0" : item["Other_Registration_HirePurchase"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["Other_Registration_DigitalRegistrationFee"].ToString()) ? "0" : item["Other_Registration_DigitalRegistrationFee"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["Other_OwnershipTransfer_DigitalRegistrationFee"].ToString()) ? "0" : item["Other_OwnershipTransfer_DigitalRegistrationFee"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["Other_Registration_SupplementaryDutyAmount"].ToString()) ? "0" : item["Other_Registration_SupplementaryDutyAmount"]) +
                Convert.ToDecimal(String.IsNullOrEmpty(item["Other_Registration_FinancialAssistanceFund"].ToString()) ? "0" : item["Other_Registration_FinancialAssistanceFund"]);
            }
            dictioneryList.AddRange(GetTableRows(dataTable));
            return View(dictioneryList);
        }

        public ActionResult Index()
        {
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var pk_vehicles = bll.db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN").Where(m => accessibleDepoes.Contains(m.FK_Depo)).Select(c => c.PK_Vehicle).ToList();
            var _list = bll.db.Events.Where(m => pk_vehicles.Contains(m.FK_Vehicle)).ToList();
            var list = _list.GroupBy(m => m.FK_Vehicle).Select(
                m =>
                new
                {
                    PK_Vehicle = m.Key,
                    RegistrationNumber = m.FirstOrDefault().Vehicle.RegistrationNumber,
                    insurance = m.Where(n => n.FK_EventType == "insurance").Any() ? CommonClass.ConvertToDateString(m.Where(n => n.FK_EventType == "insurance").OrderByDescending(n => n.IssueDate).FirstOrDefault().IssueDate) : "-",
                    route_permit = m.Where(n => n.FK_EventType == "route_permit").Any() ? CommonClass.ConvertToDateString(m.Where(n => n.FK_EventType == "route_permit").OrderByDescending(n => n.IssueDate).FirstOrDefault().IssueDate) : "-",
                    fitness_paper = m.Where(n => n.FK_EventType == "fitness_paper").Any() ? CommonClass.ConvertToDateString(m.Where(n => n.FK_EventType == "fitness_paper").OrderByDescending(n => n.IssueDate).FirstOrDefault().IssueDate) : "-",
                    tax_token = m.Where(n => n.FK_EventType == "tax_token").Any() ? CommonClass.ConvertToDateString(m.Where(n => n.FK_EventType == "tax_token").OrderByDescending(n => n.IssueDate).FirstOrDefault().IssueDate) : "-",
                }
                ).ToList();
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            foreach (var item in list)
            {
                var dict = new Dictionary<string, string>();
                dict.Add("PK_Vehicle", item.PK_Vehicle.ToString());
                dict.Add("RegistrationNumber", item.RegistrationNumber);
                dict.Add("insurance", item.insurance);
                dict.Add("route_permit", item.route_permit);
                dict.Add("fitness_paper", item.fitness_paper);
                dict.Add("tax_token", item.tax_token);
                dictioneryList.Add(dict);
            }
            ViewBag.dictioneryList = dictioneryList;
            return View();
        }

        public ActionResult Index2(String RegistrationNumber, string FK_EventType)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            List<Event> list = new List<Event>();

            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var query = bll.db.Events.Where(m => m.IsDeleted != true && accessibleDepoes.Contains(m.Vehicle.FK_Depo)).AsEnumerable();
            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query = query.Where(m => m.Vehicle.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            //PRG_Type
            if (FK_EventType == null)
            {
                query = query.Where(m => m.Vehicle.Depo.PRG_Type == null);
            }
            else if (FK_EventType != "all")
            {
                query = query.Where(m => m.FK_EventType == FK_EventType);
            }
            List<SelectListItem> EventTypeList = new List<SelectListItem>();
            EventTypeList.Add(new SelectListItem() { Value = "all", Text = "All" });
            EventTypeList.AddRange(bll.db.EventTypes.AsEnumerable().Select(m => new SelectListItem { Value = m.PK_EventType, Text = m.Title }));
            ViewBag.EventTypes = new SelectList(EventTypeList.OrderBy(m => m.Text), "Value", "Text", FK_EventType);

            if ((!string.IsNullOrEmpty(RegistrationNumber)) || (!string.IsNullOrEmpty(FK_EventType)))
            {
                list = query.ToList();
            }


            return View(list);
        }

        public ActionResult ViewVehicleEventDocs(Guid FK_Vehicle)
        {
            var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == FK_Vehicle).FirstOrDefault();
            return View(vehicle);
        }

        public ActionResult View(Guid PK_Event)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (PK_Event == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Events.Find(PK_Event);
                if (model != null)
                {
                    var viewModel = bll.ConvertToViewModel(model);
                    var list = bll.db.EventDocuments.Where(m => m.FK_Event == model.PK_Event && m.IsDeleted == false).ToList();
                    viewModel.EventDocuments = list;
                    ViewBag.parentKey = model.FK_Vehicle;
                    return View(viewModel);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        //public ActionResult Create(Guid? FK_Vehicle)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    var model = new Event();
        //    ViewBag.model = model;
        //    if (FK_Vehicle != null)
        //    {
        //        var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
        //        ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(c.FK_Depo)).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", FK_Vehicle);
        //    }
        //    else
        //    {
        //        var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
        //        ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(c.FK_Depo)).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
        //    }
        //    ViewBag.EventTypes = new SelectList(bll.db.EventTypes.Where(c => c.IsDeleted == false).OrderBy(m => m.Title), "PK_EventType", "Title");
        //    ViewBag.OtherEventTypeDetailDict = new SelectList(OtherEventTypeDetailDict.OrderBy(m => m.Value), "Key", "Value");
        //    ViewBag.YesNoDict = new SelectList(YesNoDict, "Key", "Value");
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Create(FormCollection formCollection, List<HttpPostedFileBase> ImageFiles)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    try
        //    {
        //        //# Event
        //        var eventModel = new Event();
        //        eventModel.PK_Event = Guid.NewGuid();
        //        eventModel.IsDeleted = false;
        //        eventModel.CreatedAt = DateTime.Now;
        //        eventModel.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;


        //        eventModel.FK_Vehicle = Guid.Parse(formCollection["FK_Vehicle"]);
        //        eventModel.FK_EventType = formCollection["FK_EventType"];
        //        eventModel.IssueDate = Convert.ToDateTime(formCollection["IssueDate"]);

        //        if (!string.IsNullOrEmpty(formCollection["DepositDate"]))
        //        {
        //            eventModel.DepositDate = Convert.ToDateTime(formCollection["DepositDate"]);
        //        }

        //        if (eventModel.FK_EventType == "insurance" && formCollection["PolicyNumber"] != "")
        //        {
        //            eventModel.PolicyNumber = formCollection["PolicyNumber"];
        //        }
        //        else
        //        {
        //            eventModel.PolicyNumber = null;
        //        }
        //        if (formCollection["PrimaryAmount"] != "")
        //        {
        //            eventModel.PrimaryAmount = Convert.ToDecimal(formCollection["PrimaryAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.PrimaryAmount = null;
        //        }

        //        if ((eventModel.FK_EventType == "fitness_paper" || eventModel.FK_EventType == "tax_token") && formCollection["AdvancedIncomeTax"] != "")
        //        {
        //            eventModel.AdvancedIncomeTax = Convert.ToDecimal(formCollection["AdvancedIncomeTax"]);
        //        }
        //        else
        //        {
        //            eventModel.AdvancedIncomeTax = null;
        //        }

        //        if (eventModel.FK_EventType == "insurance" && formCollection["PremiumAmount"] != "")
        //        {
        //            eventModel.PremiumAmount = Convert.ToDecimal(formCollection["PremiumAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.PremiumAmount = null;
        //        }

        //        if (eventModel.FK_EventType == "others")
        //        {
        //            eventModel.OtherEventTypeDetail = formCollection["OtherEventTypeDetail"];
        //            if (eventModel.OtherEventTypeDetail == "Registration" && formCollection["DigitalRegistrationFee"] != "")
        //            {
        //                eventModel.DigitalRegistrationFee = Convert.ToDecimal(formCollection["DigitalRegistrationFee"]);
        //            }
        //            else
        //            {
        //                eventModel.DigitalRegistrationFee = null;
        //            }

        //            if (eventModel.OtherEventTypeDetail == "Registration" && formCollection["HirePurchase"] != "")
        //            {
        //                eventModel.HirePurchase = Convert.ToDecimal(formCollection["HirePurchase"]);
        //            }
        //            else
        //            {
        //                eventModel.DigitalRegistrationFee = null;
        //            }

        //        }


        //        if (formCollection["FineAmount"] != "")
        //        {
        //            eventModel.FineAmount = Convert.ToDecimal(formCollection["FineAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.FineAmount = null;
        //        }

        //        if (eventModel.FK_EventType == "tax_token" && formCollection["AdditionalAmount"] != "")
        //        {
        //            eventModel.AdditionalAmount = Convert.ToDecimal(formCollection["AdditionalAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.AdditionalAmount = null;
        //        }

        //        if (formCollection["OtherAmount"] != "")
        //        {
        //            eventModel.OtherAmount = Convert.ToDecimal(formCollection["OtherAmount"]);
        //            eventModel.OtherNote = formCollection["OtherNote"];
        //        }
        //        else
        //        {
        //            eventModel.OtherAmount = null;
        //            eventModel.OtherNote = null;
        //        }

        //        if (formCollection["TotalAmount"] != "")
        //        {
        //            eventModel.TotalAmount = Convert.ToDecimal(formCollection["TotalAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.TotalAmount = null;
        //        }

        //        // alert
        //        if (eventModel.FK_EventType != "others")
        //        {
        //            var eventType = bll.db.EventTypes.Where(m => m.PK_EventType == eventModel.FK_EventType).FirstOrDefault();
        //            eventModel.ExpirationDate = eventModel.IssueDate.AddDays(eventType.NextCycleDays);
        //            eventModel.AlertDate = eventModel.IssueDate.AddDays(eventType.NextCycleDays - eventType.AlertBeforeNextCycleDays);
        //            eventModel.IsAlertable = true;
        //            eventModel.AlertOn = true;
        //        }
        //        //# for Others
        //        else if (formCollection["ExpirationDate"] != "")
        //        {
        //            eventModel.OtherEventTypeDetail = formCollection["OtherEventTypeDetail"];
        //            eventModel.ExpirationDate = Convert.ToDateTime(formCollection["ExpirationDate"]);
        //            eventModel.AlertDate = Convert.ToDateTime(formCollection["ExpirationDate"]).AddDays(-10);
        //            eventModel.IsAlertable = true;
        //            eventModel.AlertOn = true;
        //        }
        //        else
        //        {
        //            eventModel.OtherEventTypeDetail = formCollection["OtherEventTypeDetail"];
        //            eventModel.ExpirationDate = eventModel.IssueDate;
        //            eventModel.AlertDate = null;
        //            eventModel.IsAlertable = false;
        //            eventModel.AlertOn = false;
        //        }

        //        bll.db.Events.Add(eventModel);
        //        //# Event Doc


        //        //# create folder
        //        var _EventTypeTitle = bll.db.EventTypes.Find(eventModel.FK_EventType).PK_EventType;
        //        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + eventModel.FK_Vehicle + "/" + _EventTypeTitle + "/";
        //        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
        //        if (!Directory.Exists(physicalFolderPath))
        //        {
        //            Directory.CreateDirectory(physicalFolderPath);
        //        }

        //        int totalDocument = Convert.ToInt32(formCollection["rowCount"]);
        //        for (int i = 0; i < totalDocument; i++)
        //        {
        //            var eventDocument = new EventDocument();

        //            eventDocument.PK_EventDocument = Guid.NewGuid();
        //            eventDocument.IsDeleted = false;

        //            eventDocument.CreatedAt = DateTime.Now;
        //            eventDocument.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;

        //            eventDocument.FK_Event = eventModel.PK_Event;
        //            eventDocument.Title = formCollection["Title_" + i];
        //            eventDocument.IdentitficaitonKey = formCollection["IdentitficaitonKey_" + i];
        //            eventDocument.IdentitficaitonValue = formCollection["IdentitficaitonValue_" + i];

        //            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " " + i + " " + eventDocument.PK_EventDocument + "." + ImageFiles[i].FileName.Split('.').Last();
        //            ImageFiles[i].SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

        //            eventDocument.ImageLocation = virtualFilePath;

        //            bll.db.EventDocuments.Add(eventDocument);
        //        }

        //        //Get previous event
        //        var previousEvents = bll.db.Events.Where(m => m.FK_Vehicle == eventModel.FK_Vehicle && m.FK_EventType == eventModel.FK_EventType && m.AlertOn == true && m.IssueDate < eventModel.IssueDate).OrderByDescending(m => m.IssueDate).ToList();
        //        if (previousEvents.Count > 0)
        //        {
        //            foreach (var previousEvent in previousEvents)
        //            {
        //                previousEvent.AlertOn = false;
        //                previousEvent.RenewedOn = eventModel.IssueDate;
        //                previousEvent.PK_RenewedEvent = eventModel.PK_Event;
        //                previousEvent.FK_AppUser_RenewedBy = eventModel.FK_CreatedByUser;
        //            }
        //            CreateAlertMessage(AlertMessageType.Success, "Success", "Event is successfully added. (Updated the older " + eventModel.EventType.Title + ").");
        //        }
        //        else
        //        {
        //            CreateAlertMessage(AlertMessageType.Success, "Success", "Event is successfully added.");
        //        }
        //        bll.db.SaveChanges();
        //        return RedirectToAction("Create", new { FK_Vehicle = eventModel.FK_Vehicle });
        //    }
        //    catch (Exception exception)
        //    {
        //        CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
        //        return RedirectToAction("Create");
        //    }
        //}

        public ActionResult Event_Create(Guid? FK_Vehicle)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = new Event();
            ViewBag.model = model;
            if (FK_Vehicle != null)
            {
                ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.PK_Vehicle == FK_Vehicle), "PK_Vehicle", "RegistrationNumber", FK_Vehicle);
            }
            else
            {
                ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.PK_Vehicle == null), "PK_Vehicle", "RegistrationNumber");
            }
            ViewBag.EventTypes = new SelectList(bll.db.EventTypes.Where(c => c.IsDeleted == false).OrderBy(m => m.Title), "PK_EventType", "Title");
            ViewBag.OtherEventTypeDetailDict = new SelectList(OtherEventTypeDetailDict.OrderBy(m => m.Value), "Key", "Value");
            ViewBag.YesNoDict = new SelectList(YesNoDict, "Key", "Value");
            return View();
        }
        [HttpPost]
        public ActionResult Event_Create(FormCollection formCollection, List<HttpPostedFileBase> ImageFiles)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                //# Event
                var eventModel = new Event();
                eventModel.PK_Event = Guid.NewGuid();
                eventModel.IsDeleted = false;
                eventModel.CreatedAt = DateTime.Now;
                eventModel.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;


                eventModel.FK_Vehicle = Guid.Parse(formCollection["FK_Vehicle"]);
                eventModel.FK_EventType = formCollection["FK_EventType"];
                if (eventModel.FK_EventType == "others")
                {
                    eventModel.OtherEventTypeDetail = formCollection["OtherEventTypeDetail"];
                }
                else
                {
                    eventModel.OtherEventTypeDetail = null;
                }

                if (!string.IsNullOrEmpty(formCollection["DepositDate"]))
                {
                    eventModel.DepositDate = Convert.ToDateTime(formCollection["DepositDate"]);
                }
                //#PrimaryAmount
                if (formCollection["PrimaryAmount"] != "")
                {
                    eventModel.PrimaryAmount = Convert.ToDecimal(formCollection["PrimaryAmount"]);
                }
                else
                {
                    eventModel.PrimaryAmount = null;
                }
                //#PolicyNumber
                if (eventModel.FK_EventType == "insurance" && formCollection["PolicyNumber"] != "")
                {
                    eventModel.PolicyNumber = formCollection["PolicyNumber"];
                }
                else
                {
                    eventModel.PolicyNumber = null;
                }
                //#PremiumAmount
                if (eventModel.FK_EventType == "insurance" && formCollection["PremiumAmount"] != "")
                {
                    eventModel.PremiumAmount = Convert.ToDecimal(formCollection["PremiumAmount"]);
                }
                else
                {
                    eventModel.PremiumAmount = null;
                }
                //#AdvancedIncomeTax
                if ((eventModel.FK_EventType == "fitness_paper" || eventModel.FK_EventType == "tax_token") && formCollection["AdvancedIncomeTax"] != "")
                {
                    eventModel.AdvancedIncomeTax = Convert.ToDecimal(formCollection["AdvancedIncomeTax"]);
                }
                else
                {
                    eventModel.AdvancedIncomeTax = null;
                }
                //#DigitalRegistrationFee
                if ((eventModel.OtherEventTypeDetail == "Registration" || eventModel.FK_EventType == "OwnershipTransfer") && formCollection["DigitalRegistrationFee"] != "")
                {
                    eventModel.DigitalRegistrationFee = Convert.ToDecimal(formCollection["DigitalRegistrationFee"]);
                }
                else
                {
                    eventModel.DigitalRegistrationFee = null;
                }
                //#HirePurchase
                if (eventModel.OtherEventTypeDetail == "Registration" && formCollection["HirePurchase"] != "")
                {
                    eventModel.HirePurchase = Convert.ToDecimal(formCollection["HirePurchase"]);
                }
                else
                {
                    eventModel.HirePurchase = null;
                }
                //#WithdrawHirePurchaseAmount
                if (eventModel.OtherEventTypeDetail == "OwnershipTransfer" && formCollection["WithdrawHirePurchaseAmount"] != "")
                {
                    eventModel.WithdrawHirePurchaseAmount = Convert.ToDecimal(formCollection["WithdrawHirePurchaseAmount"]);
                }
                else
                {
                    eventModel.WithdrawHirePurchaseAmount = null;
                }
                //#SupplementaryDutyAmount
                if ((eventModel.FK_EventType == "fitness_paper" || eventModel.FK_EventType == "tax_token" || eventModel.OtherEventTypeDetail == "Registration") && formCollection["SupplementaryDutyAmount"] != "")
                {
                    eventModel.SupplementaryDutyAmount = Convert.ToDecimal(formCollection["SupplementaryDutyAmount"]);
                }
                else
                {
                    eventModel.SupplementaryDutyAmount = null;
                }
                //#FinancialAssistanceFund
                if ((eventModel.FK_EventType == "fitness_paper" || eventModel.FK_EventType == "tax_token" || eventModel.OtherEventTypeDetail == "Registration") && formCollection["FinancialAssistanceFund"] != "")
                {
                    eventModel.FinancialAssistanceFund = Convert.ToDecimal(formCollection["FinancialAssistanceFund"]);
                }
                else
                {
                    eventModel.FinancialAssistanceFund = null;
                }
                //#AdditionalAmount
                if (eventModel.OtherEventTypeDetail == "OwnershipTransfer" && formCollection["AdditionalAmount"] != "")
                {
                    eventModel.AdditionalAmount = Convert.ToDecimal(formCollection["AdditionalAmount"]);
                }
                else
                {
                    eventModel.AdditionalAmount = null;
                }
                //#FineAmount
                if (formCollection["FineAmount"] != "")
                {
                    eventModel.FineAmount = Convert.ToDecimal(formCollection["FineAmount"]);
                }
                else
                {
                    eventModel.FineAmount = null;
                }

                //#OtherAmount
                if (formCollection["OtherAmount"] != "")
                {
                    eventModel.OtherAmount = Convert.ToDecimal(formCollection["OtherAmount"]);
                    eventModel.OtherNote = formCollection["OtherNote"];
                }
                else
                {
                    eventModel.OtherAmount = null;
                    eventModel.OtherNote = null;
                }

                if (formCollection["TotalAmount"] != "")
                {
                    eventModel.TotalAmount = Convert.ToDecimal(formCollection["TotalAmount"]);
                }
                else
                {
                    eventModel.TotalAmount = null;
                }

                // alert
                if (eventModel.FK_EventType != "others")
                {
                    var eventType = bll.db.EventTypes.Where(m => m.PK_EventType == eventModel.FK_EventType).FirstOrDefault();
                    //eventModel.ExpirationDate = eventModel.IssueDate.AddDays(eventType.NextCycleDays);
                    //eventModel.AlertDate = eventModel.IssueDate.AddDays(eventType.NextCycleDays - eventType.AlertBeforeNextCycleDays);
                    //eventModel.IsAlertable = true;
                    //eventModel.AlertOn = true;
                }
                //# for Others
                else if (formCollection["ExpirationDate"] != "")
                {
                    eventModel.OtherEventTypeDetail = formCollection["OtherEventTypeDetail"];
                    //eventModel.ExpirationDate = Convert.ToDateTime(formCollection["ExpirationDate"]);
                    //eventModel.AlertDate = Convert.ToDateTime(formCollection["ExpirationDate"]).AddDays(-10);
                    //eventModel.IsAlertable = true;
                    //eventModel.AlertOn = true;
                }
                else
                {
                    eventModel.OtherEventTypeDetail = formCollection["OtherEventTypeDetail"];
                    //eventModel.ExpirationDate = eventModel.IssueDate;
                    //eventModel.AlertDate = null;
                    //eventModel.IsAlertable = false;
                    //eventModel.AlertOn = false;
                }
                eventModel.StatusText = "Created";
                bll.db.Events.Add(eventModel);
                //# Event Doc


                //# create folder
                var _EventTypeTitle = bll.db.EventTypes.Find(eventModel.FK_EventType).PK_EventType;
                string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + eventModel.FK_Vehicle + "/" + _EventTypeTitle + "/";
                string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                if (!Directory.Exists(physicalFolderPath))
                {
                    Directory.CreateDirectory(physicalFolderPath);
                }

                int totalDocument = Convert.ToInt32(formCollection["rowCount"]);
                for (int i = 0; i < totalDocument; i++)
                {
                    var eventDocument = new EventDocument();

                    eventDocument.PK_EventDocument = Guid.NewGuid();
                    eventDocument.IsDeleted = false;

                    eventDocument.CreatedAt = DateTime.Now;
                    eventDocument.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;

                    eventDocument.FK_Event = eventModel.PK_Event;
                    eventDocument.Title = formCollection["Title_" + i];
                    eventDocument.IdentitficaitonKey = formCollection["IdentitficaitonKey_" + i];
                    eventDocument.IdentitficaitonValue = formCollection["IdentitficaitonValue_" + i];

                    string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " " + i + " " + eventDocument.PK_EventDocument + "." + ImageFiles[i].FileName.Split('.').Last();
                    ImageFiles[i].SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                    eventDocument.ImageLocation = virtualFilePath;

                    bll.db.EventDocuments.Add(eventDocument);
                }

                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Event is successfully added.");
                return RedirectToAction("Event_Create", new { FK_Vehicle = eventModel.FK_Vehicle });
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Event_Create");
            }
        }

        public ActionResult Event_Issue(Guid? PK_Event)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.Events.Where(m => m.PK_Event == PK_Event && m.StatusText == EventStatus.Created).FirstOrDefault();
            ViewBag.model = model;

            ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN" && c.PK_Vehicle == model.FK_Vehicle).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);

            ViewBag.EventTypes = new SelectList(bll.db.EventTypes.Where(c => c.IsDeleted == false).OrderBy(m => m.Title), "PK_EventType", "Title", model.FK_EventType);
            ViewBag.OtherEventTypeDetailDict = new SelectList(OtherEventTypeDetailDict.OrderBy(m => m.Value), "Key", "Value", model.OtherEventTypeDetail);
            ViewBag.YesNoDict = new SelectList(YesNoDict, "Key", "Value");

            ViewBag.EventDocuments = bll.db.EventDocuments.Where(m => m.FK_Event == PK_Event).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Event_Issue(FormCollection formCollection, List<HttpPostedFileBase> ImageFiles)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                //# Event
                var PK_Event = Guid.Parse(formCollection["PK_Event"]);
                var eventModel = bll.db.Events.Where(m => m.PK_Event == PK_Event && m.StatusText == EventStatus.Created).FirstOrDefault();

                eventModel.IssuedAt = DateTime.Now;
                eventModel.FK_IssuedByUser = CurrentUser.PK_User;

                eventModel.IssueDate = Convert.ToDateTime(formCollection["IssueDate"]);

                if (formCollection["AdditionalAmount"] != "")
                {
                    eventModel.AdditionalAmount = Convert.ToDecimal(formCollection["AdditionalAmount"]);
                }
                else
                {
                    eventModel.AdditionalAmount = null;
                }

                if (formCollection["TotalAmount"] != "")
                {
                    eventModel.TotalAmount = Convert.ToDecimal(formCollection["TotalAmount"]);
                }
                else
                {
                    eventModel.TotalAmount = null;
                }

                if (formCollection["ExpirationDate"] != "")
                {
                    eventModel.ExpirationDate = Convert.ToDateTime(formCollection["ExpirationDate"]);
                    eventModel.IsAlertable = true;
                    eventModel.AlertOn = true;
                }
                else
                {
                    eventModel.ExpirationDate = null;
                    eventModel.IsAlertable = false;
                    eventModel.AlertOn = false;
                }

                eventModel.StatusText = EventStatus.Issued;
                //# Event Doc


                //# create folder
                var _EventTypeTitle = bll.db.EventTypes.Find(eventModel.FK_EventType).PK_EventType;
                string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + eventModel.FK_Vehicle + "/" + _EventTypeTitle + "/";
                string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                if (!Directory.Exists(physicalFolderPath))
                {
                    Directory.CreateDirectory(physicalFolderPath);
                }

                int totalDocument = Convert.ToInt32(formCollection["rowCount"]);
                for (int i = 0; i < totalDocument; i++)
                {
                    var eventDocument = new EventDocument();

                    eventDocument.PK_EventDocument = Guid.NewGuid();
                    eventDocument.IsDeleted = false;

                    eventDocument.CreatedAt = DateTime.Now;
                    eventDocument.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;

                    eventDocument.FK_Event = eventModel.PK_Event;
                    eventDocument.Title = formCollection["Title_" + i];
                    eventDocument.IdentitficaitonKey = formCollection["IdentitficaitonKey_" + i];
                    eventDocument.IdentitficaitonValue = formCollection["IdentitficaitonValue_" + i];

                    string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " " + i + " " + eventDocument.PK_EventDocument + "." + ImageFiles[i].FileName.Split('.').Last();
                    ImageFiles[i].SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                    eventDocument.ImageLocation = virtualFilePath;

                    bll.db.EventDocuments.Add(eventDocument);
                }

                //Get previous event
                var previousEvents = bll.db.Events.Where(m => m.FK_Vehicle == eventModel.FK_Vehicle && m.FK_EventType == eventModel.FK_EventType && m.AlertOn == true && m.IssueDate < eventModel.IssueDate).OrderByDescending(m => m.IssueDate).ToList();
                if (previousEvents.Count > 0)
                {
                    foreach (var previousEvent in previousEvents)
                    {
                        previousEvent.AlertOn = false;
                        previousEvent.RenewedOn = eventModel.IssueDate;
                        previousEvent.PK_RenewedEvent = eventModel.PK_Event;
                        previousEvent.FK_AppUser_RenewedBy = eventModel.FK_CreatedByUser;
                    }
                }
                bll.db.SaveChanges();
                if (previousEvents.Count > 0)
                {
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Event is successfully Issued. (Updated the older " + eventModel.EventType.Title + ").");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Event is successfully Issued.");
                }
                return RedirectToAction("Event_Index");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Event_Issue", new { PK_Event = formCollection["PK_Event"] });
            }
        }


        //public ActionResult Edit(Guid? PK_Event)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    var model = bll.db.Events.Where(m => m.PK_Event == PK_Event).FirstOrDefault();
        //    ViewBag.model = model;

        //    var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
        //    ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(c.FK_Depo)).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);

        //    ViewBag.EventTypes = new SelectList(bll.db.EventTypes.Where(c => c.IsDeleted == false).OrderBy(m => m.Title), "PK_EventType", "Title", model.FK_EventType);
        //    ViewBag.OtherEventTypeDetailDict = new SelectList(OtherEventTypeDetailDict.OrderBy(m => m.Value), "Key", "Value", model.OtherEventTypeDetail);
        //    ViewBag.YesNoDict = new SelectList(YesNoDict, "Key", "Value");

        //    ViewBag.EventDocuments = bll.db.EventDocuments.Where(m => m.FK_Event == PK_Event).ToList();
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Edit(FormCollection formCollection, List<HttpPostedFileBase> ImageFiles)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    try
        //    {
        //        //# Event
        //        var PK_Event = Guid.Parse(formCollection["PK_Event"]);
        //        var eventModel = bll.db.Events.Where(m => m.PK_Event == PK_Event).FirstOrDefault();
        //        //eventModel.PK_Event = Guid.NewGuid();
        //        eventModel.IsDeleted = false;
        //        eventModel.UpdatedAt = DateTime.Now;
        //        eventModel.FK_UpdatedByUser = CommonClass.GetCurrentUser().PK_User;


        //        eventModel.FK_Vehicle = Guid.Parse(formCollection["FK_Vehicle"]);
        //        eventModel.FK_EventType = formCollection["FK_EventType"];
        //        eventModel.IssueDate = Convert.ToDateTime(formCollection["IssueDate"]);

        //        if (!string.IsNullOrEmpty(formCollection["DepositDate"]))
        //        {
        //            eventModel.DepositDate = Convert.ToDateTime(formCollection["DepositDate"]);
        //        }

        //        if (eventModel.FK_EventType == "insurance" && formCollection["PolicyNumber"] != "")
        //        {
        //            eventModel.PolicyNumber = formCollection["PolicyNumber"];
        //        }
        //        else
        //        {
        //            eventModel.PolicyNumber = null;
        //        }
        //        if (formCollection["PrimaryAmount"] != "")
        //        {
        //            eventModel.PrimaryAmount = Convert.ToDecimal(formCollection["PrimaryAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.PrimaryAmount = null;
        //        }

        //        if (eventModel.FK_EventType == "fitness_paper" && formCollection["AdvancedIncomeTax"] != "")
        //        {
        //            eventModel.AdvancedIncomeTax = Convert.ToDecimal(formCollection["AdvancedIncomeTax"]);
        //        }
        //        else
        //        {
        //            eventModel.AdvancedIncomeTax = null;
        //        }

        //        if (eventModel.FK_EventType == "insurance" && formCollection["PremiumAmount"] != "")
        //        {
        //            eventModel.PremiumAmount = Convert.ToDecimal(formCollection["PremiumAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.PremiumAmount = null;
        //        }

        //        if (eventModel.FK_EventType == "others")
        //        {
        //            eventModel.OtherEventTypeDetail = formCollection["OtherEventTypeDetail"];
        //            if (eventModel.OtherEventTypeDetail == "Registration" && formCollection["DigitalRegistrationFee"] != "")
        //            {
        //                eventModel.DigitalRegistrationFee = Convert.ToDecimal(formCollection["DigitalRegistrationFee"]);
        //            }
        //            else
        //            {
        //                eventModel.DigitalRegistrationFee = null;
        //            }

        //            if (eventModel.OtherEventTypeDetail == "Registration" && formCollection["HirePurchase"] != "")
        //            {
        //                eventModel.HirePurchase = Convert.ToDecimal(formCollection["HirePurchase"]);
        //            }
        //            else
        //            {
        //                eventModel.DigitalRegistrationFee = null;
        //            }

        //        }


        //        if (formCollection["FineAmount"] != "")
        //        {
        //            eventModel.FineAmount = Convert.ToDecimal(formCollection["FineAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.FineAmount = null;
        //        }

        //        if (eventModel.FK_EventType == "tax_token" && formCollection["AdditionalAmount"] != "")
        //        {
        //            eventModel.AdditionalAmount = Convert.ToDecimal(formCollection["AdditionalAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.AdditionalAmount = null;
        //        }

        //        if (formCollection["OtherAmount"] != "")
        //        {
        //            eventModel.OtherAmount = Convert.ToDecimal(formCollection["OtherAmount"]);
        //            eventModel.OtherNote = formCollection["OtherNote"];
        //        }
        //        else
        //        {
        //            eventModel.OtherAmount = null;
        //            eventModel.OtherNote = null;
        //        }

        //        if (formCollection["TotalAmount"] != "")
        //        {
        //            eventModel.TotalAmount = Convert.ToDecimal(formCollection["TotalAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.TotalAmount = null;
        //        }

        //        // alert
        //        if (eventModel.FK_EventType != "others")
        //        {
        //            var eventType = bll.db.EventTypes.Where(m => m.PK_EventType == eventModel.FK_EventType).FirstOrDefault();
        //            eventModel.ExpirationDate = eventModel.IssueDate.AddDays(eventType.NextCycleDays);
        //            eventModel.AlertDate = eventModel.IssueDate.AddDays(eventType.NextCycleDays - eventType.AlertBeforeNextCycleDays);
        //            eventModel.IsAlertable = true;
        //            eventModel.AlertOn = true;
        //        }
        //        //# for Others
        //        else if (formCollection["ExpirationDate"] != "")
        //        {
        //            eventModel.OtherEventTypeDetail = formCollection["OtherEventTypeDetail"];
        //            eventModel.ExpirationDate = Convert.ToDateTime(formCollection["ExpirationDate"]);
        //            eventModel.AlertDate = Convert.ToDateTime(formCollection["ExpirationDate"]).AddDays(-10);
        //            eventModel.IsAlertable = true;
        //            eventModel.AlertOn = true;
        //        }
        //        else
        //        {
        //            eventModel.OtherEventTypeDetail = formCollection["OtherEventTypeDetail"];
        //            eventModel.ExpirationDate = eventModel.IssueDate;
        //            eventModel.AlertDate = null;
        //            eventModel.IsAlertable = false;
        //            eventModel.AlertOn = false;
        //        }

        //        //bll.db.Events.Add(eventModel);
        //        //# Event Doc


        //        //# create folder
        //        var _EventTypeTitle = bll.db.EventTypes.Find(eventModel.FK_EventType).PK_EventType;
        //        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + eventModel.FK_Vehicle + "/" + _EventTypeTitle + "/";
        //        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
        //        if (!Directory.Exists(physicalFolderPath))
        //        {
        //            Directory.CreateDirectory(physicalFolderPath);
        //        }

        //        int totalDocument = Convert.ToInt32(formCollection["rowCount"]);
        //        for (int i = 0; i < totalDocument; i++)
        //        {
        //            var eventDocument = new EventDocument();

        //            eventDocument.PK_EventDocument = Guid.NewGuid();
        //            eventDocument.IsDeleted = false;

        //            eventDocument.CreatedAt = DateTime.Now;
        //            eventDocument.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;

        //            eventDocument.FK_Event = eventModel.PK_Event;
        //            eventDocument.Title = formCollection["Title_" + i];
        //            eventDocument.IdentitficaitonKey = formCollection["IdentitficaitonKey_" + i];
        //            eventDocument.IdentitficaitonValue = formCollection["IdentitficaitonValue_" + i];

        //            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " " + i + " " + eventDocument.PK_EventDocument + "." + ImageFiles[i].FileName.Split('.').Last();
        //            ImageFiles[i].SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

        //            eventDocument.ImageLocation = virtualFilePath;

        //            bll.db.EventDocuments.Add(eventDocument);
        //        }

        //        //Get previous event
        //        var previousEvents = bll.db.Events.Where(m => m.FK_Vehicle == eventModel.FK_Vehicle && m.FK_EventType == eventModel.FK_EventType && m.AlertOn == true && m.IssueDate < eventModel.IssueDate).OrderByDescending(m => m.IssueDate).ToList();
        //        if (previousEvents.Count > 0)
        //        {
        //            foreach (var previousEvent in previousEvents)
        //            {
        //                previousEvent.AlertOn = false;
        //                previousEvent.RenewedOn = eventModel.IssueDate;
        //                previousEvent.PK_RenewedEvent = eventModel.PK_Event;
        //                previousEvent.FK_AppUser_RenewedBy = eventModel.FK_CreatedByUser;
        //            }
        //            CreateAlertMessage(AlertMessageType.Success, "Success", "Event is successfully added. (Updated the older " + eventModel.EventType.Title + ").");
        //        }
        //        else
        //        {
        //            CreateAlertMessage(AlertMessageType.Success, "Success", "Event is successfully added.");
        //        }
        //        bll.db.SaveChanges();
        //        return RedirectToAction("Edit", new { PK_Event = formCollection["PK_Event"] });
        //    }
        //    catch (Exception exception)
        //    {
        //        CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
        //        return RedirectToAction("Edit", new { PK_Event = formCollection["PK_Event"] });
        //    }
        //}

        //public Event ConvertToModel(FormCollection formCollection, List<HttpPostedFileBase> ImageFiles)
        //{
        //    var model = new Event();
        //    model.FK_Vehicle = formCollection["FK_Vehicle"];
        //    model.FK_EventType = Guid.Parse(formCollection["FK_EventType"]);
        //    model.Title = formCollection["Title"];
        //    if (formCollection["IssueDate"] != "")
        //    {
        //        model.IssueDate = Convert.ToDateTime(formCollection["IssueDate"]);
        //    }
        //    if (formCollection["IsAlertable"] == "True")
        //    {
        //        model.IsAlertable = true;
        //        model.AlertDate = Convert.ToDateTime(formCollection["AlertDate"]);
        //    }
        //    else
        //    {
        //        model.IsAlertable = false;
        //    }

        //    // get doc info
        //    int rowCount = Convert.ToInt32(formCollection["rowCount"]) - 1;
        //    for (int i = 0; i < rowCount; i++)
        //    {
        //        var eventDocument = new EventDocument();
        //        eventDocument.PK_EventDocument = Guid.NewGuid();
        //        eventDocument.Title = formCollection["Title_" + i];
        //        eventDocument.IdentitficaitonKey = formCollection["IdentitficaitonKey" + i];
        //        eventDocument.IdentitficaitonValue = formCollection["IdentitficaitonValue" + i];
        //        eventDocument.ImageLocation = formCollection["IdentitficaitonValue" + i];

        //        var _EventTypeTitle = bll.db.EventTypes.Find(model.FK_EventType).Title;
        //        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + model.FK_Vehicle + "/" + _EventTypeTitle + "/";

        //        //# create folder
        //        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
        //        if (!Directory.Exists(physicalFolderPath))
        //        {
        //            Directory.CreateDirectory(physicalFolderPath);
        //        }

        //        string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + "." + VehicleImage.FileName.Split('.').Last();
        //        ImageFiles[i].SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

        //        eventDocument.ImageLocation = virtualFilePath;

        //        model.EventDocuments.Add(eventDocument);
        //    }



        //    return model;
        //}































        //[HttpPost]
        //public ActionResult Create(Event model)
        //{
        //    if (CommonClass.IsIvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    string modelValidator = bll.IsValidModel_ToCreate(model);
        //    if (modelValidator == ValidationStatus.OK)
        //    {
        //        try
        //        {
        //            var db_model = bll.FilterToDBModel(model);
        //            bll.db.Events.Add(db_model);
        //            bll.db.SaveChanges();
        //            CreateAlertMessage(AlertMessageType.Success, "Success", "Event is successfully added.");
        //            return RedirectToAction("Index", new { id = model.FK_Vehicle });
        //        }
        //        catch (Exception exception)
        //        {
        //            CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
        //        }
        //    }
        //    else
        //    {
        //        CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", modelValidator);
        //    }
        //    ViewBag.model = model;
        //    ViewBag.EventTypes = new SelectList(bll.db.EventTypes.Where(c => c.IsDeleted == false), "PK_EventType", "Title", model.FK_EventType);
        //    ViewBag.YesNoDict = new SelectList(YesNoDict, "Key", "Value", model.IsAlertable);
        //    return View();
        //}

        //public ActionResult Edit(Guid id)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var model = bll.db.Events.Find(id);
        //    ViewBag.model = model;
        //    ViewBag.EventDocuments = bll.db.EventDocuments.Where(c => c.IsDeleted == false && c.FK_Event == id).ToList();
        //    ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);
        //    ViewBag.EventTypes = new SelectList(bll.db.EventTypes.Where(c => c.IsDeleted == false).OrderBy(m => m.Title), "PK_EventType", "Title", model.FK_EventType);
        //    ViewBag.OtherEventTypeDetailDict = new SelectList(OtherEventTypeDetailDict.OrderBy(m => m.Value), "Key", "Value", model.OtherEventTypeDetail);
        //    ViewBag.YesNoDict = new SelectList(YesNoDict, "Key", "Value", model.IsAlertable);
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Edit(FormCollection formCollection, List<HttpPostedFileBase> ImageFiles)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }

        //    try
        //    {
        //        //# Event
        //        var eventModel = bll.db.Events.Find(Guid.Parse(formCollection["PK_Event"]));
        //        eventModel.UpdatedAt = DateTime.Now;
        //        eventModel.FK_UpdatedByUser = CommonClass.GetCurrentUser().PK_User;


        //        eventModel.FK_Vehicle = Guid.Parse(formCollection["FK_Vehicle"]);
        //        eventModel.FK_EventType = formCollection["FK_EventType"];
        //        eventModel.IssueDate = Convert.ToDateTime(formCollection["IssueDate"]);

        //        if (formCollection["PrimaryAmount"] != "")
        //        {
        //            eventModel.PrimaryAmount = Convert.ToDecimal(formCollection["PrimaryAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.PrimaryAmount = null;
        //        }

        //        if (eventModel.FK_EventType == "fitness_paper" && formCollection["AdvancedIncomeTax"] != "")
        //        {
        //            eventModel.AdvancedIncomeTax = Convert.ToDecimal(formCollection["AdvancedIncomeTax"]);
        //        }
        //        else
        //        {
        //            eventModel.AdvancedIncomeTax = null;
        //        }

        //        if (eventModel.FK_EventType == "insurance" && formCollection["PremiumAmount"] != "")
        //        {
        //            eventModel.PremiumAmount = Convert.ToDecimal(formCollection["PremiumAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.PremiumAmount = null;
        //        }

        //        if (eventModel.FK_EventType == "others")
        //        {
        //            eventModel.OtherEventTypeDetail = formCollection["OtherEventTypeDetail"];
        //            if (eventModel.OtherEventTypeDetail == "Registration" && formCollection["DigitalRegistrationFee"] != "")
        //            {
        //                eventModel.DigitalRegistrationFee = Convert.ToDecimal(formCollection["DigitalRegistrationFee"]);
        //            }
        //            else
        //            {
        //                eventModel.DigitalRegistrationFee = null;
        //            }

        //            if (eventModel.OtherEventTypeDetail == "Registration" && formCollection["HirePurchase"] != "")
        //            {
        //                eventModel.HirePurchase = Convert.ToDecimal(formCollection["HirePurchase"]);
        //            }
        //            else
        //            {
        //                eventModel.DigitalRegistrationFee = null;
        //            }

        //        }


        //        if (formCollection["FineAmount"] != "")
        //        {
        //            eventModel.FineAmount = Convert.ToDecimal(formCollection["FineAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.FineAmount = null;
        //        }

        //        if (eventModel.FK_EventType == "tax_token" && formCollection["AdditionalAmount"] != "")
        //        {
        //            eventModel.AdditionalAmount = Convert.ToDecimal(formCollection["AdditionalAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.AdditionalAmount = null;
        //        }

        //        if (formCollection["OtherAmount"] != "")
        //        {
        //            eventModel.OtherAmount = Convert.ToDecimal(formCollection["OtherAmount"]);
        //            eventModel.OtherNote = formCollection["OtherNote"];
        //        }
        //        else
        //        {
        //            eventModel.OtherAmount = null;
        //            eventModel.OtherNote = null;
        //        }

        //        if (formCollection["TotalAmount"] != "")
        //        {
        //            eventModel.TotalAmount = Convert.ToDecimal(formCollection["TotalAmount"]);
        //        }
        //        else
        //        {
        //            eventModel.TotalAmount = null;
        //        }

        //        // alert
        //        if (eventModel.FK_EventType != "others")
        //        {
        //            var eventType = bll.db.EventTypes.Where(m => m.PK_EventType == eventModel.FK_EventType).FirstOrDefault();
        //            eventModel.ExpirationDate = eventModel.IssueDate.AddDays(eventType.NextCycleDays);
        //            eventModel.AlertDate = eventModel.IssueDate.AddDays(eventType.NextCycleDays - eventType.AlertBeforeNextCycleDays);
        //            eventModel.IsAlertable = true;
        //            eventModel.AlertOn = true;
        //        }
        //        //# for Others
        //        else if (formCollection["ExpirationDate"] != "")
        //        {
        //            eventModel.OtherEventTypeDetail = formCollection["OtherEventTypeDetail"];
        //            eventModel.ExpirationDate = Convert.ToDateTime(formCollection["ExpirationDate"]);
        //            eventModel.AlertDate = Convert.ToDateTime(formCollection["ExpirationDate"]).AddDays(-10);
        //            eventModel.IsAlertable = true;
        //            eventModel.AlertOn = true;
        //        }
        //        else
        //        {
        //            eventModel.OtherEventTypeDetail = formCollection["OtherEventTypeDetail"];
        //            eventModel.ExpirationDate = eventModel.IssueDate;
        //            eventModel.AlertDate = null;
        //            eventModel.IsAlertable = false;
        //            eventModel.AlertOn = false;
        //        }


        //        //# Event Doc
        //        //# create folder
        //        var _EventTypeTitle = bll.db.EventTypes.Find(eventModel.FK_EventType).Title;
        //        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + eventModel.FK_Vehicle + "/" + _EventTypeTitle + "/";
        //        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
        //        if (!Directory.Exists(physicalFolderPath))
        //        {
        //            Directory.CreateDirectory(physicalFolderPath);
        //        }

        //        string rowCount = formCollection["rowCount"];
        //        int totalDocument = Convert.ToInt32(formCollection["rowCount"]);
        //        for (int i = 0; i < totalDocument; i++)
        //        {
        //            var eventDocument = new EventDocument();

        //            eventDocument.PK_EventDocument = Guid.NewGuid();
        //            eventDocument.IsDeleted = false;

        //            eventDocument.CreatedAt = DateTime.Now;
        //            eventDocument.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;

        //            eventDocument.FK_Event = eventModel.PK_Event;
        //            eventDocument.Title = formCollection["Title_" + i];
        //            eventDocument.IdentitficaitonKey = formCollection["IdentitficaitonKey_" + i];
        //            eventDocument.IdentitficaitonValue = formCollection["IdentitficaitonValue_" + i];

        //            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " " + i + " " + eventDocument.PK_EventDocument + "." + ImageFiles[i].FileName.Split('.').Last();
        //            ImageFiles[i].SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

        //            eventDocument.ImageLocation = virtualFilePath;

        //            bll.db.EventDocuments.Add(eventDocument);
        //        }
        //        bll.db.SaveChanges();
        //        CreateAlertMessage(AlertMessageType.Success, "Success", "Event is successfully updated.");
        //    }
        //    catch (Exception exception)
        //    {
        //        CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
        //    }
        //    return RedirectToAction("Index");
        //}

        public ActionResult Delete(Guid id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Events.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;

                        model.DeletedAt = DateTime.Now;
                        model.FK_DeletedByUser = CommonClass.GetCurrentUser().PK_User;

                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Event is successfully deleted.");
                        return RedirectToAction("Index", new { id = model.FK_Vehicle });
                    }
                    catch (Exception exception)
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                        return RedirectToAction("Index", new { id = model.FK_Vehicle });
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        public ActionResult EventReportInDateRange(DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var vehicleWiseGroupedDictList = new List<Dictionary<string, string>>();
            if (StartingDate != null && EndingDate != null)
            {
                ViewBag.StartingDate = CommonClass.ConvertToDateString(StartingDate);
                ViewBag.EndingDate = CommonClass.ConvertToDateString(EndingDate);
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                var accessiblevehicles = bll.db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false).Where(m => accessibleDepoes.Contains(m.FK_Depo)).Select(c => c.PK_Vehicle).ToList();
                var eventList = bll.db.Events.Where(m => m.IsDeleted != true && accessibleDepoes.Contains(m.Vehicle.FK_Depo) && m.IssueDate >= StartingDate && m.IssueDate <= EndingDate).ToList();
                var vehicleWiseGroupedList = eventList.GroupBy(m => m.FK_Vehicle).Select(g => new
                {
                    g.Key,
                    g.FirstOrDefault().Vehicle.RegistrationNumber,

                    tax_token_TotalPrimaryAmount = g.Where(m => m.FK_EventType == "tax_token").Any() ? g.Where(m => m.FK_EventType == "tax_token").Select(m => m.PrimaryAmount).Sum() : 0,

                    fitness_paper_TotalPrimaryAmount = g.Where(m => m.FK_EventType == "fitness_paper").Any() ? g.Where(m => m.FK_EventType == "fitness_paper").Select(m => m.PrimaryAmount).Sum() : 0,
                    fitness_paper_TotalOtherAmount = g.Where(m => m.FK_EventType == "fitness_paper").Any() ? g.Where(m => m.FK_EventType == "fitness_paper").Select(m => m.OtherAmount).Sum() : 0,

                    route_permit_TotalPrimaryAmount = g.Where(m => m.FK_EventType == "route_permit").Any() ? g.Where(m => m.FK_EventType == "route_permit").Select(m => m.PrimaryAmount).Sum() : 0,
                    route_permit_TotalOtherAmount = g.Where(m => m.FK_EventType == "route_permit").Any() ? g.Where(m => m.FK_EventType == "route_permit").Select(m => m.OtherAmount).Sum() : 0,

                    Registration_TotalPrimaryAmount = g.Where(m => m.OtherEventTypeDetail == "Registration").Any() ? g.Where(m => m.OtherEventTypeDetail == "Registration").Select(m => m.PrimaryAmount).Sum() : 0,
                    Registration_TotalOtherAmount = g.Where(m => m.OtherEventTypeDetail == "Registration").Any() ? g.Where(m => m.OtherEventTypeDetail == "Registration").Select(m => m.OtherAmount).Sum() : 0,

                    fitness_paper_TotalFitnessPaper_AdvancedIncomeTax = g.Where(m => m.FK_EventType == "fitness_paper").Any() ? g.Where(m => m.FK_EventType == "fitness_paper").Select(m => m.AdvancedIncomeTax).Sum() : 0,

                    DepoName = g.FirstOrDefault().Vehicle.Depo != null ? g.FirstOrDefault().Vehicle.Depo.Name : "",
                    UserCompanyName = g.FirstOrDefault().Vehicle.Company != null ? g.FirstOrDefault().Vehicle.Company.Name : ""
                }).ToList();


                foreach (var item in vehicleWiseGroupedList)
                {
                    vehicleWiseGroupedDictList.Add(
                        new Dictionary<string, string>()
                        {
                        { "Key",item.Key.ToString()},
                        { "RegistrationNumber",item.RegistrationNumber},

                        { "tax_token_TotalPrimaryAmount",item.tax_token_TotalPrimaryAmount.ToString()},

                        { "fitness_paper_TotalPrimaryAmount",item.fitness_paper_TotalPrimaryAmount.ToString()},
                        { "fitness_paper_TotalOtherAmount",item.fitness_paper_TotalOtherAmount.ToString()},

                        { "route_permit_TotalPrimaryAmount",item.route_permit_TotalPrimaryAmount.ToString()},
                        { "route_permit_TotalOtherAmount",item.route_permit_TotalOtherAmount.ToString()},

                        { "Registration_TotalPrimaryAmount",item.Registration_TotalPrimaryAmount.ToString()},
                        { "Registration_TotalOtherAmount",item.Registration_TotalOtherAmount.ToString()},

                        { "fitness_paper_TotalFitnessPaper_AdvancedIncomeTax",item.fitness_paper_TotalFitnessPaper_AdvancedIncomeTax.ToString()},

                        { "DepoName",item.DepoName},
                        { "UserCompanyName",item.UserCompanyName},
                        }
                        );
                }
                return View(vehicleWiseGroupedDictList);
            }
            return View(vehicleWiseGroupedDictList);
        }
        public ActionResult EventReportInDateRangeWithDepositDate(DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var vehicleWiseGroupedDictList = new List<Dictionary<string, string>>();
            if (StartingDate != null && EndingDate != null)
            {
                ViewBag.StartingDate = CommonClass.ConvertToDateString(StartingDate);
                ViewBag.EndingDate = CommonClass.ConvertToDateString(EndingDate);
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                var accessiblevehicles = bll.db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false).Where(m => accessibleDepoes.Contains(m.FK_Depo)).Select(c => c.PK_Vehicle).ToList();
                var eventList = bll.db.Events.Where(m => m.IsDeleted != true && accessibleDepoes.Contains(m.Vehicle.FK_Depo) && m.IssueDate >= StartingDate && m.IssueDate <= EndingDate).ToList();
                var vehicleWiseGroupedList = eventList.GroupBy(m => m.FK_Vehicle).Select(g => new
                {
                    g.Key,
                    g.FirstOrDefault().Vehicle.RegistrationNumber,

                    tax_token_TotalPrimaryAmount = g.Where(m => m.FK_EventType == "tax_token").Any() ? g.Where(m => m.FK_EventType == "tax_token").Select(m => m.PrimaryAmount).Sum() : 0,
                    tax_token_DepositDate = g.Where(m => m.FK_EventType == "tax_token").Any() ? g.Where(m => m.FK_EventType == "tax_token").OrderByDescending(m => m.IssueDate).Select(m => CommonClass.ConvertToDateString(m.DepositDate)).FirstOrDefault() : "",

                    fitness_paper_TotalPrimaryAmount = g.Where(m => m.FK_EventType == "fitness_paper").Any() ? g.Where(m => m.FK_EventType == "fitness_paper").Select(m => m.PrimaryAmount).Sum() : 0,
                    fitness_paper_TotalOtherAmount = g.Where(m => m.FK_EventType == "fitness_paper").Any() ? g.Where(m => m.FK_EventType == "fitness_paper").Select(m => m.OtherAmount).Sum() : 0,
                    fitness_paper_DepositDate = g.Where(m => m.FK_EventType == "fitness_paper").Any() ? g.Where(m => m.FK_EventType == "fitness_paper").OrderByDescending(m => m.IssueDate).Select(m => CommonClass.ConvertToDateString(m.DepositDate)).FirstOrDefault() : "",

                    route_permit_TotalPrimaryAmount = g.Where(m => m.FK_EventType == "route_permit").Any() ? g.Where(m => m.FK_EventType == "route_permit").Select(m => m.PrimaryAmount).Sum() : 0,
                    route_permit_TotalOtherAmount = g.Where(m => m.FK_EventType == "route_permit").Any() ? g.Where(m => m.FK_EventType == "route_permit").Select(m => m.OtherAmount).Sum() : 0,
                    route_permit_DepositDate = g.Where(m => m.FK_EventType == "route_permit").Any() ? g.Where(m => m.FK_EventType == "route_permit").OrderByDescending(m => m.IssueDate).Select(m => CommonClass.ConvertToDateString(m.DepositDate)).FirstOrDefault() : "",

                    Registration_TotalPrimaryAmount = g.Where(m => m.OtherEventTypeDetail == "Registration").Any() ? g.Where(m => m.OtherEventTypeDetail == "Registration").Select(m => m.PrimaryAmount).Sum() : 0,
                    Registration_TotalOtherAmount = g.Where(m => m.OtherEventTypeDetail == "Registration").Any() ? g.Where(m => m.OtherEventTypeDetail == "Registration").Select(m => m.OtherAmount).Sum() : 0,
                    Registration_DepositDate = g.Where(m => m.OtherEventTypeDetail == "Registration").Any() ? g.Where(m => m.OtherEventTypeDetail == "Registration").OrderByDescending(m => m.IssueDate).Select(m => CommonClass.ConvertToDateString(m.DepositDate)).FirstOrDefault() : "",

                    fitness_paper_TotalFitnessPaper_AdvancedIncomeTax = g.Where(m => m.FK_EventType == "fitness_paper").Any() ? g.Where(m => m.FK_EventType == "fitness_paper").Select(m => m.AdvancedIncomeTax).Sum() : 0,

                    DepoName = g.FirstOrDefault().Vehicle.Depo != null ? g.FirstOrDefault().Vehicle.Depo.Name : "",
                    UserCompanyName = g.FirstOrDefault().Vehicle.Company != null ? g.FirstOrDefault().Vehicle.Company.Name : ""
                }).ToList();


                foreach (var item in vehicleWiseGroupedList)
                {
                    vehicleWiseGroupedDictList.Add(
                        new Dictionary<string, string>()
                        {
                        { "Key",item.Key.ToString()},
                        { "RegistrationNumber",item.RegistrationNumber},

                        { "tax_token_TotalPrimaryAmount",item.tax_token_TotalPrimaryAmount.ToString()},
                        { "tax_token_DepositDate",item.tax_token_DepositDate.ToString()},

                        { "fitness_paper_TotalPrimaryAmount",item.fitness_paper_TotalPrimaryAmount.ToString()},
                        { "fitness_paper_TotalOtherAmount",item.fitness_paper_TotalOtherAmount.ToString()},
                        { "fitness_paper_DepositDate",item.fitness_paper_DepositDate.ToString()},

                        { "route_permit_TotalPrimaryAmount",item.route_permit_TotalPrimaryAmount.ToString()},
                        { "route_permit_TotalOtherAmount",item.route_permit_TotalOtherAmount.ToString()},
                        { "route_permit_DepositDate",item.route_permit_DepositDate.ToString()},

                        { "Registration_TotalPrimaryAmount",item.Registration_TotalPrimaryAmount.ToString()},
                        { "Registration_TotalOtherAmount",item.Registration_TotalOtherAmount.ToString()},
                        { "Registration_DepositDate",item.Registration_DepositDate.ToString()},

                        { "fitness_paper_TotalFitnessPaper_AdvancedIncomeTax",item.fitness_paper_TotalFitnessPaper_AdvancedIncomeTax.ToString()},

                        { "DepoName",item.DepoName},
                        { "UserCompanyName",item.UserCompanyName},
                        }
                        );
                }
                return View(vehicleWiseGroupedDictList);
            }
            return View(vehicleWiseGroupedDictList);
        }

        public ActionResult InsuranceReportInDateRange(DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<Event>();
            if (StartingDate != null && EndingDate != null)
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", EndingDate);
            }
            return View(list);
        }
        public ActionResult InsuranceReportInDateRange_SummuryView(DateTime? StartingDate, DateTime? EndingDate)
        {
            var list = new List<Event>();
            if (StartingDate != null && EndingDate != null)
            {
                ViewBag.StartingDate_View = CommonClass.ConvertToDateString(StartingDate);
                ViewBag.EndingDate_View = CommonClass.ConvertToDateString(EndingDate);
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                var accessiblevehicles = bll.db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false).Where(m => accessibleDepoes.Contains(m.FK_Depo)).Select(c => c.PK_Vehicle).ToList();
                list = bll.db.Events.Where(m => accessiblevehicles.Contains(m.FK_Vehicle) && m.FK_EventType == "insurance" && m.IssueDate >= StartingDate && m.IssueDate <= EndingDate && m.TotalAmount != null).ToList();
            }
            return View(list);
        }
        public ActionResult InsuranceReportInDateRange_DetailView(DateTime? StartingDate, DateTime? EndingDate)
        {
            var list = new List<VM_Event>();
            if (StartingDate != null && EndingDate != null)
            {
                ViewBag.StartingDate_View = CommonClass.ConvertToDateString(StartingDate);
                ViewBag.EndingDate_View = CommonClass.ConvertToDateString(EndingDate);
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                var accessiblevehicles = bll.db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false).Where(m => accessibleDepoes.Contains(m.FK_Depo)).Select(c => c.PK_Vehicle).ToList();
                var _list = bll.db.Events.Where(m => accessiblevehicles.Contains(m.FK_Vehicle) && m.FK_EventType == "insurance" && m.IssueDate >= StartingDate && m.IssueDate <= EndingDate && m.TotalAmount != null).ToList();
                list = _list.Select(m => bll.ConvertToViewModel(m)).ToList();
            }
            return View(list);
        }

        //# AJAX METHOD
        public JsonResult GetEventsBy_FK_Vehicle_and_FK_EventType(Guid FK_Vehicle, String FK_EventType, Guid? PK_Event)
        {
            var list = (from eve in bll.db.Events.AsEnumerable()
                        where eve.FK_Vehicle == FK_Vehicle && eve.FK_EventType == FK_EventType && eve.PK_Event != PK_Event
                        select new
                        {
                            eve.PK_Event,
                            EventType_Title = eve.EventType.Title,
                            IssueDate = eve.IssueDate != null ? CommonClass.ConvertToDateString(eve.IssueDate) : "",
                            ExpirationDate = eve.ExpirationDate != null ? CommonClass.ConvertToDateString(eve.ExpirationDate) : "",
                        }
                       ).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEventTypeDetail(string PK_EventType)
        {
            var item = bll.db.EventTypes.Where(m => m.PK_EventType == PK_EventType).Select(m => m).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        #region Mail Event ALert
        public string TrySendEventAlertEmail()
        {
            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendEventAlertEmail-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();

            var today = DateTime.Now.Date;
            var mailRequestTIme = DateTime.Now;
            var mails = bll.db.AlertEmails.Where(m => m.IsDeleted != true && (m.EventAlert_1 == true || m.EventAlert_2 == true || m.EventAlert_3 == true)).ToList();
            var _total = mails.Count;
            var _sent = 0;
            foreach (var mail in mails)
            {
                string mailSubject = "";
                if (mail.EventAlert_1 == true)
                {
                    mailSubject = "Vehicle event alert";
                    var Depo_PKs = bll.db.AlertEmailAttachedDepoes.Where(m => m.FK_AlertEmail == mail.PK_AlertEmail && m.IsAttachable == true).Select(m => m.FK_Depo).ToList();
                    var hasAny = (from eve in bll.db.Events.AsEnumerable().Where(e => e.FK_EventType != "insurance" && e.IsAlertable == true && e.AlertOn == true && today.AddDays(10) < e.ExpirationDate)
                                  join vehicle in bll.db.Vehicles.Where(v => Depo_PKs.Contains(v.FK_Depo)) on eve.FK_Vehicle equals vehicle.PK_Vehicle
                                  select new
                                  {
                                  }).Any();
                    if (hasAny == false)
                    {
                        continue;
                    }
                    else
                    {
                        string url = "";
#if DEBUG
                        url = ConfigurationManager.AppSettings["DEBUG_DOMAIN"];
#else
url = ConfigurationManager.AppSettings["LIVE_DOMAIN"];
#endif
                        url = url + @"Event/EventAlertEmailBodyGenerator?PK_AlertEmail=" + mail.PK_AlertEmail + "&EventAlertLevel=1";
                        WebClient myWebClient = new WebClient();
                        byte[] myDataBuffer = myWebClient.DownloadData(url);
                        string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                        try
                        {
                            SendMail_Single(mail.MailAddress, mailSubject, mailBody_HTML);
                            _sent++;
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
                                CallingMessage = "TrySendEventAlertEmail-Error" + guid,
                                UserDefinedMessage = errrorMessage
                            }
                            );
                            bll.db.SaveChanges();
                        }

                    }

                }
                if (mail.EventAlert_2 == true)
                {
                    mailSubject = "Urgent vehicle event alert";
                    var Depo_PKs = bll.db.AlertEmailAttachedDepoes.Where(m => m.FK_AlertEmail == mail.PK_AlertEmail && m.IsAttachable == true).Select(m => m.FK_Depo).ToList();
                    var hasAny = (from eve in bll.db.Events.AsEnumerable().Where(e => e.FK_EventType != "insurance" && e.IsAlertable == true && e.AlertOn == true && today.AddDays(7) >= e.ExpirationDate && today < e.ExpirationDate)
                                  join vehicle in bll.db.Vehicles.Where(v => Depo_PKs.Contains(v.FK_Depo)) on eve.FK_Vehicle equals vehicle.PK_Vehicle
                                  select new
                                  {
                                  }).Any();
                    if (hasAny == false)
                    {
                        continue;
                    }
                    else
                    {
                        string url = "";
#if DEBUG
                        url = ConfigurationManager.AppSettings["DEBUG_DOMAIN"];
#else
url = ConfigurationManager.AppSettings["LIVE_DOMAIN"];
#endif
                        url = url + @"Event/EventAlertEmailBodyGenerator?PK_AlertEmail=" + mail.PK_AlertEmail + "&EventAlertLevel=2";
                        WebClient myWebClient = new WebClient();
                        byte[] myDataBuffer = myWebClient.DownloadData(url);
                        string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                        try
                        {
                            SendMail_Single(mail.MailAddress, mailSubject, mailBody_HTML);
                            _sent++;
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
                                CallingMessage = "TrySendEventAlertEmail-Error" + guid,
                                UserDefinedMessage = errrorMessage
                            }
                            );
                            bll.db.SaveChanges();
                        }

                    }

                }
                if (mail.EventAlert_3 == true)
                {
                    mailSubject = "Expiration of vehicle event alert";
                    var Depo_PKs = bll.db.AlertEmailAttachedDepoes.Where(m => m.FK_AlertEmail == mail.PK_AlertEmail && m.IsAttachable == true).Select(m => m.FK_Depo).ToList();
                    var hasAny = (from eve in bll.db.Events.AsEnumerable().Where(e => e.FK_EventType != "insurance" && e.IsAlertable == true && e.AlertOn == true && today >= e.ExpirationDate)
                                  join vehicle in bll.db.Vehicles.Where(v => Depo_PKs.Contains(v.FK_Depo)) on eve.FK_Vehicle equals vehicle.PK_Vehicle
                                  select new
                                  {
                                  }).Any();
                    if (hasAny == false)
                    {
                        continue;
                    }
                    else
                    {
                        string url = "";
#if DEBUG
                        url = ConfigurationManager.AppSettings["DEBUG_DOMAIN"];
#else
url = ConfigurationManager.AppSettings["LIVE_DOMAIN"];
#endif
                        url = url + @"Event/EventAlertEmailBodyGenerator?PK_AlertEmail=" + mail.PK_AlertEmail + "&EventAlertLevel=3";
                        WebClient myWebClient = new WebClient();
                        byte[] myDataBuffer = myWebClient.DownloadData(url);
                        string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                        try
                        {
                            SendMail_Single(mail.MailAddress, mailSubject, mailBody_HTML);
                            _sent++;
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
                                CallingMessage = "TrySendEventAlertEmail-Error" + guid,
                                UserDefinedMessage = errrorMessage
                            }
                            );
                            bll.db.SaveChanges();
                        }

                    }
                }
                else
                {
                    continue;
                }

            }

            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendEventAlertEmail-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = "total:" + _total + " sent:" + _sent
                  }
                  );
            bll.db.SaveChanges();
            return "Sent total:" + _total + " sent:" + _sent;
        }
        public ActionResult EventAlertEmailBodyGenerator(Guid PK_AlertEmail, int EventAlertLevel)
        {
            var today = DateTime.Now.Date;
            var mail = bll.db.AlertEmails.Where(m => m.PK_AlertEmail == PK_AlertEmail).FirstOrDefault();
            var eventAlertEmailRowList = new List<EventAlertEmailRow>();

            if (EventAlertLevel == 1)
            {
                ViewBag.Message_1 = "The list of vehicle document will be expired soon (Within 10 days). Please, take necessary action.";
                var Depo_PKs = bll.db.AlertEmailAttachedDepoes.Where(m => m.FK_AlertEmail == mail.PK_AlertEmail && m.IsAttachable == true).Select(m => m.FK_Depo).ToList();
                var list = (from eve in bll.db.Events.AsEnumerable().Where(e => e.FK_EventType != "insurance" && e.IsAlertable == true && e.AlertOn == true && today.AddDays(10) >= e.ExpirationDate && today < e.ExpirationDate)
                            join vehicle in bll.db.Vehicles.Where(v => Depo_PKs.Contains(v.FK_Depo)) on eve.FK_Vehicle equals vehicle.PK_Vehicle
                            select new
                            {
                                vehicle_Reg = vehicle.RegistrationNumber,
                                vehicle_Depo = vehicle.Depo.Name,
                                event_type = eve.FK_EventType,
                                event_expirationDate = CommonClass.ConvertToDateString(eve.ExpirationDate),
                            }).OrderByDescending(m => m.event_expirationDate).ToList();

                foreach (var item in list)
                {
                    var row = eventAlertEmailRowList.Where(m => m.vehicle_Reg == item.vehicle_Reg).FirstOrDefault();
                    if (row == null)
                    {
                        row = new EventAlertEmailRow();
                        row.vehicle_Reg = item.vehicle_Reg;
                        row.vehicle_Depo = item.vehicle_Depo;

                        row.fitness_paper_expirationDate = "X";
                        row.insurance_expirationDate = "X";
                        row.route_permit_expirationDate = "X";
                        row.tax_token_expirationDate = "X";

                        if (item.event_type == "fitness_paper")
                        {
                            row.fitness_paper_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "insurance")
                        {
                            row.insurance_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "route_permit")
                        {
                            row.route_permit_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "tax_token")
                        {
                            row.tax_token_expirationDate = item.event_expirationDate;
                        }
                        eventAlertEmailRowList.Add(row);
                    }
                    else
                    {
                        if (item.event_type == "fitness_paper")
                        {
                            row.fitness_paper_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "insurance")
                        {
                            row.insurance_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "route_permit")
                        {
                            row.route_permit_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "tax_token")
                        {
                            row.tax_token_expirationDate = item.event_expirationDate;
                        }
                    }
                }
                return View(eventAlertEmailRowList.OrderBy(m => m.vehicle_Depo));
            }
            else if (EventAlertLevel == 2)
            {
                ViewBag.Message_1 = "Within 7 days vehicle document will be expired. Please, take necessary action.";
                var Depo_PKs = bll.db.AlertEmailAttachedDepoes.Where(m => m.FK_AlertEmail == mail.PK_AlertEmail && m.IsAttachable == true).Select(m => m.FK_Depo).ToList();
                var list = (from eve in bll.db.Events.AsEnumerable().Where(e => e.FK_EventType != "insurance" && e.IsAlertable == true && e.AlertOn == true && today.AddDays(7) >= e.ExpirationDate && today < e.ExpirationDate)
                            join vehicle in bll.db.Vehicles.Where(v => Depo_PKs.Contains(v.FK_Depo)) on eve.FK_Vehicle equals vehicle.PK_Vehicle
                            select new
                            {
                                vehicle_Reg = vehicle.RegistrationNumber,
                                vehicle_Depo = vehicle.Depo.Name,
                                event_type = eve.FK_EventType,
                                event_expirationDate = CommonClass.ConvertToDateString(eve.ExpirationDate),
                            }).OrderByDescending(m => m.event_expirationDate).ToList();
                foreach (var item in list)
                {
                    var row = eventAlertEmailRowList.Where(m => m.vehicle_Reg == item.vehicle_Reg).FirstOrDefault();
                    if (row == null)
                    {
                        row = new EventAlertEmailRow();
                        row.vehicle_Reg = item.vehicle_Reg;
                        row.vehicle_Depo = item.vehicle_Depo;

                        row.fitness_paper_expirationDate = "X";
                        row.insurance_expirationDate = "X";
                        row.route_permit_expirationDate = "X";
                        row.tax_token_expirationDate = "X";

                        if (item.event_type == "fitness_paper")
                        {
                            row.fitness_paper_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "insurance")
                        {
                            row.insurance_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "route_permit")
                        {
                            row.route_permit_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "tax_token")
                        {
                            row.tax_token_expirationDate = item.event_expirationDate;
                        }
                        eventAlertEmailRowList.Add(row);
                    }
                    else
                    {
                        if (item.event_type == "fitness_paper")
                        {
                            row.fitness_paper_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "insurance")
                        {
                            row.insurance_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "route_permit")
                        {
                            row.route_permit_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "tax_token")
                        {
                            row.tax_token_expirationDate = item.event_expirationDate;
                        }
                    }
                }
                return View(eventAlertEmailRowList.OrderBy(m => m.vehicle_Depo));
            }
            else if (EventAlertLevel == 3)
            {
                ViewBag.Message_1 = "List of expired vehicle documents.";
                var Depo_PKs = bll.db.AlertEmailAttachedDepoes.Where(m => m.FK_AlertEmail == mail.PK_AlertEmail && m.IsAttachable == true).Select(m => m.FK_Depo).ToList();
                var list = (from eve in bll.db.Events.AsEnumerable().Where(e => e.FK_EventType != "insurance" && e.IsAlertable == true && e.AlertOn == true && today >= e.ExpirationDate)
                            join vehicle in bll.db.Vehicles.Where(v => Depo_PKs.Contains(v.FK_Depo)) on eve.FK_Vehicle equals vehicle.PK_Vehicle
                            select new
                            {
                                vehicle_Reg = vehicle.RegistrationNumber,
                                vehicle_Depo = vehicle.Depo.Name,
                                event_type = eve.FK_EventType,
                                event_expirationDate = CommonClass.ConvertToDateString(eve.ExpirationDate),
                            }).OrderByDescending(m => m.event_expirationDate).ToList();
                foreach (var item in list)
                {
                    var row = eventAlertEmailRowList.Where(m => m.vehicle_Reg == item.vehicle_Reg).FirstOrDefault();
                    if (row == null)
                    {
                        row = new EventAlertEmailRow();
                        row.vehicle_Reg = item.vehicle_Reg;
                        row.vehicle_Depo = item.vehicle_Depo;

                        row.fitness_paper_expirationDate = "X";
                        row.insurance_expirationDate = "X";
                        row.route_permit_expirationDate = "X";
                        row.tax_token_expirationDate = "X";

                        if (item.event_type == "fitness_paper")
                        {
                            row.fitness_paper_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "insurance")
                        {
                            row.insurance_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "route_permit")
                        {
                            row.route_permit_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "tax_token")
                        {
                            row.tax_token_expirationDate = item.event_expirationDate;
                        }
                        eventAlertEmailRowList.Add(row);
                    }
                    else
                    {
                        if (item.event_type == "fitness_paper")
                        {
                            row.fitness_paper_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "insurance")
                        {
                            row.insurance_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "route_permit")
                        {
                            row.route_permit_expirationDate = item.event_expirationDate;
                        }
                        else if (item.event_type == "tax_token")
                        {
                            row.tax_token_expirationDate = item.event_expirationDate;
                        }
                    }
                }
                return View(eventAlertEmailRowList.OrderBy(m => m.vehicle_Depo));
            }
            else
            {
                ViewBag.Message_1 = "N/A";
                return View(eventAlertEmailRowList);
            }
        }
        public class EventAlertEmailRow
        {
            public string vehicle_Reg { get; set; }
            public string vehicle_Depo { get; set; }
            public string fitness_paper_expirationDate { get; set; }
            public string insurance_expirationDate { get; set; }
            public string route_permit_expirationDate { get; set; }
            public string tax_token_expirationDate { get; set; }
        }
        #endregion

    }
}