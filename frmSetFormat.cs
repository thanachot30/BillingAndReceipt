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
    public partial class frmSetFormat : Form
    {
        DATACLASS DATA = new DATACLASS();
        PROCESS PROC = new PROCESS();
        public frmSetFormat()
        {
            InitializeComponent();
        }

        private void frmSetFormat_Load(object sender, EventArgs e)
        {
            //
            DataTable dt = new DataTable();
            dt = PROC.GETDATA_RUNNO();
            if (dt.Rows.Count == 0)
            {
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("DOCTYPE");
                    dt.Columns.Add("LENGTH");
                    dt.Columns.Add("PREFIX");
                    dt.Columns.Add("RUNNO");
                    dt.Columns.Add("COMP");
                    dt.Columns.Add("RCPTYPE");
                }
                //init format
                string Len = "10";
                string format = String.Format("{0:D"+Len+"}", 0);

                String[] RowBill = new string[] { "Billing Note", Len, "BTS-", format, PROC.GETDATA_COMP() };
                String[] RowReceipt = new string[] { "Receipt", Len, "RCP-", format, PROC.GETDATA_COMP() };
                dt.Rows.Add(RowBill);
                dt.Rows.Add(RowReceipt);
                dt.AcceptChanges();
            }

            dataGridViewFormatCode.DataSource = dt;
            dataGridViewFormatCode.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = PROC.CONVERTDGVTODT(dataGridViewFormatCode);

            bool result = DATA.INSERTFMSMASTERRUNNO(dt);
            if (result)
            {
                MessageBox.Show("Save Format Complete", "Notification", MessageBoxButtons.OK, MessageBoxIcon.None);
                this.Close();
            }
            else
            {
                MessageBox.Show("Incomplete save", "Notification");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
