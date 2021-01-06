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
    /// Class lưu các hàm CRUD bảng HoSoThau.
    /// KHÔNG ĐƯỢC SỬA HOẶC KHAI BÁO THÊM HÀM TRONG CLASS NÀY.
    /// </summary>
    public class HoSoThauDataManipulation : CrudDataAccess
    {
        protected HoSoThauDataManipulation()
        {
            SetupConnection(DatabaseConnectionString.CONNECTION_STRING);
            identityColumnName = "HoSoID";
        }

        public void Create(HoSoThau _objModel)
        {
            objModel = _objModel;
            Create();
        }

        public List<HoSoThau> Read(HoSoThau _objModel)
        {
            objModel = _objModel;
            Read();
            List<HoSoThau> returnList = new List<HoSoThau>();
            foreach (DataRow dr in returnDataTable.Rows)
            {
                HoSoThau model = new HoSoThau();
                if (!string.IsNullOrWhiteSpace(dr["HoSoID"].ToString())) model.HoSoID = Convert.ToInt32(dr["HoSoID"]); else model.HoSoID = null;
                if (!string.IsNullOrWhiteSpace(dr["TenGoiThau"].ToString())) model.TenGoiThau = dr["TenGoiThau"].ToString(); else model.TenGoiThau = null;
                if (!string.IsNullOrWhiteSpace(dr["MoTa"].ToString())) model.MoTa = dr["MoTa"].ToString(); else model.MoTa = null;
                if (!string.IsNullOrWhiteSpace(dr["BPQLThauID"].ToString())) model.BPQLThauID = Convert.ToInt32(dr["BPQLThauID"]); else model.BPQLThauID = null;
                if (!string.IsNullOrWhiteSpace(dr["LoaiHinhThauID"].ToString())) model.LoaiHinhThauID = Convert.ToInt32(dr["LoaiHinhThauID"]); else model.LoaiHinhThauID = null;
                if (!string.IsNullOrWhiteSpace(dr["ThoiGianMo"].ToString())) model.ThoiGianMo = DateTime.Parse(dr["ThoiGianMo"].ToString()); else model.ThoiGianMo = null;
                if (!string.IsNullOrWhiteSpace(dr["ThoiGianDong"].ToString())) model.ThoiGianDong = DateTime.Parse(dr["ThoiGianDong"].ToString()); else model.ThoiGianDong = null;
                if (!string.IsNullOrWhiteSpace(dr["IsActive"].ToString())) model.IsActive = Convert.ToInt32(dr["IsActive"]); else model.IsActive = null;
                if (!string.IsNullOrWhiteSpace(dr["ArticleID"].ToString())) model.ArticleID = Convert.ToInt32(dr["ArticleID"]); else model.ArticleID = null;
                if (!string.IsNullOrWhiteSpace(dr["CreatedDate"].ToString())) model.CreatedDate = DateTime.Parse(dr["CreatedDate"].ToString()); else model.CreatedDate = null;
                if (!string.IsNullOrWhiteSpace(dr["CreatedBy"].ToString())) model.CreatedBy = dr["CreatedBy"].ToString(); else model.CreatedBy = null;
                if (!string.IsNullOrWhiteSpace(dr["ModifiedDate"].ToString())) model.ModifiedDate = DateTime.Parse(dr["ModifiedDate"].ToString()); else model.ModifiedDate = null;
                if (!string.IsNullOrWhiteSpace(dr["ModifiedBy"].ToString())) model.ModifiedBy = dr["ModifiedBy"].ToString(); else model.ModifiedBy = null;
                if (!string.IsNullOrWhiteSpace(dr["HanXetThau"].ToString())) model.HanXetThau = float.Parse(dr["HanXetThau"].ToString()); else model.HanXetThau = null;
                if (!string.IsNullOrWhiteSpace(dr["SoLanGiaHan"].ToString())) model.SoLanGiaHan = Convert.ToInt32(dr["SoLanGiaHan"]); else model.SoLanGiaHan = null;
                if (!string.IsNullOrWhiteSpace(dr["IsBaoGia"].ToString())) model.IsBaoGia = Convert.ToInt32(dr["IsBaoGia"]); else model.IsBaoGia = null;
                returnList.Add(model);
            }
            return returnList;
        }

        public void Update(HoSoThau _objModel)
        {
            objModel = _objModel;
            Update();
        }

        public void Delete(HoSoThau _objModel)
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

