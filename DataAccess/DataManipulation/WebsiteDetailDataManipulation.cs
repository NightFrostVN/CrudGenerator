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
    /// Class lưu các hàm CRUD bảng WebsiteDetail.
    /// KHÔNG ĐƯỢC SỬA HOẶC KHAI BÁO THÊM HÀM TRONG CLASS NÀY.
    /// </summary>
    public class WebsiteDetailDataManipulation : CrudDataAccess
    {
        protected WebsiteDetailDataManipulation()
        {
            SetupConnection(DatabaseConnectionString.CONNECTION_STRING);
            identityColumnName = "Id";
        }

        public void Create(WebsiteDetail _objModel)
        {
            objModel = _objModel;
            Create();
        }

        public List<WebsiteDetail> Read(WebsiteDetail _objModel)
        {
            objModel = _objModel;
            Read();
            List<WebsiteDetail> returnList = new List<WebsiteDetail>();
            foreach (DataRow dr in returnDataTable.Rows)
            {
                WebsiteDetail model = new WebsiteDetail();
                if (!string.IsNullOrWhiteSpace(dr["Id"].ToString())) model.Id = Convert.ToInt32(dr["Id"]); else model.Id = null;
                if (!string.IsNullOrWhiteSpace(dr["WebsiteId"].ToString())) model.WebsiteId = Convert.ToInt32(dr["WebsiteId"]); else model.WebsiteId = null;
                if (!string.IsNullOrWhiteSpace(dr["Detail"].ToString())) model.Detail = dr["Detail"].ToString(); else model.Detail = null;
                if (!string.IsNullOrWhiteSpace(dr["PublishDate"].ToString())) model.PublishDate = DateTime.Parse(dr["PublishDate"].ToString()); else model.PublishDate = null;
                returnList.Add(model);
            }
            return returnList;
        }

        public void Update(WebsiteDetail _objModel)
        {
            objModel = _objModel;
            Update();
        }

        public void Delete(WebsiteDetail _objModel)
        {
            objModel = _objModel;
            Delete();
        }
        public new void ExecuteProcedure(string procedureName, List<SqlParameter> listParam, bool isReturnDataTable)
        {
            base.ExecuteProcedure(procedureName, listParam, isReturnDataTable);
        }
    }
}

