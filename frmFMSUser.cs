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
    public partial class frmFMSUser : Form
    {
        
        public frmFMSUser()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtFMSuser.Text == "admin" && txtFMSpass.Text == "admin")
            {
                //control poproty other form
                frmSettingDB.frmSettingDBInstance.panel1.Enabled = true;
                frmSettingDB.frmSettingDBInstance.groupBox1.Enabled = true;
                frmSettingDB.frmSettingDBInstance.groupBox2.Enabled = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Username and Passward: Incorrect", "Notification",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
    }
}
