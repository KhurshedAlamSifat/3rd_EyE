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
using System.IO;
using _3rdEyE.BLLs;

namespace _3rdEyE.Controllers
{
    public class InstantRequisitionController : BaseController
    {
        Dictionary<string, string> ProfitSharePolicyDict = new Dictionary<string, string> { { "Common Policy-1", "Common Policy-1::start-end,driver,helper,tpt; 1000-2000,150,105,100; 2001-3500,180,126,100; 3501,5000,200,140,150; 5001,6500,250,175,150; 6501,9000,300,210,200; 9001,12000,350,245,200; 12001,15500,400,280,300; 15501,20000,450,315,300; 20001,25000,500,350,300; 25001,30000,550,385,300; 30001,Above,600,420,300;" } };
        BLL_InstantRequisition bll = new BLL_InstantRequisition();

        public ActionResult IndexBy_Agent(DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<InstantRequisition>();
            //var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var query = bll.db.InstantRequisitions.AsEnumerable().Where(c => c.IsDeleted == false);
            if (StartingDate != null)
            {
                query = query.Where(m => m.CreatedAt >= StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-1));
            }
            if (EndingDate != null)
            {
                var _EndingDateString = String.Format("{0:yyyy-MM-dd}", EndingDate);
                var _EndingDate = Convert.ToDateTime(_EndingDateString).AddDays(1);
                query = query.Where(m => m.CreatedAt <= _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", _EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }
            if (StartingDate != null || EndingDate != null)
            {
                list = query.OrderByDescending(m => m.CreatedAt).ToList();
            }
            return View(list);
        }
        public ActionResult IndexBy_PaymentReceiver(DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<InstantRequisition>();
            //var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var query = bll.db.InstantRequisitions.AsEnumerable().Where(c => c.IsDeleted == false);
            if (StartingDate != null)
            {
                query = query.Where(m => m.CreatedAt >= StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-1));
            }
            if (EndingDate != null)
            {
                var _EndingDateString = String.Format("{0:yyyy-MM-dd}", EndingDate);
                var _EndingDate = Convert.ToDateTime(_EndingDateString).AddDays(1);
                query = query.Where(m => m.CreatedAt <= _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", _EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }
            if (StartingDate != null || EndingDate != null)
            {
                list = query.OrderByDescending(m => m.CreatedAt).ToList();
            }
            return View(list);
        }
        public void GetExcel(DateTime? StartingDate, DateTime? EndingDate)
        {
            var list = new List<InstantRequisition>();
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var query = bll.db.InstantRequisitions.AsEnumerable().Where(c => c.IsDeleted == false).Where(m => accessibleDepoes.Contains(m.Vehicle.FK_Depo));
            if (StartingDate != null)
            {
                query = query.Where(m => m.CreatedAt >= StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-7));
            }
            if (EndingDate != null)
            {
                var _EndingDateString = String.Format("{0:yyyy-MM-dd}", EndingDate);
                var _EndingDate = Convert.ToDateTime(_EndingDateString).AddDays(1);
                query = query.Where(m => m.CreatedAt <= _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", _EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }
            if (StartingDate != null || EndingDate != null)
            {
                list = query.ToList();
            }
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Instant_Requisition_List.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            //# Add Header Row
            Response.Output.Write("Created At" + "\t");
            Response.Output.Write("Vehicle" + "\t");
            Response.Output.Write("DriverStaffID" + "\t");
            Response.Output.Write("DriverName" + "\t");
            Response.Output.Write("HelperInfo" + "\t");
            Response.Output.Write("Depo_Booking" + "\t");
            Response.Output.Write("Depo_Destination" + "\t");
            Response.Output.Write("StartingLocation" + "\t");
            Response.Output.Write("FinishingLocation" + "\t");
            Response.Output.Write("TripFare1" + "\t");
            Response.Output.Write("AdvancedToDriver1" + "\t");
            Response.Output.Write("Distance_Empty1" + "\t");
            Response.Output.Write("KPL_Empty1" + "\t");
            Response.Output.Write("Distance_Loaded1" + "\t");
            Response.Output.Write("KPL_Loaded1" + "\t");
            Response.Output.Write("Distance_Loaded_8_To_12_Tons_Out1" + "\t");
            Response.Output.Write("KPL_Loaded_8_To_12_Tons_Out1" + "\t");
            Response.Output.Write("Distance_Loaded_12_To_25_Tons_Out1" + "\t");
            Response.Output.Write("KPL_Loaded_12_To_25_Tons_Out1" + "\t");
            Response.Output.Write("FuelConsumedLitre1" + "\t");
            Response.Output.Write("FuelPricePerLitre1" + "\t");
            Response.Output.Write("FuelExpenceGivenCashOrOil" + "\t");
            Response.Output.Write("StayCharge1" + "\t");
            Response.Output.Write("TripBillDriver1" + "\t");
            Response.Output.Write("TripBillHelper1" + "\t");
            Response.Output.Write("EntertainmentACharge1" + "\t");
            Response.Output.Write("RepairCharge1" + "\t");
            Response.Output.Write("BridgeTollFerryCharge1" + "\t");
            Response.Output.Write("OpenBodyCharge1" + "\t");
            Response.Output.Write("TransportAgencyName" + "\t");
            Response.Output.Write("TransportAgencyContactNumber" + "\t");
            Response.Output.Write("ResponsibleParsonName" + "\t");
            Response.Output.Write("ResponsibleParsonContactNumber" + "\t");
            Response.Output.Write("Note1" + "\t");
            Response.Output.Write("TotalParent1" + "\t");

            //#phase 2
            Response.Output.Write("BillPaidAt" + "\t");
            Response.Output.Write("DriversMoney2" + "\t");
            Response.Output.Write("HelpersMoney2" + "\t");
            Response.Output.Write("TPTsMoney2" + "\t");
            Response.Output.Write("ScaleCharge2" + "\t");
            Response.Output.Write("NightStayCharge2" + "\t");
            Response.Output.Write("StayCharge2" + "\t");
            Response.Output.Write("TransportCommissionCharge2" + "\t");
            Response.Output.Write("EntertainmentACharge2" + "\t");
            Response.Output.Write("GiveDemerageCharge2" + "\t");
            Response.Output.Write("TotalGiven2" + "\t");
            Response.Output.Write("Note2" + "\t");


            //#phase 3
            Response.Output.Write("Distance_Empty3" + "\t");
            Response.Output.Write("Distance_Loaded3" + "\t");
            Response.Output.Write("Distance_Loaded_8_To_12_Tons_Out3" + "\t");
            Response.Output.Write("Distance_Loaded_12_To_25_Tons_Out3" + "\t");
            Response.Output.Write("FuelConsumedLitre3" + "\t");
            Response.Output.Write("FuelPricePerLitre3" + "\t");
            Response.Output.Write("FuelExpence3" + "\t");
            Response.Output.Write("BridgeTollFerryCharge3" + "\t");
            Response.Output.Write("TakeDemerageCharge3" + "\t");
            Response.Output.Write("TPTsMoney3" + "\t");
            Response.Output.Write("OthersCharge3" + "\t");
            Response.Output.Write("TotalTaken3" + "\t");
            Response.Output.Write("NetProfit3" + "\t");
            Response.Output.Write("GrossTaken3" + "\t");
            Response.Output.Write("Note3" + "\t");
            Response.Output.Write("AdjustedAt" + "\t");
            Response.Output.Write("NetProfit4" + "\t");
            Response.Output.Write("GrossProfit4" + "\t");
            Response.Output.Write("DepositeToAcccountLocation" + "\t");
            Response.Output.Write("MRR" + "\t");


            Response.Output.WriteLine();
            foreach (var item in list)
            {
                Response.Output.Write(item.CreatedAt + "\t");
                Response.Output.Write(item.Vehicle.RegistrationNumber + "\t");
                if (item.DriverStaffID != null)
                {
                    Response.Output.Write(item.DriverStaffID + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.DriverName != null)
                {
                    Response.Output.Write(item.DriverName + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.HelperInfo != null)
                {
                    Response.Output.Write(item.HelperInfo + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Depo != null)
                {
                    Response.Output.Write(item.Depo.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Depo1 != null)
                {
                    Response.Output.Write(item.Depo1.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.StartingLocation != null)
                {
                    Response.Output.Write(item.StartingLocation + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.FinishingLocation != null)
                {
                    Response.Output.Write(item.FinishingLocation + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.TripFare1 != null)
                {
                    Response.Output.Write(item.TripFare1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.AdvancedToDriver1 != null)
                {
                    Response.Output.Write(item.AdvancedToDriver1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                if (item.Distance_Empty1 != null)
                {
                    Response.Output.Write(item.Distance_Empty1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.KPL_Empty1 != null)
                {
                    Response.Output.Write(item.KPL_Empty1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                if (item.Distance_Loaded1 != null)
                {
                    Response.Output.Write(item.Distance_Loaded1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.KPL_Loaded1 != null)
                {
                    Response.Output.Write(item.KPL_Loaded1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                if (item.Distance_Loaded_8_To_12_Tons_Out1 != null)
                {
                    Response.Output.Write(item.Distance_Loaded_8_To_12_Tons_Out1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.KPL_Loaded_8_To_12_Tons_Out1 != null)
                {
                    Response.Output.Write(item.KPL_Loaded_8_To_12_Tons_Out1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                if (item.Distance_Loaded_12_To_25_Tons_Out1 != null)
                {
                    Response.Output.Write(item.Distance_Loaded_12_To_25_Tons_Out1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.KPL_Loaded_12_To_25_Tons_Out1 != null)
                {
                    Response.Output.Write(item.KPL_Loaded_12_To_25_Tons_Out1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                if (item.FuelConsumedLitre1 != null)
                {
                    Response.Output.Write(item.FuelConsumedLitre1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                if (item.FuelPricePerLitre1 != null)
                {
                    Response.Output.Write(item.FuelPricePerLitre1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                if (item.FuelExpenceGivenCashOrOil != null)
                {
                    Response.Output.Write((item.FuelExpenceGivenCashOrOil == true ? "Cash" : "Oil") + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.StayCharge1 != null)
                {
                    Response.Output.Write(item.StayCharge1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.TripBillDriver1 != null)
                {
                    Response.Output.Write(item.TripBillDriver1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.TripBillHelper1 != null)
                {
                    Response.Output.Write(item.TripBillHelper1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.EntertainmentACharge1 != null)
                {
                    Response.Output.Write(item.EntertainmentACharge1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.RepairCharge1 != null)
                {
                    Response.Output.Write(item.RepairCharge1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.BridgeTollFerryCharge1 != null)
                {
                    Response.Output.Write(item.BridgeTollFerryCharge1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.OpenBodyCharge1 != null)
                {
                    Response.Output.Write(item.OpenBodyCharge1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.TransportAgencyName != null)
                {
                    Response.Output.Write(item.TransportAgencyName + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.TransportAgencyContactNumber != null)
                {
                    Response.Output.Write(item.TransportAgencyContactNumber + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.ResponsibleParsonName != null)
                {
                    Response.Output.Write(item.ResponsibleParsonName + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.ResponsibleParsonContactNumber != null)
                {
                    Response.Output.Write(item.ResponsibleParsonContactNumber + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Note1 != null)
                {
                    Response.Output.Write(item.Note1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.TotalParent1 != null)
                {
                    Response.Output.Write(item.TotalParent1 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                //#phase 2
                if (item.BillPaidAt != null)
                {
                    Response.Output.Write(item.BillPaidAt + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.DriversMoney2 != null)
                {
                    Response.Output.Write(item.DriversMoney2 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.HelpersMoney2 != null)
                {
                    Response.Output.Write(item.HelpersMoney2 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.TPTsMoney2 != null)
                {
                    Response.Output.Write(item.TPTsMoney2 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.ScaleCharge2 != null)
                {
                    Response.Output.Write(item.ScaleCharge2 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.NightStayCharge2 != null)
                {
                    Response.Output.Write(item.NightStayCharge2 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.StayCharge2 != null)
                {
                    Response.Output.Write(item.StayCharge2 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.TransportCommissionCharge2 != null)
                {
                    Response.Output.Write(item.TransportCommissionCharge2 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.EntertainmentACharge2 != null)
                {
                    Response.Output.Write(item.EntertainmentACharge2 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.DemerageChargeGiven2 != null)
                {
                    Response.Output.Write(item.DemerageChargeGiven2 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.TotalGiven2 != null)
                {
                    Response.Output.Write(item.TotalGiven2 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Note2 != null)
                {
                    Response.Output.Write(item.Note2 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }


                //#Phase 3
                if (item.Distance_Empty3 != null)
                {
                    Response.Output.Write(item.Distance_Empty3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Distance_Loaded3 != null)
                {
                    Response.Output.Write(item.Distance_Loaded3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Distance_Loaded_8_To_12_Tons_Out3 != null)
                {
                    Response.Output.Write(item.Distance_Loaded_8_To_12_Tons_Out3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Distance_Loaded_12_To_25_Tons_Out3 != null)
                {
                    Response.Output.Write(item.Distance_Loaded_12_To_25_Tons_Out3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.FuelConsumedLitre3 != null)
                {
                    Response.Output.Write(item.FuelConsumedLitre3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.FuelPricePerLitre3 != null)
                {
                    Response.Output.Write(item.FuelPricePerLitre3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.FuelExpence3 != null)
                {
                    Response.Output.Write(item.FuelExpence3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.BridgeTollFerryCharge3 != null)
                {
                    Response.Output.Write(item.BridgeTollFerryCharge3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.DemerageChargeTaken3 != null)
                {
                    Response.Output.Write(item.DemerageChargeTaken3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.TPTsMoney3 != null)
                {
                    Response.Output.Write(item.TPTsMoney3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.AdjustedEvAmount != null)
                {
                    Response.Output.Write(item.AdjustedEvAmount + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.TotalTaken3 != null)
                {
                    Response.Output.Write(item.TotalTaken3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.NetProfit3 != null)
                {
                    Response.Output.Write(item.NetProfit3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.GrossTaken3 != null)
                {
                    Response.Output.Write(item.GrossTaken3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Note3 != null)
                {
                    Response.Output.Write(item.Note3 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.AdjustedAt != null)
                {
                    Response.Output.Write(item.AdjustedAt + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.NetProfit4 != null)
                {
                    Response.Output.Write(item.NetProfit4 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.GrossProfit4 != null)
                {
                    Response.Output.Write(item.GrossProfit4 + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.DepositeToAcccountLocation != null)
                {
                    Response.Output.Write(item.DepositeToAcccountLocation + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.MRR != null)
                {
                    Response.Output.Write(item.MRR + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }



                Response.Output.WriteLine();

            }
            Response.End();
        }
        public ActionResult View(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.InstantRequisitions.Find(id);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }

        }

        public ActionResult PrintView(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.InstantRequisitions.Find(id);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }

        }

        public ActionResult Create()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = new InstantRequisition();
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.PK_Vehicle == null), "PK_Vehicle", "RegistrationNumber");
            ViewBag.TransportAgency = new SelectList(bll.db.TransportAgencies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Select(m => new { PK_TransportAgency = m.PK_TransportAgency, Name = m.Name + " " + m.DistrictName + " " + m.MobileNumber_Primary + " " + m.MobileNumber_Secondary }), "PK_TransportAgency", "Name");
            ViewBag.LocationsFrom = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", CurrentUser.FK_Location);
            ViewBag.LocationsTo = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            ViewBag.ProfitSharePolicyDict = new SelectList(ProfitSharePolicyDict.OrderBy(m => m.Value), "Key", "Key");
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(InstantRequisition model)
        {
            try
            {
                model.Status = 0;
                model.StatusText = "Created";
                model.FK_RequisitionAgent = CurrentUser.PK_User;
                model.CreatedAt = DateTime.Now;
                model.REF_FK_Depo = CurrentUser.FK_Depo;
                bll.db.InstantRequisitions.Add(model);
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Instant Requisition is successfully added.");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.PK_Vehicle == model.FK_Vehicle).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);
                ViewBag.TransportAgency = new SelectList(bll.db.TransportAgencies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name).Select(m => new { PK_TransportAgency = m.PK_TransportAgency, Name = m.Name + " " + m.DistrictName + " " + m.MobileNumber_Primary + " " + m.MobileNumber_Secondary }), "PK_TransportAgency", "Name", model.FK_TransportAgency);
                ViewBag.LocationsFrom = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location_From);
                ViewBag.LocationsTo = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location_To);
                ViewBag.ProfitSharePolicyDict = new SelectList(ProfitSharePolicyDict.OrderBy(m => m.Value), "Key", "Key");
                //return View(model);
            }
            return RedirectToAction("IndexBy_Agent");
        }

        public ActionResult Settle(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var db_model = bll.db.InstantRequisitions.Where(m => m.PK_InstantRequisition == id).FirstOrDefault();
            if (db_model != null && db_model.StatusText == "Created")
            {
                return View(db_model);
            }
            else if (db_model != null && db_model.StatusText != "Created")
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Instant Requisition Status: " + db_model.StatusText + ".");
                return RedirectToAction("IndexBy_PaymentReceiver");
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Instant Requisition not found.");
                return RedirectToAction("IndexBy_PaymentReceiver");
            }
        }
        [HttpPost]
        public ActionResult Settle(InstantRequisition model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var db_model = bll.db.InstantRequisitions.Where(m => m.PK_InstantRequisition == model.PK_InstantRequisition).FirstOrDefault();
            if (db_model != null && db_model.StatusText == "Created")
            {
                db_model.StayCharge1 = model.StayCharge1;
                db_model.DemerageChargeGiven2 = model.DemerageChargeGiven2;
                db_model.DemerageChargeGiven2 = model.DemerageChargeGiven2;
                db_model.DemerageChargeTaken3 = model.DemerageChargeTaken3;
                db_model.DemerageNote = model.DemerageNote;
                db_model.AdjustedEvNumber = model.AdjustedEvNumber;
                db_model.AdjustedEvAmount = model.AdjustedEvAmount;
                db_model.AdjustedEvNote = model.AdjustedEvNote;
                //db_model.ReceiverStaffId = model.ReceiverStaffId;
                //db_model.ReceiverStaffName = model.ReceiverStaffName;
                db_model.TotalParent1= model.TotalParent1;
                db_model.DepositeAmount= model.DepositeAmount;
                db_model.MRNumber = model.MRNumber;

                db_model.StatusText = "Settled";

                db_model.FK_AppUser_AdjustedBy = CurrentUser.PK_User;
                db_model.AdjustedAt = DateTime.Now;
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Successfully Settled.");
                return RedirectToAction("IndexBy_PaymentReceiver");
            }
            else if (db_model != null && db_model.StatusText != "Created")
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Instant Requisition Status: " + model.StatusText + ".");
                return RedirectToAction("IndexBy_PaymentReceiver");
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Instant Requisition not found.");
                return RedirectToAction("IndexBy_PaymentReceiver");
            }
        }

        public ActionResult PayBill(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.InstantRequisitions.Where(m => m.PK_InstantRequisition == id).FirstOrDefault();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Instant Requisition not found.");
                return RedirectToAction("IndexBy_PaymentReceiver");
            }
        }
        [HttpPost]
        public ActionResult PayBill(InstantRequisition model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var db_model = bll.db.InstantRequisitions.Where(m => m.PK_InstantRequisition == model.PK_InstantRequisition).FirstOrDefault();
            if (db_model != null)
            {
                if (db_model.Status == 0)
                {
                    db_model.Status = 1;
                    db_model.BillPaidAt = DateTime.Now;
                    db_model.FK_AppUser_BillPaidBy = CurrentUser.PK_User;
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Instant Requisition bill is successfully paid.");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Instant Requisition bill is already paid.");
                }
                return RedirectToAction("IndexBy_PaymentReceiver");
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult PayAdjustmnet(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.InstantRequisitions.Where(m => m.PK_InstantRequisition == id).FirstOrDefault();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Instant Requisition not found.");
                return RedirectToAction("IndexBy_PaymentReceiver");
            }
        }
        [HttpPost]
        public ActionResult PayAdjustmnet(InstantRequisition model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var db_model = bll.db.InstantRequisitions.Where(m => m.PK_InstantRequisition == model.PK_InstantRequisition).FirstOrDefault();
            if (db_model != null)
            {
                if (db_model.Status == 1)
                {
                    db_model.FK_AppUser_AdjustedBy = CurrentUser.PK_User;
                    db_model.AdjustedAt = DateTime.Now;
                    db_model.Status = 2;
                    //# Adjustment-Given

                    db_model.DriversMoney2 = model.DriversMoney2 != null ? model.DriversMoney2 : 0;
                    db_model.HelpersMoney2 = model.HelpersMoney2 != null ? model.HelpersMoney2 : 0;
                    db_model.TPTsMoney2 = model.TPTsMoney2 != null ? model.TPTsMoney2 : 0;
                    db_model.ScaleCharge2 = model.ScaleCharge2 != null ? model.ScaleCharge2 : 0;
                    db_model.NightStayCharge2 = model.NightStayCharge2 != null ? model.NightStayCharge2 : 0;
                    db_model.StayCharge2 = model.StayCharge2 != null ? model.StayCharge2 : 0;
                    db_model.TransportCommissionCharge2 = model.TransportCommissionCharge2 != null ? model.TransportCommissionCharge2 : 0;
                    db_model.EntertainmentACharge2 = model.EntertainmentACharge2 != null ? model.EntertainmentACharge2 : 0;
                    db_model.DemerageChargeGiven2 = model.DemerageChargeGiven2 != null ? model.DemerageChargeGiven2 : 0;
                    db_model.TotalGiven2 = model.TotalGiven2 != null ? model.TotalGiven2 : 0;
                    db_model.Note2 = model.Note2;

                    //# Adjustment-Taken
                    db_model.Distance_Empty3 = model.Distance_Empty3 != null ? model.Distance_Empty3 : 0;
                    db_model.Distance_Loaded3 = model.Distance_Loaded3 != null ? model.Distance_Loaded3 : 0;
                    db_model.Distance_Loaded_8_To_12_Tons_Out3 = model.Distance_Loaded_8_To_12_Tons_Out3 != null ? model.Distance_Loaded_8_To_12_Tons_Out3 : 0;
                    db_model.Distance_Loaded_12_To_25_Tons_Out3 = model.Distance_Loaded_12_To_25_Tons_Out3 != null ? model.Distance_Loaded_12_To_25_Tons_Out3 : 0;
                    db_model.FuelConsumedLitre3 = model.FuelConsumedLitre3 != null ? model.FuelConsumedLitre3 : 0;
                    db_model.FuelPricePerLitre3 = model.FuelPricePerLitre3 != null ? model.FuelPricePerLitre3 : 0;
                    db_model.FuelExpence3 = model.FuelExpence3 != null ? model.FuelExpence3 : 0;
                    db_model.BridgeTollFerryCharge3 = model.BridgeTollFerryCharge3 != null ? model.BridgeTollFerryCharge3 : 0;
                    db_model.DemerageChargeTaken3 = model.DemerageChargeTaken3 != null ? model.DemerageChargeTaken3 : 0;
                    db_model.TPTsMoney3 = model.TPTsMoney3 != null ? model.TPTsMoney3 : 0;
                    db_model.AdjustedEvAmount = model.AdjustedEvAmount != null ? model.AdjustedEvAmount : 0;
                    db_model.TotalGiven2 = model.TotalGiven2 != null ? model.TotalGiven2 : 0;
                    db_model.TotalTaken3 = model.TotalTaken3 != null ? model.TotalTaken3 : 0;
                    db_model.Note3 = model.Note3;
                    db_model.NetProfit4 = model.NetProfit4 != null ? model.NetProfit4 : 0;
                    db_model.GrossProfit4 = model.GrossProfit4 != null ? model.GrossProfit4 : 0;
                    db_model.DepositeToAcccountLocation = model.DepositeToAcccountLocation;

                    bll.db.SaveChanges(); CreateAlertMessage(AlertMessageType.Success, "Success", "Instant Requisition is successfully adjusted.");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Instant Requisition is already adjusted.");
                }

                return RedirectToAction("IndexBy_PaymentReceiver");
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult AddMRR(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.InstantRequisitions.Where(m => m.PK_InstantRequisition == id).FirstOrDefault();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Instant Requisition not found.");
                return RedirectToAction("IndexBy_PaymentReceiver");
            }
        }
        [HttpPost]
        public ActionResult AddMRR(InstantRequisition model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var db_model = bll.db.InstantRequisitions.Where(m => m.PK_InstantRequisition == model.PK_InstantRequisition).FirstOrDefault();
            if (db_model != null)
            {
                if (db_model.Status == 2)
                {
                    db_model.Status = 3;
                    db_model.MRR = model.MRR;
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Instant Requisition MRR is added.");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Instant Requisition MRR is already added.");
                }
                return RedirectToAction("IndexBy_PaymentReceiver");
            }
            else
            {
                return HttpNotFound();
            }
        }


        public JsonResult GetVehicleKPLs(Guid PK_Vehicle)
        {
            var res = bll.db.Vehicles.Where(m => m.PK_Vehicle == PK_Vehicle)
                .Select(m => new
                {
                    m.Internal_KPL_Loaded,
                    m.Internal_KPL_Empty,
                    m.Internal_KPL_Loaded_8_To_12_Tons_Out,
                    m.Internal_KPL_Loaded_12_To_25_Tons_Out
                }).FirstOrDefault();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //# Ajax
        public JsonResult FindOwnVehicle(string RegistrationNumber)
        {
            var list = bll.db.Vehicles.Where(m => m.IsDeleted != true && (m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT") && m.RegistrationNumber.Contains(RegistrationNumber)).Select(m =>
               new
               {
                   m.PK_Vehicle,
                   m.RegistrationNumber,
                   m.OWN_MHT_DHT,
                   ContactNumber = (m.Internal_VehicleContactNumber != null ? m.Internal_VehicleContactNumber : "") + (m.MHT_DHT_DriverContactNumber != null ? m.MHT_DHT_DriverContactNumber : "")
               }
            ).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }


}