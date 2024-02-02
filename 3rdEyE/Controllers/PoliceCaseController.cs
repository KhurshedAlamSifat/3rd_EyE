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
using System.Collections.Specialized;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Configuration;

namespace _3rdEyE.Controllers
{
    public class PoliceCaseController : BaseController
    {

        BLL_PoliceCase bll = new BLL_PoliceCase();
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult PoliceCaseDashBoard()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }
        public ActionResult PoliceCaseDashBoard2()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }
        public JsonResult GetPoliceCaseDashBoardData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = int.MaxValue;
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            cmd.Connection.Open();
            var StartingDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var EndingDate = StartingDate.AddMonths(1).AddDays(-1);
            //var PointOfLawsWithCountTitle = "বেশি সংখ্যক ভঙ্গকৃত আইন সমূহ : সেপ্টেম্বর-২১";
            var PointOfLawsWithCountTitle = "The Most Denyed Laws : " + StartingDate.ToString("MMMM") + "-" + StartingDate.ToString("YYYY");
            #region
            /*
            Delete  from ReadyReport where ReportName = 'PointOfLawsWithCount';
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key_nvarchar],Value1)
            select 'PoliceCase/PoliceCaseDashBoard','PointOfLawsWithCount','PRAN',CONVERT(NVARCHAR(300),PoliceCaseLaw.LawDetail), count(*) as value1 from PoliceCase
            join PoliceCase_PoliceCaseLaw on PoliceCase_PoliceCaseLaw.FK_PoliceCase = PoliceCase.PK_PoliceCase
            join PoliceCaseLaw on PoliceCaseLaw.PK_PoliceCaseLaw = PoliceCase_PoliceCaseLaw.FK_PoliceCaseLaw

            join Vehicle on Vehicle.PK_Vehicle = PoliceCase.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            where 
            1=1
            AND Depo.PRG_Type = 'PRAN'
			AND PoliceCaseLaw.PK_PoliceCaseLaw != '0'
            AND PoliceCase.IssueDate >= '2022-05-01' and PoliceCase.IssueDate < '2022-06-01'
            GROUP BY PoliceCaseLaw.LawDetail
            order by value1 desc;

			insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key_nvarchar],Value1)
            select 'PoliceCase/PoliceCaseDashBoard','PointOfLawsWithCount','RFL',CONVERT(NVARCHAR(300),PoliceCaseLaw.LawDetail), count(*) as value1 from PoliceCase
            join PoliceCase_PoliceCaseLaw on PoliceCase_PoliceCaseLaw.FK_PoliceCase = PoliceCase.PK_PoliceCase
            join PoliceCaseLaw on PoliceCaseLaw.PK_PoliceCaseLaw = PoliceCase_PoliceCaseLaw.FK_PoliceCaseLaw

            join Vehicle on Vehicle.PK_Vehicle = PoliceCase.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            where 
            1=1
            AND Depo.PRG_Type = 'RFL'
			AND PoliceCaseLaw.PK_PoliceCaseLaw != '0'
            AND PoliceCase.IssueDate >= '2022-05-01' and PoliceCase.IssueDate < '2022-06-01'
            GROUP BY PoliceCaseLaw.LawDetail
            order by value1 desc;

            select * from ReadyReport where ReportName = 'PointOfLawsWithCount';
             */
            var PointOfLawsWithCount_query = @"
            Delete  from ReadyReport where ReportName = 'PointOfLawsWithCount';
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key_nvarchar],Value1)
            select 'PoliceCase/PoliceCaseDashBoard','PointOfLawsWithCount','PRAN',CONVERT(NVARCHAR(300),PoliceCaseLaw.LawDetail), count(*) as value1 from PoliceCase
            join PoliceCase_PoliceCaseLaw on PoliceCase_PoliceCaseLaw.FK_PoliceCase = PoliceCase.PK_PoliceCase
            join PoliceCaseLaw on PoliceCaseLaw.PK_PoliceCaseLaw = PoliceCase_PoliceCaseLaw.FK_PoliceCaseLaw

            join Vehicle on Vehicle.PK_Vehicle = PoliceCase.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            where 
            1=1
            AND Depo.PRG_Type = 'PRAN'
			AND PoliceCaseLaw.PK_PoliceCaseLaw != '0'
            AND PoliceCase.IssueDate >= '2023-12-01' and PoliceCase.IssueDate < '2024-01-01'
            GROUP BY PoliceCaseLaw.LawDetail
            order by value1 desc;

			insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key_nvarchar],Value1)
            select 'PoliceCase/PoliceCaseDashBoard','PointOfLawsWithCount','RFL',CONVERT(NVARCHAR(300),PoliceCaseLaw.LawDetail), count(*) as value1 from PoliceCase
            join PoliceCase_PoliceCaseLaw on PoliceCase_PoliceCaseLaw.FK_PoliceCase = PoliceCase.PK_PoliceCase
            join PoliceCaseLaw on PoliceCaseLaw.PK_PoliceCaseLaw = PoliceCase_PoliceCaseLaw.FK_PoliceCaseLaw

            join Vehicle on Vehicle.PK_Vehicle = PoliceCase.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            where 
            1=1
            AND Depo.PRG_Type = 'RFL'
			AND PoliceCaseLaw.PK_PoliceCaseLaw != '0'
            AND PoliceCase.IssueDate >= '2023-12-01' and PoliceCase.IssueDate < '2024-01-01'
            GROUP BY PoliceCaseLaw.LawDetail
            order by value1 desc;

            select * from ReadyReport where ReportName = 'PointOfLawsWithCount';";

            cmd.CommandText = PointOfLawsWithCount_query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection.CreateCommand();
            cmd.ExecuteNonQuery();
            //con.Open();
            //int rowsAffected = cmd.ExecuteNonQuery();
            //con.Close();

            object PointOfLawsWithCount;
            if (CurrentUser.PRG_Type == "ALL")
            {
                PointOfLawsWithCount = bll.db.ReadyReports.Where(m => m.ReportName == "PointOfLawsWithCount").GroupBy(g => g.Key_nvarchar).Select(m => new
                {
                    Key_nvarchar = m.Key,
                    Value1 = m.Sum(n => n.Value1)
                }).OrderByDescending(m => m.Value1).ToList();
            }
            else
            {
                PointOfLawsWithCount = bll.db.ReadyReports.Where(m => m.ReportName == "PointOfLawsWithCount" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => new
                {
                    m.Key_nvarchar,
                    m.Value1
                }).OrderByDescending(m => m.Value1).ToList();
            }

            #endregion

            //# DoughnutChart
            var DoughnutChartTitle = "With Case vs Without Case : December-2023";
            #region
            /*
            Delete  from ReadyReport where ReportName = 'CasedVsNonCased';
            DECLARE @HasCase BIGINT;
            DECLARE @AllVehicle BIGINT;
            ----------//-------------
            set @HasCase = 0;
            select @HasCase = count(distinct(Vehicle.RegistrationNumber)) 
            from Depo
            join Vehicle on Vehicle.FK_Depo = Depo.PK_Depo
            join PoliceCase on PoliceCase.FK_Vehicle = Vehicle.PK_Vehicle
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'PRAN')
            AND PoliceCase.IssueDate >= '2022-05-01' AND PoliceCase.IssueDate < '2022-06-01';

            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1)
            select 'PoliceCase/PoliceCaseDashBoard','CasedVsNonCased','PRAN','HasCase',@HasCase;

            set @AllVehicle = 0;
            select @AllVehicle = count(distinct(Vehicle.RegistrationNumber)) 
            from Depo
            join Vehicle on Vehicle.FK_Depo = Depo.PK_Depo
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'PRAN');
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1)
            select 'PoliceCase/PoliceCaseDashBoard','CasedVsNonCased','PRAN','HasNotCase',(@AllVehicle-@HasCase);
            ----------//-------------
            set @HasCase = 0;
            select @HasCase = count(distinct(Vehicle.RegistrationNumber)) 
            from Depo
            join Vehicle on Vehicle.FK_Depo = Depo.PK_Depo
            join PoliceCase on PoliceCase.FK_Vehicle = Vehicle.PK_Vehicle
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'RFL')
            AND PoliceCase.IssueDate >= '2022-05-01' AND PoliceCase.IssueDate < '2022-06-01';

            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1)
            select 'PoliceCase/PoliceCaseDashBoard','CasedVsNonCased','RFL','HasCase',@HasCase;

            set @AllVehicle = 0;
            select @AllVehicle = count(distinct(Vehicle.RegistrationNumber)) 
            from Depo
            join Vehicle on Vehicle.FK_Depo = Depo.PK_Depo
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'RFL');
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1)
            select 'PoliceCase/PoliceCaseDashBoard','CasedVsNonCased','RFL','HasNotCase',(@AllVehicle-@HasCase);

            ----------//-------------
            select * from ReadyReport where ReportName = 'CasedVsNonCased';
             */
            var CasedVsNonCased_query = @"
            Delete  from ReadyReport where ReportName = 'CasedVsNonCased';
            DECLARE @HasCase BIGINT;
            DECLARE @AllVehicle BIGINT;
            ----------//-------------
            set @HasCase = 0;
            select @HasCase = count(distinct(Vehicle.RegistrationNumber)) 
            from Depo
            join Vehicle on Vehicle.FK_Depo = Depo.PK_Depo
            join PoliceCase on PoliceCase.FK_Vehicle = Vehicle.PK_Vehicle
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'PRAN')
            AND PoliceCase.IssueDate >= '2023-12-01' AND PoliceCase.IssueDate < '2024-01-01';

            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1)
            select 'PoliceCase/PoliceCaseDashBoard','CasedVsNonCased','PRAN','HasCase',@HasCase;

            set @AllVehicle = 0;
            select @AllVehicle = count(distinct(Vehicle.RegistrationNumber)) 
            from Depo
            join Vehicle on Vehicle.FK_Depo = Depo.PK_Depo
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'PRAN');
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1)
            select 'PoliceCase/PoliceCaseDashBoard','CasedVsNonCased','PRAN','HasNotCase',(@AllVehicle-@HasCase);
            ----------//-------------
            set @HasCase = 0;
            select @HasCase = count(distinct(Vehicle.RegistrationNumber)) 
            from Depo
            join Vehicle on Vehicle.FK_Depo = Depo.PK_Depo
            join PoliceCase on PoliceCase.FK_Vehicle = Vehicle.PK_Vehicle
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'RFL')
            AND PoliceCase.IssueDate >= '2023-12-01' AND PoliceCase.IssueDate < '2024-01-01';

            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1)
            select 'PoliceCase/PoliceCaseDashBoard','CasedVsNonCased','RFL','HasCase',@HasCase;

            set @AllVehicle = 0;
            select @AllVehicle = count(distinct(Vehicle.RegistrationNumber)) 
            from Depo
            join Vehicle on Vehicle.FK_Depo = Depo.PK_Depo
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'RFL');
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1)
            select 'PoliceCase/PoliceCaseDashBoard','CasedVsNonCased','RFL','HasNotCase',(@AllVehicle-@HasCase);

            ----------//-------------
            select * from ReadyReport where ReportName = 'CasedVsNonCased';";
            cmd.CommandText = CasedVsNonCased_query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection.CreateCommand();
            cmd.ExecuteNonQuery();

            object CasedVsNonCased_HasCase;
            object CasedVsNonCased_HasNotCase;
            if (CurrentUser.PRG_Type == "ALL")
            {
                CasedVsNonCased_HasCase = bll.db.ReadyReports.Where(m => m.ReportName == "CasedVsNonCased" && m.Key == "HasCase").Select(m => m.Value1).Sum();
                CasedVsNonCased_HasNotCase = bll.db.ReadyReports.Where(m => m.ReportName == "CasedVsNonCased" && m.Key == "HasNotCase").Select(m => m.Value1).Sum();
            }
            else
            {
                CasedVsNonCased_HasCase = bll.db.ReadyReports.Where(m => m.ReportName == "CasedVsNonCased" && m.Key == "HasCase" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
                CasedVsNonCased_HasNotCase = bll.db.ReadyReports.Where(m => m.ReportName == "CasedVsNonCased" && m.Key == "HasNotCase" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => m.Value1).Sum();
            }
            #endregion

            var DepoWiseExpenseTitle = "Case Expense On December-2023";
            #region
            /*
            delete from ReadyReport where ReportName = 'DepoWiseExpense';
            
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1,Value2)
            select 'PoliceCase/PoliceCaseDashBoard','DepoWiseExpense','PRAN', Depo.Name, sum(PoliceCase.TotalAmount), COUNT(PoliceCase.PK_PoliceCase) as total from PoliceCase
            join Vehicle on Vehicle.PK_Vehicle = PoliceCase.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'PRAN')
            AND PoliceCase.IssueDate >= '2021-10-01' AND PoliceCase.IssueDate < '2021-11-01'
            group by Depo.Name
            having sum(PoliceCase.TotalAmount) > 0;

            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1,Value2)
            select 'PoliceCase/PoliceCaseDashBoard','DepoWiseExpense','RFL', Depo.Name, sum(PoliceCase.TotalAmount), COUNT(PoliceCase.PK_PoliceCase) as total from PoliceCase
            join Vehicle on Vehicle.PK_Vehicle = PoliceCase.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'RFL')
            AND PoliceCase.IssueDate >= '2022-05-01' AND PoliceCase.IssueDate < '2022-06-01'
            group by Depo.Name
            having sum(PoliceCase.TotalAmount) > 0;


            select * from ReadyReport where ReportName = 'DepoWiseExpense';
             */

            var DepoWiseExpense_query = @"
            delete from ReadyReport where ReportName = 'DepoWiseExpense';
            
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1,Value2)
            select 'PoliceCase/PoliceCaseDashBoard','DepoWiseExpense','PRAN', Depo.Name, sum(PoliceCase.TotalAmount), COUNT(PoliceCase.PK_PoliceCase) as total from PoliceCase
            join Vehicle on Vehicle.PK_Vehicle = PoliceCase.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'PRAN')
            AND PoliceCase.IssueDate >= '2023-12-01' AND PoliceCase.IssueDate < '2024-01-01'
            group by Depo.Name
            having sum(PoliceCase.TotalAmount) > 0;

            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1,Value2)
            select 'PoliceCase/PoliceCaseDashBoard','DepoWiseExpense','RFL', Depo.Name, sum(PoliceCase.TotalAmount), COUNT(PoliceCase.PK_PoliceCase) as total from PoliceCase
            join Vehicle on Vehicle.PK_Vehicle = PoliceCase.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'RFL')
            AND PoliceCase.IssueDate >= '2023-12-01' AND PoliceCase.IssueDate < '2024-01-01'
            group by Depo.Name
            having sum(PoliceCase.TotalAmount) > 0;


            select * from ReadyReport where ReportName = 'DepoWiseExpense';";

            cmd.CommandText = DepoWiseExpense_query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection.CreateCommand();
            cmd.ExecuteNonQuery();

            object DepoWiseExpense;
            if (CurrentUser.PRG_Type == "ALL")
            {
                DepoWiseExpense = bll.db.ReadyReports.Where(m => m.ReportName == "DepoWiseExpense").Select(m => new
                {
                    m.Key,
                    m.Value1,
                    m.Value2
                }).ToList();
            }
            else
            {
                DepoWiseExpense = bll.db.ReadyReports.Where(m => m.ReportName == "DepoWiseExpense" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => new
                {
                    m.Key,
                    m.Value1,
                    m.Value2
                }).ToList();
            }
            #endregion

            //# District Data for Map
            var DistrictWiseCaseDensityTitle = "Case Density In Dstricts : On December-2023";
            
            #region
            /*
            delete from ReadyReport where ReportName = 'DistrictWiseCaseDensity';
            
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1,Value2)
            select 'PoliceCase/PoliceCaseDashBoard','DistrictWiseCaseDensity','PRAN', District.Name, count(PoliceCase.PK_PoliceCase), COUNT(PoliceCase.PK_PoliceCase) as total from PoliceCase
            join Vehicle on Vehicle.PK_Vehicle = PoliceCase.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            join District on PoliceCase.FK_District = District.PK_District
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'PRAN')
            AND PoliceCase.IssueDate >= '2022-05-01' AND PoliceCase.IssueDate < '2022-06-01'
            group by District.Name
            having COUNT(PoliceCase.PK_PoliceCase) > 0;
			
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1,Value2)
            select 'PoliceCase/PoliceCaseDashBoard','DistrictWiseCaseDensity','RFL', District.Name, count(PoliceCase.PK_PoliceCase), COUNT(PoliceCase.PK_PoliceCase) as total from PoliceCase
            join Vehicle on Vehicle.PK_Vehicle = PoliceCase.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            join District on PoliceCase.FK_District = District.PK_District
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'RFL')
            AND PoliceCase.IssueDate >= '2022-05-01' AND PoliceCase.IssueDate < '2022-06-01'
            group by District.Name
            having COUNT(PoliceCase.PK_PoliceCase) > 0;

             */

            var DistrictWiseCaseDensity_query = @"
            delete from ReadyReport where ReportName = 'DistrictWiseCaseDensity';
            
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1,Value2)
            select 'PoliceCase/PoliceCaseDashBoard','DistrictWiseCaseDensity','PRAN', District.Name, count(PoliceCase.PK_PoliceCase), COUNT(PoliceCase.PK_PoliceCase) as total from PoliceCase
            join Vehicle on Vehicle.PK_Vehicle = PoliceCase.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            join District on PoliceCase.FK_District = District.PK_District
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'PRAN')
            AND PoliceCase.IssueDate >= '2023-12-01' AND PoliceCase.IssueDate < '2024-01-01'
            group by District.Name
            having COUNT(PoliceCase.PK_PoliceCase) > 0;
			
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1,Value2)
            select 'PoliceCase/PoliceCaseDashBoard','DistrictWiseCaseDensity','RFL', District.Name, count(PoliceCase.PK_PoliceCase), COUNT(PoliceCase.PK_PoliceCase) as total from PoliceCase
            join Vehicle on Vehicle.PK_Vehicle = PoliceCase.FK_Vehicle
            join Depo on Depo.PK_Depo = Vehicle.FK_Depo
            join District on PoliceCase.FK_District = District.PK_District
            where 1=1
            AND Vehicle.OWN_MHT_DHT = 'OWN'
            AND (Depo.PRG_Type = 'RFL')
            AND PoliceCase.IssueDate >= '2023-12-01' AND PoliceCase.IssueDate < '2024-01-01'
            group by District.Name
            having COUNT(PoliceCase.PK_PoliceCase) > 0;";

            cmd.CommandText = DistrictWiseCaseDensity_query;
            cmd.CommandType = CommandType.Text;
            cmd.Connection.CreateCommand();
            cmd.ExecuteNonQuery();

            object DistrictWiseCaseDensity;

            if (CurrentUser.PRG_Type == "ALL")
            {
                DistrictWiseCaseDensity = bll.db.ReadyReports.Where(m => m.ReportName == "DistrictWiseCaseDensity").GroupBy(a => a.Key)
                         .Select(g => new { g.Key, Value1 = g.Sum(c => c.Value1) }).OrderByDescending(m => m.Value1).ToList();
            }
            else
            {
                DistrictWiseCaseDensity = bll.db.ReadyReports.Where(m => m.ReportName == "DistrictWiseCaseDensity" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => new
                {
                    m.Key,
                    m.Value1
                }).OrderByDescending(m => m.Value1).ToList();
            }
            #endregion
            return Json(new
            {
                PointOfLawsWithCountTitle,
                PointOfLawsWithCount,

                DistrictWiseCaseDensityTitle,
                DistrictWiseCaseDensity,

                DoughnutChartTitle,
                CasedVsNonCased_HasCase,
                CasedVsNonCased_HasNotCase,

                DepoWiseExpenseTitle,
                DepoWiseExpense
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnsolvedIndex(Guid? FK_Vehicle, DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<PoliceCase>();
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var query = bll.db.PoliceCases.AsEnumerable().Where(c => c.IsDeleted == false & c.IsSolved != true).Where(m => accessibleDepoes.Contains(m.Vehicle.FK_Depo));
            if (FK_Vehicle != null)
            {
                query = query.Where(m => m.FK_Vehicle == FK_Vehicle);
                ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(c.FK_Depo)).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", FK_Vehicle);
            }
            else
            {
                ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(c.FK_Depo)).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
            }
            if (StartingDate != null)
            {
                query = query.Where(m => m.IssueDate >= StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-15));
            }
            if (EndingDate != null)
            {
                var _EndingDateString = String.Format("{0:yyyy-MM-dd}", EndingDate);
                var _EndingDate = Convert.ToDateTime(_EndingDateString).AddDays(1);
                query = query.Where(m => m.IssueDate <= _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", _EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }
            query = query.AsQueryable();
            if (FK_Vehicle != null || StartingDate != null || EndingDate != null)
            {
                list = query.ToList();
            }
            return View(list);
        }
        public ActionResult SolvedIndex(Guid? FK_Vehicle, DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<PoliceCase>();
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var query = bll.db.PoliceCases.AsEnumerable().Where(c => c.IsDeleted == false && c.IsSolved == true).Where(m => accessibleDepoes.Contains(m.Vehicle.FK_Depo));
            if (FK_Vehicle != null)
            {
                query = query.Where(m => m.FK_Vehicle == FK_Vehicle);
                ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(c.FK_Depo)).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", FK_Vehicle);
            }
            else
            {
                ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(c.FK_Depo)).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
            }
            if (StartingDate != null)
            {
                query = query.Where(m => m.IssueDate >= StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-15));
            }
            if (EndingDate != null)
            {
                var _EndingDateString = String.Format("{0:yyyy-MM-dd}", EndingDate);
                var _EndingDate = Convert.ToDateTime(_EndingDateString).AddDays(1);
                query = query.Where(m => m.IssueDate <= _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", _EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }
            if (FK_Vehicle != null || StartingDate != null || EndingDate != null)
            {
                list = query.Select(m => m).ToList();
            }
            return View(list);
        }

        public ActionResult View(Guid id)
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
                var model = bll.db.PoliceCases.Find(id);
                if (model != null)
                {
                    var viewModel = bll.ConvertToViewModel(model);
                    var list = bll.db.PoliceCaseDocuments.Where(m => m.FK_PoliceCase == model.PK_PoliceCase && m.IsDeleted == false).ToList();
                    viewModel.PoliceCaseDocuments = list;
                    ViewBag.parentKey = model.FK_Vehicle;
                    return View(viewModel);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }


        public ActionResult Create(Guid? FK_Vehicle)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = new PoliceCase();
            ViewBag.model = model;
            if (FK_Vehicle != null)
            {
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(c.FK_Depo)).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", FK_Vehicle);
                var vehicle = bll.db.Vehicles.Where(m => m.IsDeleted == false && m.PK_Vehicle == FK_Vehicle).FirstOrDefault();
                if (vehicle != null)
                {
                    ViewBag.AdvertisingCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true).OrderBy(m => m.Name), "PK_Company", "Name", vehicle.Internal_FK_AdvertisingCompany);
                }
                else
                {
                    ViewBag.AdvertisingCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true).OrderBy(m => m.Name), "PK_Company", "Name");
                }
            }
            else
            {
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(c.FK_Depo)).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
                ViewBag.AdvertisingCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true).OrderBy(m => m.Name), "PK_Company", "Name");
            }
            ViewBag.Districts = new SelectList(bll.db.Districts, "PK_District", "Name");
            ViewBag.PointOfLaws = new SelectList(bll.db.PoliceCaseLaws, "PK_PoliceCaseLaw", "LawDetail");
            ViewBag.YesNoDict = new SelectList(YesNoDict, "Key", "Value");
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection, List<HttpPostedFileBase> ImageFiles)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            try
            {
                //# PoliceCase
                var policeCaseModel = new PoliceCase();
                policeCaseModel.PK_PoliceCase = Guid.NewGuid();
                policeCaseModel.IsDeleted = false;
                policeCaseModel.CreatedAt = DateTime.Now;
                policeCaseModel.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;
                policeCaseModel.IsSolved = false;
                policeCaseModel.FK_Vehicle = Guid.Parse(formCollection["FK_Vehicle"]);
                if (formCollection["AccusedDriverStaffID"] != "")
                {
                    policeCaseModel.AccusedDriverStaffID = formCollection["AccusedDriverStaffID"];
                }
                if (formCollection["CaseID"] != "")
                {
                    policeCaseModel.CaseID = formCollection["CaseID"];
                    var _existingPoliceCase = bll.db.PoliceCases.Where(m => m.FK_Vehicle == policeCaseModel.FK_Vehicle && m.CaseID == policeCaseModel.CaseID).FirstOrDefault();
                    if (_existingPoliceCase != null)
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", "Case ID: " + policeCaseModel.CaseID + " is aleady added for " + _existingPoliceCase.Vehicle.RegistrationNumber + ". Issue date is " + _existingPoliceCase.IssueDate.ToString());
                        return RedirectToAction("Create");
                    }
                }
                if (formCollection["PoliceContactNumber"] != "")
                {
                    policeCaseModel.PoliceContactNumber = formCollection["PoliceContactNumber"];
                }
                if (formCollection["AccusedDriverName"] != "")
                {
                    policeCaseModel.AccusedDriverName = formCollection["AccusedDriverName"];
                }
                if (formCollection["FK_AdvertisingCompany"] != "")
                {
                    policeCaseModel.FK_AdvertisingCompany = Guid.Parse(formCollection["FK_AdvertisingCompany"]);
                }

                if (formCollection["FK_District"] != "")
                {
                    policeCaseModel.FK_District = Convert.ToInt64(formCollection["FK_District"]);
                }
                if (formCollection["FK_Upazila"] != null && formCollection["FK_Upazila"] != "")
                {
                    policeCaseModel.FK_Upazila = Convert.ToInt64(formCollection["FK_Upazila"]);
                }
                if (formCollection["TypeOfFault"] != "")
                {
                    policeCaseModel.TypeOfFault = formCollection["TypeOfFault"];
                }
                policeCaseModel.Note = formCollection["Note"];
                if (formCollection["IssueDate"] != "")
                {
                    policeCaseModel.IssueDate = Convert.ToDateTime(formCollection["IssueDate"]);
                }

                if (formCollection["IsAlertable"] == "True")
                {
                    policeCaseModel.IsAlertable = true;
                    policeCaseModel.AlertDate = Convert.ToDateTime(formCollection["AlertDate"]);
                }
                else
                {
                    policeCaseModel.IsAlertable = false;
                }
                bll.db.PoliceCases.Add(policeCaseModel);

                if (formCollection["PointOfLawsString"] != "")
                {
                    var PointOfLawsString = formCollection["PointOfLawsString"];
                    var PointOfLawsStringArray = PointOfLawsString.TrimEnd(',').Split(',');
                    foreach (var item in PointOfLawsStringArray)
                    {
                        bll.db.PoliceCase_PoliceCaseLaw.Add(
                            new PoliceCase_PoliceCaseLaw()
                            {
                                FK_PoliceCase = policeCaseModel.PK_PoliceCase,
                                FK_PoliceCaseLaw = item
                            }
                            );
                    }
                }
                //# PoliceCase Doc
                //# create folder
                string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + policeCaseModel.FK_Vehicle + "/" + "Police_Case" + "/";
                string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                if (!Directory.Exists(physicalFolderPath))
                {
                    Directory.CreateDirectory(physicalFolderPath);
                }

                int totalDocument = Convert.ToInt32(formCollection["rowCount"]);
                for (int i = 0; i < totalDocument; i++)
                {
                    var PoliceCaseDocument = new PoliceCaseDocument();

                    PoliceCaseDocument.PK_PoliceCaseDocument = Guid.NewGuid();
                    PoliceCaseDocument.IsDeleted = false;

                    PoliceCaseDocument.CreatedAt = DateTime.Now;
                    PoliceCaseDocument.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;

                    PoliceCaseDocument.FK_PoliceCase = policeCaseModel.PK_PoliceCase;
                    PoliceCaseDocument.Title = formCollection["Title_" + i];
                    PoliceCaseDocument.IdentitficaitonKey = formCollection["IdentitficaitonKey_" + i];
                    PoliceCaseDocument.IdentitficaitonValue = formCollection["IdentitficaitonValue_" + i];

                    string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " " + i + " " + PoliceCaseDocument.PK_PoliceCaseDocument + "." + ImageFiles[i].FileName.Split('.').Last();
                    ImageFiles[i].SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                    PoliceCaseDocument.ImageLocation = virtualFilePath;

                    bll.db.PoliceCaseDocuments.Add(PoliceCaseDocument);
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Police case is successfully added.");
                return RedirectToAction("Create", new { FK_Vehicle = policeCaseModel.FK_Vehicle });
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Create");
            }
        }

        public ActionResult Edit(Guid id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = bll.db.PoliceCases.Find(id);
            ViewBag.model = model;
            ViewBag.PoliceCaseDocuments = bll.db.PoliceCaseDocuments.Where(c => c.IsDeleted == false && c.FK_PoliceCase == id).ToList();
            ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN").OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);
            ViewBag.Districts = new SelectList(bll.db.Districts, "PK_District", "Name", model.FK_District);
            ViewBag.Upazilas = new SelectList(bll.db.Upazilas, "PK_Upazila", "Name", model.FK_Upazila);
            ViewBag.PointOfLaws = new MultiSelectList(bll.db.PoliceCaseLaws, "PK_PoliceCaseLaw", "LawDetail", bll.db.PoliceCase_PoliceCaseLaw.Where(m => m.FK_PoliceCase == model.PK_PoliceCase).Select(m => m.FK_PoliceCaseLaw).ToArray());
            ViewBag.YesNoDict = new SelectList(YesNoDict, "Key", "Value");

            return View();
        }

        [HttpPost]
        public ActionResult Edit(FormCollection formCollection, List<HttpPostedFileBase> ImageFiles)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            try
            {
                //# PoliceCase
                var policeCaseModel = bll.db.PoliceCases.Find(Guid.Parse(formCollection["PK_PoliceCase"]));
                policeCaseModel.IsDeleted = false;
                policeCaseModel.UpdatedAt = DateTime.Now;
                policeCaseModel.FK_UpdatedByUser = CommonClass.GetCurrentUser().PK_User;
                //policeCaseModel.IsSolved = false;
                policeCaseModel.FK_Vehicle = Guid.Parse(formCollection["FK_Vehicle"]);
                if (formCollection["AccusedDriverStaffID"] != "")
                {
                    policeCaseModel.AccusedDriverStaffID = formCollection["AccusedDriverStaffID"];
                }
                if (formCollection["CaseID"] != "")
                {
                    policeCaseModel.CaseID = formCollection["CaseID"];
                }
                if (formCollection["PoliceContactNumber"] != "")
                {
                    policeCaseModel.PoliceContactNumber = formCollection["PoliceContactNumber"];
                }
                if (formCollection["AccusedDriverName"] != "")
                {
                    policeCaseModel.AccusedDriverName = formCollection["AccusedDriverName"];
                }
                if (formCollection["FK_District"] != "")
                {
                    policeCaseModel.FK_District = Convert.ToInt64(formCollection["FK_District"]);
                }
                if (formCollection["FK_Upazila"] != null && formCollection["FK_Upazila"] != "")
                {
                    policeCaseModel.FK_Upazila = Convert.ToInt64(formCollection["FK_Upazila"]);
                }
                if (formCollection["TypeOfFault"] != "")
                {
                    policeCaseModel.TypeOfFault = formCollection["TypeOfFault"];
                }
                policeCaseModel.Note = formCollection["Note"];
                if (formCollection["IssueDate"] != "")
                {
                    policeCaseModel.IssueDate = Convert.ToDateTime(formCollection["IssueDate"]);
                }

                if (formCollection["IsAlertable"] == "True")
                {
                    policeCaseModel.IsAlertable = true;
                    policeCaseModel.AlertDate = Convert.ToDateTime(formCollection["AlertDate"]);
                }
                else
                {
                    policeCaseModel.IsAlertable = false;
                }

                if (formCollection["PrimaryAmount"] != "")
                {
                    policeCaseModel.PrimaryAmount = Convert.ToInt64(formCollection["PrimaryAmount"]);
                    policeCaseModel.TotalAmount = policeCaseModel.PrimaryAmount;
                }
                else
                {
                    policeCaseModel.PrimaryAmount = null;
                    policeCaseModel.TotalAmount = 0;
                }
                if (formCollection["OtherAmount"] != "")
                {
                    policeCaseModel.OtherAmount = Convert.ToInt64(formCollection["OtherAmount"]);
                    policeCaseModel.OtherNote = formCollection["OtherNote"];
                    policeCaseModel.TotalAmount = policeCaseModel.TotalAmount + policeCaseModel.OtherAmount;
                }
                else
                {
                    policeCaseModel.OtherAmount = null;
                    policeCaseModel.OtherNote = null;
                }
                if (formCollection["SolvedOn"] != "")
                {
                    policeCaseModel.IsSolved = true;
                    policeCaseModel.SolvedOn = Convert.ToDateTime(formCollection["SolvedOn"]);

                    policeCaseModel.IsAlertable = false;
                }
                policeCaseModel.SolvedNote = formCollection["SolvedNote"];
                bll.db.PoliceCase_PoliceCaseLaw.RemoveRange(bll.db.PoliceCase_PoliceCaseLaw.Where(m => m.FK_PoliceCase == policeCaseModel.PK_PoliceCase).ToList());
                if (formCollection["PointOfLawsString"] != "")
                {
                    var PointOfLawsString = formCollection["PointOfLawsString"];
                    var PointOfLawsStringArray = PointOfLawsString.TrimEnd(',').Split(',');
                    foreach (var item in PointOfLawsStringArray)
                    {
                        bll.db.PoliceCase_PoliceCaseLaw.Add(
                            new PoliceCase_PoliceCaseLaw()
                            {
                                FK_PoliceCase = policeCaseModel.PK_PoliceCase,
                                FK_PoliceCaseLaw = item
                            }
                            );
                    }
                }

                //# PoliceCase Doc
                //# create folder
                string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + policeCaseModel.FK_Vehicle + "/" + "Police_Case" + "/";
                string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                if (!Directory.Exists(physicalFolderPath))
                {
                    Directory.CreateDirectory(physicalFolderPath);
                }

                int totalDocument = Convert.ToInt32(formCollection["rowCount"]);
                for (int i = 0; i < totalDocument; i++)
                {
                    var PoliceCaseDocument = new PoliceCaseDocument();

                    PoliceCaseDocument.PK_PoliceCaseDocument = Guid.NewGuid();
                    PoliceCaseDocument.IsDeleted = false;

                    PoliceCaseDocument.CreatedAt = DateTime.Now;
                    PoliceCaseDocument.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;

                    PoliceCaseDocument.FK_PoliceCase = policeCaseModel.PK_PoliceCase;
                    PoliceCaseDocument.Title = formCollection["Title_" + i];
                    PoliceCaseDocument.IdentitficaitonKey = formCollection["IdentitficaitonKey_" + i];
                    PoliceCaseDocument.IdentitficaitonValue = formCollection["IdentitficaitonValue_" + i];

                    string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " " + i + " " + PoliceCaseDocument.PK_PoliceCaseDocument + "." + ImageFiles[i].FileName.Split('.').Last();
                    ImageFiles[i].SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                    PoliceCaseDocument.ImageLocation = virtualFilePath;

                    bll.db.PoliceCaseDocuments.Add(PoliceCaseDocument);
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Police case is successfully updated.");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return RedirectToAction("Edit", new { id = formCollection["PK_PoliceCase"] });
        }

        public ActionResult Solve(Guid id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = bll.db.PoliceCases.Find(id);
            ViewBag.model = model;
            ViewBag.PoliceCaseDocuments = bll.db.PoliceCaseDocuments.Where(c => c.IsDeleted == false && c.FK_PoliceCase == id).ToList();
            ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN").OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);
            ViewBag.YesNoDict = new SelectList(YesNoDict, "Key", "Value", model.IsAlertable);
            var viewModel = bll.ConvertToViewModel(model);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Solve(FormCollection formCollection, List<HttpPostedFileBase> ImageFiles)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            try
            {
                //# PoliceCase
                var policeCaseModel = bll.db.PoliceCases.Find(Guid.Parse(formCollection["PK_PoliceCase"]));

                if (formCollection["PrimaryAmount"] != "")
                {
                    policeCaseModel.PrimaryAmount = Convert.ToInt64(formCollection["PrimaryAmount"]);
                    policeCaseModel.TotalAmount = policeCaseModel.PrimaryAmount;
                }
                else
                {
                    policeCaseModel.PrimaryAmount = null;
                    policeCaseModel.TotalAmount = 0;
                }
                if (formCollection["OtherAmount"] != "")
                {
                    policeCaseModel.OtherAmount = Convert.ToInt64(formCollection["OtherAmount"]);
                    policeCaseModel.OtherNote = formCollection["OtherNote"];
                    policeCaseModel.TotalAmount = policeCaseModel.TotalAmount + policeCaseModel.OtherAmount;
                }
                else
                {
                    policeCaseModel.OtherAmount = null;
                    policeCaseModel.OtherNote = null;
                }

                policeCaseModel.IsSolved = true;
                policeCaseModel.SolvedEntryGivenAt = DateTime.Now;
                policeCaseModel.FK_SolvedEntryGivenUser = CommonClass.GetCurrentUser().PK_User;
                policeCaseModel.SolvedOn = Convert.ToDateTime(formCollection["SolvedOn"]);
                policeCaseModel.SolvedNote = formCollection["SolvedNote"];

                policeCaseModel.IsAlertable = false;

                //# PoliceCase Doc
                //# create folder
                string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + policeCaseModel.FK_Vehicle + "/" + "Police_Case" + "/";
                string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                if (!Directory.Exists(physicalFolderPath))
                {
                    Directory.CreateDirectory(physicalFolderPath);
                }

                int totalDocument = Convert.ToInt32(formCollection["rowCount"]);
                for (int i = 0; i < totalDocument; i++)
                {
                    var PoliceCaseDocument = new PoliceCaseDocument();

                    PoliceCaseDocument.PK_PoliceCaseDocument = Guid.NewGuid();
                    PoliceCaseDocument.IsDeleted = false;

                    PoliceCaseDocument.CreatedAt = DateTime.Now;
                    PoliceCaseDocument.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;

                    PoliceCaseDocument.FK_PoliceCase = policeCaseModel.PK_PoliceCase;
                    PoliceCaseDocument.Title = formCollection["Title_" + i];
                    PoliceCaseDocument.IdentitficaitonKey = formCollection["IdentitficaitonKey_" + i];
                    PoliceCaseDocument.IdentitficaitonValue = formCollection["IdentitficaitonValue_" + i];

                    string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " " + i + " " + PoliceCaseDocument.PK_PoliceCaseDocument + "." + ImageFiles[i].FileName.Split('.').Last();
                    ImageFiles[i].SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                    PoliceCaseDocument.ImageLocation = virtualFilePath;

                    bll.db.PoliceCaseDocuments.Add(PoliceCaseDocument);
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Police case is successfully updated.");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return RedirectToAction("UnsolvedIndex");
        }

        public ActionResult Pay(Guid id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = bll.db.PoliceCases.Find(id);
            model.IsPaid = true;
            model.FK_PaidEntryGivenUser = CurrentUser.PK_User;
            model.PaidEntryGivenAt = DateTime.Now;
            bll.db.SaveChanges();
            return Redirect(Request.ServerVariables["http_referer"]);
        }

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
                var model = bll.db.PoliceCases.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;

                        model.DeletedAt = DateTime.Now;
                        model.FK_DeletedByUser = CommonClass.GetCurrentUser().PK_User;

                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Police case is successfully deleted.");
                        return RedirectToAction("UnsolvedIndex");
                    }
                    catch (Exception exception)
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                        return RedirectToAction("UnsolvedIndex");
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }


        public FileResult DownloadFile(Guid id)
        {
            var model = bll.db.PoliceCaseDocuments.Find(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath(model.ImageLocation)));
            string fileName = model.ImageLocation.Split('/').Last();

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult DeleteFile(Guid id)
        {
            var model = bll.db.PoliceCaseDocuments.Find(id);
            var FK_PoliceCase = model.FK_PoliceCase;
            try
            {
                System.IO.File.Delete(Path.Combine(Server.MapPath(model.ImageLocation)));
                bll.db.PoliceCaseDocuments.Remove(model);
                bll.db.SaveChanges();
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
            }
            return RedirectToAction("Edit", new { id = FK_PoliceCase });
        }
        //# AJAX METHOD
        [HttpPost]
        public JsonResult GetUpazilasOfDistrict(Int64 FK_District)
        {
            var list = bll.db.Upazilas.Where(m => m.FK_District == FK_District).Select(m => new { m.PK_Upazila, m.Name }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //# Mail
        public string TrySendPoliceCaseAlertEmail()
        {
            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendPoliceCaseAlertEmail-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();

            var today = DateTime.Now.Date;
            var mailRequestTIme = DateTime.Now;
            var mails = bll.db.AlertEmails.Where(m => m.IsDeleted != true && (m.PoliceCaseAlert_1 == true || m.PoliceCaseAlert_2 == true)).ToList();
            var _total = mails.Count;
            var _sent = 0;
            foreach (var mail in mails)
            {
                string mailSubject = "";
                if (mail.PoliceCaseAlert_1 == true)
                {
                    mailSubject = "Vehicle PoliceCase alert";
                    var Depo_PKs = bll.db.AlertEmailAttachedDepoes.Where(m => m.FK_AlertEmail == mail.PK_AlertEmail && m.IsAttachable == true).Select(m => m.FK_Depo).ToList();
                    var hasAny = (from eve in bll.db.PoliceCases.AsEnumerable().Where(e => e.IsAlertable == true && today >= e.AlertDate)
                                  join vehicle in bll.db.Vehicles.Where(v => Depo_PKs.Contains(v.FK_Depo)) on eve.FK_Vehicle equals vehicle.PK_Vehicle
                                  select new
                                  {
                                  }).Any();
                    if (hasAny == false)
                    {
                        continue;
                    }

                }
                else
                {
                    continue;
                }

                string url = "";
#if DEBUG
                url = ConfigurationManager.AppSettings["DEBUG_DOMAIN"];
#else
url = ConfigurationManager.AppSettings["LIVE_DOMAIN"];
#endif
                url = url + @"PoliceCase/PoliceCaseAlertEmailBodyGenerator?PK_AlertEmail=" + mail.PK_AlertEmail;
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
                        CallingMessage = "TrySendPoliceCaseAlertEmail-Error" + guid,
                        UserDefinedMessage = errrorMessage
                    }
                    );
                    bll.db.SaveChanges();
                }
            }

            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendPoliceCaseAlertEmail-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = "total:" + _total + " sent:" + _sent
                  }
                  );
            bll.db.SaveChanges();
            return "Sent " + _sent + "/" + _total;
        }
        public ActionResult PoliceCaseAlertEmailBodyGenerator(Guid PK_AlertEmail)
        {
            var today = DateTime.Now.Date;
            var mail = bll.db.AlertEmails.Where(m => m.PK_AlertEmail == PK_AlertEmail).FirstOrDefault();
            var list = new List<PoliceCase>();
            if (mail.EventAlert_1 == true)
            {
                ViewBag.Message_1 = "This are the unsolved police cases which alert date is today or passed before. Please, take necessary action.";
                var Depo_PKs = bll.db.AlertEmailAttachedDepoes.Where(m => m.FK_AlertEmail == mail.PK_AlertEmail && m.IsAttachable == true).Select(m => m.FK_Depo).ToList();
                list = (from eve in bll.db.PoliceCases.AsEnumerable().Where(e => e.IsAlertable == true && today >= e.AlertDate)
                        join vehicle in bll.db.Vehicles.Where(v => Depo_PKs.Contains(v.FK_Depo)) on eve.FK_Vehicle equals vehicle.PK_Vehicle
                        select eve).OrderBy(m => m.Vehicle.Depo.Name).ToList();
                return View(list);
            }
            else
            {
                ViewBag.Message_1 = "N/A";
                return View(list);
            }
        }
    }
}