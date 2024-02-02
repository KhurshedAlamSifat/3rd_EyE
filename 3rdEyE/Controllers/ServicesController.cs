using _3rdEyE.BLL;
using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class ServicesController : BaseController
    {
        public void CallAllServices()
        {
            //Disconnected Device >> TrackingController
            try
            {
                new TrackingController().TrySendDisconnectedDeviceAlertEmail();
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);

                bll.db.ServiceCalls.Add(
                    new ServiceCall()
                    {
                        CallingTime = DateTime.Now,
                        CallingMessage = "Services/CallAllServices: new TrackingController().TrySendDisconnectedDeviceAlertEmail();",
                        UserDefinedMessage = errrorMessage
                    }
                 );
                bll.db.SaveChanges();
            }

            //Event >> EventController
            try
            {
                new EventController().TrySendEventAlertEmail();
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if
                        (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);

                bll.db.ServiceCalls.Add(
                    new ServiceCall()
                    {
                        CallingTime = DateTime.Now,
                        CallingMessage = "Services/CallAllServices: new EventController().TrySendEventAlertEmail();",
                        UserDefinedMessage = errrorMessage
                    }
                 );
                bll.db.SaveChanges();
            }

            //PoliceCase >> PoliceCaseController
            try
            {
                new PoliceCaseController().TrySendPoliceCaseAlertEmail();
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if
                        (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);

                bll.db.ServiceCalls.Add(
                    new ServiceCall()
                    {
                        CallingTime = DateTime.Now,
                        CallingMessage = "Services/CallAllServices: new PoliceCaseController().TrySendPoliceCaseAlertEmail();",
                        UserDefinedMessage = errrorMessage
                    }
                 );
                bll.db.SaveChanges();
            }

            //GenrateHaltReport
            try
            {
                BulkMakeHaltReport();
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
                bll.db.ServiceCalls.Add(
                    new ServiceCall()
                    {
                        CallingTime = DateTime.Now,
                        CallingMessage = "Services/CallAllServices: BulkMakeHaltReport();",
                        UserDefinedMessage = errrorMessage
                    }
                 );
                bll.db.SaveChanges();
            }

            //DatabaseBackUp
            try
            {
                DatabaseBackUp();
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
                bll.db.ServiceCalls.Add(
                    new ServiceCall()
                    {
                        CallingTime = DateTime.Now,
                        CallingMessage = "Services/CallAllServices: DatabaseBackUp();",
                        UserDefinedMessage = errrorMessage
                    }
                 );
                bll.db.SaveChanges();
            }


            ////Monthly Bill
            //var date_1 = DateTime.Now.Day;
            //if (date_1 == 1)
            //{
            //    try
            //    {
            //        new AccountingController().SendMonthlyBillMail_BulkCall();
            //    }
            //    catch (Exception e)
            //    {
            //        var errrorMessage = "";
            //        do
            //        {
            //            errrorMessage = "#" + errrorMessage + e.Message;
            //            if (e.InnerException != null)
            //            {
            //                e = e.InnerException;
            //            }
            //            else
            //            {
            //                break;
            //            }
            //        } while (true);
            //        bll.db.ServiceCalls.Add(
            //            new ServiceCall()
            //            {
            //                CallingTime = DateTime.Now,
            //                CallingMessage = "Services/CallAllServices: AccountingController().SendMonthlyBillMail_BulkCall();",
            //                UserDefinedMessage = errrorMessage
            //            }
            //         );
            //        bll.db.SaveChanges();
            //    }
            //}

        }

        #region CheckVehicleInDepo
        public string CheckVehicleInDepo()
        {
            try
            {
                var currentTime = DateTime.Now.AddMinutes(-5);
                var vehicles = (from v in bll.db.Vehicles
                                join vt in bll.db.VehicleTrackings.Where(m => m.UpdateTime > currentTime)
                                on v.PK_Vehicle equals vt.PK_Vehicle
                                select v
                            ).ToList();
                var depoes = bll.db.Depoes.Where(m => m.DepoBorders.Count() > 2).ToList();
                var rfl_depoes = depoes.Where(m => m.PRG_Type == "RFL").ToList();
                var pran_depoes = depoes.Where(m => m.PRG_Type == "PRAN").ToList();
                var queries = "";
                foreach (var vehicle in vehicles)
                {
                    var _found = false;
                    Depo _foundDepo = new Depo();
                    if (vehicle.Depo.PRG_Type == "PRAN")
                    {
                        foreach (var depo in pran_depoes)
                        {
                            LatLong[] borderPoints = depo.DepoBorders.Select(m => new LatLong() { Latitude = m.Latitude, Longitude = m.Longitude }).ToArray();
                            var point = new LatLong() { Latitude = vehicle.VehicleTrackingInformation.VehicleTracking.Latitude, Longitude = vehicle.VehicleTrackingInformation.VehicleTracking.Longitude };
                            var IsintInborderPoints = IsPointInborderPoints(borderPoints, point);
                            if (IsintInborderPoints == true)
                            {
                                if (vehicle.VehicleTrackingInformation.VehicleTracking.FK_Depo_In != depo.PK_Depo)
                                {
                                    vehicle.VehicleTrackingInformation.VehicleTracking.DepoInDateTime = DateTime.Now;
                                }
                                vehicle.VehicleTrackingInformation.VehicleTracking.FK_Depo_In = depo.PK_Depo;
                                queries = queries + "EXEC dbo.VehicleInDepo_Insert '" + vehicle.PK_Vehicle + "','" + depo.PK_Depo + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.UpdateTime + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.Latitude + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.Longitude + "'; ";
                                _found = true;
                                _foundDepo = depo;
                                break;
                            }
                        }
                        if (_found == false)
                        {
                            foreach (var depo in rfl_depoes)
                            {
                                LatLong[] borderPoints = depo.DepoBorders.Select(m => new LatLong() { Latitude = m.Latitude, Longitude = m.Longitude }).ToArray();
                                var point = new LatLong() { Latitude = vehicle.VehicleTrackingInformation.VehicleTracking.Latitude, Longitude = vehicle.VehicleTrackingInformation.VehicleTracking.Longitude };
                                var IsintInborderPoints = IsPointInborderPoints(borderPoints, point);
                                if (IsintInborderPoints == true)
                                {
                                    if (vehicle.VehicleTrackingInformation.VehicleTracking.FK_Depo_In != depo.PK_Depo)
                                    {
                                        vehicle.VehicleTrackingInformation.VehicleTracking.DepoInDateTime = DateTime.Now;
                                    }
                                    vehicle.VehicleTrackingInformation.VehicleTracking.FK_Depo_In = depo.PK_Depo;
                                    queries = queries + "EXEC dbo.VehicleInDepo_Insert '" + vehicle.PK_Vehicle + "','" + depo.PK_Depo + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.UpdateTime + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.Latitude + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.Longitude + "'; ";
                                    _found = true;
                                    _foundDepo = depo;
                                    break;
                                }
                            }
                        }
                    }
                    if (vehicle.Depo.PRG_Type == "RFL")
                    {
                        foreach (var depo in rfl_depoes)
                        {
                            LatLong[] borderPoints = depo.DepoBorders.Select(m => new LatLong() { Latitude = m.Latitude, Longitude = m.Longitude }).ToArray();
                            var point = new LatLong() { Latitude = vehicle.VehicleTrackingInformation.VehicleTracking.Latitude, Longitude = vehicle.VehicleTrackingInformation.VehicleTracking.Longitude };
                            var IsintInborderPoints = IsPointInborderPoints(borderPoints, point);
                            if (IsintInborderPoints == true)
                            {
                                if (vehicle.VehicleTrackingInformation.VehicleTracking.FK_Depo_In != depo.PK_Depo)
                                {
                                    vehicle.VehicleTrackingInformation.VehicleTracking.DepoInDateTime = DateTime.Now;
                                }
                                vehicle.VehicleTrackingInformation.VehicleTracking.FK_Depo_In = depo.PK_Depo;
                                queries = queries + "EXEC dbo.VehicleInDepo_Insert '" + vehicle.PK_Vehicle + "','" + depo.PK_Depo + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.UpdateTime + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.Latitude + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.Longitude + "'; ";
                                _found = true;
                                _foundDepo = depo;
                                break;
                            }
                        }
                        if (_found == false)
                        {
                            foreach (var depo in pran_depoes)
                            {
                                LatLong[] borderPoints = depo.DepoBorders.Select(m => new LatLong() { Latitude = m.Latitude, Longitude = m.Longitude }).ToArray();
                                var point = new LatLong() { Latitude = vehicle.VehicleTrackingInformation.VehicleTracking.Latitude, Longitude = vehicle.VehicleTrackingInformation.VehicleTracking.Longitude };
                                var IsintInborderPoints = IsPointInborderPoints(borderPoints, point);
                                if (IsintInborderPoints == true)
                                {
                                    if (vehicle.VehicleTrackingInformation.VehicleTracking.FK_Depo_In != depo.PK_Depo)
                                    {
                                        vehicle.VehicleTrackingInformation.VehicleTracking.DepoInDateTime = DateTime.Now;
                                    }
                                    vehicle.VehicleTrackingInformation.VehicleTracking.FK_Depo_In = depo.PK_Depo;
                                    queries = queries + "EXEC dbo.VehicleInDepo_Insert '" + vehicle.PK_Vehicle + "','" + depo.PK_Depo + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.UpdateTime + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.Latitude + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.Longitude + "'; ";
                                    _found = true;
                                    _foundDepo = depo;
                                    break;
                                }
                            }
                        }
                    }
                    if (_found == false)
                    {
                        queries = queries + "EXEC dbo.VehicleInDepo_Insert '" + vehicle.PK_Vehicle + "', null,'" + vehicle.VehicleTrackingInformation.VehicleTracking.UpdateTime + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.Latitude + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.Longitude + "'; ";
                        if (vehicle.VehicleTrackingInformation.VehicleTracking.FK_Depo_In != null)
                        {
                            //queries = queries + "EXEC dbo.VehicleInDepo_Insert '" + vehicle.PK_Vehicle + "', null,'" + vehicle.VehicleTrackingInformation.VehicleTracking.UpdateTime + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.Latitude + "','" + vehicle.VehicleTrackingInformation.VehicleTracking.Longitude + "'; ";
                            vehicle.VehicleTrackingInformation.VehicleTracking.FK_Depo_Out = vehicle.VehicleTrackingInformation.VehicleTracking.FK_Depo_In;
                            vehicle.VehicleTrackingInformation.VehicleTracking.DepoOutDateTime = DateTime.Now;
                        }
                        vehicle.VehicleTrackingInformation.VehicleTracking.FK_Depo_In = null;
                    }
                    else if (_foundDepo.Category == "Restricted Area")
                    {
                        if (_foundDepo.PRG_Type == "PRAN")
                        {
                            var _bodyText = vehicle.RegistrationNumber + " found in " + _foundDepo.Name + " which is a restricted area at " + vehicle.VehicleTrackingInformation.VehicleTracking.UpdateTime;
                            var MailToList = new List<string>() { "automation17@mis.prangroup.com", "dist100@prangroup.com" };
                            var MailCCList = new List<string>() { };
                            SendMail_Multiple(MailToList, MailCCList, "Vehicle Entrance In Restricted Area", _bodyText);

                        }
                        else if (_foundDepo.PRG_Type == "RFL")
                        {
                            var _bodyText = vehicle.RegistrationNumber + " found in " + _foundDepo.Name + " which is a restricted area at " + vehicle.VehicleTrackingInformation.VehicleTracking.UpdateTime;
                            var MailToList = new List<string>() { "automation17@mis.prangroup.com" };
                            var MailCCList = new List<string>() { };
                            SendMail_Multiple(MailToList, MailCCList, "Vehicle Entrance In Restricted Area", _bodyText);
                        }
                    }
                }

                bll.db.SaveChanges();
                if (queries != "")
                {
                    bll.db.Database.SqlQuery<object>(queries).FirstOrDefault();
                }

            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                  new AppErrorLog()
                  {
                      ErrorMessage = e.Message,
                      ErrorTime = DateTime.Now,
                      UserDefinedMessage = "Services/CheckVehicleInDepo"
                  }
                  );
                bll.db.SaveChanges();
            }
            return "Done";
        }
        public bool IsPointInborderPoints(LatLong[] borderPoints, LatLong point)
        {
            bool result = false;
            int j = borderPoints.Count() - 1;
            for (int i = 0; i < borderPoints.Count(); i++)
            {
                if (borderPoints[i].Longitude < point.Longitude && borderPoints[j].Longitude >= point.Longitude || borderPoints[j].Longitude < point.Longitude && borderPoints[i].Longitude >= point.Longitude)
                {
                    if (borderPoints[i].Latitude + (point.Longitude - borderPoints[i].Longitude) / (borderPoints[j].Longitude - borderPoints[i].Longitude) * (borderPoints[j].Latitude - borderPoints[i].Latitude) < point.Latitude)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
        public class LatLong
        {
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
        }
        #endregion

        #region Auto Generate Halt Report
        public string BulkMakeHaltReport()
        {

            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "BulkMakeHaltReport-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();

            try
            {
                var startingDate = DateTime.Now.AddDays(-1).Date;
                var list = (from v in bll.db.Vehicles
                            join vt in bll.db.VehicleTrackings on v.PK_Vehicle equals vt.PK_Vehicle
                            select new
                            {
                                v.PK_Vehicle,
                                v.RegistrationNumber
                            }).OrderBy(m => m.RegistrationNumber).ToList();

                bll.db.Database.CommandTimeout = 32000;
                for (int i = 0; i < list.Count; i++)
                {
                    var query = "";
                    query = "EXEC GenerateReport_GetVehicleHaltTime_Helper_Helper '" + list[i].PK_Vehicle + "', '" + startingDate.ToString() + "'";
                    bll.db.Database.ExecuteSqlCommand(query);
                    query = "EXEC GenerateReport_GetVehicleHaltTime_Helper '" + list[i].PK_Vehicle + "', '" + startingDate.ToString() + "', '" + startingDate.AddDays(1).ToString() + "'";
                    bll.db.Database.ExecuteSqlCommand(query);

                    query = "EXEC GenerateReport_GetVehicleHaltTime '" + list[i].PK_Vehicle + "', '" + startingDate.ToString() + "', '" + startingDate.AddDays(1).ToString() + "'";
                    bll.db.Database.ExecuteSqlCommand(query);
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
                bll.db.ServiceCalls.Add(
                new ServiceCall()
                {
                    CallingTime = DateTime.Now,
                    CallingMessage = "BulkMakeHaltReport-Error" + guid,
                    UserDefinedMessage = errrorMessage
                }
                );
                bll.db.SaveChanges();
            }

            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "BulkMakeHaltReport-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            return "Done";
        }
        #endregion

        #region Auto BackUp Database
        public string DatabaseBackUp()
        {
            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "DatabaseBackUp-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();

            try
            {
                var query = "";
                query = "EXEC DataBaseBackup";
                bll.db.Database.CommandTimeout = int.MaxValue;
                bll.db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, query);
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
                bll.db.ServiceCalls.Add(
                new ServiceCall()
                {
                    CallingTime = DateTime.Now,
                    CallingMessage = "DatabaseBackUp-Error" + guid,
                    UserDefinedMessage = errrorMessage
                }
                );
                bll.db.SaveChanges();
            }

            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "DatabaseBackUp-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            return "Done";
        }
        #endregion

        #region Auto BackUp Database
        public string VTS_InactiveLogEntry()
        {
            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "VTS_InactiveLogEntry-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""

                  }
                  );
            bll.db.SaveChanges();

            try
            {
                var now = DateTime.Now;
                var minLimit = now.AddDays(-1);
                var listToAdd = new List<VTS_InactiveLog>();
                var list1 = bll.db.VehicleTrackings.Where(m => m.ServerTime < minLimit && m.VehicleTrackingInformation.GpsMobileNumber != null).ToList();

                foreach (var item in list1)
                {
                    listToAdd.Add(new VTS_InactiveLog()
                    {
                        ReportDate = now.Date,
                        FK_Vehicle = item.PK_Vehicle,
                        SimNumber = item.VehicleTrackingInformation.GpsMobileNumber,
                        ReportTime = now,
                        LastConnectedTime = item.ServerTime
                    });
                }
                bll.db.VTS_InactiveLog.AddRange(listToAdd);
                bll.db.SaveChanges();

                var list2 = bll.db.VehicleTrackingVT1.Where(m => m.ServerTime < minLimit && m.VehicleTrackingInformation.GpsMobileNumber != null).ToList();
                foreach (var item in list2)
                {
                    listToAdd.Add(new VTS_InactiveLog()
                    {
                        ReportDate = now.Date,
                        FK_Vehicle = item.PK_Vehicle,
                        SimNumber = item.VehicleTrackingInformation.GpsMobileNumber,
                        ReportTime = now,
                        LastConnectedTime = item.ServerTime
                    });
                }
                bll.db.VTS_InactiveLog.AddRange(listToAdd);
                bll.db.SaveChanges();

            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
                bll.db.ServiceCalls.Add(
                new ServiceCall()
                {
                    CallingTime = DateTime.Now,
                    CallingMessage = "VTS_InactiveLogEntry-Error" + guid,
                    UserDefinedMessage = errrorMessage
                }
                );
                bll.db.SaveChanges();
            }

            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "VTS_InactiveLogEntry-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            return "Done";
        }
        #endregion

        #region Auto BackUp Database
        public string DatabaseRegularJob()
        {
            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "DatabaseRegularJob-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();

            try
            {
                var query = "";
                query = "EXEC SP_JOB_RegularJob";
                bll.db.Database.CommandTimeout = int.MaxValue;
                bll.db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, query);
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
                bll.db.ServiceCalls.Add(
                new ServiceCall()
                {
                    CallingTime = DateTime.Now,
                    CallingMessage = "DatabaseRegularJob-Error" + guid,
                    UserDefinedMessage = errrorMessage
                }
                );
                bll.db.SaveChanges();
            }

            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "DatabaseRegularJob-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            return "Done";
        }
        #endregion

        #region SMS vehirifcation
        public string VTS_SIM_Verification()
        {

            var guid = Guid.NewGuid();
            //bll.db.ServiceCalls.Add(
            //      new ServiceCall()
            //      {
            //          CallingMessage = "VTS_SIM_Verification-Start-" + guid,
            //          CallingTime = DateTime.Now,
            //          UserDefinedMessage = ""
            //      }
            //      );
            //bll.db.SaveChanges();

            try
            {
                var startingDate = DateTime.Now.AddDays(-1).Date;

                //          ,[SimNumber]
                //,[AppVehicleRegistration]
                //,[AppVehicleIMEI]
                //,[SentAt]
                //,[SentMessage]
                //,[ReceivedAt]
                //,[ReceivedMessage]
                //,[ResIMEI]
                //bll.db.Database.CommandTimeout = 32000;

                var list = (from v in bll.db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN" && m.GpsMobileNumber != null && m.GpsMobileNumber != "")
                            select new
                            {
                                v.GpsMobileNumber,
                                v.RegistrationNumber
                            }).OrderBy(m => m.RegistrationNumber).ToList();

                for (int i = 0; i < list.Count; i++)
                {
                    var query = "";
                    query = "EXEC VTS_SIM_VerificationSend '01937722003', 'appVehicleRegistration', 'imei', '2020-02-01', 'text message'" + "\n";
                    bll.db.Database.ExecuteSqlCommand(query);
                    //if (i % 10 == 0)
                    //{
                    //    Task.Delay(2000).Wait();
                    //}
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
                bll.db.ServiceCalls.Add(
                new ServiceCall()
                {
                    CallingTime = DateTime.Now,
                    CallingMessage = "VTS_SIM_Verification-Error" + guid,
                    UserDefinedMessage = errrorMessage
                }
                );
                bll.db.SaveChanges();
            }

            //bll.db.ServiceCalls.Add(
            //      new ServiceCall()
            //      {
            //          CallingMessage = "VTS_SIM_Verification-End-" + guid,
            //          CallingTime = DateTime.Now,
            //          UserDefinedMessage = ""
            //      }
            //      );
            //bll.db.SaveChanges();
            return "Done";
        }
        #endregion

    }
}