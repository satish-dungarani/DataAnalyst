using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalystDB;

namespace DataAnalystDA
{
    public class clsMenuRoleRights
    {
        DataAnalystDBEntities _cnn;
        public clsMenuRoleRights()
        {
            _cnn = new DataAnalystDBEntities();
        }

        public bool? saveMenuRoleRights(int pRefRoleId, int pRefMenuId, bool? pCanInsert, bool? pCanUpdat, bool? pCanDelete, bool? pCanView, int pUser, string pTerminal)
        {
            bool? retval = false;
            try
            {
                _cnn.sp_MenuRoleRights_Save(pRefRoleId, pRefMenuId, pCanInsert, pCanUpdat, pCanDelete, pCanView, pUser, pTerminal);
                retval = true;
            }
            catch (Exception)
            {

                return retval;
            }
            return retval;
        }

        public bool? deleteMenuRoleRights(int pRefRoleId)
        {
            bool? _retval = false;
            try
            {
                _cnn.sp_MenuRoleRights_Delete(pRefRoleId);
                _retval = true;
            }
            catch (Exception)
            {
                return _retval;
            }
            return _retval;
        }

        public List<sp_MenuRoleRights_Select_Result> getMenuRoleRights(int pRefRoleId)
        {
            List<sp_MenuRoleRights_Select_Result> retval = null;
            try
            {
                retval = _cnn.sp_MenuRoleRights_Select(pRefRoleId).ToList();
            }
            catch (Exception)
            {
                return retval;
            }
            return retval;
        }
    }
}
