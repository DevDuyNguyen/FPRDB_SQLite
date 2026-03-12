using BLL;
using BLL.DomainObject;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPRDB_SQLite.GUI
{
    public partial class frmNewSchema : DevExpress.XtraEditors.XtraForm
    {
        public class SchemaAttribute
        {
            [DisplayName("Primary Key")]
            public bool isPrimaryKey { get; set; } = false;

            [DisplayName("Attribute Name")]
            public string attributeName { get; set; } = string.Empty;

            [DisplayName("Data Type")]
            public FieldType dataType { get; set; } = FieldType.INT;
        }
        public frmNewSchema()
        {
            InitializeComponent();
            InitGrid();
        }
        // Hàm khởi tạo GridControl
        private void InitGrid()
        {
            grdcSchemaAttribute.DataSource = new BindingList<SchemaAttribute>();
            grdvSchemaAttribute.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            repositoryItemComboBox1.Items.AddRange(new object[]
            {
                FieldType.INT,
                FieldType.FLOAT,
                FieldType.VARCHAR
            });
            repositoryItemComboBox1.NullText = "<Choose data type>";
            repositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string schemaName = txtSchemaName.Text.Trim();
            grdvSchemaAttribute.CloseEditor();
            grdvSchemaAttribute.UpdateCurrentRow();

            var rows = grdvSchemaAttribute.DataSource as BindingList<SchemaAttribute>;

            //schema = convertGridToSchema(schemaName, rows);
            //defineFPRDBSchema(schema)
        }
        private FPRDBSchema convertGridToSchema(string schemaName, BindingList<SchemaAttribute> attributes)
        {
            List<Field> fields = new List<Field>();
            List<string> primaryKeys = new List<string>();
            foreach (var attr in attributes)
            {
                var fieldInfo = new FieldInfo(attr.dataType, 255);
                var field = new Field(attr.attributeName, fieldInfo);
                fields.Add(field);
                if (attr.isPrimaryKey)
                {
                    primaryKeys.Add(attr.attributeName);
                }
            }
            return new FPRDBSchema(schemaName, fields, primaryKeys);
        }
    }
}