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
using GUI.GlobalStates;
using BLL.Exceptions;
using BLL.DTO;
using System.IO;

namespace FPRDB_SQLite.GUI
{
    public partial class frmNewRelation : DevExpress.XtraEditors.XtraForm
    {
        private readonly CompositionRoot compRoot;
        private readonly DatabaseService databaseService;
        private FPRDBSchemaDTO selectedSchema;
        private FPRDBRelationService fprdbRelationService;
        public frmNewRelation(CompositionRoot compRoot)
        {
            InitializeComponent();
            this.compRoot = compRoot;
            this.databaseService = compRoot.getDatabaseService();
            this.fprdbRelationService = compRoot.getFPRDBRelationService();
            cboSchemaName.Properties.Items.AddRange(AppStates.loadFPRDBSchemas.Select(s => s.schemaName).ToArray());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string relName = txtRelName.Text.Trim();
            string schemaName = (string)cboSchemaName.EditValue;
            try
            {
                FPRDBSchemaDTO sche=null;
                foreach(FPRDBSchemaDTO v in AppStates.loadFPRDBSchemas)
                {
                    if(v.schemaName==schemaName)
                    {
                        sche = v;
                        break;
                    }
                }
                FPRDBRelationDTO rel = new FPRDBRelationDTO(relName, sche, schemaName);
                
                if (this.fprdbRelationService.createFPRDBRelation(rel))
                {
                    XtraMessageBox.Show("Relation added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    AppStates.loadFPRDBSchemaRelations.Add(rel);
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (SemanticException ex)
            {
                XtraMessageBox.Show($"Error: {ex.Message}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.DialogResult = DialogResult.Abort;
            }
            catch (InvalidDataException ex)
            {
                XtraMessageBox.Show($"Error: {ex.Message}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.DialogResult = DialogResult.Abort;
            }

        }

        private void cboSchemaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = cboSchemaName.SelectedItem?.ToString();
            selectedSchema = AppStates.loadFPRDBSchemas.FirstOrDefault(s => s.schemaName == selectedName);
        }
    }
}