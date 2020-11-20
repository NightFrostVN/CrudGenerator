using CrudCoreSystem;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.DataManipulation
{
    /// <summary>
    /// Class lưu các hàm CRUD bảng WebsiteDetail.
    /// KHÔNG ĐƯỢC SỬA HOẶC KHAI BÁO THÊM HÀM TRONG CLASS NÀY.
    /// </summary>
    public class WebsiteDetailDataManipulation : CrudDataAccess
    {
        protected WebsiteDetailDataManipulation()
        {
            identityColumnName = "Id";
        }

        public void Create(WebsiteDetail _objModel)
        {
            objModel = _objModel;
            CrudCreate();
        }

        public List<WebsiteDetail> Read(WebsiteDetail _objModel)
        {
            objModel = _objModel;
            CrudRead();
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
            CrudUpdate();
        }

        public void Delete(WebsiteDetail _objModel)
        {
            objModel = _objModel;
            CrudDelete();
        }
    }
}

