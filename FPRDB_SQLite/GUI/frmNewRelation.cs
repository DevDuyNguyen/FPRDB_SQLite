using BLL.Common;
using BLL.DomainObject;
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
    public partial class frmNewRelation : DevExpress.XtraEditors.XtraForm
    {
        private readonly CompositionRoot compRoot;
        private readonly DatabaseService databaseService;
        private readonly List<FPRDBSchema> schemas;
        private FPRDBSchema selectedSchema;
        public frmNewRelation(CompositionRoot compRoot)
        {
            InitializeComponent();
            this.compRoot = compRoot;
            this.databaseService = compRoot.getDatabaseService();
            this.schemas = databaseService.getFPRDBSchemas();
            cboSchemaName.Properties.Items.AddRange(schemas.Select(s => s.getSchemaName()).ToArray());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string relName = txtRelName.Text.Trim();
            // createFPRDBRelation(relName, selectedSchema, selectedSchema.getSchemaName());
        }

        private void cboSchemaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = cboSchemaName.SelectedItem?.ToString();
            selectedSchema = schemas.FirstOrDefault(s => s.getSchemaName() == selectedName);
        }
    }
}