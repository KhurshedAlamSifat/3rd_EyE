using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using _3rdEyE.BLL;
using _3rdEyE.Models;
using Newtonsoft.Json;

namespace _3rdEyE.Controllers
{
    public static class AuditLog
    {
        public static void auditLog(string a, string b)
        {

        }

        internal static void auditLog(string gpsIMEINumber, string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
    public class VTSController : Controller
    {
        public class DB_Helper
        {
            DBEnityModelContainer db = new DBEnityModelContainer();





            public string PushDeviceData_PORT_6060(DB_Helper_Data new_data)
            {

                if (new_data.UpdateTime > DateTime.Now.AddMinutes(5))
                {
                    return "Got Invalid Updatetime from device GpsIMEINumber: " + new_data.GpsIMEINumber.ToString() + ". UpdateTime : " + new_data.UpdateTime;
                }


                try
                {
                    #region AuditLog
                    AuditLog.auditLog((JsonConvert.SerializeObject(new_data)).ToString(), "12");
                    #endregion
                }
                catch (Exception e)
                {

                }

                try
                {
                    string reponseFromProcedure = "";
                    double _distance = 0;
                    var old_data = (from v in db.VehicleTrackingInformations.Where(v => v.GpsIMEINumber == new_data.GpsIMEINumber)
                                    join vt in db.VehicleTrackings on v.PK_Vehicle equals vt.PK_Vehicle
                                    select new
                                    {
                                        v.PK_Vehicle,
                                        v.Internal_ShowTemperature,
                                        vt.Latitude,
                                        vt.Longitude,
                                        vt.Altitude,
                                        vt.EngineStatus,
                                        vt.Course,
                                        vt.Temperature,
                                        vt.Fuel,
                                        vt.Speed,
                                        vt.Distance,
                                        vt.UpdateTime,
                                        vt.ServerTime
                                    }).FirstOrDefault();

                    if (old_data == null)
                    {
                        try
                        {
                            reponseFromProcedure = reponseFromProcedure + db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData_Insert_Insert '" + new_data.GpsIMEINumber + "','" + new_data.UpdateTime.ToString() + "','" + new_data.Latitude + "','" + new_data.Longitude + "','" + new_data.Altitude + "','" + new_data.EngineStatus + "','" + new_data.Course + "','" + new_data.Temperature + "','" + new_data.Fuel + "','" + new_data.Speed + "','" + _distance + "'").FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            #region AuditLog
                            AuditLog.auditLog(ex.Message, "103");
                            #endregion
                        }
                    }
                    else
                    {
                        if (new_data.EngineStatus == "0" && new_data.Speed > 0 && new_data.UpdateTime > old_data.UpdateTime)
                        {
                            try
                            {
                                reponseFromProcedure = db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData_UpdateTime '" + old_data.PK_Vehicle + "','" + new_data.UpdateTime.ToString() + "'").FirstOrDefault();
                            }
                            catch (Exception ex)
                            {
                                #region AuditLog
                                AuditLog.auditLog(ex.Message, "101.1");
                                AuditLog.auditLog("locked in case 101.1", "locked");
                                #endregion
                            }
                        }
                        else if (new_data.UpdateTime > old_data.UpdateTime && old_data.EngineStatus == "0" && (new_data.EngineStatus == "0" || new_data.Speed == 0) && (Math.Round(old_data.Latitude, 2) != Math.Round(new_data.Latitude, 2) || Math.Round(old_data.Longitude, 2) != Math.Round(new_data.Longitude, 2)))
                        {
                            try
                            {
                                new_data.Latitude = old_data.Latitude;
                                new_data.Longitude = old_data.Longitude;
                                _distance = 0;
                                reponseFromProcedure = reponseFromProcedure + db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData_Update_Insert '" + old_data.PK_Vehicle + "','" + new_data.UpdateTime.ToString() + "','" + new_data.Latitude + "','" + new_data.Longitude + "','" + new_data.Altitude + "','" + new_data.EngineStatus + "','" + new_data.Course + "','" + new_data.Temperature + "','" + new_data.Fuel + "','" + new_data.Speed + "','" + _distance + "'").FirstOrDefault();
                            }
                            catch (Exception ex)
                            {
                                #region AuditLog
                                AuditLog.auditLog(ex.Message, "101.2");
                                AuditLog.auditLog("locked in case 101.2", "locked");
                                #endregion
                            }
                        }
                        else if (Math.Round(old_data.Latitude, 3) != Math.Round(new_data.Latitude, 3) || Math.Round(old_data.Longitude, 3) != Math.Round(new_data.Longitude, 3) || Math.Round(old_data.Altitude, 3) != Math.Round(new_data.Altitude, 3) || old_data.EngineStatus != new_data.EngineStatus || old_data.Course != new_data.Course || old_data.Temperature != new_data.Temperature || old_data.Fuel != new_data.Fuel || old_data.Speed != new_data.Speed)
                        {
                            if (new_data.UpdateTime > old_data.UpdateTime)
                            {
                                try
                                {
                                    if (old_data.Latitude != new_data.Latitude || old_data.Longitude != new_data.Longitude)
                                    {
                                        _distance = distanceInKmBetweenEarthCoordinates(Convert.ToDouble(old_data.Latitude), Convert.ToDouble(old_data.Longitude), Convert.ToDouble(new_data.Latitude), Convert.ToDouble(new_data.Longitude));
                                        _distance = Math.Round(_distance, 2);
                                    }

                                    reponseFromProcedure = reponseFromProcedure + db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData_Update_Insert '" + old_data.PK_Vehicle + "','" + new_data.UpdateTime.ToString() + "','" + new_data.Latitude + "','" + new_data.Longitude + "','" + new_data.Altitude + "','" + new_data.EngineStatus + "','" + new_data.Course + "','" + new_data.Temperature + "','" + new_data.Fuel + "','" + new_data.Speed + "','" + _distance + "'").FirstOrDefault();
                                }
                                catch (Exception ex)
                                {
                                    AuditLog.auditLog(ex.Message, "102.1");
                                }
                            }
                            else
                            {
                                try
                                {
                                    _distance = 0;
                                    reponseFromProcedure = reponseFromProcedure + db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData__Insert '" + old_data.PK_Vehicle + "','" + new_data.UpdateTime.ToString() + "','" + new_data.Latitude + "','" + new_data.Longitude + "','" + new_data.Altitude + "','" + new_data.EngineStatus + "','" + new_data.Course + "','" + new_data.Temperature + "','" + new_data.Fuel + "','" + new_data.Speed + "','" + _distance + "'").FirstOrDefault();
                                }
                                catch (Exception ex)
                                {
                                    AuditLog.auditLog(ex.Message, "102.2");
                                }
                            }


                        }
                        else
                        {
                            try
                            {
                                reponseFromProcedure = db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData_UpdateTime '" + old_data.PK_Vehicle + "','" + new_data.UpdateTime.ToString() + "'").FirstOrDefault();
                            }
                            catch (Exception ex)
                            {
                                #region AuditLog
                                AuditLog.auditLog(ex.Message, "101.3");
                                #endregion
                            }
                        }
                    }

                    #region AuditLog
                    AuditLog.auditLog("reponseFromProcedure : " + reponseFromProcedure.ToString(), "100");
                    #endregion

                    return reponseFromProcedure;
                }
                catch (Exception ex)
                {
                    #region AuditLog
                    AuditLog.auditLog(ex.Message, "100");
                    #endregion

                    return "Exception: " + ex.Message;
                }

            }
            public string PushDeviceData_PORT_6061(DB_Helper_Data new_data)
            {

                if (new_data.UpdateTime > DateTime.Now.AddMinutes(5))
                {
                    return "Got Invalid Updatetime from device GpsIMEINumber: " + new_data.GpsIMEINumber.ToString() + ". UpdateTime : " + new_data.UpdateTime;
                }


                try
                {
                    #region AuditLog
                    AuditLog.auditLog(new_data.GpsIMEINumber, (JsonConvert.SerializeObject(new_data)).ToString(), "12");
                    #endregion
                }
                catch (Exception e)
                {

                }

                try
                {
                    string reponseFromProcedure = "";

                    reponseFromProcedure = reponseFromProcedure + db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData_VT1 '" + new_data.GpsIMEINumber + "','" + new_data.UpdateTime.ToString() + "','" + new_data.Latitude + "','" + new_data.Longitude + "','" + new_data.Altitude + "','" + new_data.EngineStatus + "','" + new_data.Course + "','" + new_data.Temperature + "','" + new_data.Fuel + "','" + new_data.Speed + "','" + new_data.Distance + "','" + new_data.RemainingCash + "'").FirstOrDefault();

                    #region AuditLog
                    AuditLog.auditLog(new_data.GpsIMEINumber, "reponseFromProcedure : " + new_data.GpsIMEINumber + " - " + reponseFromProcedure.ToString(), "100");
                    #endregion

                    return reponseFromProcedure;
                }
                catch (Exception ex)
                {
                    #region AuditLog
                    AuditLog.auditLog(new_data.GpsIMEINumber, new_data.GpsIMEINumber + " - " + ex.Message, "100");
                    #endregion

                    return "Exception: " + ex.Message;
                }

            }

            public string _PushDeviceData_PORT_6061(DB_Helper_Data new_data)
            {

                if (new_data.UpdateTime > DateTime.Now.AddMinutes(5))
                {
                    return "Got Invalid Updatetime from device GpsIMEINumber: " + new_data.GpsIMEINumber.ToString() + ". UpdateTime : " + new_data.UpdateTime;
                }


                try
                {
                    #region AuditLog
                    AuditLog.auditLog((JsonConvert.SerializeObject(new_data)).ToString(), "12");
                    #endregion
                }
                catch (Exception e)
                {

                }

                try
                {
                    string reponseFromProcedure = "";
                    double _distance = 0;
                    var old_data = (from v in db.VehicleTrackingInformations.Where(v => v.GpsIMEINumber == new_data.GpsIMEINumber)
                                    join vt in db.VehicleTrackings on v.PK_Vehicle equals vt.PK_Vehicle
                                    select new
                                    {
                                        v.PK_Vehicle,
                                        v.Internal_ShowTemperature,
                                        vt.Latitude,
                                        vt.Longitude,
                                        vt.Altitude,
                                        vt.EngineStatus,
                                        vt.Course,
                                        vt.Temperature,
                                        vt.Fuel,
                                        vt.Speed,
                                        vt.Distance,
                                        vt.UpdateTime,
                                        vt.ServerTime
                                    }).FirstOrDefault();

                    if (old_data == null)
                    {
                        try
                        {
                            reponseFromProcedure = reponseFromProcedure + db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData_Insert_Insert '" + new_data.GpsIMEINumber + "','" + new_data.UpdateTime.ToString() + "','" + new_data.Latitude + "','" + new_data.Longitude + "','" + new_data.Altitude + "','" + new_data.EngineStatus + "','" + new_data.Course + "','" + new_data.Temperature + "','" + new_data.Fuel + "','" + new_data.Speed + "','" + _distance + "'").FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            #region AuditLog
                            AuditLog.auditLog(ex.Message, "103");
                            #endregion
                        }
                    }
                    else
                    {
                        try
                        {
                            if (old_data.Latitude != new_data.Latitude || old_data.Longitude != new_data.Longitude)
                            {
                                _distance = distanceInKmBetweenEarthCoordinates(Convert.ToDouble(old_data.Latitude), Convert.ToDouble(old_data.Longitude), Convert.ToDouble(new_data.Latitude), Convert.ToDouble(new_data.Longitude));
                                _distance = Math.Round(_distance, 2);
                            }

                            reponseFromProcedure = reponseFromProcedure + db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData_Update_Insert '" + old_data.PK_Vehicle + "','" + new_data.UpdateTime.ToString() + "','" + new_data.Latitude + "','" + new_data.Longitude + "','" + new_data.Altitude + "','" + new_data.EngineStatus + "','" + new_data.Course + "','" + new_data.Temperature + "','" + new_data.Fuel + "','" + new_data.Speed + "','" + _distance + "'").FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            AuditLog.auditLog(ex.Message, "102.1");
                        }
                    }

                    #region AuditLog
                    AuditLog.auditLog("reponseFromProcedure : " + reponseFromProcedure.ToString(), "100");
                    #endregion

                    return reponseFromProcedure;
                }
                catch (Exception ex)
                {
                    #region AuditLog
                    AuditLog.auditLog(ex.Message, "100");
                    #endregion

                    return "Exception: " + ex.Message;
                }

            }

            public string PushDeviceData_PORT_6062(DB_Helper_Data new_data)
            {

                if (new_data.UpdateTime > DateTime.Now.AddMinutes(5))
                {
                    return "Got Invalid Updatetime from device GpsIMEINumber: " + new_data.GpsIMEINumber.ToString() + ". UpdateTime : " + new_data.UpdateTime;
                }


                try
                {
                    #region AuditLog
                    AuditLog.auditLog((JsonConvert.SerializeObject(new_data)).ToString(), "12");
                    #endregion
                }
                catch (Exception e)
                {

                }

                try
                {
                    string reponseFromProcedure = "";
                    double _distance = 0;
                    var old_data = (from v in db.VehicleTrackingInformations.Where(v => v.GpsIMEINumber == new_data.GpsIMEINumber)
                                    join vt in db.VehicleTrackings on v.PK_Vehicle equals vt.PK_Vehicle
                                    select new
                                    {
                                        v.PK_Vehicle,
                                        v.Internal_ShowTemperature,
                                        vt.Latitude,
                                        vt.Longitude,
                                        vt.Altitude,
                                        vt.EngineStatus,
                                        vt.Course,
                                        vt.Temperature,
                                        vt.Fuel,
                                        vt.Speed,
                                        vt.Distance,
                                        vt.UpdateTime,
                                        vt.ServerTime
                                    }).FirstOrDefault();

                    if (old_data == null)
                    {
                        try
                        {
                            reponseFromProcedure = reponseFromProcedure + db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData_Insert_Insert '" + new_data.GpsIMEINumber + "','" + new_data.UpdateTime.ToString() + "','" + new_data.Latitude + "','" + new_data.Longitude + "','" + new_data.Altitude + "','" + new_data.EngineStatus + "','" + new_data.Course + "','" + new_data.Temperature + "','" + new_data.Fuel + "','" + new_data.Speed + "','" + _distance + "'").FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            #region AuditLog
                            AuditLog.auditLog(ex.Message, "103");
                            #endregion
                        }
                    }
                    else
                    {
                        //# Temperature Calculation
                        if (old_data.Internal_ShowTemperature == true)
                        {
                            if (new_data.Temperature == 0)
                            {
                                new_data.Temperature = old_data.Temperature;
                            }
                            else if (new_data.Temperature > 50)
                            {
                                new_data.Temperature = old_data.Temperature;
                            }
                            else
                            {
                                //# keep as it is
                            }
                        }
                        else
                        {
                            new_data.Temperature = 0;
                        }


                        if (new_data.EngineStatus == "0" && new_data.Speed > 0 && new_data.UpdateTime > old_data.UpdateTime)
                        {
                            try
                            {
                                reponseFromProcedure = db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData_UpdateTime '" + old_data.PK_Vehicle + "','" + new_data.UpdateTime.ToString() + "'").FirstOrDefault();
                            }
                            catch (Exception ex)
                            {
                                #region AuditLog
                                AuditLog.auditLog(ex.Message, "101.1");
                                AuditLog.auditLog("locked in case 101.1", "locked");
                                #endregion
                            }
                        }
                        else if (new_data.UpdateTime > old_data.UpdateTime && old_data.EngineStatus == "0" && (new_data.EngineStatus == "0" || new_data.Speed == 0) && (Math.Round(old_data.Latitude, 2) != Math.Round(new_data.Latitude, 2) || Math.Round(old_data.Longitude, 2) != Math.Round(new_data.Longitude, 2)))
                        {
                            try
                            {
                                new_data.Latitude = old_data.Latitude;
                                new_data.Longitude = old_data.Longitude;
                                _distance = 0;
                                reponseFromProcedure = reponseFromProcedure + db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData_Update_Insert '" + old_data.PK_Vehicle + "','" + new_data.UpdateTime.ToString() + "','" + new_data.Latitude + "','" + new_data.Longitude + "','" + new_data.Altitude + "','" + new_data.EngineStatus + "','" + new_data.Course + "','" + new_data.Temperature + "','" + new_data.Fuel + "','" + new_data.Speed + "','" + _distance + "'").FirstOrDefault();
                            }
                            catch (Exception ex)
                            {
                                #region AuditLog
                                AuditLog.auditLog(ex.Message, "101.2");
                                AuditLog.auditLog("locked in case 101.2", "locked");
                                #endregion
                            }
                        }
                        else if (Math.Round(old_data.Latitude, 3) != Math.Round(new_data.Latitude, 3) || Math.Round(old_data.Longitude, 3) != Math.Round(new_data.Longitude, 3) || Math.Round(old_data.Altitude, 3) != Math.Round(new_data.Altitude, 3) || old_data.EngineStatus != new_data.EngineStatus || old_data.Course != new_data.Course || old_data.Temperature != new_data.Temperature || old_data.Fuel != new_data.Fuel || old_data.Speed != new_data.Speed)
                        {
                            if (new_data.UpdateTime > old_data.UpdateTime)
                            {
                                try
                                {
                                    if (old_data.Latitude != new_data.Latitude || old_data.Longitude != new_data.Longitude)
                                    {
                                        _distance = distanceInKmBetweenEarthCoordinates(Convert.ToDouble(old_data.Latitude), Convert.ToDouble(old_data.Longitude), Convert.ToDouble(new_data.Latitude), Convert.ToDouble(new_data.Longitude));
                                        _distance = Math.Round(_distance, 2);
                                    }

                                    reponseFromProcedure = reponseFromProcedure + db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData_Update_Insert '" + old_data.PK_Vehicle + "','" + new_data.UpdateTime.ToString() + "','" + new_data.Latitude + "','" + new_data.Longitude + "','" + new_data.Altitude + "','" + new_data.EngineStatus + "','" + new_data.Course + "','" + new_data.Temperature + "','" + new_data.Fuel + "','" + new_data.Speed + "','" + _distance + "'").FirstOrDefault();
                                }
                                catch (Exception ex)
                                {
                                    AuditLog.auditLog(ex.Message, "102.1");
                                }
                            }
                            else
                            {
                                try
                                {
                                    _distance = 0;
                                    reponseFromProcedure = reponseFromProcedure + db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData__Insert '" + old_data.PK_Vehicle + "','" + new_data.UpdateTime.ToString() + "','" + new_data.Latitude + "','" + new_data.Longitude + "','" + new_data.Altitude + "','" + new_data.EngineStatus + "','" + new_data.Course + "','" + new_data.Temperature + "','" + new_data.Fuel + "','" + new_data.Speed + "','" + _distance + "'").FirstOrDefault();
                                }
                                catch (Exception ex)
                                {
                                    AuditLog.auditLog(ex.Message, "102.2");
                                }
                            }


                        }
                        else
                        {
                            try
                            {
                                reponseFromProcedure = db.Database.SqlQuery<string>("EXEC dbo.PushDeviceData_UpdateTime '" + old_data.PK_Vehicle + "','" + new_data.UpdateTime.ToString() + "'").FirstOrDefault();
                            }
                            catch (Exception ex)
                            {
                                #region AuditLog
                                AuditLog.auditLog(ex.Message, "101.3");
                                #endregion
                            }
                        }
                    }

                    #region AuditLog
                    AuditLog.auditLog("reponseFromProcedure : " + reponseFromProcedure.ToString(), "100");
                    #endregion

                    return reponseFromProcedure;
                }
                catch (Exception ex)
                {
                    #region AuditLog
                    AuditLog.auditLog(ex.Message, "100");
                    #endregion

                    return "Exception: " + ex.Message;
                }

            }







            public string CleanDeviceData()
            {
                string reponseFromProcedure = "";
                try
                {
                    reponseFromProcedure = db.Database.SqlQuery<string>("EXEC dbo.CleanDeviceData").FirstOrDefault();
                    return reponseFromProcedure;
                }
                catch (Exception ex)
                {
                    #region AuditLog
                    AuditLog.auditLog(ex.Message, "100");
                    #endregion

                    return "Exception: " + ex.Message;
                }
            }





            public double degreesToRadians(double degrees)
            {
                return degrees * Math.PI / 180;
            }

            public double distanceInKmBetweenEarthCoordinates(double lat1, double lon1, double lat2, double lon2)
            {
                var earthRadiusKm = 6371;

                var dLat = degreesToRadians(lat2 - lat1);
                var dLon = degreesToRadians(lon2 - lon1);

                lat1 = degreesToRadians(lat1);
                lat2 = degreesToRadians(lat2);

                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return earthRadiusKm * c;
            }

        }

        public class DB_Helper_Data
        {

            public string GpsIMEINumber { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double Altitude { get; set; }
            public string EngineStatus { get; set; }
            public double Course { get; set; }
            public double Temperature { get; set; }
            public double Fuel { get; set; }
            public double Speed { get; set; }
            public string Distance { get; set; }
            public Nullable<int> RemainingCash { get; set; }
            public DateTime UpdateTime { get; set; }

        }
    }

}
