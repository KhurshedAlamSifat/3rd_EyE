using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;

namespace _3rdEyE.ViewModels
{
    public class VM_Company
    {
        //public long RowSerial { get; set; }
        //public System.Guid PK_Company { get; set; }
        //public Nullable<bool> IsDeleted { get; set; }
        //public bool IsActive { get; set; }
        //public System.Guid FK_GroupOfCompany { get; set; }
        //public string Name { get; set; }
        //public string CompanyRegistrationNumber { get; set; }
        //public string CompnayAddress { get; set; }
        //public string ExternalOwnersFullName { get; set; }
        //public string ExternalOwnersNID { get; set; }
        //public string ExternalOwnersFathersName { get; set; }
        //public string ExternalOwnersContactNumber { get; set; }
        //public string ExternalOwnersAddress { get; set; }
        public Company Model { get; set; }

        //# Relational property
        public GroupOfCompany GroupOfCompany { get; set; }

        //# Only view property
        public string IsActive_Text { get; set; }
        public string IsDeleted_Text { get; set; }
    }
}