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
    
    public partial class VehicleOutInManual
    {
        public long PK_VehicleOutInManual { get; set; }
        public string DevelopersNote { get; set; }
        public System.Guid FK_Vehicle { get; set; }
        public System.Guid Out_FK_Location { get; set; }
        public System.Guid Out_FK_CreatedByUser { get; set; }
        public System.DateTime Out_IssueDateTime { get; set; }
        public long Out_FK_VehicleInOutManual { get; set; }
        public Nullable<System.Guid> In_FK_Location { get; set; }
        public Nullable<System.Guid> In_FK_CreatedByUser { get; set; }
        public Nullable<System.DateTime> In_IssueDateTime { get; set; }
        public Nullable<long> In_FK_VehicleInOutManual { get; set; }
        public Nullable<long> OutStayTimeMinute { get; set; }
    }
}