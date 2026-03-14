using BLL;
using BLL.Common;
using BLL.DTO;
using BLL.Services;
using DevExpress.Map.Kml.Model;
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
    public partial class frmAddDiscreteFuzzySet : DevExpress.XtraEditors.XtraForm
    {
        CompositionRoot compRoot;
        FuzzySetService service;
        public frmAddDiscreteFuzzySet(CompositionRoot compRoot)
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
        // Helper để xử lý tạo Discrete Fuzzy Set dựa trên kiểu dữ liệu được chọn
        private void HandleDiscreteFuzzySet<T>()
        {
            var dto = discreteFuzzySetInfo.getDiscreteFuzzySet<T>();
            service.createFuzzySet<T>(dto);
        }
        // Hàm xử lý khi click "Save" button
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!discreteFuzzySetInfo.ValidateControls())
            {
                XtraMessageBox.Show("Please fill out all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FieldType type = discreteFuzzySetInfo.getFuzzySetType();
            switch (type)
            {
                case FieldType.INT:
                    HandleDiscreteFuzzySet<int>();
                    break;
                case FieldType.FLOAT:
                    HandleDiscreteFuzzySet<float>();
                    break;
                case FieldType.VARCHAR:
                    HandleDiscreteFuzzySet<string>();
                    break;

            }
            XtraMessageBox.Show("Discrete fuzzy set added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}