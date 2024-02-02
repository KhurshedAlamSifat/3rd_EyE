using _3rdEyE.BLL;
using _3rdEyE.ManagingTools;
using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class MixedController : BaseController
    {
        [HttpGet]
        public ActionResult AddDepoGeofence()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddDepoGeofence(FormCollection form)
        {
            try
            {
                var _Fk_Depo = form["FK_Depo"];
                var FK_Depo = Guid.Parse(_Fk_Depo);
                var oldBorders = bll.db.DepoBorders.Where(m => m.FK_Depo == FK_Depo).ToList();
                if (oldBorders.Count > 0)
                {
                    bll.db.DepoBorders.RemoveRange(oldBorders);
                }
                var newBorders = new List<DepoBorder>();
                var LatLongs_string = form["LatLongs"];
                var LatLongs_array = LatLongs_string.Split('#');
                for (int i = 0; i < LatLongs_array.Count(); i++)
                {
                    var lat_string = LatLongs_array[i].Split('*')[0];
                    var lng_string = LatLongs_array[i].Split('*')[1];
                    newBorders.Add(
                        new DepoBorder()
                        {
                            FK_Depo = FK_Depo,
                            Latitude = Convert.ToDouble(lat_string),
                            Longitude = Convert.ToDouble(lng_string)
                        }
                        );
                }
                bll.db.DepoBorders.AddRange(newBorders);
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Border Added");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return View();
        }
        public JsonResult GetDepoListForGeoFence()
        {
            var depoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true && bll.db.DepoBorders.Where(b => b.FK_Depo == m.FK_Depo).Count() == 0).Select(m => new
            {
                m.Depo.Name,
                m.Depo.PK_Depo,
                m.Depo.Latitude,
                m.Depo.Longitude,
            }).ToList();
            return Json(depoes, JsonRequestBehavior.AllowGet);
        }
    }
}