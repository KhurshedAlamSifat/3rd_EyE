using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.Models;
using _3rdEyE.BLL;
//using _3rdEyE.ManagingTools;
using System.Threading;
using System.Threading.Tasks;
using static _3rdEyE.ManagingTools.CommonClass;
using System.IO;
using System.Web.Script.Serialization;
using _3rdEyE.ManagingTools;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Configuration;
using _3rdEyE.ViewModels;

namespace _3rdEyE.Controllers
{
    public class AccessAPIController : Controller
    {
        public DBEnityModelContainer db = new DBEnityModelContainer();
        // GET: Access
        [HttpPost]
        public JsonResult Login(AppUser model)
        {
            AppUser db_model = db.AppUsers.Where(u => u.IsDeleted != true && u.IsActive != false && u.UniqueIDNumber == model.UniqueIDNumber).FirstOrDefault();
            if (db_model != null)
            {
                db_model.LoginTryDateTime = DateTime.Now;
                db_model.LoginTryStatus = false;
                db.SaveChanges();

                //if (db_model.IsSingleDeviceAccess == true && db_model.DeviceId != null && db_model.DeviceId != model.DeviceId)
                if (db.Mappings.Where(m => m.MappingKey == "User Name:Device Id" && m.IndependentKeyValue == model.UniqueIDNumber).Any() &&
                    !(db.Mappings.Where(m => m.MappingKey == "User Name:Device Id" && m.IndependentKeyValue == model.UniqueIDNumber && m.DependentKeyValue == model.DeviceId).Any()))
                {
                    db_model.DevelopersNote = "Unauthorized Device Id:" + model.DeviceId;
                    db.SaveChanges();
                    return Json(new { flag = "Unauthorized Device Id", status = false, message = "Unauthorized Device Id:" + model.DeviceId }, JsonRequestBehavior.AllowGet);
                }

                if (db_model.AppUserType == "Internal Admin")
                {
                    if (db_model.Password == model.Password)
                    {
                        db_model.LoginTryStatus = true;
                        db_model.FID = model.FID;
                        db_model.DeviceId = model.DeviceId;

                        return LoginSuccess(db_model);
                    }
                    else
                    {
                        return Json(new { flag = "Invalid Credential", status = false, message = "Invalid Credential" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (db_model.AppUserType == "Internal Gate Entry Device" || db_model.AppUserType == "Internal Parking Entry Device")
                {
                    if (db_model.Password == model.Password /*&& db_model.IMEI == model.IMEI*/)
                    {
                        db_model.LoginTryStatus = true;
                        db.SaveChanges();

                        db_model.FID = model.FID;
                        db_model.DeviceId = model.DeviceId;
                        return LoginSuccess(db_model);
                    }
                    else
                    {
                        return Json(new { flag = "Invalid Credential", status = false, message = "Invalid Credential" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (db_model.AppUserType.Contains("External"))
                {
                    if (db_model.Password == model.Password)
                    {
                        db_model.LoginTryStatus = true;
                        db.SaveChanges();

                        db_model.FID = model.FID;
                        db_model.DeviceId = model.DeviceId;
                        return LoginSuccess(db_model);
                    }
                    else
                    {
                        return Json(new { flag = "Invalid Credential", status = false, message = "Invalid Credential" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (db_model.AppUserType.Contains("Internal"))
                {
                    //HRIS access
                    var CHECK_HRIS_LOGIN = ConfigurationManager.AppSettings["CHECK_HRIS_LOGIN"];
                    var message = "";
                    if (CHECK_HRIS_LOGIN == "1")
                    {
                        try
                        {
                            var res_string = "";
                            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://hris.prangroup.com:8696/Login/LoginHris");
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Method = "POST";
                            httpWebRequest.Headers.Add("Authorization", "Basic YXV0aDoxMlByYW5AMTIzNDU2JA==");

                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                string json = new JavaScriptSerializer().Serialize(new
                                {
                                    username = model.UniqueIDNumber,
                                    password = model.Password
                                });
                                streamWriter.Write(json);
                            }

                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                res_string = streamReader.ReadToEnd();
                                //res = "{\"status\":\"Success\",\"message\":\"Logged in successfully\"}";
                            }
                            var res_dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(res_string);
                            if (res_dict["status"] == "Success")
                            {
                                db_model.LoginTryStatus = true;
                                db_model.Password_Encrypted = Encrypt(model.Password);
                                db.SaveChanges();
                            }
                            else if (res_dict["status"] == "Wrong")
                            {
                                db_model.LoginTryStatus = true;
                                db.SaveChanges();
                            }
                            else
                            {
                                message = res_dict["message"];
                                db.SaveChanges();
                            }
                        }
                        catch (Exception)
                        {
                            message = "Can not connect HRIS for Login. Please, try again later.";
                            return Json(new { flag = "Can not connect HRIS. Please, try again later.", status = false, message = message }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else if (db_model.Password_Encrypted == Encrypt(model.Password))
                    {
                        db_model.LoginTryStatus = true;
                        db_model.DevelopersNote = "Login-Success Without CHECK_HRIS_LOGIN";
                        db.SaveChanges();
                    }

                    //HRIS receive data & Update Data
                    if (CHECK_HRIS_LOGIN == "1" && db_model.LoginTryStatus == true)
                    {
                        //var res_dict = new Dictionary<string, string>();
                        StaffResult staffDetail = null;
                        string res_stirng = "";
                        try
                        {
                            res_stirng = new WebClient().DownloadString("http://hris.prangroup.com:8686/api/hrisapi.svc/Staff/" + model.UniqueIDNumber);

                            if (res_stirng != "{\"StaffResult\":\"[]\"}" || res_stirng != "{\"StaffResult\":\"Value cannot be null.\\u000d\\u000aParameter name: source\"}")
                            {
                                res_stirng = res_stirng.Replace("\\", "");
                                res_stirng = res_stirng.Replace("\"[", "[").Replace("]\"", "]");
                                var _HRISAPIData = JsonConvert.DeserializeObject<VM_HRISAPIData>(res_stirng);
                                if (_HRISAPIData.StaffResult.Count > 0)
                                {
                                    staffDetail = _HRISAPIData.StaffResult.FirstOrDefault();
                                }
                            }
                        }
                        catch (Exception)
                        {
                            db_model.DevelopersNote = "Login-Interna-HRIS_ReadInfoAPI-ConError";
                            db.SaveChanges();
                        }

                        //HRIS Update Data
                        if (staffDetail != null)
                        {
                            try
                            {
                                db_model.FullName = staffDetail.NAME;
                                db_model.ContactNumber = staffDetail.CONTACTNO;
                                db_model.Email = staffDetail.EMAIL;
                                db_model.HGroupName = staffDetail.GROUPNAME;
                                db_model.HCompany = staffDetail.COMPANY;
                                db_model.HDepartment = staffDetail.DEPARTMENT;
                                db_model.HLocationName = staffDetail.LOCATIONNAME;
                                db_model.HDesignation = staffDetail.DESIGNATION;
                                db_model.HSatus = staffDetail.STATUS;
                                db_model.LoginHRIS_Update = db_model.LoginTryDateTime;
                                db.SaveChanges();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    if (db_model.LoginTryStatus == true)
                    {
                        db_model.FID = model.FID;
                        db_model.DeviceId = model.DeviceId;
                        return LoginSuccess(db_model);
                    }
                    else
                    {
                        return Json(new { flag = "Invalid Credential", status = false, message = message }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    db_model.DevelopersNote = "AccessAPI-Unexpected_AppUserType:" + db_model.AppUserType + "AppUserSubType:" + db_model.AppUserSubType;
                    db.SaveChanges();
                    return Json(new { flag = "Unexpected User Type, contact to MIS-Automation", status = false, message = "Unexpected User Type, contact to MIS-Automation" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { flag = "Invalid Credential", status = false, message = "Invalid Credential" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LoginSuccess(AppUser currentUser)
        {
            var _currentUser = db.AppUsers.Where(u => u.PK_User == currentUser.PK_User).FirstOrDefault();

            //Save last given FID
            if (currentUser.FID != null)
            {
                _currentUser.FID = currentUser.FID;
            }
            //Save DeviceId
            _currentUser.DeviceId = currentUser.DeviceId;

            var appUserLoginHistory =
                    new AppUserLoginHistory()
                    {
                        FK_AppUser = currentUser.PK_User,
                        LoginTime = DateTime.Now,
                        ExpirationTime = DateTime.Now,
                        Reason = "Login By Mobile App"
                    };
            db.AppUserLoginHistories.Add(appUserLoginHistory);
            db.SaveChanges();

            List<ManagingTools.MobileMenu> RoleMenuList = (from role_menu in db.MobileRole_MobileMenu.Where(m => m.FK_MobileRole == currentUser.FK_MobileRole && m.IsAccessible == true)
                                                           join menu in db.MobileMenus.Where(m => m.IsDeleted != true && m.IsActive != false) on role_menu.FK_MobileMenu equals menu.PK_MobileMenu
                                                           //orderby role_menu.Sequence
                                                           select new ManagingTools.MobileMenu()
                                                           {
                                                               FullName = menu.FullName,
                                                           }).ToList();
            if (RoleMenuList.Count == 0)
            {
                RoleMenuList.Add(new ManagingTools.MobileMenu()
                {
                    FullName = "No Menu Permission"
                });
            }

            return Json(new
            {
                flag = "Success",
                status = true,
                message = "",
                data = new
                {
                    currentUser.PK_User,
                    currentUser.FK_Location, //#Added for visitior management system
                    currentUser.FullName,
                    currentUser.UniqueIDNumber,
                    currentUser.ContactNumber,
                    currentUser.Email,
                    RoleMenuList
                }
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RecheckUserActivityStatus_Version7(Guid PK_User, string AppVersionCode, string DeviceId)
        {
            var currentUser = db.AppUsers.Where(u => u.PK_User == PK_User).FirstOrDefault();
            {
                currentUser.AppVersionCode = AppVersionCode;
                db.SaveChanges();
            }
            var AppVersion_Latest = db.AppSettings.Where(m => m.Name == "AppVersionCode_Current" && m.IsActive == true).FirstOrDefault();
            var AppVersionCode_OldUsable = db.AppSettings.Where(m => m.Name == "AppVersionCode_OldUsable" && m.IsActive == true).Select(m => m.Value).ToList();
            var IsUsableVersion = false;
            var LatestVersionCode = AppVersion_Latest.Value;
            var LatestVersionName = AppVersion_Latest.Value2;
            var IsBanned = false;
            var IsActive = false;
            var IsUnauthorizedDevice = false;

            var model = db.AppUsers.Where(m => m.PK_User == PK_User).FirstOrDefault();
            if (model != null)
            {
                IsBanned = model.IsBanned == true ? true : false;
                IsActive = model.IsActive == false ? false : true;
            }

            //IsSingleDeviceAccess
            //if (currentUser.IsSingleDeviceAccess == true && currentUser.DeviceId != null && currentUser.DeviceId != DeviceId)
            if (db.Mappings.Where(m => m.MappingKey == "User Name:Device Id" && m.IndependentKeyValue == model.UniqueIDNumber).Any() &&
                !(db.Mappings.Where(m => m.MappingKey == "User Name:Device Id" && m.IndependentKeyValue == model.UniqueIDNumber && m.DependentKeyValue == model.DeviceId).Any()))
            {
                IsUnauthorizedDevice = true;
            }

            //AppVersionCode
            if (AppVersionCode == AppVersion_Latest.Value || AppVersionCode_OldUsable.Contains(AppVersionCode))
            {
                IsUsableVersion = true;
            }
            //else
            //{
            //    IsBanned = true;
            //}

            var AccessibleModuleList = (from role_menu in db.MobileRole_MobileMenu.Where(m => m.FK_MobileRole == currentUser.FK_MobileRole && m.IsAccessible == true)
                                        join menu in db.MobileMenus.Where(m => m.IsDeleted != true && m.IsActive != false) on role_menu.FK_MobileMenu equals menu.PK_MobileMenu
                                        //orderby role_menu.Sequence
                                        select new
                                        {
                                            ModuleName = menu.FullName,
                                        }).ToList();
            return Json(new
            {
                IsBanned,
                IsUsableVersion,
                IsUnauthorizedDevice,
                IsActive,
                LatestVersionCode,
                LatestVersionName,
                AccessibleModuleList
            }, JsonRequestBehavior.AllowGet);
        }

        //#Version9/+
        public JsonResult RecheckUserActivityStatus(Guid PK_User, string AppVersionCode, string DeviceId)
        {
            var currentUser = db.AppUsers.Where(u => u.PK_User == PK_User).FirstOrDefault();
            {
                currentUser.AppVersionCode = AppVersionCode;
                db.SaveChanges();
            }
            var AppVersion_Latest = db.AppSettings.Where(m => m.Name == "AppVersionCode_Current" && m.IsActive == true).FirstOrDefault();
            var AppVersionCode_OldUsable = db.AppSettings.Where(m => m.Name == "AppVersionCode_OldUsable" && m.IsActive == true).Select(m => m.Value).ToList();
            var IsUsableVersion = false;
            var LatestVersionCode = AppVersion_Latest.Value;
            var LatestVersionName = AppVersion_Latest.Value2;
            var IsBanned = false;
            var IsActive = false;
            var IsUnauthorizedDevice = false;

            var model = db.AppUsers.Where(m => m.PK_User == PK_User).FirstOrDefault();
            if (model != null)
            {
                IsBanned = model.IsBanned == true ? true : false;
                IsActive = model.IsActive == false ? false : true;
            }

            //IsSingleDeviceAccess
            //if (currentUser.IsSingleDeviceAccess == true && currentUser.DeviceId != null && currentUser.DeviceId != DeviceId)
            if (db.Mappings.Where(m => m.MappingKey == "User Name:Device Id" && m.IndependentKeyValue == model.UniqueIDNumber).Any() &&
                !(db.Mappings.Where(m => m.MappingKey == "User Name:Device Id" && m.IndependentKeyValue == model.UniqueIDNumber && m.DependentKeyValue == model.DeviceId).Any()))
            {
                IsUnauthorizedDevice = true;
            }

            //AppVersionCode
            if (AppVersionCode == AppVersion_Latest.Value || AppVersionCode_OldUsable.Contains(AppVersionCode))
            {
                IsUsableVersion = true;
            }
            //else
            //{
            //    IsBanned = true;
            //}

            var AccessibleModuleList = (from role_menu in db.MobileRole_MobileMenu.Where(m => m.FK_MobileRole == currentUser.FK_MobileRole && m.IsAccessible == true)
                                        join menu in db.MobileMenus.Where(m => m.IsDeleted != true && m.IsActive != false) on role_menu.FK_MobileMenu equals menu.PK_MobileMenu
                                        orderby role_menu.Sequence
                                        select new
                                        {
                                            ModuleName = menu.FullName,
                                        }).ToList();
            return Json(new
            {
                IsBanned,
                IsUsableVersion,
                IsUnauthorizedDevice,
                IsActive,
                LatestVersionCode,
                LatestVersionName,
                AccessibleModuleList
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOut(Int64 CurrentLoginID)
        {
            //var currentUser = CommonClass.GetCurrentUser();
            //if (currentUser != null)
            //{
            //    var lastLogin = db.AppUserLoginHistories.Where(m => m.PK_AppUserLoginHistory == CurrentLoginID).OrderByDescending(m => m.PK_AppUserLoginHistory).FirstOrDefault();
            //    if (lastLogin != null)
            //    {
            //        lastLogin.ExpirationTime = DateTime.Now;
            //        lastLogin.Reason = "Log out";
            //        db.SaveChanges();
            //    }
            //}
            //SessionClass.ClearAllSession();
            return RedirectToAction("Login");
        }
        public string Encrypt(string input)
        {
            var res = "";
            try
            {
                var key = "3e.prangroup.com";
                byte[] inputArray = System.Text.UTF8Encoding.UTF8.GetBytes(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = System.Text.UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                res = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception)
            {
            }
            return res;
        }
    }
}