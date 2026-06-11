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
            int contentHeight = CalculateGridHeight(gridView);
            panelContainer.Height = contentHeight + panelContainer.Padding.Bottom;

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
        }
        private int CalculateGridHeight(DevExpress.XtraGrid.Views.Grid.GridView view, int maxRowsBeforeScroll = 10)
        {
            // Lấy thông tin hiển thị thực tế của Grid (GridInfo)
            var viewInfo = view.GetViewInfo() as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo;

            if (viewInfo == null || viewInfo.RowsInfo.Count == 0)
            {
                int estimatedRowHeight = 22;
                int estimatedHeaderHeight = 28;
                int rowCount = Math.Min(view.DataRowCount, maxRowsBeforeScroll);
                return estimatedHeaderHeight + (rowCount * estimatedRowHeight) + 10;
            }

            // 1. Lấy chiều cao của phần Header cột
            int headerHeight = viewInfo.ColumnRowHeight;

            // 2. Tính tổng chiều cao của các dòng dữ liệu (Giới hạn tối đa số dòng hiển thị)
            int rowsHeight = 0;
            int rowsToCount = Math.Min(view.DataRowCount, maxRowsBeforeScroll);

            for (int i = 0; i < rowsToCount; i++)
            {
                // Nếu đã có UI info thì lấy chiều cao chính xác của dòng đó, ngược lại lấy chiều cao mặc định
                rowsHeight += (i < viewInfo.RowsInfo.Count) ? viewInfo.RowsInfo[i].Bounds.Height : view.RowHeight;
            }

            // 3. Cộng thêm khoảng sai số của Border (đường viền trên dưới của Grid)
            int totalHeight = headerHeight + rowsHeight + 8;

            return totalHeight;
        }
    }

}
