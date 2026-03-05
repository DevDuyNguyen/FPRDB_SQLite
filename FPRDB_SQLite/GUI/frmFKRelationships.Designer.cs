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
            btnSave = new DevExpress.XtraEditors.SimpleButton();
            btnClose = new DevExpress.XtraEditors.SimpleButton();
            cboPKRelName = new DevExpress.XtraEditors.ComboBoxEdit();
            lblPKRelName = new DevExpress.XtraEditors.LabelControl();
            lblFKRelName = new DevExpress.XtraEditors.LabelControl();
            lblFKName = new DevExpress.XtraEditors.LabelControl();
            txtFKName = new DevExpress.XtraEditors.TextEdit();
            txtFKRelName = new DevExpress.XtraEditors.TextEdit();
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
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
            ((System.ComponentModel.ISupportInitialize)this.cboPKRelName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtFKName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtFKRelName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemLookUpEdit1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemLookUpEdit2).BeginInit();
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
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(143, 308);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(75, 25);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "&Delete";
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
            // 
            // grpFKDetail
            // 
            grpFKDetail.Controls.Add(gridControl1);
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
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(253, 308);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(75, 25);
            btnSave.TabIndex = 3;
            btnSave.Text = "&Save";
            // 
            // btnClose
            // 
            btnClose.Location = new System.Drawing.Point(334, 308);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(75, 25);
            btnClose.TabIndex = 3;
            btnClose.Text = "&Close";
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
            // txtFKRelName
            // 
            txtFKRelName.Location = new System.Drawing.Point(211, 99);
            txtFKRelName.Name = "txtFKRelName";
            txtFKRelName.Size = new System.Drawing.Size(190, 22);
            txtFKRelName.TabIndex = 13;
            // 
            // gridControl1
            // 
            gridControl1.Location = new System.Drawing.Point(5, 127);
            gridControl1.MainView = gridView1;
            gridControl1.Name = "gridControl1";
            gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemLookUpEdit1, repositoryItemLookUpEdit2 });
            gridControl1.Size = new System.Drawing.Size(396, 175);
            gridControl1.TabIndex = 14;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView1 });
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { gridColumn1, gridColumn2 });
            gridView1.GridControl = gridControl1;
            gridView1.Name = "gridView1";
            gridView1.OptionsView.ShowColumnHeaders = false;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = false;
            // 
            // gridColumn1
            // 
            gridColumn1.ColumnEdit = repositoryItemLookUpEdit2;
            gridColumn1.MinWidth = 25;
            gridColumn1.Name = "gridColumn1";
            gridColumn1.Visible = true;
            gridColumn1.VisibleIndex = 0;
            gridColumn1.Width = 94;
            // 
            // gridColumn2
            // 
            gridColumn2.ColumnEdit = repositoryItemLookUpEdit1;
            gridColumn2.MinWidth = 25;
            gridColumn2.Name = "gridColumn2";
            gridColumn2.Visible = true;
            gridColumn2.VisibleIndex = 1;
            gridColumn2.Width = 94;
            // 
            // repositoryItemLookUpEdit1
            // 
            repositoryItemLookUpEdit1.AutoHeight = false;
            repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // repositoryItemLookUpEdit2
            // 
            repositoryItemLookUpEdit2.AutoHeight = false;
            repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
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
            ((System.ComponentModel.ISupportInitialize)this.cboPKRelName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtFKName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtFKRelName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemLookUpEdit1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemLookUpEdit2).EndInit();
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
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
    }
}