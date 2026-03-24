namespace FPRDB_SQLite.GUI
{
    partial class frmFKRelationships
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFKRelationships));
            splitcontFKRelationships = new DevExpress.XtraEditors.SplitContainerControl();
            btnAdd = new DevExpress.XtraEditors.SimpleButton();
            btnDelete = new DevExpress.XtraEditors.SimpleButton();
            grpFKSelected = new DevExpress.XtraEditors.GroupControl();
            lstFKSelected = new DevExpress.XtraEditors.ListBoxControl();
            grpFKDetail = new DevExpress.XtraEditors.GroupControl();
            grdMappingAttr = new DevExpress.XtraGrid.GridControl();
            grdviewMappingAttr = new DevExpress.XtraGrid.Views.Grid.GridView();
            grdcolPKAttr = new DevExpress.XtraGrid.Columns.GridColumn();
            repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            grdcolFKAttr = new DevExpress.XtraGrid.Columns.GridColumn();
            repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            txtFKRelName = new DevExpress.XtraEditors.TextEdit();
            btnSave = new DevExpress.XtraEditors.SimpleButton();
            btnClose = new DevExpress.XtraEditors.SimpleButton();
            cboPKRelName = new DevExpress.XtraEditors.ComboBoxEdit();
            lblPKRelName = new DevExpress.XtraEditors.LabelControl();
            lblFKRelName = new DevExpress.XtraEditors.LabelControl();
            lblFKName = new DevExpress.XtraEditors.LabelControl();
            txtFKName = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)splitcontFKRelationships).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitcontFKRelationships.Panel1).BeginInit();
            splitcontFKRelationships.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitcontFKRelationships.Panel2).BeginInit();
            splitcontFKRelationships.Panel2.SuspendLayout();
            splitcontFKRelationships.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grpFKSelected).BeginInit();
            grpFKSelected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lstFKSelected).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grpFKDetail).BeginInit();
            grpFKDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdMappingAttr).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grdviewMappingAttr).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemLookUpEdit2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemLookUpEdit1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtFKRelName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cboPKRelName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtFKName.Properties).BeginInit();
            SuspendLayout();
            // 
            // splitcontFKRelationships
            // 
            splitcontFKRelationships.Dock = System.Windows.Forms.DockStyle.Fill;
            splitcontFKRelationships.Location = new System.Drawing.Point(0, 0);
            splitcontFKRelationships.Name = "splitcontFKRelationships";
            // 
            // splitcontFKRelationships.Panel1
            // 
            splitcontFKRelationships.Panel1.Controls.Add(btnAdd);
            splitcontFKRelationships.Panel1.Controls.Add(btnDelete);
            splitcontFKRelationships.Panel1.Controls.Add(grpFKSelected);
            splitcontFKRelationships.Panel1.Text = "Panel1";
            // 
            // splitcontFKRelationships.Panel2
            // 
            splitcontFKRelationships.Panel2.Controls.Add(grpFKDetail);
            splitcontFKRelationships.Panel2.Text = "Panel2";
            splitcontFKRelationships.Size = new System.Drawing.Size(653, 345);
            splitcontFKRelationships.SplitterPosition = 220;
            splitcontFKRelationships.TabIndex = 0;
            // 
            // btnAdd
            // 
            btnAdd.Location = new System.Drawing.Point(65, 308);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(75, 25);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "&Add";
            btnAdd.Click += btnAdd_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(143, 308);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(75, 25);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "&Delete";
            btnDelete.Click += btnDelete_Click;
            // 
            // grpFKSelected
            // 
            grpFKSelected.Controls.Add(lstFKSelected);
            grpFKSelected.Dock = System.Windows.Forms.DockStyle.Top;
            grpFKSelected.Location = new System.Drawing.Point(0, 0);
            grpFKSelected.Name = "grpFKSelected";
            grpFKSelected.Size = new System.Drawing.Size(220, 302);
            grpFKSelected.TabIndex = 0;
            grpFKSelected.Text = "Selected Foreign Key";
            // 
            // lstFKSelected
            // 
            lstFKSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            lstFKSelected.Location = new System.Drawing.Point(2, 28);
            lstFKSelected.Name = "lstFKSelected";
            lstFKSelected.Size = new System.Drawing.Size(216, 272);
            lstFKSelected.TabIndex = 0;
            lstFKSelected.SelectedIndexChanged += lstFKSelected_SelectedIndexChanged;
            // 
            // grpFKDetail
            // 
            grpFKDetail.Controls.Add(grdMappingAttr);
            grpFKDetail.Controls.Add(txtFKRelName);
            grpFKDetail.Controls.Add(btnSave);
            grpFKDetail.Controls.Add(btnClose);
            grpFKDetail.Controls.Add(cboPKRelName);
            grpFKDetail.Controls.Add(lblPKRelName);
            grpFKDetail.Controls.Add(lblFKRelName);
            grpFKDetail.Controls.Add(lblFKName);
            grpFKDetail.Controls.Add(txtFKName);
            grpFKDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            grpFKDetail.Location = new System.Drawing.Point(0, 0);
            grpFKDetail.Name = "grpFKDetail";
            grpFKDetail.Size = new System.Drawing.Size(421, 345);
            grpFKDetail.TabIndex = 0;
            grpFKDetail.Text = "Foreign Key Detail";
            grpFKDetail.Visible = false;
            // 
            // grdMappingAttr
            // 
            grdMappingAttr.Location = new System.Drawing.Point(5, 127);
            grdMappingAttr.MainView = grdviewMappingAttr;
            grdMappingAttr.Name = "grdMappingAttr";
            grdMappingAttr.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemLookUpEdit1, repositoryItemLookUpEdit2 });
            grdMappingAttr.Size = new System.Drawing.Size(396, 173);
            grdMappingAttr.TabIndex = 14;
            grdMappingAttr.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grdviewMappingAttr });
            // 
            // grdviewMappingAttr
            // 
            grdviewMappingAttr.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { grdcolPKAttr, grdcolFKAttr });
            grdviewMappingAttr.GridControl = grdMappingAttr;
            grdviewMappingAttr.Name = "grdviewMappingAttr";
            grdviewMappingAttr.OptionsView.ShowColumnHeaders = false;
            grdviewMappingAttr.OptionsView.ShowGroupPanel = false;
            grdviewMappingAttr.OptionsView.ShowIndicator = false;
            // 
            // grdcolPKAttr
            // 
            grdcolPKAttr.ColumnEdit = repositoryItemLookUpEdit2;
            grdcolPKAttr.FieldName = "PKAttr";
            grdcolPKAttr.MinWidth = 25;
            grdcolPKAttr.Name = "grdcolPKAttr";
            grdcolPKAttr.Visible = true;
            grdcolPKAttr.VisibleIndex = 0;
            grdcolPKAttr.Width = 94;
            // 
            // repositoryItemLookUpEdit2
            // 
            repositoryItemLookUpEdit2.AutoHeight = false;
            repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
            // 
            // grdcolFKAttr
            // 
            grdcolFKAttr.ColumnEdit = repositoryItemLookUpEdit1;
            grdcolFKAttr.FieldName = "FKAttr";
            grdcolFKAttr.MinWidth = 25;
            grdcolFKAttr.Name = "grdcolFKAttr";
            grdcolFKAttr.Visible = true;
            grdcolFKAttr.VisibleIndex = 1;
            grdcolFKAttr.Width = 94;
            // 
            // repositoryItemLookUpEdit1
            // 
            repositoryItemLookUpEdit1.AutoHeight = false;
            repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // txtFKRelName
            // 
            txtFKRelName.Location = new System.Drawing.Point(211, 99);
            txtFKRelName.Name = "txtFKRelName";
            txtFKRelName.Size = new System.Drawing.Size(190, 22);
            txtFKRelName.TabIndex = 13;
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(253, 308);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(75, 25);
            btnSave.TabIndex = 3;
            btnSave.Text = "&Save";
            btnSave.Click += btnSave_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new System.Drawing.Point(334, 308);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(75, 25);
            btnClose.TabIndex = 3;
            btnClose.Text = "&Close";
            btnClose.Click += btnClose_Click;
            // 
            // cboPKRelName
            // 
            cboPKRelName.Location = new System.Drawing.Point(5, 99);
            cboPKRelName.Name = "cboPKRelName";
            cboPKRelName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cboPKRelName.Size = new System.Drawing.Size(190, 22);
            cboPKRelName.TabIndex = 12;
            // 
            // lblPKRelName
            // 
            lblPKRelName.Location = new System.Drawing.Point(5, 77);
            lblPKRelName.Name = "lblPKRelName";
            lblPKRelName.Size = new System.Drawing.Size(118, 16);
            lblPKRelName.TabIndex = 10;
            lblPKRelName.Text = "Primary Key Relation";
            // 
            // lblFKRelName
            // 
            lblFKRelName.Location = new System.Drawing.Point(211, 77);
            lblFKRelName.Name = "lblFKRelName";
            lblFKRelName.Size = new System.Drawing.Size(117, 16);
            lblFKRelName.TabIndex = 9;
            lblFKRelName.Text = "Foreign Key Relation";
            // 
            // lblFKName
            // 
            lblFKName.Location = new System.Drawing.Point(5, 43);
            lblFKName.Name = "lblFKName";
            lblFKName.Size = new System.Drawing.Size(109, 16);
            lblFKName.TabIndex = 8;
            lblFKName.Text = "Foreign Key Name:";
            // 
            // txtFKName
            // 
            txtFKName.Location = new System.Drawing.Point(133, 40);
            txtFKName.Name = "txtFKName";
            txtFKName.Size = new System.Drawing.Size(268, 22);
            txtFKName.TabIndex = 7;
            // 
            // frmFKRelationships
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(653, 345);
            Controls.Add(splitcontFKRelationships);
            IconOptions.Image = (System.Drawing.Image)resources.GetObject("frmFKRelationships.IconOptions.Image");
            Name = "frmFKRelationships";
            Text = "Foreign Key Relationships";
            ((System.ComponentModel.ISupportInitialize)splitcontFKRelationships.Panel1).EndInit();
            splitcontFKRelationships.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitcontFKRelationships.Panel2).EndInit();
            splitcontFKRelationships.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitcontFKRelationships).EndInit();
            splitcontFKRelationships.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grpFKSelected).EndInit();
            grpFKSelected.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)lstFKSelected).EndInit();
            ((System.ComponentModel.ISupportInitialize)grpFKDetail).EndInit();
            grpFKDetail.ResumeLayout(false);
            grpFKDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)grdMappingAttr).EndInit();
            ((System.ComponentModel.ISupportInitialize)grdviewMappingAttr).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemLookUpEdit2).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemLookUpEdit1).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtFKRelName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cboPKRelName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtFKName.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitcontFKRelationships;
        private DevExpress.XtraEditors.GroupControl grpFKSelected;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.ListBoxControl lstFKSelected;
        private DevExpress.XtraEditors.GroupControl grpFKDetail;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.ComboBoxEdit cboPKRelName;
        private DevExpress.XtraEditors.ComboBoxEdit cboFKRelName;
        private DevExpress.XtraEditors.LabelControl lblPKRelName;
        private DevExpress.XtraEditors.LabelControl lblFKRelName;
        private DevExpress.XtraEditors.LabelControl lblFKName;
        private DevExpress.XtraEditors.TextEdit txtFKName;
        private DevExpress.XtraEditors.TextEdit txtFKRelName;
        private DevExpress.XtraGrid.GridControl grdMappingAttr;
        private DevExpress.XtraGrid.Views.Grid.GridView grdviewMappingAttr;
        private DevExpress.XtraGrid.Columns.GridColumn grdcolPKAttr;
        private DevExpress.XtraGrid.Columns.GridColumn grdcolFKAttr;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
    }
}