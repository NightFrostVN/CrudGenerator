using System;
using System.Text;
using System.Windows.Forms;
using CrudGenerator.Utility;
using System.Linq;
using System.Threading.Tasks;

namespace CrudGenerator
{
    public partial class Main : Form
    {
        private string connectionString;
        public Main()
        {
            InitializeComponent();
        }

        void EnableForm()
        {
            foreach (Control control in Controls)
            {
                control.Enabled = true;
            }
            UseWaitCursor = false;
            progressBar.Style = ProgressBarStyle.Blocks;
        }

        void DisableForm()
        {
            foreach (Control control in Controls)
            {
                control.Enabled = false;
            }
            UseWaitCursor = true;
            progressBar.Style = ProgressBarStyle.Marquee;
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConnectionString.Text))
                return;
            connectionString = txtConnectionString.Text;
            lblStatus.Text = "Getting table info";
            DisableForm();
            await Task.Run(() => ConnectDB());
            lblStatus.Text = "";
            EnableForm();
        }

        void ConnectDB()
        {
            Invoke((MethodInvoker)delegate ()
            {
                ckListTable.Items.Clear();
            });
            DatabaseUtilities.GetTableData(connectionString);
            Invoke((MethodInvoker)delegate ()
            {
                ckListTable.Items.Add("Check all", true);
                for (int i = 0; i < CrudUtilities.LIST_TABLE.Count; i++)
                {
                    ckListTable.Items.Add(CrudUtilities.LIST_TABLE[i].TableName, true);
                }
            });
        }

        void Generate()
        {
            if (ckGenerateConnection.Checked)
                ClassUtilities.GenerateConnection(connectionString);
            if (ckGenerateModel.Checked)
                ClassUtilities.GenerateModel();
            if (ckGenerateRepository.Checked)
                ClassUtilities.GenerateRepository();
            if (ckGenerateManipulation.Checked)
                ClassUtilities.GenerateManipulation();

            StringBuilder sbCreate = DatabaseUtilities.GenerateCreate();
            StringBuilder sbRead = DatabaseUtilities.GenerateRead();
            StringBuilder sbUpdate = DatabaseUtilities.GenerateUpdate();
            StringBuilder sbDelete = DatabaseUtilities.GenerateDelete();
            Invoke((MethodInvoker)delegate ()
            {
                txtOutput.AppendText("-----CREATE-----" + Environment.NewLine);
                txtOutput.AppendText(sbCreate.ToString());
                txtOutput.AppendText("-----READ-----" + Environment.NewLine);
                txtOutput.AppendText(sbRead.ToString());
                txtOutput.AppendText("-----UPDATE-----" + Environment.NewLine);
                txtOutput.AppendText(sbUpdate.ToString());
                txtOutput.AppendText("-----DELETE-----" + Environment.NewLine);
                txtOutput.AppendText(sbDelete.ToString());
            });
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                return;
            CrudUtilities.LIST_TABLE.ForEach(m => m.Active = false);
            foreach (var item in ckListTable.CheckedItems)
            {
                if (!item.ToString().Equals("Check all"))
                    CrudUtilities.LIST_TABLE.First(m => m.TableName.Equals(item)).Active = true;
            }
            txtOutput.Clear();
            lblStatus.Text = "Generating";
            DisableForm();
            await Task.Run(() => Generate());
            lblStatus.Text = "";
            EnableForm();
        }

        private void ckListTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ckListTable.SelectedIndex == 0)
            {
                for (int i = 1; i < ckListTable.Items.Count; i++)
                {
                    if (ckListTable.GetItemChecked(0))
                        ckListTable.SetItemChecked(i, true);
                    else
                        ckListTable.SetItemChecked(i, false);
                }
            }
        }
    }
}
