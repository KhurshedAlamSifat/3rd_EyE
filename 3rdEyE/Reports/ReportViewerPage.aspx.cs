using _3rdEyE.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _3rdEyE.Reports
{
    public partial class ReportViewerPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ////#Pattern-1
                //SqlConnection connection = new SqlConnection(@"Data Source=172.17.9.160,1433;Initial Catalog=3rdEyE;Persist Security Info=True;User ID=sa;Password=asdf@1234;MultipleActiveResultSets=True;Application Name=EntityFramework;");
                //SqlCommand command = new SqlCommand("select top 100 * from Depo;", connection);
                //SqlDataAdapter adapter = new SqlDataAdapter(command);
                //DataTable dataTable = new DataTable();
                //adapter.Fill(dataTable);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dataTable);
                //ReportViewer1.LocalReport.DataSources.Add(reportDataSource);

                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report1.rdlc");
                //ReportViewer1.LocalReport.Refresh();

                //#Pattern-2
                DataTable dataTable2 = new DataTable();
                dataTable2.Columns.Add("DataColumn1", typeof(string));
                dataTable2.Columns.Add("DataColumn2", typeof(string));
                dataTable2.Columns.Add("DataColumn3", typeof(string));

                // Add rows with fake data
                for (int i = 0; i < 100; i++)
                {
                    dataTable2.Rows.Add("John Doe", "5000", "Finance");
                    dataTable2.Rows.Add("Jane Smith", "6000", "Human Resources");
                    dataTable2.Rows.Add("Mike Johnson", "4500", "Marketing");
                    dataTable2.Rows.Add("Sarah Thompson", "5500", "IT");
                    dataTable2.Rows.Add("David Lee", "7000", "Sales");
                }
                //ReportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource reportDataSource2 = new ReportDataSource("DataSet1", dataTable2);
                ReportViewer1.LocalReport.DataSources.Add(reportDataSource2);

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report1.rdlc");
                ReportViewer1.LocalReport.Refresh();
            }


        }
    }
}