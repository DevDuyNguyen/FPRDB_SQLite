namespace FPRDB_SQLite.GUI
{
    partial class frmManageFuzzySet
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManageFuzzySet));
            splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            grpFuzzySetResults = new DevExpress.XtraEditors.GroupControl();
            lstFuzzySetResults = new DevExpress.XtraEditors.ListBoxControl();
            grpSearchFuzzySet = new DevExpress.XtraEditors.GroupControl();
            btnClear = new DevExpress.XtraEditors.SimpleButton();
            btnSearch = new DevExpress.XtraEditors.SimpleButton();
            txtFuzzySetName = new DevExpress.XtraEditors.TextEdit();
            tabFuzzySetDetail = new DevExpress.XtraTab.XtraTabControl();
            tabpgFuzzySetMeaning = new DevExpress.XtraTab.XtraTabPage();
            pnlFuzzySetMeaning = new DevExpress.XtraEditors.PanelControl();
            continuosFuzzySetInfo = new FPRDB_SQLite.GUI.UserControls.ContinuosFuzzySet();
            btnClose = new DevExpress.XtraEditors.SimpleButton();
            btnDelete = new DevExpress.XtraEditors.SimpleButton();
            btnUpdate = new DevExpress.XtraEditors.SimpleButton();
            discreteFuzzySetInfo = new FPRDB_SQLite.GUI.UserControls.DiscreteFuzzySet();
            tabpgFuzzySetChart = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1.Panel1).BeginInit();
            splitContainerControl1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1.Panel2).BeginInit();
            splitContainerControl1.Panel2.SuspendLayout();
            splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grpFuzzySetResults).BeginInit();
            grpFuzzySetResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lstFuzzySetResults).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grpSearchFuzzySet).BeginInit();
            grpSearchFuzzySet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtFuzzySetName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tabFuzzySetDetail).BeginInit();
            tabFuzzySetDetail.SuspendLayout();
            tabpgFuzzySetMeaning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlFuzzySetMeaning).BeginInit();
            pnlFuzzySetMeaning.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainerControl1
            // 
            splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            splitContainerControl1.Name = "splitContainerControl1";
            // 
            // splitContainerControl1.Panel1
            // 
            splitContainerControl1.Panel1.Controls.Add(grpFuzzySetResults);
            splitContainerControl1.Panel1.Controls.Add(grpSearchFuzzySet);
            splitContainerControl1.Panel1.Text = "Panel1";
            // 
            // splitContainerControl1.Panel2
            // 
            splitContainerControl1.Panel2.Controls.Add(tabFuzzySetDetail);
            splitContainerControl1.Panel2.Text = "Panel2";
            splitContainerControl1.Size = new System.Drawing.Size(847, 400);
            splitContainerControl1.SplitterPosition = 289;
            splitContainerControl1.TabIndex = 0;
            // 
            // grpFuzzySetResults
            // 
            grpFuzzySetResults.Controls.Add(lstFuzzySetResults);
            grpFuzzySetResults.Dock = System.Windows.Forms.DockStyle.Fill;
            grpFuzzySetResults.Location = new System.Drawing.Point(0, 108);
            grpFuzzySetResults.Name = "grpFuzzySetResults";
            grpFuzzySetResults.Size = new System.Drawing.Size(289, 292);
            grpFuzzySetResults.TabIndex = 1;
            grpFuzzySetResults.Text = "Fuzzy Set Results";
            // 
            // lstFuzzySetResults
            // 
            lstFuzzySetResults.Dock = System.Windows.Forms.DockStyle.Fill;
            lstFuzzySetResults.Location = new System.Drawing.Point(2, 28);
            lstFuzzySetResults.Name = "lstFuzzySetResults";
            lstFuzzySetResults.Size = new System.Drawing.Size(285, 262);
            lstFuzzySetResults.TabIndex = 0;
            lstFuzzySetResults.SelectedIndexChanged += lstFuzzySetResults_SelectedIndexChanged;
            // 
            // grpSearchFuzzySet
            // 
            grpSearchFuzzySet.Controls.Add(btnClear);
            grpSearchFuzzySet.Controls.Add(btnSearch);
            grpSearchFuzzySet.Controls.Add(txtFuzzySetName);
            grpSearchFuzzySet.Dock = System.Windows.Forms.DockStyle.Top;
            grpSearchFuzzySet.Location = new System.Drawing.Point(0, 0);
            grpSearchFuzzySet.Name = "grpSearchFuzzySet";
            grpSearchFuzzySet.Size = new System.Drawing.Size(289, 108);
            grpSearchFuzzySet.TabIndex = 0;
            grpSearchFuzzySet.Text = "Search Fuzzy Set";
            // 
            // btnClear
            // 
            btnClear.Location = new System.Drawing.Point(201, 64);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(75, 25);
            btnClear.TabIndex = 2;
            btnClear.Text = "&Clear";
            btnClear.Click += btnClear_Click;
            // 
            // btnSearch
            // 
            btnSearch.Location = new System.Drawing.Point(120, 64);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(75, 25);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "&Search";
            btnSearch.Click += btnSearch_Click;
            // 
            // txtFuzzySetName
            // 
            txtFuzzySetName.Location = new System.Drawing.Point(12, 36);
            txtFuzzySetName.Name = "txtFuzzySetName";
            txtFuzzySetName.Size = new System.Drawing.Size(264, 22);
            txtFuzzySetName.TabIndex = 0;
            // 
            // tabFuzzySetDetail
            // 
            tabFuzzySetDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            tabFuzzySetDetail.Location = new System.Drawing.Point(0, 0);
            tabFuzzySetDetail.Name = "tabFuzzySetDetail";
            tabFuzzySetDetail.SelectedTabPage = tabpgFuzzySetMeaning;
            tabFuzzySetDetail.Size = new System.Drawing.Size(546, 400);
            tabFuzzySetDetail.TabIndex = 0;
            tabFuzzySetDetail.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] { tabpgFuzzySetMeaning, tabpgFuzzySetChart });
            // 
            // tabpgFuzzySetMeaning
            // 
            tabpgFuzzySetMeaning.Controls.Add(pnlFuzzySetMeaning);
            tabpgFuzzySetMeaning.Name = "tabpgFuzzySetMeaning";
            tabpgFuzzySetMeaning.Size = new System.Drawing.Size(544, 370);
            tabpgFuzzySetMeaning.Text = "Detail";
            // 
            // pnlFuzzySetMeaning
            // 
            pnlFuzzySetMeaning.Controls.Add(continuosFuzzySetInfo);
            pnlFuzzySetMeaning.Controls.Add(btnClose);
            pnlFuzzySetMeaning.Controls.Add(btnDelete);
            pnlFuzzySetMeaning.Controls.Add(btnUpdate);
            pnlFuzzySetMeaning.Controls.Add(discreteFuzzySetInfo);
            pnlFuzzySetMeaning.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlFuzzySetMeaning.Location = new System.Drawing.Point(0, 0);
            pnlFuzzySetMeaning.Name = "pnlFuzzySetMeaning";
            pnlFuzzySetMeaning.Size = new System.Drawing.Size(544, 370);
            pnlFuzzySetMeaning.TabIndex = 0;
            pnlFuzzySetMeaning.Visible = false;
            // 
            // continuosFuzzySetInfo
            // 
            continuosFuzzySetInfo.Location = new System.Drawing.Point(-1, 0);
            continuosFuzzySetInfo.Name = "continuosFuzzySetInfo";
            continuosFuzzySetInfo.Size = new System.Drawing.Size(546, 328);
            continuosFuzzySetInfo.TabIndex = 6;
            continuosFuzzySetInfo.Visible = false;
            // 
            // btnClose
            // 
            btnClose.Location = new System.Drawing.Point(460, 334);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(75, 25);
            btnClose.TabIndex = 5;
            btnClose.Text = "&Close";
            btnClose.Click += btnClose_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(379, 334);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(75, 25);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "&Delete";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new System.Drawing.Point(298, 334);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new System.Drawing.Size(75, 25);
            btnUpdate.TabIndex = 3;
            btnUpdate.Text = "&Update";
            btnUpdate.Click += btnUpdate_Click;
            // 
            // discreteFuzzySetInfo
            // 
            discreteFuzzySetInfo.Location = new System.Drawing.Point(0, 0);
            discreteFuzzySetInfo.Name = "discreteFuzzySetInfo";
            discreteFuzzySetInfo.Size = new System.Drawing.Size(539, 330);
            discreteFuzzySetInfo.TabIndex = 0;
            discreteFuzzySetInfo.Visible = false;
            // 
            // tabpgFuzzySetChart
            // 
            tabpgFuzzySetChart.Name = "tabpgFuzzySetChart";
            tabpgFuzzySetChart.Size = new System.Drawing.Size(544, 370);
            tabpgFuzzySetChart.Text = "Chart";
            // 
            // frmManageFuzzySet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(847, 400);
            Controls.Add(splitContainerControl1);
            IconOptions.Image = (System.Drawing.Image)resources.GetObject("frmManageFuzzySet.IconOptions.Image");
            Name = "frmManageFuzzySet";
            Text = "Fuzzy Set Detail";
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1.Panel1).EndInit();
            splitContainerControl1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1.Panel2).EndInit();
            splitContainerControl1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerControl1).EndInit();
            splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grpFuzzySetResults).EndInit();
            grpFuzzySetResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)lstFuzzySetResults).EndInit();
            ((System.ComponentModel.ISupportInitialize)grpSearchFuzzySet).EndInit();
            grpSearchFuzzySet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)txtFuzzySetName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tabFuzzySetDetail).EndInit();
            tabFuzzySetDetail.ResumeLayout(false);
            tabpgFuzzySetMeaning.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pnlFuzzySetMeaning).EndInit();
            pnlFuzzySetMeaning.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.GroupControl grpFuzzySetResults;
        private DevExpress.XtraEditors.GroupControl grpSearchFuzzySet;
        private DevExpress.XtraEditors.ListBoxControl lstFuzzySetResults;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraEditors.TextEdit txtFuzzySetName;
        private DevExpress.XtraTab.XtraTabControl tabFuzzySetDetail;
        private DevExpress.XtraTab.XtraTabPage tabpgFuzzySetMeaning;
        private DevExpress.XtraEditors.PanelControl pnlFuzzySetMeaning;
        private FPRDB_SQLite.GUI.UserControls.DiscreteFuzzySet discreteFuzzySetInfo;
        private DevExpress.XtraTab.XtraTabPage tabpgFuzzySetChart;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnUpdate;
        private FPRDB_SQLite.GUI.UserControls.ContinuosFuzzySet continuosFuzzySetInfo;
    }
}