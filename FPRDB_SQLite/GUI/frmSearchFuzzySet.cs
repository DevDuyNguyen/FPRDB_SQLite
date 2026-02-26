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
    public partial class frmSearchFuzzySet : DevExpress.XtraEditors.XtraForm
    {
        public frmSearchFuzzySet()
        {
            InitializeComponent();
            lstFuzzySets.Items.Add("Very Low");
        }

        private void lstFuzzySets_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            // Click position
            var index = lstFuzzySets.IndexFromPoint(e.Location);

            if (index >= 0)
            {
                // Update SelectedIndex after double-click
                lstFuzzySets.SelectedIndex = index;

                // Get fuzzy set info based on the selected item

                // Open the form to edit the selected fuzzy set (extend: type of fuzzy set, pass the fuzzy set info)
                // if...else...
                new frmManageDiscreteFuzzySet().ShowDialog();
            }
        }
    }
}