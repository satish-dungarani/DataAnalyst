using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAnalystDA;
using DataAnalyst.Models;
using System.Data;

namespace DataAnalyst.Controllers
{
    public class ProcessController : BaseController
    {
        clsProcessData _ObjProcess = new clsProcessData();
        //
        // GET: /Process/
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public JsonResult GetSupplierName()
        //{
        //    //DateTime _StartDate,DateTime _EndDate
        //    try
        //    {
        //        var _SupplierList = new string[] { "" };
        //        //_SupplierList = _ObjProcess.GetProcessDataSupplierWise(_StartDate, _EndDate).Select(x => x.SupplierName).ToArray();
        //        _SupplierList = _ObjProcess.GetProcessDataSupplierWise(Convert.ToDateTime("2010/02/22"), DateTime.Parse("2011/02/22")).Select(x => x.SupplierName).ToArray();
        //        return Json(_SupplierList,JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public ActionResult SupplierSummaryRpt()
        {
            try
            {
                //get all pharmacy name
                ViewData["PharmacyList"] = clsCommonUI.PharmacyList(-1);

                //get all Supplier name
                ViewData["SupplierList"] = clsCommonUI.SupplierList(-1);

                //get Financial Year list
                ViewData["FinYearList"] = clsCommonUI.FinYearList();

                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult SupplierComparitionReport()
        {
            try
            {
                //get all pharmacy name
                ViewData["PharmacyList"] = clsCommonUI.PharmacyList(-1);

                //get all Supplier name
                ViewData["SupplierList"] = clsCommonUI.SupplierList(-1);

                //get Financial Year list
                ViewData["FinYearList"] = clsCommonUI.FinYearList();

                //get Duration Name list
                ViewData["DurationNameList"] = clsCommonUI.DurationNameList("M");

                return View("SupplierWisePharmacyYearlyComparitionRpt");
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public PartialViewResult ReloadDurationNameList(string _DurationType)
        {
            try
            {
                //get Duration Name list
                ViewData["DurationNameList"] = clsCommonUI.DurationNameList(_DurationType);

                return PartialView("_DurationNameListPartial");
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

                return PartialView("MultiplePharmacyPartial");
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
                //get list of Supplier name
                ViewData["SupplierList"] = clsCommonUI.SupplierList(pMember);

                return PartialView("MultipleSupplierPartial");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public PartialViewResult GetSupplierSummaryData(string DateRange, int pMemberType, string SupplierList, string PharmacyList, string pDuration, string pRptType)
        {
            DataTable _DT = new DataTable();
            try
            {
                if (pRptType == "C")
                {
                    DateRange = DateRange.Substring(0, 4) + "/04/01-" + DateRange.Substring(4, 4) + "/03/31";
                }

                if (PharmacyList == "")
                    PharmacyList = null;
                if (SupplierList == "")
                    SupplierList = null;
                _DT = _ObjProcess.GetSupplierWiseSummaryRpt(DateRange, pMemberType, SupplierList, PharmacyList, pDuration, pRptType);
                ViewBag.RptType = pRptType;
                return PartialView("_SupplierSummaryPartial", _DT);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public PartialViewResult GetSupplierComparitionData(string FinYearList, int pMemberType, string SupplierList, string PharmacyList, string pDurationType, string pDurationName)
        {
            DataTable _DT = new DataTable();
            try
            {
                
                if (PharmacyList == "")
                    PharmacyList = null;
                if (SupplierList == "")
                    SupplierList = null;
                _DT = _ObjProcess.GetSupplierComparitionData(FinYearList, pMemberType, SupplierList, PharmacyList, pDurationType, pDurationName);
                ViewBag.RptType = "C";
                return PartialView("_SupplierSummaryPartial", _DT);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}