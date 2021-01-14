namespace CrudGenerator
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.ckListTable = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckGenerateManipulation = new System.Windows.Forms.CheckBox();
            this.ckGenerateRepository = new System.Windows.Forms.CheckBox();
            this.ckGenerateModel = new System.Windows.Forms.CheckBox();
            this.ckGenerateConnection = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection string:";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(107, 10);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(604, 20);
            this.txtConnectionString.TabIndex = 1;
            this.txtConnectionString.Text = "Server=.;Database=Crud;Integrated security=True";
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(717, 8);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(6, 19);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(545, 258);
            this.txtOutput.TabIndex = 3;
            // 
            // ckListTable
            // 
            this.ckListTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ckListTable.CheckOnClick = true;
            this.ckListTable.FormattingEnabled = true;
            this.ckListTable.IntegralHeight = false;
            this.ckListTable.Location = new System.Drawing.Point(6, 17);
            this.ckListTable.Name = "ckListTable";
            this.ckListTable.Size = new System.Drawing.Size(199, 379);
            this.ckListTable.TabIndex = 4;
            this.ckListTable.SelectedIndexChanged += new System.EventHandler(this.ckListTable_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ckGenerateManipulation);
            this.groupBox1.Controls.Add(this.ckGenerateRepository);
            this.groupBox1.Controls.Add(this.ckGenerateModel);
            this.groupBox1.Controls.Add(this.ckGenerateConnection);
            this.groupBox1.Controls.Add(this.btnGenerate);
            this.groupBox1.Location = new System.Drawing.Point(229, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(563, 112);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Option";
            // 
            // ckGenerateManipulation
            // 
            this.ckGenerateManipulation.AutoSize = true;
            this.ckGenerateManipulation.Checked = true;
            this.ckGenerateManipulation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckGenerateManipulation.Location = new System.Drawing.Point(6, 88);
            this.ckGenerateManipulation.Name = "ckGenerateManipulation";
            this.ckGenerateManipulation.Size = new System.Drawing.Size(132, 17);
            this.ckGenerateManipulation.TabIndex = 12;
            this.ckGenerateManipulation.Text = "Generate manipulation";
            this.ckGenerateManipulation.UseVisualStyleBackColor = true;
            // 
            // ckGenerateRepository
            // 
            this.ckGenerateRepository.AutoSize = true;
            this.ckGenerateRepository.Checked = true;
            this.ckGenerateRepository.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckGenerateRepository.Location = new System.Drawing.Point(6, 65);
            this.ckGenerateRepository.Name = "ckGenerateRepository";
            this.ckGenerateRepository.Size = new System.Drawing.Size(118, 17);
            this.ckGenerateRepository.TabIndex = 11;
            this.ckGenerateRepository.Text = "Generate repository";
            this.ckGenerateRepository.UseVisualStyleBackColor = true;
            // 
            // ckGenerateModel
            // 
            this.ckGenerateModel.AutoSize = true;
            this.ckGenerateModel.Checked = true;
            this.ckGenerateModel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckGenerateModel.Location = new System.Drawing.Point(6, 42);
            this.ckGenerateModel.Name = "ckGenerateModel";
            this.ckGenerateModel.Size = new System.Drawing.Size(101, 17);
            this.ckGenerateModel.TabIndex = 10;
            this.ckGenerateModel.Text = "Generate model";
            this.ckGenerateModel.UseVisualStyleBackColor = true;
            // 
            // ckGenerateConnection
            // 
            this.ckGenerateConnection.AutoSize = true;
            this.ckGenerateConnection.Checked = true;
            this.ckGenerateConnection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckGenerateConnection.Location = new System.Drawing.Point(6, 19);
            this.ckGenerateConnection.Name = "ckGenerateConnection";
            this.ckGenerateConnection.Size = new System.Drawing.Size(126, 17);
            this.ckGenerateConnection.TabIndex = 9;
            this.ckGenerateConnection.Text = "Generate connection";
            this.ckGenerateConnection.UseVisualStyleBackColor = true;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(482, 83);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 8;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.ckListTable);
            this.groupBox2.Location = new System.Drawing.Point(12, 36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(211, 402);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "List table";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.txtOutput);
            this.groupBox3.Location = new System.Drawing.Point(235, 155);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(557, 283);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SQL Output";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(692, 444);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 23);
            this.progressBar.TabIndex = 10;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(480, 444);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(206, 23);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 474);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.label1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CRUD Generator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.CheckedListBox ckListTable;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.CheckBox ckGenerateConnection;
        private System.Windows.Forms.CheckBox ckGenerateModel;
        private System.Windows.Forms.CheckBox ckGenerateRepository;
        private System.Windows.Forms.CheckBox ckGenerateManipulation;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
    }
}

