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
    
    public partial class GPS_DeviceChangeLog
    {
        public long PK_GPS_DeviceChangeLog { get; set; }
        public System.Guid FK_Vehicle { get; set; }
        public string GpsDeviceModel { get; set; }
        public string GpsIMEINumber { get; set; }
        public string GpsMobileNumber { get; set; }
        public Nullable<System.Guid> FK_AppUser_CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}