using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rdEyE.ViewModels
{
	public class VM_HRISImageAPIData
	{
		public string status { get; set; }
		public string message { get; set; }
		public List<staff_personal_details> staff_personal_details { get; set; }
	}
	public class staff_personal_details
	{
		public string staff_id { get; set; }
		public string employee_name { get; set; }
		public string tin { get; set; }
		public string marital_status { get; set; }
		public string nid { get; set; }
		public string birth_certificate { get; set; }
		public string blood_group { get; set; }
		public string date_of_birth { get; set; }
		public string passportid { get; set; }
		public string photo { get; set; }
		public string imageurl { get; set; }
	}
}