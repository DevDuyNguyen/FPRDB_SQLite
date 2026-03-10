using DevExpress.XtraEditors;
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

namespace FPRDB_SQLite.GUI
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private DatabaseService databaseService;
        public frmMain(DatabaseService databaseService)
        {
            this.databaseService = databaseService;
            InitializeComponent();
        }

        private void buttonAdd_groupDis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new frmAddDiscreteFuzzySet().ShowDialog();
        }

        private void buttonAdd_groupCont_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new frmAddContinuousFuzzySet().ShowDialog();
        }

        private void LoadDatabaseTree()
        {
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
            var schemas = this.databaseService.getFPRDBSchemas();

            if (schemas != null && schemas.Count > 0)
            {
                // Cuốn sổ ghi nhớ các Lược đồ đã vẽ
                HashSet<string> addedSchemas = new HashSet<string>();

                foreach (var schema in schemas)
                {
                    string schemaName = schema.getSchemaName();

                    // LỌC: Nếu Lược đồ này đã vẽ rồi thì bỏ qua luôn, chuyển sang cái tiếp theo
                    if (addedSchemas.Contains(schemaName)) continue;

                    // Đánh dấu là đã vẽ
                    addedSchemas.Add(schemaName);

                    TreeNode schemaNode = new TreeNode(schemaName);
                    schemaNode.ImageIndex = 8;
                    schemaNode.SelectedImageIndex = 8;
                    tablesRootNode.Nodes.Add(schemaNode);

                    var fields = schema.getFields();
                    if (fields != null)
                    {
                        List<string> primaryKeys = schema.getPrimarykey();

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
            var relations = this.databaseService.getFPRDBRelations();

            if (relations != null && relations.Count > 0)
            {
                // Cuốn sổ ghi nhớ các Quan hệ đã vẽ
                HashSet<string> addedRelations = new HashSet<string>();

                foreach (var relation in relations)
                {
                    string relName = relation.getRelName();

                    // LỌC: Nếu Quan hệ này đã vẽ rồi thì bỏ qua luôn
                    if (addedRelations.Contains(relName)) continue;

                    // Đánh dấu là đã vẽ
                    addedRelations.Add(relName);

                    TreeNode instanceNode = new TreeNode(relName);
                    instanceNode.ImageIndex = 7;
                    instanceNode.SelectedImageIndex = 7;
                    relationRootNode.Nodes.Add(instanceNode);

                    var refSchema = relation.getSchema();
                    if (refSchema != null)
                    {
                        string refSchemaName = refSchema.getSchemaName();
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

        private string GetRootPath(string path)
        {
            // Hàm này tự động hiểu và lấy ra "C:\", "D:\"... một cách an toàn
            return System.IO.Path.GetPathRoot(path);
        }

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
                    LoadDatabaseTree();
                }
                catch (IOException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

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
                    LoadDatabaseTree();
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
    }
}
