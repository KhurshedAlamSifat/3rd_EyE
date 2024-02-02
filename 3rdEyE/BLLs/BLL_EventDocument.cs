using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;


namespace _3rdEyE.BLL
{
    public class BLL_EventDocument : BaseBLL
    {
        //    //public List<EventDocument> GetAllModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Select(c => c ).ToList();
        //    //    return list;
        //    //}
        //    //public List<EventDocument> GetActiveModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Where(c => c.IsActive == true).Select(c => c).ToList();
        //    //    return list;
        //    //}

        //    //public List<VM_EventDocument> GetAllViewModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Select(c => ConvertToViewModel(c)).ToList();
        //    //    return list;
        //    //}
        public List<VM_EventDocument> GetAllViewModels()
        {
            var list = db.EventDocuments.AsEnumerable().Where(c => c.IsDeleted == false).Select(c => ConvertToViewModel(c)).ToList();
            return list;
        }

        public List<VM_EventDocument> GetAllViewModelsBy_FK_Event(Guid FK_Event)
        {
            var list = db.EventDocuments.AsEnumerable().Where(c => c.IsDeleted == false && c.FK_Event == FK_Event).Select(c => ConvertToViewModel(c)).ToList();
            return list;
        }

        public string IsValidModel_ToCreate(EventDocument model)
        {
            string result = "";
            //if (db.EventDocuments.Where(c => c.UniqueIDNumber.ToUpper() == model.UniqueIDNumber.Trim()).Any())
            //{
            //    result += "This unique ID number is already used by another EventDocument. Please, use an unique id number.";
            //    
            //}

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit(EventDocument model)
        {
            string result = "";

            //# checks, registraton number is unique
            //var existing = db.EventDocuments.Where(c => c.PK_EventDocument != model.PK_EventDocument && c.UniqueIDNumber.ToUpper() == model.UniqueIDNumber.Trim()).Select(v => v).ToList().Count();
            //if (existing > 1)
            //{
            //    result += "This unique ID number is already used by another EventDocument. Please, use an unique id number.";
            //    
            //}

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }

        public string IsValidImage(HttpPostedFileBase Image)
        {
            string result = "";


            //# checks, Imgae is bigger than 1 mb, 1mb=1048576
            if (Image.ContentLength > (1048576 * 1))
            {
                result += "Image is too big. please, upload small image ";
                
            }
            if (CommonClass.IsInvalidImageFormat(Image.ContentType))
            {
                result += "Given file is not an image. ";
                
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }

        public EventDocument FilterToDBModel(EventDocument model)
        {
            EventDocument db_model;
            if (model.PK_EventDocument.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                db_model = new EventDocument();
                db_model.PK_EventDocument = Guid.NewGuid();
                db_model.IsDeleted = false;

                db_model.CreatedAt = DateTime.Now;
                db_model.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;
            }
            else
            {
                db_model = db.EventDocuments.Find(model.PK_EventDocument);
            }
            db_model.FK_Event = model.FK_Event;
            db_model.Title = model.Title;
            db_model.Title = model.Title;
            db_model.IdentitficaitonKey = model.IdentitficaitonKey;
            db_model.IdentitficaitonValue = model.IdentitficaitonValue;
            db_model.ImageLocation = model.ImageLocation;
            return db_model;
        }

        public VM_EventDocument ConvertToViewModel(EventDocument model)
        {
            var viewModel = new VM_EventDocument();
            viewModel.Model = model;
            return viewModel;
        }
    }
}