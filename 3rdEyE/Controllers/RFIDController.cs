using _3rdEyE.BLL;
using _3rdEyE.BLLs;
using _3rdEyE.ManagingTools;
using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class RFIDController : BaseController
    {
        // GET: RFID
        public ActionResult LogIndex(DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (StartingDate != null && EndingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
                var list = bll.db.RFID_EntryLog.AsEnumerable().Where(m => m.EntryLogedAt >= _StartingDate && m.EntryLogedAt <= _EndingDate).OrderByDescending(m => m.ID).ToList();
                return View(list);
            }
            else
            {
                var now = DateTime.Now;
                var today7 = DateTime.Now.Date;
                if (now > today7)
                {
                    StartingDate = today7;
                    EndingDate = today7.AddDays(1);
                }
                else
                {
                    StartingDate = today7.AddDays(-365);
                    EndingDate = today7;
                }
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", EndingDate);
                var list = new List<RFID_EntryLog>();
                return View(list);
            }
        }
        public ActionResult EntryIndex()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = bll.db.RFID_Entry.AsEnumerable()./*Where(m => m.LastEntryAt >= _StartingDate && m.LastEntryAt <= _EndingDate).*/OrderByDescending(m => m.ID).ToList();
            return View(list);
        }
    }
}