using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Services;
using BLL.SQLProcessing;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using GUI.GlobalStates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
        private SQLProcessor sqlProcessor;
        public frmMain(CompositionRoot compRoot)
        {
            this.compRoot = compRoot;
            this.databaseService = this.compRoot.getDatabaseService();
            this.sqlProcessor = this.compRoot.getSQLProcessor();
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
        // Hàm xử lý sự kiện khi click "Select top 100 tuples"
        private void barButtonSelectTuples_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraMessageBox.Show("Select top 100 tuples successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        // Hàm show popup menu cho node relation
        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = treeView.GetNodeAt(e.X, e.Y);
                if (node == null) return;

                treeView.SelectedNode = node;

                if (node.Tag?.ToString() == "relation")
                {
                    popupMenuTreeView.ShowPopup(treeView.PointToScreen(e.Location));
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
                    instanceNode.Tag = "relation";
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
            XtraTabPage queryTab = xtraTabControlDatabase.TabPages[2]; // use your tab name
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
        // Hàm xử lý sự kiện click cho nút Xóa lược đồ
        private void iDeleteSchema_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 1. Lấy node đang được chọn trên TreeView
            TreeNode selectedNode = treeView.SelectedNode;

            // Kiểm tra xem người dùng đã chọn đúng node lược đồ chưa
            if (selectedNode == null || selectedNode.Tag?.ToString() != "schema")
            {
                XtraMessageBox.Show("Please select a schema to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string schemaName = selectedNode.Text;

            try
            {
                // 2. Kiểm tra khi không có relation nào tham chiếu
                var allRelations = this.databaseService.getFPRDBRelations();

                // Lọc ra các quan hệ đang sử dụng lược đồ này
                var dependentRelations = allRelations.Where(r => r.fprdbSchema != null && r.fprdbSchema.schemaName == schemaName)
                    .Select(r => r.relName).ToList();

                // Xử lý ngoại lệ
                if (dependentRelations.Count > 0)
                {
                    string rellList = string.Join(", ", dependentRelations);
                    XtraMessageBox.Show($"Cannot delete schema '{schemaName}' because it is referenced by the following relations: {rellList}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3. Xác nhận xóa lược đồ
                DialogResult result = XtraMessageBox.Show($"Are you sure you want to delete the schema '{schemaName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // GỌI XUỐNG TẦNG SERVICE ĐỂ XÓA (Bạn cần đảm bảo databaseService có hàm này)
                    // Ví dụ: bool isDeleted = this.databaseService.removeFPRDBSchema(schemaName);

                    // tạm thời giả lập gọi hàm
                    //bool isDeleted = true; // Giả sử xóa thành công

                    //if (isDeleted)
                    //{
                    //    XtraMessageBox.Show("Schema deleted successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    // 4. Cập nhật lại TreeView sau khi xóa thành công
                    //    LoadDatabaseTree();
                    //}
                    //else
                    //{
                    //    XtraMessageBox.Show("Failed to delete schema. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                    // ---- BẮT ĐẦU THỰC THI XÓA DƯỚI DATABASE ----

                    //bool isDeleted = this.databaseService.removeFPRDBSchema(schemaName); hàm nếu có
                    bool isDeleted = false;
                    string sqlDropCommand = $"DROP TABLE {schemaName}"; // lệnh string SQL để xóa lược đồ [not don't]
                    isDeleted = this.sqlProcessor.executeDataDefinition(sqlDropCommand);

                    if (isDeleted)
                    {
                        XtraMessageBox.Show("Schema deleted successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 4. Cập nhật lại cache AppStates từ Database trước khi vẽ lại cây
                        AppStates.loadFPRDBSchemas = this.databaseService.getFPRDBSchemas();
                        AppStates.loadFPRDBSchemaRelations = this.databaseService.getFPRDBRelations();

                        // 5. Cập nhật lại TreeView sau khi xóa thành công
                        reLoadDatabaseTree();
                    }
                    else
                    {
                        XtraMessageBox.Show("Failed to delete schema. Check syntax or database constraints.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // Kết thúc thực thi xóa
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"An error occurred while trying to delete the schema: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void iDeleteRelation_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 1. Lấy node đang được chọn trên TreeView
            TreeNode selectedNode = treeView.SelectedNode;

            // Kiểm tra xem người dùng đã chọn đúng node quan hệ chưa
            if (selectedNode == null || selectedNode.Tag?.ToString() != "relation")
            {
                XtraMessageBox.Show("Please select a relation to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string relName = selectedNode.Text;

            try
            {
                // 2. Kiểm tra Quy tắc nghiệp vụ B6: Không có base relation nào khác trỏ tới nó (Khóa ngoại)
                // LƯU Ý: Ở tầng DatabaseService, cần viết hàm (getReferencingRelations) kiểm tra xem có quan hệ nào đang chứa khóa ngoại trỏ tới quan hệ đang xóa không
                // để query các ConstraintDTO có ConstraintType == REFERENTIAL và referencedRelation trùng với relName.
                // Dưới đây là cách gọi giả định:
                //List<string> referencingRelations = this.databaseService.getReferencingRelations(relName);
                List<string> referencingRelations = null; // giả lập
                // Xử lý ngoại lệ E1
                if (referencingRelations != null && referencingRelations.Count > 0)
                {
                    string relList = string.Join(", ", referencingRelations);
                    XtraMessageBox.Show($"Cannot delete relation '{relName}' because it is referenced by foreign keys in the following relations: {relList}",
                                        "Constraint Violation (B6)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3. Luồng cơ bản: Xác nhận xóa
                DialogResult result = XtraMessageBox.Show($"Are you sure you want to delete the relation '{relName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Lấy đối tượng relation DTO hiện tại từ cache AppStates để truyền vào Service
                    var relToDelete = AppStates.loadFPRDBSchemaRelations.FirstOrDefault(r => r.relName == relName);

                    if (relToDelete == null) return;

                    // 4. Gọi Service thực thi xóa dữ liệu dưới Database
                    //bool isDeleted = this.databaseService.removeFPRDBRelation(relToDelete);
                    bool isDeleted = false;// giả lập khi chưa có hàm removeFPRDBRelation
                    if (isDeleted)
                    {
                        XtraMessageBox.Show("Relation deleted successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 5. Cập nhật lại cache AppStates từ Database
                        AppStates.loadFPRDBSchemaRelations = this.databaseService.getFPRDBRelations();

                        // 6. Cập nhật lại giao diện TreeView
                        reLoadDatabaseTree();
                    }
                    else
                    {
                        XtraMessageBox.Show("Failed to delete relation. Please check database constraints.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"An error occurred while trying to delete the relation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
