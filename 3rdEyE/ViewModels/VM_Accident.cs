using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;

namespace _3rdEyE.ViewModels
{
    public class VM_Accident
    {
        public Accident Model { get; set; }
        
        //# Only view property
        public string IsDeleted_Text { get; set; }
        public string OccuranceDate_Text { get; set; }
        public string Status_Text { get; set; }

        public List<AccidentDocument> PoliceCaseDocuments { get; set; }
    }
}