//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _3rdEyE.Models
{
    using System;
    
    public partial class Report_GetVehicleHaltTime_ReadyReport_Result
    {
        public long PK { get; set; }
        public Nullable<System.Guid> FK_Vehicle { get; set; }
        public string C_rowType { get; set; }
        public string PK_RowData_Start { get; set; }
        public string PK_RowData_End { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<long> HaltTime { get; set; }
        public string EngineStatus { get; set; }
        public Nullable<double> HaltCount { get; set; }
        public Nullable<decimal> TotalDistance { get; set; }
        public Nullable<double> MaximumSpeed { get; set; }
        public Nullable<double> AverageSpeed { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string NearestMapLocation { get; set; }
        public Nullable<double> NearestMapLocationDistance { get; set; }
    }
}
