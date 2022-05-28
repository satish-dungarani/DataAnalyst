using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAnalyst.Models;
using DataAnalystDA;
using DataAnalystDB;
using System.Data;

namespace DataAnalyst.Controllers
{
    public class PharmacyController : BaseController
    {
        clsPharmacyMaster _ObjPharmacy = new clsPharmacyMaster();
        //
        // GET: /Pharmacy/
        public ActionResult Index()
        {
            //DataTable _dt = new DataTable();
            //_dt = _ObjPharmacy.GetPharmacyDetailSelectWhereInDatatable(" Order By PharmacyName");
            //Dictionary<>
            return View();
        }
        public JsonResult EditSession(int? pId)
        {
            string _Message = string.Empty;
            try
            {
                if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "UPDATE", "PHARMACY")))
                {
                    Session["PharmacyId"] = pId;
                    return Json(new { Allow = "Yes" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _Message = "Update Rights not given!";
                    return Json(new { Allow = "No", _Message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Manage()
        {
            bool result = true;
            if (Session["PharmacyId"] == null)
            {
                result = Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "PHARMACY"));
            }
            if (result)
            {
                PharmacyMasterModel _ObjPharmacyMas = new PharmacyMasterModel();
                clsSupplierMaster _ObjSupp = new clsSupplierMaster();

                try
                {


                    int pID = 0;
                    if (Session["PharmacyId"] != null && !string.IsNullOrEmpty(Session["PharmacyId"].ToString()))
                        pID = Convert.ToInt32(Session["PharmacyId"].ToString());

                    _ObjPharmacyMas = _ObjPharmacy.GetPharmacyDetail(pID).Select(x => new PharmacyMasterModel
                    {

                        PharmacyId = x.PharmacyID,
                        PharmacyName = x.PharmacyName,
                        CompanyName = x.CompanyName,
                        Address1 = x.Address1,
                        Address2 = x.Address2,
                        City = x.City,
                        PostCode = x.PostCode,
                        RespPerson = x.RespPerson,
                        Telephone = x.Telephone,
                        FaxNo = x.Fax,
                        EmailID = x.EmailID,
                        DCRegNo = x.DCRegNo,
                        PharmacoRegNo = x.PharmacoRegNo,
                        IsActive = x.IsActive,
                        InsUser = x.InsUser,
                        InsDate = x.InsDate,
                        InsTerminal = x.InsTerminal,
                        UpdUser = x.UpdUser.HasValue ? x.UpdUser.Value : 0,
                        UpdDate = x.UpdDate,
                        UpdTerminal = x.UpdTerminal

                    }).FirstOrDefault();

                    if (_ObjPharmacyMas == null)
                    {
                        _ObjPharmacyMas = new PharmacyMasterModel();

                    }

                    _ObjPharmacyMas.CSAssociation = _ObjPharmacy.GetCSAssociation(pID).Select(x => new CustSuppAssociationModel
                    {
                        Id = x.ID,
                        refSupplierId = x.SupplierId,
                        SupplierName = x.SupplierName,
                        AccountNo = x.AccountNo
                    }).ToList();

                    Session["PharmacyId"] = null;
                }

                catch (Exception ex)
                {
                    throw ex;
                }
                return View(_ObjPharmacyMas);
            }
            else
            {
                TempData["Warning"] = "Insert rights not given!";
                Index();
                return View("Index");
            }
        }

        public ActionResult Save(PharmacyMasterModel _ObjModel)
        {
            bool _Result = false;
            List<sp_CustSuppAssociation_Select_Result> _ObjCSA;
            try
            {

                _ObjCSA = _ObjModel.CSAssociation.Select(x => new sp_CustSuppAssociation_Select_Result
                {
                    ID = x.Id,
                    refPharmacyID = x.refPharmacyId,
                    SupplierId = x.refSupplierId,
                    AccountNo = x.AccountNo
                }).ToList();

                _Result = _ObjPharmacy.SavePharmarcyMaster(_ObjModel.PharmacyId, _ObjModel.PharmacyName, _ObjModel.CompanyName, _ObjModel.Address1, _ObjModel.Address2,
                                _ObjModel.City, _ObjModel.PostCode, _ObjModel.RespPerson, _ObjModel.Telephone, _ObjModel.FaxNo, _ObjModel.EmailID,
                                _ObjModel.DCRegNo, _ObjModel.PharmacoRegNo, _ObjModel.IsActive, clsCommonUI._User, clsCommonUI._Terminal, _ObjCSA);

                if (_Result)
                {

                    if (_ObjModel.PharmacyId == 0)
                    {
                        TempData["Success"] = "Pharmacy Success fully inserted.";
                    }
                    else
                    {
                        TempData["Success"] = "Pharmacy Success fully updated.";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Warning"] = "Pharmacy fail to insert. There are Some server error occure.";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public JsonResult Delete(int inID)
        {
            try
            {

                bool _Result = _ObjPharmacy.DeletePharmacyMaster(inID);
                if (_Result)
                {
                    return Json(new { _result = true, _Message = "Pharmacy success fully deleted." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { _result = false, _Message = "Pharmacy fail to delete." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { _result = false, _Message = ex.Message }, JsonRequestBehavior.AllowGet);
                throw ex;
            }
        }

        //public JsonResult CheckAllowToDelete(int PharmacyId)
        //{
        //    clsProcessData _ObjProc = new clsProcessData();
           
        //    try
        //    {
        //        //Are you sure to delete pharmacy profile along with its data?
        //        if (_ObjProc.GetProcessDataSelectWhere(" and refPharmacyId = " + PharmacyId).ToList().Count > 0)
        //            return Json(new { Result = false , Message = "Pharmacy Associated with the Supplier and also process data with them!" }, JsonRequestBehavior.AllowGet);
        //        else if (_ObjPharmacy.GetCustSuppAssociationSlectWhere("and RefPharmacyId = " + PharmacyId + " and AccountNo Is NULL").ToList().Count != _ObjPharmacy.GetCustSuppAssociationSlectWhere("and RefPharmacyId = " + PharmacyId ).ToList().Count)
        //            return Json(new { Result = false, Message = "Pharmacy only Associated with the Supplier!" }, JsonRequestBehavior.AllowGet);

        //        return Json(new { Result = true , Message = ""}, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public PartialViewResult AjaxHandler()
        {
            //List<sp_PharmacyMaster_SelectWhere_Result> _ObjPharmacyList;
            try
            {
                //_ObjPharmacyList = _ObjPharmacy.GetPharmacyDetailSelectWhere("");

                //var result = from s in _ObjPharmacyList
                //             select new[]
                //             {
                //                s.PharmacyName,s.Address1 + " " + s.Address2 + " " + s.City + " " + s.PostCode,
                //                s.RespPerson,s.Telephone,s.EmailID,s.DCRegNo,
                //                s.PharmacoRegNo,s.Fax,Convert.ToString(s.IsActive),Convert.ToString(s.PharmacyID) 
                //             };

                //return Json(new
                //{
                //    sEcho = "",
                //    iTotalRecords = _ObjPharmacyList.Count(),
                //    iTotalDisplayRecords = _ObjPharmacyList.Count(),
                //    aaData = result
                //}, JsonRequestBehavior.AllowGet);

                DataTable _dt = new DataTable();
                _dt = _ObjPharmacy.GetPharmacyDetailSelectWhereInDatatable(" Order By PharmacyName");

                return PartialView("PharmacyDetailPartial", _dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult ValidateDuplicatevalue(string pName)
        {
            try
            {
                bool _Result = _ObjPharmacy.CheckDupliaction(pName);
                return Json(new { Result = _Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public JsonResult SetPharmacyStatus(int pId, bool pIsActive)
        {
            try
            {
                if (pIsActive)
                    pIsActive = false;
                else
                    pIsActive = true;

                bool _Result = _ObjPharmacy.SetPharmacyStatus(pId, pIsActive, clsCommonUI._User, clsCommonUI._Terminal);
                return Json(new { Result = _Result }, JsonRequestBehavior.AllowGet);
                //return View("Index");
            }
            catch (Exception ex)
            {
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
                //throw ex;
            }
        }


        public JsonResult CheckDuplicateAccountNo(string pAccountNo,int pCustAssId)
        {
            bool _Result = true;
            try
            {
                if (pAccountNo != null)
                {
                    if (_ObjPharmacy.GetCustSuppAssociationSlectWhere(" and ID <> " + pCustAssId  + " and Upper(AccountNo) = '" + pAccountNo.ToUpper() + "'").Count > 0)
                        _Result = true;
                    else
                        _Result = false;
                }
                return Json(_Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}