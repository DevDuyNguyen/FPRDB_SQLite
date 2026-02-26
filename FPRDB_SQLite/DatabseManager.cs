using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPRDB_SQLite
{
    public class DatabseManager
    {
        private string GetRootPath(string path)
        {
            // Hàm này tự động hiểu và lấy ra "C:\", "D:\"... một cách an toàn
            return System.IO.Path.GetPathRoot(path);
        }
        public void SaveDB()
        {
            try
            {
                SaveFileDialog DialogNew = new SaveFileDialog();
                DialogNew.Title = "Create New Fuzzy Probabilistic Relational Database (FPRDB)";
                DialogNew.Filter = "Database file (*.pdb)|*.pdb";
                DialogNew.DefaultExt = "pdb";
                DialogNew.AddExtension = true;
                DialogNew.RestoreDirectory = true;
                DialogNew.InitialDirectory = GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());
                DialogNew.SupportMultiDottedExtensions = true;

                if (DialogNew.ShowDialog() == DialogResult.OK)
                {
                    // 1. Tạo trực tiếp một file SQLite mới tại đường dẫn người dùng chọn
                    System.Data.SQLite.SQLiteConnection.CreateFile(DialogNew.FileName);

                    // 2. Cập nhật chuỗi kết nối mới vào Resource
                    Resource.ConnectionString = $"Data Source={DialogNew.FileName};Version=3;";
                    // 3. Cập nhật lại giao diện
                    using (var conn = new System.Data.SQLite.SQLiteConnection(Resource.ConnectionString))
                    {
                        conn.Open();
                        string createTableQuery = "CREATE TABLE SinhVien (ID INTEGER PRIMARY KEY AUTOINCREMENT, HoTen VARCHAR, Diem FLOAT)";
                        using (var cmd = new System.Data.SQLite.SQLiteCommand(createTableQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    XtraMessageBox.Show("Create new database successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                DialogNew.Dispose();
            }
            catch (Exception Ex)
            {
                XtraMessageBox.Show(Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        public List<FPRDBSchema> getFPRDBSchemas()
        {
            List<FPRDBSchema> schemas = new List<FPRDBSchema>();

            // Lấy chuỗi kết nối (dựa theo code trong hàm loadDB của bạn)
            string connectionString = Resource.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
                return schemas;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Lấy danh sách các bảng (Schema)
                    // Thêm điều kiện: KHÔNG lấy những bảng có tên bắt đầu bằng chữ 'sqlite_'
                    string queryTables = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%';";
                    using (SQLiteCommand cmdTables = new SQLiteCommand(queryTables, conn))
                    {
                        using (SQLiteDataReader readerTables = cmdTables.ExecuteReader())
                        {
                            while (readerTables.Read())
                            {
                                FPRDBSchema schema = new FPRDBSchema();
                                string tableName = readerTables["name"].ToString();

                                // Gán tên bảng (Bạn cần viết thêm hàm setSchemaName trong FPRDBSchema)
                                schema.setSchemaName(tableName);

                                List<Field> fields = new List<Field>();

                                // Lấy danh sách các cột (Fields) của bảng đó
                                string queryColumns = $"PRAGMA table_info('{tableName}');";
                                using (SQLiteCommand cmdCols = new SQLiteCommand(queryColumns, conn))
                                {
                                    using (SQLiteDataReader readerCols = cmdCols.ExecuteReader())
                                    {
                                        while (readerCols.Read())
                                        {
                                            Field field = new Field();
                                            string colName = readerCols["name"].ToString();

                                            // 1. Kiểm tra xem có phải khóa chính không
                                            if (Convert.ToInt32(readerCols["pk"]) > 0)
                                            {
                                                // 2. Nếu đúng, thêm TÊN CỘT đó vào danh sách PrimaryKeys của schema
                                                // (Dùng schema.PrimaryKeys hoặc schema.getPrimaryKeys() tùy cách bạn viết)
                                                schema.PrimaryKeys.Add(colName);
                                            }

                                            // Gán dữ liệu cho Field bình thường
                                            field.setFieldName(colName);

                                            fields.Add(field);
                                        }
                                    }
                                }

                                // Gán danh sách fields vào schema (Bạn cần viết thêm hàm setFields)
                                schema.setFields(fields);

                                schemas.Add(schema);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                XtraMessageBox.Show(Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return schemas;
        }
        public List<FPRDBRelation> getFPRDBRelations()
        {
            List<FPRDBRelation> relations = new List<FPRDBRelation>();

            try
            {
                // Tận dụng hàm đã viết để lấy toàn bộ Schemas (Cấu trúc các bảng)
                List<FPRDBSchema> schemas = this.getFPRDBSchemas();

                foreach (FPRDBSchema schema in schemas)
                {
                    FPRDBRelation relation = new FPRDBRelation();

                    // 1. Lấy tên từ Schema để làm tên cho Relation (thường trùng với tên bảng trong SQLite)
                    string name = schema.getSchemaName();
                    relation.setRelName(name);

                    // 2. Gán toàn bộ cấu trúc Schema đó vào Relation
                    relation.setSchema(schema);

                    relations.Add(relation);
                }
            }
            catch (Exception Ex)
            {
                // Sử dụng DevExpress XtraMessageBox như cấu trúc của bạn
                DevExpress.XtraEditors.XtraMessageBox.Show("Lỗi khi tải Relations: " + Ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return relations;
        }
        public bool openDB()
        {
            try
            {
                OpenFileDialog DialogOpen = new OpenFileDialog();
                DialogOpen.Title = "Open Fuzzy Probabilistic Relational Database (FPRDB)";
                // Chỉ lọc ra các file có đuôi .pdb hoặc .sqlite
                DialogOpen.Filter = "Database file (*.pdb)|*.pdb|SQLite file (*.sqlite;*.db)|*.sqlite;*.db|All files (*.*)|*.*";
                DialogOpen.DefaultExt = "pdb";
                DialogOpen.InitialDirectory = GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());

                if (DialogOpen.ShowDialog() == DialogResult.OK)
                {
                    // 1. Cập nhật chuỗi kết nối trỏ tới file người dùng vừa chọn
                    Resource.ConnectionString = $"Data Source={DialogOpen.FileName};Version=3;";

                    XtraMessageBox.Show("Open database successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DialogOpen.Dispose();
                    return true; // Trả về true nếu mở thành công
                }

                DialogOpen.Dispose();
                return false; // Trả về false nếu người dùng bấm Cancel
            }
            catch (Exception Ex)
            {
                XtraMessageBox.Show(Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
