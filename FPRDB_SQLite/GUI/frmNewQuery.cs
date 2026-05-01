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
using GUI.GlobalStates;

namespace FPRDB_SQLite.GUI.GUI
{
    public partial class frmNewQuery : DevExpress.XtraEditors.XtraForm
    {
        public string QueryName { get; private set; }
        private InDataBaseSQLFileService inDataBaseSQLFileService;
        public frmNewQuery(InDataBaseSQLFileService inDataBaseSQLFileService)
        {
            this.inDataBaseSQLFileService = inDataBaseSQLFileService;
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string queryName = txtQueryName.Text.Trim();
            this.QueryName = queryName;
            try
            {
                this.inDataBaseSQLFileService.createFile(this.QueryName, null);
                AppStates.listOfInDatabaseSQLFiles.Add(this.QueryName);
            }
            catch(InvalidOperationException ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}