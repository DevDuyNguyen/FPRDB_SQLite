using BLL;
using BLL.Common;
using BLL.DTO;
using BLL.Services;
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
        CompositionRoot root = new CompositionRoot();
        FuzzySetService service;
        public frmAddDiscreteFuzzySet()
        {
            InitializeComponent();
            service = root.getFuzzySetService();
        }

        // Click "Cancel" button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Helper for adding a new discrete fuzzy set
        private void HandleDiscreteFuzzySet<T>()
        {
            var dto = discreteFuzzySetInfo.getDiscreteFuzzySet<T>();
            if (service.checkIfFuzzySetValid(dto))
                service.createFuzzySet<T>(dto);
        }
        // Click "Save" button
        private void btnSave_Click(object sender, EventArgs e)
        {
            FieldType type = discreteFuzzySetInfo.getFuzzySetType();
            switch (type)
            {
                case FieldType.distFS_INT:
                    HandleDiscreteFuzzySet<int>();
                    break;
                case FieldType.distFS_FLOAT:
                    HandleDiscreteFuzzySet<float>();
                    break;
                case FieldType.distFS_TEXT:
                    HandleDiscreteFuzzySet<string>();
                    break;

            }
        }
    }
}