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

namespace FPRDB_SQLite.GUI
{
    public partial class frmManageFuzzySet : DevExpress.XtraEditors.XtraForm
    {
        List<string> fuzzySetData = new List<string>();
        public frmManageFuzzySet()
        {
            InitializeComponent();
        }
        private void frmManageFuzzySet_Load(object sender, EventArgs e)
        {
            // Thêm dữ liệu giả vào List để lưu thông tin
            fuzzySetData.Add("Discrete");
            fuzzySetData.Add("Continuous");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtFuzzySetName.Text.ToLower();
            // Xóa danh sách trước đó
            lstFuzzySetResults.Items.Clear();
            // Lọc dữ liệu theo keyword
            var results = fuzzySetData.Where(x => x.ToLower().Contains(keyword)).ToList();
            // Hiển thị kết quả lọc
            lstFuzzySetResults.Items.AddRange(results.ToArray());
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
                string selectedName = lstFuzzySetResults.SelectedItem.ToString();
                MessageBox.Show("Bạn đã chọn: " + selectedName);
                pnlFuzzySetMeaning.Visible = true;
                if (selectedName == "Discrete")
                {
                    discreteFuzzySetInfo.Visible = true;
                    continuosFuzzySetInfo.Visible = false;
                }
                else if (selectedName == "Continuous")
                {
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

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }
    }
}