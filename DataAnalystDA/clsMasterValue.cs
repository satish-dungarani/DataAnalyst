using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DataAnalystDB;

namespace DataAnalystDA
{
    public class clsMasterValue
    {
        DataAnalystDBEntities _cnn;

        public clsMasterValue()
        {
            _cnn = new DataAnalystDBEntities();
        }

        public bool? saveMasterValue(int pRefMasterID, int pID, string pValueName, string pShortName, int pOrdNo,
                                    bool pIsActive, int pUser, string pTerminal)
        {
            bool? retVal = false;
            try
            {
                var _obj = _cnn.sp_MasterValue_Save(pRefMasterID, pID, pValueName, pShortName, pOrdNo,
                                                 pIsActive, pUser, pTerminal);
                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public bool? deleteMasterValue(int pID)
        {
            bool? retVal = false;
            try
            {
                var _obj = _cnn.sp_MasterValue_Delete(pID);
                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public bool? updateStatus(int pID, bool pFlag)
        {
            bool? retVal = false;
            try
            {
                var _obj = (from x in _cnn.MasterValue
                            where x.ID == pID
                            select x).SingleOrDefault();

                _obj.IsActive = pFlag;
                _cnn.SaveChanges();

                retVal = true;

            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;

        }

        public List<sp_MasterValue_Select_Result> getMasterValue(int pRefMasterID, int pID)
        {
            List<sp_MasterValue_Select_Result> retVal;
            try
            {
                retVal = _cnn.sp_MasterValue_Select(pRefMasterID, pID).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public List<sp_MasterValue_SelectWhere_Result> getMasterValueWhere(int pRefMasterID, string pInValue)
        {
            List<sp_MasterValue_SelectWhere_Result> retVal;

            try
            {
                retVal = _cnn.sp_MasterValue_SelectWhere(pRefMasterID, pInValue).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public bool isValueExists(int pRefMasterID, int pID, string pValueName)
        {
            bool retVal = false;

            try
            {
                int _resp;
                if (pID == 0)
                {
                    _resp = _cnn.sp_MasterValue_SelectWhere(pRefMasterID, " and ValueName='" + pValueName.Trim() + "'").ToList().Count;
                }
                else
                {
                    _resp = _cnn.sp_MasterValue_SelectWhere(pRefMasterID, " and ID !=" + pID.ToString() +
                                                                                     " and ValueName='" + pValueName.Trim() + "'").ToList().Count;
                }

                if (_resp > 0)
                {
                    retVal = true;
                }
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

    }
}
