using CrudGenerator.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudGenerator.Utility
{
    public static class ClassUtilities
    {
        private static string ROOT_PATH = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        private static string DATA_ACCESS_FOLDER_NAME = "DataAccess";
        private static string DATA_MANIPULATION_FOLDER_NAME = "DataManipulation";
        private static string REPOSITORY_FOLDER_NAME = "DataRepository";
        private static string MODEL_FOLDER_NAME = "DataModel";
        private static string DATABASE_CONFIG_FOLDER_NAME = "DatabaseConfig";

        /// <summary>
        /// Tạo class lưu chuỗi kết nối.
        /// </summary>
        /// <param name="connectionString"></param>
        public static void GenerateConnection(string connectionString)
        {
            Directory.CreateDirectory(ROOT_PATH + "\\" + DATA_ACCESS_FOLDER_NAME + "\\" + DATABASE_CONFIG_FOLDER_NAME);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace " + DATA_ACCESS_FOLDER_NAME + "." + DATABASE_CONFIG_FOLDER_NAME);
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// Class lưu chuỗi kết nối DB.");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine("    public class DatabaseConnectionString");
            sb.AppendLine("    {");
            sb.AppendLine("        public static string CONNECTION_STRING = \"" + connectionString + "\";");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            using (StreamWriter file = new StreamWriter(ROOT_PATH + "\\" + DATA_ACCESS_FOLDER_NAME + "\\" + DATABASE_CONFIG_FOLDER_NAME + "\\DatabaseConnectionString.cs"))
            {
                file.WriteLine(sb.ToString());
            }
            sb.Clear();
        }

        /// <summary>
        /// Tạo các class Model, lưu trong thư mục MODEL_FOLDER_NAME.
        /// </summary>
        public static void GenerateModel()
        {
            Directory.CreateDirectory(ROOT_PATH + "\\" + DATA_ACCESS_FOLDER_NAME + "\\" + MODEL_FOLDER_NAME);
            StringBuilder sb = new StringBuilder();
            foreach (Table table in CrudUtilities.LIST_TABLE.Where(m => m.Active == true))
            {
                string modelName = table.TableName;
                sb.AppendLine("using System;");
                sb.AppendLine("using static CrudCoreSystem.CustomAttribute;");
                sb.AppendLine("");
                sb.AppendLine("namespace " + DATA_ACCESS_FOLDER_NAME + "." + MODEL_FOLDER_NAME);
                sb.AppendLine("{");
                sb.AppendLine("    public class " + modelName);
                sb.AppendLine("    {");
                foreach (TableColumn column in table.TableColumn)
                {
                    if (CrudUtilities.IsIdentityColumn(column))
                    {
                        sb.AppendLine("        [IdentityField]");
                        sb.AppendLine("        public int? " + column.ColumnName + " { get; set; }");
                    }
                    else if (CrudUtilities.IsIntDataTypeColumn(column))
                        sb.AppendLine("        public int? " + column.ColumnName + " { get; set; }");
                    else if (CrudUtilities.IsCharDataTypeColumn(column))
                        sb.AppendLine("        public string " + column.ColumnName + " { get; set; }");
                    else if (CrudUtilities.IsDateDataTypeColumn(column))
                    {
                        sb.AppendLine("        public DateTime? " + column.ColumnName + " { get; set; }");
                        sb.AppendLine("        [DateSearchField]");
                        sb.AppendLine("        public DateTime? " + column.ColumnName + "From { get; set; }");
                        sb.AppendLine("        [DateSearchField]");
                        sb.AppendLine("        public DateTime? " + column.ColumnName + "To { get; set; }");
                    }
                    else if (CrudUtilities.IsFloatDataTypeColumn(column))
                        sb.AppendLine("        public float? " + column.ColumnName + " { get; set; }");
                }
                sb.AppendLine("    }");
                sb.AppendLine("}");
                using (StreamWriter file = new StreamWriter(ROOT_PATH + "\\" + DATA_ACCESS_FOLDER_NAME + "\\" + MODEL_FOLDER_NAME + "\\" + modelName + ".cs"))
                {
                    file.WriteLine(sb.ToString());
                }
                sb.Clear();
            }
        }

        /// <summary>
        /// Tạo các class DataManipulation. Lưu trong thư mục DATA_MANIPULATION_FOLDER_NAME.
        /// Những bảng nào không có id tự tăng thì sẽ không có hàm Update và Delete.
        /// </summary>
        public static void GenerateManipulation()
        {
            Directory.CreateDirectory(ROOT_PATH + "\\" + DATA_ACCESS_FOLDER_NAME + "\\" + DATA_MANIPULATION_FOLDER_NAME);
            StringBuilder sb = new StringBuilder();
            foreach (Table table in CrudUtilities.LIST_TABLE.Where(m => m.Active == true))
            {
                string modelName = table.TableName;
                string identityColumnName = "";
                foreach (TableColumn column in table.TableColumn)
                {
                    if (CrudUtilities.IsIdentityColumn(column))
                        identityColumnName = column.ColumnName;
                }
                sb.AppendLine("using CrudCoreSystem;");
                sb.AppendLine("using " + DATA_ACCESS_FOLDER_NAME + "." + MODEL_FOLDER_NAME + ";");
                sb.AppendLine("using " + DATA_ACCESS_FOLDER_NAME + "." + DATABASE_CONFIG_FOLDER_NAME + ";");
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.Data;");
                sb.AppendLine("using System.Data.SqlClient;");
                sb.AppendLine("");
                sb.AppendLine("namespace " + DATA_ACCESS_FOLDER_NAME + "." + DATA_MANIPULATION_FOLDER_NAME );
                sb.AppendLine("{");
                sb.AppendLine("    /// <summary>");
                sb.AppendLine("    /// Class lưu các hàm CRUD bảng " + modelName + ".");
                sb.AppendLine("    /// KHÔNG ĐƯỢC SỬA HOẶC KHAI BÁO THÊM HÀM TRONG CLASS NÀY.");
                sb.AppendLine("    /// </summary>");
                sb.AppendLine("    public class " + modelName + DATA_MANIPULATION_FOLDER_NAME + " : " + "CrudDataAccess");
                sb.AppendLine("    {");
                sb.AppendLine("        protected " + modelName + DATA_MANIPULATION_FOLDER_NAME + "()");
                sb.AppendLine("        {");
                sb.AppendLine("            SetupConnection(DatabaseConnectionString.CONNECTION_STRING);");
                sb.AppendLine("        }");
                sb.AppendLine("");
                sb.AppendLine("        public void Create(" + modelName + " _objModel)");
                sb.AppendLine("        {");
                sb.AppendLine("            ObjModel = _objModel;");
                sb.AppendLine("            Create();");
                sb.AppendLine("        }");
                sb.AppendLine("");
                sb.AppendLine("        public List<" + modelName + "> Read(" + modelName + " _objModel)");
                sb.AppendLine("        {");
                sb.AppendLine("            ObjModel = _objModel;");
                sb.AppendLine("            Read();");
                sb.AppendLine("            List<" + modelName + "> returnList = new List<" + modelName + ">();");
                sb.AppendLine("            foreach (DataRow dr in ReturnDataTable.Rows)");
                sb.AppendLine("            {");
                sb.AppendLine("                " + modelName + " model = new " + modelName + "();");
                foreach (TableColumn column in table.TableColumn)
                {
                    string columnName = column.ColumnName;
                    if (CrudUtilities.IsIntDataTypeColumn(column))
                        sb.AppendLine("                if (!string.IsNullOrWhiteSpace(dr[\"" + columnName + "\"].ToString())) model." + columnName + " = Convert.ToInt32(dr[\"" + columnName + "\"]); else model." + columnName + " = null;");
                    else if (CrudUtilities.IsCharDataTypeColumn(column))
                        sb.AppendLine("                if (!string.IsNullOrWhiteSpace(dr[\"" + columnName + "\"].ToString())) model." + columnName + " = dr[\"" + columnName + "\"].ToString(); else model." + columnName + " = null;");
                    else if (CrudUtilities.IsDateDataTypeColumn(column))
                        sb.AppendLine("                if (!string.IsNullOrWhiteSpace(dr[\"" + columnName + "\"].ToString())) model." + columnName + " = DateTime.Parse(dr[\"" + columnName + "\"].ToString()); else model." + columnName + " = null;");
                    if (CrudUtilities.IsFloatDataTypeColumn(column))
                        sb.AppendLine("                if (!string.IsNullOrWhiteSpace(dr[\"" + columnName + "\"].ToString())) model." + columnName + " = float.Parse(dr[\"" + columnName + "\"].ToString()); else model." + columnName + " = null;");
                }
                sb.AppendLine("                returnList.Add(model);");
                sb.AppendLine("            }");
                sb.AppendLine("            return returnList;");
                sb.AppendLine("        }");
                sb.AppendLine("");
                if (!string.IsNullOrWhiteSpace(identityColumnName))
                {
                    sb.AppendLine("        public void Update(" + modelName + " _objModel)");
                    sb.AppendLine("        {");
                    sb.AppendLine("            ObjModel = _objModel;");
                    sb.AppendLine("            Update();");
                    sb.AppendLine("        }");
                    sb.AppendLine("");
                    sb.AppendLine("        public void Delete(" + modelName + " _objModel)");
                    sb.AppendLine("        {");
                    sb.AppendLine("            ObjModel = _objModel;");
                    sb.AppendLine("            Delete();");
                    sb.AppendLine("        }");
                    sb.AppendLine("");
                }
                sb.AppendLine("        public new void ExecuteProcedure(string procedureName, List<SqlParameter> listParam, bool isReturnDataTable)");
                sb.AppendLine("        {");
                sb.AppendLine("            base.ExecuteProcedure(procedureName, listParam, isReturnDataTable);");
                sb.AppendLine("        }");
                sb.AppendLine("    }");
                sb.AppendLine("}");
                using (StreamWriter file = new StreamWriter(ROOT_PATH + "\\" + DATA_ACCESS_FOLDER_NAME + "\\" + DATA_MANIPULATION_FOLDER_NAME + "\\" + modelName + DATA_MANIPULATION_FOLDER_NAME + ".cs"))
                {
                    file.WriteLine(sb.ToString());
                }
                sb.Clear();
            }
        }

        /// <summary>
        /// Tạo các class Repository. Lưu trong thư mục REPOSITORY_FOLDER_NAME.
        /// </summary>
        public static void GenerateRepository()
        {
            Directory.CreateDirectory(ROOT_PATH + "\\" + DATA_ACCESS_FOLDER_NAME + "\\" + REPOSITORY_FOLDER_NAME);
            StringBuilder sb = new StringBuilder();
            foreach (Table table in CrudUtilities.LIST_TABLE.Where(m => m.Active == true))
            {
                string modelName = table.TableName;
                sb.AppendLine("using " + DATA_ACCESS_FOLDER_NAME + "." + DATA_MANIPULATION_FOLDER_NAME + ";");
                sb.AppendLine("");
                sb.AppendLine("namespace " + DATA_ACCESS_FOLDER_NAME + "." + REPOSITORY_FOLDER_NAME);
                sb.AppendLine("{");
                sb.AppendLine("    /// <summary>");
                sb.AppendLine("    /// Class lưu các phương thức thao tác dữ liệu khác của bảng " + modelName + ".");
                sb.AppendLine("    /// </summary>");
                sb.AppendLine("    public class " + modelName + REPOSITORY_FOLDER_NAME + " : " + modelName + DATA_MANIPULATION_FOLDER_NAME);
                sb.AppendLine("    {");
                sb.AppendLine("");
                sb.AppendLine("    }");
                sb.AppendLine("}");
                using (StreamWriter file = new StreamWriter(ROOT_PATH + "\\" + DATA_ACCESS_FOLDER_NAME + "\\" + REPOSITORY_FOLDER_NAME + "\\" + modelName + REPOSITORY_FOLDER_NAME + ".cs"))
                {
                    file.WriteLine(sb.ToString());
                }
                sb.Clear();
            }
        }
    }
}
