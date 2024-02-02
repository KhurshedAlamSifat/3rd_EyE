using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rdEyE.ViewModels
{
    public class VM_HRISAPIData
    {
        public List<StaffResult> StaffResult { get; set; }
    }
    public class StaffResult
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string CONTACTNO { get; set; }
        public string EMAIL { get; set; }
        public string GROUPNAME { get; set; }
        public string COMPANY { get; set; }
        public string DEPARTMENT { get; set; }
        public string LOCATIONNAME { get; set; }
        public string DESIGNATION { get; set; }
        public string STATUS { get; set; }
        public string GENDER { get; set; }
    }
}