namespace FPRDB_SQLite.GUI
{
    partial class frmSearchFuzzySet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchFuzzySet));
            pnlSearch = new DevExpress.XtraEditors.PanelControl();
            cboNameFuzzySet = new DevExpress.XtraEditors.ComboBoxEdit();
            btnClear = new DevExpress.XtraEditors.SimpleButton();
            btnSearch = new DevExpress.XtraEditors.SimpleButton();
            lblNameFuzzySet = new DevExpress.XtraEditors.LabelControl();
            pnlResult = new DevExpress.XtraEditors.PanelControl();
            pnlButton = new DevExpress.XtraEditors.PanelControl();
            btnUpdate = new DevExpress.XtraEditors.SimpleButton();
            btnView = new DevExpress.XtraEditors.SimpleButton();
            btnDelete = new DevExpress.XtraEditors.SimpleButton();
            btnCancel = new DevExpress.XtraEditors.SimpleButton();
            continuosFuzzySetInfo = new FPRDB_SQLite.GUI.UserControls.ContinuosFuzzySet();
            discreteFuzzySetInfo = new FPRDB_SQLite.GUI.UserControls.DiscreteFuzzySet();
            ((System.ComponentModel.ISupportInitialize)pnlSearch).BeginInit();
            pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cboNameFuzzySet.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pnlResult).BeginInit();
            pnlResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlButton).BeginInit();
            pnlButton.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSearch
            // 
            pnlSearch.Controls.Add(cboNameFuzzySet);
            pnlSearch.Controls.Add(btnClear);
            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Controls.Add(lblNameFuzzySet);
            pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            pnlSearch.Location = new System.Drawing.Point(0, 0);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new System.Drawing.Size(609, 55);
            pnlSearch.TabIndex = 0;
            // 
            // cboNameFuzzySet
            // 
            cboNameFuzzySet.Location = new System.Drawing.Point(120, 19);
            cboNameFuzzySet.Name = "cboNameFuzzySet";
            cboNameFuzzySet.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cboNameFuzzySet.Size = new System.Drawing.Size(282, 22);
            cboNameFuzzySet.TabIndex = 6;
            // 
            // btnClear
            // 
            btnClear.Location = new System.Drawing.Point(522, 17);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(75, 25);
            btnClear.TabIndex = 5;
            btnClear.Text = "&Clear";
            // 
            // btnSearch
            // 
            btnSearch.Location = new System.Drawing.Point(423, 17);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(75, 25);
            btnSearch.TabIndex = 4;
            btnSearch.Text = "&Search";
            btnSearch.Click += btnSearch_Click;
            // 
            // lblNameFuzzySet
            // 
            lblNameFuzzySet.Location = new System.Drawing.Point(23, 21);
            lblNameFuzzySet.Name = "lblNameFuzzySet";
            lblNameFuzzySet.Size = new System.Drawing.Size(82, 16);
            lblNameFuzzySet.TabIndex = 0;
            lblNameFuzzySet.Text = "Search Name:";
            // 
            // pnlResult
            // 
            pnlResult.Controls.Add(pnlButton);
            pnlResult.Controls.Add(continuosFuzzySetInfo);
            pnlResult.Controls.Add(discreteFuzzySetInfo);
            pnlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlResult.Location = new System.Drawing.Point(0, 55);
            pnlResult.Name = "pnlResult";
            pnlResult.Size = new System.Drawing.Size(609, 380);
            pnlResult.TabIndex = 1;
            pnlResult.Visible = false;
            // 
            // pnlButton
            // 
            pnlButton.Controls.Add(btnUpdate);
            pnlButton.Controls.Add(btnView);
            pnlButton.Controls.Add(btnDelete);
            pnlButton.Controls.Add(btnCancel);
            pnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlButton.Location = new System.Drawing.Point(2, 333);
            pnlButton.Name = "pnlButton";
            pnlButton.Size = new System.Drawing.Size(605, 45);
            pnlButton.TabIndex = 10;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new System.Drawing.Point(281, 10);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new System.Drawing.Size(75, 25);
            btnUpdate.TabIndex = 5;
            btnUpdate.Text = "&Update";
            // 
            // btnView
            // 
            btnView.Location = new System.Drawing.Point(362, 10);
            btnView.Name = "btnView";
            btnView.Size = new System.Drawing.Size(75, 25);
            btnView.TabIndex = 6;
            btnView.Text = "&View";
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(443, 10);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(75, 25);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "&Delete";
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(524, 10);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 25);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "&Cancel";
            // 
            // continuosFuzzySetInfo
            // 
            continuosFuzzySetInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            continuosFuzzySetInfo.Location = new System.Drawing.Point(2, 2);
            continuosFuzzySetInfo.Name = "continuosFuzzySetInfo";
            continuosFuzzySetInfo.Size = new System.Drawing.Size(605, 376);
            continuosFuzzySetInfo.TabIndex = 9;
            continuosFuzzySetInfo.Visible = false;
            // 
            // discreteFuzzySetInfo
            // 
            discreteFuzzySetInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            discreteFuzzySetInfo.Location = new System.Drawing.Point(2, 2);
            discreteFuzzySetInfo.Name = "discreteFuzzySetInfo";
            discreteFuzzySetInfo.Size = new System.Drawing.Size(605, 376);
            discreteFuzzySetInfo.TabIndex = 1;
            discreteFuzzySetInfo.Visible = false;
            // 
            // frmSearchFuzzySet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(609, 435);
            Controls.Add(pnlResult);
            Controls.Add(pnlSearch);
            IconOptions.Image = (System.Drawing.Image)resources.GetObject("frmSearchFuzzySet.IconOptions.Image");
            Name = "frmSearchFuzzySet";
            Text = "Search FuzzySet";
            ((System.ComponentModel.ISupportInitialize)pnlSearch).EndInit();
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cboNameFuzzySet.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)pnlResult).EndInit();
            pnlResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pnlButton).EndInit();
            pnlButton.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlSearch;
        private DevExpress.XtraEditors.LabelControl lblNameFuzzySet;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraEditors.PanelControl pnlResult;
        private UserControls.DiscreteFuzzySet discreteFuzzySetInfo;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnView;
        private DevExpress.XtraEditors.SimpleButton btnUpdate;
        private UserControls.ContinuosFuzzySet continuosFuzzySetInfo;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraEditors.ComboBoxEdit cboNameFuzzySet;
        private DevExpress.XtraEditors.PanelControl pnlButton;
    }
}