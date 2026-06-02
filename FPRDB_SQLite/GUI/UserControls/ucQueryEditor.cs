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
        }
        public void ViewError()
        {
            splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
            xtraTabControlResultQuery.SelectedTabPage = MessagextraTabPage;
            gridControlResultQuery.DataSource = null;
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
            //QueryResultxtraTabPage.Controls.Clear();
            memoEditMessage.Text = string.Empty;
        }
        // Method to create a new grdResultQuery
        public void CreateNewGridResult(DataTable dt)
        {
            var panelContainer = new DevExpress.XtraEditors.PanelControl();
            panelContainer.Height = 280; // Bạn có thể tăng/giảm chiều cao bảng tùy ý
            panelContainer.Dock = System.Windows.Forms.DockStyle.Top;
            panelContainer.Padding = new System.Windows.Forms.Padding(5, 5, 5, 15); // Tạo khoảng trống cách bảng dưới
            panelContainer.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            var gridControl = new DevExpress.XtraGrid.GridControl();
            var gridView = new DevExpress.XtraGrid.Views.Grid.GridView();

            gridControl.MainView = gridView;
            gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView });
            gridControl.Dock = System.Windows.Forms.DockStyle.Fill;

            // Cấu hình GridView hiển thị dữ liệu giống logic cũ của bạn
            gridControl.DataSource = dt;
            gridView.PopulateColumns();

            // Tùy chỉnh thêm để giao diện gọn gàng khi xếp chồng
            gridView.OptionsBehavior.Editable = false;
            gridView.OptionsView.ShowGroupPanel = false; // Tắt khu vực kéo thả nhóm cột

            // 3. Đưa các control vào panel bọc ngoài
            panelContainer.Controls.Add(gridControl);
            QueryResultxtraTabPage.Controls.Add(panelContainer);

            panelContainer.BringToFront();

            gridControl.ForceInitialize();
            int contentHeight = CalculateGridHeight(gridView);
            panelContainer.Height = contentHeight + panelContainer.Padding.Bottom;
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
