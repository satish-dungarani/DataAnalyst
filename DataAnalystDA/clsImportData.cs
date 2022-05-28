using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAnalystDB;
using System.Data;
using System.Configuration;
using System.Data.OleDb;

namespace DataAnalystDA
{
    public class clsImportData
    {
        DataAnalystDBEntities _cnn;
        public clsImportData()
        {
            _cnn = new DataAnalystDBEntities();
        }

        public List<sp_ImportData_SelectWhere_Result> SaveImportData(string FilePath, string pDataYear, string pDataMonth, int pUser, string pTerminal)
        {
            List<sp_ImportData_SelectWhere_Result> _retval = new List<sp_ImportData_SelectWhere_Result>();
            bool _rsltval = false;
            int _recordcount = 0;
            DataTable _dtImportData = new DataTable();
            try
            {
                decimal n;
                _dtImportData = ExcelToDatatable(FilePath);
                foreach (DataRow _Dr in _dtImportData.Rows)
                {
                    //if (string.IsNullOrEmpty(_Dr["PharmacyName"].ToString()))
                    //    throw new Exception("Enter Pharmacy Name at Line No. " + (_recordcount + 1));
                    if (string.IsNullOrEmpty(_Dr["AccountNo"].ToString()))
                        throw new Exception("Enter Account No at Line No. " + (_recordcount + 1));
                    else if (string.IsNullOrEmpty(_Dr["TotalSpend"].ToString()))
                        throw new Exception("Enter Total Spend at Line No. " + (_recordcount + 1));
                    else if (string.IsNullOrEmpty(_Dr["TotalRebate"].ToString()))
                        throw new Exception("Enter Total Rebate at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["Generic"].ToString(), out n))
                    //        throw new Exception("Enter Numeric value In Generic Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["EthicalPI"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In EthicalPI Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["SurgicalDressing"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In SurgicalDressing Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["NonGeneric"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In NonGeneric Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["NonPrescription"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In NonPrescription Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["Insulin"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In Insulin Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["Status"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In Status Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["Electrical"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In Electrical Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["Drinks"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In Drinks Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["NonDiscount"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In NonDiscount Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["OTC"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In OTC Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["Mobility"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In Mobility Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["Specials"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In Specials Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["NP8"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In NP8 Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["Other1"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In Other1 Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["Other2"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In Other2 Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["Other3"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In Other3 Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["TotalSpend"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In TotalSpend Field at Line No. " + (_recordcount + 1));
                    //else if (!decimal.TryParse(_Dr["TotalRebate"].ToString(), out n))
                    //    throw new Exception("Enter Numeric value In TotalRebate Field at Line No. " + (_recordcount + 1));

                    _rsltval = _cnn.sp_ImportData_Save(pDataYear, pDataMonth,
                                                        Convert.ToString(_Dr["PharmacyName"]), Convert.ToString(_Dr["AccountNo"]),
                                                        string.IsNullOrEmpty(_Dr["Generic"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["Generic"]),
                                                        string.IsNullOrEmpty(_Dr["EthicalPI"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["EthicalPI"]),
                                                        string.IsNullOrEmpty(_Dr["SurgicalDressing"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["SurgicalDressing"]),
                                                        string.IsNullOrEmpty(_Dr["NonGeneric"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["NonGeneric"]),
                                                        string.IsNullOrEmpty(_Dr["NonPrescription"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["NonPrescription"]),
                                                        string.IsNullOrEmpty(_Dr["Insulin"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["Insulin"]),
                                                        string.IsNullOrEmpty(_Dr["Status"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["Status"]),
                                                        string.IsNullOrEmpty(_Dr["Electrical"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["Electrical"]),
                                                        string.IsNullOrEmpty(_Dr["Drinks"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["Drinks"]),
                                                        string.IsNullOrEmpty(_Dr["NonDiscount"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["NonDiscount"]),
                                                        string.IsNullOrEmpty(_Dr["OTC"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["OTC"]),
                                                        string.IsNullOrEmpty(_Dr["Mobility"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["Mobility"]),
                                                        string.IsNullOrEmpty(_Dr["Specials"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["Specials"]),
                                                        string.IsNullOrEmpty(_Dr["NP8"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["NP8"]),
                                                        string.IsNullOrEmpty(_Dr["Other1"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["Other1"]),
                                                        string.IsNullOrEmpty(_Dr["Other2"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["Other2"]),
                                                        string.IsNullOrEmpty(_Dr["Other3"].ToString()) ? (decimal?)null : Convert.ToDecimal(_Dr["Other3"]),
                                                        Convert.ToDecimal(_Dr["TotalSpend"]),
                                                        Convert.ToDecimal(_Dr["TotalRebate"]),
                                                        pUser, pTerminal).FirstOrDefault().Value;

                    if (_rsltval)
                        _recordcount++;
                }

                if (_recordcount > 0)
                {
                    _retval = GetImportDataLsit(" and DataYear = '" + pDataYear + "' and DataMonth = '" + pDataMonth + "'");
                    _rsltval = true;
                }
                return _retval;
            }
            catch (Exception ex)
            {
                DeleteImportData(pDataYear, pDataMonth);
                throw ex;
            }
        }

        public List<sp_ImportData_SelectWhere_Result> GetImportDataLsit(string _Condtion)
        {
            List<sp_ImportData_SelectWhere_Result> _ObjImpData = new List<sp_ImportData_SelectWhere_Result>();
            try
            {
                _ObjImpData = _cnn.sp_ImportData_SelectWhere(_Condtion).ToList();
                return _ObjImpData;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable ExcelToDatatable(string FilePath)
        {
            DataTable _Dt = new DataTable();
            try
            {
                string FileExtention = FilePath.Substring(FilePath.LastIndexOf('.') + 1, (FilePath.Length - FilePath.LastIndexOf('.')) - 1);
                string _CnnStr = string.Empty;
                if (FileExtention.ToLower() == "xls")
                {
                    _CnnStr = ConfigurationManager.ConnectionStrings["ConnectionForxls"].ConnectionString;
                }
                else if (FileExtention.ToLower() == "xlsx")
                {
                    _CnnStr = ConfigurationManager.ConnectionStrings["ConnectionForxlsx"].ConnectionString;
                }

                _CnnStr = string.Format(_CnnStr, FilePath);
                OleDbConnection _Con = new OleDbConnection(_CnnStr);
                OleDbCommand _Cmd = new OleDbCommand();
                OleDbDataAdapter _Da = new OleDbDataAdapter();

                // get name of first sheet
                _Con.Open();
                DataTable _DtExcelShema;
                _DtExcelShema = _Con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = _DtExcelShema.Rows[0]["TABLE_NAME"].ToString();
                _Con.Close();

                //read data from first sheet
                _Con.Open();
                _Cmd.CommandText = "Select *  From [" + SheetName + "]";
                _Cmd.Connection = _Con;
                _Da.SelectCommand = _Cmd;
                _Da.Fill(_Dt);
                _Con.Close();

                return _Dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool SaveProcessData(string pDataYear, string pDataMonth, int pUser, string pTerminal)
        {
            bool _retval = false;
            try
            {
                _retval = _cnn.sp_ProcessData_Save(pDataYear, pDataMonth, pUser, pTerminal).FirstOrDefault().Value;
                return _retval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteImportData(string pDataYear, string pDataMonth)
        {
            try
            {
                return _cnn.sp_ImportData_Delete(pDataYear, pDataMonth).FirstOrDefault().Value;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}