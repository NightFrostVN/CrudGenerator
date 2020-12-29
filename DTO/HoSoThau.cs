using System;

namespace DTO
{
    public class HoSoThau
    {
        public int? HoSoID { get; set; }
        public string TenGoiThau { get; set; }
        public string MoTa { get; set; }
        public int? BPQLThauID { get; set; }
        public int? LoaiHinhThauID { get; set; }
        public DateTime? ThoiGianMo { get; set; }
        public DateTime? ThoiGianDong { get; set; }
        public int? IsActive { get; set; }
        public int? Status { get; set; }
        public int? ArticleID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}

