using CrudCoreSystem;
using DataAccess.DataModel;
using DataAccess.DatabaseConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.DataManipulation
{
    /// <summary>
    /// Class lưu các hàm CRUD bảng Temp.
    /// KHÔNG ĐƯỢC SỬA HOẶC KHAI BÁO THÊM HÀM TRONG CLASS NÀY.
    /// </summary>
    public class TempDataManipulation : CrudDataAccess
    {
        protected TempDataManipulation()
        {
            SetupConnection(DatabaseConnectionString.CONNECTION_STRING);
            identityColumnName = "";
        }

        public void Create(Temp _objModel)
        {
            objModel = _objModel;
            Create();
        }

        public List<Temp> Read(Temp _objModel)
        {
            objModel = _objModel;
            Read();
            List<Temp> returnList = new List<Temp>();
            foreach (DataRow dr in returnDataTable.Rows)
            {
                Temp model = new Temp();
                if (!string.IsNullOrWhiteSpace(dr["Name"].ToString())) model.Name = dr["Name"].ToString(); else model.Name = null;
                if (!string.IsNullOrWhiteSpace(dr["Status"].ToString())) model.Status = dr["Status"].ToString(); else model.Status = null;
                returnList.Add(model);
            }
            return returnList;
        }

    }
}

