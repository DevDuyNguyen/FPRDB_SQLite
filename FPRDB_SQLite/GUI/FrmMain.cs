using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Services;
using BLL.SQLProcessing;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using GUI.GlobalStates;
using GUI.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static FPRDB_SQLite.GUI.frmNewSchema;

namespace FPRDB_SQLite.GUI
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private CompositionRoot compRoot;
        private DatabaseService databaseService;
        private FPRDBSchemaService fprdbSchemaSerivce;
        private FPRDBRelationService fprdbRelationService;
        //private SQLFileService sqlFileService;
        private bool isDatabaseLoaded = false;
        private string currentSQLFilePath = string.Empty;
        private bool isSQLFileModified = false;
        private SQLProcessor sqlProcessor;
        private FPRDBRelationDTO _selectedRelation;
        private string _currentEditingColumn;
        private int _currentEditingRow;
        private string _currentEditingColumnType;
        private bool _isOpeningFuzzySet = false;

        private Field selectedField;
        private FieldType selectedFieldType;
        private bool isSelectedFieldText;
        public frmMain(CompositionRoot compRoot)
        {
            this.compRoot = compRoot;
            this.databaseService = this.compRoot.getDatabaseService();
            this.sqlProcessor = this.compRoot.getSQLProcessor();
            this.fprdbSchemaSerivce = compRoot.getFPRDBSchemaService();
            this.fprdbRelationService = compRoot.getFPRDBRelationService();
            //this.sqlFileService = this.compRoot.getSQLFileService();
            InitializeComponent();
            changeStatusTab();
        }
        // Hàm để enable/disable tab và các nút khi load database
        private void changeStatusTab()
        {
            if (!isDatabaseLoaded)
            {
                SchemaRibbonPage.Visible = false;
                pageFuzzySet.Visible = false;
                RelationRibbonPage.Visible = false;    // Tab "Relation"
                QueryRibbonPage.Visible = false;    // Tab "Query"
                xtraTabControlDatabase.Visible = false;
            }
            else
            {
                SchemaRibbonPage.Visible = true;
                pageFuzzySet.Visible = true;
                RelationRibbonPage.Visible = true;    // Tab "Relation"
                QueryRibbonPage.Visible = true;    // Tab "Query"
                xtraTabControlDatabase.Visible = true;
                SetQueryTabState(false);
            }
        }
        private string GetRootPath(string path)
        {
            // Hàm này tự động hiểu và lấy ra "C:\", "D:\"... một cách an toàn
            return System.IO.Path.GetPathRoot(path);
        }
        // Load dữ liệu từ chuỗi của Fuzzy Probalistic Value thành 1 bảng liệt kê giá trị
        private void LoadFuzzyProbalisticValueDetail(string probValue)
        {
            

            DataTable dt = new DataTable();
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("MinProb", typeof(string));
            dt.Columns.Add("MaxProb", typeof(string));

            if (probValue != null)
            {
                //// Liệt kê nội dung cần lấy
                //string content = probValue.Trim('{', '}');
                //// Liệt kê các value (1 value -> 1 tuple)
                //string[] tuples = content.Split(new string[] { "), (" }, StringSplitOptions.None);

                //// Kiểm tra từng value
                //foreach (string tuple in tuples)
                //{
                //    string clean = tuple.Trim('(', ')');
                //    int bracketIndex = clean.IndexOf(",[");
                //    if (bracketIndex < 0) continue;

                //    string value = clean.Substring(0, bracketIndex);

                //    //remove string literal delimiter " from the possible value
                //    //if (this.isSelectedFieldText)
                //    //    value = value.Trim('\"');

                //    string bounds = clean.Substring(bracketIndex + 2).Trim('[', ']');
                //    // Mảng: lowerBound -> index 0, upperBound -> index 1
                //    string[] boundParts = bounds.Split(',');
                //    if (boundParts.Length < 2) continue;


                //    dt.Rows.Add(value, boundParts[0].Trim(), boundParts[1].Trim());
                //}
                List<(string, string, string)> processedFProbValue = FuzzyProbabilisticValueUtilities.extractValuesIntervalProbabilitiesAsString(probValue);
                foreach(var (value, lowerProb, upperProb) in processedFProbValue)
                {
                    dt.Rows.Add(value, lowerProb, upperProb);
                }

            }

            dt.RowChanged += BottomGrid_RowChanged;
            dt.RowDeleted += BottomGrid_RowChanged;

            gridControlValueRelation.DataSource = dt;
            gridView4.BestFitColumns();

            GridColumn col = gridView4.Columns["Value"];
            if (_currentEditingColumnType == "distFS_INT"
                || _currentEditingColumnType == "distFS_FLOAT"
                || _currentEditingColumnType == "distFS_TEXT"
                || _currentEditingColumnType == "contFS")
            {
                col.OptionsColumn.AllowEdit = true;
                var btnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
                btnEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                btnEdit.Buttons.Clear();
                btnEdit.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(
                    DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis));

                gridView4.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
                gridControlValueRelation.RepositoryItems.Add(btnEdit);

                btnEdit.ButtonClick += (sender, e) =>
                {
                    var frm = new frmListFuzzySet(compRoot, selectedFieldType);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFSName = frm.selectedFSName;
                        if (selectedFSName != null || selectedFSName!=default)
                            gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "Value", selectedFSName);
                    }
                };

                col.ColumnEdit = btnEdit;
            }
            else
            {
                col.ColumnEdit = null;
                col.OptionsColumn.AllowEdit = true;
            }
         }
        // Hàm xử lý khi người dùng nhấp ra khỏi hàng ở Grid liệt kê giá trị
        private void BottomGrid_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action != DataRowAction.Add &&
                e.Action != DataRowAction.Change &&
                e.Action != DataRowAction.Delete) return;
            string newFuzzyString = BuildFuzzyStringFromBottomGrid();
            int rowHandle = gridView3.GetRowHandle(_currentEditingRow);

            if (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
            {
                gridView3.SetRowCellValue(rowHandle, gridView3.Columns[_currentEditingColumn], newFuzzyString);

                gridView3.FocusedRowHandle = rowHandle;
                gridView3.ShowEditor();

                if (gridView3.ActiveEditor != null)
                {
                    gridView3.ActiveEditor.IsModified = true;
                }
            }
        }
        // Hàm kiểm tra dữ liệu hàng khi người dùng nhấp ra ngoài
        private void gridView4_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            var view = sender as GridView;
            if (view == null) return;

            foreach (GridColumn col in view.Columns)
            {
                var val = view.GetRowCellValue(e.RowHandle, col);
                if (val == null || val == DBNull.Value ||
                    string.IsNullOrWhiteSpace(val.ToString()))
                {
                    e.Valid = false;
                    e.ErrorText = $"{col.Caption} cannot be empty!";
                    return;
                }
            }
        }
        // Hàm thông báo cho Grid liệt kê giá trị
        private void gridView4_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
            MessageBox.Show(e.ErrorText, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        // Parse giá trị ngược lại cho Grid relation ở trên (khi có cập nhật)
        private string BuildFuzzyStringFromBottomGrid()
        {
            DataTable dt = gridControlValueRelation.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0) return "";

            StringBuilder sb = new StringBuilder("{");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (row.RowState == DataRowState.Deleted) continue;

                string value = row["Value"].ToString();

                //add string literal delimiter
                //if (this.isSelectedFieldText)
                //    value = "\"" + value + "\"";

                string minProb = row["MinProb"].ToString();
                string maxProb = row["MaxProb"].ToString();

                if (string.IsNullOrEmpty(value)) continue;

                sb.Append($"({value},[{minProb},{maxProb}])");
                if (i < dt.Rows.Count - 1)
                    sb.Append(", ");
            }
            sb.Append("}");
            return sb.ToString();
        }
        #region Tab Page Fuzzy Set
        private void buttonAdd_groupDis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new frmAddDiscreteFuzzySet(compRoot).ShowDialog();
        }

        private void buttonAdd_groupCont_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new frmAddContinuousFuzzySet(compRoot).ShowDialog();
        }
        private void iSearchFuzzySet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new frmManageFuzzySet(compRoot).ShowDialog();
        }
        #endregion
        #region Tab Page Home
        // Hàm hiển thị thông tin chi tiết của Schema
        private void DisplaySchemaDetail(FPRDBSchemaDTO schema)
        {
            BindingList<SchemaAttribute> list = new BindingList<SchemaAttribute>();
            List<Field> fields = schema.fields;
            List<string> primaryKeys = schema.primarykey;
            var sortedFields = fields
                .OrderByDescending(attr => primaryKeys.Contains(attr.getFieldName()))
                .ThenBy(attr => attr.getFieldName());
            foreach (var attr in sortedFields)
            {
                var fielInfo = attr.getFieldInfo();
                list.Add(new SchemaAttribute
                {
                    isPrimaryKey = primaryKeys.Contains(attr.getFieldName()),
                    attributeName = attr.getFieldName(),
                    dataType = FieldTypeUtilities.fromFieldTypeEnumToSQLFieldType(fielInfo.getType()),
                    length = fielInfo.getTXTLength()
                });
            }
            gridControlScheme.DataSource = null;
            gridView.Columns.Clear();
            gridControlScheme.DataSource = list;
            gridView.BestFitColumns();
        }
        private DataRow getTupleAsDataRow(List<Field> schemaFields, Scan s, DataTable table)
        {
            DataRow row = table.NewRow();
            Type domainType;
            string fieldName;
            foreach (Field f in schemaFields)
            {
                domainType = FieldTypeUtilities.getDomainType(f.getFieldInfo().getType());
                fieldName = f.getFieldName();
                if (domainType == typeof(int))
                {
                    FuzzyProbabilisticValue<int> fprobValue = s.getFieldContent<int>(fieldName);
                    row[fieldName] = fprobValue.ToString();
                }
                else if (domainType == typeof(float))
                {
                    FuzzyProbabilisticValue<float> fprobValue = s.getFieldContent<float>(fieldName);
                    row[fieldName] = fprobValue.ToString();
                }
                else if (domainType == typeof(string))
                {
                    FuzzyProbabilisticValue<string> fprobValue = s.getFieldContent<string>(fieldName);
                    row[fieldName] = fprobValue.ToString();
                }
                else //if (domainType == typeof(bool))
                {
                    FuzzyProbabilisticValue<bool> fprobValue = s.getFieldContent<bool>(fieldName);
                    row[fieldName] = fprobValue.ToString();
                }
            }
            return row;
        }

        // Hàm hiển thị nội dung của Relation
        private void DisplayRelationDetail(FPRDBRelationDTO relInfo)
        {
            var schema = relInfo.fprdbSchema;
            List<Field> schemaFields = schema.fields;
            // Sử dụng DataTable để hiện thị thông tin Relation
            DataTable relationContent = new DataTable();

            foreach (var field in schemaFields)
            {
                string fieldName = field.getFieldName();
                DataColumn dataCol = new DataColumn(fieldName, typeof(string));
                relationContent.Columns.Add(dataCol);
            }

            //fake data
            //Dictionary<string, string> row1 = new Dictionary<string, string>
            //{
            //    {"id", "{(ID01,[1,1])}"},
            //    {"name", "{(John,[0.1,0.3]), (Mary,[0.4,0.6]), (Nick,[0.7,0.9])}" }
            //};
            //Dictionary<string, string> row2 = new Dictionary<string, string>
            //{
            //    {"id", "{(ID02,[1,1])}"},
            //    {"name", "{(Tom,[0.1,0.3]), (Anna,[0.4,0.6]), (James,[0.7,0.9])}" }
            //};
            //List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            //result.Add(row1);
            //result.Add(row2);
            // Xét từng dòng trong bảng result (giả dụ)
            //foreach (var row in result)
            //{
            //    DataRow fakeRow = relInfo.NewRow();
            //    // Lấy từng ô theo fielName
            //    foreach (var field in fields)
            //    {
            //        string fieldName = field.getFieldName();
            //        string content = "";
            //        // Lấy giá trị tại 1 ô
            //        if (row.TryGetValue(fieldName, out var value))
            //        {
            //            content = value?.ToString();
            //        }
            //        fakeRow[fieldName] = content;
            //    }
            //    relInfo.Rows.Add(fakeRow);
            //}

            //extract the top 100 tuples from the current selected relation for grid view
            //not done: Doesn’t have LIMIT OFFSET for “select top 100 row”
            Plan relPlan = new RelationPlan(relInfo.relName, this.compRoot.getMetaDataManger(), this.compRoot.getDBMgr(), this.compRoot.getParser(), this.compRoot.getConstraintService());
            Scan relScan = relPlan.open();
            int count = 0;
            while (relScan.next() && count < AppStates.maxSelectTop)
            {
                relationContent.Rows.Add(this.getTupleAsDataRow(schemaFields, relScan, relationContent));
                count++;
            }

            relationContent.AcceptChanges();
            gridControlRelation.DataSource = relationContent;

            gridView3.Columns.Clear();
            gridView3.PopulateColumns();

            foreach (var field in schemaFields)
            {
                string fieldName = field.getFieldName();

                GridColumn col = gridView3.Columns[fieldName];
                if (col == null) continue;

                col.Caption = fieldName;
                col.OptionsColumn.AllowEdit = true;
                col.OptionsColumn.ReadOnly = true;
                col.Tag = field.getFieldInfo().getType();
            }

            gridView3.BestFitColumns();

            gridControlRelation.EmbeddedNavigator.ButtonClick -= Navigator_ButtonClick;
            gridControlRelation.EmbeddedNavigator.ButtonClick += Navigator_ButtonClick;

        }
        // Hàm xử lý sự kiện khi click "Select top 100 tuples"
        private void barButtonSelectTuples_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraTabPage relationTab = xtraTabControlDatabase.TabPages[1];
            relationTab.Text = _selectedRelation.relName;
            xtraTabControlDatabase.SelectedTabPageIndex = 1;
            DisplayRelationDetail(_selectedRelation);
            return;
        }
        // Hàm show popup menu cho node relation
        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode node = treeView.GetNodeAt(e.X, e.Y);
            if (node == null) return;
            treeView.SelectedNode = node;

            if (e.Button == MouseButtons.Right)
            {

                if (node.Tag is FPRDBRelationDTO relation)
                {
                    _selectedRelation = relation;
                    popupMenuTreeView.ShowPopup(treeView.PointToScreen(e.Location));
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (node.Tag is FPRDBSchemaDTO schema)
                {
                    XtraTabPage schemaTab = xtraTabControlDatabase.TabPages[0];
                    schemaTab.Text = schema.schemaName;
                    xtraTabControlDatabase.SelectedTabPageIndex = 0;
                    DisplaySchemaDetail(schema);
                }
                if (node.Tag is FPRDBRelationDTO relation)
                {
                    _selectedRelation = relation;
                    DisplayRelationDetail(_selectedRelation);
                }
            }
        }
        // Hàm load cây Database sau khi mở hoặc tạo mới Database
        private void LoadDatabaseTree()
        {
            changeStatusTab();
            // 1. Xóa cây cũ
            treeView.Nodes.Clear();

            // 2. Lấy tên file Database thông qua databaseService
            string dbName = "DATABASE";
            if (!string.IsNullOrEmpty(this.databaseService.getDatabaseName()))
            {
                dbName = this.databaseService.getDatabaseName();
            }

            if (string.IsNullOrEmpty(dbName) || dbName == "DATABASE") return;

            // 3. Tạo Node Root (VD: db1)
            TreeNode rootNode = new TreeNode(dbName);
            rootNode.ImageIndex = 3; // Icon Database
            rootNode.SelectedImageIndex = 3;
            treeView.Nodes.Add(rootNode);

            // ====================================================================
            // TẠO 2 THƯ MỤC CHA: Tables VÀ Relation (Ngang hàng nhau)
            // ====================================================================
            TreeNode tablesRootNode = new TreeNode("FPRDB Schemas");
            tablesRootNode.ImageIndex = 4; // Icon Folder màu vàng
            tablesRootNode.SelectedImageIndex = 4;
            rootNode.Nodes.Add(tablesRootNode);

            TreeNode relationRootNode = new TreeNode("Relation");
            relationRootNode.ImageIndex = 4; // Icon Folder màu vàng
            relationRootNode.SelectedImageIndex = 4;
            rootNode.Nodes.Add(relationRootNode);

            // ====================================================================
            // NHÁNH 1: ĐỔ DỮ LIỆU SCHEMAS VÀ LỌC TRÙNG LẶP
            // ====================================================================
            AppStates.loadFPRDBSchemas = this.databaseService.getFPRDBSchemas();
            var schemas = AppStates.loadFPRDBSchemas;

            if (schemas != null && schemas.Count > 0)
            {
                // Cuốn sổ ghi nhớ các Lược đồ đã vẽ
                HashSet<string> addedSchemas = new HashSet<string>();

                foreach (var schema in schemas)
                {
                    string schemaName = schema.schemaName;

                    // LỌC: Nếu Lược đồ này đã vẽ rồi thì bỏ qua luôn, chuyển sang cái tiếp theo
                    if (addedSchemas.Contains(schemaName)) continue;

                    // Đánh dấu là đã vẽ
                    addedSchemas.Add(schemaName);

                    TreeNode schemaNode = new TreeNode(schemaName);
                    schemaNode.ImageIndex = 8;
                    schemaNode.SelectedImageIndex = 8;
                    schemaNode.Tag = "schema";
                    schemaNode.Tag = schema;
                    tablesRootNode.Nodes.Add(schemaNode);

                    var fields = schema.fields;
                    if (fields != null)
                    {
                        List<string> primaryKeys = schema.primarykey;

                        foreach (var field in fields)
                        {
                            string fieldName = field.getFieldName();
                            bool isPrimaryKey = primaryKeys != null && primaryKeys.Contains(fieldName);

                            TreeNode fieldNode = new TreeNode(fieldName);
                            fieldNode.ImageIndex = isPrimaryKey ? 5 : 2;
                            fieldNode.SelectedImageIndex = isPrimaryKey ? 5 : 2;
                            schemaNode.Nodes.Add(fieldNode);
                        }
                    }
                }
            }
            else
            {
                TreeNode emptySchemaNode = new TreeNode("(Chưa có Lược đồ nào)");
                emptySchemaNode.ImageIndex = 4;
                emptySchemaNode.SelectedImageIndex = 4;
                tablesRootNode.Nodes.Add(emptySchemaNode);
            }

            // ====================================================================
            // NHÁNH 2: ĐỔ DỮ LIỆU RELATIONS VÀ LỌC TRÙNG LẶP
            // ====================================================================
            AppStates.loadFPRDBSchemaRelations = this.databaseService.getFPRDBRelations();
            var relations = AppStates.loadFPRDBSchemaRelations;
            if (relations != null && relations.Count > 0)
            {
                // Cuốn sổ ghi nhớ các Quan hệ đã vẽ
                HashSet<string> addedRelations = new HashSet<string>();

                foreach (var relation in relations)
                {
                    string relName = relation.relName;

                    // LỌC: Nếu Quan hệ này đã vẽ rồi thì bỏ qua luôn
                    if (addedRelations.Contains(relName)) continue;

                    // Đánh dấu là đã vẽ
                    addedRelations.Add(relName);

                    TreeNode instanceNode = new TreeNode(relName);
                    instanceNode.ImageIndex = 7;
                    instanceNode.SelectedImageIndex = 7;
                    instanceNode.Tag = relation;
                    relationRootNode.Nodes.Add(instanceNode);

                    var refSchema = relation.fprdbSchema;
                    if (refSchema != null)
                    {
                        string refSchemaName = refSchema.schemaName;
                        TreeNode refSchemaNode = new TreeNode(refSchemaName);
                        refSchemaNode.ImageIndex = 8;
                        refSchemaNode.SelectedImageIndex = 8;
                        instanceNode.Nodes.Add(refSchemaNode);
                    }
                }
            }
            else
            {
                TreeNode emptyRelNode = new TreeNode("(Chưa có Quan hệ nào)");
                emptyRelNode.ImageIndex = 4;
                emptyRelNode.SelectedImageIndex = 4;
                relationRootNode.Nodes.Add(emptyRelNode);
            }

            treeView.ExpandAll();
        }
        private void reLoadDatabaseTree()
        {
            changeStatusTab();
            // 1. Xóa cây cũ
            treeView.Nodes.Clear();
            // 2. Lấy tên file Database thông qua databaseService
            string dbName = "DATABASE";
            if (!string.IsNullOrEmpty(this.databaseService.getDatabaseName()))
            {
                dbName = this.databaseService.getDatabaseName();
            }

            if (string.IsNullOrEmpty(dbName) || dbName == "DATABASE") return;
            // 3. Tạo Node Root (VD: db1)
            TreeNode rootNode = new TreeNode(dbName);
            rootNode.ImageIndex = 3; // Icon Database
            rootNode.SelectedImageIndex = 3;
            treeView.Nodes.Add(rootNode);

            // ====================================================================
            // TẠO 2 THƯ MỤC CHA: Tables VÀ Relation (Ngang hàng nhau)
            // ====================================================================
            TreeNode tablesRootNode = new TreeNode("FPRDB Schemas");
            tablesRootNode.ImageIndex = 4; // Icon Folder màu vàng
            tablesRootNode.SelectedImageIndex = 4;
            rootNode.Nodes.Add(tablesRootNode);

            TreeNode relationRootNode = new TreeNode("Relation");
            relationRootNode.ImageIndex = 4; // Icon Folder màu vàng
            relationRootNode.SelectedImageIndex = 4;
            rootNode.Nodes.Add(relationRootNode);

            // ====================================================================
            // NHÁNH 1: ĐỔ DỮ LIỆU SCHEMAS VÀ LỌC TRÙNG LẶP
            // ====================================================================
            var schemas = AppStates.loadFPRDBSchemas;

            if (schemas != null && schemas.Count > 0)
            {
                // Cuốn sổ ghi nhớ các Lược đồ đã vẽ
                HashSet<string> addedSchemas = new HashSet<string>();

                foreach (var schema in schemas)
                {
                    string schemaName = schema.schemaName;

                    // LỌC: Nếu Lược đồ này đã vẽ rồi thì bỏ qua luôn, chuyển sang cái tiếp theo
                    if (addedSchemas.Contains(schemaName)) continue;

                    // Đánh dấu là đã vẽ
                    addedSchemas.Add(schemaName);

                    TreeNode schemaNode = new TreeNode(schemaName);
                    schemaNode.ImageIndex = 8;
                    schemaNode.SelectedImageIndex = 8;
                    schemaNode.Tag = schema;
                    tablesRootNode.Nodes.Add(schemaNode);

                    var fields = schema.fields;
                    if (fields != null)
                    {
                        List<string> primaryKeys = schema.primarykey;

                        foreach (var field in fields)
                        {
                            string fieldName = field.getFieldName();
                            bool isPrimaryKey = primaryKeys != null && primaryKeys.Contains(fieldName);

                            TreeNode fieldNode = new TreeNode(fieldName);
                            fieldNode.ImageIndex = isPrimaryKey ? 5 : 2;
                            fieldNode.SelectedImageIndex = isPrimaryKey ? 5 : 2;
                            schemaNode.Nodes.Add(fieldNode);
                        }
                    }
                }
            }
            else
            {
                TreeNode emptySchemaNode = new TreeNode("(Chưa có Lược đồ nào)");
                emptySchemaNode.ImageIndex = 4;
                emptySchemaNode.SelectedImageIndex = 4;
                tablesRootNode.Nodes.Add(emptySchemaNode);
            }

            // ====================================================================
            // NHÁNH 2: ĐỔ DỮ LIỆU RELATIONS VÀ LỌC TRÙNG LẶP
            // ====================================================================
            var relations = AppStates.loadFPRDBSchemaRelations;
            if (relations != null && relations.Count > 0)
            {
                // Cuốn sổ ghi nhớ các Quan hệ đã vẽ
                HashSet<string> addedRelations = new HashSet<string>();

                foreach (var relation in relations)
                {
                    string relName = relation.relName;

                    // LỌC: Nếu Quan hệ này đã vẽ rồi thì bỏ qua luôn
                    if (addedRelations.Contains(relName)) continue;

                    // Đánh dấu là đã vẽ
                    addedRelations.Add(relName);

                    TreeNode instanceNode = new TreeNode(relName);
                    instanceNode.ImageIndex = 7;
                    instanceNode.SelectedImageIndex = 7;
                    instanceNode.Tag = "relation";
                    instanceNode.Tag = relation;
                    relationRootNode.Nodes.Add(instanceNode);

                    var refSchema = relation.fprdbSchema;
                    if (refSchema != null)
                    {
                        string refSchemaName = refSchema.schemaName;
                        TreeNode refSchemaNode = new TreeNode(refSchemaName);
                        refSchemaNode.ImageIndex = 8;
                        refSchemaNode.SelectedImageIndex = 8;
                        instanceNode.Nodes.Add(refSchemaNode);
                    }
                }
            }
            else
            {
                TreeNode emptyRelNode = new TreeNode("(Chưa có Quan hệ nào)");
                emptyRelNode.ImageIndex = 4;
                emptyRelNode.SelectedImageIndex = 4;
                relationRootNode.Nodes.Add(emptyRelNode);
            }

            treeView.ExpandAll();
        }
        // Hàm xử lý khi click "Open" button
        private void buttonOpen_pageHome_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog DialogOpen = new OpenFileDialog();
            DialogOpen.Title = "Open Fuzzy Probabilistic Relational Database (FPRDB)";
            // Chỉ lọc ra các file có đuôi .pdb hoặc .sqlite
            DialogOpen.Filter = "Database file (*.db)|*.db";
            DialogOpen.DefaultExt = "db";
            DialogOpen.InitialDirectory = GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());

            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.databaseService.loadDB(DialogOpen.FileName);
                    XtraMessageBox.Show("Open database successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isDatabaseLoaded = true;
                    LoadDatabaseTree();
                    //load field types
                    AppStates.createSChemaFieldTypes = this.databaseService.getFieldTypes();
                }
                catch (IOException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // Hàm xử lý khi click "New" button
        private void buttonNew_pageHome_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //CompositionRoot.databaseService.SaveDB();
            try
            {
                SaveFileDialog DialogNew = new SaveFileDialog();
                DialogNew.Title = "Create New Fuzzy Probabilistic Relational Database (FPRDB)";
                DialogNew.Filter = "Database file (*.db)|*.db";
                DialogNew.DefaultExt = "db";
                DialogNew.AddExtension = true;
                DialogNew.RestoreDirectory = true;
                DialogNew.InitialDirectory = GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());
                DialogNew.SupportMultiDottedExtensions = true;

                if (DialogNew.ShowDialog() == DialogResult.OK)
                {
                    this.databaseService.createDB(DialogNew.FileName);
                    XtraMessageBox.Show("Create new database successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isDatabaseLoaded = true;
                    LoadDatabaseTree();
                    //load field types
                    AppStates.createSChemaFieldTypes = this.databaseService.getFieldTypes();
                }
            }
            catch (FileNotFoundException ex)
            {
                XtraMessageBox.Show($"Directory doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (IOException ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonExit_pageHome_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DialogResult result = XtraMessageBox.Show("Are you sure want to exit?", "Exit FPRDB Visual Management System", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            catch (Exception Ex)
            {
                XtraMessageBox.Show(Ex.Message);
            }
        }
        #endregion
        #region Tab Page Query
        #region Page Group File
        // Hàm enable Query Edtor khi đã load Query thành công
        private void SetQueryTabState(bool isLoaded, string fileName = "")
        {
            XtraTabPage queryTab = xtraTabControlDatabase.TabPages[2];
            splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;
            if (isLoaded)
            {
                queryTab.PageEnabled = true;
                queryTab.Text = fileName;
                conjunctionRibbonPageGroup.Enabled = true;
                disjunctionRibbonPageGroup.Enabled = true;
                differenceRibbonPageGroup.Enabled = true;
                operatorRibbonPageGroup.Enabled = true;
                excuteQueryribbonPageGroup.Enabled = true;
                iSaveQuery.Enabled = true;
                xtraTabControlDatabase.SelectedTabPageIndex = 2; // focus vào tab Query
            }
            else
            {
                queryTab.PageEnabled = false;
                queryTab.Text = "Query";
                iSaveQuery.Enabled = false;
                conjunctionRibbonPageGroup.Enabled = false;
                disjunctionRibbonPageGroup.Enabled = false;
                differenceRibbonPageGroup.Enabled = false;
                operatorRibbonPageGroup.Enabled = false;
                excuteQueryribbonPageGroup.Enabled = false;
            }
        }
        // Hàm warning file chưa được save
        private void WarningUnsaved()
        {
            if (isSQLFileModified)
            {
                DialogResult result = XtraMessageBox.Show(
                    "You have unsaved changes.",
                    "Warning",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                    SaveCurrentFile();
                else if (result == DialogResult.Cancel)
                    return;
            }
        }
        // Hàm tạo mới SQL File
        private void CreateNewQuery()
        {
            WarningUnsaved();
            try
            {
                SaveFileDialog DialogNew = new SaveFileDialog();
                DialogNew.Title = "Create New SQL File";
                DialogNew.Filter = "SQL File (*.fprdbsql)|*.fprdbsql";
                DialogNew.DefaultExt = "fprdbsql";
                DialogNew.AddExtension = true;
                DialogNew.RestoreDirectory = true;
                DialogNew.InitialDirectory = GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());
                DialogNew.SupportMultiDottedExtensions = true;

                if (DialogNew.ShowDialog() == DialogResult.OK)
                {
                    //sqlFileService.createFile(DialogNew.FileName)
                    currentSQLFilePath = DialogNew.FileName;
                    // Tạo file trống
                    File.WriteAllText(currentSQLFilePath, string.Empty, Encoding.Unicode);
                    memoEditTxtQuery.Text = string.Empty;
                    // Set trạng thái enable cho tab Query sau khi tạo mới thành công
                    SetQueryTabState(true, Path.GetFileName(DialogNew.FileName));
                    XtraMessageBox.Show("Create new SQL file successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FileNotFoundException ex)
            {
                XtraMessageBox.Show($"Directory doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (IOException ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // Hàm mở SQL File đã tồn tại
        private void OpenQuery()
        {
            WarningUnsaved();
            OpenFileDialog DialogOpen = new OpenFileDialog();
            DialogOpen.Title = "Open SQL File";
            DialogOpen.Filter = "SQL File (*.fprdbsql)|*.fprdbsql";
            DialogOpen.DefaultExt = "fprdbsql";
            DialogOpen.InitialDirectory = GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());
            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //string sqlContent = sqlFileService.loadFile(DialogOpen.FileName);
                    currentSQLFilePath = DialogOpen.FileName;
                    // Đọc file và lấy nội dung file gán váo Query Editor
                    memoEditTxtQuery.Text = File.ReadAllText(currentSQLFilePath, Encoding.Unicode);
                    // Set trạng thái enable cho tab Query sau khi mở file thành công
                    SetQueryTabState(true, Path.GetFileName(DialogOpen.FileName));
                    XtraMessageBox.Show("Open SQL file successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (IOException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // Xử lý sự kiện click cho nút New
        private void iNewQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateNewQuery();
        }
        // Xử lý sự kiện click cho nút Open
        private void iOpenQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenQuery();
        }
        // Hàm thêm * nếu file chưa được save
        private void memoEditTxtQuery_TextChanged(object sender, EventArgs e)
        {
            if (!isSQLFileModified)
            {
                isSQLFileModified = true;
                XtraTabPage queryTab = xtraTabControlDatabase.TabPages[2];
                if (!queryTab.Text.StartsWith("*"))
                    queryTab.Text = "*" + queryTab.Text;
            }
        }
        // Hàm lưu file SQL
        private void SaveCurrentFile()
        {
            try
            {
                // Viết nội dung hiện tại trên editor vào file
                File.WriteAllText(currentSQLFilePath, memoEditTxtQuery.Text, Encoding.Unicode);

                // Bỏ dấu * sau khi save
                isSQLFileModified = false;
                XtraTabPage queryTab = xtraTabControlDatabase.TabPages[2];
                queryTab.Text = queryTab.Text.TrimStart('*');
                XtraMessageBox.Show("Save SQL file successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Error saving file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Xử lý sự kiện click cho nút Save
        private void iSaveQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveCurrentFile();
        }
        #endregion
        //hàm chèn ký tự
        private void InsertSymbol(string symbol)
        {
            // 1. Đảm bảo ô nhập liệu được focus trước khi thao tác
            memoEditTxtQuery.Focus();

            // 2. Thay thế đoạn đang chọn (nếu có) bằng symbol, 
            // hoặc chèn symbol vào đúng vị trí con trỏ nếu không chọn gì.
            // Cách này không làm thay đổi toàn bộ thuộc tính .Text nên không gây bôi đen lại từ đầu.
            memoEditTxtQuery.SelectedText = symbol;

            // 3. Đưa con trỏ về sau ký tự vừa chèn (phòng trường hợp DevExpress tự reset)
            memoEditTxtQuery.SelectionStart = memoEditTxtQuery.SelectionStart;
            memoEditTxtQuery.SelectionLength = 0;
        }
        #region Page Group Conjuntion
        private void iConjunctionIgnorance_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨂_ig ");
        private void iConjunctionIndependence_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨂_in ");
        private void iConjunctionMutual_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨂_me ");
        private void iConjunctionPositive_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨂_pc ");
        #endregion
        #region Page Group Disjunction
        private void iDisjunctionIgnorance_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨁_ig ");
        private void iDisjunctionIndependence_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨁_in ");
        private void iDisjunctionMutual_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨁_me ");
        private void iDisjunctionPositive_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨁_pc ");
        #endregion
        #region Page Group Difference
        private void iDifferenceIgnorance_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⦵_ig ");
        private void iDifferenceIndependence_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⦵_in ");
        private void iDiferenceMutual_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⦵_me ");
        private void iDifferencePositive_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⦵_pc ");
        #endregion
        private void ExcuteQueryDemo(string sql)
        {
            //try
            //{
            //    // hiển thị 2 panel của SplitContainerControl để show kết quả truy vấn và cây giải thuật
            //    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;

            //    // Hiển thị cả 2 panel (Query Editor và phần Kết quả)
            //    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;

            //    // Xóa dữ liệu cũ trên GridView
            //    gridControlResultQuery.DataSource = null;
            //    gridViewResultQuery.Columns.Clear();
            //    if (string.IsNullOrEmpty(sql.Trim()))
            //    {
            //        XtraMessageBox.Show("Query does not exist!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    // TODO: Truyền đối tượng database hiện tại vào.
            //    //query = new QueryExecutionBLL(sql, null /* thay bằng đối tượng probDatabase của bạn */);


            //    if (query.ExecuteQuery())
            //    {
            //        if (query.relationResult.FproTuples.Count <= 0)
            //        {
            //            MessagextraTabPage.Text = "No tuple satisfies the condition";
            //            // Chuyển sang Tab Message (Index 1)
            //            xtraTabControlResultQuery.SelectedTabPageIndex = 1;
            //        }
            //        else
            //        {
            //            // Chuẩn bị DataTable để chứa dữ liệu
            //            DataTable dtquery = new DataTable();
            //            dtquery.Columns.Add("NoNumber", typeof(int));

            //            // 1. Khởi tạo cột STT (Number)
            //            DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
            //            col.Caption = "Number";
            //            col.Name = "NoNumber";
            //            col.FieldName = "NoNumber";
            //            col.OptionsColumn.ReadOnly = true;
            //            col.OptionsColumn.AllowEdit = false;
            //            col.Visible = true;
            //            col.VisibleIndex = 0;
            //            gridViewResultQuery.Columns.Add(col);

            //            // 2. Khởi tạo các cột dữ liệu dựa trên thuộc tính được chọn
            //            int i = 1;
            //            foreach (var att in query.selectedAttributes)
            //            {
            //                dtquery.Columns.Add("Col" + i, typeof(String));
            //                DevExpress.XtraGrid.Columns.GridColumn columnDiff = new DevExpress.XtraGrid.Columns.GridColumn();
            //                columnDiff.Caption = att.AttributeName;
            //                columnDiff.Name = "Col" + i;
            //                columnDiff.FieldName = "Col" + i; // FieldName phải map đúng với tên cột trong DataTable
            //                columnDiff.OptionsColumn.ReadOnly = true;
            //                columnDiff.OptionsColumn.AllowEdit = false;

            //                // Đổi màu nền cho các tập mờ
            //                if (att.FproDataType.TypeName == "DiscreteFuzzySet" || att.FproDataType.TypeName == "ContinuousFuzzySet")
            //                {
            //                    columnDiff.AppearanceCell.Options.UseBackColor = true;
            //                    columnDiff.AppearanceCell.BackColor = Color.LightCyan; // Màu nền chính
            //                    // columnDiff.AppearanceCell.BackColor2 = Color.DeepSkyBlue; // (Tùy chọn) Màu Gradient
            //                }

            //                columnDiff.Visible = true;
            //                columnDiff.VisibleIndex = i;
            //                gridViewResultQuery.Columns.Add(columnDiff);
            //                i++;
            //            }

            //            // 3. Đổ dữ liệu từ Tuples vào DataTable
            //            int k = -1;
            //            foreach (var tuple in query.relationResult.FproTuples)
            //            {
            //                DataRow row = dtquery.NewRow();
            //                k++;
            //                int j = 1;
            //                row[0] = k + 1; // STT

            //                foreach (var triple in tuple.FproTriples)
            //                {
            //                    row[j] = triple.GetStrValue();
            //                    j++;
            //                }
            //                dtquery.Rows.Add(row);
            //            }

            //            // 4. Gán DataSource và hiển thị Tab kết quả
            //            gridControlResultQuery.DataSource = dtquery;
            //            // Tự động căn chỉnh độ rộng các cột cho đẹp
            //            gridViewResultQuery.BestFitColumns();

            //            xtraTabControlResultQuery.SelectedTabPageIndex = 0; // Tab Query Result
            //        }
            //    }
            //    else
            //    {
            //        // Truy vấn lỗi (sai cú pháp...)
            //        MessagextraTabPage.Text = query.MessageError;
            //        xtraTabControlResultQuery.SelectedTabPageIndex = 1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show($"Error executing query: {ex.Message}", "Error",
            //        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    // Dù có lỗi hay không thì vẫn phải đảm bảo tập trung con trỏ vào ô nhập liệu để người dùng tiện sửa lỗi hoặc tiếp tục nhập query
            //    memoEditTxtQuery.Focus();

            //    // có hàm ClearAll()(nếu có) để xóa sạch dữ liệu cũ trước khi hiển thị kết quả mới, hãy gọi nó ở đây
            //    // ClearAll();
            //}
            // createQueryPlan

            // 

            // dữ liệu giả để test giao diện
            try
            {
                // Mở rộng giao diện để xem cả query và kết quả
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;

                // 1. Tạo một DataTable giả lập cấu trúc
                DataTable dtMock = new DataTable();
                dtMock.Columns.Add("Number", typeof(int));
                dtMock.Columns.Add("doctor1.ID", typeof(string));
                dtMock.Columns.Add("doctor1.AGE", typeof(string));
                dtMock.Columns.Add("doctor2.NAME", typeof(string));
                dtMock.Columns.Add("doctor2.AGE", typeof(string));

                // 2. Thêm dữ liệu giả lập (giống hệt hình ảnh bạn cung cấp)
                dtMock.Rows.Add(4, "{ DT093 }[ 1, 1 ]", "{ approx_30 }[ 1, 1 ]", "{ L.V. Cuong }[ 1, 1 ]", "{ 30 }[ 0.4, 0.6 ]");
                dtMock.Rows.Add(5, "{ DT093 }[ 1, 1 ]", "{ approx_30 }[ 1, 1 ]", "{ N.V. Hung }[ 1, 1 ]", "{ middle_age }[ 0.8, 1 ]");
                dtMock.Rows.Add(6, "{ DT093 }[ 1, 1 ]", "{ approx_30 }[ 1, 1 ]", "{ N.T. Dat }[ 1, 1 ]", "{ 54 }[ 0.5, 0.5 ]");
                dtMock.Rows.Add(7, "{ DT102 }[ 1, 1 ]", "{ 55 }[ 0.5, 0.5 ]", "{ L.V. Cuong }[ 1, 1 ]", "{ 30 }[ 0.4, 0.6 ]");
                dtMock.Rows.Add(8, "{ DT102 }[ 1, 1 ]", "{ 55 }[ 0.5, 0.5 ]", "{ N.V. Hung }[ 1, 1 ]", "{ middle_age }[ 0.8, 1 ]");
                dtMock.Rows.Add(9, "{ DT102 }[ 1, 1 ]", "{ 55 }[ 0.5, 0.5 ]", "{ N.T. Dat }[ 1, 1 ]", "{ 54 }[ 0.5, 0.5 ]");
                //next()
                // 3. Gán dữ liệu giả vào GridControl
                gridControlResultQuery.DataSource = dtMock;

                // 4. Yêu cầu GridView tự động tạo các cột dựa trên DataTable
                gridViewResultQuery.PopulateColumns();

                dtMock.Rows.Add(10, "{ DT102 }[ 1, 1 ]", "{ 55 }[ 0.5, 0.5 ]", "{ N.T. Dat }[ 1, 1 ]", "{ 54 }[ 0.5, 0.5 ]");

                // 5. Tùy chỉnh hiển thị cột (Frontend)
                if (gridViewResultQuery.Columns.Count > 0)
                {
                    // Chỉnh độ rộng và không cho phép sửa trực tiếp trên lưới
                    gridViewResultQuery.OptionsBehavior.Editable = false;
                    gridViewResultQuery.BestFitColumns();
                }

                // Chuyển sang Tab hiển thị kết quả (Giả sử index 0 là Query Result)
                xtraTabControlResultQuery.SelectedTabPageIndex = 0;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi hiển thị dữ liệu giả: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void iExcuteQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string sql = memoEditTxtQuery.Text;
            string firstString = sql.Split(' ')[0];
            switch (firstString.ToUpper())
            {
                case "INSERT":
                case "DELETE":
                case "UPDATE":
                case "DROP":
                    ExecuteUpdate(sql);
                    break;
                case "CREATE":
                    ExecuteDataDefinition(sql);
                    break;
                default:
                    ExecuteQuery(sql);
                    break;
            }

        }
        private void ExecuteDataDefinition(string sql)
        {
            try
            {
                this.sqlProcessor.executeDataDefinition(sql);
                memoEditMessage.Text = $"Success";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (SQLSyntaxException ex)
            {
                memoEditMessage.Text = $"[SQL Syntax Error]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (SemanticException ex)
            {
                memoEditMessage.Text = $"[SQL Semantic Error]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (InvalidCastException ex)
            {
                memoEditMessage.Text = $"[Invalid Cast Exception Error]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (MismatchTokenType ex)
            {
                memoEditMessage.Text = $"[Token Mismatch]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (NotSupportedException ex)
            {
                memoEditMessage.Text = $"[Not Supported]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            finally
            {
                // Đảm bảo splitContainer luôn hiện để xem được kết quả/lỗi
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
            }

        }
        private void ExecuteUpdate(string sql)
        {
            try
            {
                int noRowsAffected = this.sqlProcessor.executeUpdate(sql);
                memoEditMessage.Text = $"[Number of rows affected]\r\n{noRowsAffected}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (SQLSyntaxException ex)
            {
                memoEditMessage.Text = $"[SQL Syntax Error]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (SemanticException ex)
            {
                memoEditMessage.Text = $"[SQL Semantic Error]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (InvalidCastException ex)
            {
                memoEditMessage.Text = $"[Invalid Cast Exception Error]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (MismatchTokenType ex)
            {
                memoEditMessage.Text = $"[Token Mismatch]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (NotSupportedException ex)
            {
                memoEditMessage.Text = $"[Not Supported]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            finally
            {
                // Đảm bảo splitContainer luôn hiện để xem được kết quả/lỗi
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
            }

        }
        private void ExecuteQuery(string sql)
        {
            try
            {
                DataTable resultForGridView = new DataTable();
                Plan p = this.sqlProcessor.createQueryPlan(sql);
                FPRDBSchema schema = p.getSchema();
                Scan s = p.open();

                //create columns for grid view of query result
                foreach (Field f in schema.getFields())
                {
                    resultForGridView.Columns.Add(f.getFieldName(), typeof(string));
                }
                //Extract the result for grid view
                string[] tupleForGridView = new string[schema.getFields().Count];
                Field field;
                List<Field> fields = schema.getFields();
                while (s.next())
                {
                    for (int i = 0; i < schema.getFields().Count; ++i)
                    {
                        field = fields[i];
                        switch (field.getFieldInfo().getType())
                        {
                            case FieldType.INT:
                            case FieldType.distFS_INT:
                                tupleForGridView[i] = s.getFieldContent<int>(field.getFieldName()).ToString();
                                break;
                            case FieldType.FLOAT:
                            case FieldType.distFS_FLOAT:
                            case FieldType.contFS:
                                //tupleForGridView.Add((s.getFieldContent<float>(field.getFieldName())).ToString());
                                tupleForGridView[i] = s.getFieldContent<float>(field.getFieldName()).ToString();
                                break;
                            case FieldType.CHAR:
                            case FieldType.VARCHAR:
                            case FieldType.distFS_TEXT:
                                //tupleForGridView.Add((s.getFieldContent<string>(field.getFieldName())).ToString());
                                tupleForGridView[i] = s.getFieldContent<string>(field.getFieldName()).ToString();
                                break;
                            case FieldType.BOOLEAN:
                                //tupleForGridView.Add((s.getFieldContent<bool>(field.getFieldName())).ToString());
                                tupleForGridView[i] = s.getFieldContent<bool>(field.getFieldName()).ToString();
                                break;
                        }
                    }
                    resultForGridView.Rows.Add(tupleForGridView);


                }
                //bind the result to the grid control
                gridControlResultQuery.DataSource = resultForGridView;
                // Mở rộng giao diện để xem cả query và kết quả
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
                //Yêu cầu GridView tự động tạo các cột dựa trên DataTable
                gridViewResultQuery.PopulateColumns();

                // Sau khi thành công, mở rộng Panel và đảm bảo tab Result được hiển thị
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
                xtraTabControlResultQuery.SelectedTabPage = QueryResultxtraTabPage;

                // Ghi thông báo thành công vào MemoEdit
                memoEditMessage.Text = $"Query executed successfully";
            }
            catch (SQLSyntaxException ex)
            {
                memoEditMessage.Text = $"[SQL Syntax Error]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (SemanticException ex)
            {
                memoEditMessage.Text = $"[SQL Semantic Error]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (InvalidCastException ex)
            {
                memoEditMessage.Text = $"[Invalid Cast Exception Error]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (MismatchTokenType ex)
            {
                memoEditMessage.Text = $"[Token Mismatch]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            catch (NotSupportedException ex)
            {
                memoEditMessage.Text = $"[Not Supported]\r\n{ex.Message}";
                xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            }
            finally
            {
                // Đảm bảo splitContainer luôn hiện để xem được kết quả/lỗi
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
            }
        }
        private void iOperator_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string symbol = " ⇒ ";
            // Kiểm tra nếu ô query đang trống thì gán thẳng
            if (string.IsNullOrEmpty(memoEditTxtQuery.Text))
            {
                memoEditTxtQuery.Text = symbol;
                memoEditTxtQuery.SelectionStart = symbol.Length;
            }
            else
            {
                // Lấy vị trí con trỏ hiện tại
                int index = memoEditTxtQuery.SelectionStart;

                // Chèn ký hiệu vào đúng vị trí con trỏ
                memoEditTxtQuery.Text = memoEditTxtQuery.Text.Insert(index, symbol);

                // Đặt lại vị trí con trỏ sau khi chèn
                memoEditTxtQuery.SelectionStart = index + symbol.Length;
            }

            // Tập trung con trỏ lại vào ô nhập liệu sau khi nhấn nút
            memoEditTxtQuery.Focus();
        }
        #endregion
        #region Tab Page Schema
        private void iNewSchema_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmNewSchema childform = new frmNewSchema(compRoot))
            {
                if (childform.ShowDialog() == DialogResult.OK)
                {
                    reLoadDatabaseTree();
                }
            }
        }
        #endregion
        // Hàm xử lý sự kiện click cho nút Xóa lược đồ
        private void iDeleteSchema_ItemClick(object sender, ItemClickEventArgs e)
        {
            TreeNode selectedNode = treeView.SelectedNode;

            // Kiểm tra tính hợp lệ:
            // 1. Node không null
            // 2. Node cha của nó phải có tên là "FPRDB Schemas"
            if (selectedNode == null || selectedNode.Parent == null || selectedNode.Parent.Text != "FPRDB Schemas")
            {
                XtraMessageBox.Show("Please select a schema from the 'FPRDB Schemas' folder to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string schemaName = selectedNode.Text;

            try
            {
                // Truy vấn ngược lại DTO từ AppStates dựa vào Tên (Text) của Node
                FPRDBSchemaDTO schemaToDelete = AppStates.loadFPRDBSchemas.FirstOrDefault(s => s.schemaName == schemaName);

                if (schemaToDelete == null)
                {
                    XtraMessageBox.Show("Schema data not found in system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //// --- Logic kiểm tra ràng buộc ---
                //var allRelations = this.databaseService.getFPRDBRelations();
                //var dependentRelations = allRelations.Where(r => r.fprdbSchema != null && r.fprdbSchema.schemaName == schemaName)
                //                                     .Select(r => r.relName).ToList();

                //if (dependentRelations.Count > 0)
                //{
                //    string rellList = string.Join(", ", dependentRelations);
                //    XtraMessageBox.Show($"Cannot delete schema '{schemaName}' because it is referenced by: {rellList}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                // --- Thực hiện xóa ---
                if (XtraMessageBox.Show($"Are you sure you want to delete schema '{schemaName}'?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        this.fprdbSchemaSerivce.removeFPRDBSchema(schemaToDelete);
                        XtraMessageBox.Show("Schema deleted successfully!");
                        AppStates.loadFPRDBSchemas = this.databaseService.getFPRDBSchemas();
                        reLoadDatabaseTree();
                    }
                    catch(SemanticException ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Semantic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex) { /* Handle ex */ }
        }
        #region Tab Page Relation
        private void iNewRelation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmNewRelation childForm = new frmNewRelation(compRoot))
            {
                if (childForm.ShowDialog() == DialogResult.OK)
                    reLoadDatabaseTree();
            }

        }

        private void iDeleteRelation_ItemClick(object sender, ItemClickEventArgs e)
        {
            TreeNode selectedNode = treeView.SelectedNode;

            // Kiểm tra: Node cha phải là "Relation"
            if (selectedNode == null || selectedNode.Parent == null || selectedNode.Parent.Text != "Relation")
            {
                XtraMessageBox.Show("Please select a relation from the 'Relation' folder to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string relName = selectedNode.Text;

            try
            {
                // Tìm DTO tương ứng trong danh sách Relations
                FPRDBRelationDTO relToDelete = AppStates.loadFPRDBSchemaRelations.FirstOrDefault(r => r.relName == relName);

                if (relToDelete == null) return;

                if (XtraMessageBox.Show($"Are you sure you want to delete relation '{relName}'?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Giả sử bạn dùng executeUpdate hoặc hàm remove trong service
                    // bool isDeleted = this.databaseService.removeFPRDBRelation(relToDelete); 

                    try
                    {
                        this.fprdbRelationService.removeFPRDBRelation(relToDelete);
                        // Cập nhật lại UI
                        XtraMessageBox.Show("Relation deleted successfully!");
                        AppStates.loadFPRDBSchemaRelations = this.databaseService.getFPRDBRelations();
                        reLoadDatabaseTree();
                    }
                    catch (SemanticException ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Semantic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex) { /* Handle ex */ }
        }
        #endregion
        // 2 hàm parse dữ liệu xuống dưới Grid liệt kê giá trị
        private void gridView3_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridView3.FocusedColumn == null) return;
            var cellValue = gridView3.GetFocusedRowCellValue(gridView3.FocusedColumn);
            string fuzzyProbalisticValue = cellValue?.ToString() ?? string.Empty;
            _currentEditingColumn = gridView3.FocusedColumn.FieldName;
            _currentEditingRow = gridView3.FocusedRowHandle;
            _currentEditingColumnType = gridView3.FocusedColumn.Tag?.ToString() ?? string.Empty;
            LoadFuzzyProbalisticValueDetail(fuzzyProbalisticValue);
        }
        private void gridView3_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (gridView3.FocusedColumn == null) return;
            var cellValue = gridView3.GetFocusedRowCellValue(gridView3.FocusedColumn);
            string fuzzyProbalisticValue = cellValue?.ToString() ?? string.Empty;
            _currentEditingColumn = gridView3.FocusedColumn.FieldName;
            _currentEditingRow = gridView3.FocusedRowHandle;
            _currentEditingColumnType = gridView3.FocusedColumn.Tag?.ToString() ?? string.Empty;

            string fldName = this._currentEditingColumn;
            this.selectedField = this._selectedRelation.fprdbSchema.fields.FirstOrDefault(n => n.getFieldName() == fldName);
            this.selectedFieldType = selectedField.getFieldInfo().getType();
            this.isSelectedFieldText = selectedFieldType == FieldType.CHAR || selectedFieldType == FieldType.VARCHAR || selectedFieldType == FieldType.distFS_TEXT;

            LoadFuzzyProbalisticValueDetail(fuzzyProbalisticValue);
        }
        // Hàm ngăn không cho xóa hết dòng (đối với cột khóa chính)
        private void gridView4_RowDeleting(object sender, DevExpress.Data.RowDeletingEventArgs e)
        {
            DataTable dt = gridControlValueRelation.DataSource as DataTable;
            if (dt == null) return;

            var schema = _selectedRelation.fprdbSchema;
            List<string> pks = schema.primarykey;

            if (!pks.Contains(_currentEditingColumn)) return;

            int validRowCount = dt.Rows.Cast<DataRow>()
                .Count(r => r.RowState != DataRowState.Deleted) - 1;

            if (validRowCount <= 0)
            {
                e.Cancel = true;
                MessageBox.Show("Primary key cannot be empty! At least 1 row required.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Navigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType != NavigatorButtonType.Remove
                && e.Button.ButtonType != NavigatorButtonType.EndEdit) return;

            if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                DataRowView currentRow = gridView3.GetFocusedRow() as DataRowView;
                DataRow row = currentRow.Row;
                var schema = _selectedRelation.fprdbSchema;
                List<string> pks = schema.primarykey;

                string sbRow = $"DELETE FROM {this._selectedRelation.relName} WHERE";
                List<(string, string, string)> procssedFProbValue;

                foreach (var pk in pks)
                {
                    string val = gridView3.GetFocusedRowCellValue(pk) as string;
                    procssedFProbValue = FuzzyProbabilisticValueUtilities.extractValuesIntervalProbabilitiesAsString(val);
                    //[not done] not support null value
                    //if (val == null || val == DBNull.Value)
                    //{
                    //    sbRow.AppendLine($"{pk}: (empty)");
                    //    continue;
                    //}

                    sbRow+=$" ({pk}={procssedFProbValue[0].Item1})[1,1] AND";
                }
                int trailingAND = sbRow.LastIndexOf("AND");
                sbRow = sbRow.Substring(0, trailingAND);
                try
                {
                    this.sqlProcessor.executeUpdate(sbRow);
                    row.AcceptChanges();
                }
                catch(SemanticException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Semantic error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                
            }
            else
            {
                gridView3.CloseEditor();
                gridView3.UpdateCurrentRow();

                DataRowView currentRow = gridView3.GetFocusedRow() as DataRowView;

                if (currentRow != null)
                {
                    DataRow row = currentRow.Row;
                    var schema = _selectedRelation.fprdbSchema;
                    List<Field> fields = schema.fields;
                    string sbRow;
                    if (row.RowState == DataRowState.Added)
                    {
                        sbRow = $"INSERT INTO {this._selectedRelation.relName} ( ";
                        foreach (Field f in fields)
                        {
                            sbRow += $" {f.getFieldName()},";
                        }
                        sbRow = sbRow.TrimEnd(',') + ")";
                        sbRow += " VALUES ( ";
                        foreach (Field field in fields)
                        {
                            sbRow += $"{currentRow[field.getFieldName()]},";
                        }
                        sbRow = sbRow.TrimEnd(',') + " )";
                        try
                        {
                            this.sqlProcessor.executeUpdate(sbRow);
                            row.AcceptChanges();
                        }
                        catch (MismatchTokenType ex)
                        {
                            XtraMessageBox.Show(ex.Message, "FPRDB SQL syntax error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (SQLSyntaxException ex)
                        {
                            XtraMessageBox.Show($"Near token {ex.nearToken} at column line {ex.line}, {ex.column}: {ex.Message}", "FPRDB SQL syntax error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (SemanticException ex)
                        {
                            XtraMessageBox.Show(ex.Message, "Semantic error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (InvalidOperationException ex)
                        {
                            XtraMessageBox.Show(ex.Message, "Invalid operation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (row.RowState == DataRowState.Modified)
                    {
                        List<string> pks = schema.primarykey;
                        List<string> setClauses = new List<string>();
                        foreach (var field in fields)
                        {
                            string fName = field.getFieldName();
                            object newVal = row[fName, DataRowVersion.Current];

                            //[not done] not supported null yet
                            //string formattedVal = (newVal == DBNull.Value) ? "NULL" : $"'{newVal.ToString().Replace("'", "''")}'";
                            string formattedVal = newVal as string;
                            setClauses.Add($"{fName} = {formattedVal}");
                        }

                        // 2. Xây dựng phần WHERE (Dữ liệu ĐỊNH DANH CŨ)
                        string whereClause="WHERE";
                        foreach (string pkName in pks)
                        {
                            string oldPkVal = row[pkName, DataRowVersion.Original] as string;

                            //[not done] not supported null yet
                            //string formattedOldVal = (oldPkVal == DBNull.Value) ? "NULL" : $"'{oldPkVal.ToString().Replace("'", "''")}'";
                            oldPkVal = this.extractValueFromTrueExactFuzzyProbabilisitcValue(oldPkVal);
                            whereClause+=$" ({pkName} = {oldPkVal})[1,1] AND";
                        }
                        int trailingAND = whereClause.LastIndexOf("AND");
                        whereClause = whereClause.Substring(0, trailingAND);

                        string fprdbUpdateSQL;
                        try
                        {
                            for (int i = 0; i < setClauses.Count; ++i)
                            {
                                fprdbUpdateSQL = $"UPDATE {this._selectedRelation.relName} SET {setClauses[i]} "+whereClause;
                                this.sqlProcessor.executeUpdate(fprdbUpdateSQL);
                            }
                            row.AcceptChanges();
                        }
                        catch (MismatchTokenType ex)
                        {
                            XtraMessageBox.Show(ex.Message, "FPRDB SQL syntax error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (SQLSyntaxException ex)
                        {
                            XtraMessageBox.Show($"Near token {ex.nearToken} at column line {ex.line}, {ex.column}: {ex.Message}", "FPRDB SQL syntax error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (SemanticException ex)
                        {
                            XtraMessageBox.Show(ex.Message, "Semantic error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (InvalidOperationException ex)
                        {
                            XtraMessageBox.Show(ex.Message, "Invalid operation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
            }
        }
        //{("DT201",[1,1])}->DT201
        private string extractValueFromTrueExactFuzzyProbabilisitcValue(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            int startOfPossibleValue = input.IndexOf('(');//( in (1,[1,1])
            if (startOfPossibleValue == -1)
                throw new InvalidOperationException("invalid fuzzy probabilistic value");
            int commaDelimitPossibleValueAndIntervalProbbability = input.IndexOf(',');//first , in (1,[1,1])
            if (commaDelimitPossibleValueAndIntervalProbbability == -1)
                throw new InvalidOperationException("invalid fuzzy probabilistic value");
            string possibleValue = input.Substring(startOfPossibleValue + 1, commaDelimitPossibleValueAndIntervalProbbability - 1 - startOfPossibleValue);
            return possibleValue;
        }
        private void gridControlRelation_Click(object sender, EventArgs e)
        {

        }

        private void iExportFS_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SaveFileDialog DialogNew = new SaveFileDialog();
                DialogNew.Title = "Export Fuzzy Set File";
                DialogNew.Filter = "Fuzzy Set Files (*.fset)|*.fset";
                DialogNew.DefaultExt = "fset";
                DialogNew.AddExtension = true;
                DialogNew.RestoreDirectory = true;
                DialogNew.InitialDirectory = GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());
                DialogNew.SupportMultiDottedExtensions = true;

                if (DialogNew.ShowDialog() == DialogResult.OK)
                {
                    //this.databaseService.createDB(DialogNew.FileName);
                    XtraMessageBox.Show($"Export fuzzy set successfully!\n{DialogNew.FileName}", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FileNotFoundException ex)
            {
                XtraMessageBox.Show($"Directory doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (IOException ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void iImportFS_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenFileDialog DialogOpen = new OpenFileDialog();
            DialogOpen.Title = "Import Fuzzy Set File";
            // Chỉ lọc ra các file có đuôi .pdb hoặc .sqlite
            DialogOpen.Filter = "Fuzzy Set Files (*.fset)|*.fset";
            DialogOpen.DefaultExt = "fset";
            DialogOpen.InitialDirectory = GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());

            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //this.databaseService.loadDB(DialogOpen.FileName);
                    XtraMessageBox.Show($"Import fuzzy set successfully!\n{DialogOpen.FileName}", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (IOException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void barButtonRelationships_ItemClick(object sender, ItemClickEventArgs e)
        {
            new frmFKRelationships(compRoot, _selectedRelation).ShowDialog();
        }
    }
}
