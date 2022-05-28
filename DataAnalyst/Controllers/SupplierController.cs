using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAnalystDA;
using DataAnalyst.Models;
using DataAnalystDB;

namespace DataAnalyst.Controllers
{
    public class SupplierController : BaseController
    {
        clsSupplierMaster _ObjSupplier = new clsSupplierMaster();
        //
        // GET: /Supplier/
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult EditSession(int? pId)
        {
            string _Message = string.Empty;
            try
            {
                if (Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "UPDATE", "SUPPLIER")))
                {
                    Session["SupplierId"] = pId;
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
            if (Session["SupplierId"] == null)
            {
                result = Convert.ToBoolean(clsCommonUI.checkAccessIndividual((List<sp_RetrieveMenuRightsWise_Select_Result>)Session["AccessMenuList"], "INSERT", "SUPPLIER"));
            }
            if (result)
            {

                SupplierMasterModel _ObjSupplierMas = new SupplierMasterModel();
                try
                {
                    int pID = 0;
                    if (Session["SupplierId"] != null && !string.IsNullOrEmpty(Session["SupplierId"].ToString()))
                        pID = Convert.ToInt32(Session["SupplierId"].ToString());

                    _ObjSupplierMas = _ObjSupplier.GetSupplierDetail(pID).Select(x => new SupplierMasterModel
                    {

                        SupplierId = x.SupplierId,
                        SupplierName = x.SupplierName,
                        Address1 = x.Address1,
                        Address2 = x.Address2,
                        City = x.City,
                        PostCode = x.PostCode,
                        RespPerson = x.RespPerson,
                        Telephone = x.Telephone,
                        FaxNo = x.FaxNo,
                        EmailID = x.EmailID,
                        IsActive = x.IsActive,
                        InsUser = x.InsUser,
                        InsDate = x.InsDate,
                        InsTerminal = x.InsTerminal,
                        UpdUser = x.UpdUser.HasValue ? x.UpdUser.Value : 0,
                        UpdDate = x.UpdDate,
                        UpdTerminal = x.UpdTerminal

                    }).FirstOrDefault();

                    if (_ObjSupplierMas == null)
                    {
                        _ObjSupplierMas = new SupplierMasterModel();
                    }

                    Session["SupplierId"] = null;
                }

                catch (Exception ex)
                {
                    throw ex;
                }
                return View(_ObjSupplierMas);
            }
            else
            {
                TempData["Warning"] = "Insert rights not given!";
                Index();
                return View("Index");
            }
        }

        public ActionResult Save(SupplierMasterModel _ObjModel)
        {
            bool _Result = false;
            try
            {
                _Result = _ObjSupplier.SaveSupplierMaster(_ObjModel.SupplierId, _ObjModel.SupplierName, _ObjModel.Address1, _ObjModel.Address2,
                                _ObjModel.City, _ObjModel.PostCode, _ObjModel.RespPerson, _ObjModel.Telephone, _ObjModel.FaxNo, _ObjModel.EmailID,
                                _ObjModel.IsActive, clsCommonUI._User, clsCommonUI._Terminal);

                if (_Result)
                {
                    if (_ObjModel.SupplierId == 0)
                    {
                        TempData["Success"] = "Supplier Success fully inserted.";
                    }
                    else
                    {
                        TempData["Success"] = "Supplier Success fully updated.";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Warning"] = "Supplier fail to insert. There are Some server error occure.";
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
                bool _Result = _ObjSupplier.DeleteSupplierMaster(inID);
                if (_Result)
                {
                    return Json(new { _result = true, _Message = "Supplier success fully deleted." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { _result = false, _Message = "Supplier fail to delete." }, JsonRequestBehavior.AllowGet);
                }
                    
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public JsonResult CheckAllowToDelete(int SupplierId)
        {
            clsPharmacyMaster _ObjPhar = new clsPharmacyMaster();
            try
            {
                if (_ObjPhar.GetCustSuppAssociationSlectWhere("and RefSupplierId = " + SupplierId + " and AccountNo Is NULL").ToList().Count != _ObjPhar.GetCustSuppAssociationSlectWhere("and RefSupplierId = " + SupplierId).ToList().Count)
                    return Json(new { Result = false}, JsonRequestBehavior.AllowGet);

                return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult AjaxHandler()
        {
            clsSupplierMaster _ObjSupplier = new clsSupplierMaster();
            List<sp_SupplierMaster_SelectWhere_Result> _ObjSupplierList;
            try
            {
                _ObjSupplierList = _ObjSupplier.GetSupplierDetailSelectWhere("");

                var result = from s in _ObjSupplierList
                             select new[]
                             {
                                Convert.ToString(s.IsActive),Convert.ToString(s.SupplierId), s.SupplierName,s.Address1 ,s.Address2 , s.City ,s.PostCode,
                                s.RespPerson,s.Telephone,s.EmailID, s.FaxNo
                             };

                return Json(new
                {
                    sEcho = "",
                    iTotalRecords = _ObjSupplierList.Count(),
                    iTotalDisplayRecords = _ObjSupplierList.Count(),
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
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
                bool _Result = _ObjSupplier.CheckDupliaction(pName);
                return Json(new { Result = _Result } , JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult SetSupplierStatus(int pSupplierId, bool pIsActive)
        {
            try
            {
                if (pIsActive)
                    pIsActive = false;
                else
                    pIsActive = true;

                bool _Result = _ObjSupplier.SetSupplierStatus(pSupplierId, pIsActive,clsCommonUI._User, clsCommonUI._Terminal);
                return Json(new { Result = _Result }, JsonRequestBehavior.AllowGet);
                //return View("Index");
            }
            catch (Exception ex)
            {
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
                //throw ex;
            }
        }
    }
}