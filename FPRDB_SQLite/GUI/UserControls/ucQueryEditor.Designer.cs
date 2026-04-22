namespace FPRDB_SQLite.GUI.GUI.UserControls
{
    partial class ucQueryEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            memoEditTxtQuery = new DevExpress.XtraEditors.MemoEdit();
            xtraTabControlResultQuery = new DevExpress.XtraTab.XtraTabControl();
            QueryResultxtraTabPage = new DevExpress.XtraTab.XtraTabPage();
            gridControlResultQuery = new DevExpress.XtraGrid.GridControl();
            gridViewResultQuery = new DevExpress.XtraGrid.Views.Grid.GridView();
            MessagextraTabPage = new DevExpress.XtraTab.XtraTabPage();
            memoEditMessage = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1.Panel1).BeginInit();
            splitContainerControl1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1.Panel2).BeginInit();
            splitContainerControl1.Panel2.SuspendLayout();
            splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)memoEditTxtQuery.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabControlResultQuery).BeginInit();
            xtraTabControlResultQuery.SuspendLayout();
            QueryResultxtraTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControlResultQuery).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridViewResultQuery).BeginInit();
            MessagextraTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)memoEditMessage.Properties).BeginInit();
            SuspendLayout();
            // 
            // splitContainerControl1
            // 
            splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerControl1.Horizontal = false;
            splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            splitContainerControl1.Name = "splitContainerControl1";
            // 
            // splitContainerControl1.Panel1
            // 
            splitContainerControl1.Panel1.Controls.Add(memoEditTxtQuery);
            splitContainerControl1.Panel1.Text = "Panel1";
            // 
            // splitContainerControl1.Panel2
            // 
            splitContainerControl1.Panel2.Controls.Add(xtraTabControlResultQuery);
            splitContainerControl1.Panel2.Text = "Panel2";
            splitContainerControl1.Size = new System.Drawing.Size(762, 363);
            splitContainerControl1.SplitterPosition = 129;
            splitContainerControl1.TabIndex = 1;
            // 
            // memoEditTxtQuery
            // 
            memoEditTxtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            memoEditTxtQuery.Location = new System.Drawing.Point(0, 0);
            memoEditTxtQuery.Name = "memoEditTxtQuery";
            memoEditTxtQuery.Size = new System.Drawing.Size(762, 129);
            memoEditTxtQuery.TabIndex = 0;
            // 
            // xtraTabControlResultQuery
            // 
            xtraTabControlResultQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            xtraTabControlResultQuery.Location = new System.Drawing.Point(0, 0);
            xtraTabControlResultQuery.Name = "xtraTabControlResultQuery";
            xtraTabControlResultQuery.SelectedTabPage = QueryResultxtraTabPage;
            xtraTabControlResultQuery.Size = new System.Drawing.Size(762, 222);
            xtraTabControlResultQuery.TabIndex = 0;
            xtraTabControlResultQuery.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] { QueryResultxtraTabPage, MessagextraTabPage });
            // 
            // QueryResultxtraTabPage
            // 
            QueryResultxtraTabPage.Controls.Add(gridControlResultQuery);
            QueryResultxtraTabPage.Name = "QueryResultxtraTabPage";
            QueryResultxtraTabPage.Size = new System.Drawing.Size(760, 192);
            QueryResultxtraTabPage.Text = "Query Result";
            // 
            // gridControlResultQuery
            // 
            gridControlResultQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            gridControlResultQuery.EmbeddedNavigator.Buttons.Append.Visible = false;
            gridControlResultQuery.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            gridControlResultQuery.EmbeddedNavigator.Buttons.Edit.Visible = false;
            gridControlResultQuery.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            gridControlResultQuery.EmbeddedNavigator.Buttons.Remove.Visible = false;
            gridControlResultQuery.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0, true);
            gridControlResultQuery.Location = new System.Drawing.Point(0, 0);
            gridControlResultQuery.MainView = gridViewResultQuery;
            gridControlResultQuery.Name = "gridControlResultQuery";
            gridControlResultQuery.Size = new System.Drawing.Size(760, 192);
            gridControlResultQuery.TabIndex = 0;
            gridControlResultQuery.UseEmbeddedNavigator = true;
            gridControlResultQuery.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridViewResultQuery });
            // 
            // gridViewResultQuery
            // 
            gridViewResultQuery.Appearance.DetailTip.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            gridViewResultQuery.Appearance.DetailTip.Options.UseFont = true;
            gridViewResultQuery.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            gridViewResultQuery.Appearance.HeaderPanel.Options.UseFont = true;
            gridViewResultQuery.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            gridViewResultQuery.Appearance.Row.Options.UseFont = true;
            gridViewResultQuery.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            gridViewResultQuery.Appearance.ViewCaption.Options.UseFont = true;
            gridViewResultQuery.GridControl = gridControlResultQuery;
            gridViewResultQuery.Name = "gridViewResultQuery";
            gridViewResultQuery.OptionsBehavior.Editable = false;
            gridViewResultQuery.OptionsBehavior.ReadOnly = true;
            gridViewResultQuery.OptionsView.ShowGroupPanel = false;
            // 
            // MessagextraTabPage
            // 
            MessagextraTabPage.Controls.Add(memoEditMessage);
            MessagextraTabPage.Name = "MessagextraTabPage";
            MessagextraTabPage.Size = new System.Drawing.Size(684, 155);
            MessagextraTabPage.Text = "Message";
            // 
            // memoEditMessage
            // 
            memoEditMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            memoEditMessage.Location = new System.Drawing.Point(0, 0);
            memoEditMessage.Name = "memoEditMessage";
            memoEditMessage.Properties.ReadOnly = true;
            memoEditMessage.Size = new System.Drawing.Size(684, 155);
            memoEditMessage.TabIndex = 0;
            // 
            // ucQueryEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainerControl1);
            Name = "ucQueryEditor";
            Size = new System.Drawing.Size(762, 363);
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1.Panel1).EndInit();
            splitContainerControl1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1.Panel2).EndInit();
            splitContainerControl1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1).EndInit();
            splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)memoEditTxtQuery.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabControlResultQuery).EndInit();
            xtraTabControlResultQuery.ResumeLayout(false);
            QueryResultxtraTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControlResultQuery).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridViewResultQuery).EndInit();
            MessagextraTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)memoEditMessage.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.MemoEdit memoEditTxtQuery;
        private DevExpress.XtraTab.XtraTabControl xtraTabControlResultQuery;
        private DevExpress.XtraTab.XtraTabPage QueryResultxtraTabPage;
        private DevExpress.XtraGrid.GridControl gridControlResultQuery;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewResultQuery;
        private DevExpress.XtraTab.XtraTabPage MessagextraTabPage;
        private DevExpress.XtraEditors.MemoEdit memoEditMessage;
    }
}
