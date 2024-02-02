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
using System.IO;
using System.IO.Compression;

namespace _3rdEyE.Controllers
{
    public class EventDocumentController : BaseController
    {

        BLL_EventDocument bll = new BLL_EventDocument();

        //public ActionResult Index()
        //{
        //    var list = bll.GetAllViewModels();
        //    SetPermission(true, true, true, true);
        //    return View(list);
        //}

        public ActionResult IndexBy_Event(Guid id)
        {
            ViewBag.parentKey = id;
            var list = bll.GetAllViewModelsBy_FK_Event(id);
            return View(list);
        }

        public ActionResult _IndexBy_Event(Guid id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.parentKey = id;
            var list = bll.GetAllViewModelsBy_FK_Event(id);
            return View(list);
        }

        public ActionResult View(Guid id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.EventDocuments.Find(id);
                if (model != null)
                {
                    var viewModel = bll.ConvertToViewModel(model);

                    ViewBag.viewModel = viewModel;
                    return View();
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        public ActionResult Create(Guid parentKey)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (parentKey == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new EventDocument();
            model.FK_Event = parentKey;
            ViewBag.model = model;
            return View();
        }

        [HttpPost]
        public ActionResult Create(EventDocument model, HttpPostedFileBase ImageInput)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            string modelValidator = bll.IsValidModel_ToCreate(model);

            string imageValidator = ValidationStatus.OK;
            if (ImageInput != null)
            {
                imageValidator = bll.IsValidImage(ImageInput);
            }

            if (modelValidator == ValidationStatus.OK && imageValidator == ValidationStatus.OK)
            {

                try
                {
                    if (ImageInput != null)
                    {
                        var _event = bll.db.Events.Where(e => e.PK_Event == model.FK_Event).FirstOrDefault();
                        //# to generate a folder like /DHAKA METRO 22 2124/Registraion
                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + _event.FK_Vehicle + "/" + _event.EventType.Title + "/";

                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }

                        string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + "." + ImageInput.FileName.Split('.').Last();
                        ImageInput.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                        model.ImageLocation = virtualFilePath;

                    }
                    var db_model = bll.FilterToDBModel(model);
                    bll.db.EventDocuments.Add(db_model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Document is successfully added.");
                    return RedirectToAction("View", "Event", new { id = model.FK_Event });
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                string validators = "";
                if (modelValidator != ValidationStatus.OK)
                {
                    validators = validators + modelValidator;
                }
                if (imageValidator != ValidationStatus.OK)
                {
                    validators = validators + imageValidator;
                }
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", validators);
            }
            return RedirectToAction("Create", new { parentKey = model.FK_Event });
        }

        public ActionResult Delete(Guid id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.EventDocuments.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;

                        model.DeletedAt = DateTime.Now;
                        model.FK_DeletedByUser = CommonClass.GetCurrentUser().PK_User;

                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Document is successfully deleted.");
                        return RedirectToAction("View", "Event", new { id = model.FK_Event });
                    }
                    catch (Exception exception)
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                        return RedirectToAction("View", "Event", new { id = model.FK_Event });
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        public FileResult Download(Guid id)
        {
            var model = bll.db.EventDocuments.Find(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath(model.ImageLocation)));
            string fileName = model.ImageLocation.Split('/').Last();

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        //public FileResult DownloadAll(Guid id)
        //{
        //    var documentList = bll.db.EventDocuments.Where(m => m.FK_Event == id && m.IsDeleted == false).ToList(); ;

        //    var archive = Server.MapPath("~/archive.zip");
        //    var temp = Server.MapPath("~/__DD_TemporaryFolder");

        //    // clear any existing archive
        //    if (System.IO.File.Exists(archive))
        //    {
        //        System.IO.File.Delete(archive);
        //    }
        //    // empty the temp folder
        //    Directory.EnumerateFiles(temp).ToList().ForEach(f => System.IO.File.Delete(f));

        //    // copy the selected files to the temp folder
        //    //documentList.ForEach(f => System.IO.File.Copy(CommonClass.ImageDirectory + f.ImageLocation, Path.Combine(temp, Path.GetFileName(f.ImageLocation))));
        //    foreach (var doc in documentList)
        //    {
        //        System.IO.File.Copy(Path.Combine(Server.MapPath(doc.ImageLocation)), "~"+doc.ImageLocation);
        //    }

        //    // create a new archive
        //    ZipFile.CreateFromDirectory(temp, archive);

        //    return File(archive, "application/zip", "archive.zip");
        //}

    }
}