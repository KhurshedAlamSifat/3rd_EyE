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
    
    public partial class RouteChart
    {
        public long PK_RouteChart { get; set; }
        public System.Guid FK_Depo1 { get; set; }
        public System.Guid FK_Depo2 { get; set; }
        public double Distance { get; set; }
        public decimal DriversMoney { get; set; }
        public decimal HelpersMoney { get; set; }
        public int ApproxTimeHour { get; set; }
    }
}
