using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalystDB;

namespace DataAnalystDA
{
    public class clsUserMaster
    {
        DataAnalystDBEntities _cnn;

        public clsUserMaster()
        {
            _cnn = new DataAnalystDBEntities();
        }

        public bool saveUserMaster(int pID, string pFirstName, string pLastName, string pGender,
                            DateTime? pDOB, string pMobileNo, string pContactNo1, string pContactNo2, string pEmailID, bool pIsActive,
                            int? pRefRoleID, string pUserName,
                            int pUser, string pTerminal)
        {
            bool retVal = false;
            try
            {
                var _obj = _cnn.sp_UserMaster_Save(pID, pFirstName, pLastName,
                                                    pGender, pDOB, pMobileNo, pContactNo1, pContactNo2, pEmailID,
                                                    pIsActive, pRefRoleID, pUserName,
                                                      pUser, pTerminal).FirstOrDefault().Value;

                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public bool? deleteUserMaster(int pID)
        {
            bool? retVal = false;
            try
            {
                var _obj = _cnn.sp_UserMaster_Delete(pID);
                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public bool? SetUserRole(int ID, int RefRoleId, string UserName, int User, string Terminal)
        {
            bool? retVal = true;
            try
            {
                var _obj = _cnn.sp_UserMaster_SetUserRole(ID, RefRoleId, UserName, User, Terminal);

            }
            catch (Exception)
            {

                return false;
            }
            return retVal;
        }

        public List<sp_UserMaster_Select_Result> getUserMaster(int pID)
        {
            List<sp_UserMaster_Select_Result> retVal;

            try
            {
                retVal = _cnn.sp_UserMaster_Select(pID).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }


        public List<sp_UserMaster_SelectWhere_Result> getUserMasterWhere(string pInValue)
        {
            List<sp_UserMaster_SelectWhere_Result> retVal;

            try
            {
                retVal = _cnn.sp_UserMaster_SelectWhere(pInValue).ToList();
            }
            catch (Exception)
            {
                retVal = null;
            }

            return retVal;
        }

        public bool? updateStatus_IsActive(int pID, bool pFlag)
        {
            bool? retVal = false;
            try
            {
                //var _obj = (from x in _cnn.UserMaster
                //            where x.ID == pID
                //            select x).SingleOrDefault();

                //_obj.IsActive = pFlag;
                //clsCommon.Commit(_cnn);

                UserMaster _ObjUserMas = _cnn.UserMaster.Find(pID);
                //db.SaveChanges();

                retVal = saveUserMaster(pID, _ObjUserMas.FirstName, _ObjUserMas.LastName, _ObjUserMas.Gender,
                            _ObjUserMas.DOB, _ObjUserMas.MobileNo, _ObjUserMas.ContactNo1, _ObjUserMas.ContactNo2, _ObjUserMas.EmailID, pFlag,
                            _ObjUserMas.RefRoleID, _ObjUserMas.UserName, _ObjUserMas.InsUser, _ObjUserMas.InsTerminal);

                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;

        }

        public bool isUserExists(int pID, string pValueName)
        {
            bool retVal = false;

            try
            {
                int _resp;
                if (pID == 0)
                {
                    _resp = _cnn.sp_UserMaster_SelectWhere(" and UserName='" + pValueName.Trim() + "'").ToList().Count;
                }
                else
                {
                    _resp = _cnn.sp_UserMaster_SelectWhere(" and ID !=" + pID.ToString() + " and UserName='" + pValueName.Trim() + "'").ToList().Count;
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
