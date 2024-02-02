using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;

namespace _3rdEyE.ViewModels
{
    public class VM_Event
    {
        public Event Model { get; set; }
        
        //# Only view property
        public string IsDeleted_Text { get; set; }
        public string IssueDate_Text { get; set; }
        public string DepositDate_Text { get; set; }
        public string IsAlertable_Text { get; set; }
        public string AlertDate_Text { get; set; }
        public string ExpirationDate_Text { get; set; }
        public string AlertOn_Text { get; set; }

        public List<EventDocument> EventDocuments { get; set; }
    }
}