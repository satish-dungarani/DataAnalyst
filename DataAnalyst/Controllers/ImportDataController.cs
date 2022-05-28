using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAnalyst.Models;
using DataAnalystDA;
using DataAnalystDB;
using System.IO;

namespace DataAnalyst.Controllers
{
    public class ImportDataController : BaseController
    {
        clsImportData _ObjImportData = new clsImportData();
        clsProcessData _ObjProcessData = new clsProcessData();

        //
        // GET: /ImportData/
        public ActionResult Index()
        {
            return View();
        }

        //public JsonResult AjaxHandler()
        //{
        //    //List<sp_PharmacyMaster_SelectWhere_Result> _ObjPharmacyList;
        //    try
        //    {
        //        //_ObjPharmacyList = _ObjPharmacy.GetPharmacyDetailSelectWhere("");

        //        //var result = from s in _ObjPharmacyList
        //        //             select new[]
        //        //             {
        //        //                s.PharmacyName,s.Address1 + " " + s.Address2 + " " + s.City + " " + s.PostCode,
        //        //                s.RespPerson,s.Telephone,s.EmailID,Convert.ToString(s.IsActive),Convert.ToString(s.PharmacyID)
        //        //             };

        //        return Json(new
        //        {
        //            sEcho = "",
        //            iTotalRecords = "",
        //            iTotalDisplayRecords = "",
        //            aaData = ""
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        [HttpPost]
        public ActionResult SaveImport(ImportDataModel _ObjModel, string FullPath)
        {
            List<ImportDataModel> _ObjImportList = new List<ImportDataModel>();
            try
            {
                _ObjImportData.DeleteImportData(_ObjModel.DataYear, _ObjModel.DataMonth);
                _ObjProcessData.DeleteProcessData(_ObjModel.DataYear, _ObjModel.DataMonth);

                _ObjImportList = _ObjImportData.SaveImportData(FullPath, _ObjModel.DataYear, _ObjModel.DataMonth, clsCommonUI._User, clsCommonUI._Terminal).Select(x => new ImportDataModel
                {
                    ImportId = x.ImportId,
                    DataYear = x.DataYear,
                    DataMonth = x.DataMonth,
                    PharmacyName = x.PharmacyName,
                    AccountNo = x.AccountNo,
                    Generic = x.Generic,
                    EthicalPI = x.EthicalPI,
                    SurgicalDressing = x.SurgicalDressing,
                    NonGeneric = x.NonGeneric,
                    NonPrescription = x.NonGeneric,
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
                    TotalSpend = x.TotalSpend,
                    TotalRebate = x.TotalRebate
                }).ToList();

                System.IO.File.Delete(FullPath);

                return PartialView("_ImportDataListPartial", _ObjImportList);
            }
            catch (Exception ex)
            {
                //throw ex;
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }

        public PartialViewResult GetImportList(string DataYear, string DataMonth)
        {
            List<ImportDataModel> _ObjImportList = new List<ImportDataModel>();
            try
            {
                _ObjImportList = _ObjImportData.GetImportDataLsit(" and DataYear = '" + DataYear + "' and DataMonth = '" + DataMonth + "'").Select(x => new ImportDataModel
                {
                    ImportId = x.ImportId,
                    DataYear = x.DataYear,
                    DataMonth = x.DataMonth,
                    PharmacyName = x.PharmacyName,
                    AccountNo = x.AccountNo,
                    Generic = x.Generic,
                    EthicalPI = x.EthicalPI,
                    SurgicalDressing = x.SurgicalDressing,
                    NonGeneric = x.NonGeneric,
                    NonPrescription = x.NonGeneric,
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
                    TotalSpend = x.TotalSpend,
                    TotalRebate = x.TotalRebate
                }).ToList();

                return PartialView("_ImportDataListPartial", _ObjImportList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult UpLoadFile()
        {
            try
            {
                bool Result = false;
                string _FullPath = "";
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase FilePath = null;
                    FilePath = Request.Files[0];
                    if (FilePath.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && FilePath.ContentType != "application/vnd.ms-excel")
                        throw new Exception("Please select proper file.");
                    if (FilePath != null && FilePath.ContentLength > 0)
                    {
                        string _Path = Server.MapPath("~/Files");
                        if (!Directory.Exists(_Path))
                        {
                            Directory.CreateDirectory(_Path);
                        }
                        _FullPath = Path.Combine(_Path, FilePath.FileName);
                        FilePath.SaveAs(_FullPath);
                        Result = true;
                    }
                }
                return Json(new { Result = Result, Msg = "Uploaded successfully.", FPath = _FullPath }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult SaveProcessData(string DataYear, string DataMonth, bool pDataExist)
        {
            clsProcessData _ObjProcess = new clsProcessData();
            bool _Result = false;
            try
            {
                //if (pDataExist)
                //    _ObjProcess.DeleteProcessData(DataYear, DataMonth);

                _Result = _ObjImportData.SaveProcessData(DataYear, DataMonth, clsCommonUI._User, clsCommonUI._Terminal);
                if (!_Result)
                    throw new Exception("Process fail. some server error occure.");

                return GetImportList(DataYear, DataMonth);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PartialViewResult DeleteImportData(string pDataYear, string pDataMonth)
        {
            bool _RetResult = false;
            try
            {
                _RetResult = _ObjImportData.DeleteImportData(pDataYear, pDataMonth);
                return GetImportList(pDataYear, pDataMonth);
                //return Json(new { Result =  _RetResult });
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult DeleteProcessData(string DataYear, string DataMonth)
        {
            clsProcessData _ObjProcess = new clsProcessData();
            try
            {
                _ObjProcess.DeleteProcessData(DataYear, DataMonth);
                return Json(new { Rslt = true, Msg = "Process Data Successfully Deleted!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }

        public JsonResult ProcessDataExiest(string pDataYear, string pDataMonth)
        {
            clsProcessData _ObjProcess = new clsProcessData();
            bool _Retrslt = false;
            try
            {
                if(_ObjProcess.GetProcessDataSelectWhere(" and DataYear = '" + pDataYear + "' and DataMonth = '" + pDataMonth + "'").ToList().Count > 0)
                    _Retrslt = true;
                return Json(_Retrslt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public JsonResult ImportDataExiest(string pDataYear, string pDataMonth)
        {
            clsImportData _ObjImport = new clsImportData();
            bool _Retrslt = false;
            try
            {
                if (_ObjImport.GetImportDataLsit(" and DataYear = '" + pDataYear + "' and DataMonth = '" + pDataMonth + "'").ToList().Count > 0)
                    _Retrslt = true;
                if (!_Retrslt)
                    ProcessDataExiest(pDataYear, pDataMonth);
                return Json(_Retrslt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}