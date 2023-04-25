namespace BillAppSapB1
{
    partial class frmSearch
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
            this.label2 = new System.Windows.Forms.Label();
            this.cboFindBy = new System.Windows.Forms.ComboBox();
            this.cboOption = new System.Windows.Forms.ComboBox();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.DataGridViewSearch = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find By:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Filter:";
            // 
            // cboFindBy
            // 
            this.cboFindBy.FormattingEnabled = true;
            this.cboFindBy.Items.AddRange(new object[] {
            "รหัสลูกค้า",
            "ชื่อลูกค้า"});
            this.cboFindBy.Location = new System.Drawing.Point(92, 10);
            this.cboFindBy.Name = "cboFindBy";
            this.cboFindBy.Size = new System.Drawing.Size(172, 24);
            this.cboFindBy.TabIndex = 2;
            // 
            // cboOption
            // 
            this.cboOption.FormattingEnabled = true;
            this.cboOption.Items.AddRange(new object[] {
            "Contains",
            "Start with"});
            this.cboOption.Location = new System.Drawing.Point(92, 40);
            this.cboOption.Name = "cboOption";
            this.cboOption.Size = new System.Drawing.Size(172, 24);
            this.cboOption.TabIndex = 3;
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(92, 72);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(171, 22);
            this.txtFilter.TabIndex = 4;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // DataGridViewSearch
            // 
            this.DataGridViewSearch.AllowUserToAddRows = false;
            this.DataGridViewSearch.AllowUserToDeleteRows = false;
            this.DataGridViewSearch.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.DataGridViewSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewSearch.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.DataGridViewSearch.Location = new System.Drawing.Point(7, 103);
            this.DataGridViewSearch.Name = "DataGridViewSearch";
            this.DataGridViewSearch.ReadOnly = true;
            this.DataGridViewSearch.RowTemplate.Height = 24;
            this.DataGridViewSearch.Size = new System.Drawing.Size(622, 444);
            this.DataGridViewSearch.TabIndex = 5;
            this.DataGridViewSearch.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewSearch_CellDoubleClick);
            // 
            // frmSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(638, 559);
            this.Controls.Add(this.DataGridViewSearch);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.cboOption);
            this.Controls.Add(this.cboFindBy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Finder - Customer";
            this.Load += new System.EventHandler(this.frmSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboFindBy;
        private System.Windows.Forms.ComboBox cboOption;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.DataGridView DataGridViewSearch;
    }
}