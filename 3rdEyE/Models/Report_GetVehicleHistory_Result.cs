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
    
    public partial class Report_GetVehicleHistory_Result
    {
        public Nullable<System.Guid> FK_Vehicle { get; set; }
        public long PK { get; set; }
        public System.Guid USER_KEY { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string EngineStatus { get; set; }
        public double Speed { get; set; }
        public string NearestMapLocation { get; set; }
        public string NearestMapLocationDistance { get; set; }
    }
}
