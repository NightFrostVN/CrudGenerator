using CrudCoreSystem;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.DataManipulation
{
    /// <summary>
    /// Class lưu các hàm CRUD bảng Website.
    /// KHÔNG ĐƯỢC SỬA HOẶC KHAI BÁO THÊM HÀM TRONG CLASS NÀY.
    /// </summary>
    public class WebsiteDataManipulation : CrudDataAccess
    {
        protected WebsiteDataManipulation()
        {
            identityColumnName = "Id";
        }

        public void Create(Website _objModel)
        {
            objModel = _objModel;
            CrudCreate();
        }

        public List<Website> Read(Website _objModel)
        {
            objModel = _objModel;
            CrudRead();
            List<Website> returnList = new List<Website>();
            foreach (DataRow dr in returnDataTable.Rows)
            {
                Website model = new Website();
                if (!string.IsNullOrWhiteSpace(dr["Id"].ToString())) model.Id = Convert.ToInt32(dr["Id"]); else model.Id = null;
                if (!string.IsNullOrWhiteSpace(dr["Name"].ToString())) model.Name = dr["Name"].ToString(); else model.Name = null;
                if (!string.IsNullOrWhiteSpace(dr["Description"].ToString())) model.Description = dr["Description"].ToString(); else model.Description = null;
                if (!string.IsNullOrWhiteSpace(dr["CreatedBy"].ToString())) model.CreatedBy = dr["CreatedBy"].ToString(); else model.CreatedBy = null;
                if (!string.IsNullOrWhiteSpace(dr["CreatedDate"].ToString())) model.CreatedDate = DateTime.Parse(dr["CreatedDate"].ToString()); else model.CreatedDate = null;
                if (!string.IsNullOrWhiteSpace(dr["ModifiedBy"].ToString())) model.ModifiedBy = dr["ModifiedBy"].ToString(); else model.ModifiedBy = null;
                if (!string.IsNullOrWhiteSpace(dr["ModifiedDate"].ToString())) model.ModifiedDate = DateTime.Parse(dr["ModifiedDate"].ToString()); else model.ModifiedDate = null;
                if (!string.IsNullOrWhiteSpace(dr["Status"].ToString())) model.Status = Convert.ToInt32(dr["Status"]); else model.Status = null;
                returnList.Add(model);
            }
            return returnList;
        }

        public void Update(Website _objModel)
        {
            objModel = _objModel;
            CrudUpdate();
        }

        public void Delete(Website _objModel)
        {
            objModel = _objModel;
            CrudDelete();
        }
    }
}

