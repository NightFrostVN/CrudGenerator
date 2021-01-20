using System;

namespace CrudCoreSystem
{
    /// <summary>
    /// Class lưu các attribute cho class model.
    /// KHÔNG ĐƯỢC SỬA HOẶC KHAI BÁO THÊM HÀM TRONG CLASS NÀY.
    /// </summary>
    public class CustomAttribute
    {
        /// <summary>
        /// Dùng cho các trường có kiểu Id tự tăng.
        /// </summary>
        public class IdentityField : Attribute { }

        /// <summary>
        /// Dùng cho các trường kiểu DateTime để search theo kiểu từ ngày đến ngày.
        /// </summary>
        public class DateSearchField : Attribute { }
    }
}
