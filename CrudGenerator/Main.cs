using System;
using System.Text;
using System.Windows.Forms;
using CrudGenerator.Utility;
using System.Linq;

namespace CrudGenerator
{
    public partial class Main : Form
    {
        private string connectionString;
        public Main()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConnectionString.Text))
                return;
            ckListTable.Items.Clear();
            connectionString = txtConnectionString.Text;
            DatabaseUtilities.GetTableData(connectionString);
            for (int i = 0; i < CrudUtilities.LIST_TABLE.Count; i++)
            {
                ckListTable.Items.Add(CrudUtilities.LIST_TABLE[i].TableName, true);
            }
        }

        void GenerateScript()
        {
            StringBuilder sbCreate = DatabaseUtilities.GenerateCreate();
            txtOutput.AppendText("-----CREATE-----" + Environment.NewLine);
            txtOutput.AppendText(sbCreate.ToString());

            StringBuilder sbRead = DatabaseUtilities.GenerateRead();
            txtOutput.AppendText("-----READ-----" + Environment.NewLine);
            txtOutput.AppendText(sbRead.ToString());

            StringBuilder sbUpdate = DatabaseUtilities.GenerateUpdate();
            txtOutput.AppendText("-----UPDATE-----" + Environment.NewLine);
            txtOutput.AppendText(sbUpdate.ToString());

            StringBuilder sbDelete = DatabaseUtilities.GenerateDelete();
            txtOutput.AppendText("-----DELETE-----" + Environment.NewLine);
            txtOutput.AppendText(sbDelete.ToString());
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                return;
            foreach (var item in ckListTable.CheckedItems)
            {
                CrudUtilities.LIST_TABLE.First(m => m.TableName.Equals(item)).Active = true;
            }

            txtOutput.Clear();
            if (ckGenerateConnection.Checked)
                ClassUtilities.GenerateConnection(connectionString);
            if (ckGenerateModel.Checked)
                ClassUtilities.GenerateModel();
            if (ckGenerateRepository.Checked)
                ClassUtilities.GenerateRepository();
            if (ckGenerateManipulation.Checked)
                ClassUtilities.GenerateManipulation();
            GenerateScript();

        }
    }
}
