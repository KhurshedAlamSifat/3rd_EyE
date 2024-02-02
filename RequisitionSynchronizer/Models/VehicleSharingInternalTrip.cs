//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RequisitionSynchronizer.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VehicleSharingInternalTrip
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VehicleSharingInternalTrip()
        {
            this.AppUsers = new HashSet<AppUser>();
            this.AppUsers1 = new HashSet<AppUser>();
            this.Vehicles = new HashSet<Vehicle>();
            this.Vehicles1 = new HashSet<Vehicle>();
        }
    
        public long PK_VehicleSharingInternalTrip { get; set; }
        public long FK_VehicleSharing { get; set; }
        public Nullable<System.Guid> FK_Vehicle { get; set; }
        public Nullable<System.Guid> FK_AppUser_Assigner { get; set; }
        public Nullable<System.Guid> FK_AppUser_Driver { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsTest { get; set; }
        public Nullable<System.DateTime> PossibleJourneyStartDateTime { get; set; }
        public Nullable<System.DateTime> AssingedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public string StatusText { get; set; }
        public Nullable<int> PrintCopy { get; set; }
        public string AdjustmentStatusText { get; set; }
        public Nullable<System.DateTime> EnteredStartingLocationDateTime { get; set; }
        public Nullable<System.DateTime> LoadingStartDateTime { get; set; }
        public Nullable<System.DateTime> LoadingDoneDateTime { get; set; }
        public Nullable<System.DateTime> PossibleJourneyFinishDateTime { get; set; }
        public Nullable<int> PossibleJourneyTimeHour { get; set; }
        public Nullable<System.DateTime> LeftStartingLoactionDateTime { get; set; }
        public Nullable<System.DateTime> EnteredFinishingLocationDateTime { get; set; }
        public Nullable<System.DateTime> UnloadingStartDateTime { get; set; }
        public Nullable<System.DateTime> UnloadingDoneDateTime { get; set; }
        public Nullable<System.DateTime> BillCreatedAt { get; set; }
        public Nullable<System.Guid> FK_AppUser_BillCreator { get; set; }
        public Nullable<System.DateTime> BillApprovedAt { get; set; }
        public Nullable<System.Guid> FK_AppUser_BillApprover { get; set; }
        public Nullable<System.DateTime> BillPaidAt { get; set; }
        public Nullable<System.Guid> FK_AppUser_BillPayer { get; set; }
        public Nullable<bool> IsNotifiedToDriver { get; set; }
        public Nullable<System.DateTime> NotifiedToDriverAt { get; set; }
        public Nullable<double> Distance_Empty { get; set; }
        public Nullable<double> KPL_Empty { get; set; }
        public Nullable<double> Distance_Loaded { get; set; }
        public Nullable<double> KPL_Loaded { get; set; }
        public Nullable<double> Distance_Loaded_Plastic { get; set; }
        public Nullable<double> KPL_Loaded_Plastic { get; set; }
        public Nullable<double> Distance_InterCity { get; set; }
        public Nullable<double> KPL_InterCity { get; set; }
        public Nullable<double> Distance_InterCHT { get; set; }
        public Nullable<double> KPL_InterCHT { get; set; }
        public Nullable<double> Distance_Hill { get; set; }
        public Nullable<double> KPL_Hill { get; set; }
        public Nullable<double> Distance_OnlyMover { get; set; }
        public Nullable<double> KPL_OnlyMover { get; set; }
        public Nullable<double> Distance_Loaded_8_To_12_Tons { get; set; }
        public Nullable<double> KPL_Loaded_8_To_12_Tons { get; set; }
        public Nullable<double> Distance_Loaded_12_To_25_Tons { get; set; }
        public Nullable<double> KPL_Loaded_12_To_25_Tons { get; set; }
        public Nullable<decimal> DistanceTrip { get; set; }
        public Nullable<double> DistanceGoogle { get; set; }
        public Nullable<double> DistanceRouteChart { get; set; }
        public Nullable<double> DistanceManual { get; set; }
        public Nullable<double> FuelConsumedLitre { get; set; }
        public Nullable<double> FuelPricePerLitre { get; set; }
        public Nullable<decimal> FuelExpence { get; set; }
        public Nullable<bool> FuelExpenceGivenCashOrOil { get; set; }
        public Nullable<decimal> DriversMoney { get; set; }
        public Nullable<decimal> HelpersMoney { get; set; }
        public Nullable<decimal> BridgeTollFerryCharge { get; set; }
        public Nullable<decimal> LoadingCost { get; set; }
        public Nullable<decimal> UnloadingCost { get; set; }
        public Nullable<decimal> LaborCharge { get; set; }
        public Nullable<decimal> EntertainmentCCharge { get; set; }
        public Nullable<decimal> ParkingCharge { get; set; }
        public Nullable<decimal> EntertainmentACharge { get; set; }
        public Nullable<decimal> RepairCharge { get; set; }
        public Nullable<decimal> OverStayCharge { get; set; }
        public Nullable<decimal> OpenBodyCharge { get; set; }
        public Nullable<decimal> DemurrageCharge { get; set; }
        public Nullable<decimal> TotalExpense { get; set; }
        public string BillingNote { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppUser> AppUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppUser> AppUsers1 { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual AppUser AppUser1 { get; set; }
        public virtual AppUser AppUser2 { get; set; }
        public virtual AppUser AppUser3 { get; set; }
        public virtual AppUser AppUser4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vehicle> Vehicles1 { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual VehicleSharing VehicleSharing { get; set; }
        public virtual VehicleSharingInternalTripAdjustment VehicleSharingInternalTripAdjustment { get; set; }
    }
}
