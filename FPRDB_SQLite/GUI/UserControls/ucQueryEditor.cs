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

        public ucQueryEditor()
        {
            InitializeComponent();
            splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;
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
    }
}
