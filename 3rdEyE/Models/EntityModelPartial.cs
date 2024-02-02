using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rdEyE.Models
{
    public partial class Requisition
    {
        public HttpPostedFileBase AttachedFile1 { get; set; }
        public HttpPostedFileBase AttachedFile2 { get; set; }
        public HttpPostedFileBase AttachedFile3 { get; set; }
    }
}