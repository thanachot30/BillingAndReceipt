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
    public partial class frmBatchBilling : Form
    {
        PROCESS PROR = new PROCESS();
        public DataTable dtAllCus;
        public DataTable dtbilldetailTemp;

        public int indexConfig { get; set; }
        public frmBatchBilling()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Function New
            frmBillingNote frmBillingNote = new frmBillingNote();
            frmBillingNote.indexConfig = indexConfig;
            frmBillingNote.page = "New";
            frmBillingNote.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Function Open
            frmBillingNote frmBillingNote = new frmBillingNote();
            frmBillingNote.indexConfig = indexConfig;
            frmBillingNote.page = "Open";
            frmBillingNote.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void frmBatchBilling_Load(object sender, EventArgs e)
        {
            dtAllCus = PROR.GETDATA_AllCustomer();
            dtbilldetailTemp = PROR.GETDATA_ALLBILLDETAIL();
            // show batchbill
            ShowBatchBill();
        }

        private void Display_BATCHBILL(DataTable dtbatch)
        {
            dataGridViewBatchBilling.DataSource = null;
            //แสดง Display BatchBilling
            dataGridViewBatchBilling.DataSource = dtbatch;
            //Header name setting
            dataGridViewBatchBilling.Columns["BILLNO"].HeaderCell.Value = "เลขที่ใบวางบิล";
            dataGridViewBatchBilling.Columns["BILLNO"].Width = 150;
            dataGridViewBatchBilling.Columns["IDCUST"].HeaderCell.Value = "รหัสลูกค้า";
            dataGridViewBatchBilling.Columns["IDCUST"].Width = 120;
            dataGridViewBatchBilling.Columns["IDCUST"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewBatchBilling.Columns["CUSTNAME"].HeaderCell.Value = "ชื่อลูกค้า";
            dataGridViewBatchBilling.Columns["CUSTNAME"].Width = 280;
            dataGridViewBatchBilling.Columns["CUSTNAME"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewBatchBilling.Columns["INVDATE"].HeaderCell.Value = "วันที่เอกสาร";
            dataGridViewBatchBilling.Columns["INVDATE"].Width = 110;
            dataGridViewBatchBilling.Columns["DUEDATE"].HeaderCell.Value = "วันที่นัดชำระ";
            dataGridViewBatchBilling.Columns["DUEDATE"].Width = 110;
            dataGridViewBatchBilling.Columns["NETAMT"].HeaderCell.Value = "จำนวนเงิน";
            dataGridViewBatchBilling.Columns["NETAMT"].Width = 140;
            dataGridViewBatchBilling.Columns["NETAMT"].DefaultCellStyle.Format = "N2";
            dataGridViewBatchBilling.Columns["NETAMT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewBatchBilling.Columns["STA_0"].HeaderCell.Value = "สถานะ";
            dataGridViewBatchBilling.Columns["STA_0"].Width = 90;
            dataGridViewBatchBilling.Columns["STA_0"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewBatchBilling.Columns["NUMBERDETAIL"].HeaderCell.Value = "จำนวนใบแจ้งหนี้";
            dataGridViewBatchBilling.Columns["NUMBERDETAIL"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewBatchBilling.Columns["NUMBERDETAIL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //Visible setting
            dataGridViewBatchBilling.Columns["BILLSEQ"].Visible = false;
            //Color setting
            dataGridViewBatchBilling.EnableHeadersVisualStyles = false;
            dataGridViewBatchBilling.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Menu;
        }

        private DataTable FillNameCustandCNTLine(DataTable dt)
        {
            if (dt.Rows.Count > 0 )
            {
                for (int i =0;i<= dt.Rows.Count-1;i++)
                {
                    string idcust = dt.Rows[i]["IDCUST"].ToString();
                    string NameCust = getCustomerName(idcust);
                    // assign namecust to datatable
                    dt.Rows[i]["CUSTNAME"] = NameCust;

                    string billseq = dt.Rows[i]["BILLSEQ"].ToString();
                    string billno = dt.Rows[i]["BILLNO"].ToString();
                    int CNTbill = getCNTLINE(billseq, billno);
                    dt.Rows[i]["NUMBERDETAIL"] = CNTbill.ToString();
                }
                return dt;
            }
            return dt;
        }

        private string getCustomerName(string idcust)
        {
            //get all customername 
            DataRow[] result = dtAllCus.Select("CardCode = '" + idcust + "'");
            foreach (DataRow row in result)
            {
                string name = row["CardName"].ToString().Trim();
                return name;
            }
            //if not thing
            return "";
        }
        private int getCNTLINE(string billseq,string billno)
        {
            //เอา billdetail ทั้งหมดออก //แต่ถ้าเป็นแบบนี้เท่ากับว่าทำทุกรอบ เอาออกไปข้างนอก
            //เอาเข้ามาข้างในเพื่อ update billdetail
            dtbilldetailTemp = PROR.GETDATA_ALLBILLDETAIL();
            int CNT_Length = dtbilldetailTemp.Select("BILLSEQ = '" + billseq + "'" + " AND " + "BILLNO = '" + billno + "'").Length;
            return CNT_Length;
        }

        public void button7_Click(object sender, EventArgs e)
        {
            //Refresh
            ShowBatchBill();
        }

        public void ShowBatchBill()
        {
            DataTable dtbatch = PROR.GETDATA_BATCHBILL(cboShow.Checked);
            dtbatch = FillNameCustandCNTLine(dtbatch);
            Display_BATCHBILL(dtbatch);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewBatchBilling_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // โค้ดนี้ไว้สำหรับ แก้ไข Object โดยที่ Form ยังคงเปิดอยู่
            //frmBatchBilling batchbill = (frmBatchBilling)Application.OpenForms["frmBatchBilling"];
            //batchbill.button7_Click(null, null);

            // โค้ดนี้สำหรับ แก้ไข object โดยที่เปิด form ใหม่
            //frmBillingNote billnote = new frmBillingNote();
            //billnote.Show();
            //billnote.txtCardcode.Text = "9999";

            DataGridViewRow row = new DataGridViewRow();
            row = dataGridViewBatchBilling.Rows[e.RowIndex];
            frmBillingNote billnote = new frmBillingNote();
            billnote.page = "Open";
            billnote.txtCardcode.Text = row.Cells["IDCUST"].Value.ToString();
            billnote.txtCardName.Text = row.Cells["CUSTNAME"].Value.ToString();
            billnote.txtBillNo.Text = row.Cells["BILLNO"].Value.ToString();
            billnote.txtStatus.Text = row.Cells["STA_0"].Value.ToString();
            //get data billhead form database
            DataTable dthead = PROR.GET_1BILLHEAD(row.Cells["BILLNO"].Value.ToString(), row.Cells["IDCUST"].Value.ToString());
            if (dthead.Rows.Count > 0)
            {
                billnote.txtBillSEQ.Text = dthead.Rows[0]["BILLSEQ"].ToString();
                billnote.txtCommentHeader.Text = dthead.Rows[0]["COMMENTS"].ToString();
            }
            billnote.Show();
        }
    }
}
