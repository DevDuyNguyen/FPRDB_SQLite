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

namespace FPRDB_SQLite.GUI
{
    public partial class frmAddContinuousFuzzySet : DevExpress.XtraEditors.XtraForm
    {
        public frmAddContinuousFuzzySet()
        {
            InitializeComponent();
        }

        // Click "Cancel" button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Click "Save" button
        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}