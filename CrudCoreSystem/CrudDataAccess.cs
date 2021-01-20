using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using static CrudCoreSystem.CustomAttribute;

namespace CrudCoreSystem
{
    /// <summary>
    /// Class lưu các hàm thực hiện CRUD chung.
    /// KHÔNG ĐƯỢC SỬA HOẶC KHAI BÁO THÊM HÀM TRONG CLASS NÀY.
    /// </summary>
    public class CrudDataAccess : ICrudReturnData
    {
        private SqlConnection conn;

        #region "Properties"
        public object ObjModel { private get; set; }
        public DataTable ReturnDataTable { get; private set; }
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
                if (listParam != null)
                {
                    foreach (SqlParameter param in listParam)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                if (!isReturnDataTable)
                {
                    cmd.ExecuteNonQuery();
                    if (listParam != null)
                    {
                        foreach (SqlParameter param in cmd.Parameters)
                        {
                            if (param.ParameterName.Equals(CrudConstant.RETURN_CODE_PARAM_NAME))
                                ReturnCode = string.IsNullOrWhiteSpace(param.Value.ToString()) ? CrudConstant.RETURN_DATA_DEFAULT : Convert.ToInt32(param.Value.ToString());
                            if (param.ParameterName.Equals(CrudConstant.RETURN_MESS_PARAM_NAME))
                                ReturnMess = param.Value.ToString();
                            if (param.ParameterName.Equals(CrudConstant.RETURN_DATA_PARAM_NAME))
                                ReturnData = string.IsNullOrWhiteSpace(param.Value.ToString()) ? CrudConstant.RETURN_DATA_DEFAULT : Convert.ToInt32(param.Value.ToString());
                        }
                    }
                }
                else
                {
                    ReturnDataTable = null;
                    ReturnDataTable = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ReturnDataTable);
                    da.Dispose();

                    ReturnCode = CrudConstant.RETURN_CODE_SUCCESS;
                    ReturnMess = CrudConstant.RETURN_MESS_SUCCESS;
                    ReturnData = CrudConstant.RETURN_DATA_DEFAULT;
                }
            }
            catch (Exception e1)
            {
                ReturnCode = CrudConstant.RETURN_CODE_FAIL;
                ReturnMess = e1.ToString();
                ReturnData = CrudConstant.RETURN_DATA_DEFAULT;
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
                    if (Attribute.IsDefined(info, typeof(DateSearchField)))
                        continue;
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
                        if (Attribute.IsDefined(info, typeof(IdentityField)))
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
                        ReturnCode = CrudConstant.RETURN_CODE_FAIL;
                        ReturnMess = "Identity column not found!";
                        ReturnData = CrudConstant.RETURN_DATA_DEFAULT;
                        return;
                    }
                }

                //param output
                SqlParameter paramReturnCode;
                SqlParameter paramReturnMess;
                SqlParameter paramReturnData;
                paramReturnData = cmd.Parameters.Add(CrudConstant.RETURN_DATA_PARAM_NAME, SqlDbType.Int);
                paramReturnData.Direction = ParameterDirection.Output;
                paramReturnCode = cmd.Parameters.Add(CrudConstant.RETURN_CODE_PARAM_NAME, SqlDbType.Int);
                paramReturnCode.Direction = ParameterDirection.Output;
                paramReturnMess = cmd.Parameters.Add(CrudConstant.RETURN_MESS_PARAM_NAME, SqlDbType.NVarChar, 500);
                paramReturnMess.Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                foreach (SqlParameter param in cmd.Parameters)
                {
                    if (param.ParameterName.Equals(CrudConstant.RETURN_CODE_PARAM_NAME))
                        ReturnCode = string.IsNullOrWhiteSpace(param.Value.ToString()) ? CrudConstant.RETURN_DATA_DEFAULT : Convert.ToInt32(param.Value.ToString());
                    if (param.ParameterName.Equals(CrudConstant.RETURN_MESS_PARAM_NAME))
                        ReturnMess = param.Value.ToString();
                    if (param.ParameterName.Equals(CrudConstant.RETURN_DATA_PARAM_NAME))
                        ReturnData = string.IsNullOrWhiteSpace(param.Value.ToString()) ? CrudConstant.RETURN_DATA_DEFAULT : Convert.ToInt32(param.Value.ToString());
                }
            }
            catch (Exception e1)
            {
                ReturnCode = CrudConstant.RETURN_CODE_FAIL;
                ReturnMess = e1.ToString();
                ReturnData = CrudConstant.RETURN_DATA_DEFAULT;
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

                ReturnCode = CrudConstant.RETURN_CODE_SUCCESS;
                ReturnMess = CrudConstant.RETURN_MESS_SUCCESS;
                ReturnData = CrudConstant.RETURN_DATA_DEFAULT;
            }
            catch (Exception e1)
            {
                ReturnCode = CrudConstant.RETURN_CODE_FAIL;
                ReturnMess = e1.ToString();
                ReturnData = CrudConstant.RETURN_DATA_DEFAULT;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
