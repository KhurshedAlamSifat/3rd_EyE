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
    using System.Collections.Generic;
    
    public partial class Report_VehicleOutOverStay
    {
        public long PK_Report_VehicleOutOverStay { get; set; }
        public System.Guid FK_Vehicle { get; set; }
        public System.DateTime Start_UpdateTime { get; set; }
        public double Start_Latitude { get; set; }
        public double Start_Longitude { get; set; }
        public string Start_EngineStatus { get; set; }
        public double Start_Speed { get; set; }
        public string Start_NearestMapLocation { get; set; }
        public string Start_NearestMapLocationDistance { get; set; }
        public System.DateTime Finish_UpdateTime { get; set; }
        public double Finish_Latitude { get; set; }
        public double Finish_Longitude { get; set; }
        public string Finish_EngineStatus { get; set; }
        public double Finish_Speed { get; set; }
        public string Finish_NearestMapLocation { get; set; }
        public string Finish_NearestMapLocationDistance { get; set; }
        public int StayTimeMinute { get; set; }
    
        public virtual Vehicle Vehicle { get; set; }
    }
}
