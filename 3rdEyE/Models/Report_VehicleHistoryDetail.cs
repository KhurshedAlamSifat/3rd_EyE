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
    
    public partial class Report_VehicleHistoryDetail
    {
        public long PK { get; set; }
        public string USER_KEY { get; set; }
        public string FK_Vehicle { get; set; }
        public string UpdateTime { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string EngineStatus { get; set; }
        public string Speed { get; set; }
    }
}