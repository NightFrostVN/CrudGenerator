using System.Collections.Generic;
using CrudGenerator.Model;

namespace CrudGenerator.Utility
{
    public static class CrudUtilities
    {
        /// <summary>
        /// List các bảng.
        /// </summary>
        public static List<Table> LIST_TABLE = new List<Table>();

        /// <summary>
        /// List các cột id tự tăng.
        /// </summary>
        public static List<TableColumn> LIST_IDENTITY_COLUMN = new List<TableColumn>();

        /// <summary>
        /// List các bảng không generate.
        /// </summary>
        public static List<string> LIST_EXCLUDE_TABLE_NAME = new List<string>()
        {
            "sysdiagrams"
        };

        /// <summary>
        /// List các kiểu dữ liệu không cho vào param khi read, do các kiểu dữ liệu này không hỗ trợ câu lệnh like trong SQL.
        /// </summary>
        public static List<string> LIST_EXCLUDE_READ_PARAM_COLUMN_DATA_TYPE = new List<string>()
        {
            "text",
            "ntext"
        };

        /// <summary>
        /// Giá trị cột Status với những bản ghi đã bị xóa
        /// </summary>
        public static string DELETE_STATUS = "-1";

        /// <summary>
        /// Kiểm tra xem có phải cột id tự tăng không.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool IsIdentityColumn(TableColumn column)
        {
            for (int i = 0; i < LIST_IDENTITY_COLUMN.Count; i++)
            {
                if (LIST_IDENTITY_COLUMN[i].TableName == column.TableName && LIST_IDENTITY_COLUMN[i].ColumnName == column.ColumnName)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Kiểm tra xem cột có phải kiểu dữ liệu ký tự không.
        /// Trả về true nếu kiểu dữ liệu có chứa cụm từ "char".
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool IsCharDataTypeColumn(TableColumn column)
        {
            if (column.DataType.Contains("char"))
                return true;
            return false;
        }

        /// <summary>
        /// Kiểm tra xem cột có phải kiểu dữ liệu float không.
        /// Trả về true nếu kiểu dữ liệu có chứa cụm từ "float".
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool IsFloatDataTypeColumn(TableColumn column)
        {
            if (column.DataType.Contains("float"))
                return true;
            return false;
        }

        /// <summary>
        /// Kiểm tra xem cột có phải kiểu dữ liệu int không.
        /// Trả về true nếu kiểu dữ liệu có chứa cụm từ "int".
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool IsIntDataTypeColumn(TableColumn column)
        {
            if (column.DataType.Contains("int"))
                return true;
            return false;
        }

        /// <summary>
        /// Kiểm tra xem cột có phải kiểu dữ liệu ngày giờ không.
        /// Trả vể true nếu kiểu dữ liệu là "date" hoặc "datetime".
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool IsDateDataTypeColumn(TableColumn column)
        {
            if (column.DataType.Equals("date") || column.DataType.Equals("datetime"))
                return true;
            return false;
        }

        /// <summary>
        /// Kiểm tra xem cột có giới hạn số lượng ký tự không.
        /// Trả về true nếu cột có đặt MaxLenght và kiểu dữ liệu khác "ntext".
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool IsMaxLengthColumn(TableColumn column)
        {
            if (!string.IsNullOrWhiteSpace(column.MaxLength) && !column.DataType.Equals("ntext"))
                return true;
            return false;
        }

        /// <summary>
        /// Kiểm tra xem cột có phải là cột lưu Status không.
        /// Trả về true nếu tên cột là "status" và kiểu dữ liệu là "int".
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static bool IsStatusColumn(TableColumn column)
        {
            if (column.ColumnName.ToLower().Equals("status") && column.DataType.Equals("int"))
                return true;
            return false;
        }
    }
}
