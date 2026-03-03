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
using BLL;

namespace FPRDB_SQLite
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private DatabaseService databaseService;
        public Form1(DatabaseService databaseService)
        {
            this.databaseService = databaseService;
;            InitializeComponent();
        }
        private void LoadDatabaseTree()
        {
            // 1. Xóa cây cũ
            treeView1.Nodes.Clear();

            // Lấy tên file Database để hiển thị (ví dụ từ C:\Data\SimpleDatabase.pdb -> SIMPLEDATABASE)
            string dbName = "DATABASE";
            if (!string.IsNullOrEmpty(this.databaseService.getDatabaseName()))
            {
                dbName = this.databaseService.getDatabaseName();
            }

            // 2. Tạo Node Root (Tên Database - Icon số 3)
            TreeNode rootNode = new TreeNode(dbName);
            rootNode.ImageIndex = 3;
            rootNode.SelectedImageIndex = 3;
            treeView1.Nodes.Add(rootNode);

            // 3. Tạo Node Folder "Tables" (Icon số 4)
            TreeNode tablesFolderNode = new TreeNode("Tables");
            tablesFolderNode.ImageIndex = 4;
            tablesFolderNode.SelectedImageIndex = 4;
            rootNode.Nodes.Add(tablesFolderNode); // Thêm thư mục Tables vào Root

            // 4. Lấy dữ liệu
            var relations = this.databaseService.getFPRDBRelations();

            // Xử lý nếu DB trống
            if (relations == null || relations.Count == 0)
            {
                // Tạo một Node con để báo hiệu DB đang trống
                TreeNode emptyNode = new TreeNode("(Chưa có bảng dữ liệu nào)");
                emptyNode.ImageIndex = 4;         // Dùng icon folder.png của bạn
                emptyNode.SelectedImageIndex = 4;

                tablesFolderNode.Nodes.Add(emptyNode); // Nhét vào rootNode để nó có thể bấm xổ xuống
                treeView1.ExpandAll();
                return; // Dừng hàm tại đây
            }

            // 5. Duyệt qua các bảng và nhét vào thư mục "Tables"
            foreach (var relation in relations)
            {
                var schema = relation.getSchema();
                if (schema == null) continue;

                string tableName = schema.getSchemaName();
                // Node Bảng (Icon số 7 - relation.jpg)
                TreeNode tableNode = new TreeNode(tableName);
                tableNode.ImageIndex = 7;
                tableNode.SelectedImageIndex = 7;

                var fields = schema.getFields();
                if (fields != null && fields.Count > 0)
                {
                    // Lấy danh sách khóa chính của cái Bảng (Schema) này ra
                    List<string> primaryKeys = schema.getPrimarykey();

                    foreach (var field in fields)
                    {
                        string fieldName = field.getFieldName();
                        TreeNode fieldNode = new TreeNode(fieldName);

                        // KIỂM TRA: Nếu tên cột này có mặt trong danh sách khóa chính của Bảng
                        if (primaryKeys != null && primaryKeys.Contains(fieldName))
                        {
                            fieldNode.ImageIndex = 5;         // Icon chìa khóa (key.png)
                            fieldNode.SelectedImageIndex = 5;
                        }
                        else
                        {
                            fieldNode.ImageIndex = 2;         // Icon cột bình thường (attribute.png)
                            fieldNode.SelectedImageIndex = 2;
                        }

                        tableNode.Nodes.Add(fieldNode); // Nhét Cột vào Bảng
                    }
                }

                // Quan trọng: Nhét Bảng vào thư mục "Tables" (Thay vì nhét vào Root như trước)
                tablesFolderNode.Nodes.Add(tableNode);
            }

            // Mở rộng tất cả các nhánh để hiển thị giống hình mẫu
            treeView1.ExpandAll();
        }
        private string GetRootPath(string path)
        {
            // Hàm này tự động hiểu và lấy ra "C:\", "D:\"... một cách an toàn
            return System.IO.Path.GetPathRoot(path);
        }
        private void newDB_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

        private void OpenDB_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                catch(IOException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }
    }
}
