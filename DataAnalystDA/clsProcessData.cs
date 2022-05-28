using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAnalystDB;
using System.Data;
using System.Configuration;
using System.Data.Common;

namespace DataAnalystDA
{

    public class clsProcessData
    {
        DataAnalystDBEntities _cnn;
        public clsProcessData()
        {
            _cnn = new DataAnalystDBEntities();
        }

        public List<sp_ProcessData_SelectCondition_Result> GetProcessDataSelectWhere(string _Condtion)
        {
            List<sp_ProcessData_SelectCondition_Result> _Obj;
            try
            {
                _Obj = _cnn.sp_ProcessData_SelectCondition(_Condtion).ToList();
                return _Obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<sp_ProcessData_SupplierWise_Result> GetProcessDataSupplierWise(DateTime? _StartDate, DateTime? _EndDate, int pMemberType)
        {
            List<sp_ProcessData_SupplierWise_Result> _Obj;
            try
            {
                _Obj = _cnn.sp_ProcessData_SupplierWise(_StartDate, _EndDate, pMemberType).ToList();
                return _Obj;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<sp_ProcessData_PharmacyWise_Result> GetProcessDataPharmacyWise(DateTime? _StartDate, DateTime? _EndDate, int pMemberType)
        {
            List<sp_ProcessData_PharmacyWise_Result> _Obj;
            try
            {
                _Obj = _cnn.sp_ProcessData_PharmacyWise(_StartDate, _EndDate, pMemberType).ToList();
                return _Obj;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<sp_ProcessData_MonthlyForAllSupp_Result> GetMonthlyData(DateTime? _StartDate, DateTime? _EndDate, int pMemberType)
        {
            List<sp_ProcessData_MonthlyForAllSupp_Result> _Obj;
            try
            {
                _Obj = _cnn.sp_ProcessData_MonthlyForAllSupp(_StartDate, _EndDate, pMemberType).ToList();
                return _Obj;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<sp_ProcessData_PharmacyMonthlyPurchase_Result> GetMonthlyPharmacyPurchaseData(DateTime? _StartDate, DateTime? _EndDate, string pMonthFilterType, int? pPharmacyId)
        {
            List<sp_ProcessData_PharmacyMonthlyPurchase_Result> _Obj;
            try
            {
                _Obj = _cnn.sp_ProcessData_PharmacyMonthlyPurchase(_StartDate, _EndDate, pMonthFilterType, pPharmacyId).ToList();
                return _Obj;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<sp_ProcessData_PharmacyProcessData_Result> GetPharmacyProcessData(DateTime? _StartDate, DateTime? _EndDate, int? pPharmacyId)
        {
            List<sp_ProcessData_PharmacyProcessData_Result> _Obj;
            try
            {
                _Obj = _cnn.sp_ProcessData_PharmacyProcessData(_StartDate, _EndDate, pPharmacyId).ToList();
                return _Obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetSupplierWiseSummaryRpt(string DateRange, int pMemberType, string SupplierList, string PharmacyList, string pDuration, string pRptType)
        {
            DataSet _DS = new DataSet();
            try
            {

                DateTime? _StartDate = null;
                DateTime? _EndDate = null;
                if (DateRange != null && DateRange != "")
                {
                    _StartDate = Convert.ToDateTime(DateRange.Split('-')[0].Trim());
                    _EndDate = Convert.ToDateTime(DateRange.Split('-')[1].Trim());
                }

                Database objDB = new SqlDatabase(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                using (DbCommand objCMD = objDB.GetStoredProcCommand("sp_ProcessData_SelectWhere"))
                {

                    objDB.AddInParameter(objCMD, "@pStartDate",
                                         DbType.Date, _StartDate);
                    objDB.AddInParameter(objCMD, "@pEndDate",
                                         DbType.Date, _EndDate);
                    objDB.AddInParameter(objCMD, "@pMemberType",
                                         DbType.Int32, pMemberType);
                    objDB.AddInParameter(objCMD, "@pSupplierIdList",
                                         DbType.String, SupplierList);
                    objDB.AddInParameter(objCMD, "@pPharmacyIdList",
                                         DbType.String, PharmacyList);
                    objDB.AddInParameter(objCMD, "@pDurationType",
                                         DbType.String, pDuration);
                    objDB.AddInParameter(objCMD, "@pRptType",
                                         DbType.String, pRptType);

                    _DS = objDB.ExecuteDataSet(objCMD);
                }

                return _DS.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<sp_ProcessData_SupplierMonthlySales_Result> GetMonthlySupplierSalesData(DateTime? _StartDate, DateTime? _EndDate, int pMemberType, string pMonthFilterType, string pSupplierIdList)
        {
            List<sp_ProcessData_SupplierMonthlySales_Result> _Obj;
            try
            {
                _Obj = _cnn.sp_ProcessData_SupplierMonthlySales(_StartDate, _EndDate, pMemberType, pMonthFilterType, pSupplierIdList).ToList();
                return _Obj;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<sp_ProcessData_SupplierProcessData_Result> GetSupplierProcessData(DateTime? _StartDate, DateTime? _EndDate, int pMemberType, string pSupplierIdList)
        {
            List<sp_ProcessData_SupplierProcessData_Result> _Obj;
            try
            {
                _Obj = _cnn.sp_ProcessData_SupplierProcessData(_StartDate, _EndDate, pMemberType, pSupplierIdList).ToList();
                return _Obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<sp_ProcessData_PharmacyPurchaseComparison_Result> GetPharmacyPurchaseComparition(DateTime? _StartDate, DateTime? _EndDate, string pDurationType, int? pPharmacyId)
        {
            List<sp_ProcessData_PharmacyPurchaseComparison_Result> _Obj;
            try
            {
                _Obj = _cnn.sp_ProcessData_PharmacyPurchaseComparison(_StartDate, _EndDate, pDurationType, pPharmacyId).ToList();
                return _Obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<sp_ProcessData_SupplierSalesComparison_Result> GetSupplierSalesComparition(DateTime? _StartDate, DateTime? _EndDate, int pMemberType, string pDurationType, string pSupplierIdList)
        {
            List<sp_ProcessData_SupplierSalesComparison_Result> _Obj;
            try
            {
                _Obj = _cnn.sp_ProcessData_SupplierSalesComparison(_StartDate, _EndDate, pMemberType, pDurationType, pSupplierIdList).ToList();
                return _Obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<sp_ProcessData_SupplierSalesBaseOnPharmacy_Result> GetSupplierSalesBaseOnPharmacy(DateTime? _StartDate, DateTime? _EndDate, int? pPharmacyId)
        {
            List<sp_ProcessData_SupplierSalesBaseOnPharmacy_Result> _Obj;
            try
            {
                _Obj = _cnn.sp_ProcessData_SupplierSalesBaseOnPharmacy(_StartDate, _EndDate, pPharmacyId).ToList();
                return _Obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<sp_ProcessData_PharmacyPurchaseBaseOnSupplier_Result> GetPharmacyPurchaseBaseOnSupplier(DateTime? _StartDate, DateTime? _EndDate, int pMemberType, string pSupplierIdList)
        {
            List<sp_ProcessData_PharmacyPurchaseBaseOnSupplier_Result> _Obj;
            try
            {
                _Obj = _cnn.sp_ProcessData_PharmacyPurchaseBaseOnSupplier(_StartDate, _EndDate, pMemberType, pSupplierIdList).ToList();
                return _Obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteProcessData(string pDataYear, string pDataMonth)
        {
            try
            {
                return _cnn.sp_ProcessData_Delete(pDataYear, pDataMonth).FirstOrDefault().Value;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<sp_ProcessData_SelectedSupplierMonthlySales_Result> GetSelectedSupplierMonthlySales(DateTime? pStartDate, DateTime? pEndDate, int pMemberType, string pDuration, string pSupplierIdList)
        {
            List<sp_ProcessData_SelectedSupplierMonthlySales_Result> _ObjSSS;
            try
            {
                _ObjSSS = _cnn.sp_ProcessData_SelectedSupplierMonthlySales(pStartDate, pEndDate, pMemberType, pDuration, pSupplierIdList).ToList();
                return _ObjSSS;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable GetSupplierComparitionData(string DateRange, int pMemberType, string SupplierList, string PharmacyList, string pDuration, string pDurationName)
        {
            try
            {
                Database _Db = new SqlDatabase(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                using (DbCommand _Objcmd = _Db.GetStoredProcCommand("sp_ProcessData_PharmacyWiseYearlyComparition"))
                {
                    _Db.AddInParameter(_Objcmd, "@pYearList", DbType.String, DateRange);
                    _Db.AddInParameter(_Objcmd, "@pMemberType", DbType.Int32, pMemberType);
                    _Db.AddInParameter(_Objcmd, "@pSupplierIdList", DbType.String, SupplierList);
                    _Db.AddInParameter(_Objcmd, "@pPharmacyIdList", DbType.String, PharmacyList);
                    _Db.AddInParameter(_Objcmd, "@pDurationType", DbType.String, pDuration);
                    _Db.AddInParameter(_Objcmd, "@pDurationNo", DbType.String, pDurationName);

                    return _Db.ExecuteDataSet(_Objcmd).Tables[0];
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}