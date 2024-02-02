using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;
using _3rdEyE.BLLs;
using _3rdEyE.BLL;


namespace _3rdEyE.BLLs
{
    public class BLL_Depo : BaseBLL
    {
        public string IsValidModel_ToCreate(Depo model)
        {
            string result = "";

            //# checks, name is unique
            if (db.Depoes.Where(c => c.Name.ToUpper() == model.Name.ToUpper().Trim()).Any())
            {
                result += "This Depo name is already used by another compnay. Please, use an unique name. ";

            }
            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit(Depo model)
        {
            string result = "";

            //# checks, name is unique
            if (db.Depoes.Where(c => c.PK_Depo != model.PK_Depo && c.Name.ToUpper() == model.Name.ToUpper().Trim()).Any())
            {
                result += "This Depo name is already used by another compnay. Please, use an unique name. ";

            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }

    }
}