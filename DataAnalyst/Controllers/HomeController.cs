using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAnalyst.Models;
using DataAnalystDA;
using DataAnalystDB;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAnalyst.Controllers
{
    public class HomeController : BaseController
    {
        clsSupplierMaster _ObjSuppmas = new clsSupplierMaster();
        clsPharmacyMaster _ObjPharma = new clsPharmacyMaster();
        clsProcessData _ObjProcess = new clsProcessData();

      
        public ActionResult Index()
        {
            //if (Session["UserName"] == null || Session == null)
            //    return RedirectToAction("Login", "Account");
            //else
            ViewBag.SupplierCount = _ObjSuppmas.GetSupplierDetail(-1).Count;
            ViewBag.DcMembers = _ObjPharma.GetPharmacyDetailSelectWhere(" and DCRegNo Is NOT NUll").Count;
            ViewBag.NonDcMembers = _ObjPharma.GetPharmacyDetailSelectWhere(" and DCRegNo Is NUll").Count;
            ViewBag.OverAll = ViewBag.DcMembers + ViewBag.NonDcMembers;

            ViewBag.DcSpend = Convert.ToInt32(_ObjProcess.GetProcessDataSelectWhere(" and refPharmacyID In (Select PharmacyID from PharmacyMas Where DCRegNo IS NOT NULL)").Sum(x => x.TotalSpend));
            ViewBag.NonDcSpend = Convert.ToInt32(_ObjProcess.GetProcessDataSelectWhere(" and refPharmacyID In (Select PharmacyID from PharmacyMas Where DCRegNo IS NULL)").Sum(x => x.TotalSpend));
            ViewBag.OverallSpend = (ViewBag.DcSpend + ViewBag.NonDcSpend).ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));
            ViewBag.DcSpend = ViewBag.DcSpend.ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));
            ViewBag.NonDcSpend = ViewBag.NonDcSpend.ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));

            ViewBag.DcRebatePie = Convert.ToInt32(_ObjProcess.GetProcessDataSelectWhere(" and refPharmacyID In (Select PharmacyID from PharmacyMas Where DCRegNo IS NOT NULL)").Sum(x => x.TotalRebate));
            ViewBag.NonDcRebatePie = Convert.ToInt32(_ObjProcess.GetProcessDataSelectWhere(" and refPharmacyID In (Select PharmacyID from PharmacyMas Where DCRegNo IS NULL)").Sum(x => x.TotalRebate));
            ViewBag.OverallRebate = (ViewBag.DcRebatePie + ViewBag.NonDcRebatePie).ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));
            ViewBag.DcRebate = ViewBag.DcRebatePie.ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));
            ViewBag.NonDcRebate = ViewBag.NonDcRebatePie.ToString("C0", CultureInfo.CreateSpecificCulture("en-GB"));

            return View();
        }

        public ActionResult PharmacyDashBoard()
        {
            try
            {
                //get list of pharmacy name
                ViewData["PharmacyList"] = clsCommonUI.PharmacyList(-1);

                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public PartialViewResult ReloadPharmacyList(int pMember)
        {
            try
            {
                //get list of pharmacy name
                ViewData["PharmacyList"] = clsCommonUI.PharmacyList(pMember);

                return PartialView("PharmacyListPartial");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public PartialViewResult ReloadSupplierList(int pMember)
        {
            try
            {
                //get list of supplier name
                ViewData["SupplierList"] = clsCommonUI.SupplierList(pMember);

                return PartialView("SupplierListPartial");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult SupplierDashBoard()
        {
            try
            {
                //get list of supplier name
                ViewData["SupplierList"] = clsCommonUI.SupplierList(-1);

                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult editSession(int? inRefMasterID)
        {
            Session["refMasterID"] = inRefMasterID;
            return Json(new { Saved = "Yes" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSupplierName(string DateRang, int pMemberType)
        {
            DateTime? _StartDate = null;
            DateTime? _EndDate = null;
            if (DateRang != null && DateRang != "")
            {
                _StartDate = Convert.ToDateTime(DateRang.Split('-')[0].Trim());
                _EndDate = Convert.ToDateTime(DateRang.Split('-')[1].Trim());
            }

            try
            {
                var _SupplierList = new string[] { "" };
                Nullable<decimal>[] _TotalSpend = new Nullable<decimal>[] { 0 };
                Nullable<decimal>[] _TotalRebate = new Nullable<decimal>[] { 0 };
                //string[] _TotalRebate = new string[] { "" };
                decimal[] _RebatePer = new decimal[] { 0 };
                _SupplierList = _ObjProcess.GetProcessDataSupplierWise(_StartDate, _EndDate, pMemberType).Select(x => x.SupplierName).ToArray();
                _TotalSpend = _ObjProcess.GetProcessDataSupplierWise(_StartDate, _EndDate, pMemberType).Select(x => x.TotalSpands).ToArray();
                _TotalRebate = _ObjProcess.GetProcessDataSupplierWise(_StartDate, _EndDate, pMemberType).Select(x => x.TotalRebates).ToArray();
                _RebatePer = _ObjProcess.GetProcessDataSupplierWise(_StartDate, _EndDate, pMemberType).Select(x => x.RebatePer).ToArray();
                return Json(new { Supplier = _SupplierList, Spend = _TotalSpend, Rebate = _TotalRebate, RebatePer = _RebatePer }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetPharmacyList(string DateRang, int pMemberType)
        {
            DateTime? _StartDate = null;
            DateTime? _EndDate = null;
            if (DateRang != null && DateRang != "")
            {
                _StartDate = Convert.ToDateTime(DateRang.Split('-')[0].Trim());
                _EndDate = Convert.ToDateTime(DateRang.Split('-')[1].Trim());
            }

            try
            {
                var _Pharmacy = new string[] { "" };
                Nullable<decimal>[] _TotalSpend = new Nullable<decimal>[] { 0 };
                Nullable<decimal>[] _TotalRebate = new Nullable<decimal>[] { 0 };
                decimal[] _RebatePer = new decimal[] { 0 };
                _Pharmacy = _ObjProcess.GetProcessDataPharmacyWise(_StartDate, _EndDate, pMemberType).Select(x => x.PharmacyName).ToArray();
                _TotalSpend = _ObjProcess.GetProcessDataPharmacyWise(_StartDate, _EndDate, pMemberType).Select(x => x.TotalSpands).ToArray();
                _TotalRebate = _ObjProcess.GetProcessDataPharmacyWise(_StartDate, _EndDate, pMemberType).Select(x => x.TotalRebates).ToArray();
                _RebatePer = _ObjProcess.GetProcessDataPharmacyWise(_StartDate, _EndDate, pMemberType).Select(x => x.RebatePer).ToArray();
                return Json(new { Pharmacy = _Pharmacy, Spend = _TotalSpend, Rebate = _TotalRebate, RebatePer = _RebatePer }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetMonthlySalesList(string DateRang, int pMemberType)
        {
            DateTime? _StartDate = null;
            DateTime? _EndDate = null;
            if (DateRang != null && DateRang != "")
            {
                _StartDate = Convert.ToDateTime(DateRang.Split('-')[0].Trim());
                _EndDate = Convert.ToDateTime(DateRang.Split('-')[1].Trim());
            }

            try
            {
                var _Months = new string[] { "" };
                Nullable<decimal>[] _TotalSpend = new Nullable<decimal>[] { 0 };
                Nullable<decimal>[] _TotalRebate = new Nullable<decimal>[] { 0 };
                decimal[] _RebatePer = new decimal[] { 0 };
                _Months = _ObjProcess.GetMonthlyData(_StartDate, _EndDate, pMemberType).Select(x => x.MonthYear).ToArray();
                _TotalSpend = _ObjProcess.GetMonthlyData(_StartDate, _EndDate, pMemberType).Select(x => x.TotalSpands).ToArray();
                _TotalRebate = _ObjProcess.GetMonthlyData(_StartDate, _EndDate, pMemberType).Select(x => x.TotalRebates).ToArray();
                _RebatePer = _ObjProcess.GetMonthlyData(_StartDate, _EndDate, pMemberType).Select(x => x.RebatePer).ToArray();
                return Json(new { Months = _Months, Spend = _TotalSpend, Rebate = _TotalRebate, RebatePer = _RebatePer }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetPharmacyPurchaseDetail(string DateRang, string pDuration, int? pPharmacyId)
        {
            DateTime? _StartDate = null;
            DateTime? _EndDate = null;
            if (DateRang != null && DateRang != "")
            {
                _StartDate = Convert.ToDateTime(DateRang.Split('-')[0].Trim());
                _EndDate = Convert.ToDateTime(DateRang.Split('-')[1].Trim());
            }

            try
            {
                var _Duration = new string[] { "" };
                Nullable<decimal>[] _TotalSpend = new Nullable<decimal>[] { 0 };
                Nullable<decimal>[] _TotalRebate = new Nullable<decimal>[] { 0 };
                decimal[] _RebatePer = new decimal[] { 0 };
                _Duration = _ObjProcess.GetMonthlyPharmacyPurchaseData(_StartDate, _EndDate, pDuration, pPharmacyId).Select(x => x.Duration).ToArray();
                _TotalSpend = _ObjProcess.GetMonthlyPharmacyPurchaseData(_StartDate, _EndDate, pDuration, pPharmacyId).Select(x => x.TotalSpands).ToArray();
                _TotalRebate = _ObjProcess.GetMonthlyPharmacyPurchaseData(_StartDate, _EndDate, pDuration, pPharmacyId).Select(x => x.TotalRebates).ToArray();
                _RebatePer = _ObjProcess.GetMonthlyPharmacyPurchaseData(_StartDate, _EndDate, pDuration, pPharmacyId).Select(x => x.RebatePer).ToArray();
                return Json(new { Duration = _Duration, Spend = _TotalSpend, Rebate = _TotalRebate, RebatePer = _RebatePer }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult GetPharmacyProcessData(string DateRang, int? pPharmacyId)
        {
            DateTime? _StartDate = null;
            DateTime? _EndDate = null;
            if (DateRang != null && DateRang != "")
            {
                _StartDate = Convert.ToDateTime(DateRang.Split('-')[0].Trim());
                _EndDate = Convert.ToDateTime(DateRang.Split('-')[1].Trim());
            }
            List<ProcessDataModel> _ObjProcessList = new List<ProcessDataModel>();
            try
            {
                _ObjProcessList = _ObjProcess.GetPharmacyProcessData(_StartDate, _EndDate, pPharmacyId).Select(x => new ProcessDataModel
                {
                    refSupplierId = x.SupplierId,
                    SupplierName = x.SupplierName,
                    Generic = x.Generic,
                    EthicalPI = x.EthicalPI,
                    SurgicalDressing = x.SurgicalDressing,
                    NonGeneric = x.NonGeneric,
                    NonPrescription = x.NonPrescription,
                    Insulin = x.Insulin,
                    Status = x.Status,
                    Electrical = x.Electrical,
                    Drinks = x.Drinks,
                    NonDiscount = x.NonDiscount,
                    OTC = x.OTC,
                    Mobility = x.Mobility,
                    Specials = x.Specials,
                    NP8 = x.NP8,
                    Other1 = x.Other1,
                    Other2 = x.Other2,
                    Other3 = x.Other3,
                    TotalSpend = Convert.ToDecimal(x.TotalSpend),
                    TotalRebate = Convert.ToDecimal(x.TotalRebate)
                }).ToList();

                return PartialView("_PharmacyProcessDataPartial", _ObjProcessList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetSupplierSalesDetail(string DateRang, int pMemberType, string pDuration, string pSupplierIdList)
        {
            DateTime? _StartDate = null;
            DateTime? _EndDate = null;
            if (DateRang != null && DateRang != "")
            {
                _StartDate = Convert.ToDateTime(DateRang.Split('-')[0].Trim());
                _EndDate = Convert.ToDateTime(DateRang.Split('-')[1].Trim());
            }

            try
            {
                if (pSupplierIdList == "")
                    pSupplierIdList = null;
                var _Duration = new string[] { "" };
                Nullable<decimal>[] _TotalSpend = new Nullable<decimal>[] { 0 };
                Nullable<decimal>[] _TotalRebate = new Nullable<decimal>[] { 0 };
                decimal[] _RebatePer = new decimal[] { 0 };
                _Duration = _ObjProcess.GetMonthlySupplierSalesData(_StartDate, _EndDate, pMemberType, pDuration, pSupplierIdList).Select(x => x.Duration).ToArray();
                _TotalSpend = _ObjProcess.GetMonthlySupplierSalesData(_StartDate, _EndDate, pMemberType, pDuration, pSupplierIdList).Select(x => x.TotalSpands).ToArray();
                _TotalRebate = _ObjProcess.GetMonthlySupplierSalesData(_StartDate, _EndDate, pMemberType, pDuration, pSupplierIdList).Select(x => x.TotalRebates).ToArray();
                _RebatePer = _ObjProcess.GetMonthlySupplierSalesData(_StartDate, _EndDate, pMemberType, pDuration, pSupplierIdList).Select(x => x.RebatePer).ToArray();
                return Json(new { Duration = _Duration, Spend = _TotalSpend, Rebate = _TotalRebate, RebatePer = _RebatePer }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult GetSupplierProcessData(string DateRang, int pMemberType, string pSupplierIdList)
        {
            DateTime? _StartDate = null;
            DateTime? _EndDate = null;
            if (DateRang != null && DateRang != "")
            {
                _StartDate = Convert.ToDateTime(DateRang.Split('-')[0].Trim());
                _EndDate = Convert.ToDateTime(DateRang.Split('-')[1].Trim());
            }
            List<ProcessDataModel> _ObjProcessList = new List<ProcessDataModel>();
            try
            {
                if (pSupplierIdList == "")
                    pSupplierIdList = null;
                _ObjProcessList = _ObjProcess.GetSupplierProcessData(_StartDate, _EndDate, pMemberType, pSupplierIdList).Select(x => new ProcessDataModel
                {
                    refPhatmacyId = x.PharmacyID,
                    PharmacyName = x.PharmacyName,
                    Generic = x.Generic,
                    EthicalPI = x.EthicalPI,
                    SurgicalDressing = x.SurgicalDressing,
                    NonGeneric = x.NonGeneric,
                    NonPrescription = x.NonPrescription,
                    Insulin = x.Insulin,
                    Status = x.Status,
                    Electrical = x.Electrical,
                    Drinks = x.Drinks,
                    NonDiscount = x.NonDiscount,
                    OTC = x.OTC,
                    Mobility = x.Mobility,
                    Specials = x.Specials,
                    NP8 = x.NP8,
                    Other1 = x.Other1,
                    Other2 = x.Other2,
                    Other3 = x.Other3,
                    TotalSpend = Convert.ToDecimal(x.TotalSpend),
                    TotalRebate = Convert.ToDecimal(x.TotalRebate)
                }).ToList();

                return PartialView("_SupplierProcessDataPartial", _ObjProcessList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult GetPharmacyPurchaseComparition(string DateRang, string pDurationType, int? pPharmacyId)
        {
            DateTime? _StartDate = null;
            DateTime? _EndDate = null;
            if (DateRang != null && DateRang != "")
            {
                _StartDate = Convert.ToDateTime(DateRang.Split('-')[0].Trim());
                _EndDate = Convert.ToDateTime(DateRang.Split('-')[1].Trim());
            }
            List<ProcessDataModel> _ObjProcessList = new List<ProcessDataModel>();
            try
            {

                _ObjProcessList = _ObjProcess.GetPharmacyPurchaseComparition(_StartDate, _EndDate, pDurationType, pPharmacyId).Select(x => new ProcessDataModel
                {
                    Duration = x.MonthYear,
                    ValueId = x.SupplierId,
                    ValueName = x.SupplierName,
                    TotalSpend = Convert.ToDecimal(x.TotalSpend),
                    TotalRebate = Convert.ToDecimal(x.TotalRebate)
                }).ToList();
                ViewBag.Type = "Supplier";
                return PartialView("_ComparisionPartial", _ObjProcessList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult GetSupplierSalesComparition(string DateRang, int pMemberType, string pDurationType, string pSupplierIdList)
        {
            DateTime? _StartDate = null;
            DateTime? _EndDate = null;
            if (DateRang != null && DateRang != "")
            {
                _StartDate = Convert.ToDateTime(DateRang.Split('-')[0].Trim());
                _EndDate = Convert.ToDateTime(DateRang.Split('-')[1].Trim());
            }
            List<ProcessDataModel> _ObjProcessList = new List<ProcessDataModel>();
            try
            {
                if (pSupplierIdList == "")
                    pSupplierIdList = null;
                _ObjProcessList = _ObjProcess.GetSupplierSalesComparition(_StartDate, _EndDate, pMemberType, pDurationType, pSupplierIdList).Select(x => new ProcessDataModel
                {
                    Duration = x.MonthYear,
                    ValueId = x.PharmacyID,
                    ValueName = x.PharmacyName,
                    TotalSpend = Convert.ToDecimal(x.TotalSpend),
                    TotalRebate = Convert.ToDecimal(x.TotalRebate)
                }).ToList();

                ViewBag.Type = "Pharmacy";
                return PartialView("_ComparisionPartial", _ObjProcessList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetSupplierSalesBaseOnPharmacy(string DateRang, int? pPharmacyId)
        {
            DateTime? _StartDate = null;
            DateTime? _EndDate = null;
            if (DateRang != null && DateRang != "")
            {
                _StartDate = Convert.ToDateTime(DateRang.Split('-')[0].Trim());
                _EndDate = Convert.ToDateTime(DateRang.Split('-')[1].Trim());
            }

            try
            {
                var _Supplier = new string[] { "" };
                Nullable<decimal>[] _TotalSpend = new Nullable<decimal>[] { 0 };
                Nullable<decimal>[] _TotalRebate = new Nullable<decimal>[] { 0 };
                decimal[] _RebatePer = new decimal[] { 0 };
                _Supplier = _ObjProcess.GetSupplierSalesBaseOnPharmacy(_StartDate, _EndDate, pPharmacyId).Select(x => x.SupplierName).ToArray();
                _TotalSpend = _ObjProcess.GetSupplierSalesBaseOnPharmacy(_StartDate, _EndDate, pPharmacyId).Select(x => x.TotalSpands).ToArray();
                _TotalRebate = _ObjProcess.GetSupplierSalesBaseOnPharmacy(_StartDate, _EndDate, pPharmacyId).Select(x => x.TotalRebates).ToArray();
                _RebatePer = _ObjProcess.GetSupplierSalesBaseOnPharmacy(_StartDate, _EndDate, pPharmacyId).Select(x => x.RebatePer).ToArray();
                return Json(new { Supplier = _Supplier, Spend = _TotalSpend, Rebate = _TotalRebate, RebatePer = _RebatePer }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetPharmacyPurchaseBaseOnSupplier(string DateRang, string pSupplierIdList, int pMemberType)
        {
            DateTime? _StartDate = null;
            DateTime? _EndDate = null;
            if (DateRang != null && DateRang != "")
            {
                _StartDate = Convert.ToDateTime(DateRang.Split('-')[0].Trim());
                _EndDate = Convert.ToDateTime(DateRang.Split('-')[1].Trim());
            }

            try
            {
                if (pSupplierIdList == "")
                    pSupplierIdList = null;
                var _Pharmacy = new string[] { "" };
                Nullable<decimal>[] _TotalSpend = new Nullable<decimal>[] { 0 };
                Nullable<decimal>[] _TotalRebate = new Nullable<decimal>[] { 0 };
                decimal[] _RebatePer = new decimal[] { 0 };
                _Pharmacy = _ObjProcess.GetPharmacyPurchaseBaseOnSupplier(_StartDate, _EndDate, pMemberType, pSupplierIdList).Select(x => x.PharmacyName).ToArray();
                _TotalSpend = _ObjProcess.GetPharmacyPurchaseBaseOnSupplier(_StartDate, _EndDate, pMemberType, pSupplierIdList).Select(x => x.TotalSpands).ToArray();
                _TotalRebate = _ObjProcess.GetPharmacyPurchaseBaseOnSupplier(_StartDate, _EndDate, pMemberType, pSupplierIdList).Select(x => x.TotalRebates).ToArray();
                _RebatePer = _ObjProcess.GetPharmacyPurchaseBaseOnSupplier(_StartDate, _EndDate, pMemberType, pSupplierIdList).Select(x => x.RebatePer).ToArray();
                return Json(new { Pharmacy = _Pharmacy, Spend = _TotalSpend, Rebate = _TotalRebate, RebatePer = _RebatePer }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpGet]
        //public JsonResult GetMultiSupllierMonthlySales(string DateRang, int pMemberType, string pSupplierIdList, string pDuration, int pSuppCunt)
        //{
        //    List<ProcessDataModel> _ObjProcessModel = new List<ProcessDataModel>();
        //    clsSupplierMaster _ObjSuppMas = new clsSupplierMaster();
        //    DateTime? _StartDate = null;
        //    DateTime? _EndDate = null;
        //    if (DateRang != null && DateRang != "")
        //    {
        //        _StartDate = Convert.ToDateTime(DateRang.Split('-')[0].Trim());
        //        _EndDate = Convert.ToDateTime(DateRang.Split('-')[1].Trim());
        //    }

        //    try
        //    {
        //        if (pSupplierIdList == "")
        //            pSupplierIdList = null;


        //        var _Pharmacy = new string[] { "" };
        //        Nullable<decimal>[] _TotalSpend = new Nullable<decimal>[] { 0 };
        //        Nullable<decimal>[] _TotalRebate = new Nullable<decimal>[] { 0 };
        //        string[] _Duration = new string[1];

        //        decimal[] _RebatePer = new decimal[] { 0 };
        //        _ObjProcessModel = _ObjProcess.GetSelectedSupplierMonthlySales(_StartDate, _EndDate, pMemberType, pDuration, pSupplierIdList).Select(x => new ProcessDataModel
        //        {
        //            Duration = x.Duration,
        //            YearMon = x.YearMon,
        //            refSupplierId = x.SupplierId,
        //            SupplierName = x.SupplierName,
        //            TotalSpend = Convert.ToDecimal(x.TotalSpands),
        //            TotalRebate = Convert.ToDecimal(x.TotalRebates),
        //            FinalSpend = Convert.ToDecimal(x.FinalSpands),
        //            FinalRebate = Convert.ToDecimal(x.FinalRebate)
        //        }).ToList();

        //        if (pSuppCunt == 0)
        //        {
        //            pSuppCunt = _ObjSuppMas.GetSupplierDetail(-1).Count;
        //            foreach (var _ObjSupId in _ObjSuppMas.GetSupplierDetail(-1).Select(x => x.SupplierId).ToList())
        //            {
        //                if (pSupplierIdList == null)
        //                    pSupplierIdList = "";
        //                else
        //                    pSupplierIdList = pSupplierIdList + ",";
        //                pSupplierIdList = pSupplierIdList + _ObjSupId;
        //            }
        //        }

        //        _Duration = _ObjProcessModel.Select(x => x.Duration).Distinct().ToArray();

        //        dynamic[] _MultiSupplierData = new dynamic[pSuppCunt + 1];
        //        dynamic[] _MultiSupplierRebateData = new dynamic[pSuppCunt + 1];


        //        for (int i = 0; i < pSuppCunt; i++)
        //        {
        //            var _SingleSupplierData = new Dictionary<string, dynamic>();
        //            var _SetPoundSign = new Dictionary<string, dynamic>();
        //            //for (int j = 0; j < 3; j++)
        //            //{
        //            //if (j == 0)
        //            _SetPoundSign["valuePrefix"] = "£";
        //            _SingleSupplierData["type"] = "column";
        //            //else if (j == 1)
        //            _SingleSupplierData["name"] = _ObjProcessModel.Where(x => x.refSupplierId == Convert.ToInt32(pSupplierIdList.Split(',')[i])).Select(x => x.SupplierName).FirstOrDefault();
        //            //else
        //            _SingleSupplierData["data"] = _ObjProcessModel.Where(y => y.refSupplierId == Convert.ToInt32(pSupplierIdList.Split(',')[i])).Select(y => y.TotalSpend).ToArray();
        //            _SingleSupplierData["tooltip"] = _SetPoundSign;
        //            //}
        //            if (_MultiSupplierData[i] == null)
        //                _MultiSupplierData[i] = _SingleSupplierData;

        //        }

        //        for (int i = 0; i < pSuppCunt; i++)
        //        {
        //            var _SingleSupplierRebateData = new Dictionary<string, dynamic>();
        //            var _SetPoundSign = new Dictionary<string, dynamic>();
        //            //for (int j = 0; j < 3; j++)
        //            //{
        //            //if (j == 0)
        //            _SetPoundSign["valuePrefix"] = "£";
        //            _SingleSupplierRebateData["type"] = "column";
        //            //else if (j == 1)
        //            _SingleSupplierRebateData["name"] = _ObjProcessModel.Where(x => x.refSupplierId == Convert.ToInt32(pSupplierIdList.Split(',')[i])).Select(x => x.SupplierName).FirstOrDefault();
        //            //else
        //            _SingleSupplierRebateData["data"] = _ObjProcessModel.Where(y => y.refSupplierId == Convert.ToInt32(pSupplierIdList.Split(',')[i])).Select(y => y.TotalRebate).ToArray();
        //            _SingleSupplierRebateData["tooltip"] = _SetPoundSign;
        //            //}

        //            _MultiSupplierRebateData[i] = _SingleSupplierRebateData;

        //        }

        //        decimal[] _FinalTotalSpend = new decimal[1];
        //        _FinalTotalSpend = (_ObjProcessModel.GroupBy(x => x.Duration).Select(x => x.First())).Select(x => x.FinalSpend).ToArray();
        //        decimal[] _FinalTotalRebate = new decimal[1];
        //        _FinalTotalRebate = (_ObjProcessModel.GroupBy(x => x.Duration).Select(x => x.First())).Select(x => x.FinalRebate).ToArray();

        //        var _Settooltip = new Dictionary<string, dynamic>();
        //        _Settooltip["valuePrefix"] = "£";

        //        var _TotalSpendStyle = new Dictionary<string, dynamic>();
        //        _TotalSpendStyle["lineWidth"] = 2;
        //        _TotalSpendStyle["lineColor"] = "Highcharts.getOptions().colors[3]";
        //        _TotalSpendStyle["fillColor"] = "Red";
        //        var _DurationTotalSpend = new Dictionary<string, dynamic>();
        //        _DurationTotalSpend["type"] = "spline";
        //        _DurationTotalSpend["name"] = "Total Spend";
        //        _DurationTotalSpend["yAxis"] = 1;
        //        _DurationTotalSpend["data"] = _FinalTotalSpend;
        //        _DurationTotalSpend["tooltip"] = _Settooltip;
        //        _DurationTotalSpend["marker"] = _TotalSpendStyle;
        //        var _DurationTotalRebate = new Dictionary<string, dynamic>();
        //        _DurationTotalRebate["type"] = "spline";
        //        _DurationTotalRebate["name"] = "Total Spend";
        //        _DurationTotalRebate["yAxis"] = 1;
        //        _DurationTotalRebate["data"] = _FinalTotalRebate;
        //        _DurationTotalRebate["tooltip"] = _Settooltip;
        //        _DurationTotalRebate["marker"] = _TotalSpendStyle;

        //        _MultiSupplierData[pSuppCunt] = _DurationTotalSpend;
        //        _MultiSupplierRebateData[pSuppCunt] = _DurationTotalRebate;

        //        return Json(new { Duration = _Duration, SupplierData = _MultiSupplierData, MultiSupplierRebateData = _MultiSupplierRebateData }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public JsonResult GetMultiSupllierMonthlySales(string DateRang, int pMemberType, string pSupplierIdList, string pDuration, int pSuppCunt)
        {
            List<ProcessDataModel> _ObjProcessModel = new List<ProcessDataModel>();
            clsSupplierMaster _ObjSuppMas = new clsSupplierMaster();
            DateTime? _StartDate = null;
            DateTime? _EndDate = null;
            if (DateRang != null && DateRang != "")
            {
                _StartDate = Convert.ToDateTime(DateRang.Split('-')[0].Trim());
                _EndDate = Convert.ToDateTime(DateRang.Split('-')[1].Trim());
            }

            try
            {
                if (pSupplierIdList == "")
                    pSupplierIdList = null;

                var _Pharmacy = new string[] { "" };
                Nullable<decimal>[] _TotalSpend = new Nullable<decimal>[] { 0 };
                Nullable<decimal>[] _TotalRebate = new Nullable<decimal>[] { 0 };
                string[] _Duration = new string[1];

                decimal[] _RebatePer = new decimal[] { 0 };
                _ObjProcessModel = _ObjProcess.GetSelectedSupplierMonthlySales(_StartDate, _EndDate, pMemberType, pDuration, pSupplierIdList).Select(x => new ProcessDataModel
                {
                    Duration = x.Duration,
                    YearMon = x.YearMon,
                    refSupplierId = x.SupplierId,
                    SupplierName = x.SupplierName,
                    TotalSpend = Convert.ToDecimal(x.TotalSpands),
                    TotalRebate = Convert.ToDecimal(x.TotalRebates),
                    FinalSpend = Convert.ToDecimal(x.FinalSpands),
                    FinalRebate = Convert.ToDecimal(x.FinalRebate)
                }).ToList();

                if (pSuppCunt == 0)
                {
                    pSuppCunt = _ObjSuppMas.GetSupplierDetail(-1).Count;
                    foreach (var _ObjSupId in _ObjSuppMas.GetSupplierDetail(-1).Select(x => x.SupplierId).ToList())
                    {
                        if (pSupplierIdList == null)
                            pSupplierIdList = "";
                        else
                            pSupplierIdList = pSupplierIdList + ",";
                        pSupplierIdList = pSupplierIdList + _ObjSupId;
                    }
                }

                _Duration = _ObjProcessModel.Select(x => x.Duration).Distinct().ToArray();

                dynamic[] _CombineSpendRebateData = new dynamic[(pSuppCunt * 2) + 2];
                dynamic[] _MultiSupplierData = new dynamic[pSuppCunt + 1];
                dynamic[] _MultiSupplierRebateData = new dynamic[pSuppCunt + 1];

                int _ArrayLen = 0;
                for (int i = 0; i < pSuppCunt; i++)
                {
                    var _SingleSupplierData = new Dictionary<string, dynamic>();
                    var _SingleSupplierRebateData = new Dictionary<string, dynamic>();
                    var _SetPoundSign = new Dictionary<string, dynamic>();
                    //for (int j = 0; j < 3; j++)
                    //{
                    //if (j == 0)
                    _SetPoundSign["valuePrefix"] = "£";
                    _SingleSupplierData["type"] = "column";
                    //else if (j == 1)
                    _SingleSupplierData["name"] = _ObjProcessModel.Where(x => x.refSupplierId == Convert.ToInt32(pSupplierIdList.Split(',')[i])).Select(x => x.SupplierName).FirstOrDefault() + " (Spend)";
                    //else
                    //_SingleSupplierData["yAxis"] = 2;
                    _SingleSupplierData["data"] = _ObjProcessModel.Where(y => y.refSupplierId == Convert.ToInt32(pSupplierIdList.Split(',')[i])).Select(y => y.TotalSpend).ToArray();
                    _SingleSupplierData["tooltip"] = _SetPoundSign;

                    //_SetPoundSign["valuePrefix"] = "£";
                    _SingleSupplierRebateData["type"] = "column";
                    //else if (j == 1)
                    _SingleSupplierRebateData["name"] = _ObjProcessModel.Where(x => x.refSupplierId == Convert.ToInt32(pSupplierIdList.Split(',')[i])).Select(x => x.SupplierName).FirstOrDefault() + " (Rebate)";
                    //else
                    _SingleSupplierRebateData["yAxis"] = 1;
                    _SingleSupplierRebateData["data"] = _ObjProcessModel.Where(y => y.refSupplierId == Convert.ToInt32(pSupplierIdList.Split(',')[i])).Select(y => y.TotalRebate).ToArray();
                    _SingleSupplierRebateData["tooltip"] = _SetPoundSign;
                    //}

                    //_MultiSupplierRebateData[i] = _SingleSupplierRebateData;
                    ////}

                    //if (_MultiSupplierData[i] == null)
                    //{
                    //    _MultiSupplierData[i] = _SingleSupplierData;
                    //    _MultiSupplierRebateData[i] = _SingleSupplierRebateData;
                    //}

                    _CombineSpendRebateData[_ArrayLen] = _SingleSupplierData;
                    _ArrayLen++;
                    _CombineSpendRebateData[_ArrayLen] = _SingleSupplierRebateData;
                    _ArrayLen++;
                }

                //for (int i = 0; i < pSuppCunt; i++)
                //{
                //    var _SingleSupplierRebateData = new Dictionary<string, dynamic>();
                //    var _SetPoundSign = new Dictionary<string, dynamic>();
                //    //for (int j = 0; j < 3; j++)
                //    //{
                //    //if (j == 0)
                //    _SetPoundSign["valuePrefix"] = "£";
                //    _SingleSupplierRebateData["type"] = "column";
                //    //else if (j == 1)
                //    _SingleSupplierRebateData["name"] = _ObjProcessModel.Where(x => x.refSupplierId == Convert.ToInt32(pSupplierIdList.Split(',')[i])).Select(x => x.SupplierName).FirstOrDefault();
                //    //else
                //    _SingleSupplierRebateData["data"] = _ObjProcessModel.Where(y => y.refSupplierId == Convert.ToInt32(pSupplierIdList.Split(',')[i])).Select(y => y.TotalRebate).ToArray();
                //    _SingleSupplierRebateData["tooltip"] = _SetPoundSign;
                //    //}

                //    _MultiSupplierRebateData[i] = _SingleSupplierRebateData;

                //}

                decimal[] _FinalTotalSpend = new decimal[1];
                _FinalTotalSpend = (_ObjProcessModel.GroupBy(x => x.Duration).Select(x => x.First())).Select(x => x.FinalSpend).ToArray();
                decimal[] _FinalTotalRebate = new decimal[1];
                _FinalTotalRebate = (_ObjProcessModel.GroupBy(x => x.Duration).Select(x => x.First())).Select(x => x.FinalRebate).ToArray();

                var _Settooltip = new Dictionary<string, dynamic>();
                _Settooltip["valuePrefix"] = "£";

                var _TotalSpendStyle = new Dictionary<string, dynamic>();
                _TotalSpendStyle["lineWidth"] = 2;
                _TotalSpendStyle["lineColor"] = "Highcharts.getOptions().colors[3]";
                _TotalSpendStyle["fillColor"] = "Red";
                var _DurationTotalSpend = new Dictionary<string, dynamic>();
                _DurationTotalSpend["type"] = "spline";
                _DurationTotalSpend["name"] = "Spend";
                //_DurationTotalSpend["yAxis"] = 1;
                _DurationTotalSpend["data"] = _FinalTotalSpend;
                _DurationTotalSpend["tooltip"] = _Settooltip;
                _DurationTotalSpend["marker"] = _TotalSpendStyle;
                var _DurationTotalRebate = new Dictionary<string, dynamic>();
                _DurationTotalRebate["type"] = "spline";
                _DurationTotalRebate["name"] = "Rebate";
                _DurationTotalRebate["yAxis"] = 1;
                _DurationTotalRebate["data"] = _FinalTotalRebate;
                _DurationTotalRebate["tooltip"] = _Settooltip;
                _DurationTotalRebate["marker"] = _TotalSpendStyle;

                //_MultiSupplierData[pSuppCunt] = _DurationTotalSpend;
                //_MultiSupplierRebateData[pSuppCunt] = _DurationTotalRebate;
                _CombineSpendRebateData[_ArrayLen] = _DurationTotalSpend;
                _ArrayLen++;
                _CombineSpendRebateData[_ArrayLen] = _DurationTotalRebate;


                return Json(new { Duration = _Duration, CombineData = _CombineSpendRebateData, SupplierData = _MultiSupplierData, MultiSupplierRebateData = _MultiSupplierRebateData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


       
    }
}