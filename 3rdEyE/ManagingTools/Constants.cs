using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rdEyE.ManagingTools
{
    //# DON'T CHANGE VARIABLE NAMES
    public static class ValidationStatus
    {
        public const string OK = "OK";
    }

    public static class RequistionNotificationCategory
    {
        public const string Individual_Requisition_Created = "Individual_Requisition_Created";
        public const string Individual_Requisition_Bid_Created = "Individual_Requisition_Bid_Created";
        public const string Individual_Requisition_Bid_Updated = "Individual_Requisition_Bid_Updated";
        public const string Individual_Requisition_Approved = "Individual_Requisition_Approved";
        public const string Individual_Requisition_Rejected = "Individual_Requisition_Rejected";

        public const string Contractual_Requisition_Entry_Created = "Contractual_Requisition_Entry_Created";
        public const string Contractual_Requisition_Entry_Approved = "Contractual_Requisition_Entry_Approved";
    }
}