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
using BLL.Common;
using BLL.Services;

namespace FPRDB_SQLite.GUI
{
    public partial class frmAddContinuousFuzzySet : DevExpress.XtraEditors.XtraForm
    {
        private CompositionRoot root = new CompositionRoot();
        private FuzzySetService service;
        public frmAddContinuousFuzzySet()
        {
            InitializeComponent();
            service = root.getFuzzySetService();
        }

        // Click "Cancel" button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Click "Save" button
        private void btnSave_Click(object sender, EventArgs e)
        {
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