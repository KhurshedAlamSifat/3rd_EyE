using _3rdEyE.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.Models;

namespace _3rdEyE.Controllers
{
    public class RFID_APIController : Controller
    {
        public BaseBLL bll = new BaseBLL();
        public JsonResult Insert(string TotalText_Raw, string GateID)
        {
            string _DistText_Raw = "", _LetterText_Raw = "", _LastText_Raw = "";
            try
            {
                var array = TotalText_Raw.Split('-').ToArray();
                _DistText_Raw = array[0] + "-" + array[1];
                var _DistAuto = bll.db.RFID_AutoDistSuggession.Where(m => m.RawText == _DistText_Raw).FirstOrDefault();
                _LetterText_Raw = array[2];
                var _LetterAuto = bll.db.RFID_AutoLetterSuggession.Where(m => m.RawText == _DistText_Raw).FirstOrDefault();
                _LastText_Raw = array[3] + "-" + array[4];

                var _RFID_Entry = bll.db.RFID_Entry.Where(m => m.TotalText_Raw == TotalText_Raw).FirstOrDefault();
                if (_RFID_Entry != null)
                {
                    if (_DistAuto != null)
                    {
                        _RFID_Entry.DistText_Auto = _DistAuto.DistText;
                        _RFID_Entry.FK_Dist_ID = _DistAuto.ID;
                    }
                    if (_LetterAuto != null)
                    {
                        _RFID_Entry.LetterText_Auto = _LetterAuto.LetterText;
                        _RFID_Entry.FK_Letter_ID = _LetterAuto.ID;
                    }
                    _RFID_Entry.LastEntryAt = DateTime.Now;

                    var _RFID_EntryLog = new RFID_EntryLog();
                    _RFID_EntryLog.TotalText_Raw = TotalText_Raw;
                    _RFID_EntryLog.FK_RFID_Entry = _RFID_Entry.ID;
                    _RFID_EntryLog.EntryLogedAt = _RFID_Entry.LastEntryAt;

                    if (GateID == "1")
                    {
                        _RFID_EntryLog.GateID = GateID;
                        _RFID_Entry.GateInID = GateID;
                        _RFID_Entry.GateInDateTime = _RFID_Entry.LastEntryAt;
                    }
                    else if (GateID == "2")
                    {
                        _RFID_EntryLog.GateID = GateID;
                        _RFID_Entry.GateOutID = GateID;
                        _RFID_Entry.GateOutDateTime = _RFID_Entry.LastEntryAt;
                    }
                    bll.db.RFID_EntryLog.Add(_RFID_EntryLog);
                    bll.db.SaveChanges();
                }
                else
                {
                    _RFID_Entry = new RFID_Entry();

                    _RFID_Entry.Status = 0;
                    var vehicles = bll.db.Vehicles.Where(m => m.RegistrationNumber.Contains(_LastText_Raw)).Select(m => m.RegistrationNumber).ToList();
                    if (vehicles.Count() == 1)
                    {
                        _RFID_Entry.TotalText_API = vehicles.FirstOrDefault();
                        var _SplittedTotalText_API = vehicles.FirstOrDefault().Split('-');
                        _RFID_Entry.DistText_API = _SplittedTotalText_API[0];
                        _RFID_Entry.LetterText_API = _SplittedTotalText_API[1];
                    }
                    else if(vehicles.Count() > 1)
                    {
                        var _TotalTextMutiple_API = "";
                        foreach (var item in vehicles)
                        {
                            _TotalTextMutiple_API = _TotalTextMutiple_API + item + ", ";
                        }
                        _RFID_Entry.TotalTextMutiple_API = _TotalTextMutiple_API;
                    }

                    _RFID_Entry.TotalText_Raw = TotalText_Raw;
                    if (_DistAuto != null)
                    {
                        _RFID_Entry.DistText_Auto = _DistAuto.DistText;
                        _RFID_Entry.FK_Dist_ID = _DistAuto.ID;
                    }
                    if (_LetterAuto != null)
                    {
                        _RFID_Entry.LetterText_Auto = _LetterAuto.LetterText;
                        _RFID_Entry.FK_Letter_ID = _LetterAuto.ID;
                    }
                    _RFID_Entry.LastEntryAt = DateTime.Now;
                    bll.db.RFID_Entry.Add(_RFID_Entry);
                    bll.db.SaveChanges();

                    var _RFID_EntryLog = new RFID_EntryLog();
                    _RFID_EntryLog.TotalText_Raw = TotalText_Raw;
                    _RFID_EntryLog.FK_RFID_Entry = _RFID_Entry.ID;
                    _RFID_EntryLog.GateID = GateID;
                    _RFID_EntryLog.EntryLogedAt = _RFID_Entry.LastEntryAt;

                    if (GateID == "1")
                    {
                        _RFID_EntryLog.GateID = GateID;
                        _RFID_Entry.GateInID = GateID;
                        _RFID_Entry.GateInDateTime = _RFID_Entry.LastEntryAt;
                    }
                    else if (GateID == "2")
                    {
                        _RFID_EntryLog.GateID = GateID;
                        _RFID_Entry.GateOutID = GateID;
                        _RFID_Entry.GateOutDateTime = _RFID_Entry.LastEntryAt;
                    }
                    bll.db.RFID_EntryLog.Add(_RFID_EntryLog);
                    bll.db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                int d = 1;
            }
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindVehicle(string RowText)
        {
            var response = "";
            try
            {
                var LastText = "";
                var RowTexts = RowText.Split('-');
                if (RowTexts[4].Count() == 3)
                {
                    LastText = RowTexts[3] + "-0" + RowTexts[4];
                }
                else
                {
                    LastText = RowTexts[3] + "-" + RowTexts[4];
                }
                var vehicles = bll.db.Vehicles.Where(m => m.RegistrationNumber.Contains(LastText)).Select(m => m.RegistrationNumber).ToList();
                if (vehicles.Count() == 1)
                {
                    response = vehicles.FirstOrDefault();
                }
                else
                {
                    response = "0";
                }
            }
            catch (Exception e)
            {
                response = "Error in API";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}