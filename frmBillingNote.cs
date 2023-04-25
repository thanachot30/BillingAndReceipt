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
    public partial class frmBillingNote : Form
    {
        Log log = new Log();
        PROCESS PROC = new PROCESS();
        DATACLASS DATA = new DATACLASS();
        public int indexConfig { get; set; }
        public string page { get; set; }
        public int cnt = 0;
        
        public string INVDATE;
        public string DUEDATE;
        public string DOCDATE;
        public DataTable dt = new DataTable();
        public DataTable dt2 = new DataTable();
        public DataTable dt3 = new DataTable();

        private decimal SumAMT = 0;

        private decimal NETAMT = 0;
        public frmBillingNote()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmSearch frmSearch = new frmSearch(this);
            frmSearch.indexConfig = indexConfig;
            frmSearch.page = "frmBillingNote";
            frmSearch.OBJ = "CUSTOMER";
            frmSearch.ShowDialog();
        }

        private void frmBillingNote_Load(object sender, EventArgs e)
        {
            //Load
            switch (page)
            {
                case "New":
                    this.Text = "Billing Note-" + page;
                    txtBillNo.Text = "***New***";
                    //ช่องสถาณะ 'new' ก็ต้อง Open
                    txtStatus.Text = "Open";
                    break;

                case "Open":
                    this.Text = "Billing Note-" + page;

                    button6_Click(null, null);
                    break;
            }
        }

        private void txtCardcode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                string CardCode = txtCardcode.Text;
                string CardName = txtCardName.Text;
                CboFillContactPerson(CardCode);
            }
        }

        private void CboFillContactPerson(string CardCode)
        {
            DataTable dt = new DataTable();
            cboContactPerson.Items.Clear();
            dt = PROC.GETDATA_ContactPerson(CardCode);
            foreach (DataRow row in dt.Rows)
            {
                cboContactPerson.Items.Add(row["Name"].ToString());
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCardcode_TextChanged(object sender, EventArgs e)
        {
            string CardCode = txtCardcode.Text;
            string CardName = txtCardName.Text;
            CboFillContactPerson(CardCode);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            cnt += 1;
            string CardCode = txtCardcode.Text;
            string CardName = txtCardName.Text;
            string ContactPerson = cboContactPerson.Text;
            string DateForm = dateForm.Value.Date.ToString("yyyy-MM-dd");
            string DataTo = dateTo.Value.Date.ToString("yyyy-MM-dd");
            // Button 'go'
            switch (page)
            {
                case "New":
                    //BillDetailINV (A/R Invoice) Temp
                    dt = PROC.GETDATA_BillDetailINV(CardCode, DateForm, DataTo);
                    dt = PreprocessTableINV(dt,"Open");
                    dt = CheckAlreadyNo("INV", dt);
                    //BillDetailCN (A/R Credit Memo)
                    dt2 = PROC.GETDATA_BillDetailCN(CardCode, DateForm, DataTo);
                    dt2 = PreprocessTableCN(dt2, "Open");
                    dt2 = CheckAlreadyNo("CN", dt2);
                    //BillDetailDT ()
                    dt3 = PROC.GETDATA_BillDetailDT(CardCode, DateForm, DataTo);
                    dt3 = PreprocessTableDT(dt3, "Open");
                    dt3 = CheckAlreadyNo("DT", dt3);
                    //Merge datatable dt/dt2/dt3
                    dt.Merge(dt2);
                    dt.Merge(dt3);
                    //Display DGV
                    DisplayDGVBillNote(dt);
                    break;
                case "Open":
                    // แสดงบิลที่เคยเปิดไว้แล้วก่อน และ mearge ด้วย บิลที่จะคสดว่าจะเปิดใหม่
                    string IDCUST = txtCardcode.Text;
                    string BILLNO = txtBillNo.Text;
                    //บิลที่เคยเปิดไว้แล้ว
                    DataTable dtopen = new DataTable();
                    dtopen = PROC.GETDATA_OpenBillDetail(IDCUST, BILLNO);
                    //บิลที่ยังไม่ได้เปิด เพิ่มเติม เมื่อ กด go ครั้งที่ 2
                    if (cnt > 1)
                    {
                        DataTable dtOtherinv = new DataTable();
                        dtOtherinv = PROC.GETDATA_BillDetailINV(CardCode, DateForm, DataTo);
                        dtOtherinv = PreprocessTableINV(dtOtherinv, "Open");
                        dtOtherinv = CheckAlreadyNo("INV", dtOtherinv);//Other table inv/cn/dt
                        DataTable dtOtherCN = new DataTable();
                        dtOtherCN = PROC.GETDATA_BillDetailCN(CardCode, DateForm, DataTo);
                        dtOtherCN = PreprocessTableCN(dtOtherCN, "Open");
                        dtOtherCN = CheckAlreadyNo("CN", dtOtherCN);
                        DataTable dtOtherDT = new DataTable();
                        dtOtherDT = PROC.GETDATA_BillDetailDT(CardCode, DateForm, DataTo);
                        dtOtherDT = PreprocessTableDT(dtOtherDT, "Open");
                        dtOtherDT = CheckAlreadyNo("DT", dtOtherDT);
                        //Merge datatable 
                        dtopen.Merge(dtOtherinv,true, MissingSchemaAction.Ignore);
                        dtopen.Merge(dtOtherCN,true, MissingSchemaAction.Ignore);
                        dtopen.Merge(dtOtherDT, true, MissingSchemaAction.Ignore);
                    }
                    //Display DGV
                    DisplayDGVBillNote(dtopen);
                    break;
            }
        }

        private DataTable CheckAlreadyNo(string type,DataTable dttemp)
        {
            //check already number in billdetail
            DataTable dtdetailTemp = dtdetailTemp = PROC.GETDATA_ALLBILLDETAIL(); //all row of 'FMSBILLDETAIL'
            switch (type)
            {
                case "INV"://'IN'
                    if (dttemp.Rows.Count > 0)
                    {
                        dttemp = EditdtTemp(dttemp, dtdetailTemp,"IN");
                    }
                    break;
                case "CN":
                    if (dttemp.Rows.Count > 0)
                    {
                        dttemp = EditdtTemp(dttemp, dtdetailTemp, "CN");
                    }
                        break;
                case "DT":
                    if (dttemp.Rows.Count > 0)
                    {
                        dttemp = EditdtTemp(dttemp, dtdetailTemp, "DT");
                    }
                    break;
            }
            return dttemp;
        }

        public DataTable EditdtTemp(DataTable dttemp,DataTable dtdetailTemp,string Doctype)
        {
            //loop datatable DGV
            for (int i = dttemp.Rows.Count - 1; i >= 0; i--)
            {
                string inv = dttemp.Rows[i]["DocNum"].ToString();
                int result = dtdetailTemp.Select("DOCNUM = '" + inv + "' AND DOCTYPE = '" + Doctype + "'").Length;
                if (result != 0)
                {
                    //แสดงว่ามีการเปิด INV ในนี้แล้ว
                    //ตรวจสอบยอดคงเหลือ ทั้งหมด ต่อไป
                    //select หาใน FMSBILLDETAIL ว่าเคยเปิดไว้กี่รายการแล้วบ้าง
                    DataRow[] results = dtdetailTemp.Select("DOCNUM = '" + inv + "' AND DOCTYPE = '" + Doctype + "'"); 
                    decimal doctotal = Convert.ToDecimal(dttemp.Rows[i]["DocTotal"]);//ยอดเต็ม
                    decimal netamtLoop = 0;                                          //ยอดบวกรวม ที่เคยเปิดไปแล้ว
                    decimal amtoutstand = 0;                                        //ยอดคงเหลือ
                    for (int j = 0; j <= results.Length - 1; j++)
                    {
                        //บวกรวมยอดที่เคยเปิด 
                        netamtLoop += Convert.ToDecimal(results[j]["NETAMT"]);
                    }
                    amtoutstand = doctotal - netamtLoop;
                    if (amtoutstand == 0)
                    {
                        //เปิดเต็มจำนวนแล้ว ไม่มียอดคงเหลือ
                        //สามารถลบ Row temp การแสดงผลนั้นได้เลย 
                        dttemp.Rows.RemoveAt(i);
                    }
                    else
                    {
                        //แสดงว่ายังมียอดค้างชำระ แก้เป็นจำนวนยอดคงเหลือ จำนวนที่เหลือเข้าไป
                        dttemp.Rows[i]["AMT"] = amtoutstand;
                        dttemp.Rows[i]["AMTOUTSTAND"] = doctotal - netamtLoop - Convert.ToDecimal(dttemp.Rows[i]["AMT"]);
                            
                    }

                }

            }
            return dttemp;
        }
     
        public DataTable PreprocessTableINV(DataTable dt,string STA_0)
        {
            // Only INV from SAPB1 ข้อมูลมาจาก SAPB1 
            //New Column
            if (dt.Columns.Contains("DOCTYPE") == false)
            {
                dt.Columns.Add("STA_0");
                dt.Columns.Add("DOCTYPE");
                dt.Columns.Add("INVNO");
                dt.Columns.Add("AMTOUTSTAND", typeof(decimal));
                dt.Columns.Add("AMT", typeof(decimal));
                dt.Columns.Add("COMMENTDETAIL");
                dt.Columns.Add("CHECK_0");
                dt.Columns.Add("WTBase");
                dt.Columns.Add("WTSum");
            }
            
            if (dt.Rows.Count > 0)
            {
                // loop each Rows
                for (int i=0;i<= dt.Rows.Count -1;i++)
                {
                    // กรองค่า
                    decimal resDocTotal = Convert.ToDecimal(dt.Rows[i]["DocTotal"]);
                    decimal resPaidToDate = Convert.ToDecimal(dt.Rows[i]["PaidToDate"]);
                    decimal AMTOUTSTAND = resDocTotal - resPaidToDate;
                    if (AMTOUTSTAND == 0)
                    {
                        dt.Rows[i].Delete();
                        continue;
                    }
                    else
                    {   
                        //DocType 'IN'/'CN'/'DT'
                        dt.Rows[i]["DocType"] = "IN";
                        dt.Rows[i]["STA_0"] = STA_0;
                        //WTBase
                        dt.Rows[i]["WTBase"] = PROC.GETDATA_WTBase(dt.Rows[i]["DocEntry"].ToString());
                        dt.Rows[i]["WTSum"] = PROC.GETDATA_WTSum(dt.Rows[i]["DocEntry"].ToString());
                        // Concat INVNO
                        if (dt.Rows[i]["BeginStr"] == DBNull.Value)
                        {
                            dt.Rows[i]["INVNO"] = dt.Rows[i]["DocNum"].ToString().Trim();
                        }
                        else
                        {
                            dt.Rows[i]["INVNO"] = dt.Rows[i]["BeginStr"].ToString().Trim() + dt.Rows[i]["DocNum"].ToString().Trim();
                        }
                        //Doc Date format
                        dt.Rows[i]["TaxDate"] = dt.Rows[i]["TaxDate"].ToString();
                        dt.Rows[i]["DocDueDate"] = dt.Rows[i]["DocDueDate"].ToString();
                        //Doc Total
                        dt.Rows[i]["DocTotal"] = dt.Rows[i]["DocTotal"];
                        dt.Rows[i]["AMT"] = AMTOUTSTAND; //เปิดใบแจ้งหนี้ยอดเต็ม
                        dt.Rows[i]["AMTOUTSTAND"] = Convert.ToDecimal(dt.Rows[i]["DocTotal"]) - Convert.ToDecimal(dt.Rows[i]["AMT"]);
                        dt.Rows[i]["COMMENTDETAIL"] = "";
                    }
                }
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public DataTable PreprocessTableCN(DataTable dt, string STA_0)
        {
            //New Column
            if (dt.Columns.Contains("DOCTYPE") == false)
            {
                dt.Columns.Add("STA_0");
                dt.Columns.Add("DOCTYPE");
                dt.Columns.Add("INVNO");
                dt.Columns.Add("AMTOUTSTAND", typeof(decimal));
                dt.Columns.Add("AMT", typeof(decimal));
                dt.Columns.Add("COMMENTDETAIL");
                dt.Columns.Add("CHECK_0");
                dt.Columns.Add("WTBase");
                dt.Columns.Add("WTSum");
            }
            
            if (dt.Rows.Count > 0)
            {
                // loop each Rows
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    decimal resDocTotal = Convert.ToDecimal(dt.Rows[i]["DocTotal"]);
                    decimal resPaidToDate = Convert.ToDecimal(dt.Rows[i]["PaidToDate"]);
                    decimal AMTOUTSTAND = resDocTotal - resPaidToDate;
                    if (AMTOUTSTAND == 0)
                    {
                        dt.Rows[i].Delete();
                        continue;
                    }
                    else
                    {
                        //DocType 'IN'/'CN'/'DT'
                        dt.Rows[i]["DocType"] = "CN";
                        dt.Rows[i]["STA_0"] = STA_0;
                        //WTBase
                        dt.Rows[i]["WTBase"] = PROC.GETDATA_WTBase(dt.Rows[i]["DocEntry"].ToString());
                        dt.Rows[i]["WTSum"] = PROC.GETDATA_WTSum(dt.Rows[i]["DocEntry"].ToString());
                        // Concat INVNO
                        if (dt.Rows[i]["BeginStr"] == DBNull.Value)
                        {
                            dt.Rows[i]["INVNO"] = dt.Rows[i]["DocNum"].ToString().Trim();
                        }
                        else
                        {
                            dt.Rows[i]["INVNO"] = dt.Rows[i]["BeginStr"].ToString().Trim() + dt.Rows[i]["DocNum"].ToString().Trim();
                        }
                        //Doc Date format
                        dt.Rows[i]["TaxDate"] = dt.Rows[i]["TaxDate"].ToString();
                        dt.Rows[i]["DocDueDate"] = dt.Rows[i]["DocDueDate"].ToString();
                        //Doc Total * -1
                        decimal InvertDocTotal = Convert.ToDecimal(dt.Rows[i]["DocTotal"].ToString()) * -1;
                        dt.Rows[i]["DocTotal"] = InvertDocTotal;
                        //
                        dt.Rows[i]["AMT"] = dt.Rows[i]["DocTotal"];
                        dt.Rows[i]["AMTOUTSTAND"] = InvertDocTotal - Convert.ToDecimal(dt.Rows[i]["AMT"]);
                        dt.Rows[i]["COMMENTDETAIL"] = "";
                    }
                }
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public DataTable PreprocessTableDT(DataTable dt, string STA_0)
        {
            if (dt.Columns.Contains("DOCTYPE") == false)
            {
                dt.Columns.Add("STA_0");
                dt.Columns.Add("DOCTYPE");
                dt.Columns.Add("INVNO");
                dt.Columns.Add("AMTOUTSTAND", typeof(decimal));
                dt.Columns.Add("AMT", typeof(decimal));
                dt.Columns.Add("COMMENTDETAIL");
                dt.Columns.Add("CHECK_0");
                dt.Columns.Add("WTBase");
                dt.Columns.Add("WTSum");
            }
            
            if (dt.Rows.Count > 0)
            {
                // loop each Rows
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    decimal resDocTotal = Convert.ToDecimal(dt.Rows[i]["DocTotal"]);
                    decimal resPaidToDate = Convert.ToDecimal(dt.Rows[i]["PaidToDate"]);
                    decimal AMTOUTSTAND = resDocTotal - resPaidToDate;
                    if (AMTOUTSTAND == 0)
                    {
                        dt.Rows[i].Delete();
                        continue;
                    }
                    else
                    {
                        //DocType 'IN'/'CN'/'DT'
                        dt.Rows[i]["DocType"] = "DT";
                        dt.Rows[i]["STA_0"] = STA_0;
                        //WTBase
                        dt.Rows[i]["WTBase"] = PROC.GETDATA_WTBase(dt.Rows[i]["DocEntry"].ToString());
                        dt.Rows[i]["WTSum"] = PROC.GETDATA_WTSum(dt.Rows[i]["DocEntry"].ToString());
                        // Concat INVNO
                        if (dt.Rows[i]["BeginStr"] == DBNull.Value)
                        {
                            dt.Rows[i]["INVNO"] = dt.Rows[i]["DocNum"].ToString().Trim();
                        }
                        else
                        {
                            dt.Rows[i]["INVNO"] = dt.Rows[i]["BeginStr"].ToString().Trim() + dt.Rows[i]["DocNum"].ToString().Trim();
                        }
                        //Doc Date format
                        dt.Rows[i]["TaxDate"] = dt.Rows[i]["TaxDate"].ToString();
                        dt.Rows[i]["DocDueDate"] = dt.Rows[i]["DocDueDate"].ToString();
                        //Doc Total
                        dt.Rows[i]["DocTotal"] = dt.Rows[i]["DocTotal"];
                        
                        dt.Rows[i]["AMT"] = dt.Rows[i]["DocTotal"];
                        dt.Rows[i]["AMTOUTSTAND"] = Convert.ToDecimal(dt.Rows[i]["DocTotal"]) - Convert.ToDecimal(dt.Rows[i]["AMT"]);
                        dt.Rows[i]["COMMENTDETAIL"] = "";
                    }
                }
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public void DisplayDGVBillNote(DataTable dt)
        {
            dataGridViewBillNote.DataSource = null;
            //Display Run number new
            if (dt.Columns.Contains("No.") == false)
            {
                dt.Columns.Add("No.");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0;i <= dt.Rows.Count -1;i++)
                    {
                        dt.Rows[i]["No."] = i + 1;
                    }
                }
            }
            else
            {
                //Display after save
            }
            //
            dataGridViewBillNote.DataSource = dt;
            //
            dataGridViewBillNote.Columns["No."].DisplayIndex = 0;
            dataGridViewBillNote.Columns["No."].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewBillNote.Columns["No."].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewBillNote.Columns["No."].Width = 40;

            //Create checkbox in row
            if (dataGridViewBillNote.Columns.Contains("CHECK_0") == true)
            {
                DataGridViewCheckBoxColumn dtgCheckbox = new DataGridViewCheckBoxColumn();
                dtgCheckbox.DataPropertyName = "CHECK_0";
                dtgCheckbox.HeaderText = "Select";
                dtgCheckbox.Name = "CHECK_0";
                dtgCheckbox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dtgCheckbox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgCheckbox.Width = 60;
                dtgCheckbox.ReadOnly = false;
                dtgCheckbox.DisplayIndex = 1;
                dataGridViewBillNote.Columns.Add(dtgCheckbox);
            }

            //Display Size
            dataGridViewBillNote.Columns["DocType"].HeaderCell.Value = "ประเภทเอกสาร";
            dataGridViewBillNote.Columns["DocType"].Width = 100;
            dataGridViewBillNote.Columns["DocType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewBillNote.Columns["INVNO"].HeaderCell.Value = "เลขที่เอกสาร";
            dataGridViewBillNote.Columns["INVNO"].Width = 200;
            dataGridViewBillNote.Columns["INVNO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewBillNote.Columns["TaxDate"].HeaderCell.Value = "วันที่ใบแจ้งหนี้";
            dataGridViewBillNote.Columns["TaxDate"].Width = 100;
            dataGridViewBillNote.Columns["TaxDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewBillNote.Columns["DocDueDate"].HeaderCell.Value = "วันที่ครบกำหนด";
            dataGridViewBillNote.Columns["DocDueDate"].Width = 100;
            dataGridViewBillNote.Columns["DocDueDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewBillNote.Columns["DocTotal"].HeaderCell.Value = "จำนวนเงินตามใบแจ้งหนี้";
            dataGridViewBillNote.Columns["DocTotal"].Width = 150;
            dataGridViewBillNote.Columns["DocTotal"].DefaultCellStyle.Format = "N2";
            dataGridViewBillNote.Columns["DocTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewBillNote.Columns["AMTOUTSTAND"].HeaderCell.Value = "ยอดค้างชำระ";
            dataGridViewBillNote.Columns["AMTOUTSTAND"].Width = 150;
            dataGridViewBillNote.Columns["AMTOUTSTAND"].DefaultCellStyle.Format = "N2";
            dataGridViewBillNote.Columns["AMTOUTSTAND"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewBillNote.Columns["AMT"].HeaderCell.Value = "จำนวนเงินวางบิล";
            dataGridViewBillNote.Columns["AMT"].Width = 150;
            dataGridViewBillNote.Columns["AMT"].DefaultCellStyle.Format = "N2";
            dataGridViewBillNote.Columns["AMT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewBillNote.Columns["COMMENTDETAIL"].HeaderCell.Value = "หมายเหตุ";
            dataGridViewBillNote.Columns["COMMENTDETAIL"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridViewBillNote.Columns["COMMENTDETAIL"].Width = 200;
            dataGridViewBillNote.Columns["COMMENTDETAIL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //Display Index
            dataGridViewBillNote.Columns["DocType"].DisplayIndex = 2;
            dataGridViewBillNote.Columns["INVNO"].DisplayIndex = 3;
            dataGridViewBillNote.Columns["TaxDate"].DisplayIndex = 4;
            //Edit 
            dataGridViewBillNote.Columns["No."].ReadOnly = true;
            dataGridViewBillNote.Columns["DocType"].ReadOnly = true;
            dataGridViewBillNote.Columns["INVNO"].ReadOnly = true;
            dataGridViewBillNote.Columns["TaxDate"].ReadOnly = true;
            dataGridViewBillNote.Columns["DocDueDate"].ReadOnly = true;
            dataGridViewBillNote.Columns["DocTotal"].ReadOnly = true;
            dataGridViewBillNote.Columns["AMTOUTSTAND"].ReadOnly = true;
            //Visible Index
            if (page == "Open")
            {
                dataGridViewBillNote.Columns["DOCTYPE1"].Visible = false;
                dataGridViewBillNote.Columns["TaxDate1"].Visible = false;
                dataGridViewBillNote.Columns["DocDueDate1"].Visible = false;
                dataGridViewBillNote.Columns["DocTotal1"].Visible = false;
            }
            

            dataGridViewBillNote.Columns["STA_0"].Visible = false;
            dataGridViewBillNote.Columns["CHECK_0"].Visible = false;
            dataGridViewBillNote.Columns["BeginStr"].Visible = false;
            dataGridViewBillNote.Columns["DocNum"].Visible = false;
            dataGridViewBillNote.Columns["CardCode"].Visible = false;
            dataGridViewBillNote.Columns["PaidToDate"].Visible = false;
            dataGridViewBillNote.Columns["VatSum"].Visible = false;
            dataGridViewBillNote.Columns["DocEntry"].Visible = false;
            dataGridViewBillNote.Columns["WTBase"].Visible = false;
            dataGridViewBillNote.Columns["WTSum"].Visible = false;
            //color setting
            dataGridViewBillNote.EnableHeadersVisualStyles = false;
            dataGridViewBillNote.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Menu;
            //update
            dataGridViewBillNote.Update();
            dataGridViewBillNote.Refresh();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //cbo select All
            CheckBox_0();
            dataGridViewBillNote.Refresh();
            SHOW_SumAMT();
        }


        private void CheckBox_0()
        {
            bool checkBox = cboSelectAll.Checked;
            if (dataGridViewBillNote.Rows.Count > 0)
            {
                //select all
                for (int i = 0; i <= dataGridViewBillNote.Rows.Count - 1; i++)
                {
                    dataGridViewBillNote.Rows[i].Cells["CHECK_0"].Value = checkBox;
                }
                dataGridViewBillNote.Refresh();
            }
            else
            {
                return;
            }
        }

        private void dataGridViewBillNote_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewBillNote.EndEdit();
            // checkbox
            if (dataGridViewBillNote.Columns[e.ColumnIndex].Name == "CHECK_0")
            {
                dataGridViewBillNote.EndEdit();
                //MessageBox.Show(e.RowIndex.ToString());
                //Decimal TempDocTotal = Convert.ToDecimal(dataGridViewBillNote.Rows[e.RowIndex].Cells["AMT"].Value);
                SHOW_SumAMT();
                //Show SumAMT
            }
        }

        private void SHOW_SumAMT()
        {
            SumAMT = 0;
            if (dataGridViewBillNote.Rows.Count > 0)
            {
                //loop each row
                for (int i=0;i<= dataGridViewBillNote.Rows.Count-1;i++)
                {
                    // check checkbox
                    if (dataGridViewBillNote.Rows[i].Cells["CHECK_0"].Value.ToString() == "True")
                    {
                        SumAMT += Convert.ToDecimal(dataGridViewBillNote.Rows[i].Cells["AMT"].Value);
                    }
                }
            }
            txtSumAmt.Text = SumAMT.ToString("N2");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //SAVE
            DataTable dt = new DataTable();
            if (checkBoxAlready()) //checkbox at least
            {
                DialogResult result = MessageBox.Show("Do you Confirm to save Billing", "Notification",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    switch (page)
                    {
                        case "New":
                            //remove row is not checkbox
                            ClearRowNotCheck();
                            dt = PROC.CONVERTDGVTODT(dataGridViewBillNote);
                            // BillHead
                            string head = ProcessBillHead();
                            // BillDetail
                            bool detail = ProcessBillDetail(dt);
                            if ( head != "" && detail == true)
                            {
                                
                                txtBillNo.Text = head.Trim();
                                txtBillNo.BackColor = Color.Yellow;
                                MessageBox.Show("SAVE: " + page + " 'Success' " + "\n"+
                                                " BILLNO: " + head, "Notification",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                            }
                            break;
                        case "Open":
                            //save 'open' edit
                            string opBILLNO = txtBillNo.Text;
                            string opIDCUST = txtCardcode.Text;
                            string opBILLSEQ = txtBillSEQ.Text;
                            ClearRowNotCheck();
                            dt = PROC.CONVERTDGVTODT(dataGridViewBillNote);
                            // Delete old billdetail
                            DATA.DELETEFMSBILL(opBILLNO, opIDCUST);
                            string ophead = ProcessBillHead();
                            bool opdetail = ProcessBillDetail(dt);
                            if (ophead != "" && opdetail == true)
                            {
                                txtBillNo.Text = ophead.Trim();
                                txtBillNo.BackColor = Color.Yellow;
                                MessageBox.Show("Save Edit: " + page + "'Success' " + "\n" +
                                                "BILLNO: " + ophead, "Notification", MessageBoxButtons.OK, MessageBoxIcon.None);
                            }

                            break;
                    }
                    //Refresh BatchBilling
                    RefreshBatchBill();
                }
                else
                {
                    // cancle save
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please Select invoice, before save", "Notification",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void RefreshBatchBill()
        {
            frmBatchBilling batchbill = (frmBatchBilling)Application.OpenForms["frmBatchBilling"];
            batchbill.button7_Click(null,null);
        }

        private void ClearRowNotCheck()
        {
            if (dataGridViewBillNote.Rows.Count > 0)
            {
                int loop = dataGridViewBillNote.Rows.Count;
                for (int i = loop-1;i>=0;i--)
                {
                    string check_0 = dataGridViewBillNote.Rows[i].Cells["CHECK_0"].Value.ToString();
                    if (check_0  != "True")
                    {
                        dataGridViewBillNote.Rows.RemoveAt(i);
                    }
                    else
                    {
                        // checkbox pass
                    }
                }
            }
        }

        private bool ProcessBillDetail(DataTable dt)
        {
            try
            {
                DataTable dtBillDetail = new DataTable();
                switch (page)
                {
                    case "New":
                        string BILLSEQ = PROC.GETDATA_SEQ("BillHead", "Last");
                        string BILLNO = PROC.GETDATA_NO("BillHead", "Last");
                        //common information
                        string IDCUST = txtCardcode.Text.Trim();
                        string INVDATE = datetimeInv.Value.Date.ToString("yyyyMMdd");
                        string DUEDATE = datetimeDuedate.Value.Date.ToString("yyyyMMdd");
                        string DOCDATE = datetimeDoc.Value.Date.ToString("yyyyMMdd");
                        //lopp each row
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            string LINEAMOUNT = dt.Rows[i]["DocTotal"].ToString();
                            string AMTOUTSTAND = dt.Rows[i]["AMTOUTSTAND"].ToString();
                            string NETAMT = dt.Rows[i]["AMT"].ToString();
                            string VATSUM = dt.Rows[i]["VATSUM"].ToString();
                            string WTBASE = dt.Rows[i]["WTBASE"].ToString();
                            string WTSUM = dt.Rows[i]["WTSUM"].ToString();
                            string CHECK_0 = dt.Rows[i]["CHECK_0"].ToString();
                            string LINECOMMENT = dt.Rows[i]["COMMENTDETAIL"].ToString();
                            string STA_0 = dt.Rows[i]["STA_0"].ToString();
                            string INVNO = dt.Rows[i]["INVNO"].ToString();
                            string DOCNUM = dt.Rows[i]["DocNum"].ToString();
                            string DOCTYPE = dt.Rows[i]["DOCTYPE"].ToString();

                            DataTable dtTemp = PROC.BillDataDetail(BILLSEQ, BILLNO, INVNO, DOCNUM, DOCTYPE, IDCUST, INVDATE, DOCDATE, DUEDATE, LINEAMOUNT, AMTOUTSTAND
                                                , NETAMT, CHECK_0, LINECOMMENT, STA_0, VATSUM, WTBASE, WTSUM);
                            dtBillDetail.Merge(dtTemp);
                        }
                        //next insert
                        DATA.INSERTBILLDETAIL(dtBillDetail);
                        return true;
                        break;
                    case "Open":
                        string opBILLSEQ = txtBillSEQ.Text;
                        string opBILLNO = txtBillNo.Text;
                        //common information
                        string opIDCUST = txtCardcode.Text.Trim();
                        string opINVDATE = datetimeInv.Value.Date.ToString("yyyyMMdd");
                        string opDUEDATE = datetimeDuedate.Value.Date.ToString("yyyyMMdd");
                        string opDOCDATE = datetimeDoc.Value.Date.ToString("yyyyMMdd");
                        //lopp each row
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            string LINEAMOUNT = dt.Rows[i]["DocTotal"].ToString();
                            string AMTOUTSTAND = dt.Rows[i]["AMTOUTSTAND"].ToString();
                            string NETAMT = dt.Rows[i]["AMT"].ToString();
                            string VATSUM = dt.Rows[i]["VATSUM"].ToString();
                            string WTBASE = dt.Rows[i]["WTBASE"].ToString();
                            string WTSUM = dt.Rows[i]["WTSUM"].ToString();
                            string CHECK_0 = dt.Rows[i]["CHECK_0"].ToString();
                            string LINECOMMENT = dt.Rows[i]["COMMENTDETAIL"].ToString();
                            string STA_0 = dt.Rows[i]["STA_0"].ToString();
                            string INVNO = dt.Rows[i]["INVNO"].ToString();
                            string DOCNUM = dt.Rows[i]["DocNum"].ToString();
                            string DOCTYPE = dt.Rows[i]["DOCTYPE"].ToString();

                            DataTable dtTemp = PROC.BillDataDetail(opBILLSEQ, opBILLNO, INVNO, DOCNUM, DOCTYPE, opIDCUST, opINVDATE, opDUEDATE, opDOCDATE, LINEAMOUNT, AMTOUTSTAND
                                                , NETAMT, CHECK_0, LINECOMMENT, STA_0, VATSUM, WTBASE, WTSUM);
                            dtBillDetail.Merge(dtTemp);

                        }
                        //next insert
                        DATA.INSERTBILLDETAIL(dtBillDetail);
                        return true;
                        break;
                }
                return false;
            }
            catch(Exception ex)
            {
                log.LogWrite("Error (frmBillingNote/ProcessBillDetail): " + ex.Message);
                MessageBox.Show("Error (frmBillingNote/ProcessBillDetail): " + ex.Message);
                return false;
            }
            
        }

        private string ProcessBillHead()
        {
            try
            {
                string INVDATE = datetimeInv.Value.Date.ToString("yyyyMMdd");
                string DUEDATE = datetimeDuedate.Value.Date.ToString("yyyyMMdd");
                string DOCDATE = datetimeDoc.Value.Date.ToString("yyyyMMdd");
                string AMT = txtSumAmt.Text;
                string NETAMT = AMT;
                string COMMENT = txtCommentHeader.Text;
                string STA_0 = txtStatus.Text;
                switch (page)
                {
                    case"New":
                        DataTable dtBillHead = new DataTable();
                        string BILLSEQ = PROC.GETDATA_SEQ("BillHead","Next");
                        string BILLNO = PROC.GETDATA_NO("BillHead","Next");
                        string IDCUST = txtCardcode.Text.Trim();
                        dtBillHead = PROC.BillDataHead(BILLSEQ, BILLNO, IDCUST, INVDATE, DUEDATE, DOCDATE, AMT, NETAMT, COMMENT, STA_0);
                        // next to Insert BillHead
                        DATA.INSERTBILLHEAD(dtBillHead);
                        // Update RUNNING NO 
                        DATA.UPDATEFMSRUNNO("Billing Note", BILLNO);
                        return BILLNO;
                        break;

                    case "Open":
                        string opBILLSEQ = txtBillSEQ.Text;
                        string opBILLNO = txtBillNo.Text;
                        string opIDCUST = txtCardcode.Text;
                        
                        dtBillHead = PROC.BillDataHead(opBILLSEQ, opBILLNO, opIDCUST, INVDATE, DUEDATE, DOCDATE, AMT, NETAMT, COMMENT, STA_0);
                        DATA.INSERTBILLHEAD(dtBillHead);
                        DATA.UPDATEFMSRUNNO("Billing Note", opBILLNO);
                        return opBILLNO;
                        break;
                }
                return "";
            }
            catch(Exception ex)
            {
                log.LogWrite("error (frmBillingNote/ProcessBillHead) : " + ex.Message);
                MessageBox.Show("error (frmBillingNote/ProcessBillHead) : " + ex.Message);
                return "";
            }
            
            
        }

        private bool checkBoxAlready()
        {
            if (dataGridViewBillNote.Rows.Count > 0)
            {
                for (int i=0;i<= dataGridViewBillNote.Rows.Count-1;i++)
                {
                    if (dataGridViewBillNote.Rows[i].Cells["CHECK_0"].Value.ToString() == "True")
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        private void dataGridViewBillNote_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // after edit cell
            decimal newvalue = Convert.ToDecimal(dataGridViewBillNote.Rows[e.RowIndex].Cells["AMT"].Value); //ช่องที่เรากรอดตัวเลขเอง;
            decimal DocTotal = 0;
            DocTotal = Convert.ToDecimal(dataGridViewBillNote.Rows[e.RowIndex].Cells["DocTotal"].Value); //LineAmount ยอดเต็ม
            decimal AMTOUTSTAND = 0;
            //get DOCNUM
            string[] DOCNUM = dataGridViewBillNote.Rows[e.RowIndex].Cells["INVNO"].Value.ToString().Split('-');
            string Doctype = dataGridViewBillNote.Rows[e.RowIndex].Cells["Doctype"].Value.ToString().Trim();
            decimal SUMAMT = PROC.GETDATA_SumAMT(DOCNUM[1], Doctype); //ยอดรวม AMT ทั้งหมดตามเลข DocNum,Doctype

            if (dataGridViewBillNote.Columns[e.ColumnIndex].Name == "AMT")
            {
                if (DocTotal > 0)
                {
                    switch (newvalue)
                    {
                        case var N when newvalue < 0:
                            //case กรอกข้อมูล < 0
                            newvalue = Convert.ToDecimal(dataGridViewBillNote.Rows[e.RowIndex].Cells["AMT"].Value); //ช่องที่เรากรอดตัวเลขเอง
                            DocTotal = Convert.ToDecimal(dataGridViewBillNote.Rows[e.RowIndex].Cells["DocTotal"].Value); //LineAmount ยอดเต็ม
                            AMTOUTSTAND = DocTotal - SUMAMT;
                            MessageBox.Show("value < 0 invalid value", "Notification", MessageBoxButtons.OK, MessageBoxIcon.None);
                            dataGridViewBillNote.Rows[e.RowIndex].Cells["AMT"].Value = dataGridViewBillNote.Rows[e.RowIndex].Cells["DocTotal"].Value;
                            dataGridViewBillNote.Rows[e.RowIndex].Cells["AMTOUTSTAND"].Value = DocTotal - Convert.ToDecimal(dataGridViewBillNote.Rows[e.RowIndex].Cells["AMT"].Value);
                            dataGridViewBillNote.RefreshEdit();
                            break;
                        case var N when newvalue > DocTotal:
                            //case กรอกข้อมูล > ยอดทั้งหมด
                            newvalue = Convert.ToDecimal(dataGridViewBillNote.Rows[e.RowIndex].Cells["AMT"].Value); //ช่องที่เรากรอดตัวเลขเอง
                            DocTotal = Convert.ToDecimal(dataGridViewBillNote.Rows[e.RowIndex].Cells["DocTotal"].Value); //LineAmount ยอดเต็ม
                            AMTOUTSTAND = DocTotal - SUMAMT;
                            MessageBox.Show("value > 'จำนวนเงินตามใบแจ้งหนี้' invalid value", "Notification", MessageBoxButtons.OK, MessageBoxIcon.None);
                            dataGridViewBillNote.Rows[e.RowIndex].Cells["AMT"].Value = dataGridViewBillNote.Rows[e.RowIndex].Cells["DocTotal"].Value;
                            dataGridViewBillNote.Rows[e.RowIndex].Cells["AMTOUTSTAND"].Value = DocTotal - Convert.ToDecimal(dataGridViewBillNote.Rows[e.RowIndex].Cells["AMT"].Value);
                            dataGridViewBillNote.RefreshEdit();
                            break;
                        default:
                            //case ถูกต้อง
                            newvalue = Convert.ToDecimal(dataGridViewBillNote.Rows[e.RowIndex].Cells["AMT"].Value); //ช่องที่เรากรอดตัวเลขเอง
                            DocTotal = Convert.ToDecimal(dataGridViewBillNote.Rows[e.RowIndex].Cells["DocTotal"].Value); //LineAmount ยอดเต็ม
                            if (DocTotal == SUMAMT)
                            {
                                //สมการ กรณีนี้ เปิดเต็มแล้วมา ลดที่หลัง
                                AMTOUTSTAND = DocTotal - newvalue;
                            }
                            else
                            {
                                //สมการ กรณี เปิดไม่เต็ม แล้วมาเพิ่มที่หลัง
                                AMTOUTSTAND = DocTotal - SUMAMT - newvalue;
                            }
                            
                            dataGridViewBillNote.Rows[e.RowIndex].Cells["AMTOUTSTAND"].Value = AMTOUTSTAND;
                            dataGridViewBillNote.RefreshEdit();
                            SHOW_SumAMT();
                            break;
                    }
                }
                else
                {
                    // case edit 'CN' invalid to not edit
                    MessageBox.Show("'CN' can not Edit","Notification",MessageBoxButtons.OK,MessageBoxIcon.None);
                    dataGridViewBillNote.Rows[e.RowIndex].Cells["AMT"].Value = dataGridViewBillNote.Rows[e.RowIndex].Cells["DocTotal"].Value;
                    dataGridViewBillNote.Rows[e.RowIndex].Cells["AMTOUTSTAND"].Value = DocTotal - Convert.ToDecimal(dataGridViewBillNote.Rows[e.RowIndex].Cells["AMT"].Value);
                    dataGridViewBillNote.RefreshEdit();
                    
                }
            }
        }

        
    }
}
