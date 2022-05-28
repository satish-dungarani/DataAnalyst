using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using DataAnalystDA;
using DataAnalystDB;
using DataAnalyst.Models;
using System.IO;
using System.Globalization;
using System.Net.Mail;

namespace DataAnalyst
{
    public static class clsCommonUI
    {

        public static int _User = 0;
        public static string _Terminal = string.Empty;
        public static int? _UserRoleId = 0;
        public static bool _IsReader = false;

        public enum MasterType
        {
            UserRole = 1,
        }

        public static SelectList PharmacyList(int pMember)
        {
            List<SelectListItem> _SelectPharmacy = new List<SelectListItem>();
            clsPharmacyMaster _ObjPharma = new clsPharmacyMaster();
            try
            {
                string _Condition = "";
                // 1 = DC
                // 0 = Non DC
                // -1 = All
                if (pMember == 1)
                    _Condition = " and DCRegNo Is Not NULL";
                else if (pMember == 0)
                    _Condition = " and DCRegNo Is NULL";

                _Condition = _Condition + " Order By PharmacyName";
                var _ObjPharmaList = _ObjPharma.GetPharmacyDetailSelectWhere(_Condition).ToList();
                foreach (var _Obj in _ObjPharmaList)
                {
                    _SelectPharmacy.Add(new SelectListItem { Selected = false, Text = _Obj.PharmacyName + " - " + _Obj.City, Value = _Obj.PharmacyID.ToString() });
                }

                return new SelectList(_SelectPharmacy, "Value", "Text");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SelectList FinYearList()
        {
            clsProcessData _ObjProcess = new clsProcessData();
            List<SelectListItem> _SelectedList = new List<SelectListItem>(); ;
            try
            {
                foreach (var _Obj in _ObjProcess.GetProcessDataSelectWhere(" Order By FinYear").Select(x => x.FinYear).Distinct().ToList())
                {
                    _SelectedList.Add(new SelectListItem { Selected = false, Text = _Obj.Substring(0, 4) + "-" + _Obj.Substring(4, 4), Value = _Obj.ToString() });
                }

                return new SelectList(_SelectedList, "Value", "Text");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static SelectList SupplierList(int pMember)
        {
            List<SelectListItem> _SelectSupplier = new List<SelectListItem>();
            clsSupplierMaster _ObjSupp = new clsSupplierMaster();
            try
            {
                string _Condition = "";
                // 1 = DC
                // 0 = Non DC
                // -1 = All
                if (pMember == 1)
                    _Condition = " and SupplierId In (select refSupplierID from ProcessData " +
                                    "Where refPharmacyID In (Select PharmacyID From PharmacyMas Where DCRegNo Is Not NULL))";
                else if (pMember == 0)
                    _Condition = " and SupplierId In (select refSupplierID from ProcessData " +
                                    "Where refPharmacyID In (Select PharmacyID From PharmacyMas Where DCRegNo Is NULL))";

                _Condition = _Condition + " Order By SupplierName";
                var _ObjPharmaList = _ObjSupp.GetSupplierDetailSelectWhere(_Condition).ToList();
                foreach (var _Obj in _ObjPharmaList)
                {
                    _SelectSupplier.Add(new SelectListItem { Selected = false, Text = _Obj.SupplierName, Value = _Obj.SupplierId.ToString() });
                }

                return new SelectList(_SelectSupplier, "Value", "Text");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SelectList DurationNameList(string _DurationType)
        {
            List<SelectListItem> _SelectDuration = new List<SelectListItem>();
            int iCounter = 0;
            try
            {
                if (_DurationType.ToUpper() == "Q")
                    iCounter = 4;
                else if (_DurationType.ToUpper() == "M")
                    iCounter = 12;
                else if (_DurationType.ToUpper() == "H")
                    iCounter = 2;
                for (int i = 1; i <= iCounter; i++)
                {
                    if (_DurationType.ToUpper() == "Q" || _DurationType.ToUpper() == "H")
                        _SelectDuration.Add(new SelectListItem { Selected = false, Text = (_DurationType + i).ToString(), Value = (_DurationType + i).ToString() });
                    else if(_DurationType.ToUpper() == "M")
                        _SelectDuration.Add(new SelectListItem { Selected = false, Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3), Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3) });
                }
                return new SelectList(_SelectDuration, "Value", "Text");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static SelectList fillMasterValue(int pRefMasterId)
        {
            #region Valid values for pID are as follow.
            //===============================================
            //  Id	MasterName	                OrdNo
            //===============================================

            //  1  UserRole                    1.00

            #endregion

            clsMasterValue _objMasterValue = new clsMasterValue();

            List<SelectListItem> Selectlist = new List<SelectListItem>();

            foreach (var obj in _objMasterValue.getMasterValue(pRefMasterId, -1))
            {
                Selectlist.Add(new SelectListItem { Selected = false, Text = obj.ValueName, Value = obj.ID.ToString() });

            }
            return new SelectList(Selectlist, "Value", "Text");
        }

        public static bool? checkAccessAlloworNot(List<sp_RetrieveMenuRightsWise_Select_Result> _objParam)
        {
            bool _retval = false;
            try
            {
                string _RowUrlString = HttpContext.Current.Request.RawUrl.ToString();
                string _ControllerName = _RowUrlString.Substring(1, _RowUrlString.IndexOf('/', 1) - 1);
                string _ActionName = _RowUrlString.Substring(_RowUrlString.LastIndexOf('/') + 1, _RowUrlString.Length - _RowUrlString.LastIndexOf('/') - 1);
                if (_ControllerName.ToUpper() == "HOME")
                {
                    _retval = true;
                    return _retval;
                }
                foreach (sp_RetrieveMenuRightsWise_Select_Result _obj in _objParam)
                {
                    if (_obj.ControllerName != null)
                    {
                        if (_ControllerName.ToUpper() == _obj.ControllerName.ToUpper())
                        {
                            _retval = true;
                            break;
                        }
                    }
                }
                if (_retval == true)
                {
                    if (_ActionName.ToUpper() == "MANAGE")
                    {
                        foreach (sp_RetrieveMenuRightsWise_Select_Result _obj in _objParam)
                        {
                            if ((_obj.CanInsert == true || _obj.CanUpdate == true) && _ControllerName.ToUpper() == _obj.ControllerName.ToUpper())
                            {
                                return _retval;
                            }
                        }
                        _retval = false;
                    }
                }
            }
            catch (Exception)
            {

                return _retval;
            }
            return _retval;
        }

        public static bool? checkAccessIndividual(List<sp_RetrieveMenuRightsWise_Select_Result> _objParam, string pOperation, string pControllerName)
        {
            bool _retval = false;

            try
            {
                if (pOperation.ToUpper() == "INSERT")
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.ControllerName != null)
                        {
                            if (pControllerName == _Obj.ControllerName.ToUpper() && _Obj.CanInsert == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }
                else if (pOperation.ToUpper() == "UPDATE")
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.ControllerName != null)
                        {
                            if (pControllerName == _Obj.ControllerName.ToUpper() && _Obj.CanUpdate == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }
                else if (pOperation.ToUpper() == "DELETE")
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.ControllerName != null)
                        {
                            if (pControllerName == _Obj.ControllerName.ToUpper() && _Obj.CanDelete == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                return false;

            }
            return _retval;
        }

        public static bool? checkAccessIndividualInMapPath(List<sp_RetrieveMenuRightsWise_Select_Result> _objParam, string pOperation, string pMenuPath)
        {
            bool _retval = false;

            try
            {
                if (pOperation.ToUpper() == "INSERT")
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.MenuPath != null)
                        {
                            if (pMenuPath.ToUpper() == _Obj.MenuPath.ToUpper() && _Obj.CanInsert == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }
                else if (pOperation.ToUpper() == "UPDATE")
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.MenuPath != null)
                        {
                            if (pMenuPath.ToUpper() == _Obj.MenuPath.ToUpper() && _Obj.CanUpdate == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }
                else if (pOperation.ToUpper() == "DELETE")
                {
                    foreach (var _Obj in _objParam)
                    {
                        if (_Obj.MenuPath != null)
                        {
                            if (pMenuPath.ToUpper() == _Obj.MenuPath.ToUpper() && _Obj.CanDelete == true)
                            {
                                _retval = true;
                                return _retval;
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                return false;

            }
            return _retval;
        }

        public static bool SendMail(string _EmailId,string _Subj, string _Body )
        {
            try
            {
                if (_EmailId != "" && _EmailId != null)
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(_EmailId);
                    mail.From = new MailAddress("demomail@jayamsoft.net");
                    mail.Subject = _Subj;
                    mail.Body = _Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "jayamsoft.net";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential
                    ("demomail@jayamsoft.net", "demo!@#123");// Enter seders User name and password
                    smtp.EnableSsl = false;
                    smtp.Send(mail);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }

    }
}