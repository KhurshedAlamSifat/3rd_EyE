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
//using _3rdEyE.ManagingTools;
using System.Threading;
using System.Threading.Tasks;
using static _3rdEyE.ManagingTools.CommonClass;
using System.IO;
using System.Web.Script.Serialization;
using _3rdEyE.ManagingTools;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Data.SqlClient;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;
namespace _3rdEyE.Controllers
{
    public class VehicleGateNewAPIController : BaseAPIController
    {
        static class InternalTripStatus
        {
            public const string Assigned = "Assigned";
            public const string EnteredStartingLocation = "Entered Starting Location";
            //public const string StartedLoading = "Started Loading";
            public const string Started = "Started";

            //public const string FinishedLoading = "Finished Loading";
            public const string StartedEmptyTrip = "Started Empty Trip";
            //public const string CreatedBill = "Created Bill";
            //public const string PaidBill = "Paid Bill";
            public const string LeftStartingLoaction = "Left Starting Loaction";
            public const string EnteredFinishingLocation = "Entered Finishing Location";
            //public const string StartedUnloading = "Started Unloading";

            //public const string FinishedUnloading = "Finished Unloading";
            //public const string FinishedEmptyTrip = "Finished Empty Trip";
            public const string Finished = "Finished";
        }
        static class ExternalTripStatus
        {
            public const string Assigned = "Assigned";
            public const string EnteredStartingLocation = "Entered Starting Location";
            public const string LeftStartingLoaction = "Left Starting Loaction";
            public const string EnteredFinishingLocation = "Entered Finishing Location";
            public const string LeftFinishingLocation = "Left Finishing Location";
        }
        List<string> LoadUnloadStatus = new List<string> { "লোডেড/Loaded", "এম্পটি/Empty" };

        public JsonResult GetCommon_Utilities_Version7()
        {
            var flag = "n/a";
            try
            {
                //VehicleInManualReasons
                var VehicleInManualReasons = db.PRG_Type.Where(m => m.IsDeleted != true && m.Show_VehicleGateInOutManual == true).Select(m => new
                {
                    m.PK_PRG_Type,
                    m.Title
                }).ToList();

                //DistrictNameListvar
                var DistrictNameList = new List<string>();
                DistrictNameList.Add("সিলেক্ট করুন");
                DistrictNameList.Add("ঢাকা মেট্রো/DHAKA METRO");
                DistrictNameList.Add("ঢাকা/DHAKA");
                DistrictNameList.Add("বাগেরহাট/BAGERHAT");
                DistrictNameList.Add("বান্দরবান/BANDARBAN");
                DistrictNameList.Add("বরগুনা/BARGUNA");
                DistrictNameList.Add("বরিশাল/BARISAL");
                DistrictNameList.Add("ভোলা/BHOLA");
                DistrictNameList.Add("বগুড়া/BOGRA");
                DistrictNameList.Add("ব্রাহ্মণবাড়িয়া/BRAHMANBARIA");
                DistrictNameList.Add("চাঁদপুর/CHANDPUR");
                DistrictNameList.Add("চাঁপাইনবাবগঞ্জ/CHAPAI");
                DistrictNameList.Add("চট্টগ্রাম মেট্রো/CHITTAGONG METRO");
                DistrictNameList.Add("চট্টগ্রাম/CHITTAGONG");
                DistrictNameList.Add("চুয়াডাঙ্গা/CHUADANGA");
                DistrictNameList.Add("কুমিল্লা/COMILLA");
                DistrictNameList.Add("কক্সবাজার/COXS BAZAR");

                DistrictNameList.Add("দিনাজপুর/DINAJPUR");

                DistrictNameList.Add("ফরিদপুর/FARIDPUR");
                DistrictNameList.Add("ফেনী/FENI");
                DistrictNameList.Add("গাইবান্ধা/GAIBANDHA");
                DistrictNameList.Add("গাজীপুর/GAZIPUR");
                DistrictNameList.Add("গোপালগঞ্জ/GOPALGANJ");
                DistrictNameList.Add("হবিগঞ্জ/HABIGANJ");
                DistrictNameList.Add("জামালপুর/JAMALPUR");
                DistrictNameList.Add("যশোর/JESSORE");
                DistrictNameList.Add("ঝালকাঠি/JHALOKATI");
                DistrictNameList.Add("ঝিনাইদহ/JHENAIDAH");
                DistrictNameList.Add("জয়পুরহাট/JOYPURHAT");
                DistrictNameList.Add("খাগড়াছড়ি/KHAGRACHHARI");
                DistrictNameList.Add("খুলনা মেট্রো/KHULNA METRO");
                DistrictNameList.Add("খুলনা/KHULNA");
                DistrictNameList.Add("কিশোরগঞ্জ/KISHOREGANJ");
                DistrictNameList.Add("কুড়িগ্রাম/KURIGRAM");
                DistrictNameList.Add("কুষ্টিয়া/KUSHTIA");
                DistrictNameList.Add("লক্ষ্মীপুর/LAKSHMIPUR");
                DistrictNameList.Add("লালমনিরহাট/LALMONIRHAT");
                DistrictNameList.Add("মাদারিপুর/MADARIPUR");
                DistrictNameList.Add("মাগুরা/MAGURA");
                DistrictNameList.Add("মানিকগঞ্জ/MANIKGANJ");
                DistrictNameList.Add("মেহেরপুর/MEHERPUR");
                DistrictNameList.Add("মৌলভীবাজার/MOULVIBAZAR");
                DistrictNameList.Add("মুন্সিগঞ্জ/MUNSHIGANJ");
                DistrictNameList.Add("ময়মনসিংহ/MYMENSINGH");
                DistrictNameList.Add("নওগাঁ/NAOGAON");
                DistrictNameList.Add("নড়াইল/NARAIL");
                DistrictNameList.Add("নারায়ণগঞ্জ/NARAYANGANJ");
                DistrictNameList.Add("নরসিংদি/NARSINGDI");
                DistrictNameList.Add("নাটোর/NATORE");
                DistrictNameList.Add("নেত্রকোনা/NETROKONA");
                DistrictNameList.Add("নীলফামারি/NILPHAMARI");
                DistrictNameList.Add("নোয়াখালি/NOAKHALI");
                DistrictNameList.Add("পাবনা/PABNA");
                DistrictNameList.Add("পঞ্চগড়/PANCHAGARH");
                DistrictNameList.Add("পটুয়াখালি/PATUAKHALI");
                DistrictNameList.Add("পিরোজপুর/PIROJPUR");
                DistrictNameList.Add("রাজবাড়ি/RAJBARI");
                DistrictNameList.Add("রাজশাহী মেট্রো/RAJSHAHI METRO");
                DistrictNameList.Add("রাজশাহী/RAJSHAHI");
                DistrictNameList.Add("রাঙামাটি/RANGAMATI");
                DistrictNameList.Add("রংপুর/RANGPUR");
                DistrictNameList.Add("সাতক্ষীরা/SATKHIRA");
                DistrictNameList.Add("শরিয়তপুর/SHARIATPUR");
                DistrictNameList.Add("শেরপুর/SHERPUR");
                DistrictNameList.Add("সিরাজগঞ্জ/SIRAJGANJ");
                DistrictNameList.Add("সুনামগঞ্জ/SUNAMGANJ");
                DistrictNameList.Add("বাগেরহাট/BAGERHAT");
                DistrictNameList.Add("সিলেট মেট্রো/SYLHET METRO");
                DistrictNameList.Add("সিলেট/SYLHET");
                DistrictNameList.Add("টাঙ্গাইল/TANGAIL");
                DistrictNameList.Add("ঠাকুরগাঁও/THAKURGAON");

                //AlphabetList
                var AlphabetList = new List<string>();
                AlphabetList.Add("সিলেক্ট করুন");
                AlphabetList.Add("এ/A");
                AlphabetList.Add("অ/AU");
                AlphabetList.Add("ব/BA");
                AlphabetList.Add("ভ/BHA");
                AlphabetList.Add("ছ/CAA");
                AlphabetList.Add("চ/CHA");
                AlphabetList.Add("ড/DA");
                AlphabetList.Add("দ/DAW");
                AlphabetList.Add("ঢ/DHA");
                AlphabetList.Add("ই/E");
                //  list2.Add("ফ/FA");
                AlphabetList.Add("গ/GA");
                AlphabetList.Add("ঘ/GHA");
                //  list2.Add("হ/HA");
                AlphabetList.Add("জ/JA");
                AlphabetList.Add("ঝ/JHA");
                AlphabetList.Add("ক/KA");
                AlphabetList.Add("খ/KHA");
                //  list2.Add("ল/LA");
                AlphabetList.Add("ম/MA");
                AlphabetList.Add("ন/NA");
                AlphabetList.Add("প/PA");
                AlphabetList.Add("র/RA");
                AlphabetList.Add("স/SA");
                AlphabetList.Add("শ/SHA");
                AlphabetList.Add("ট/TA");
                AlphabetList.Add("থ/TAW");
                AlphabetList.Add("ঠ/THA");
                AlphabetList.Add("ঊ/U");
                AlphabetList.Add("ঙ/WUA");
                AlphabetList.Add("য/ZA");
                flag = "found";
                return Json(new { flag, VehicleInManualReasons, DistrictNameList, AlphabetList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                flag = "internal_error";
                return Json(new { flag }, JsonRequestBehavior.AllowGet);
            }
        }
        //#Version9/+
        public JsonResult GetCommon_Utilities()
        {
            var flag = "n/a";
            try
            {
                //VehicleInManualReasons
                var VehicleInManualReasons = db.PRG_Type.Where(m => m.IsDeleted != true && m.Show_VehicleGateInOutManual == true).Select(m => new
                {
                    m.PK_PRG_Type,
                    m.Title
                }).ToList();

                //DistrictNameListvar
                var DistrictNameList = new List<string>();
                DistrictNameList.Add("সিলেক্ট করুন");
                DistrictNameList.Add("ঢাকা মেট্রো/DHAKA METRO");
                DistrictNameList.Add("ঢাকা/DHAKA");
                DistrictNameList.Add("বাগেরহাট/BAGERHAT");
                DistrictNameList.Add("বান্দরবান/BANDARBAN");
                DistrictNameList.Add("বরগুনা/BARGUNA");
                DistrictNameList.Add("বরিশাল/BARISAL");
                DistrictNameList.Add("ভোলা/BHOLA");
                DistrictNameList.Add("বগুড়া/BOGRA");
                DistrictNameList.Add("ব্রাহ্মণবাড়িয়া/BRAHMANBARIA");
                DistrictNameList.Add("চাঁদপুর/CHANDPUR");
                DistrictNameList.Add("চাঁপাইনবাবগঞ্জ/CHAPAI");
                DistrictNameList.Add("চট্টগ্রাম মেট্রো/CHITTAGONG METRO");
                DistrictNameList.Add("চট্টগ্রাম/CHITTAGONG");
                DistrictNameList.Add("চুয়াডাঙ্গা/CHUADANGA");
                DistrictNameList.Add("কুমিল্লা/COMILLA");
                DistrictNameList.Add("কক্সবাজার/COXS BAZAR");

                DistrictNameList.Add("দিনাজপুর/DINAJPUR");

                DistrictNameList.Add("ফরিদপুর/FARIDPUR");
                DistrictNameList.Add("ফেনী/FENI");
                DistrictNameList.Add("গাইবান্ধা/GAIBANDHA");
                DistrictNameList.Add("গাজীপুর/GAZIPUR");
                DistrictNameList.Add("গোপালগঞ্জ/GOPALGANJ");
                DistrictNameList.Add("হবিগঞ্জ/HABIGANJ");
                DistrictNameList.Add("জামালপুর/JAMALPUR");
                DistrictNameList.Add("যশোর/JESSORE");
                DistrictNameList.Add("ঝালকাঠি/JHALOKATI");
                DistrictNameList.Add("ঝিনাইদহ/JHENAIDAH");
                DistrictNameList.Add("জয়পুরহাট/JOYPURHAT");
                DistrictNameList.Add("খাগড়াছড়ি/KHAGRACHHARI");
                DistrictNameList.Add("খুলনা মেট্রো/KHULNA METRO");
                DistrictNameList.Add("খুলনা/KHULNA");
                DistrictNameList.Add("কিশোরগঞ্জ/KISHOREGANJ");
                DistrictNameList.Add("কুড়িগ্রাম/KURIGRAM");
                DistrictNameList.Add("কুষ্টিয়া/KUSHTIA");
                DistrictNameList.Add("লক্ষ্মীপুর/LAKSHMIPUR");
                DistrictNameList.Add("লালমনিরহাট/LALMONIRHAT");
                DistrictNameList.Add("মাদারিপুর/MADARIPUR");
                DistrictNameList.Add("মাগুরা/MAGURA");
                DistrictNameList.Add("মানিকগঞ্জ/MANIKGANJ");
                DistrictNameList.Add("মেহেরপুর/MEHERPUR");
                DistrictNameList.Add("মৌলভীবাজার/MOULVIBAZAR");
                DistrictNameList.Add("মুন্সিগঞ্জ/MUNSHIGANJ");
                DistrictNameList.Add("ময়মনসিংহ/MYMENSINGH");
                DistrictNameList.Add("নওগাঁ/NAOGAON");
                DistrictNameList.Add("নড়াইল/NARAIL");
                DistrictNameList.Add("নারায়ণগঞ্জ/NARAYANGANJ");
                DistrictNameList.Add("নরসিংদি/NARSINGDI");
                DistrictNameList.Add("নাটোর/NATORE");
                DistrictNameList.Add("নেত্রকোনা/NETROKONA");
                DistrictNameList.Add("নীলফামারি/NILPHAMARI");
                DistrictNameList.Add("নোয়াখালি/NOAKHALI");
                DistrictNameList.Add("পাবনা/PABNA");
                DistrictNameList.Add("পঞ্চগড়/PANCHAGARH");
                DistrictNameList.Add("পটুয়াখালি/PATUAKHALI");
                DistrictNameList.Add("পিরোজপুর/PIROJPUR");
                DistrictNameList.Add("রাজবাড়ি/RAJBARI");
                DistrictNameList.Add("রাজশাহী মেট্রো/RAJSHAHI METRO");
                DistrictNameList.Add("রাজশাহী/RAJSHAHI");
                DistrictNameList.Add("রাঙামাটি/RANGAMATI");
                DistrictNameList.Add("রংপুর/RANGPUR");
                DistrictNameList.Add("সাতক্ষীরা/SATKHIRA");
                DistrictNameList.Add("শরিয়তপুর/SHARIATPUR");
                DistrictNameList.Add("শেরপুর/SHERPUR");
                DistrictNameList.Add("সিরাজগঞ্জ/SIRAJGANJ");
                DistrictNameList.Add("সুনামগঞ্জ/SUNAMGANJ");
                DistrictNameList.Add("বাগেরহাট/BAGERHAT");
                DistrictNameList.Add("সিলেট মেট্রো/SYLHET METRO");
                DistrictNameList.Add("সিলেট/SYLHET");
                DistrictNameList.Add("টাঙ্গাইল/TANGAIL");
                DistrictNameList.Add("ঠাকুরগাঁও/THAKURGAON");

                //AlphabetList
                var AlphabetList = new List<string>();
                AlphabetList.Add("সিলেক্ট করুন");
                AlphabetList.Add("এ/A");
                AlphabetList.Add("অ/AU");
                AlphabetList.Add("ব/BA");
                AlphabetList.Add("ভ/BHA");
                AlphabetList.Add("ছ/CAA");
                AlphabetList.Add("চ/CHA");
                AlphabetList.Add("ড/DA");
                AlphabetList.Add("দ/DAW");
                AlphabetList.Add("ঢ/DHA");
                AlphabetList.Add("ই/E");
                //  list2.Add("ফ/FA");
                AlphabetList.Add("গ/GA");
                AlphabetList.Add("ঘ/GHA");
                //  list2.Add("হ/HA");
                AlphabetList.Add("জ/JA");
                AlphabetList.Add("ঝ/JHA");
                AlphabetList.Add("ক/KA");
                AlphabetList.Add("খ/KHA");
                //  list2.Add("ল/LA");
                AlphabetList.Add("ম/MA");
                AlphabetList.Add("ন/NA");
                AlphabetList.Add("প/PA");
                AlphabetList.Add("র/RA");
                AlphabetList.Add("স/SA");
                AlphabetList.Add("শ/SHA");
                AlphabetList.Add("ট/TA");
                AlphabetList.Add("থ/TAW");
                AlphabetList.Add("ঠ/THA");
                AlphabetList.Add("ঊ/U");
                AlphabetList.Add("ঙ/WUA");
                AlphabetList.Add("য/ZA");
                flag = "found";
                return Json(new { flag, VehicleInManualReasons, DistrictNameList, AlphabetList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                flag = "internal_error";
                return Json(new { flag }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCommon_Utilities_INDIA()
        {
            var flag = "n/a";
            try
            {
                //VehicleInManualReasons
                var VehicleInManualReasons = db.PRG_Type.Where(m => m.IsDeleted != true && m.Show_VehicleGateInOutManual == true).Select(m => new
                {
                    m.PK_PRG_Type,
                    m.Title
                }).ToList();

                //DistrictNameListvar
                var StateCodeList = new List<string>();

                StateCodeList.Add("AP");
                StateCodeList.Add("AR");
                StateCodeList.Add("AN");
                StateCodeList.Add("CG");
                StateCodeList.Add("DN");
                StateCodeList.Add("GA");
                StateCodeList.Add("HP");
                StateCodeList.Add("JK");
                StateCodeList.Add("KL");
                StateCodeList.Add("MP");
                StateCodeList.Add("ML");
                StateCodeList.Add("LD");
                StateCodeList.Add("NL");
                StateCodeList.Add("OD");
                StateCodeList.Add("SK");
                StateCodeList.Add("TN");
                StateCodeList.Add("UK");
                StateCodeList.Add("WB");
                StateCodeList.Add("AS");
                StateCodeList.Add("BR");
                StateCodeList.Add("CH");
                StateCodeList.Add("GJ");
                StateCodeList.Add("DD");
                StateCodeList.Add("HR");
                StateCodeList.Add("JH");
                StateCodeList.Add("KA");
                StateCodeList.Add("MH");
                StateCodeList.Add("MN");
                StateCodeList.Add("MZ");
                StateCodeList.Add("DL");
                StateCodeList.Add("PY");
                StateCodeList.Add("PB");
                StateCodeList.Add("RJ");
                StateCodeList.Add("TR");
                StateCodeList.Add("UP");
                StateCodeList.Add("TS");

                flag = "found";
                return Json(new { flag, VehicleInManualReasons, StateCodeList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                flag = "internal_error";
                return Json(new { flag }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult FindVehicle(string RegistrationNumber)
        {
            var flag = "n/a";
            try
            {
                var list = (db.Vehicles.Where(m => m.RegistrationNumber.Contains(RegistrationNumber) && (m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT")).Select(m => new { m.RegistrationNumber }))
                    .Union(db.DHT_RequisitionTrip.Where(m => m.RegistrationNumber.Contains(RegistrationNumber)).Select(m => new { m.RegistrationNumber }).Distinct())
                    .ToList();
                if (list.Count() > 0)
                {
                    flag = "found";
                    return Json(new { flag, list }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    flag = "not_found";
                    return Json(new { flag }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                flag = "internal_error";
                return Json(new { flag }, JsonRequestBehavior.AllowGet);
            }
        }

        string FormatRegistrationNumber(string RegistrationNumber)
        {
            //string RegistrationNumber = "বাগেরহাট/BAGERHAT Metro-ঠ/THA-52-0091";
            var v1 = RegistrationNumber.Split('-')[0].Split('/')[1];
            var v2 = RegistrationNumber.Split('-')[1].Split('/')[1];
            var v3 = RegistrationNumber.Split('-')[2];
            var v4 = RegistrationNumber.Split('-')[3];
            RegistrationNumber = v1 + "-" + v2 + "-" + v3 + "-" + v4;
            if (!RegistrationNumber.Contains("METRO"))
            {
                RegistrationNumber = RegistrationNumber.Replace(" ", "");
            }
            return RegistrationNumber;
        }
        public class GateInOutModel
        {
            public string AppVersionCode { get; set; }
            public string IsFormatedRegistrationNumber { get; set; } //"yes"/"no"
            public Nullable<bool> IsScannedEntry { get; set; } //"yes"/"no"
            public string RegistrationNumber { get; set; }
            public string FK_PRG_Type { get; set; }
            public string FK_CreatedByUser { get; set; }
            public string DeviceId { get; set; }
            public string FK_VehicleInOutManualReason { get; set; }
            public string LoadOrEmpty { get; set; }
            public string GPNumber { get; set; }// Fro Out Only
        }

        public JsonResult GetGateIn_Utilities()
        {
            var flag = "n/a";
            try
            {
                var _LoadUnloadStatus = LoadUnloadStatus.Select(m => new { Title = m }).ToList();

                var VehicleInManualReasons = db.VehicleInOutManualReasons.Where(m => m.IsDeleted != true && m.IsInReasson == true).Select(m => new
                {
                    m.PK_VehicleInOutManualReason,
                    m.TitleBangla
                }).ToList();
                flag = "found";
                return Json(new { flag, LoadUnloadStatus = _LoadUnloadStatus, VehicleInManualReasons }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                flag = "internal_error";
                return Json(new { flag }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GateIn(GateInOutModel gateInModel)
        {
            var _PK_User = Guid.Parse(gateInModel.FK_CreatedByUser);
            var gateUser = db.AppUsers.Where(m => m.PK_User == _PK_User).FirstOrDefault();

            //# >>Checking acceptable app version
            var AppVersionCode_Current = db.AppSettings.Where(m => m.Name == "AppVersionCode_Current" && m.IsActive == true).Select(m => m.Value).FirstOrDefault();
            var AppVersionCode_OldUsable = db.AppSettings.Where(m => m.Name == "AppVersionCode_OldUsable" && m.IsActive == true).Select(m => m.Value).ToList();

            var flag = "n/a";
            var message = "";
            var message_english = "";

            if (!(gateInModel.AppVersionCode == AppVersionCode_Current || AppVersionCode_OldUsable.Contains(gateInModel.AppVersionCode)))
            {
                flag = "appversion_not_usable";
                message = "বিফল হয়েছে। সফটয়ারটি আপডেট না করলে এন্ট্রী দেওয়া যাবে না। শীঘ্রই ট্রান্সপোর্ট ডিপার্ট্মেন্টকে জানান।";
                message_english = "Failed. Invalid App Version";
                return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
            }
            //# <<Checking acceptable app version
            try
            {
                gateInModel.RegistrationNumber = gateInModel.RegistrationNumber.ToUpper().Replace("CHITTAGONG", "CHATTA");

                //# >>Convert api request model to TemporaryVehicl
                TemporaryVehicle temporaryVehicle = new TemporaryVehicle();
                //model.DevelopersNote = "Test:VehicleGateNewAPI/GateIn-existing";
                temporaryVehicle.RegistrationNumber = gateInModel.IsFormatedRegistrationNumber == "yes" ? gateInModel.RegistrationNumber : FormatRegistrationNumber(gateInModel.RegistrationNumber);
                temporaryVehicle.FK_VehicleInOutManualReason = Convert.ToInt64(gateInModel.FK_VehicleInOutManualReason);
                temporaryVehicle.LoadOrEmpty = gateInModel.LoadOrEmpty.Split('/')[1];
                temporaryVehicle.IssueDateTime = DateTime.Now;
                temporaryVehicle.IsScannedEntry = gateInModel.IsFormatedRegistrationNumber == "yes" ? true : false;
                temporaryVehicle.FK_CreatedByLocationGate = Guid.Parse(gateInModel.FK_CreatedByUser);
                temporaryVehicle.DeviceId = gateInModel.DeviceId;
                //# <<Convert api request model to TemporaryVehicle

                //# >>Finding vehicle/TemporaryVehicle from DB
                var vehicle = db.Vehicles.Where(m => m.RegistrationNumber == temporaryVehicle.RegistrationNumber).FirstOrDefault();
                var db_temporaryVehicle = db.TemporaryVehicles.Where(m => m.RegistrationNumber == temporaryVehicle.RegistrationNumber).FirstOrDefault();
                temporaryVehicle.FK_Locaiton = gateUser.FK_Location;
                try
                {
                    if (!string.IsNullOrEmpty(gateInModel.FK_PRG_Type))
                    {
                        temporaryVehicle.FK_PRG_Type = Convert.ToInt64(gateInModel.FK_PRG_Type);
                    }
                }
                catch (Exception)
                { }
                //# <<Finding vehicle/TemporaryVehicle from DB

                //# If vehicle exists in 'Vehicle' table
                if (vehicle != null)
                {
                    //# check Vehicle is inside of any location
                    if (vehicle.LocationInOrOut == true)
                    {
                        flag = "vehicle_is_in_another_dipo";
                        message = "বিফল হয়েছে। " + vehicle.RegistrationNumber + " গাড়িটি " + db.Locations.Where(m => m.PK_Location == vehicle.FK_LocationInOut).FirstOrDefault().Name + " লোকেশনের ভিতরে রয়েছে।";
                        message_english = "Failed. '" + vehicle.RegistrationNumber + "' is inside '" + db.Locations.Where(m => m.PK_Location == vehicle.FK_LocationInOut).FirstOrDefault().Name + "'";
                        return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                    }

                    //# check Vehicle is assigned for Outside visit in last parking entry
                    if (gateUser.AppUserType == "Internal Gate Entry Device" && vehicle.LocationInOrOut == false && vehicle.FK_ParkingInOut_Last != null)
                    {
                        var lastParkingIn = db.ParkingInOuts.Where(m => m.PK_ParkingInOut == vehicle.FK_ParkingInOut_Last && m.FactoryIn_FK_CreatedByUser == null && m.BayAssign_FK_LocationBuilding != null).FirstOrDefault();
                        if (lastParkingIn != null)
                        {
                            var locationBuilding = db.LocationBuildings.Where(m => m.PK_LocationBuilding == lastParkingIn.BayAssign_FK_LocationBuilding).FirstOrDefault();
                            if (lastParkingIn.FK_Location == gateUser.FK_Location && locationBuilding.Name.Contains("Out Side"))
                            {
                                flag = "vehicle_assignd_for_outside";
                                message = "বিফল হয়েছে। গাড়িটি " + locationBuilding.Name + "-এ এসাইন করা হয়েছে। পুনরায় পার্কিং-এ প্রবেশ করতে হবে।";
                                message_english = "Failed. '" + vehicle.RegistrationNumber + "' is assigned for loading/unloading to '" + locationBuilding.Name + "'. Vehicle should re-enter to parking";
                                return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    //# After passing all check points
                    var response = GateIn_Existing(vehicle.PK_Vehicle, temporaryVehicle);
                    if (response == "OK")
                    {
                        flag = "success";
                        message = "প্রবেশ করার এন্ট্রি সফল হয়েছে।";
                        message_english = "Succeded. Get in '" + vehicle.RegistrationNumber + "'.";
                        return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        flag = "internal_error";
                        message = "বিফল হয়েছে। MIS ডিপার্ট্মেন্টকে জানান(থার্ড আই সার্ভার সমস্যা-২)। " + response;
                        message_english = "Failed. Server error-2. Please, Contact MIS.";
                        return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                    }
                }
                //# if vehicle neither exist in 'Vehicle' table nor in 'TemporaryVehicle' table
                else if (db_temporaryVehicle == null)
                {
                    temporaryVehicle.DevelopersNote = "VehicleGateNewAPI/GateIn/TemporaryVehicle-insert";
                    temporaryVehicle.FK_CreatedByLocationGate = gateUser.PK_User;
                    temporaryVehicle.StatusText = "Created";
                    temporaryVehicle.EntryCount = 1;//for future check how many times it entered in any location
                    db.TemporaryVehicles.Add(temporaryVehicle);
                    db.SaveChanges();
                    flag = "success";
                    message = "প্রবেশ করার এন্ট্রি সফল হয়েছে। শীঘ্রই ট্রান্সপোর্ট ডিপার্ট্মেন্টকে জানান।";
                    message_english = "Succeded. Please, Inform transport Dept ASAP to confirm '" + temporaryVehicle.RegistrationNumber + "'.";
                    return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                }
                //# If vehicle exists in 'TemporaryVehicle' table
                else
                {
                    db_temporaryVehicle.FK_VehicleInOutManualReason = temporaryVehicle.FK_VehicleInOutManualReason;
                    db_temporaryVehicle.LoadOrEmpty = temporaryVehicle.LoadOrEmpty;
                    db_temporaryVehicle.IssueDateTime = temporaryVehicle.IssueDateTime;
                    db_temporaryVehicle.IsScannedEntry = temporaryVehicle.IsScannedEntry;
                    db_temporaryVehicle.FK_Locaiton = temporaryVehicle.FK_Locaiton;
                    db_temporaryVehicle.FK_PRG_Type = temporaryVehicle.FK_PRG_Type;

                    temporaryVehicle.DevelopersNote = "VehicleGateNewAPI/GateIn/TemporaryVehicle-update";
                    db_temporaryVehicle.FK_CreatedByLocationGate = gateUser.PK_User;
                    db_temporaryVehicle.DeviceId = gateInModel.DeviceId;
                    db_temporaryVehicle.StatusText = "Updated";
                    db_temporaryVehicle.EntryCount = db_temporaryVehicle.EntryCount + 1;
                    db.SaveChanges();
                    flag = "success";
                    message = "প্রবেশ করার এন্ট্রি সফল হয়েছ। শীঘ্রই ট্রান্সপোর্ট ডিপার্ট্মেন্টকে জানান।";
                    message_english = "Succeded. Please, Inform transport Dept ASAP to confirm '" + temporaryVehicle.RegistrationNumber + "'.";
                    return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var error_message = e.Message;
                while (e.InnerException != null) { e = e.InnerException; error_message = e.Message; }
                db.AppErrorLogs.Add(
                    new AppErrorLog()
                    {
                        ErrorMessage = error_message,
                        ErrorTime = DateTime.Now,
                        UserDefinedMessage = "VehicleGateNewAPI/GateIn FK_AppUser:" + gateInModel.FK_CreatedByUser
                    }
                    );
                db.SaveChanges();

                flag = "internal_error";
                message = "বিফল হয়েছে। MIS ডিপার্ট্মেন্টকে জানান(থার্ড আই সার্ভার সমস্যা)।";
                message_english = "Failed. Server Error. Please, Contact MIS";
                return Json(new { flag, message, message_english, error_message }, JsonRequestBehavior.AllowGet);
            }
        }
        public string GateIn_Existing(Guid FK_Vehicle, TemporaryVehicle temporaryVehicle)
        {
            var gateUser = db.AppUsers.Where(m => m.PK_User == temporaryVehicle.FK_CreatedByLocationGate).FirstOrDefault();
            var vehicle = db.Vehicles.Where(m => m.PK_Vehicle == FK_Vehicle).FirstOrDefault();
            
            //# >>Execution of stored procedure
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = int.MaxValue;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)db.Database.Connection;
            string query = $"EXEC VehicleInOutManual_In " +
            $"@DevelopersNote = 'VehicleGateNewAPI/GateIn/GateIn_Existing_by_SP/VehicleInOutManual-In'," +
            $"@RegistrationNumber = '{temporaryVehicle.RegistrationNumber}'," +
            $"@FK_Vehicle = '{FK_Vehicle}'," +
            $"@FK_Location = '{temporaryVehicle.FK_Locaiton}'," +
            $"@FK_PRG_Type = '{temporaryVehicle.FK_PRG_Type}'," +
            $"@InOrOut = '1'," +
            $"@In_FK_CreatedByUser = '{temporaryVehicle.FK_CreatedByLocationGate}'," +
            $"@In_LoadOrEmpty = '{temporaryVehicle.LoadOrEmpty}'," +
            $"@In_FK_VehicleInOutManualReason = '{temporaryVehicle.FK_VehicleInOutManualReason}'," +
            $"@In_IssueDateTime = '{temporaryVehicle.IssueDateTime}'," +
            $"@In_IsScannedEntry = '{(temporaryVehicle.IsScannedEntry == true ? "1" : "0")}'," +
            $"@In_DeviceId = '{temporaryVehicle.DeviceId}';";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            adpt.Fill(dataTable);
            //# <<Execution of stored procedure

            //# For successfull entry in database
            if (dataTable.Rows[0]["return_status"].ToString() == "OK")
            {
                //# For Facotry/Depot Gate Entry
                if (gateUser.AppUserType == "Internal Gate Entry Device")
                {
                    try
                    {
                        //# Finish All Started Trip of this vehicle
                        new RequisitionController().RequisitionTrip_FinishMulti(vehicle.PK_Vehicle, temporaryVehicle.FK_Locaiton ?? new Guid(), temporaryVehicle.FK_CreatedByLocationGate ?? new Guid());
                    }
                    catch (Exception)
                    {
                    }
                }
                //# For Parking Gate Entry
                else if (gateUser.AppUserType == "Internal Parking Entry Device")
                {
                    var pK_ParkingInOut = Convert.ToInt64(dataTable.Rows[0]["PK_ParkingInOut"]);
                    ParkingInOut parkingInOut = db.ParkingInOuts.Where(m=>m.PK_ParkingInOut==pK_ParkingInOut).FirstOrDefault();
                    var pK_VehicleInOutManual = Convert.ToInt64(dataTable.Rows[0]["PK_VehicleInOutManual"]);

                    //# Link this (VehicleInOut + Parking) with RequisitionTrip (which are already started towards this destination location) as Finishing VehicleInOut / Parking
                    //# and vise varsa
                    var currentTripList = db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.FK_Vehicle == FK_Vehicle && m.Requisition.FK_Location_To == parkingInOut.FK_Location && m.StatusText == InternalTripStatus.Started).ToList();
                    if (currentTripList.Any())
                    {
                        foreach (var currentTrip in currentTripList)
                        {
                            currentTrip.FK_ParkingInOut_After = parkingInOut.PK_ParkingInOut;
                            currentTrip.FK_VehicleInOutManual_After = pK_VehicleInOutManual;
                        }
                        var parentCurrentTrip = currentTripList.Where(m => m.IsParent == true).FirstOrDefault();
                        if (currentTripList != null && parentCurrentTrip != null)
                        {
                            parkingInOut.FK_RequisitionTrip = parentCurrentTrip.PK_RequisitionTrip;
                            parkingInOut.FK_Requisition = parentCurrentTrip.FK_Requisition;
                            parkingInOut.IsBeforeOrAfter = false;
                        }
                        else
                        {
                            var individualCurrentTrip = currentTripList.Where(m => m.IsParent != true && m.FK_RequisitionTrip_Parent == null).FirstOrDefault();
                            if (individualCurrentTrip != null)
                            {
                                parkingInOut.FK_RequisitionTrip = individualCurrentTrip.PK_RequisitionTrip;
                                parkingInOut.FK_Requisition = individualCurrentTrip.FK_Requisition;
                                parkingInOut.IsBeforeOrAfter = false;
                            }
                        }
                    }
                    else
                    {
                        var pendingTripList = db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.FK_Vehicle == FK_Vehicle && m.Requisition.FK_Location_From == parkingInOut.FK_Location && m.StatusText == InternalTripStatus.Assigned).ToList();
                        if (pendingTripList.Any())
                        {
                            foreach (var pendingTrip in pendingTripList)
                            {
                                pendingTrip.FK_ParkingInOut_Before = parkingInOut.PK_ParkingInOut;
                                pendingTrip.FK_VehicleInOutManual_Before = pK_VehicleInOutManual;
                            }
                            var parentPendingTrip = pendingTripList.Where(m => m.IsParent == true).FirstOrDefault();
                            if (parentPendingTrip != null)
                            {
                                parkingInOut.FK_RequisitionTrip = parentPendingTrip.PK_RequisitionTrip;
                                parkingInOut.FK_Requisition = parentPendingTrip.FK_Requisition;
                                parkingInOut.IsBeforeOrAfter = true;
                            }
                            else
                            {
                                var individualPendingTrip = pendingTripList.Where(m => m.IsParent != true && m.FK_RequisitionTrip_Parent == null).FirstOrDefault();
                                if (individualPendingTrip != null)
                                {
                                    parkingInOut.FK_RequisitionTrip = individualPendingTrip.PK_RequisitionTrip;
                                    parkingInOut.FK_Requisition = individualPendingTrip.FK_Requisition;
                                    parkingInOut.IsBeforeOrAfter = true;
                                }
                            }
                        }
                    }
                    db.SaveChanges();
                }
                return dataTable.Rows[0]["return_status"].ToString();
            }
            else
            {
                return dataTable.Rows[0]["return_status"].ToString() + "  " + dataTable.Rows[0]["return_message"].ToString();
            }
        }
        
        
        public JsonResult GetGateOut_Utilities()
        {
            var flag = "n/a";
            try
            {
                var _LoadUnloadStatus = LoadUnloadStatus.Select(m => new { Title = m }).ToList();

                var VehicleOutManualReasons = db.VehicleInOutManualReasons.Where(m => m.IsDeleted != true && m.IsOutReasson == true).Select(m => new
                {
                    m.PK_VehicleInOutManualReason,
                    m.TitleBangla
                }).ToList();
                flag = "found";
                return Json(new { flag, LoadUnloadStatus = _LoadUnloadStatus, VehicleOutManualReasons }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                flag = "internal_error";
                return Json(new { flag }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetGateOutManual_Utilities(Guid PK_User)
        {
            var flag = "n/a";
            try
            {
                var _LoadUnloadStatus = LoadUnloadStatus.Select(m => new { Title = m }).ToList();

                var VehicleOutManualReasons = db.VehicleInOutManualReasons.Where(m => m.IsDeleted != true && m.IsOutReasson == true).Select(m => new
                {
                    m.PK_VehicleInOutManualReason,
                    m.TitleBangla
                }).ToList();
                var gateUser = db.AppUsers.Where(m => m.PK_User == PK_User).FirstOrDefault();
                var InsideVehicleList = db.Vehicles.Where(m => m.LocationInOrOut == true && m.FK_LocationInOut == gateUser.FK_Location).Select(m => new
                {
                    m.RegistrationNumber,
                    Own_Hired = m.OWN_MHT_DHT == "OWN" ? "Own" : "Hired",
                    LastLoadingStatus = m.LocationInOut_Load_Unload_Workshop
                }).ToList();
                flag = "found";
                return Json(new { flag, LoadUnloadStatus = _LoadUnloadStatus, VehicleOutManualReasons, InsideVehicleList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                flag = "internal_error";
                return Json(new { flag }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CheckOcData_Test(Guid? PK_User, string RegistrationNumber = "")
        {
            var GpNumber = "GP52520016084455";// db.RequisitionTrips.Where(m => m.Requisition.FK_Location_From == gateUser.FK_Location && m.FK_Vehicle == vehicle.PK_Vehicle && m.OracleDB_GPNumber != null).Select(m => m.OracleDB_GPNumber).FirstOrDefault();
            if (string.IsNullOrEmpty(GpNumber))
            {
                return Json(new { Status = "not_found", Message = "No unused GP found" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var res = "";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://runner.prangroup.com:4005/api/dgx/");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                httpWebRequest.Headers["ss"] = "DGTX";
                httpWebRequest.Headers["yy"] = "HJDyh876Yhd765JHdgeoOUE765487sf543GDJksn";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        dgno = GpNumber
                    });
                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    res = streamReader.ReadToEnd();
                }
                var GpDetailList = JsonConvert.DeserializeObject<GpDetail>(res);

                if (GpDetailList.Status == "Y")
                {
                    foreach (var item in GpDetailList.dt)
                    {
                        item.OcMasterNo = item.ITEM_ID.Split('-')[0];
                        item.OcDetailNo = item.ITEM_ID.Split('-')[1];
                    }
                    var final_data = new
                    {
                        GpNumber,
                        OcData = GpDetailList.dt.GroupBy(m => m.OcMasterNo).OrderBy(m => m.Key).Select(m => new
                        {
                            OcMasterNo = m.Key,
                            OcDetailData = m.OrderBy(n => n.OcDetailNo).Select(n => new
                            {
                                n.OcDetailNo,
                                Item = n.ITEM_NAME,
                                Quantity = n.QTY + " " + n.UNIT,
                                n.NOTES
                            })
                        })
                    };
                    return Json(new { Status = "found", data = final_data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "not_found", Message = "GP Detail not found from remote." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = "server_error", Message = "Error occurred. Please, Contact MIS" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CheckOcData(Guid? PK_User, string RegistrationNumber = "")
        {
            //Get Gp no
            var gateUser = db.AppUsers.Where(m => m.PK_User == PK_User).FirstOrDefault();
            var vehicle = db.Vehicles.Where(m => m.IsDeleted != true && m.RegistrationNumber == RegistrationNumber).FirstOrDefault();
            if (vehicle == null)
            {
                return Json(new { Status = "not_found", Message = "No vehicle found with this number" }, JsonRequestBehavior.AllowGet);
            }

            var GpNumber = db.RequisitionTrips.Where(m => m.Requisition.FK_Location_From == gateUser.FK_Location && m.FK_Vehicle == vehicle.PK_Vehicle && m.OracleDB_GPNumber != null && m.OracleDB_GPNumberUpdatedAt != null && m.IsGatePassUsed != true).Select(m => m.OracleDB_GPNumber).FirstOrDefault();
            if (string.IsNullOrEmpty(GpNumber))
            {
                return Json(new { Status = "not_found", Message = "No unused GP found" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var res = "";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://runner.prangroup.com:4005/api/dgx/");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                httpWebRequest.Headers["ss"] = "DGTX";
                httpWebRequest.Headers["yy"] = "HJDyh876Yhd765JHdgeoOUE765487sf543GDJksn";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        dgno = GpNumber
                    });
                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    res = streamReader.ReadToEnd();
                }
                var GpDetailList = JsonConvert.DeserializeObject<GpDetail>(res);

                if (GpDetailList.Status == "Y")
                {
                    foreach (var item in GpDetailList.dt)
                    {
                        item.OcMasterNo = item.ITEM_ID.Split('-')[0];
                        item.OcDetailNo = item.ITEM_ID.Split('-')[1];
                    }
                    var final_data = new
                    {
                        GpNumber,
                        OcData = GpDetailList.dt.GroupBy(m => m.OcMasterNo).OrderBy(m => m.Key).Select(m => new
                        {
                            OcMasterNo = m.Key,
                            OcDetailData = m.OrderBy(n => n.OcDetailNo).Select(n => new
                            {
                                n.OcDetailNo,
                                Item = n.ITEM_NAME,
                                Quantity = n.QTY + " " + n.UNIT,
                                n.NOTES
                            })
                        })
                    };
                    return Json(new { Status = "found", data = final_data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = "not_found", Message = "GP Detail not found from remote." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = "server_error", Message = "Error occurred. Please, Contact MIS" }, JsonRequestBehavior.AllowGet);
            }
        }
        class GpDetail
        {
            public string Status { get; set; }
            public string Remarks { get; set; }
            public List<OcDetail> dt { get; set; }
        }
        class OcDetail
        {
            public string TRNM_ID { get; set; }
            public string ITEM_ID { get; set; }
            public string ITEM_NAME { get; set; }
            public decimal QTY { get; set; }
            public string UNIT { get; set; }
            public string NOTES { get; set; }
            public decimal LINE_NO { get; set; }

            public string OcMasterNo { get; set; }
            public string OcDetailNo { get; set; }
        }

        public JsonResult GateOut(GateInOutModel gateOutModel)
        {
            var _PK_User = Guid.Parse(gateOutModel.FK_CreatedByUser);
            var gateUser = db.AppUsers.Where(m => m.PK_User == _PK_User).FirstOrDefault();
            var RegistrationNumber = gateOutModel.IsFormatedRegistrationNumber == "yes" ? gateOutModel.RegistrationNumber : FormatRegistrationNumber(gateOutModel.RegistrationNumber);
            var vehicle = db.Vehicles.Where(m => m.RegistrationNumber == RegistrationNumber).FirstOrDefault();
            
            //# >>Checking acceptable app version
            var AppVersionCode_Current = db.AppSettings.Where(m => m.Name == "AppVersionCode_Current" && m.IsActive == true).Select(m => m.Value).FirstOrDefault();
            var AppVersionCode_OldUsable = db.AppSettings.Where(m => m.Name == "AppVersionCode_OldUsable" && m.IsActive == true).Select(m => m.Value).ToList();

            var flag = "n/a";
            var message = "";
            var message_english = "";

            if (!(gateOutModel.AppVersionCode == AppVersionCode_Current || AppVersionCode_OldUsable.Contains(gateOutModel.AppVersionCode)))
            {
                flag = "appversion_not_usable";
                message = "বিফল হয়েছে। সফটয়ারটি আপডেট না করলে এন্ট্রী দেওয়া যাবে না। শীঘ্রই ট্রান্সপোর্ট ডিপার্ট্মেন্টকে জানান।";
                message_english = "Failed. Invalid Verison Code";
                return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
            }
            //# <<Checking acceptable app version

            //# >>CUSOTM LOGIC: Restricted Parking gate for out entry from mobile
            var restrictedParkingGates = new List<string>() { "3501210", "3501208", "3501209", "1503501", };
            if (restrictedParkingGates.Contains(gateUser.UniqueIDNumber))
            {
                flag = "custom_logic";
                message = "বিফল হয়েছে। এই ইউজারের জন্য মোবাইল এপ দিয়ে গাড়ি বের করা নিষিদ্ধ। অনুগ্রহ করে, কম্পিউটার থেকে চেস্টা করুন।";
                message_english = "Failed. This user is forbidded to out vehicle from App/API";
                return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
            }
            //# <<CUSOTM LOGIC: Restricted Parking gate for out entry from mobile

            try
            {
                gateOutModel.RegistrationNumber = gateOutModel.RegistrationNumber.ToUpper().Replace("CHITTAGONG", "CHATTA");

                if (vehicle == null)
                {
                    var temppraryVehicle = db.TemporaryVehicles.Where(m => m.RegistrationNumber == RegistrationNumber).FirstOrDefault();
                    if (temppraryVehicle != null)
                    {
                        flag = "vehicle_not_confirmed";
                        message = "বিফল হয়েছে। শীঘ্রই ট্রান্সপোর্ট ডিপার্ট্মেন্টকে গাড়ির তথ্য হালনাগাদ করতে বলুন।";
                        message_english = "Failed. Please, Inform Transport Dept to confirm this vehcile.";
                        return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        flag = "vehicle_not_found";
                        message = "বিফল হয়েছে। গাড়ি খুজে পাওয়া যায়নি।";
                        message_english = "Failed. '" + temppraryVehicle.RegistrationNumber + "' not found.";
                        return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                    }
                }

                //# >> check Vehicle Out of all location
                if (vehicle.LocationInOrOut == null || vehicle.LocationInOrOut == false)
                {
                    flag = "vehicle_not_in_this_dipo";
                    message = "বিফল হয়েছে। গাড়ি এই লোকেশনের বাহিরে।";
                    message = "Failed. '" + vehicle.RegistrationNumber + "' is in outside of this location";
                    return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                }
                //# << check Vehicle Out of all location
                //# >> check Vehicle inside another location
                else if (vehicle.LocationInOrOut == true && vehicle.FK_LocationInOut != gateUser.FK_Location)
                {
                    flag = "vehicle_is_in_another_dipo";
                    message = "বিফল হয়েছে। " + vehicle.RegistrationNumber + " গাড়িটি " + db.Locations.Where(m => m.PK_Location == vehicle.FK_LocationInOut).FirstOrDefault().Name + " লোকেশনের ভিতরে রয়েছে।";
                    message_english = "Failed. '" + vehicle.RegistrationNumber + "' is in '" + db.Locations.Where(m => m.PK_Location == vehicle.FK_LocationInOut).FirstOrDefault().Name + "'.";
                    return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                }
                //# << check Vehicle inside another location

                //# Vehicle gate out from similar type gate
                if (vehicle.LocationInOrOut == true && vehicle.FK_VehicleInOutManual_Last != null)
                {
                    var lastInGate = db.VehicleInOutManuals.Where(m => m.PK_VehicleInOutManual == vehicle.FK_VehicleInOutManual_Last).Select(m => m.AppUser).FirstOrDefault();
                    if (lastInGate.AppUserType == "Internal Parking Entry Device" && lastInGate.PK_User != gateUser.PK_User)
                    {
                        flag = "vehicle_try_praking_out_try_from_nonparking";
                        message = "বিফল হয়েছে। গাড়িটি " + lastInGate.FullName + " থেকে বের হতে হবে।";
                        message_english = "Failed. '" + vehicle.RegistrationNumber + "' need to get out from '" + lastInGate.FullName + "'.";
                        return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                    }
                    else if (lastInGate.AppUserType == "Internal Gate Entry Device" && gateUser.AppUserType == "Internal Parking Entry Device")
                    {
                        flag = "vehicle_nonpraking_entry_try_from_parking";
                        message = "বিফল হয়েছে। গাড়িটি ফ্যাক্টরী/ডিপোট গেট দিয়ে বের হতে হবে। ";
                        message_english = "'" + vehicle.RegistrationNumber + "' need to gate out from any Factory/Depot.";
                        return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                    }
                }

                //# >>Custom Logic for Loading Bay Implementation
                if ((gateUser.Location.Name == "PIP" || gateUser.Location.Name == "DIP") && gateUser.AppUserType == "Internal Gate Entry Device")
                {
                    //# cehck vehcle used bay or not
                    if (vehicle.FK_ParkingInOut_Last != null)
                    {
                        var _parkingEntry = db.ParkingInOuts.Where(m => m.PK_ParkingInOut == vehicle.FK_ParkingInOut_Last).FirstOrDefault();
                        if (_parkingEntry != null && _parkingEntry.FK_Location == gateUser.FK_Location && _parkingEntry.BayAssign_IssueDateTime != null && _parkingEntry.FactoryIn_IssueDateTime != null && _parkingEntry.FK_LoadingBayUtilization == null)
                        {
                            var _building = db.LocationBuildings.Where(m => m.PK_LocationBuilding == _parkingEntry.BayAssign_FK_LocationBuilding).FirstOrDefault();
                            flag = "vehicle_did_not_use_bay";
                            message = "বিফল হয়েছে। " + _building.Name + " থেকে " + vehicle.RegistrationNumber + " গাড়িটি লোড/আনলোড শুরু করতে হবে।";
                            message_english = $"Failed. '{vehicle.RegistrationNumber}' should start Load/Unload from '{_building.Name}'.";
                            return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    //# cehck vehicle finish load/unload or not
                    if (vehicle.FK_LoadingBayUtilization_Last != null)
                    {
                        var loadingBayUtilization = db.LoadingBayUtilizations.Where(m => m.PK_LoadingBayUtilization == vehicle.FK_LoadingBayUtilization_Last).FirstOrDefault();
                        if (loadingBayUtilization != null && loadingBayUtilization.LoadingBay.LocationBuilding.FK_Location == gateUser.FK_Location)
                        {
                            flag = "vehicle_did_not_finish_bay_use";
                            message = "বিফল হয়েছে। " + loadingBayUtilization.LoadingBay.Name + " থেকে" + vehicle.RegistrationNumber + " গাড়িটির লোড/আনলোড শেষ করতে হবে।";
                            message_english = $"Failed. '{vehicle.RegistrationNumber}' should end Load/Unload from '{loadingBayUtilization.LoadingBay.Name}'.";
                            return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                //# <<Custom Logic for Loading Bay Implementation

                ////# >>GatePass Number checking
                //if (gateOutModel.GPNumber != "Not Given" && db.RequisitionTrips.Where(m => m.OracleDB_GPNumber == gateOutModel.GPNumber && m.IsGatePassUsed == true).Any())
                //{
                //    flag = "gp_alrady_used";
                //    message = "বিফল হয়েছে। জিপি নাম্বার: " + gateOutModel.GPNumber + " ইতিমধ্যে ব্যাবহার হয়েছে।";
                //    message_english = $"Failed. GP number: '{gateOutModel.GPNumber}' is already used.";
                //    return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                //}
                ////# <<GatePass Number checking

                //# >>Execution of stored procedure
                DataTable dataTable = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = int.MaxValue;
                SqlDataAdapter adpt = new SqlDataAdapter();
                cmd.Connection = (SqlConnection)db.Database.Connection;
                string query = $"EXEC VehicleInOutManual_Out " +
                $"@DevelopersNote = 'VehicleGateNewAPI/GateOut_by_SP/VehicleInOutManual-Out'," +
                $"@FK_Vehicle = '{vehicle.PK_Vehicle}'," +
                $"@InOrOut = '0'," +
                $"@Out_FK_CreatedByUser = '{gateOutModel.FK_CreatedByUser}'," +
                $"@Out_LoadOrEmpty = '{gateOutModel.LoadOrEmpty.Split('/')[1]}'," +
                $"@Out_FK_VehicleInOutManualReason = '{gateOutModel.FK_VehicleInOutManualReason}'," +
                $"@Out_IssueDateTime = '{DateTime.Now}'," +
                $"@Out_IsScannedEntry = '{gateOutModel.IsScannedEntry}'," +
                $"@GPNumber = '{gateOutModel.GPNumber}'," +
                $"@Out_DeviceId = '{gateOutModel.DeviceId}';";
                //$"@Out_DriverStaffId = '{StaffId}'," +
                //$"@Out_Note = '{}',";
                cmd.CommandText = query;
                adpt.SelectCommand = cmd;
                adpt.Fill(dataTable);
                //# <<Execution of stored procedure
                if (dataTable.Rows[0]["return_status"].ToString() == "OK")
                {
                    if (gateUser.AppUserType == "Internal Gate Entry Device")
                    {
                        try
                        {
                            new RequisitionController().RequisitionTrip_StartMulti(vehicle.PK_Vehicle, gateUser.FK_Location ?? new Guid(), gateUser.PK_User);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    flag = "success";
                    message = "বাহির হওয়ার এন্ট্রি সফল হয়েছে।";
                    message_english = $"Succeded'. {vehicle.RegistrationNumber}' is exite.";
                    return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    flag = "error";
                    message = "বিফল হয়েছে। " + dataTable.Rows[0]["return_status"].ToString() + "  " + dataTable.Rows[0]["return_mesage"].ToString();
                    message_english = "Failed. server error:" + dataTable.Rows[0]["return_status"].ToString();
                    return Json(new { flag, message, message_english }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var error_message = e.Message;
                while (e.InnerException != null) { e = e.InnerException; error_message = e.Message; }
                db.AppErrorLogs.Add(
                    new AppErrorLog()
                    {
                        ErrorMessage = error_message,
                        ErrorTime = DateTime.Now,
                        UserDefinedMessage = "VehicleGateNewAPI/GateIn FK_AppUser:" + gateOutModel.FK_CreatedByUser
                    }
                    );
                db.SaveChanges();
                flag = "internal_error";
                message = "বিফল হয়েছে। MIS ডিপার্ট্মেন্টকে জানান(থার্ড আই সার্ভার সমস্যা)।";
                message_english = $"Failed. Server error. Please contact to MIS";
                return Json(new { flag, message, message_english, error_message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ValidateGPNumber()
        {
            try
            {
                var res = "";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://172.17.107.254:808/api/dgx/");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                httpWebRequest.Headers["ss"] = "DGTX";
                httpWebRequest.Headers["yy"] = "HJDyh876Yhd765JHdgeoOUE765487sf543GDJksn";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        dgno = "12TOPIPPIPE0700378"
                    });

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    res = streamReader.ReadToEnd();
                }
                dynamic data_json = JObject.Parse(res);

                if (data_json.Status != null && data_json.Status == "Y")
                {
                    Debug.WriteLine("Valid GP");
                }

                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public string GetDisplayMessageForDotMatrixDisplay(string DeviceName)
        {

            var sampleMessages = new string[] {
            "[1]We're so glad to have you on board.[/1]",
            "[2]We're glad you're here and can't wait to get to know you better.[/2]",
            "[3]We're thrilled to have you as a customer and look forward to providing you with the best possible service.[/3]"
            };
            Random r = new Random();
            int i = r.Next(0, 3);
            return sampleMessages[i];

            try
            {
                var category = "Display Message For Gate";
                //var datetimeLowerLimit = DateTime.Now.AddMinutes(-2);
                var miscellaneousData = db.MiscellaneousDatas.Where(m => m.Category == category && m.Key_1 == DeviceName /*&& m.CreatedAt > datetimeLowerLimit*/).FirstOrDefault();
                if (miscellaneousData != null)
                {
                    return miscellaneousData.Data_1;
                }
                else
                {
                    return "-";
                }
            }
            catch (Exception e)
            {
                return "s__v_r  e_r_r";
            }
        }
        /// <summary>
        /// Request comes from SQL server 192.168.244.16, DB:DB_3rdEyE_ZKT_RFID, Table:acc_monitor_log, Insert Trigger:TR_acc_monitor_log_AfterInsert
        /// </summary>
        /// <param name="Data"></param>
        /// Data = DeviceName|Pin
        /// DeviceName = GateCode:IN/OUT 
        /// Examples of Data = 3501208:IN|1234
        /// <returns></returns>
        public string TryGateInOutFromAccessControll(string Data)
        {
            if (Data.Split('|').Count() != 2)
            {
                return $"Invalid Data:'{Data}'";
            }

            var DeviceName = Data.Split('|')[0];
            if (DeviceName.Split(':').Count() != 2 || (!DeviceName.EndsWith(":IN") && !DeviceName.EndsWith(":OUT")))
            {
                return $"Invalid DeviceName:'{DeviceName}'";
            }

            var Pin = Data.Split('|')[1];
            if (string.IsNullOrEmpty(Pin))
            {
                return $"Pin:'{Pin}'";
            }

            var miscellaneous = new Miscellaneou();
            miscellaneous.Category = "Test RFID IO";
            miscellaneous.CreatedAt = DateTime.Now;
            miscellaneous.Data_1 = DeviceName;
            miscellaneous.Data_2 = Pin;
            db.Miscellaneous.Add(miscellaneous);
            db.SaveChanges();
            return "";
            var displayMessage = "";



            if (DeviceName.EndsWith(":IN"))
            {
                //# Construct request model
                GateInOutModel gateInModel = new GateInOutModel();

                //gateInModel.AppVersionCode
                gateInModel.AppVersionCode = "N/A";
                //gateInModel.RegistrationNumber
                var vehicle = db.Vehicles.Where(m => m.IsDeleted == false && m.RFID_PIN == Pin).FirstOrDefault();
                if (vehicle == null)
                {
                    return "No vehicle found for RFID_PIN:" + Pin;
                }
                gateInModel.RegistrationNumber = vehicle.RegistrationNumber;
                //gateInModel.IsFormatedRegistrationNumber
                gateInModel.IsFormatedRegistrationNumber = "yes";
                //gateInModel.FK_VehicleInOutManualReason
                gateInModel.FK_VehicleInOutManualReason = "18";//VehicleInOutManualReason="N/A"
                //gateInModel.LoadOrEmpty
                gateInModel.LoadOrEmpty = "N.A/N.A";
                //gateInModel.IsScannedEntry
                gateInModel.IsScannedEntry = true;
                //gateInModel.FK_CreatedByUser
                var _UniqueIDNumber = DeviceName.Replace(":IN", "");
                var gateUser = db.AppUsers.Where(m => m.IsDeleted != true && m.IsActive == true && m.UniqueIDNumber == _UniqueIDNumber).FirstOrDefault();
                if (gateUser == null)
                {
                    return "Invalid Gate Code:" + _UniqueIDNumber;
                }
                gateInModel.FK_CreatedByUser = gateUser.PK_User.ToString();
                //gateInModel.DeviceId
                gateInModel.DeviceId = "RFID*";
                //FK_PRG_Type
                gateInModel.FK_PRG_Type = gateUser.PRG_Type == "PRAN" ? "1" : "2";
                var res = GateIn(gateInModel);
                string jsonString = JsonConvert.SerializeObject(res.Data);
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
                displayMessage = data["message_english"];
            }
            else if (DeviceName.EndsWith(":OUT"))
            {
                //# Construct request model
                GateInOutModel gateOutModel = new GateInOutModel();

                //gateOutModel.IsFormatedRegistrationNumber
                gateOutModel.IsFormatedRegistrationNumber = "yes";
                //gateOutModel.RegistrationNumber
                var vehicle = db.Vehicles.Where(m => m.IsDeleted == false && m.RFID_PIN == Pin).FirstOrDefault();
                if (vehicle == null)
                {
                    return "No vehicle found for RFID_PIN:" + Pin;
                }
                gateOutModel.RegistrationNumber = vehicle.RegistrationNumber;
                //gateOutModel.AppVersionCode
                gateOutModel.AppVersionCode = "N/A";
                //gateOutModel.GPNumber
                gateOutModel.GPNumber = "Not Given";
                //gateOutModel.FK_CreatedByUser
                var _UniqueIDNumber = DeviceName.Replace(":IN", "");
                var gateUser = db.AppUsers.Where(m => m.IsDeleted != true && m.IsActive == true && m.UniqueIDNumber == _UniqueIDNumber).FirstOrDefault();
                if (gateUser == null)
                {
                    return "Invalid Gate Code:" + _UniqueIDNumber;
                }
                gateOutModel.FK_CreatedByUser = gateUser.PK_User.ToString();
                //gateOutModel.LoadOrEmpty
                gateOutModel.LoadOrEmpty = "N/A";
                //gateOutModel.FK_VehicleInOutManualReason
                gateOutModel.FK_VehicleInOutManualReason = "18";//VehicleInOutManualReason="N/A"
                //gateOutModel.IsScannedEntry
                gateOutModel.IsScannedEntry = true;
                //gateOutModel.DeviceId
                gateOutModel.DeviceId = "RFID*";

                var res = GateOut(gateOutModel);
                string jsonString = JsonConvert.SerializeObject(res.Data);
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
                displayMessage = data["message_english"];
            }
            else
            {
                displayMessage = "NOT_REACHABLE";
            }
            var category = "Display Message For Gate";
            var miscellaneousData = db.MiscellaneousDatas.Where(m => m.Category == category && m.Key_1 == DeviceName).FirstOrDefault();
            if (miscellaneousData == null)
            {
                miscellaneousData = new MiscellaneousData();
                miscellaneousData.Category = category;
                miscellaneousData.CreatedAt = DateTime.Now;
                miscellaneousData.Key_1 = DeviceName;
                miscellaneousData.Data_1 = displayMessage;
                db.MiscellaneousDatas.Add(miscellaneousData);
            }
            else
            {
                miscellaneousData.Category = category;
                miscellaneousData.CreatedAt = DateTime.Now;
                miscellaneousData.Key_1 = DeviceName;
                miscellaneousData.Data_1 = displayMessage;
            }
            db.SaveChanges();
            return displayMessage;

        }

    }
}