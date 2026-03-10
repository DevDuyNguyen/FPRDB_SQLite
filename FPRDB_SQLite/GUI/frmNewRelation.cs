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
        public frmNewRelation(CompositionRoot compRoot)
        {
            InitializeComponent();
            this.compRoot = compRoot;
            this.databaseService = compRoot.getDatabaseService();
            loadSchemas();
        }
        private void loadSchemas()
        {
            List<FPRDBSchema> schemas = databaseService.getFPRDBSchemas();
            cboSchemaName.Properties.Items.AddRange(schemas.Select(s => s.getSchemaName()).ToArray());
        }
    }
}