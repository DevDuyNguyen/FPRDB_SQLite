using DevExpress.XtraEditors;
using DevExpress.XtraScheduler.Outlook.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL.Common;
using BLL.DTO;
using BLL;
using DevExpress.XtraRichEdit.API.Native;
using BLL.Services;
using DevExpress.Map.Kml.Model;

namespace FPRDB_SQLite.GUI
{
    public partial class frmManageFuzzySet : DevExpress.XtraEditors.XtraForm
    {
        // Khai báo CompositionRoot để sử dụng các service
        private CompositionRoot root = new CompositionRoot();
        // Khai báo Fuzzy Set Service
        private FuzzySetService service;
        public frmManageFuzzySet()
        {
            InitializeComponent();
            service = root.getFuzzySetService();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ ô tìm kiếm
            string keyword = txtFuzzySetName.Text.ToLower();
            // Xóa danh sách trước đó trong ListBox
            lstFuzzySetResults.Items.Clear();
            // Lọc dữ liệu theo keyword
            //var results = service.findFuzzySet(keyword);
            //if (results.Count == 0)
            //{
            //    MessageBox.Show("Không tìm thấy kết quả nào phù hợp.");
            //    return;
            //}
            //// Thiết lập DisplayMember để hiển thị tên fuzzy set trong ListBox (theo thuộc tính trong FuzzySetDTO)
            //lstFuzzySetResults.DisplayMember = "fuzzySetName";
            //// Hiển thị kết quả lọc
            //lstFuzzySetResults.Items.AddRange(results.ToArray());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFuzzySetName.Clear();
            lstFuzzySetResults.Items.Clear();
            pnlFuzzySetMeaning.Visible = false;
        }

        private void lstFuzzySetResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFuzzySetResults.SelectedItem != null)
            {
                var selectedItem = (FuzzySetDTO)lstFuzzySetResults.SelectedItem;
                pnlFuzzySetMeaning.Visible = true;
                // Hiển thị thông tin chi tiết của fuzzy set đã chọn
                if (selectedItem.fuzzySetType == FieldType.distFS_INT
                    || selectedItem.fuzzySetType == FieldType.distFS_FLOAT
                    || selectedItem.fuzzySetType == FieldType.distFS_TEXT)
                {
                    // Thông tin chi tiết Discrete Fuzzy Set
                    discreteFuzzySetInfo.LoadFuzzySet(selectedItem);
                    discreteFuzzySetInfo.Visible = true;
                    continuosFuzzySetInfo.Visible = false;
                }
                else
                {
                    // Thông tin chi tiết Continuous Fuzzy Set
                    continuosFuzzySetInfo.LoadFuzzySet(selectedItem);
                    discreteFuzzySetInfo.Visible = false;
                    continuosFuzzySetInfo.Visible = true;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Sử dụng hàm isDeleteable và removeFuzzySet từ service
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }
    }
}