using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace BillAppSapB1
{
    public partial class frmSettingDB : Form
    {
        public static frmSettingDB frmSettingDBInstance;
        CONNECTTION CONN = new CONNECTTION();
        DATACLASS DATA = new DATACLASS();
        Log Log = new Log();
        public frmSettingDB()
        {
            InitializeComponent();
            frmSettingDBInstance = this;

        }

        private void frmSettingDB_Load(object sender, EventArgs e)
        {
            // load page 
            panel1.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            DipyConfig(0);
        }

        private void DipyConfig(int index)
        {
            try
            {
                DataTable dtconfigDB = CONN.ReadConfigDB();
                if (dtconfigDB.Rows.Count > 0)
                {
                    txtid.Text = dtconfigDB.Rows[index]["ID"].ToString();
                    txtconame.Text = dtconfigDB.Rows[index]["CONAME"].ToString();
                    txtbillserver.Text = dtconfigDB.Rows[index]["SERVER"].ToString();
                    txtbilluser.Text = dtconfigDB.Rows[index]["USER"].ToString();
                    txtbillpass.Text = dtconfigDB.Rows[index]["PASSWORD"].ToString();
                    txtbilldb.Text = dtconfigDB.Rows[index]["DBBILL"].ToString();
                    //
                    txtsapserver.Text = dtconfigDB.Rows[index]["SERVER"].ToString();
                    txtsapdb.Text = dtconfigDB.Rows[index]["DBSource"].ToString();
                    txtsapuser.Text = dtconfigDB.Rows[index]["USERSAP"].ToString();
                    txtsappass.Text = dtconfigDB.Rows[index]["PASSSAP"].ToString();
                    //
                }
            }
            catch (Exception ex)
            {
                Log.LogWriter(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            //connection and save config 
            string id = txtid.Text;
            string coname = txtconame.Text;
            string billserver = txtbillserver.Text;
            string billuser = txtbilluser.Text;
            string billpass = txtbillpass.Text;
            string billDB = txtbilldb.Text;
            
            //
            string sapserver = txtsapserver.Text;
            string sapuser = txtsapuser.Text;
            string sappass = txtsappass.Text;
            string sapDB = txtsapdb.Text;
           
            dt.Columns.Add("ID");
            dt.Columns.Add("CONAME");
            dt.Columns.Add("SERVER");
            dt.Columns.Add("USER");
            dt.Columns.Add("PASSWORD");
            dt.Columns.Add("DBBILL");
            dt.Columns.Add("SERVERSource");
            dt.Columns.Add("USERSAP");
            dt.Columns.Add("PASSSAP");
            dt.Columns.Add("DBSource");

            string[] row = { id, coname, billserver, billuser ,billpass, billDB, sapserver, sapuser,sappass,sapDB};
            dt.Rows.Add(row);
            try
            {
                string result = CONN.TestConnectSQL(dt);
                if (result == "true")
                {
                    //if (CONN.ConnectSQL("Bill", Convert.ToInt32(txtid.Text) - 1) == true
                    //&& CONN.ConnectSQL("Source", Convert.ToInt32(txtid.Text) - 1) == true)
                    //{
                    //    CONN.connection.Close();

                    //}
                    // update new data to datatable
                    DataTable dtConfig = new DataTable();
                    dtConfig = CONN.ReadConfigDB();
                    
                    dtConfig.Rows[Convert.ToInt32(id) - 1]["ID"] = id;
                    dtConfig.Rows[Convert.ToInt32(id) - 1]["CONAME"] = coname;
                    dtConfig.Rows[Convert.ToInt32(id) - 1]["SERVER"] = billserver;
                    dtConfig.Rows[Convert.ToInt32(id) - 1]["USER"] = billuser;
                    dtConfig.Rows[Convert.ToInt32(id) - 1]["PASSWORD"] = billpass;
                    dtConfig.Rows[Convert.ToInt32(id) - 1]["DBBILL"] = billDB;
                    //
                    dtConfig.Rows[Convert.ToInt32(id) - 1]["SERVERSource"] = sapserver;
                    dtConfig.Rows[Convert.ToInt32(id) - 1]["USERSAP"] = sapuser;
                    dtConfig.Rows[Convert.ToInt32(id) - 1]["PASSSAP"] = sappass;
                    dtConfig.Rows[Convert.ToInt32(id) - 1]["DBSource"] = sapDB;
                    //Save new Xml Config
                    XElement BOM = new XElement("BOM");
                    XElement BO = new XElement("BO");
                    if (dtConfig.Rows.Count > 0)
                    {
                        XElement DocumentsLine = new XElement("Document_Lines");
                        for (int i=0;i <= dtConfig.Rows.Count-1;i++)
                        {
                            XElement Lrow = new XElement("row");

                            XElement xmlID = new XElement("ID", dtConfig.Rows[i]["ID"]);
                            XElement xmlSERVER = new XElement("SERVER", dtConfig.Rows[i]["SERVER"]);
                            XElement xmlCONAME = new XElement("CONAME", dtConfig.Rows[i]["CONAME"]);
                            XElement xmlUSER = new XElement("USER", dtConfig.Rows[i]["USER"]);
                            XElement xmlPASSWORD = new XElement("PASSWORD", dtConfig.Rows[i]["PASSWORD"]);
                            XElement xmlDBBill = new XElement("DBBILL", dtConfig.Rows[i]["DBBILL"]);
                            //
                            XElement xmlSERVERSource = new XElement("SERVERSource", dtConfig.Rows[i]["SERVERSource"]);
                            XElement xmlUSERSAP = new XElement("USERSAP", dtConfig.Rows[i]["USERSAP"]);
                            XElement xmlPASSSAP = new XElement("PASSSAP", dtConfig.Rows[i]["PASSSAP"]);
                            XElement xmlDBSource = new XElement("DBSource", dtConfig.Rows[i]["DBSource"]);
                            //
                            Lrow.Add(xmlID);
                            Lrow.Add(xmlSERVER);
                            Lrow.Add(xmlCONAME);
                            Lrow.Add(xmlUSER);
                            Lrow.Add(xmlPASSWORD);
                            Lrow.Add(xmlDBBill);
                            //
                            Lrow.Add(xmlSERVERSource);
                            Lrow.Add(xmlUSERSAP);
                            Lrow.Add(xmlPASSSAP);
                            Lrow.Add(xmlDBSource);
                            //
                            DocumentsLine.Add(Lrow);
                        }
                        BO.Add(DocumentsLine);
                        BOM.Add(BO);

                        //Generate xml file
                        var reader = BOM.CreateReader();
                        reader.ReadInnerXml();
                        reader.MoveToContent();

                        XmlWriterSettings settingPath = new XmlWriterSettings();
                        settingPath.Indent = true;

                        String pathaddr = Application.StartupPath.ToString() + "\\config\\DBCONFIG.xml";
                        StreamWriter path = new StreamWriter(pathaddr);
                        using (XmlTextWriter writer = new XmlTextWriter(path))
                        {
                            writer.WriteStartDocument();
                            writer.WriteRaw(reader.ReadOuterXml());
                        }
                        //Create FMSUSERLOGIN table
                        int index = Convert.ToInt32(id) - 1;
                        DATA.CREATEFMSUSERLOGIN(index);
                        //
                        MessageBox.Show("Save and Connect", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("(frmSettingDB/SaveXml: dtconfig.count is null)");
                    }
                }
                else
                {
                    MessageBox.Show("Fail Connect: " + result 
                        + Environment.NewLine  
                        + "****Plase try agin.****", "Notification"
                        , MessageBoxButtons.OK
                        , MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Log.LogWriter(ex.Message);
                MessageBox.Show("Error (frmSettingDB/button save: ): " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmFMSUser frmFMSUser = new frmFMSUser();
            frmFMSUser.TopMost = true;
            frmFMSUser.Show();
        }
    }
}
