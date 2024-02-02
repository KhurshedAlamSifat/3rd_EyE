using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class RandomTaskController : Controller
    {
        public DBEnityModelContainer db = new DBEnityModelContainer();
        public ActionResult Report_1(string FK_Locaiton)
        {
            var _FK_Location = Guid.Parse(FK_Locaiton);
            var query_1 = db.Vehicles.Where(m => m.LocationInOrOut == true && m.FK_LocationInOut == _FK_Location && m.VehicleTrackingInformation.VehicleTracking != null);
            var list_1 = query_1.Select(m => new
            {
                m.PK_Vehicle,
                m.RegistrationNumber,
                m.LocationInOutTime
                //m.VehicleTrackingInformation.VehicleTracking.Latitude
            }).Take(20).ToList();
            var datatable_1 = GetDataTable(list_1);
            var dictList_1 = GetDictionaryList(datatable_1);

            List<Dictionary<string, string>> dictList_2 = new List<Dictionary<string, string>>();
            foreach (var item in list_1)
            {
                DataTable dataTable = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = int.MaxValue;
                SqlDataAdapter adpt = new SqlDataAdapter();
                cmd.Connection = (SqlConnection)db.Database.Connection;
                string query = "";
                query = "EXEC Report_GetVehicleHistory '00000000-0000-0000-0000-000000000000', '" + item.PK_Vehicle + "', '" + item.LocationInOutTime.Value.AddMinutes(-5).ToString() + "', '" + item.LocationInOutTime.Value.AddMinutes(5).ToString() + "', " + "1";
                cmd.CommandText = query;
                adpt.SelectCommand = cmd;
                dataTable.Reset();
                adpt.Fill(dataTable);
                dictList_2.AddRange(GetDictionaryList(dataTable));
            }




            return View(new Tuple<List<Dictionary<string, string>>, List<Dictionary<string, string>>>(dictList_1, dictList_2));
        }




        #region supporting methods
        public DataTable GetDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static List<Dictionary<string, string>> GetDictionaryList(DataTable dtData)
        {
            List<Dictionary<string, string>> lstRows = new List<Dictionary<string, string>>();
            Dictionary<string, string> dictRow = null;

            foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, string>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col].ToString());
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }
        #endregion
    }
}