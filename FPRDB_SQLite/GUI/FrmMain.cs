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
using BLL.DomainObject;
using DevExpress.Xpo.DB.Helpers;
using BLL.Common;
using GUI.GlobalStates;
using static FPRDB_SQLite.GUI.frmNewSchema;

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
        //
        private void DisplaySchemaDetail(FPRDBSchema schema)
        {
            BindingList<SchemaAttribute> list = new BindingList<SchemaAttribute>();
            List<Field> fields = schema.getFields();
            List<string> primaryKeys = schema.getPrimarykey();
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
                    dataType = fielInfo.getType(),
                    length = fielInfo.getTXTLength()
                });
            }
            gridControlScheme.DataSource = null;
            gridView.Columns.Clear();
            gridControlScheme.DataSource = list;
            gridView.BestFitColumns();
        }
        private Type MapFieldType(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.INT: return typeof(int);
                default: return null;
            }
        }
        private void DisplayRelationDetail(FPRDBRelation relation)
        {
            //var schema = relation.getSchema();
            //List<Field> fields = schema.getFields();
            //// Sử dụng DataTable để hiện thị thông tin Relation
            //DataTable relInfo = new DataTable();
            //// Thêm cột bằng các thông tin Field có sẵn lấy từ Schema
            //foreach (var field in fields)
            //{
            //    string fieldName = field.getFieldName();
            //    FieldType fieldType = field.getFieldInfo().getType();
            //    Type colType = MapFieldType(fieldType);
            //    relInfo.Columns.Add(fieldName, colType);
            //}

            ////string sql = "SELECT ...";
            ////Plan plan = createQueryPlan(sql);
            ////Scan sc = plan.open();
            //scan.beforeFirst();
            //while (scan.next())
            //{
            //    DataRow row = relInfo.NewRow();
            //    foreach (var field in fields)
            //    {
            //        FieldType fieldType = field.getFieldInfo().getType();
            //        Type colType = MapFieldType(fieldType);
            //        var cellValueNotParsed = scan.getFieldConten<colType>(field.getFieldName());
            //        // Parse FuzzySetProbabilisticValue ra 1 chuỗi tring
            //        var cellValueParsed = "";
            //        row[field.getFieldName()] = cellValueParsed;
            //    }
            //    relInfo.Rows.Add(row);
            //}
            //scan.close();

            //gridControlRelation.DataSource = null;
            //gridView3.Columns.Clear();
            //gridControlRelation.DataSource = relInfo;
            //gridView3.BestFitColumns();
        }
        // Hàm xử lý sự kiện khi click "Select top 100 tuples"
        private void barButtonSelectTuples_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraMessageBox.Show("Select top 100 tuples successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                if (node.Tag is FPRDBRelation relation)
                {
                    popupMenuTreeView.ShowPopup(treeView.PointToScreen(e.Location));
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (node.Tag is FPRDBSchema schema)
                {
                    XtraTabPage schemaTab = xtraTabControlDatabase.TabPages[0];
                    schemaTab.Text = schema.getSchemaName();
                    xtraTabControlDatabase.SelectedTabPageIndex = 0;
                    DisplaySchemaDetail(schema);
                }
                if (node.Tag is FPRDBRelation relation)
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
            AppStates.loadFPRDBSchemas= this.databaseService.getFPRDBSchemas();
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
            AppStates.loadFPRDBSchemaRelations= this.databaseService.getFPRDBRelations();
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
                    //instanceNode.Tag = relation;
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
            using(frmNewSchema childform = new frmNewSchema(compRoot))
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
            using(frmNewRelation childForm = new frmNewRelation(compRoot))
            {
                if (childForm.ShowDialog() == DialogResult.OK)
                    reLoadDatabaseTree();
            }
            
        }
        #endregion
    }
}
