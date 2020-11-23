using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using CrudGenerator.Utility;
using CrudGenerator.Model;
using System.IO;

namespace CrudGenerator
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConnectionString.Text))
                return;
            txtOutput.Clear();
            DatabaseUtilities.GetTableData(txtConnectionString.Text);
            if (CrudUtilities.LIST_TABLE.Count > 0)
            {
                GenerateScript();
                GenerateClass();
            }
        }

        void GenerateScript()
        {
            StringBuilder sbCreate = DatabaseUtilities.GenerateCreate(CrudUtilities.LIST_TABLE);
            txtOutput.AppendText("-----CREATE-----" + Environment.NewLine);
            txtOutput.AppendText(sbCreate.ToString());

            StringBuilder sbRead = DatabaseUtilities.GenerateRead(CrudUtilities.LIST_TABLE);
            txtOutput.AppendText("-----READ-----" + Environment.NewLine);
            txtOutput.AppendText(sbRead.ToString());

            StringBuilder sbUpdate = DatabaseUtilities.GenerateUpdate(CrudUtilities.LIST_TABLE);
            txtOutput.AppendText("-----UPDATE-----" + Environment.NewLine);
            txtOutput.AppendText(sbUpdate.ToString());

            StringBuilder sbDelete = DatabaseUtilities.GenerateDelete(CrudUtilities.LIST_TABLE);
            txtOutput.AppendText("-----DELETE-----" + Environment.NewLine);
            txtOutput.AppendText(sbDelete.ToString());
        }

        void GenerateClass()
        {
            ClassUtilities.GenerateDTO();
            ClassUtilities.GenerateRepository();
            ClassUtilities.GenerateDataManipulation();
        }
    }
}
