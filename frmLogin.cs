using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BillAppSapB1
{
    public partial class frmLogin : Form
    {
        Log Log = new Log();
        CONNECTTION CONN = new CONNECTTION();
        DATACLASS DATA = new DATACLASS();
        public int index;
        public static string username = "";
        public static string CONAME = "";

        public frmLogin()
        {
            InitializeComponent();
            index = 0;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmSetting_DB_User frmSetting_DB_User = new frmSetting_DB_User();
            frmSetting_DB_User.Show();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            Log.LogWriter("Start program" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            GetCONAMEcbo();
            cboCONAME.SelectedIndex = 0;
           
        }

        public void GetCONAMEcbo()
        {
            //read coname from Config not data base
            int index = 0;
            bool result = CONN.ConnectSQL("Bill", index);
            if (result == true)
            {
                DataTable dtCOnfig = CONN.ReadConfigDB();
                cboCONAME.Items.Add((index+1)+":"+ dtCOnfig.Rows[index]["CONAME"].ToString());
                return;
            }
            else
            {
                MessageBox.Show("Please Seting Database","Notification",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((txtusername.Text != "") && (txtpass.Text != "") && (cboCONAME.Text != ""))
                {
                    int index = getIndex();
                    //test connection bill and base
                    if (CONN.ConnectSQL("Bill",index))
                    {
                        CONN.connection.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Connect to Bill");
                        return;
                    }
                    if (CONN.ConnectSQL("Source", index))
                    {
                        CONN.connection.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Connect to Source");
                        return;
                    }
                    // validate user Login
                    if (Validate_User(index))
                    {
                        username = txtusername.Text;
                        CONAME = cboCONAME.Text;
                        // Create init table
                        DATA.indexConfig = index;
                        DATA.CreateTableInit();
                        //
                        this.Hide();
                        frmMain frmMain = new frmMain();
                        frmMain.indexConfig = index;
                        frmMain.Show();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Plase Fill data","Notification",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
            }
            catch(Exception ex)
            {
                Log.LogWrite("Error (frmLogin/Button Login):" + ex.Message);
                MessageBox.Show(ex.Message);
                return;

            }

        }

        public int getIndex()
        {
            string[] words = cboCONAME.Text.Split(':');
            return (int.Parse(words[0]) - 1);
        }

        public bool Validate_User(int index)
        {
            string username = txtusername.Text.Trim();
            string pass = txtpass.Text.Trim();
            DataTable dtchkTemp = new DataTable();

            CONN.ConnectSQL("Bill", index);
            string sql1 = "SELECT * FROM FMSUSERLOGIN WHERE USERNAME = '" + username + "'";
            SqlDataAdapter cmdquery = new SqlDataAdapter(sql1, CONN.connection);
            cmdquery.Fill(dtchkTemp);
            if (dtchkTemp.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                string sql = "SELECT * FROM FMSUSERLOGIN WHERE USERNAME = '" + username 
                    + "' AND PASSWORD = '" + pass + "'";
                SqlDataAdapter cmd = new SqlDataAdapter(sql, CONN.connection);
                cmdquery.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    // Login Complete
                    return true;
                }
                else
                {
                    Log.LogWrite("Error (frmLogin/Validate_User: Password invalid)");
                    MessageBox.Show("Password invalid", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            else
            {
                Log.LogWrite("Error (frmLogin/Validate_User: Username invalid)");
                MessageBox.Show("Username invalid", "Notification",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return false;
            }
        }
    }
}
