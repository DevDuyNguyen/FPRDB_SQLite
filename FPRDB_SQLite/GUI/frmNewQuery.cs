using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPRDB_SQLite.GUI.GUI
{
    public partial class frmNewQuery : DevExpress.XtraEditors.XtraForm
    {
        public string QueryName { get; private set; }
        public frmNewQuery()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string queryName = txtQueryName.Text.Trim();
            // Check error (blank and existed)
            this.QueryName = queryName;
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}