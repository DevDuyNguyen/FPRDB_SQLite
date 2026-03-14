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
            ContinuousFuzzySetDTO continuousFuzzySet = new ContinuousFuzzySetDTO(10,20, 30, 40, "random2");
            DiscreteFuzzySetDTO<int> discreteFuzzySet = new DiscreteFuzzySetDTO<int>(
                new List<int>() { 22, 23, 24 },
                new List<float>() { 0.5f, 1, 0.5f },
                "about_23",
                FieldType.INT);
            List<FuzzySetDTO> results = new List<FuzzySetDTO>() { continuousFuzzySet, discreteFuzzySet };
            foreach (var item in results)
            {
                lstFuzzySetResults.Items.Add(item);
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

        private void lstFuzzySetResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFuzzySetResults.SelectedItem != null)
            {
                var selectedItem = (FuzzySetDTO)lstFuzzySetResults.SelectedItem;
                pnlFuzzySetMeaning.Visible = true;
                if (selectedItem is DiscreteFuzzySetDTO<int>||
                    selectedItem is DiscreteFuzzySetDTO<float>||
                    selectedItem is DiscreteFuzzySetDTO<string>)
                {
                    // Thông tin chi tiết Discrete Fuzzy Set
                    continuosFuzzySetInfo.Visible = false;
                    tabpgFuzzySetChart.PageVisible = false;
                    discreteFuzzySetInfo.Visible = true;
                    discreteFuzzySetInfo.LoadFuzzySet(selectedItem);
                }
                else
                {
                    // Thông tin chi tiết Continuous Fuzzy Set
                    discreteFuzzySetInfo.Visible = false;
                    continuosFuzzySetInfo.Visible = true;
                    tabpgFuzzySetChart.PageVisible = true;
                    continuosFuzzySetInfo.LoadFuzzySet(selectedItem);
                    drawChartContinuousFS(selectedItem);
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