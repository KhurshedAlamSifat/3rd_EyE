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
    
    public partial class AlertEmailLog
    {
        public long PK_AlertEmailLog { get; set; }
        public System.Guid FK_AlertEmail { get; set; }
        public System.DateTime RequestTime { get; set; }
        public string EmailContent { get; set; }
        public bool Status { get; set; }
        public string EorroMessage { get; set; }
    }
}
