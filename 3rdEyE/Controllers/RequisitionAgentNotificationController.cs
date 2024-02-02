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
using _3rdEyE.BLLs;

namespace _3rdEyE.Controllers
{
    public class RequisitionAgentNotificationController : BaseController
    {
        BLL_IndividualRequisition bll = new BLL_IndividualRequisition();
        public ActionResult Index()
        {
            var list = (from not in bll.db.RequisitionAgentNotifications
                        where not.FK_RequisitionAgent == CurrentUser.PK_User
                        orderby not.CreatedAt descending
                        select not).ToList();
            //ViewBag.Notifications = list;
            return View(list);
        }

        public JsonResult GetRequisitonAgentNotificaitonsData(Int64 lastNotificationID)

        {
            if (CurrentUser == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            var list = (from not in bll.db.RequisitionAgentNotifications
                        where not.FK_RequisitionAgent == CurrentUser.PK_User
                        && not.PK_RequisitionAgentNotification > lastNotificationID
                        && not.Status == 0
                        //orderby not.CreatedAt descending
                        select new
                        {
                            not.PK_RequisitionAgentNotification,
                            not.Title,
                            not.SubTitle,
                            not.Status,
                            not.ViewLink,
                            not.Category,
                        }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public void snoozSeenNotification(Int64 lastNotificationID)
        {
            var list = (from not in bll.db.RequisitionAgentNotifications
                        where not.FK_RequisitionAgent == CurrentUser.PK_User
                        where not.PK_RequisitionAgentNotification <= lastNotificationID && not.Status == 0
                        select not).ToList();
            foreach (var item in list)
            {
                item.Status = 1;
            }
            bll.db.SaveChanges();
        }
    }
}