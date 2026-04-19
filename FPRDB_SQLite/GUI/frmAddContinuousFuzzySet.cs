using BLL.Common;
using BLL.Exceptions;
using BLL.Services;
using DevExpress.XtraEditors;
using System;
using System.IO;
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

        // Hàm xử lý khi click "Cancel" button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Hàm xử lý khi click "Save" button
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!continuosFuzzySetInfo.ValidateControls())
            {
                XtraMessageBox.Show("Please fill out all required fields or fix invalid formats.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var dto = continuosFuzzySetInfo.getContinuousFuzzySet();
            try
            {
                service.createFuzzySet<float>(dto);
                XtraMessageBox.Show("Continuous fuzzy set added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
            catch (InvalidOperationException ex)
            {
                XtraMessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidDataException ex)
            {
                XtraMessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SQLExecutionException ex)
            {
                XtraMessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}