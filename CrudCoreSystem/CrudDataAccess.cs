using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace CrudCoreSystem
{
    /// <summary>
    /// Class lưu các hàm thực hiện CRUD chung.
    /// KHÔNG ĐƯỢC SỬA HOẶC KHAI BÁO THÊM HÀM TRONG CLASS NÀY.
    /// </summary>
    public class CrudDataAccess
    {
        /// <summary>
        /// List các tên cột không cho vào param khi read
        /// </summary>
        private List<string> LIST_EXCLUDE_READ_PARAM_COLUMN_NAME = new List<string>()
        {
            "CreatedBy",
            "ModifiedBy",
            "CreatedDate",
            "ModifiedDate"
        };

        private int returnCode;
        private string returnMess;
        private int returnData;
        private SqlConnection conn;

        protected DataTable returnDataTable = null;
        protected object objModel;
        protected string identityColumnName;

        #region "Properties"
        public int ReturnCode
        {
            get
            {
                return returnCode;
            }

            set
            {
                returnCode = value;
            }
        }

        public string ReturnMess
        {
            get
            {
                return returnMess;
            }

            set
            {
                returnMess = value;
            }
        }

        public int ReturnData
        {
            get
            {
                return returnData;
            }

            set
            {
                returnData = value;
            }
        }
        #endregion

        protected CrudDataAccess() { }

        public void SetupConnection(string connectionString)
        {
            conn = new SqlConnection(connectionString);
        }

        protected void Create()
        {
            ModifyData("CRUD_" + objModel.GetType().Name + "_Create");
        }

        protected void Delete()
        {
            ModifyData("CRUD_" + objModel.GetType().Name + "_Delete", true);
        }

        protected void Read()
        {
            ReadData("CRUD_" + objModel.GetType().Name + "_Read");
        }

        protected void Update()
        {
            ModifyData("CRUD_" + objModel.GetType().Name + "_Update");
        }

        protected void ExecuteProcedure(string procedureName, List<SqlParameter> listParam, bool isReturnDataTable)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(procedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter param in listParam)
                {
                    cmd.Parameters.Add(param);
                }
                if (!isReturnDataTable)
                {
                    cmd.ExecuteNonQuery();
                    foreach (SqlParameter param in cmd.Parameters)
                    {
                        if (param.ParameterName.Equals("@ReturnCode"))
                            returnCode = string.IsNullOrWhiteSpace(param.Value.ToString()) ? -1 : Convert.ToInt32(param.Value.ToString());
                        if (param.ParameterName.Equals("@ReturnMess"))
                            returnMess = param.Value.ToString();
                        if (param.ParameterName.Equals("@ReturnData"))
                            returnData = string.IsNullOrWhiteSpace(param.Value.ToString()) ? -1 : Convert.ToInt32(param.Value.ToString());
                    }
                }
                else
                {
                    returnDataTable = null;
                    returnDataTable = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(returnDataTable);
                    da.Dispose();

                    returnCode = 0;
                    returnMess = "success";
                    returnData = -1;
                }
            }
            catch (Exception e1)
            {
                returnCode = -1;
                returnMess = e1.ToString();
                returnData = -1;
            }
            finally
            {
                conn.Close();
            }
        }

        void ModifyData(string procedureName, bool isDelete = false)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(procedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                PropertyInfo[] arrObjectInfo = objModel.GetType().GetProperties();
                //param input
                foreach (var info in arrObjectInfo)
                {
                    if (!isDelete)
                    {
                        SqlParameter param = new SqlParameter();
                        param.ParameterName = "@" + info.Name;
                        var paramValue = (object)DBNull.Value;
                        if (info.GetValue(objModel) != null)
                        {
                            paramValue = info.GetValue(objModel);
                        }
                        param.Value = paramValue;
                        cmd.Parameters.Add(param);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(identityColumnName))
                        {
                            returnCode = -1;
                            returnMess = "Identity column not found!";
                            returnData = -1;
                            return;
                        }
                        if (info.Name.Equals(identityColumnName))
                        {
                            SqlParameter param = new SqlParameter();
                            param.ParameterName = "@" + info.Name;
                            var paramValue = (object)DBNull.Value;
                            if (info.GetValue(objModel) != null)
                            {
                                paramValue = info.GetValue(objModel);
                            }
                            param.Value = paramValue;
                            cmd.Parameters.Add(param);
                            break;
                        }
                    }
                }

                //param output
                SqlParameter paramReturnCode;
                SqlParameter paramReturnMess;
                SqlParameter paramReturnData;
                paramReturnData = cmd.Parameters.Add("@ReturnData", SqlDbType.Int);
                paramReturnData.Direction = ParameterDirection.Output;
                paramReturnCode = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
                paramReturnCode.Direction = ParameterDirection.Output;
                paramReturnMess = cmd.Parameters.Add("@ReturnMess", SqlDbType.NVarChar, 500);
                paramReturnMess.Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                foreach (SqlParameter param in cmd.Parameters)
                {
                    if (param.ParameterName.Equals("@ReturnCode"))
                        returnCode = string.IsNullOrWhiteSpace(param.Value.ToString()) ? -1 : Convert.ToInt32(param.Value.ToString());
                    if (param.ParameterName.Equals("@ReturnMess"))
                        returnMess = param.Value.ToString();
                    if (param.ParameterName.Equals("@ReturnData"))
                        returnData = string.IsNullOrWhiteSpace(param.Value.ToString()) ? -1 : Convert.ToInt32(param.Value.ToString());
                }
            }
            catch (Exception e1)
            {
                returnCode = -1;
                returnMess = e1.ToString();
                returnData = -1;
            }
            finally
            {
                conn.Close();
            }
        }

        void ReadData(string procedureName)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(procedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                PropertyInfo[] arrObjectInfo = objModel.GetType().GetProperties();
                //param input
                foreach (var info in arrObjectInfo)
                {
                    if (LIST_EXCLUDE_READ_PARAM_COLUMN_NAME.Contains(info.Name))
                        continue;
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@" + info.Name;
                    var value = (object)DBNull.Value;
                    if (info.GetValue(objModel) != null)
                        value = info.GetValue(objModel);
                    param.Value = value;
                    cmd.Parameters.Add(param);
                }
                returnDataTable = null;
                returnDataTable = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(returnDataTable);
                da.Dispose();

                returnCode = 0;
                returnMess = "success";
                returnData = -1;
            }
            catch (Exception e1)
            {
                returnCode = -1;
                returnMess = e1.ToString();
                returnData = -1;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
