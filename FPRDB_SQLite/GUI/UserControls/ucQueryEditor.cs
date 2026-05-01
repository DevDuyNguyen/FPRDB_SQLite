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
    }
}
