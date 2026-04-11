using BLL;
using BLL.Common;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Services;
using DevExpress.Map.Kml.Model;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPRDB_SQLite.GUI
{
    public partial class frmAddDiscreteFuzzySet : DevExpress.XtraEditors.XtraForm
    {
        CompositionRoot compRoot;
        FuzzySetService service;
        DatabaseService dbService;
        List<FieldType> defineDomain4FuzzySet;
        public frmAddDiscreteFuzzySet(CompositionRoot compRoot)
        {
            this.compRoot = compRoot;
            this.service = this.compRoot.getFuzzySetService();
            this.dbService = this.compRoot.getDatabaseService();
            InitializeComponent();

            this.defineDomain4FuzzySet = this.dbService.getDefineDomainForDistFuzzSet();
            this.discreteFuzzySetInfo.setDefineDomain4FuzzySet(this.defineDomain4FuzzySet);
        }

        // Hàm xử lý khi click "Cancel" button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Helper để xử lý tạo Discrete Fuzzy Set dựa trên kiểu dữ liệu được chọn
        private bool HandleDiscreteFuzzySet<T>()
        {
            var dto = discreteFuzzySetInfo.getDiscreteFuzzySet<T>();
            try
            {
                service.createFuzzySet<T>(dto);
                return true;
            }
            catch (InvalidOperationException ex)
            {
                XtraMessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (SQLExecutionException ex)
            {
                XtraMessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch(InvalidDataException ex)
            {
                XtraMessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        // Hàm xử lý khi click "Save" button
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!discreteFuzzySetInfo.ValidateControls())
            {
                XtraMessageBox.Show("Please fill out all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FieldType choosenType = discreteFuzzySetInfo.getFuzzySetType();
            bool resultState=false;
            switch (choosenType)
            {
                
                case FieldType.INT:
                    resultState=HandleDiscreteFuzzySet<int>();
                    break;
                case FieldType.FLOAT:
                    resultState=HandleDiscreteFuzzySet<float>();
                    break;
                case FieldType.VARCHAR:
                    resultState=HandleDiscreteFuzzySet<string>();
                    break;
            }
            if (resultState == true)
            {
                XtraMessageBox.Show("Discrete fuzzy set added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
            
        }
    }
}