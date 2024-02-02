using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;

namespace _3rdEyE.ViewModels
{
    public class VM_PoliceCase
    {
        public PoliceCase Model { get; set; }
        
        //# Only view property
        public string IsDeleted_Text { get; set; }
        public string IssueDate_Text { get; set; }
        public string IsAlertable_Text { get; set; }
        public string AlertDate_Text { get; set; }
        public string SolvedOn_Text { get; set; }
        public string IsSolved_Text { get; set; }

        public List<PoliceCaseDocument> PoliceCaseDocuments { get; set; }
    }
}