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
    
    public partial class MonthlyBillEntry
    {
        public long PK_MonthlyBillEntry { get; set; }
        public string PRG_Type { get; set; }
        public string DB_Month { get; set; }
        public Nullable<System.Guid> FK_Depo { get; set; }
        public string DepoName { get; set; }
        public string CompanyName { get; set; }
        public string RegistrationNumber { get; set; }
        public Nullable<System.Guid> FK_Vehicle { get; set; }
        public Nullable<int> DateCount { get; set; }
    }
}
