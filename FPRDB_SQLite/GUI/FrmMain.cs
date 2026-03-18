using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.Services;
using BLL.SQLProcessing;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLL.Services;
using BLL.DTO;
using DevExpress.Xpo.DB.Helpers;
using BLL.Common;
using GUI.GlobalStates;
using static FPRDB_SQLite.GUI.frmNewSchema;
using System.Net.Sockets;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;

namespace FPRDB_SQLite.GUI
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private CompositionRoot compRoot;
        private DatabaseService databaseService;
        //private SQLFileService sqlFileService;
        private bool isDatabaseLoaded = false;
        private string currentSQLFilePath = string.Empty;
        private bool isSQLFileModified = false;
        private FPRDBRelationDTO _selectedRelation;
        private string _currentEditingColumn;
        private int _currentEditingRow;
        public frmMain(CompositionRoot compRoot)
        {
            this.compRoot = compRoot;
            this.databaseService = this.compRoot.getDatabaseService();
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
                // Liệt kê nội dung cần lấy
                string content = probValue.Trim('{', '}');
                // Liệt kê các value (1 value -> 1 tuple)
                string[] tuples = content.Split(new string[] { "), (" }, StringSplitOptions.None);

                // Kiểm tra từng value
                foreach (string tuple in tuples)
                {
                    string clean = tuple.Trim('(', ')');
                    int bracketIndex = clean.IndexOf(",[");
                    if (bracketIndex < 0) continue;

                    string value = clean.Substring(0, bracketIndex);
                    string bounds = clean.Substring(bracketIndex + 2).Trim('[', ']');
                    // Mảng: lowerBound -> index 0, upperBound -> index 1
                    string[] boundParts = bounds.Split(',');
                    if (boundParts.Length < 2) continue;


                    dt.Rows.Add(value, boundParts[0].Trim(), boundParts[1].Trim());
                }
            }

            dt.RowChanged += BottomGrid_RowChanged;
            dt.RowDeleted += BottomGrid_RowChanged;

            gridControlValueRelation.DataSource = dt;
            gridView4.BestFitColumns();
        }
        private void BottomGrid_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action != DataRowAction.Add &&
                e.Action != DataRowAction.Change &&
                e.Action != DataRowAction.Delete) return;

            string newFuzzyString = BuildFuzzyStringFromBottomGrid();
            DataTable topDt = gridControlRelation.DataSource as DataTable;
            if (topDt != null && _currentEditingRow >= 0)
            {
                topDt.Rows[_currentEditingRow][_currentEditingColumn] = newFuzzyString;
            }
        }
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
        // Hàm hiển thị thông tin của Relation
        private void DisplayRelationDetail(FPRDBRelationDTO relation)
        {
            var schema = relation.fprdbSchema;
            List<Field> fields = schema.fields;
            // Sử dụng DataTable để hiện thị thông tin Relation
            DataTable relInfo = new DataTable();

            foreach (var field in fields)
            {
                string fieldName = field.getFieldName();
                DataColumn dataCol = new DataColumn(fieldName, typeof(string));
                relInfo.Columns.Add(dataCol);
            }

            gridView3.Columns.Clear();
            gridControlRelation.DataSource = relInfo;

            // Thêm cột bằng các thông tin Field có sẵn lấy từ Schema
            foreach (var field in fields)
            {
                string fieldName = field.getFieldName();

                GridColumn col = gridView3.Columns[fieldName];
                if (col == null) continue;

                col.Caption = fieldName;
                col.OptionsColumn.AllowEdit = false;
            }

            Dictionary<string, string> row1 = new Dictionary<string, string>
            {
                {"id", "{(ID01,[1,1])}"},
                {"name", "{(John,[0.1,0.3]), (Mary,[0.4,0.6]), (Nick,[0.7,0.9])}" }
            };
            Dictionary<string, string> row2 = new Dictionary<string, string>
            {
                {"id", "{(ID02,[1,1])}"},
                {"name", "{(Tom,[0.1,0.3]), (Anna,[0.4,0.6]), (James,[0.7,0.9])}" }
            };

            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            result.Add(row1);
            result.Add(row2);

            // Xét từng dòng trong bảng result (giả dụ)
            foreach (var row in result)
            {
                DataRow fakeRow = relInfo.NewRow();
                // Lấy từng ô theo fielName
                foreach (var field in fields)
                {
                    string fieldName = field.getFieldName();
                    string content = "";
                    // Lấy giá trị tại 1 ô
                    if (row.TryGetValue(fieldName, out var value))
                    {
                        content = value?.ToString();
                    }
                    fakeRow[fieldName] = content;
                }
                relInfo.Rows.Add(fakeRow);
            }

            gridView3.BestFitColumns();
            // Listen for data changes
            relInfo.RowChanged += RelInfo_RowChanged;
            relInfo.RowDeleting += RelInfo_RowDeleting;

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
                    DisplayRelationDetail(relation);
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
        #region Page Group Conjuntion
        private void iConjunctionIgnorance_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string symbol = " ⨂_ig ";
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

        private void iConjunctionIndependence_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string symbol = " ⨂_in ";
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

        private void iConjunctionMutual_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string symbol = " ⨂_me ";
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

        private void iConjunctionPositive_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string symbol = " ⨂_pc ";
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
        #region Page Group Disjunction
        private void iDisjunctionIgnorance_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string symbol = " ⨁_ig ";
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

        private void iDisjunctionIndependence_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string symbol = " ⨁_in ";
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

        private void iDisjunctionMutual_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string symbol = " ⨁_me ";
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

        private void iDisjunctionPositive_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string symbol = " ⨁_pc ";
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
        #region Page Group Difference
        private void iDifferenceIgnorance_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string symbol = " ⦵_ig ";
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

        private void iDifferenceIndependence_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string symbol = " ⦵_in ";
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

        private void iDiferenceMutual_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string symbol = " ⦵_me ";
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

        private void iDifferencePositive_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string symbol = " ⦵_pc ";
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
        private void ExcuteQuery(string sql)
        {
            splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
            // createQueryPlan
        }
        private void iExcuteQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string sql = memoEditTxtQuery.Text;
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
        #region Tab Page Relation
        private void iNewRelation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmNewRelation childForm = new frmNewRelation(compRoot))
            {
                if (childForm.ShowDialog() == DialogResult.OK)
                    reLoadDatabaseTree();
            }

        }

        private void RelInfo_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action != DataRowAction.Add && e.Action != DataRowAction.Change) return;

            DataRow row = e.Row;

            var schema = _selectedRelation.fprdbSchema;
            List<Field> fields = schema.fields;

            try
            {
                StringBuilder sbRow = new StringBuilder();
                sbRow.AppendLine("--- Row ---");

                foreach (var field in fields)
                {
                    string fieldName = field.getFieldName();
                    var cellValue = row[fieldName];

                    if (cellValue == DBNull.Value || cellValue == null)
                    {
                        sbRow.AppendLine($"{fieldName}: (empty)");
                        continue;
                    }
                    sbRow.AppendLine($"{fieldName}: {cellValue}");
                }

                if (e.Action == DataRowAction.Add)
                    MessageBox.Show($"Add\n\n{sbRow}", "Saved",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show($"Update\n\n{sbRow}", "Saved",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Auto save failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RelInfo_RowDeleting(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action != DataRowAction.Delete) return;

            DataRow row = e.Row;

            try
            {
                var schema = _selectedRelation.fprdbSchema;
                List<string> pks = schema.primarykey;
                List<string> pkValues = new List<string>();
                StringBuilder sbRow = new StringBuilder();
                sbRow.AppendLine("--- Row ---");

                foreach (var pk in pks)
                {
                    var cellValue = row[pk];

                    if (cellValue == DBNull.Value || cellValue == null)
                    {
                        sbRow.AppendLine($"{pk}: (empty)");
                        continue;
                    }
                    sbRow.AppendLine($"{pk}: {cellValue}");
                }

                var result = MessageBox.Show(
                    $"Delete \n\n{sbRow}",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    MessageBox.Show($"Deleted \n\n{sbRow}", "Deleted",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    e.Row.RejectChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        private void gridView3_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (e.FocusedColumn == null) return;
            var cellValue = gridView3.GetFocusedRowCellValue(e.FocusedColumn);
            string fuzzyProbalisticValue = cellValue?.ToString() ?? string.Empty;
            _currentEditingColumn = gridView3.FocusedColumn.FieldName;
            _currentEditingRow = gridView3.FocusedRowHandle;
            LoadFuzzyProbalisticValueDetail(fuzzyProbalisticValue);
        }

        private void gridView3_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridView3.FocusedColumn == null) return;
            var cellValue = gridView3.GetFocusedRowCellValue(gridView3.FocusedColumn);
            string fuzzyProbalisticValue = cellValue?.ToString() ?? string.Empty;
            _currentEditingColumn = gridView3.FocusedColumn.FieldName;
            _currentEditingRow = gridView3.FocusedRowHandle;
            LoadFuzzyProbalisticValueDetail(fuzzyProbalisticValue);
        }

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
    }
}
