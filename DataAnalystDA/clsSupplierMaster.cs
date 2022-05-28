using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAnalystDB;

namespace DataAnalystDA
{
    public class clsSupplierMaster
    {
        DataAnalystDBEntities _cnn;
        public clsSupplierMaster()
        {
            _cnn = new DataAnalystDBEntities();
        }

        //Save Supplier In SupplierMaster
        public bool SaveSupplierMaster(int? pSupplierID, string pSupplierName, string pAddress1, string pAddress2, string pCity,
                                        string pPostCode, string pRespPerson, string pTelephone, string pFaxNo, string pEmailID,
                                        bool pIsActive, int pUser, string pTerminal)
        {
            bool _retval = false;
            try
            {
                var _Result = _cnn.sp_SupplierMaster_Save(pSupplierID, pSupplierName, pAddress1, pAddress2, pCity, pPostCode, pRespPerson,
                                                        pTelephone, pFaxNo, pEmailID, pIsActive, pUser, pTerminal).FirstOrDefault().Value;
                if (_Result > 0)
                    _retval = true;
                else
                    _retval = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _retval;
        }

        //Delete Supplier from SupplierMaster
        public bool DeleteSupplierMaster(int pSupplierId)
        {
            bool _retval = false;
            try
            {
                _retval = _cnn.sp_SupplierMaster_Delete(pSupplierId).FirstOrDefault().Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _retval;
        }

        //Get Supplier Detail from SupplierMaster
        public List<sp_SupplierMaster_Select_Result> GetSupplierDetail(int pSupplierId)
        {
            List<sp_SupplierMaster_Select_Result> _retval;
            try
            {
                _retval = _cnn.sp_SupplierMaster_Select(pSupplierId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _retval;
        }

        //Get Supplier Detail from SupplierMaster Base on Where Condition
        public List<sp_SupplierMaster_SelectWhere_Result> GetSupplierDetailSelectWhere(string pCondition)
        {
            List<sp_SupplierMaster_SelectWhere_Result> _retval;
            try
            {
                _retval = _cnn.sp_SupplierMaster_SelectWhere(pCondition).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _retval;
        }

        //Check Validation For Duplicate Supplier Name
        public bool CheckDupliaction(string pSupplierName)
        {
            bool _retval = false;
            try
            {
                int _DRCount = _cnn.sp_SupplierMaster_SelectWhere(" and Upper(SupplierName) = '" + pSupplierName.ToUpper() + "'").ToList().Count;
                if (_DRCount > 0)
                    _retval = true;
                else
                    _retval = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _retval;
        }

        //Set Supplier as Active 
        public bool SetSupplierStatus(int pSupplierId,bool pIsActive, int pUser, string pTerminal)
        {
            bool _retval = false;
            try
            {
                sp_SupplierMaster_Select_Result _ObjSupp = _cnn.sp_SupplierMaster_Select(pSupplierId).FirstOrDefault();
                _retval = SaveSupplierMaster(pSupplierId, _ObjSupp.SupplierName, _ObjSupp.Address1, _ObjSupp.Address2, _ObjSupp.City,
                                        _ObjSupp.PostCode, _ObjSupp.RespPerson, _ObjSupp.Telephone, _ObjSupp.FaxNo, _ObjSupp.EmailID,
                                        pIsActive, pUser, pTerminal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _retval;
        }

    }
}