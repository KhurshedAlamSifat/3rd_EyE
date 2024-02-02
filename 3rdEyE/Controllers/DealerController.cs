using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.BLL;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;
using Microsoft.Reporting.WebForms;
using System.Data.Entity;

namespace _3rdEyE.Controllers
{
    public class DealerController : BaseController
    {
        public ActionResult AssignVehicle()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var vehicles = (from vt in bll.db.VehicleTrackings
                            join v in bll.db.Vehicles on vt.PK_Vehicle equals v.PK_Vehicle
                            select v).OrderBy(m => m.RegistrationNumber).ToList();
            ViewBag.Vehicles = new SelectList(vehicles.OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
            return View();
        }
        [HttpPost]
        public ActionResult AssignVehicle(FormCollection form)
        {
            var vehicles = (from vt in bll.db.VehicleTrackings
                            join v in bll.db.Vehicles on vt.PK_Vehicle equals v.PK_Vehicle
                            select v).OrderBy(m => m.RegistrationNumber).ToList();
            ViewBag.Vehicles = new SelectList(vehicles.OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");

            var FK_Vehicle = form["FK_Vehicle"];
            var DealerCodes = form["DealerCodes"];

            var DealerCodesArray = DealerCodes.TrimEnd(',').Split(',');
            foreach (var dc in DealerCodesArray)
            {
                var dealer = bll.db.Dealers.Where(d => d.DealerCode == dc).FirstOrDefault();
                if (dealer == null)
                {
                    dealer = new Dealer()
                    {
                        PK_Dealer = Guid.NewGuid(),
                        DealerCode = dc,
                        Password = dc,
                        AssignTime = DateTime.Now,
                        FK_Vehicle = Guid.Parse(FK_Vehicle)
                    };
                    bll.db.Dealers.Add(dealer);
                }
                else
                {
                    dealer.FK_Vehicle = Guid.Parse(FK_Vehicle);
                    dealer.AssignTime = DateTime.Now;
                }
            }
            bll.db.SaveChanges();
            CreateAlertMessage(AlertMessageType.Success, "Success", "Permission is successfully updated.");
            return View();
        }
    }
}