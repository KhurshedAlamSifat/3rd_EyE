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
    
    public partial class Report_ConsolidatedRport
    {
        public long PK_RowData { get; set; }
        public string USER_KEY { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Altitude { get; set; }
        public string EngineStatus { get; set; }
        public Nullable<double> Course { get; set; }
        public Nullable<double> Temperature { get; set; }
        public Nullable<double> Fuel { get; set; }
        public Nullable<double> Speed { get; set; }
        public Nullable<decimal> Distance { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<System.DateTime> ServerTime { get; set; }
    }
}
