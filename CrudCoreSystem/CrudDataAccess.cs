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

        private SqlConnection conn;

        #region "Properties"
        public object ObjModel { private get; set; }
        public DataTable ReturnDataTable { get; private set; }
        public string IdentityColumnName { get; set; }
        public int ReturnCode { get; private set; }
        public string ReturnMess { get; private set; }
        public int ReturnData { get; private set; }
        #endregion

        protected CrudDataAccess() { }

        public void SetupConnection(string connectionString)
        {
            conn = new SqlConnection(connectionString);
        }

        protected void Create()
        {
            ModifyData("CRUD_" + ObjModel.GetType().Name + "_Create");
        }

        protected void Delete()
        {
            ModifyData("CRUD_" + ObjModel.GetType().Name + "_Delete", true);
        }

        protected void Read()
        {
            ReadData("CRUD_" + ObjModel.GetType().Name + "_Read");
        }

        protected void Update()
        {
            ModifyData("CRUD_" + ObjModel.GetType().Name + "_Update");
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
                            ReturnCode = string.IsNullOrWhiteSpace(param.Value.ToString()) ? -1 : Convert.ToInt32(param.Value.ToString());
                        if (param.ParameterName.Equals("@ReturnMess"))
                            ReturnMess = param.Value.ToString();
                        if (param.ParameterName.Equals("@ReturnData"))
                            ReturnData = string.IsNullOrWhiteSpace(param.Value.ToString()) ? -1 : Convert.ToInt32(param.Value.ToString());
                    }
                }
                else
                {
                    ReturnDataTable = null;
                    ReturnDataTable = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ReturnDataTable);
                    da.Dispose();

                    ReturnCode = 0;
                    ReturnMess = "success";
                    ReturnData = -1;
                }
            }
            catch (Exception e1)
            {
                ReturnCode = -1;
                ReturnMess = e1.ToString();
                ReturnData = -1;
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
                PropertyInfo[] arrObjectInfo = ObjModel.GetType().GetProperties();
                //param input
                foreach (var info in arrObjectInfo)
                {
                    if (!isDelete)
                    {
                        SqlParameter param = new SqlParameter();
                        param.ParameterName = "@" + info.Name;
                        var paramValue = (object)DBNull.Value;
                        if (info.GetValue(ObjModel) != null)
                        {
                            paramValue = info.GetValue(ObjModel);
                        }
                        param.Value = paramValue;
                        cmd.Parameters.Add(param);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(IdentityColumnName))
                        {
                            ReturnCode = -1;
                            ReturnMess = "Identity column not found!";
                            ReturnData = -1;
                            return;
                        }
                        if (info.Name.Equals(IdentityColumnName))
                        {
                            SqlParameter param = new SqlParameter();
                            param.ParameterName = "@" + info.Name;
                            var paramValue = (object)DBNull.Value;
                            if (info.GetValue(ObjModel) != null)
                            {
                                paramValue = info.GetValue(ObjModel);
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
                        ReturnCode = string.IsNullOrWhiteSpace(param.Value.ToString()) ? -1 : Convert.ToInt32(param.Value.ToString());
                    if (param.ParameterName.Equals("@ReturnMess"))
                        ReturnMess = param.Value.ToString();
                    if (param.ParameterName.Equals("@ReturnData"))
                        ReturnData = string.IsNullOrWhiteSpace(param.Value.ToString()) ? -1 : Convert.ToInt32(param.Value.ToString());
                }
            }
            catch (Exception e1)
            {
                ReturnCode = -1;
                ReturnMess = e1.ToString();
                ReturnData = -1;
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
                PropertyInfo[] arrObjectInfo = ObjModel.GetType().GetProperties();
                //param input
                foreach (var info in arrObjectInfo)
                {
                    if (LIST_EXCLUDE_READ_PARAM_COLUMN_NAME.Contains(info.Name))
                        continue;
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@" + info.Name;
                    var value = (object)DBNull.Value;
                    if (info.GetValue(ObjModel) != null)
                        value = info.GetValue(ObjModel);
                    param.Value = value;
                    cmd.Parameters.Add(param);
                }
                ReturnDataTable = null;
                ReturnDataTable = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ReturnDataTable);
                da.Dispose();

                ReturnCode = 0;
                ReturnMess = "success";
                ReturnData = -1;
            }
            catch (Exception e1)
            {
                ReturnCode = -1;
                ReturnMess = e1.ToString();
                ReturnData = -1;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
