using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;
using _3rdEyE.BLL;

namespace _3rdEyE.BLLs
{
    public class BLL_VehicleTrip : BaseBLL
    {

        public List<VM_VehicleTrip> GetAllViewModels()
        {
            var list = db.VehicleTrips.AsEnumerable().Where(c => c.IsDeleted == false).OrderByDescending(m=>m.RowSerial).Select(c => ConvertToViewModel(c)).Take(500).ToList();
            return list;
        }
        public List<VM_VehicleTrip> GetViewModelsBy_AccessibleDepoesofCurrentUser()
        {
            var accessibleDepoes = db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var list = db.VehicleTrips.AsEnumerable().Where(c => c.IsDeleted == false).Where(m => accessibleDepoes.Contains(m.Vehicle.FK_Depo)).Select(c => ConvertToViewModel(c)).ToList();
            return list;
        }
        public VehicleTrip FilterToDBModel_DHT(VehicleTrip model)
        {
            VehicleTrip db_model = FilterToDBModel_Common(model);
            db_model.OWN_MHT_DHT = "DHT";
            db_model.DHT_PartyPoint = db_model.DHT_PartyPoint;
            return db_model;
        }

        public VehicleTrip FilterToDBModel_Common(VehicleTrip model)
        {
            VehicleTrip db_model;
            if (model.PK_VehicleTrip.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                db_model = new VehicleTrip();
                db_model.PK_VehicleTrip = Guid.NewGuid();
                db_model.IsDeleted = false;

                db_model.CreatedAt = DateTime.Now;
                db_model.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;
            }
            else
            {
                db_model = db.VehicleTrips.Find(model.PK_VehicleTrip);
                db_model.UpdatedAt = DateTime.Now;
                db_model.FK_UpdatedByUser = CommonClass.GetCurrentUser().PK_User;
            }
            db_model.FK_Vehicle = model.FK_Vehicle;
            db_model.FK_Depo_To = model.FK_Depo_To;
            db_model.FK_Depo_From = model.FK_Depo_From;
            db_model.DHT_PartyPoint = model.DHT_PartyPoint;
            db_model.FreshChallan= model.FreshChallan;
            db_model.Toll= model.Toll;
            db_model.Bridge= model.Bridge;
            db_model.Ferry = model.Ferry;
            db_model.CityCoorporation_LaborTrustChanda= model.CityCoorporation_LaborTrustChanda;
            db_model.Eant_A= model.Eant_A;
            db_model.OpenTruck = model.OpenTruck;
            db_model.Van_Rickshaw_Boat_TrollerRent= model.Van_Rickshaw_Boat_TrollerRent;
            db_model.AllowanceDriver= model.AllowanceDriver;
            db_model.AllowanceHelper = model.AllowanceHelper;
            db_model.MobileBill= model.MobileBill;
            db_model.LandPort_Export_LCStation_AnotherPartyPoint_Workshop= model.LandPort_Export_LCStation_AnotherPartyPoint_Workshop;
            db_model.LoadUnloadPoint= model.LoadUnloadPoint;
            db_model.NightBill = model.NightBill;
            db_model.Conveyance= model.Conveyance;
            db_model.CarriageOutword = model.CarriageOutword;
            db_model.FactoryDiesel = model.FactoryDiesel;
            db_model.MobileBill= model.MobileBill;
            db_model.Others = model.Others;

            return db_model;
        }
        public VM_VehicleTrip ConvertToViewModel(VehicleTrip model)
        {
            var viewModel = new VM_VehicleTrip();
            viewModel.Model = model;
            viewModel.CreatedAt_Text = CommonClass.ConvertToDateString(model.CreatedAt);
            //# Only view property

            return viewModel;
        }
    }
}