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
    public partial class frmMain : Form
    {
        public int indexConfig { get; set; }
        Log Log = new Log();
        CONNECTTION CONN = new CONNECTTION();
        DATACLASS DATA = new DATACLASS();
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int window_width = Screen.PrimaryScreen.Bounds.Width;
            int window_height = Screen.PrimaryScreen.Bounds.Height;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            //
            frmLogin frmLogin = new frmLogin();
            string user = frmLogin.username;
            string coname = frmLogin.CONAME;
            indexConfig = getIndex(coname);
            DataTable dtConfig = new DataTable();
            dtConfig = CONN.ReadConfigDB();
            //
            DATA.indexConfig = indexConfig;
            //
            lblusername.Text = user.Trim();
            lblconame.Text = coname.Trim();
            lblServer.Text = dtConfig.Rows[indexConfig]["SERVER"].ToString().Trim();
            lblSource.Text = dtConfig.Rows[indexConfig]["DBSource"].ToString().Trim();
            lblBill.Text = dtConfig.Rows[indexConfig]["DBBILL"].ToString().Trim();
            //
            button1_Click(this,e);
            //
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ////' use panal3 for display page so page display on panal
            Panel3.Controls.Clear();
            frmMainBilling frmMainBilling = new frmMainBilling();
            frmMainBilling.indexConfig = indexConfig;
            frmMainBilling.TopLevel = false;
            
            Panel3.Controls.Add(frmMainBilling);
            int sizeX = Panel3.Width;
            int sizeY = Panel3.Height;
            frmMainBilling.Width = sizeX;
            frmMainBilling.Height = sizeY;
            frmMainBilling.Show();
        }

        public int getIndex(string coname)
        {
            string[] words = coname.Split(':');
            return (int.Parse(words[0]) - 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ////' use panal3 for display page so page display on panal
            Panel3.Controls.Clear();
            frmMainReceipt frmMainReceipt = new frmMainReceipt();
            frmMainReceipt.TopLevel = false;
            Panel3.Controls.Add(frmMainReceipt);
            int sizeX = Panel3.Width;
            int sizeY = Panel3.Height;
            frmMainReceipt.Width = sizeX;
            frmMainReceipt.Height = sizeY;
            frmMainReceipt.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Panel3.Controls.Clear();
            frmSetting frmSetting = new frmSetting();
            frmSetting.TopLevel = false;
            Panel3.Controls.Add(frmSetting);
            int sizeX = Panel3.Width;
            int sizeY = Panel3.Height;
            frmSetting.Width = sizeX;
            frmSetting.Height = sizeY;
            frmSetting.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        
    }
}
