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
    public partial class frmMainBilling : Form
    {
        public int indexConfig { get; set; }
        public frmMainBilling()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //
            frmBatchBilling frmBatchBilling = new frmBatchBilling();
            frmBatchBilling.indexConfig = indexConfig;
            frmBatchBilling.Show();
        }

        private void frmMainBilling_Load(object sender, EventArgs e)
        {

        }
    }
}
