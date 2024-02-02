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
using _3rdEyE.BLLs;
using _3rdEyE.ViewModels;
using Microsoft.Reporting.WebForms;

namespace _3rdEyE.Controllers
{
    public class AppUserController : BaseController
    {
        BLL_AppUser bll = new BLL_AppUser();
        Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" }, { "CS", "CS" }, { "ALL", "ALL" } };
        //Dictionary<string, string> AppUserTypeDict = new Dictionary<string, string> { { "Internal", "Only User" }, { "External Transport Agent", "Only Agent" }, { "Internal", "User and Agent" } };
        //Dictionary<string, string> AppUserTypeDict = new Dictionary<string, string> { { "Internal", "Only User" }, { "Internal", "User and Agent:All" }, { "Internal", "Inter Company Requisition Raiser" }, { "Internal", "Inter Company Requisition Approver" } };
        Dictionary<string, string> AppUserTypeDict = new Dictionary<string, string> { { "Internal", "Internal" }, { "Internal Transport Agent", "Internal Transport Agent" } };

        public ActionResult Index()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<AppUser> list;
            if (CurrentUser.PRG_Type == "ALL")
            {
                list = bll.db.AppUsers.AsEnumerable().Where(c => c.IsDeleted == false && c.AppUserType.Contains("Internal")).ToList();
            }
            else
            {
                list = bll.db.AppUsers.AsEnumerable().Where(c => c.IsDeleted == false && c.AppUserType.Contains("Internal") && c.PRG_Type == CurrentUser.PRG_Type).ToList();
            }
            return View(list);
        }
        public ActionResult OtherLinkPage()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }
        public void ExportAppUserList()
        {
            List<AppUser> list;
            if (CurrentUser.PRG_Type == "ALL")
            {
                list = bll.db.AppUsers.AsEnumerable().Where(c => c.IsDeleted == false && c.AppUserType.Contains("Internal")).ToList();
            }
            else
            {
                list = bll.db.AppUsers.AsEnumerable().Where(c => c.IsDeleted == false && c.AppUserType.Contains("Internal") && c.PRG_Type == CurrentUser.PRG_Type).ToList();
            }

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=User_List.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            //# Add Header Row
            Response.Output.Write("Staff ID" + "\t");
            Response.Output.Write("PRG" + "\t");
            Response.Output.Write("Full Name" + "\t");
            Response.Output.Write("Role" + "\t");
            Response.Output.Write("Type" + "\t");
            Response.Output.Write("Sub Type" + "\t");
            Response.Output.Write("Depot" + "\t");
            Response.Output.Write("Contact Number" + "\t");
            Response.Output.Write("Contact Address" + "\t");
            Response.Output.Write("Email" + "\t");
            Response.Output.Write("HGroupName" + "\t");
            Response.Output.Write("HCompany" + "\t");
            Response.Output.Write("HDepartment" + "\t");
            Response.Output.Write("HLocationName" + "\t");
            Response.Output.Write("HDesignation" + "\t");
            Response.Output.WriteLine();

            foreach (var item in list)
            {
                Response.Output.Write(item.UniqueIDNumber + "\t");
                Response.Output.Write(item.PRG_Type + "\t");
                Response.Output.Write(item.FullName + "\t");
                if (item.AppRole != null)
                {
                    Response.Output.Write(item.AppRole.FullName + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                Response.Output.Write(item.AppUserType + "\t");
                Response.Output.Write(item.AppUserSubType + "\t");
                if (item.Depo != null)
                {
                    Response.Output.Write(item.Depo.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                Response.Output.Write(item.ContactNumber + "\t");
                Response.Output.Write(item.ContactAddress + "\t");
                Response.Output.Write(item.Email + "\t");
                Response.Output.Write(item.HGroupName + "\t");
                Response.Output.Write(item.HCompany + "\t");
                Response.Output.Write(item.HDepartment + "\t");
                Response.Output.Write(item.HLocationName + "\t");
                Response.Output.Write(item.HDesignation + "\t");
                Response.Output.WriteLine();
            }
            Response.End();

        }
        public ActionResult View(Guid PK_User)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (PK_User == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.AppUsers.Find(PK_User);
                if (model != null)
                {
                    var viewModel = bll.ConvertToViewModel(model);
                    ViewBag.AppUserAccessibleDepoes = model.AppUserAccessibleDepoes.Where(m => m.IsAccessible == true).Select(m => m.Depo.Name).ToList();
                    ViewBag.RequisitionAgentProposedDepoes = model.RequisitionAgentProposedDepoes.Where(m => m.WillPropose == true).Select(m => m.Depo.Name).ToList();
                    return View(viewModel);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        //# Update Self
        #region 
        public ActionResult Edit_CurrentUserProfile()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = CurrentUser;
            if (model != null)
            {
                ViewBag.model = model;
                ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true && (!m.Category.Contains("Physical")) && m.PRG_Type == CurrentUser.PRG_Type).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
                ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false && (m.LocationType == "Factory" || m.LocationType == "Depo" || m.LocationType == "Office")).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location);
                ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", CurrentUser.PRG_Type);
                ViewBag.AppUserTypeDict = new SelectList(AppUserTypeDict, "Key", "Value", model.AppUserType);
                return View();
            }

            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult Edit_CurrentUserProfile(AppUser model)
        {
            string modelValidator = bll.IsValidModel_ToEdit(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel_CurrentUser(model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Your profile is successfully updated.");
                    return Redirect("/Home/Index");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", modelValidator);
            }
            ViewBag.model = model;
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true && (!m.Category.Contains("Physical")) && m.PRG_Type == CurrentUser.PRG_Type).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
            ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false && (m.LocationType == "Factory" || m.LocationType == "Depo" || m.LocationType == "Office")).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location);
            ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", CurrentUser.PRG_Type);
            ViewBag.AppUserTypeDict = new SelectList(AppUserTypeDict, "Key", "Value", model.AppUserType);

            return View();
        }
        #endregion

        //#Basic info
        #region
        public ActionResult PreCreate(string UniqueIDNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            Session["CehckingUniqueIDNumber"] = UniqueIDNumber;
            var model = new AppUser();

            if (UniqueIDNumber != null)
            {
                var appUser_3e = bll.db.AppUsers.Where(m => m.UniqueIDNumber == UniqueIDNumber).FirstOrDefault();
                if (appUser_3e != null)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "This user already ready exist in 3rd Eye");
                    return View(appUser_3e);
                }
                else
                {
                    var staffDetail = GetUserDetrailFromHRISApi(UniqueIDNumber);
                    if (staffDetail != null)
                    {
                        var hrisModel = new AppUser();
                        hrisModel.FullName = staffDetail.NAME;
                        hrisModel.HDesignation = staffDetail.DESIGNATION;
                        hrisModel.HLocationName = staffDetail.LOCATIONNAME;
                        hrisModel.ContactNumber = staffDetail.CONTACTNO;
                        ViewBag.showCreate = "";
                        return View(hrisModel);
                    }
                    else
                    {
                        CreateAlertMessage(AlertMessageType.Danger, "Warning", "User Detail not found form HRIS");
                        return View(model);
                    }
                }
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult PreCreate(AppUser model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            if (model.UniqueIDNumber != null)
            {
                string CehckingUniqueIDNumber = (string)Session["CehckingUniqueIDNumber"];
                if (CehckingUniqueIDNumber != model.UniqueIDNumber)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Your truing to create another user before checking.");
                    return View(model);
                }
                var appUser_3e = bll.db.AppUsers.Where(m => m.UniqueIDNumber == model.UniqueIDNumber).FirstOrDefault();
                if (appUser_3e != null)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "This user already exist in 3rd Eye");
                    return View(appUser_3e);
                }
                else
                {
                    var staffDetail = GetUserDetrailFromHRISApi(model.UniqueIDNumber);
                    if (staffDetail != null)
                    {
                        var db_model = new AppUser();
                        db_model.PK_User = Guid.NewGuid();
                        db_model.IsDeleted = false;
                        db_model.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;
                        db_model.CreatedAt = DateTime.Now;
                        db_model.AppUserType = "Internal";// set user by defoult

                        db_model.UniqueIDNumber = staffDetail.ID;
                        //db_model.Password = model.Password;
                        db_model.FullName = staffDetail.NAME;
                        db_model.PRG_Type = CurrentUser.PRG_Type;
                        //db_model.FK_Depo = CurrentUser.FK_Depo;
                        //db_model.FK_UserDesignation = model.FK_UserDesignation;
                        db_model.ContactNumber = staffDetail.CONTACTNO;
                        db_model.Email = staffDetail.EMAIL;
                        //db_model.ContactAddress = model.ContactAddress;
                        db_model.HGroupName = staffDetail.GROUPNAME;
                        db_model.HCompany = staffDetail.COMPANY;
                        db_model.HDepartment = staffDetail.DEPARTMENT;
                        db_model.HLocationName = staffDetail.LOCATIONNAME;
                        db_model.HDesignation = staffDetail.DESIGNATION;
                        db_model.HSatus = staffDetail.STATUS;
                        //db_model.AppUserSubType = string.IsNullOrEmpty(model.AppUserSubType) ? null : model.AppUserSubType.Trim().ToUpper();

                        db_model.IsActive = true;
                        db_model.IsBanned = false;
                        db_model.IsDeleted = false;
                        bll.db.AppUsers.Add(db_model);
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Successfully added user.");
                        return RedirectToAction("Create", new { PK_User = db_model.PK_User });
                    }
                    else
                    {
                        CreateAlertMessage(AlertMessageType.Danger, "Warning", "Staff Detail is not found from HRIS");
                        return View(model);
                    }
                }
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Create(Guid PK_User)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.AppUsers.Find(PK_User);
            //ViewBag.model = model;
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true && (!m.Category.Contains("Physical")) && m.PRG_Type == model.PRG_Type).OrderBy(m => m.Name), "PK_Depo", "Name");
            ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false && (m.LocationType == "Factory" || m.LocationType == "Depo" || m.LocationType == "Office")).OrderBy(m => m.Name), "PK_Location", "Name");
            ViewBag.AppRoles = new SelectList(bll.db.AppRoles.OrderBy(m => m.FullName), "PK_AppRole", "FullName");
            ViewBag.MobileRoles = new SelectList(bll.db.MobileRoles.OrderBy(m => m.FullName), "PK_MobileRole", "FullName");
            ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", CurrentUser.PRG_Type);
            ViewBag.AppUserTypeDict = new SelectList(AppUserTypeDict, "Key", "Value");

            return View(model);
        }
        [HttpPost]
        public ActionResult Create(AppUser model)
        {
            string modelValidator = bll.IsValidModel_ToCreate(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.db.AppUsers.Find(model.PK_User);

                    //db_model.PRG_Type = model.PRG_Type;
                    db_model.FK_Depo = model.FK_Depo;
                    db_model.FK_Location = model.FK_Location;
                    db_model.FK_AppRole = model.FK_AppRole;
                    db_model.FK_MobileRole = model.FK_MobileRole;
                    db_model.ContactAddress = model.ContactAddress;
                    db_model.AppUserSubType = string.IsNullOrEmpty(model.AppUserSubType) ? null : model.AppUserSubType.Trim().ToUpper();
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "User is successfully added.");
                    return RedirectToAction("ManageWebPermission", new { FK_User = db_model.PK_User });
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", modelValidator);
            }
            ViewBag.model = model;
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true && (!m.Category.Contains("Physical")) && m.PRG_Type == CurrentUser.PRG_Type).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
            ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false && (m.LocationType == "Factory" || m.LocationType == "Depo" || m.LocationType == "Office")).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location);
            ViewBag.AppRoles = new SelectList(bll.db.AppRoles.OrderBy(m => m.FullName), "PK_AppRole", "FullName", model.FK_AppRole);
            ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", CurrentUser.PRG_Type);
            ViewBag.AppUserTypeDict = new SelectList(AppUserTypeDict, "Key", "Value", model.AppUserType);
            return View();
        }

        public ActionResult Edit_BasicInformation(Guid PK_User)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.AppUsers.Find(PK_User);
            if (model != null)
            {
                ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true && (!m.Category.Contains("Physical")) && m.PRG_Type == CurrentUser.PRG_Type).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
                ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false && (m.LocationType == "Factory" || m.LocationType == "Depo" || m.LocationType == "Office")).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location);
                ViewBag.AppRoles = new SelectList(bll.db.AppRoles.OrderBy(m => m.FullName), "PK_AppRole", "FullName", model.FK_AppRole);
                ViewBag.MobileRoles = new SelectList(bll.db.MobileRoles.OrderBy(m => m.FullName), "PK_MobileRole", "FullName", model.FK_MobileRole);
                ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", CurrentUser.PRG_Type);
                ViewBag.AppUserTypeDict = new SelectList(AppUserTypeDict, "Key", "Value", model.AppUserType);
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult Edit_BasicInformation(AppUser model)
        {
            string modelValidator = bll.IsValidModel_ToEdit(model);
            var db_model = bll.db.AppUsers.Find(model.PK_User);

            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    db_model.FK_UpdatedByUser = CommonClass.GetCurrentUser().PK_User;
                    db_model.UpdatedAt = DateTime.Now;

                    //db_model.PRG_Type = model.PRG_Type;
                    db_model.FK_Depo = model.FK_Depo;
                    db_model.FK_Location = model.FK_Location;
                    db_model.FK_AppRole = model.FK_AppRole;
                    db_model.FK_MobileRole = model.FK_MobileRole;
                    db_model.ContactAddress = model.ContactAddress;
                    db_model.AppUserSubType = string.IsNullOrEmpty(model.AppUserSubType) ? null : model.AppUserSubType.Trim().ToUpper();
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "AppUser is successfully edited.");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", modelValidator);
            }
            return RedirectToAction("Edit_BasicInformation", new { PK_User = model.PK_User });
        }
        #endregion

        //# WebPermission
        #region
        public ActionResult ManageWebPermission(Guid FK_User)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.AppUsers.Where(m => m.PK_User == FK_User).FirstOrDefault();
            if (model == null)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "User not found in Database");
                return RedirectToAction("index");
            }
            else
            {
                List<WebMenu> list = (from menu in bll.db.AppMenus.AsEnumerable()
                                      select new WebMenu()
                                      {
                                          PK_AppMenu = menu.PK_AppMenu,
                                          FullName = menu.FullName,
                                          VisibleName = menu.VisibleName,
                                          ModelName = menu.ModelName,
                                          Icon = menu.Icon,
                                          Link = menu.Link,
                                          Sequence = menu.Sequence,


                                          IsAccessible = bll.db.AppUser_AppMenu.Where(m => m.FK_AppUser == FK_User && m.FK_AppMenu == menu.PK_AppMenu && m.IsAccessible == true).Any(),

                                          SubMenuList = (from subMenu in bll.db.AppSubMenus.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                                         select new WebSubMenu()
                                                         {
                                                             PK_AppSubMenu = subMenu.PK_AppSubMenu,
                                                             VisibleName = subMenu.VisibleName,
                                                             Link = subMenu.Link,
                                                             IsAccessible = bll.db.AppUser_AppSubMenu.Where(m => m.FK_AppUser == FK_User && m.FK_AppSubMenu == subMenu.PK_AppSubMenu && m.IsAccessible == true).Any(),
                                                             IsActiveParent = subMenu.IsActive,
                                                             Sequence = subMenu.Sequence,
                                                         }
                                                      ).ToList(),

                                          PermissionList = (from permission in bll.db.AppPermissions.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                                            select new Permission()
                                                            {
                                                                PK_AppPermission = permission.PK_AppPermission,
                                                                VisibleName = permission.VisibleName,
                                                                FullName = permission.FullName,
                                                                IsAccessible = bll.db.AppUser_AppPermission.Where(m => m.FK_AppUser == FK_User && m.FK_AppPermission == permission.PK_AppPermission && m.IsAccessible == true).Any(),
                                                                IsActiveParent = permission.IsActive,
                                                                Sequence = permission.Sequence,
                                                            }
                                                      ).ToList()
                                      }
                        ).OrderBy(m => m.FullName).ThenBy(m => m.VisibleName).ToList();

                return View(new Tuple<AppUser, List<WebMenu>>(model, list));
            }
        }
        [HttpPost]
        public ActionResult ManageWebPermission(FormCollection form)
        {
            var FK_AppUser = Guid.Parse(form["FK_AppUser"]);
            try
            {
                var appMenus = bll.db.AppMenus.ToList();
                var oldRole_MenuList = bll.db.AppUser_AppMenu.Where(m => m.FK_AppUser == FK_AppUser).ToList();
                foreach (var menu in appMenus)
                {
                    var menuCehckBox = form["menuCehckBox_" + menu.PK_AppMenu];
                    var menuSequence = form["menuSequence_" + menu.PK_AppMenu];
                    if (menuCehckBox != null)
                    {
                        var roleMenu = oldRole_MenuList.Where(m => m.FK_AppMenu == menu.PK_AppMenu).FirstOrDefault();
                        if (roleMenu != null)
                        {
                            roleMenu.IsAccessible = true;
                            //if (!string.IsNullOrEmpty(menuSequence))
                            //{
                            //    roleMenu.Sequence = Convert.ToInt32(menuSequence);
                            //}
                            //else
                            //{
                            //    roleMenu.Sequence = 0;
                            //}

                            //# Manage Submenu
                            var subMenuList = (from submenu in bll.db.AppSubMenus.Where(m => m.FK_AppMenu == roleMenu.FK_AppMenu)
                                               select submenu).ToList();

                            var oldRoleSubMenuList = (from role_submenu in bll.db.AppUser_AppSubMenu.Where(m => m.FK_AppUser == FK_AppUser)
                                                      join submenu in bll.db.AppSubMenus.Where(m => m.FK_AppMenu == roleMenu.FK_AppMenu) on role_submenu.FK_AppSubMenu equals submenu.PK_AppSubMenu
                                                      select role_submenu).ToList();
                            foreach (var subMenu in subMenuList)
                            {
                                var subMenuCehckBox = form["subMenuCehckBox_" + subMenu.PK_AppSubMenu];
                                if (subMenuCehckBox != null)
                                {
                                    var permittedRoleSubMenu = oldRoleSubMenuList.Where(m => m.FK_AppSubMenu == subMenu.PK_AppSubMenu).FirstOrDefault();
                                    if (permittedRoleSubMenu == null)
                                    {
                                        bll.db.AppUser_AppSubMenu.Add(
                                                new AppUser_AppSubMenu()
                                                {
                                                    FK_AppUser = FK_AppUser,
                                                    FK_AppSubMenu = subMenu.PK_AppSubMenu,
                                                    IsAccessible = true
                                                }
                                            );
                                    }
                                    else
                                    {
                                        permittedRoleSubMenu.IsAccessible = true;
                                    }
                                }
                                else
                                {
                                    var permittedRoleSubMenu = oldRoleSubMenuList.Where(m => m.FK_AppSubMenu == subMenu.PK_AppSubMenu).FirstOrDefault();
                                    if (permittedRoleSubMenu != null)
                                    {
                                        permittedRoleSubMenu.IsAccessible = false;
                                    }
                                }
                            }

                            //# Manage Permission
                            var permissionList = (from permission in bll.db.AppPermissions.Where(m => m.FK_AppMenu == roleMenu.FK_AppMenu)
                                                  select permission).ToList();

                            var oldRolePermissionList = (from role_permission in bll.db.AppUser_AppPermission.Where(m => m.FK_AppUser == FK_AppUser)
                                                         join permission in bll.db.AppPermissions.Where(m => m.FK_AppMenu == roleMenu.FK_AppMenu) on role_permission.FK_AppPermission equals permission.PK_AppPermission
                                                         select role_permission).ToList();
                            foreach (var permission in permissionList)
                            {
                                var permissionCehckBox = form["permissionCehckBox_" + permission.PK_AppPermission];
                                if (permissionCehckBox != null)
                                {
                                    var permittedRolePermission = oldRolePermissionList.Where(m => m.FK_AppPermission == permission.PK_AppPermission).FirstOrDefault();
                                    if (permittedRolePermission == null)
                                    {
                                        bll.db.AppUser_AppPermission.Add(
                                                new AppUser_AppPermission()
                                                {
                                                    FK_AppUser = FK_AppUser,
                                                    FK_AppPermission = permission.PK_AppPermission,
                                                    IsAccessible = true
                                                }
                                            );
                                    }
                                    else
                                    {
                                        permittedRolePermission.IsAccessible = true;
                                    }
                                }
                                else
                                {
                                    var permittedRolePermission = oldRolePermissionList.Where(m => m.FK_AppPermission == permission.PK_AppPermission).FirstOrDefault();
                                    if (permittedRolePermission != null)
                                    {
                                        permittedRolePermission.IsAccessible = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            roleMenu = new AppUser_AppMenu()
                            {
                                FK_AppUser = FK_AppUser,
                                FK_AppMenu = menu.PK_AppMenu,
                                IsAccessible = true
                            };
                            //if (!string.IsNullOrEmpty(menuSequence))
                            //{
                            //    roleMenu.Sequence = Convert.ToInt32(menuSequence);
                            //}
                            //else
                            //{
                            //    roleMenu.Sequence = 0;
                            //}
                            bll.db.AppUser_AppMenu.Add(roleMenu);

                            //# Manage Submenu
                            var subMenuList = (from submenu in bll.db.AppSubMenus.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                               select submenu).ToList();

                            foreach (var subMenu in subMenuList)
                            {
                                var subMenuCehckBox = form["subMenuCehckBox_" + subMenu.PK_AppSubMenu];
                                if (subMenuCehckBox != null)
                                {
                                    bll.db.AppUser_AppSubMenu.Add(
                                        new AppUser_AppSubMenu()
                                        {
                                            FK_AppUser = FK_AppUser,
                                            FK_AppSubMenu = subMenu.PK_AppSubMenu,
                                            IsAccessible = true
                                        });
                                }
                            }

                            //# Manage permission
                            var permissionList = (from permission in bll.db.AppPermissions.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                                  select permission).ToList();

                            foreach (var permission in permissionList)
                            {
                                var permissionCehckBox = form["permissionCehckBox_" + permission.PK_AppPermission];
                                if (permissionCehckBox != null)
                                {
                                    bll.db.AppUser_AppPermission.Add(
                                        new AppUser_AppPermission()
                                        {
                                            FK_AppUser = FK_AppUser,
                                            FK_AppPermission = permission.PK_AppPermission,
                                            IsAccessible = true
                                        });
                                }
                            }
                        }
                    }
                    else
                    {
                        var roleMenu = oldRole_MenuList.Where(m => m.FK_AppMenu == menu.PK_AppMenu).FirstOrDefault();
                        if (roleMenu != null)
                        {
                            roleMenu.IsAccessible = false;
                        }
                    }

                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Extra menu permission is successfully updated.");
                return RedirectToAction("ManageUserAccessibleDepo", new { FK_User = FK_AppUser });
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit_ManageWebPermission(Guid FK_User)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.AppUsers.Where(m => m.PK_User == FK_User).FirstOrDefault();
            if (model == null)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "User not found in Database");
                return RedirectToAction("index");
            }
            else
            {
                List<WebMenu> list = (from menu in bll.db.AppMenus.AsEnumerable()
                                      select new WebMenu()
                                      {
                                          PK_AppMenu = menu.PK_AppMenu,
                                          FullName = menu.FullName,
                                          VisibleName = menu.VisibleName,
                                          ModelName = menu.ModelName,
                                          Icon = menu.Icon,
                                          Link = menu.Link,
                                          Sequence = menu.Sequence,


                                          IsAccessible = bll.db.AppUser_AppMenu.Where(m => m.FK_AppUser == FK_User && m.FK_AppMenu == menu.PK_AppMenu && m.IsAccessible == true).Any(),

                                          SubMenuList = (from subMenu in bll.db.AppSubMenus.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                                         select new WebSubMenu()
                                                         {
                                                             PK_AppSubMenu = subMenu.PK_AppSubMenu,
                                                             VisibleName = subMenu.VisibleName,
                                                             Link = subMenu.Link,
                                                             IsAccessible = bll.db.AppUser_AppSubMenu.Where(m => m.FK_AppUser == FK_User && m.FK_AppSubMenu == subMenu.PK_AppSubMenu && m.IsAccessible == true).Any(),
                                                             IsActiveParent = subMenu.IsActive,
                                                             Sequence = subMenu.Sequence,
                                                         }
                                                      ).ToList(),

                                          PermissionList = (from permission in bll.db.AppPermissions.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                                            select new Permission()
                                                            {
                                                                PK_AppPermission = permission.PK_AppPermission,
                                                                VisibleName = permission.VisibleName,
                                                                FullName = permission.FullName,
                                                                IsAccessible = bll.db.AppUser_AppPermission.Where(m => m.FK_AppUser == FK_User && m.FK_AppPermission == permission.PK_AppPermission && m.IsAccessible == true).Any(),
                                                                IsActiveParent = permission.IsActive,
                                                                Sequence = permission.Sequence,
                                                            }
                                                      ).ToList()
                                      }
                        ).OrderBy(m => m.FullName).ThenBy(m => m.VisibleName).ToList();

                return View(new Tuple<AppUser, List<WebMenu>>(model, list));
            }
        }
        [HttpPost]
        public ActionResult Edit_ManageWebPermission(FormCollection form)
        {
            var FK_AppUser = Guid.Parse(form["FK_AppUser"]);
            try
            {
                var appMenus = bll.db.AppMenus.ToList();
                var oldRole_MenuList = bll.db.AppUser_AppMenu.Where(m => m.FK_AppUser == FK_AppUser).ToList();
                foreach (var menu in appMenus)
                {
                    var menuCehckBox = form["menuCehckBox_" + menu.PK_AppMenu];
                    var menuSequence = form["menuSequence_" + menu.PK_AppMenu];
                    if (menuCehckBox != null)
                    {
                        var roleMenu = oldRole_MenuList.Where(m => m.FK_AppMenu == menu.PK_AppMenu).FirstOrDefault();
                        if (roleMenu != null)
                        {
                            roleMenu.IsAccessible = true;
                            //if (!string.IsNullOrEmpty(menuSequence))
                            //{
                            //    roleMenu.Sequence = Convert.ToInt32(menuSequence);
                            //}
                            //else
                            //{
                            //    roleMenu.Sequence = 0;
                            //}

                            //# Manage Submenu
                            var subMenuList = (from submenu in bll.db.AppSubMenus.Where(m => m.FK_AppMenu == roleMenu.FK_AppMenu)
                                               select submenu).ToList();

                            var oldRoleSubMenuList = (from role_submenu in bll.db.AppUser_AppSubMenu.Where(m => m.FK_AppUser == FK_AppUser)
                                                      join submenu in bll.db.AppSubMenus.Where(m => m.FK_AppMenu == roleMenu.FK_AppMenu) on role_submenu.FK_AppSubMenu equals submenu.PK_AppSubMenu
                                                      select role_submenu).ToList();
                            foreach (var subMenu in subMenuList)
                            {
                                var subMenuCehckBox = form["subMenuCehckBox_" + subMenu.PK_AppSubMenu];
                                if (subMenuCehckBox != null)
                                {
                                    var permittedRoleSubMenu = oldRoleSubMenuList.Where(m => m.FK_AppSubMenu == subMenu.PK_AppSubMenu).FirstOrDefault();
                                    if (permittedRoleSubMenu == null)
                                    {
                                        bll.db.AppUser_AppSubMenu.Add(
                                                new AppUser_AppSubMenu()
                                                {
                                                    FK_AppUser = FK_AppUser,
                                                    FK_AppSubMenu = subMenu.PK_AppSubMenu,
                                                    IsAccessible = true
                                                }
                                            );
                                    }
                                    else
                                    {
                                        permittedRoleSubMenu.IsAccessible = true;
                                    }
                                }
                                else
                                {
                                    var permittedRoleSubMenu = oldRoleSubMenuList.Where(m => m.FK_AppSubMenu == subMenu.PK_AppSubMenu).FirstOrDefault();
                                    if (permittedRoleSubMenu != null)
                                    {
                                        permittedRoleSubMenu.IsAccessible = false;
                                    }
                                }
                            }

                            //# Manage Permission
                            var permissionList = (from permission in bll.db.AppPermissions.Where(m => m.FK_AppMenu == roleMenu.FK_AppMenu)
                                                  select permission).ToList();

                            var oldRolePermissionList = (from role_permission in bll.db.AppUser_AppPermission.Where(m => m.FK_AppUser == FK_AppUser)
                                                         join permission in bll.db.AppPermissions.Where(m => m.FK_AppMenu == roleMenu.FK_AppMenu) on role_permission.FK_AppPermission equals permission.PK_AppPermission
                                                         select role_permission).ToList();
                            foreach (var permission in permissionList)
                            {
                                var permissionCehckBox = form["permissionCehckBox_" + permission.PK_AppPermission];
                                if (permissionCehckBox != null)
                                {
                                    var permittedRolePermission = oldRolePermissionList.Where(m => m.FK_AppPermission == permission.PK_AppPermission).FirstOrDefault();
                                    if (permittedRolePermission == null)
                                    {
                                        bll.db.AppUser_AppPermission.Add(
                                                new AppUser_AppPermission()
                                                {
                                                    FK_AppUser = FK_AppUser,
                                                    FK_AppPermission = permission.PK_AppPermission,
                                                    IsAccessible = true
                                                }
                                            );
                                    }
                                    else
                                    {
                                        permittedRolePermission.IsAccessible = true;
                                    }
                                }
                                else
                                {
                                    var permittedRolePermission = oldRolePermissionList.Where(m => m.FK_AppPermission == permission.PK_AppPermission).FirstOrDefault();
                                    if (permittedRolePermission != null)
                                    {
                                        permittedRolePermission.IsAccessible = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            roleMenu = new AppUser_AppMenu()
                            {
                                FK_AppUser = FK_AppUser,
                                FK_AppMenu = menu.PK_AppMenu,
                                IsAccessible = true
                            };
                            //if (!string.IsNullOrEmpty(menuSequence))
                            //{
                            //    roleMenu.Sequence = Convert.ToInt32(menuSequence);
                            //}
                            //else
                            //{
                            //    roleMenu.Sequence = 0;
                            //}
                            bll.db.AppUser_AppMenu.Add(roleMenu);

                            //# Manage Submenu
                            var subMenuList = (from submenu in bll.db.AppSubMenus.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                               select submenu).ToList();

                            foreach (var subMenu in subMenuList)
                            {
                                var subMenuCehckBox = form["subMenuCehckBox_" + subMenu.PK_AppSubMenu];
                                if (subMenuCehckBox != null)
                                {
                                    bll.db.AppUser_AppSubMenu.Add(
                                        new AppUser_AppSubMenu()
                                        {
                                            FK_AppUser = FK_AppUser,
                                            FK_AppSubMenu = subMenu.PK_AppSubMenu,
                                            IsAccessible = true
                                        });
                                }
                            }

                            //# Manage permission
                            var permissionList = (from permission in bll.db.AppPermissions.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                                  select permission).ToList();

                            foreach (var permission in permissionList)
                            {
                                var permissionCehckBox = form["permissionCehckBox_" + permission.PK_AppPermission];
                                if (permissionCehckBox != null)
                                {
                                    bll.db.AppUser_AppPermission.Add(
                                        new AppUser_AppPermission()
                                        {
                                            FK_AppUser = FK_AppUser,
                                            FK_AppPermission = permission.PK_AppPermission,
                                            IsAccessible = true
                                        });
                                }
                            }
                        }
                    }
                    else
                    {
                        var roleMenu = oldRole_MenuList.Where(m => m.FK_AppMenu == menu.PK_AppMenu).FirstOrDefault();
                        if (roleMenu != null)
                        {
                            roleMenu.IsAccessible = false;
                        }
                    }

                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Alternative permission is successfully updated.");
                return RedirectToAction("Edit_ManageWebPermission", new { FK_User = FK_AppUser });
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Edit_ManageWebPermission", new { FK_User = FK_AppUser });
            }
        }
        #endregion

        //# Depo Access
        #region
        public ActionResult ManageUserAccessibleDepo(Guid FK_User)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var user = bll.db.AppUsers.Where(m => m.PK_User == FK_User).FirstOrDefault();
            return View(user);
        }
        public JsonResult GetAccessibleDepoListOfUser(Guid FK_User)
        {
            var list = (from depo in bll.db.Depoes.Where(m => m.IsDeleted != true && (!m.Category.Contains("Physical"))).AsEnumerable()
                        select new
                        {
                            FK_Depo = depo.PK_Depo,
                            DepoName = depo.Name,
                            PRG_Type = depo.PRG_Type,
                            IsAccessible = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_Depo == depo.PK_Depo && m.FK_AppUser == FK_User && m.IsAccessible == true).Any()
                        }
                        ).OrderBy(m => m.PRG_Type).ThenBy(m => m.DepoName).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ManageUserAccessibleDepo(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var FK_User = Guid.Parse(form["FK_User"]);
            var PermittedDepoes = form["PermittedDepoes"];
            try
            {

                var PermittedDepoe_FKs = PermittedDepoes.Split(',');
                var oldPermittedDepoList = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == FK_User).ToList();
                foreach (var item in oldPermittedDepoList)
                {
                    //# Update existing permission
                    if (PermittedDepoe_FKs.Contains(item.FK_Depo.ToString()))
                    {
                        item.IsAccessible = true;
                    }
                    else
                    {
                        item.IsAccessible = false;
                    }
                }
                if (PermittedDepoe_FKs.Count() > 0)
                {

                    foreach (var FK in PermittedDepoe_FKs)
                    {
                        if (FK == "")
                        {
                            break;
                        }
                        //# Add new parmision
                        if (!oldPermittedDepoList.Where(m => m.FK_Depo.ToString() == FK).Any())
                        {
                            bll.db.AppUserAccessibleDepoes.Add(
                                    new AppUserAccessibleDepo()
                                    {
                                        FK_AppUser = FK_User,
                                        FK_Depo = Guid.Parse(FK),
                                        IsAccessible = true
                                    });
                        }
                    }
                }


                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Permission is successfully updated.");
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                var user = bll.db.AppUsers.Where(m => m.PK_User == FK_User).FirstOrDefault();
                return View(user);
            }
        }

        public ActionResult Edit_ManageUserAccessibleDepo(Guid FK_User)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var user = bll.db.AppUsers.Where(m => m.PK_User == FK_User).FirstOrDefault();
            //ViewBag.AppUserAccessibleDepoes = user.AppUserAccessibleDepoes.Where(m => m.IsAccessible == true).Select(m => m.Depo.Name).OrderBy(m => m).ToList();
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit_ManageUserAccessibleDepo(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var FK_User = Guid.Parse(form["FK_User"]);
            var PermittedDepoes = form["PermittedDepoes"];
            try
            {

                var PermittedDepoe_FKs = PermittedDepoes.Split(',');
                var oldPermittedDepoList = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == FK_User).ToList();
                foreach (var item in oldPermittedDepoList)
                {
                    //# Update existing permission
                    if (PermittedDepoe_FKs.Contains(item.FK_Depo.ToString()))
                    {
                        item.IsAccessible = true;
                    }
                    else
                    {
                        item.IsAccessible = false;
                    }
                }
                if (PermittedDepoe_FKs.Count() > 0)
                {

                    foreach (var FK in PermittedDepoe_FKs)
                    {
                        if (FK == "")
                        {
                            break;
                        }
                        //# Add new parmision
                        if (!oldPermittedDepoList.Where(m => m.FK_Depo.ToString() == FK).Any())
                        {
                            bll.db.AppUserAccessibleDepoes.Add(
                                    new AppUserAccessibleDepo()
                                    {
                                        FK_AppUser = FK_User,
                                        FK_Depo = Guid.Parse(FK),
                                        IsAccessible = true
                                    });
                        }
                    }
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Permission is successfully updated.");
                return RedirectToAction("Edit_ManageUserAccessibleDepo", new { FK_User = FK_User });
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                var user = bll.db.AppUsers.Where(m => m.PK_User == FK_User).FirstOrDefault();
                return View(user);
            }
        }
        #endregion



        //# Agentship and propossable Depo
        #region
        public ActionResult ManageInternalAgent_ProposableDepoAndManagableContructualCompany(Guid FK_User)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var user = bll.db.AppUsers.Where(m => m.PK_User == FK_User).FirstOrDefault();
            ViewBag.AppUserTypeDict = new SelectList(AppUserTypeDict, "Key", "Value", user.AppUserType);
            return View(user);
        }
        public JsonResult GetProposableDepoListAndManagableCOntructualCompanyByInternalAgent(Guid FK_User)
        {
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var userDepo_FK = bll.db.AppUsers.Where(m => m.PK_User == FK_User).Select(m => m.FK_Depo).FirstOrDefault();

            var depoList = (from depo in bll.db.Depoes.AsEnumerable()
                            where (!depo.Category.Contains("Physical")) && depo.PK_Depo != _invalidDepoPK && depo.PK_Depo != userDepo_FK
                            select new
                            {
                                FK_Depo = depo.PK_Depo,
                                DepoName = depo.Name,
                                PRG_Type = depo.PRG_Type,
                                IsProposable = bll.db.RequisitionAgentProposedDepoes.Where(m => m.FK_Depo == depo.PK_Depo && m.FK_RequisitionAgent == FK_User && m.WillPropose == true).Any()
                            }
                        ).OrderBy(m => m.DepoName).ToList();

            var contructualCompanyList = (from comp in bll.db.ContructualRequisitionCompanies.AsEnumerable()
                                          select new
                                          {
                                              FK_ContructualRequisitionCompany = comp.PK_ContructualRequisitionCompany,
                                              comp.Name,
                                              WillSupervise = bll.db.AppUserSurpervisedContructualCompanies.Where(m => m.FK_ContructualRequisitionCompany == comp.PK_ContructualRequisitionCompany && m.FK_AppUser == FK_User && m.WillSupervise == true).Any()
                                          }
                        ).OrderBy(m => m.Name).ToList();
            return Json(new { depoList = depoList, contructualCompanyList = contructualCompanyList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ManageInternalAgent_ProposableDepoAndManagableContructualCompany(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var FK_User = Guid.Parse(form["FK_User"]);
            var user = bll.db.AppUsers.Where(m => m.PK_User == FK_User).FirstOrDefault();
            //#set type
            var AppUserType = form["AppUserType"];
            user.AppUserType = AppUserType;

            try
            {
                //# Depo parmission
                var PermittedDepoes = form["PermittedDepoes"];
                if (user.AppUserType == "Internal")
                {
                    PermittedDepoes = "";
                }

                var PermittedDepoe_FKs = PermittedDepoes.Split(',');
                var oldPermittedDepoList = bll.db.RequisitionAgentProposedDepoes.Where(m => m.FK_RequisitionAgent == FK_User).ToList();
                foreach (var item in oldPermittedDepoList)
                {
                    //# Update existing permission
                    if (PermittedDepoe_FKs.Contains(item.FK_Depo.ToString()))
                    {
                        item.WillPropose = true;
                    }
                    else
                    {
                        item.WillPropose = false;
                    }
                }
                if (user.AppUserType == "Internal Transport Agent" && PermittedDepoe_FKs.Count() > 0)
                {
                    foreach (var FK in PermittedDepoe_FKs)
                    {
                        if (FK == "")
                        {
                            break;
                        }
                        //# Add new parmision
                        if (!oldPermittedDepoList.Where(m => m.FK_Depo.ToString() == FK).Any())
                        {
                            bll.db.RequisitionAgentProposedDepoes.Add(
                                    new RequisitionAgentProposedDepo()
                                    {
                                        FK_RequisitionAgent = FK_User,
                                        FK_Depo = Guid.Parse(FK),
                                        WillPropose = true
                                    });
                        }
                    }
                }

                //# Contructual Company parmission
                var PermittedComapanies = form["PermittedComapanies"];
                if (user.AppUserType == "Internal")
                {
                    PermittedComapanies = "";
                }


                var PermittedComapanies_FKs = PermittedComapanies.Split(',');
                var oldPermittedCompanyList = bll.db.AppUserSurpervisedContructualCompanies.Where(m => m.FK_AppUser == FK_User).ToList();
                foreach (var item in oldPermittedCompanyList)
                {
                    //# Update existing permission
                    if (PermittedComapanies_FKs.Contains(item.FK_ContructualRequisitionCompany.ToString()))
                    {
                        item.WillSupervise = true;
                    }
                    else
                    {
                        item.WillSupervise = false;
                    }
                }
                if (user.AppUserType == "Internal Transport Agent" && PermittedComapanies_FKs.Count() > 0)
                {
                    foreach (var FK in PermittedComapanies_FKs)
                    {
                        if (FK == "")
                        {
                            break;
                        }
                        //# Add new parmision
                        if (!oldPermittedCompanyList.Where(m => m.FK_ContructualRequisitionCompany.ToString() == FK).Any())
                        {
                            bll.db.AppUserSurpervisedContructualCompanies.Add(
                                    new AppUserSurpervisedContructualCompany()
                                    {
                                        FK_AppUser = FK_User,
                                        FK_ContructualRequisitionCompany = Guid.Parse(FK),
                                        WillSupervise = true
                                    });
                        }
                    }
                }
                CreateAlertMessage(AlertMessageType.Success, "Success", "Permission is successfully updated.");
                bll.db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return View(user);
            }
        }

        public ActionResult Edit_ManageInternalAgent_ProposableDepoAndManagableContructualCompany(Guid FK_User)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var user = bll.db.AppUsers.Where(m => m.PK_User == FK_User).FirstOrDefault();
            ViewBag.AppUserTypeDict = new SelectList(AppUserTypeDict, "Key", "Value", user.AppUserType);
            ViewBag.RequisitionAgentProposedDepoes = user.RequisitionAgentProposedDepoes.Where(m => m.WillPropose == true).Select(m => m.Depo.Name).OrderBy(m => m).ToList();
            ViewBag.RequisitionAgentSupervisedCompanies = user.AppUserSurpervisedContructualCompanies.Where(m => m.WillSupervise == true).Select(m => m.ContructualRequisitionCompany.Name).OrderBy(m => m).ToList();
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit_ManageInternalAgent_ProposableDepoAndManagableContructualCompany(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var FK_User = Guid.Parse(form["FK_User"]);
            var user = bll.db.AppUsers.Where(m => m.PK_User == FK_User).FirstOrDefault();
            //#set type
            var AppUserType = form["AppUserType"];
            user.AppUserType = AppUserType;

            try
            {
                //# Depo parmission
                var PermittedDepoes = form["PermittedDepoes"];
                if (user.AppUserType == "Internal")
                {
                    PermittedDepoes = "";
                }

                var PermittedDepoe_FKs = PermittedDepoes.Split(',');
                var oldPermittedDepoList = bll.db.RequisitionAgentProposedDepoes.Where(m => m.FK_RequisitionAgent == FK_User).ToList();
                foreach (var item in oldPermittedDepoList)
                {
                    //# Update existing permission
                    if (PermittedDepoe_FKs.Contains(item.FK_Depo.ToString()))
                    {
                        item.WillPropose = true;
                    }
                    else
                    {
                        item.WillPropose = false;
                    }
                }
                if (user.AppUserType == "Internal Transport Agent" && PermittedDepoe_FKs.Count() > 0)
                {
                    foreach (var FK in PermittedDepoe_FKs)
                    {
                        if (FK == "")
                        {
                            break;
                        }
                        //# Add new parmision
                        if (!oldPermittedDepoList.Where(m => m.FK_Depo.ToString() == FK).Any())
                        {
                            bll.db.RequisitionAgentProposedDepoes.Add(
                                    new RequisitionAgentProposedDepo()
                                    {
                                        FK_RequisitionAgent = FK_User,
                                        FK_Depo = Guid.Parse(FK),
                                        WillPropose = true
                                    });
                        }
                    }
                }

                //# Contructual Company parmission
                var PermittedComapanies = form["PermittedComapanies"];
                if (user.AppUserType == "Internal")
                {
                    PermittedComapanies = "";
                }


                var PermittedComapanies_FKs = PermittedComapanies.Split(',');
                var oldPermittedCompanyList = bll.db.AppUserSurpervisedContructualCompanies.Where(m => m.FK_AppUser == FK_User).ToList();
                foreach (var item in oldPermittedCompanyList)
                {
                    //# Update existing permission
                    if (PermittedComapanies_FKs.Contains(item.FK_ContructualRequisitionCompany.ToString()))
                    {
                        item.WillSupervise = true;
                    }
                    else
                    {
                        item.WillSupervise = false;
                    }
                }
                if (user.AppUserType == "Internal Transport Agent" && PermittedComapanies_FKs.Count() > 0)
                {
                    foreach (var FK in PermittedComapanies_FKs)
                    {
                        if (FK == "")
                        {
                            break;
                        }
                        //# Add new parmision
                        if (!oldPermittedCompanyList.Where(m => m.FK_ContructualRequisitionCompany.ToString() == FK).Any())
                        {
                            bll.db.AppUserSurpervisedContructualCompanies.Add(
                                    new AppUserSurpervisedContructualCompany()
                                    {
                                        FK_AppUser = FK_User,
                                        FK_ContructualRequisitionCompany = Guid.Parse(FK),
                                        WillSupervise = true
                                    });
                        }
                    }
                }
                CreateAlertMessage(AlertMessageType.Success, "Success", "Permission is successfully updated.");
                bll.db.SaveChanges();
                return RedirectToAction("Edit_ManageInternalAgent_ProposableDepoAndManagableContructualCompany", new { FK_User = FK_User });
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return View(user);
            }
        }
        #endregion

        #region //CURENT Sattus
        //public ActionResult AppUserLoginReport(DateTime? StartingDate, DateTime? EndingDate, Guid? FK_Depo, Guid? FK_User)
        public ActionResult AppUserLoginReport()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            List<SelectListItem> DepoList = new List<SelectListItem>();

            DepoList.Add(new SelectListItem() { Value = "all", Text = "All Depo" });
            DepoList.Add(new SelectListItem() { Value = null, Text = "Not selected" });
            DepoList.AddRange(bll.db.Depoes.AsEnumerable().Where(m => m.IsDeleted == false && (!m.Category.Contains("Physical")) && accessibleDepoes.Contains(m.PK_Depo)).OrderBy(m => m.Name).Select(m => new SelectListItem { Value = m.PK_Depo.ToString(), Text = m.Name }));
            ViewBag.Depoes = new SelectList(DepoList.OrderBy(m => m.Text), "Value", "Text");

            List<SelectListItem> AppUserList = new List<SelectListItem>();
            AppUserList.Add(new SelectListItem() { Value = "all", Text = "All User" });
            AppUserList.Add(new SelectListItem() { Value = null, Text = "Not selected" });
            List<AppUser> list;
            if (CurrentUser.PRG_Type == "ALL")
            {
                list = bll.db.AppUsers.AsEnumerable().Where(c => c.IsDeleted == false && (c.AppUserType == "Internal" || c.AppUserType == "Internal Transport Agent")).ToList();
            }
            else
            {
                list = bll.db.AppUsers.AsEnumerable().Where(c => c.IsDeleted == false && c.PRG_Type == CurrentUser.PRG_Type).ToList();
            }
            AppUserList.AddRange(list.OrderBy(m => m.UniqueIDNumber).Select(m => new SelectListItem { Value = m.PK_User.ToString(), Text = m.UniqueIDNumber }));
            ViewBag.AppUsers = new SelectList(AppUserList, "Value", "Text");

            return View();
        }
        public JsonResult GetppUserLoginHistory(DateTime StartingDate, DateTime EndingDate, string FK_Depo, string FK_User)
        {
            EndingDate = EndingDate.AddDays(1);
            var query = bll.db.AppUserLoginHistories.Where(m => m.LoginTime >= StartingDate && m.LoginTime <= EndingDate);
            if (FK_Depo != "all")
            {
                var guid = Guid.Parse(FK_Depo);
                query = query.Where(m => m.AppUser.FK_Depo == guid);
            }
            if (FK_User != "all")
            {
                var guid = Guid.Parse(FK_User);
                query = query.Where(m => m.FK_AppUser == guid);
            }
            var list = query.Select(m => new
            {
                m.PK_AppUserLoginHistory,
                m.AppUser.UniqueIDNumber,
                m.AppUser.FullName,
                DepoName = m.AppUser.Depo.Name,
                m.LoginTime,
                m.Reason,
                m.ExpirationTime,
                ExistingMinute = DbFunctions.DiffMinutes(m.LoginTime, m.ExpirationTime)
            }).OrderBy(m => m.LoginTime).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public ActionResult Delete(Guid PK_User)
        {
            if (PK_User == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.AppUsers.Find(PK_User);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "User is successfully deleted.");
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        //Ajax methods
        public JsonResult GetUserDetail(string UniqueIDNumber)
        {
            var appUser = bll.db.AppUsers.Where(m => m.UniqueIDNumber == UniqueIDNumber).Select(m => new
            {
                m.PK_User,
                m.PRG_Type,
                m.FullName,
                m.ContactNumber,
                m.Depo.Name
            }).FirstOrDefault();
            if (appUser != null)
            {
                return Json(appUser, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("NotFound", JsonRequestBehavior.AllowGet);
            }
        }

    }
}