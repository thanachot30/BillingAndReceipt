using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BillAppSapB1
{
    public partial class frmSearch : Form
    {
        public int indexConfig { get; set; }
        public string OBJ { get; set; }
        public string page { get; set; }

        private frmBillingNote frmBillingNote;

        public frmSearch(frmBillingNote frmBillingNote)
        {
            InitializeComponent();
            this.frmBillingNote = frmBillingNote;
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            cboFindBy.SelectedIndex = 0;
            cboOption.SelectedIndex = 0;
            txtFilter.Clear();
            DisplayDataGridView(OBJ, ConTHAItoENG(cboFindBy.Text), cboOption.Text,txtFilter.Text);
            
        }

        public void DisplayDataGridView(string OBJ,string FILE,string OPTION,string findingWord)
        {
            DataGridViewSearch.DataSource = null;
            //
            DataTable dtSearch = new DataTable();
            //Check page
            switch (OBJ)
            {
                //switch page
                case "CUSTOMER":
                    PROCESS PROC = new PROCESS();
                    PROC.indexConfig = indexConfig;
                    DataTable dt = new DataTable();
                    dt = PROC.GETDATA_Customer(FILE,OPTION, findingWord);
                    DataGridViewSearch.DataSource = dt;
                    DataGridViewSearch.RowsDefaultCellStyle.BackColor = Color.Bisque;
                    DataGridViewSearch.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
                    DataGridViewSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    break;
                case "XXX":
                    break;
            }
        }

        public String ConTHAItoENG(string wordThai)
        {
            switch (wordThai)
            {
                case "รหัสลูกค้า":
                    return "CardCode";
                    break;
                case "ชื่อลูกค้า":
                    return "CardName";
                    break;
                default:
                    return "";
                    break;
            }
        }

        private void DataGridViewSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridViewSearch.Rows[e.RowIndex];
                switch (page)
                {
                    case "frmBillingNote":
                        frmBillingNote.txtCardcode.Text = row.Cells[0].Value.ToString();
                        frmBillingNote.txtCardName.Text = row.Cells[1].Value.ToString();
                        this.Close();
                        break;
                }

            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            //
            DisplayDataGridView(OBJ, ConTHAItoENG(cboFindBy.Text), cboOption.Text, txtFilter.Text);
            //
        }
    }
}
