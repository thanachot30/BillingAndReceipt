using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BillAppSapB1
{
    public partial class frmUser : Form
    {
        CONNECTTION CONN = new CONNECTTION();
        Log Log = new Log();
        int index = 0;
        public frmUser()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sql1 = "";
                bool result = CONN.ConnectSQL("Bill", 0);
                if (result == true)
                {
                    DataTable dtchkTemp = new DataTable();
                    string STREXIST = "SELECT name FROM sys.tables WHERE name = 'FMSUSERLOGIN'";
                    SqlDataAdapter cmdquery = new SqlDataAdapter(STREXIST, CONN.connection);
                    cmdquery.Fill(dtchkTemp);
                    if (dtchkTemp.Rows.Count > 0)
                    {
                        sql1 = "INSERT INTO FMSUSERLOGIN (ID,USERNAME,PASSWORD,ROLE)" + Environment.NewLine;
                        sql1 += "VALUES ('" + txtuserid.Text + "','" + txtusername.Text + "','" + txtuserpass.Text + "','" + cboRole.Text + "')";
                        SqlCommand cmd1 = new SqlCommand(sql1, CONN.connection);
                        cmd1.ExecuteNonQuery();
                        CONN.connection.Close();
                        MessageBox.Show("Save User Complete", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid table name 'FMSUSERLOGIN'.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogWrite("Error (frmUser/button save): " + ex.Message);
                MessageBox.Show("Error(frmUser / button save): " + ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmUser_Load(object sender, EventArgs e)
        {
            //fix index 0
            DataTable dtConfig = CONN.ReadConfigDB();
            lblDatabase.Text = dtConfig.Rows[index]["SERVER"].ToString();
            txtusername.Select();
            //
            int nextUserId = NextLastUserId();
            txtuserid.Text = nextUserId.ToString();
        }

        public int NextLastUserId()
        {
            bool result = CONN.ConnectSQL("Bill", 0);
            if (result == true)
            {
                DataTable dtchkTemp = new DataTable();
                string sql1 = "SELECT TOP 1 *  from FMSUSERLOGIN ORDER BY ID DESC";
                SqlDataAdapter cmdquery = new SqlDataAdapter(sql1, CONN.connection);
                cmdquery.Fill(dtchkTemp);
                if (dtchkTemp.Rows.Count == 0)
                {
                    //first user
                    return 1;
                }
                else
                {
                    string LastUserId = dtchkTemp.Rows[0]["ID"].ToString();
                    return int.Parse(LastUserId) + 1;
                }
            }
            else
            {
                return 0;
            }

        }

        
    }
}
