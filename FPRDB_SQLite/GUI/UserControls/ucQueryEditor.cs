using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPRDB_SQLite.GUI.GUI.UserControls
{
    public partial class ucQueryEditor : DevExpress.XtraEditors.XtraUserControl
    {
        // Sql text
        public string QueryText => memoEditTxtQuery.Text;
        // Output result
        public DevExpress.XtraGrid.GridControl GridResult => gridControlResultQuery;
        public DevExpress.XtraGrid.Views.Grid.GridView GridViewResult => gridViewResultQuery;
        public DevExpress.XtraEditors.MemoEdit memoEditMessageUC => memoEditMessage;
        public DevExpress.XtraTab.XtraTabControl xtraTabControlResult => xtraTabControlResultQuery;
        public DevExpress.XtraEditors.SplitContainerControl splitContainer => splitContainerControl1;
        public DevExpress.XtraTab.XtraTabPage MessageTabPage => MessagextraTabPage;
        // Property to check if the current query is temporary (not saved to a file)
        public bool IsTemporary => string.IsNullOrEmpty(FilePath);
        // Varable to store original content for "Dirty" state checking
        private string originalContent = string.Empty;
        // Event to notify frmMain when "Dirty" state changes
        public Action<bool> OnDirtyStateChanged { get; set; }
        // File path of the current query, empty if it's a temporary file
        public string FilePath { get; set; } = string.Empty;
        public bool IsDBFile { get; set; } = false;

        public ucQueryEditor()
        {
            InitializeComponent();
            splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;
        }
        public void Initialize(string content, string path, bool isDBFile = false)
        {
            this.FilePath = path;
            this.originalContent = content;
            this.memoEditTxtQuery.Text = content;
            this.IsDBFile = isDBFile;
        }

        public void InsertTextAtCursor(string text)
        {
            memoEditTxtQuery.Focus();
            memoEditTxtQuery.SelectedText = text;
            memoEditTxtQuery.SelectionStart = memoEditTxtQuery.SelectionStart;
            memoEditTxtQuery.SelectionLength = 0;
        }
        public void ViewResult()
        {
            splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
            xtraTabControlResultQuery.SelectedTabPage = QueryResultxtraTabPage;
            // Đặt splitter ở vị trí 50% chiều cao
            splitContainerControl1.SplitterPosition = splitContainerControl1.Height / 2;
        }
        public void ViewError()
        {
            splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
            xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            gridControlResultQuery.DataSource = null;
            // Đặt splitter ở vị trí 50% chiều cao
            splitContainerControl1.SplitterPosition = splitContainerControl1.Height / 2;
        }
        private void memoEditTxtQuery_TextChanged(object sender, EventArgs e)
        {
            bool isDirty = (IsTemporary && !IsDBFile) || (memoEditTxtQuery.Text != originalContent);

            OnDirtyStateChanged?.Invoke(isDirty);
        }
        public void MarkAsSaved(string savedContent)
        {
            this.originalContent = savedContent;

            OnDirtyStateChanged?.Invoke(false);
        }
        public string GetSelectedQuery()
        {
            if (memoEditTxtQuery.SelectionLength > 0)
            {
                return memoEditTxtQuery.SelectedText;
            }
            return memoEditTxtQuery.Text;
        }
        // Clear previous results before showing new ones
        public void ClearAllGrids()
        {
            QueryResultxtraTabPage.Controls.Clear();
            memoEditMessage.Text = string.Empty;
        }
        // Method to create a new grdResultQuery
        public void CreateNewGridResult(DataTable dt)
        {
            // Nếu đang thêm kết quả thứ 2, chuyển panel đầu từ Fill → Top
            // BringToFront() giữ panel đầu tiên ở vị trí trên cùng (z-order front = top vật lý)
            if (QueryResultxtraTabPage.Controls.Count == 1)
            {
                var firstPanel = QueryResultxtraTabPage.Controls[0];
                firstPanel.Dock = System.Windows.Forms.DockStyle.Top;
                firstPanel.BringToFront();
            }

            bool isFirstGrid = QueryResultxtraTabPage.Controls.Count == 0;

            var panelContainer = new DevExpress.XtraEditors.PanelControl();
            panelContainer.Padding = new System.Windows.Forms.Padding(5, 5, 5, 15);
            panelContainer.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            var gridControl = new DevExpress.XtraGrid.GridControl();
            var gridView = new DevExpress.XtraGrid.Views.Grid.GridView();

            gridControl.MainView = gridView;
            gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView });
            gridControl.Dock = System.Windows.Forms.DockStyle.Fill;

            gridControl.DataSource = dt;
            gridView.PopulateColumns();
            gridView.OptionsBehavior.Editable = false;
            gridView.OptionsView.ShowGroupPanel = false;

            panelContainer.Controls.Add(gridControl);

            // Thêm vào cuối danh sách để đảm bảo thứ tự từ trên xuống dưới
            // (Controls[0] = kết quả đầu tiên ở trên cùng)
            QueryResultxtraTabPage.Controls.Add(panelContainer);


            gridControl.ForceInitialize();

            if (isFirstGrid)
            {
                // Chỉ 1 kết quả → fill toàn bộ vùng tab
                panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            }
            else
            {
                // Nhiều kết quả → xếp chồng Top dưới panel đầu tiên
                // Panel1 đã BringToFront (trên cùng), panel mới Controls.Add sẽ tự nằm phía sau/dưới
                panelContainer.Dock = System.Windows.Forms.DockStyle.Top;
            }

            QueryResultxtraTabPage.Update();
            QueryResultxtraTabPage.PerformLayout();

            // Bây giờ, tính toán chiều cao dựa trên gridView đã được xếp vị trí chuẩn
            int contentHeight = CalculateGridHeight(gridView, 5);

            // Gán chiều cao chính xác cho Panel (nếu là first grid thì Dock=Fill sẽ ghi đè lên giá trị này, 
            // nhưng khi dock chuyển sang Top thì chiều cao chính xác đã được lưu sẵn)
            panelContainer.Height = contentHeight + panelContainer.Padding.Bottom;

            // Làm mới lại giao diện lần cuối để tránh hiện tượng vỡ hình
            panelContainer.Update();
        }
        private int CalculateGridHeight(DevExpress.XtraGrid.Views.Grid.GridView view, int maxRowsBeforeScroll = 5)
        {
            // Lấy thông tin hiển thị thực tế của Grid (GridInfo)
            int rowHeight = view.RowHeight;
            if (rowHeight <= 0)
            {
                // Tính toán chiều cao dòng dựa trên Font chữ hiện tại nếu RowHeight = -1
                using (Graphics g = view.GridControl.CreateGraphics())
                {
                    var font = view.Appearance.Row.Font ?? view.GridControl.Font;
                    rowHeight = (int)Math.Ceiling(g.MeasureString("W", font).Height) + 7; // 7px cho padding trên dưới
                }
            }

            // Ensure a minimum row height of 28px (typical for modern DevExpress skins)
            if (rowHeight < 28)
            {
                rowHeight = 28;
            }

            // 2. Xác định chiều cao Header (Default khoảng 28)
            var viewInfo = view.GetViewInfo() as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo;
            int headerHeight = (viewInfo != null && viewInfo.ColumnRowHeight > 0) ? viewInfo.ColumnRowHeight : rowHeight + 8;
            if (headerHeight < 30)
            {
                headerHeight = 30;
            }

            // 3. Tính tổng số dòng cần hiển thị
            int dataRowCount = view.DataRowCount;
            if (dataRowCount == 0) return headerHeight + 20; // Trả về chiều cao cơ bản nếu bảng trống

            int rowsToDisplay = Math.Min(dataRowCount, maxRowsBeforeScroll);
            int totalRowsHeight = 0;

            // Ưu tiên lấy chiều cao thực tế của các dòng đã render, nếu thiếu thì bù bằng rowHeight chuẩn
            for (int i = 0; i < rowsToDisplay; i++)
            {
                if (viewInfo != null && i < viewInfo.RowsInfo.Count && viewInfo.RowsInfo[i].Bounds.Height > 0)
                {
                    totalRowsHeight += viewInfo.RowsInfo[i].Bounds.Height;
                }
                else
                {
                    totalRowsHeight += rowHeight;
                }
            }

            // 4. Cộng thêm khoảng sai số viền (Border) và khoảng trống dự phòng nhỏ (khoảng 10px)
            int finalHeight = headerHeight + totalRowsHeight + 10;
            return finalHeight;
        }
    }

}
