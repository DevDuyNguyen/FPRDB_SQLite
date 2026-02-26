using System;

namespace FPRDB_SQLite
{
    public class FieldInfo
    {
        // 1. Chuyển Enum ra public để bên ngoài có thể truyền loại Field vào
        public enum FieldType
        {
            Int, FLOAT, CHAR, VARCHAR, BOOLEAN, distFS_INT, distFS_FLOAT, distFS_TEXT, contFS
        }

        // 2. Sử dụng Auto-implemented Properties (get; set;)
        // Bạn có thể khởi tạo giá trị mặc định ngay tại đây
        private FieldType Type;

        private int TxtLength { get; set; } = 255;

        // 3. Constructor (Tùy chọn) để gán giá trị nhanh khi tạo mới
        public FieldInfo(FieldType type, int length = 0)
        {
            Type = type;
            TxtLength = length;
        }
        private FieldType GetType()
        {
            return Type;
        }
        private int GetTxtLength()
        {
            return TxtLength;
        }
    }
}