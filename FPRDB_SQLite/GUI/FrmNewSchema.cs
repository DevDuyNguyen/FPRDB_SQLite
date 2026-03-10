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
            public string dataType { get; set; } = string.Empty;
            public SchemaAttribute() { }
            public SchemaAttribute(bool isPk, string name, string type)
            {
                isPrimaryKey = isPk;
                attributeName = name;
                dataType = type;
            }
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
            repositoryItemComboBox1.Items.AddRange(new string[]
            {
                "INTEGER",
                "TEXT",
                "FLOAT"
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

            foreach (var attr in rows)
            {
                Debug.WriteLine("PK: " + attr.isPrimaryKey + ", Name: " + attr.attributeName + ", Type: " + attr.dataType);
            }
        }
    }
}