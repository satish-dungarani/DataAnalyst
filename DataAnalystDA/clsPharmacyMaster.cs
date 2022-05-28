using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAnalystDB;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Configuration;
using System.Data.Common;

namespace DataAnalystDA
{
    public class clsPharmacyMaster
    {
        DataAnalystDBEntities _cnn;
        public clsPharmacyMaster()
        {
            _cnn = new DataAnalystDBEntities();
        }

        //Save Pharmacy Data
        public bool SavePharmarcyMaster(int? pPharmacyID, string pPharmacyName, string pCopmanyName, string pAddress1, string pAddress2, string pCity,
                                        string pPostCode, string pRespPerson, string pTelephone, string pFaxNo, string pEmailID,
                                        string pDCRegNo, string pPharmacoRegNo, bool pIsActive, int pUser, string pTerminal, List<sp_CustSuppAssociation_Select_Result> _ObjParam)
        {
            bool _retval = false;
            try
            {
                var _Result = _cnn.sp_PharmacyMaster_Save(pPharmacyID, pPharmacyName,pCopmanyName, pAddress1, pAddress2, pCity,
                                        pPostCode, pRespPerson, pTelephone, pFaxNo, pEmailID,
                                        pDCRegNo, pPharmacoRegNo, pIsActive, pUser, pTerminal).FirstOrDefault().Value;
                if (_Result > 0)
                {
                    if (_ObjParam.Count >  0)
                    {
                        foreach (var _Obj in _ObjParam)
                        {
                            var _Rslt = _cnn.sp_CustAppAssociation_Save(_Obj.ID, Convert.ToInt32(_Result), _Obj.SupplierId, _Obj.AccountNo, pUser, pTerminal);
                        }
                    }
                    _retval = true;
                }
                else
                    _retval = false;
                return _retval;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Delete Pharmacy from PharmarcyMaster
        public bool DeletePharmacyMaster(int pPharmacyId)
        {
            bool _retval = false;
            try
            {
                _retval = _cnn.sp_PharmacyMaster_Delete(pPharmacyId).FirstOrDefault().Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _retval;
        }

        //Get Pharmacy Detail from PharmacyMaster
        public List<sp_PharmacyMaster_Select_Result> GetPharmacyDetail(int pPharmacyId)
        {
            List<sp_PharmacyMaster_Select_Result> _retval;
            try
            {
                _retval = _cnn.sp_PharmacyMaster_Select(pPharmacyId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _retval;
        }

        //Get Pharmacy Detail from PharmacyMaster Base on Where Condition
        public List<sp_PharmacyMaster_SelectWhere_Result> GetPharmacyDetailSelectWhere(string pCondition)
        {
            List<sp_PharmacyMaster_SelectWhere_Result> _retval;
            try
            {
                _retval = _cnn.sp_PharmacyMaster_SelectWhere(pCondition).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _retval;
        }

        public DataTable GetPharmacyDetailSelectWhereInDatatable(string pCondition)
        {
            DataSet _Ds = new DataSet();
            try
            {
                Database _ObjDB = new SqlDatabase(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                using (DbCommand _ObjCmd = _ObjDB.GetStoredProcCommand("sp_PharmacyMaster_SelectWhereWithAssociation"))
                {
                    _ObjDB.AddInParameter(_ObjCmd, "@dfltWhere", DbType.String, pCondition);
                    _Ds = _ObjDB.ExecuteDataSet(_ObjCmd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Ds.Tables[0];
        }

        //Check Validation For Duplicate Pharmacy Name
        public bool CheckDupliaction(string pPharmacyName)
        {
            bool _retval = false;
            try
            {
                int _DRCount = _cnn.sp_PharmacyMaster_SelectWhere(" and Upper(PharmacyName) = '" + pPharmacyName.ToUpper() + "'").ToList().Count;
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

        //Set Pharmacy  as Active 
        public bool SetPharmacyStatus(int pPharmacyId, bool pIsActive, int pUser, string pTerminal)
        {
            bool _retval = false;
            try
            {
                List<sp_CustSuppAssociation_Select_Result> _ObjCSA = new List<sp_CustSuppAssociation_Select_Result>();

                sp_PharmacyMaster_Select_Result _ObjPharm = _cnn.sp_PharmacyMaster_Select(pPharmacyId).FirstOrDefault();
                _retval = SavePharmarcyMaster(pPharmacyId, _ObjPharm.PharmacyName, _ObjPharm.CompanyName , _ObjPharm.Address1, _ObjPharm.Address2, _ObjPharm.City,
                                        _ObjPharm.PostCode, _ObjPharm.RespPerson, _ObjPharm.Telephone, _ObjPharm.Fax, _ObjPharm.EmailID,
                                        _ObjPharm.DCRegNo, _ObjPharm.PharmacoRegNo, pIsActive, pUser, pTerminal, _ObjCSA);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _retval;
        }

        //Get Customer Supplier Assotion Detail
        public List<sp_CustSuppAssociation_Select_Result> GetCSAssociation(int? pPharmacyId)
        {
            List<sp_CustSuppAssociation_Select_Result> _ObjCSA;
            try
            {
                _ObjCSA = _cnn.sp_CustSuppAssociation_Select(pPharmacyId).ToList();
                return _ObjCSA;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<sp_CustSuppAssociation_SelectWhere_Result> GetCustSuppAssociationSlectWhere(string _Defination)
        {
            List<sp_CustSuppAssociation_SelectWhere_Result> _Obj;
            try
            {
                _Obj = _cnn.sp_CustSuppAssociation_SelectWhere(_Defination).ToList();
                return _Obj;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}