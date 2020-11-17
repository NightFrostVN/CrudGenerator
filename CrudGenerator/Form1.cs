using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudGenerator
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// List các bảng
        /// </summary>
        List<Table> LIST_TABLE = new List<Table>();
        /// <summary>
        /// List các cột id tự tăng
        /// </summary>
        List<TableColumn> LIST_IDENTITY_COLUMN = new List<TableColumn>();
        /// <summary>
        /// List các bảng không generate
        /// </summary>
        List<string> LIST_EXCLUDE_TABLE_NAME = new List<string>()
        {
            "sysdiagrams"
        };
        /// <summary>
        /// List các tên cột không cho vào param khi read
        /// </summary>
        List<string> LIST_EXCLUDE_READ_PARAM_COLUMN_NAME = new List<string>()
        {
            "CreatedBy",
            "ModifiedBy",
            "CreatedDate",
            "ModifiedDate"
        };
        /// <summary>
        /// List các kiểu dữ liệu không cho vào param khi read
        /// </summary>
        List<string> LIST_EXCLUDE_READ_PARAM_COLUMN_DATA_TYPE = new List<string>()
        {
            "text",
            "ntext"
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConnectionString.Text))
                return;
            SqlConnection sqlConnection = new SqlConnection();
            try
            {
                sqlConnection.ConnectionString = txtConnectionString.Text;
                sqlConnection.Open();
                txtOutput.Clear();
                LIST_TABLE.Clear();
                LIST_IDENTITY_COLUMN.Clear();
                DataTable tbl = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT * FROM sys.Tables order by name", sqlConnection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tbl);
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    if (!LIST_EXCLUDE_TABLE_NAME.Contains(tbl.Rows[i]["name"].ToString()))
                        LIST_TABLE.Add(new Table { TableName = tbl.Rows[i]["name"].ToString() });
                }

                for (int i = 0; i < LIST_TABLE.Count; i++)
                {
                    tbl.Clear();
                    cmd.CommandText = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + LIST_TABLE[i].TableName + "' ORDER BY ORDINAL_POSITION;";
                    da.Fill(tbl);
                    for (int j = 0; j < tbl.Rows.Count; j++)
                    {
                        TableColumn tableColumn = new TableColumn
                        {
                            TableName = LIST_TABLE[i].TableName,
                            ColumnName = tbl.Rows[j]["COLUMN_NAME"].ToString(),
                            IsNullable = tbl.Rows[j]["IS_NULLABLE"].ToString(),
                            DataType = tbl.Rows[j]["DATA_TYPE"].ToString(),
                            MaxLength = tbl.Rows[j]["CHARACTER_MAXIMUM_LENGTH"].ToString().Equals("-1") ? "max" : tbl.Rows[j]["CHARACTER_MAXIMUM_LENGTH"].ToString()
                        };
                        LIST_TABLE[i].TableColumn.Add(tableColumn);
                    }
                }

                tbl.Clear();
                cmd.CommandText = "select COLUMN_NAME, TABLE_NAME from INFORMATION_SCHEMA.COLUMNS where COLUMNPROPERTY(object_id(TABLE_SCHEMA + '.' + TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 order by TABLE_NAME ";
                da.Fill(tbl);
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    LIST_IDENTITY_COLUMN.Add(new TableColumn { ColumnName = tbl.Rows[i]["COLUMN_NAME"].ToString(), TableName = tbl.Rows[i]["TABLE_NAME"].ToString() });
                }
                sqlConnection.Close();

                GenerateScript();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConnection.State != ConnectionState.Closed)
                    sqlConnection.Close();
            }
        }

        void GenerateScript()
        {
            StringBuilder sbCreate = GenerateCreate(LIST_TABLE);
            txtOutput.AppendText("-----CREATE-----" + Environment.NewLine);
            txtOutput.AppendText(sbCreate.ToString());

            StringBuilder sbRead = GenerateRead(LIST_TABLE);
            txtOutput.AppendText("-----READ-----" + Environment.NewLine);
            txtOutput.AppendText(sbRead.ToString());

            StringBuilder sbUpdate = GenerateUpdate(LIST_TABLE);
            txtOutput.AppendText("-----UPDATE-----" + Environment.NewLine);
            txtOutput.AppendText(sbUpdate.ToString());

            StringBuilder sbDelete = GenerateDelete(LIST_TABLE);
            txtOutput.AppendText("-----DELETE-----" + Environment.NewLine);
            txtOutput.AppendText(sbDelete.ToString());
        }

        /// <summary>
        /// Tạo thủ tục SQL Create.
        /// Nếu có cột id tự tăng thủ tục sẽ có param DataReturn trả về id đó.
        /// </summary>
        /// <param name="listTable"></param>
        /// <returns></returns>
        StringBuilder GenerateCreate(List<Table> listTable)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Table table in listTable)
            {
                bool hasIdentityColumn = false;
                string procName = "sp_" + table.TableName + "_Create";
                sb.AppendLine("if object_id('dbo." + procName + "', 'p') is null");
                sb.AppendLine("    exec ('create procedure " + procName + " as select 1')");
                sb.AppendLine("go");
                sb.AppendLine("alter procedure " + procName);
                foreach (TableColumn tableColumn in table.TableColumn)
                {
                    if (!IsIdentityColumn(tableColumn))
                    {
                        if (IsMaxLengthColumn(tableColumn))
                            sb.AppendLine("@" + tableColumn.ColumnName + " " + tableColumn.DataType + "(" + tableColumn.MaxLength + "),");
                        else
                            sb.AppendLine("@" + tableColumn.ColumnName + " " + tableColumn.DataType + ",");
                    }
                    else
                        hasIdentityColumn = true;
                }
                if (hasIdentityColumn)
                    sb.AppendLine("@DataReturn int output,");
                sb.AppendLine("@ReturnCode int output,");
                sb.AppendLine("@ReturnMess nvarchar(500) output");
                sb.AppendLine("as");
                sb.AppendLine("begin transaction;");
                sb.AppendLine("begin try");
                sb.Append("    insert into [" + table.TableName + "](");
                for (int i = 0; i < table.TableColumn.Count; i++)
                {
                    if (!IsIdentityColumn(table.TableColumn[i]))
                    {
                        sb.Append("[" + table.TableColumn[i].ColumnName + "], ");
                    }
                }
                sb.Length--;
                sb.Length--;
                sb.AppendLine(")");
                sb.Append("    values (");
                for (int i = 0; i < table.TableColumn.Count; i++)
                {
                    if (!IsIdentityColumn(table.TableColumn[i]))
                    {
                        sb.Append("@" + table.TableColumn[i].ColumnName + ", ");
                    }
                }
                sb.Length--;
                sb.Length--;
                sb.AppendLine(")");
                sb.AppendLine("end try");
                sb.AppendLine("begin catch");
                if (hasIdentityColumn)
                    sb.AppendLine("    set @DataReturn = -1;");
                sb.AppendLine("    set @ReturnCode = ERROR_NUMBER();");
                sb.AppendLine("    set @ReturnMess = ERROR_MESSAGE();");
                sb.AppendLine("    if @@TRANCOUNT > 0");
                sb.AppendLine("    rollback transaction;");
                sb.AppendLine("    return;");
                sb.AppendLine("end catch;");

                sb.AppendLine("if @@TRANCOUNT > 0");
                sb.AppendLine("commit transaction;");
                if (hasIdentityColumn)
                    sb.AppendLine("set @DataReturn = SCOPE_IDENTITY();");
                sb.AppendLine("set @ReturnCode = 0");
                sb.AppendLine("set @ReturnMess = 'success'");
                sb.AppendLine("go");
                sb.AppendLine();
            }
            return sb;
        }

        /// <summary>
        /// Tạo thủ tục SQL Read.
        /// Các cột thuộc mảng LIST_EXCLUDE_READ_PARAM_COLUMN_NAME sẽ không được truyền vào search parameter.
        /// Nếu bảng có cột Status thì sẽ lấy những bản ghi có Status >= 0
        /// </summary>
        /// <param name="listTable"></param>
        /// <returns></returns>
        StringBuilder GenerateRead(List<Table> listTable)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Table table in listTable)
            {
                string statusColumnName = null;
                Table currentTable = table;
                List<TableColumn> listParam = new List<TableColumn>(table.TableColumn);
                foreach (TableColumn tableColumn in currentTable.TableColumn)
                {
                    if (LIST_EXCLUDE_READ_PARAM_COLUMN_NAME.Contains(tableColumn.ColumnName))
                        listParam.Remove(tableColumn);
                    if (IsStatusColumn(tableColumn))
                        statusColumnName = tableColumn.ColumnName;
                }
                string procName = "sp_" + table.TableName + "_Read";
                sb.AppendLine("if object_id('dbo." + procName + "', 'p') is null");
                sb.AppendLine("    exec ('create procedure " + procName + " as select 1')");
                sb.AppendLine("go");
                sb.AppendLine("alter procedure " + procName);
                for (int i = 0; i < listParam.Count; i++)
                {
                    if (IsMaxLengthColumn(listParam[i]))
                        sb.Append("@" + listParam[i].ColumnName + " " + listParam[i].DataType + "(" + listParam[i].MaxLength + ")");
                    else
                        sb.Append("@" + listParam[i].ColumnName + " " + listParam[i].DataType);

                    if (i < listParam.Count - 1)
                        sb.AppendLine(",");
                    else
                        sb.AppendLine();
                }
                sb.AppendLine("as");
                sb.AppendLine("select * from [" + table.TableName + "] a where ");

                if (!string.IsNullOrWhiteSpace(statusColumnName))
                {
                    sb.Append("    (a.[" + statusColumnName + "] >= 0)");
                    if (listParam.Count > 0)
                        sb.AppendLine(" and");
                }
                for (int i = 0; i < listParam.Count; i++)
                {
                    if (!IsCharDataTypeColumn(listParam[i]))
                        sb.Append("    (@" + listParam[i].ColumnName + " IS NULL OR a.[" + listParam[i].ColumnName + "] = @" + listParam[i].ColumnName + ")");
                    else
                        sb.Append("    (@" + listParam[i].ColumnName + " IS NULL OR a.[" + listParam[i].ColumnName + "] like N'%' + @" + listParam[i].ColumnName + " + '%')");

                    if (i < listParam.Count - 1)
                        sb.AppendLine(" and");
                    else
                        sb.AppendLine();
                }
                sb.Append("--" + procName + " ");
                for (int i = 0; i < listParam.Count; i++)
                {
                    if (i < listParam.Count - 1)
                        sb.Append("null, ");
                    else
                        sb.AppendLine("null");
                }
                sb.AppendLine("go");
                sb.AppendLine();
            }
            return sb;
        }

        /// <summary>
        /// Tạo thủ tục SQL Update.
        /// Id truyền vào là cột có id tự tăng của bảng.
        /// Những bảng nào không có id tự tăng thì sẽ bỏ qua bảng đó.
        /// </summary>
        /// <param name="listTable"></param>
        /// <returns></returns>
        StringBuilder GenerateUpdate(List<Table> listTable)
        {
            StringBuilder returnData = new StringBuilder();
            foreach (Table table in listTable)
            {
                StringBuilder sb = new StringBuilder();
                string identityColumnName = null;
                string procName = "sp_" + table.TableName + "_Update";
                sb.AppendLine("if object_id('dbo." + procName + "', 'p') is null");
                sb.AppendLine("    exec ('create procedure " + procName + " as select 1')");
                sb.AppendLine("go");
                sb.AppendLine("alter procedure " + procName);
                foreach (TableColumn tableColumn in table.TableColumn)
                {
                    if (IsIdentityColumn(tableColumn))
                        identityColumnName = tableColumn.ColumnName;
                    if (IsMaxLengthColumn(tableColumn))
                        sb.AppendLine("@" + tableColumn.ColumnName + " " + tableColumn.DataType + "(" + tableColumn.MaxLength + "),");
                    else
                        sb.AppendLine("@" + tableColumn.ColumnName + " " + tableColumn.DataType + ",");
                }
                //Nếu bảng không có cột nào id tự tăng thì sẽ bỏ qua bảng đó
                if (string.IsNullOrWhiteSpace(identityColumnName))
                    continue;
                sb.AppendLine("@ReturnCode int output,");
                sb.AppendLine("@ReturnMess nvarchar(500) output");
                sb.AppendLine("as");
                sb.AppendLine("begin transaction;");
                sb.AppendLine("begin try");
                sb.AppendLine("    update [" + table.TableName + "]");
                sb.Append("        set ");
                foreach (TableColumn tableColumn in table.TableColumn)
                {
                    if (!IsIdentityColumn(tableColumn))
                        sb.Append("[" + tableColumn.ColumnName + "] = @" + tableColumn.ColumnName + ", ");
                }
                sb.Length--;
                sb.Length--;
                sb.AppendLine();
                sb.AppendLine("    where [" + identityColumnName + "] = @" + identityColumnName + ";");
                sb.AppendLine("end try");
                sb.AppendLine("begin catch");
                sb.AppendLine("    set @ReturnCode = ERROR_NUMBER();");
                sb.AppendLine("    set @ReturnMess = ERROR_MESSAGE();");
                sb.AppendLine("    if @@TRANCOUNT > 0");
                sb.AppendLine("    rollback transaction;");
                sb.AppendLine("    return;");
                sb.AppendLine("end catch;");

                sb.AppendLine("if @@TRANCOUNT > 0");
                sb.AppendLine("commit transaction;");
                sb.AppendLine("set @ReturnCode = 0");
                sb.AppendLine("set @ReturnMess = 'success'");
                sb.AppendLine("go");
                sb.AppendLine();
                returnData.Append(sb);
            }
            return returnData;
        }

        /// <summary>
        /// Tạo thủ tục SQL Delete.
        /// Nếu bảng có trường Status thì sẽ update Status = -1, còn không sẽ xóa bản ghi của bảng đó.
        /// Những bảng nào không có id tự tăng thì sẽ bỏ qua bảng đó.
        /// </summary>
        /// <param name="listTable"></param>
        /// <returns></returns>
        StringBuilder GenerateDelete(List<Table> listTable)
        {
            StringBuilder returnData = new StringBuilder();
            foreach (Table table in listTable)
            {
                StringBuilder sb = new StringBuilder();
                string identityColumnName = null;
                string statusColumnName = null;
                string procName = "sp_" + table.TableName + "_Delete";
                sb.AppendLine("if object_id('dbo." + procName + "', 'p') is null");
                sb.AppendLine("    exec ('create procedure " + procName + " as select 1')");
                sb.AppendLine("go");
                sb.AppendLine("alter procedure " + procName);
                foreach (TableColumn tableColumn in table.TableColumn)
                {
                    if (IsIdentityColumn(tableColumn))
                    {
                        if (IsMaxLengthColumn(tableColumn))
                            sb.AppendLine("@" + tableColumn.ColumnName + " " + tableColumn.DataType + "(" + tableColumn.MaxLength + "),");
                        else
                            sb.AppendLine("@" + tableColumn.ColumnName + " " + tableColumn.DataType + ",");
                        identityColumnName = tableColumn.ColumnName;
                    }
                    if (IsStatusColumn(tableColumn))
                        statusColumnName = tableColumn.ColumnName;
                }
                //Nếu bảng không có cột nào id tự tăng thì sẽ bỏ qua bảng đó
                if (string.IsNullOrWhiteSpace(identityColumnName))
                    continue;
                sb.AppendLine("@ReturnCode int output,");
                sb.AppendLine("@ReturnMess nvarchar(500) output");
                sb.AppendLine("as");
                sb.AppendLine("begin transaction;");
                sb.AppendLine("begin try");
                if (string.IsNullOrWhiteSpace(statusColumnName))
                {
                    //Delete bản ghi
                    sb.AppendLine("    delete from [" + table.TableName + "] where " + identityColumnName + " = @" + identityColumnName + ";");
                }
                else
                {
                    //Update status = -1
                    sb.AppendLine("    update [" + table.TableName + "] set [" + statusColumnName + "] = -1 where " + identityColumnName + " = @" + identityColumnName + ";");
                }
                sb.AppendLine("end try");
                sb.AppendLine("begin catch");
                sb.AppendLine("    set @ReturnCode = ERROR_NUMBER();");
                sb.AppendLine("    set @ReturnMess = ERROR_MESSAGE();");
                sb.AppendLine("    if @@TRANCOUNT > 0");
                sb.AppendLine("    rollback transaction;");
                sb.AppendLine("    return;");
                sb.AppendLine("end catch;");

                sb.AppendLine("if @@TRANCOUNT > 0");
                sb.AppendLine("commit transaction;");
                sb.AppendLine("set @ReturnCode = 0");
                sb.AppendLine("set @ReturnMess = 'success'");
                sb.AppendLine("go");
                sb.AppendLine();
                returnData.Append(sb);
            }
            return returnData;
        }

        /// <summary>
        /// Kiểm tra xem có phải cột id tự tăng không
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        bool IsIdentityColumn(TableColumn column)
        {
            for (int i = 0; i < LIST_IDENTITY_COLUMN.Count; i++)
            {
                if (LIST_IDENTITY_COLUMN[i].TableName == column.TableName && LIST_IDENTITY_COLUMN[i].ColumnName == column.ColumnName)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Kiểm tra xem cột có phải kiểu dữ liệu ký tự không
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        bool IsCharDataTypeColumn(TableColumn column)
        {
            if (column.DataType.Contains("char"))
                return true;
            return false;
        }

        /// <summary>
        /// Kiểm tra xem cột có giới hạn số lượng ký tự không
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        bool IsMaxLengthColumn(TableColumn column)
        {
            if (!string.IsNullOrWhiteSpace(column.MaxLength) && !column.DataType.Equals("ntext"))
                return true;
            return false;
        }

        /// <summary>
        /// Kiểm tra xem cột có phải là cột lưu Status không
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        bool IsStatusColumn(TableColumn column)
        {
            if (column.ColumnName.ToLower().Equals("status") && column.DataType.Equals("int"))
                return true;
            return false;
        }
    }
}
