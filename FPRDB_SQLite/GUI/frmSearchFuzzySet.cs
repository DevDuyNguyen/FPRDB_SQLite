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
            cboNameFuzzySet.Properties.Items.Add("Discrete");
            cboNameFuzzySet.Properties.Items.Add("Continuous");
            pnlResult.Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string selectedType = cboNameFuzzySet.Text;
            switch (selectedType)
            {
                case "Discrete":
                    pnlResult.Visible = true;
                    discreteFuzzySetInfo.Visible = true;
                    continuosFuzzySetInfo.Visible = false;
                    break;
                case "Continuous":
                    pnlResult.Visible = true;
                    continuosFuzzySetInfo.Visible = true;
                    discreteFuzzySetInfo.Visible = false;
                    break;
                default:
                    XtraMessageBox.Show("Please select a valid fuzzy set type.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }
    }
}