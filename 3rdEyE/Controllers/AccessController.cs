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
using _3rdEyE.ManagingTools;
using System.Threading;
using System.Threading.Tasks;
using static _3rdEyE.ManagingTools.CommonClass;
using System.IO;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Security.Cryptography;
using Newtonsoft.Json;
using _3rdEyE.ViewModels;

namespace _3rdEyE.Controllers
{
    public class AccessController : Controller
    {
        BaseBLL bll = new BaseBLL();
        // GET: Access
        public ActionResult Login()
        {
            ViewBag.message = "";
            ViewBag.UserName = "";
            ViewBag.Password = "";
            return View();
        }
        [HttpPost]
        public ActionResult Login(AppUser model)
        {
            AppUser db_model = bll.db.AppUsers.Where(u => u.IsDeleted != true && u.IsActive != false && u.UniqueIDNumber == model.UniqueIDNumber).FirstOrDefault();
            if (db_model != null)
            {
                db_model.LoginTryDateTime = DateTime.Now;
                db_model.LoginTryStatus = false;
                bll.db.SaveChanges();

                if (db_model.AppUserType == "Internal Admin")
                {
                    if (db_model.Password == model.Password)
                    {
                        db_model.LoginTryStatus = true;
                        bll.db.SaveChanges();
                        return LoginSuccess(db_model);
                    }
                    else
                    {
                        ViewBag.message = "Invalid Credential";
                        ViewBag.UserName = model.UniqueIDNumber;
                        ViewBag.Password = model.Password;
                        return View();
                    }
                }
                else if (db_model.AppUserType == "Internal Gate Entry Device" || db_model.AppUserType == "Internal Parking Entry Device")
                {
                    if (db_model.Password == model.Password)
                    {
                        db_model.LoginTryStatus = true;
                        bll.db.SaveChanges();
                        CommonClass.SessionTimeoutMinutes = 600;
                        return LoginSuccess(db_model);
                    }
                    else
                    {
                        ViewBag.message = "Invalid Credential";
                        ViewBag.UserName = model.UniqueIDNumber;
                        ViewBag.Password = model.Password;
                        return View();
                    }
                }
                else if (db_model.AppUserType.Contains("External"))
                {
                    if (db_model.Password == model.Password)
                    {
                        db_model.LoginTryStatus = true;
                        bll.db.SaveChanges();
                        return LoginSuccess(db_model);
                    }
                    else
                    {
                        ViewBag.message = "Invalid Credential";
                        ViewBag.UserName = model.UniqueIDNumber;
                        ViewBag.Password = model.Password;
                        return View();
                    }
                }
                else if (db_model.AppUserType.Contains("Internal"))
                {
                    //HRIS access
                    var CHECK_HRIS_LOGIN = ConfigurationManager.AppSettings["CHECK_HRIS_LOGIN"];
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
                                bll.db.SaveChanges();
                            }
                            else if (res_dict["status"] == "Wrong")
                            {
                                db_model.LoginTryStatus = true;
                                bll.db.SaveChanges();
                            }
                            else
                            {
                                ViewBag.message = res_dict["message"];
                                bll.db.SaveChanges();
                            }
                        }
                        catch (Exception)
                        {
                            bll.db.SaveChanges();
                            ViewBag.message = "Can not connect HRIS for Login. Please, try again later.";
                            ViewBag.UserName = model.UniqueIDNumber;
                            return View();
                        }
                    }
                    else if (db_model.Password_Encrypted == Encrypt(model.Password))
                    {
                        db_model.LoginTryStatus = true;
                        bll.db.SaveChanges();
                    }

                    //HRIS receive data & Update Data
                    if (CHECK_HRIS_LOGIN == "1" && db_model.LoginTryStatus == true)
                    {
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
                                bll.db.SaveChanges();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    if (db_model.LoginTryStatus == true)
                    {
                        return LoginSuccess(db_model);
                    }
                    else
                    {
                        ViewBag.message = "Invalid Credential";
                        ViewBag.UserName = model.UniqueIDNumber;
                        ViewBag.Password = model.Password;
                        return View();
                    }
                }
                else if (db_model.AppUserType == "Temp User")
                {
                    if (db_model.Password == model.Password)
                    {
                        db_model.LoginTryStatus = true;
                        bll.db.SaveChanges();
                        CommonClass.SessionTimeoutMinutes = 600;
                        return LoginSuccess(db_model);
                    }
                    else
                    {
                        ViewBag.message = "Invalid Credential";
                        ViewBag.UserName = model.UniqueIDNumber;
                        ViewBag.Password = model.Password;
                        return View();
                    }
                }
                else
                {
                    ViewBag.message = "Unexpected User Type, contact to MIS-Automation";
                    ViewBag.UserName = model.UniqueIDNumber;
                    ViewBag.Password = model.Password;
                    db_model.DevelopersNote = "Access-Unexpected_AppUserType:" + db_model.AppUserType + "AppUserSubType:" + db_model.AppUserSubType;
                    bll.db.SaveChanges();
                    return View();
                }
            }
            else
            {
                ViewBag.message = "Invalid Credential";
                ViewBag.UserName = model.UniqueIDNumber;
                ViewBag.Password = model.Password;
                return View();
            }
        }
        public ActionResult LoginSuccess(AppUser currentUser)
        {

            CommonClass.SetCurrentUser(currentUser);

            //CommonClass.SetModulePermissions(currentUser);

            CommonClass.SetRoleMenuListAndRolePermissionsList(currentUser);


            var appUserLoginHistory =
                    new AppUserLoginHistory()
                    {
                        FK_AppUser = currentUser.PK_User,
                        LoginTime = DateTime.Now,
                        ExpirationTime = DateTime.Now,
                    };
            bll.db.AppUserLoginHistories.Add(appUserLoginHistory);
            bll.db.SaveChanges();
            CommonClass.SetCurrentLoginID(appUserLoginHistory.PK_AppUserLoginHistory);
            //#rawcode
            if (currentUser.UniqueIDNumber == "108687" || currentUser.UniqueIDNumber == "sa")
            {
                CommonClass.SessionTimeoutMinutes = 60;
            }
            return Redirect("/Home/Index");
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
        public ActionResult LogOut(Int64 CurrentLoginID)
        {
            var currentUser = CommonClass.GetCurrentUser();
            if (currentUser != null)
            {
                var lastLogin = bll.db.AppUserLoginHistories.Where(m => m.PK_AppUserLoginHistory == CurrentLoginID).OrderByDescending(m => m.PK_AppUserLoginHistory).FirstOrDefault();
                if (lastLogin != null)
                {
                    lastLogin.ExpirationTime = DateTime.Now;
                    lastLogin.Reason = "Log out";
                    bll.db.SaveChanges();
                }
            }
            SessionClass.ClearAllSession();
            return RedirectToAction("Login");
        }

        public JsonResult UpdateLastEventSession(Int64 CurrentLoginID)
        {
            var lastLogin = bll.db.AppUserLoginHistories.Where(m => m.PK_AppUserLoginHistory == CurrentLoginID).FirstOrDefault();
            if (lastLogin != null && lastLogin.Reason == null)
            {
                var Now = DateTime.Now;
                var loggedInMinutes = (DateTime.Now - lastLogin.ExpirationTime).TotalMinutes;
                if (loggedInMinutes >= CommonClass.SessionTimeoutMinutes)
                {
                    lastLogin.Reason = "Session expired";
                    bll.db.SaveChanges();
                    return Json("SessionExpired", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    lastLogin.ExpirationTime = Now;
                    bll.db.SaveChanges();
                    return Json("SessinEixsts", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("SessionExpired", JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult CheckLastEventSession(Int64 CurrentLoginID)
        {
            var lastLogin = bll.db.AppUserLoginHistories.Where(m => m.PK_AppUserLoginHistory == CurrentLoginID).FirstOrDefault();
            if (lastLogin != null && lastLogin.Reason == null)
            {
                var Now = DateTime.Now;
                var loggedInMinutes = (DateTime.Now - lastLogin.ExpirationTime).TotalMinutes;
                if (loggedInMinutes >= CommonClass.SessionTimeoutMinutes)
                {
                    lastLogin.Reason = "Session expired";
                    bll.db.SaveChanges();
                    return Json("SessionExpired", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //lastLogin.ExpirationTime = Now;
                    //bll.db.SaveChanges();
                    return Json("SessinEixsts", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("SessionExpired", JsonRequestBehavior.AllowGet);
            }

        }


    }
}