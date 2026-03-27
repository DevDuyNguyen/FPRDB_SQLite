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
using DevExpress.XtraCharts;

namespace FPRDB_SQLite.GUI
{
    public partial class frmManageFuzzySet : DevExpress.XtraEditors.XtraForm
    {
        // Khai báo CompositionRoot để sử dụng các service
        private CompositionRoot compRoot = new CompositionRoot();
        // Khai báo Fuzzy Set Service
        private FuzzySetService service;
        // Khai báo danh sách để lưu kết quả tìm kiếm Fuzzy Set
        private List<FuzzySetDTO> results;
        // Khai báo DTO để lưu thông tin Fuzzy Set đang được chọn
        private FuzzySetDTO selectedFuzzySet;
        public frmManageFuzzySet(CompositionRoot compRoot)
        {
            InitializeComponent();
            this.compRoot = compRoot;
            this.service = compRoot.getFuzzySetService();
        }

        // Hàm xử lý khi nhấn nút "Search" để tìm kiếm Fuzzy Set theo tên
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ ô tìm kiếm
            string keyword = txtFuzzySetName.Text;
            if (string.IsNullOrWhiteSpace(keyword))
            {
                XtraMessageBox.Show("Please enter a fuzzy set name to search.");
                return;
            }
            // Xóa danh sách trước đó trong ListBox
            lstFuzzySetResults.Items.Clear();
            // Lọc dữ liệu theo keyword
            this.results = service.findFuzzySet(keyword);
            if (results.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả nào phù hợp.");
                return;
            }
            //ContinuousFuzzySetDTO continuousFuzzySet = new ContinuousFuzzySetDTO(10, 20, 30, 40, "random2");
            //DiscreteFuzzySetDTO<int> discreteFuzzySet = new DiscreteFuzzySetDTO<int>(
            //    new List<int>() { 22, 23, 24 },
            //    new List<float>() { 0.5f, 1, 0.5f },
            //    "about_23",
            //    FieldType.INT);
            //results = new List<FuzzySetDTO>() { continuousFuzzySet, discreteFuzzySet };
            
            foreach (var item in this.results)
            {
                lstFuzzySetResults.Items.Add(item.fuzzySetName);
            }
        }
        // Hàm vẽ đồ thị của Continuous Fuzzy Set
        private void drawChartContinuousFS(FuzzySetDTO fuzzySet)
        {
            tabpgFuzzySetChart.Controls.Clear();
            ChartControl chart = new ChartControl();
            ContinuousFuzzySetDTO continuousFuzzySet = fuzzySet as ContinuousFuzzySetDTO;
            // Create series and add points
            Series series = new Series(continuousFuzzySet.fuzzySetName, ViewType.Line);
            series.Points.Add(new SeriesPoint(continuousFuzzySet.leftBottom.ToString(), 0));
            series.Points.Add(new SeriesPoint(continuousFuzzySet.leftTop.ToString(), 1));
            series.Points.Add(new SeriesPoint(continuousFuzzySet.rightTop.ToString(), 1));
            series.Points.Add(new SeriesPoint(continuousFuzzySet.rightBottom.ToString(), 0));
            // Add series to chart
            chart.Series.Add(series);
            series.ArgumentScaleType = ScaleType.Numerical;
            ((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Dash;
            ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Circle;
            chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            XYDiagram diagram = (XYDiagram)chart.Diagram;
            diagram.EnableAxisXScrolling = true;
            diagram.AxisX.Title.Text = "Value";
            diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagram.AxisY.Title.Text = "Membership Degree";
            diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;

            chart.Dock = DockStyle.Fill;
            tabpgFuzzySetChart.Controls.Add(chart);
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFuzzySetName.Clear();
            lstFuzzySetResults.Items.Clear();
            pnlFuzzySetMeaning.Visible = false;
        }

        // Hàm xử lý khi người dùng chọn một Fuzzy Set trong danh sách kết quả
        private void lstFuzzySetResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFuzzySetResults.SelectedItem != null)
            {
                selectedFuzzySet = results.FirstOrDefault(r => r.fuzzySetName == lstFuzzySetResults.SelectedItem.ToString());
                pnlFuzzySetMeaning.Visible = true;
                if (selectedFuzzySet is DiscreteFuzzySetDTO<int> ||
                    selectedFuzzySet is DiscreteFuzzySetDTO<float> ||
                    selectedFuzzySet is DiscreteFuzzySetDTO<string>)
                {
                    // Thông tin chi tiết Discrete Fuzzy Set
                    continuosFuzzySetInfo.Visible = false;
                    tabpgFuzzySetChart.PageVisible = false;
                    discreteFuzzySetInfo.Visible = true;
                    discreteFuzzySetInfo.LoadFuzzySet(selectedFuzzySet);
                }
                else
                {
                    // Thông tin chi tiết Continuous Fuzzy Set
                    discreteFuzzySetInfo.Visible = false;
                    continuosFuzzySetInfo.Visible = true;
                    tabpgFuzzySetChart.PageVisible = true;
                    continuosFuzzySetInfo.LoadFuzzySet(selectedFuzzySet);
                    drawChartContinuousFS(selectedFuzzySet);
                }
            }
        }

        // Hàm xử lý khi nhấn nút "Close" để đóng form
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Hàm xử lý khi nhấn nút "Delete" để xóa Fuzzy Set đã chọn
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Are you sure you want to delete fuzzy set '{selectedFuzzySet.fuzzySetName}'?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                //service.removeFuzzySet(selectedFuzzySet.fuzzySetName);
                XtraMessageBox.Show("Fuzzy set deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Refresh the list after deletion
                refreshForm();
            }
        }

        // Hàm xử lý khi nhấn nút "Update" để cập nhật thông tin Fuzzy Set đã chọn
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult result = XtraMessageBox.Show($"Are you sure you want to update fuzzy set '{selectedFuzzySet.fuzzySetName}'?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (selectedFuzzySet is ContinuousFuzzySetDTO)
                {
                    var updatedFuzzySet = continuosFuzzySetInfo.getContinuousFuzzySet();
                }
                else
                {
                    switch (discreteFuzzySetInfo.getFuzzySetType())
                    {
                        case FieldType.INT:
                            var updatedIntFuzzySet = discreteFuzzySetInfo.getDiscreteFuzzySet<int>();
                            break;
                        case FieldType.FLOAT:
                            var updatedFloatFuzzySet = discreteFuzzySetInfo.getDiscreteFuzzySet<float>();
                            break;
                        case FieldType.VARCHAR:
                            var updatedStringFuzzySet = discreteFuzzySetInfo.getDiscreteFuzzySet<string>();
                            break;
                    }
                }
                //service.updateFuzzySet(updatedFuzzySet);
                XtraMessageBox.Show("Fuzzy set updated successfully.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Hàm reset lại form
        private void refreshForm()
        {
            pnlFuzzySetMeaning.Visible = false;
            tabpgFuzzySetChart.Controls.Clear();
            selectedFuzzySet = null;
        }
    }
}