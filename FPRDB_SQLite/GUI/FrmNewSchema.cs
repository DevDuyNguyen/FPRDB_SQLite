using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.DTO;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
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
            [DisplayName("Length")]
            public int length { get; set; } = 0;
        }
        private CompositionRoot compRoot;
        //private FPRDBSchemaService service;
        public frmNewSchema(CompositionRoot compRoot)
        {
            InitializeComponent();
            this.compRoot = compRoot;
            //this.service = compRoot.getFPRDBSchemaService();
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

        // Hàm xử lý khi click "Cancel" button
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Hàm xử lý khi click "Save" button
        private void btnSave_Click(object sender, EventArgs e)
        {
            string schemaName = txtSchemaName.Text.Trim();
            grdvSchemaAttribute.CloseEditor();
            grdvSchemaAttribute.UpdateCurrentRow();

            var rows = grdvSchemaAttribute.DataSource as BindingList<SchemaAttribute>;

            FPRDBSchemaDTO schema = convertGridToSchema(schemaName, rows);

            //defineFPRDBSchema(schema);
            XtraMessageBox.Show("Schema added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();

        }

        // Hàm chuyển đổi dữ liệu từ GridControl thành FPRDBSchema
        private FPRDBSchemaDTO convertGridToSchema(string schemaName, BindingList<SchemaAttribute> attributes)
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
            return new FPRDBSchemaDTO(schemaName, fields, primaryKeys);
        }

        private void grdvSchemaAttribute_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;

            // Only apply logic to the "Length" column
            if (view.FocusedColumn.FieldName != "length")
                return;

            // Get the DataType value of the current row
            object dataType = view.GetRowCellValue(view.FocusedRowHandle, "dataType");

            // Cancel editing if DataType is NOT varchar
            if (dataType == null || (FieldType)dataType != FieldType.VARCHAR)
            {
                e.Cancel = true; // Prevent editor from opening
            }
        }

        private void grdvSchemaAttribute_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName != "length")
                return;

            object dataType = grdvSchemaAttribute.GetRowCellValue(e.RowHandle, "dataType");

            if (dataType == null || (FieldType)dataType != FieldType.VARCHAR)
            {
                e.Appearance.BackColor = Color.LightGray;
                e.Appearance.ForeColor = Color.DarkGray;
            }
        }

        private void grdvSchemaAttribute_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName != "dataType")
                return;

            if (e.Value == null || (FieldType)e.Value != FieldType.VARCHAR)
            {
                grdvSchemaAttribute.SetRowCellValue(e.RowHandle, "length", null);
            }
        }
    }
}