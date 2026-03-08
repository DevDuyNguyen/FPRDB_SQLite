using BLL.Common;
using BLL.Services;
using DevExpress.Map.Kml.Model;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
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
        private CompositionRoot compRoot;
        private FuzzySetService service;
        public frmAddContinuousFuzzySet(CompositionRoot compRoot)
        {
            this.compRoot = compRoot;
            this.service = this.compRoot.getFuzzySetService();
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
            if (!continuosFuzzySetInfo.ValidateControls())
            {
                XtraMessageBox.Show("Please fix validation errors before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var dto = continuosFuzzySetInfo.GetContinuousFuzzySet();
            if (service.checkIfFuzzySetValid(dto))
            {
                service.createFuzzySet<float>(dto);
                XtraMessageBox.Show("Continuous fuzzy set added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }
    }
}