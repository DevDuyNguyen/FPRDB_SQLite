namespace FPRDB_SQLite.GUI
{
    partial class frmNewSchema
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNewSchema));
            pnlNewSchema = new DevExpress.XtraEditors.PanelControl();
            grpSchemaAttribute = new DevExpress.XtraEditors.GroupControl();
            grdcSchemaAttribute = new DevExpress.XtraGrid.GridControl();
            grdvSchemaAttribute = new DevExpress.XtraGrid.Views.Grid.GridView();
            grdcolPK = new DevExpress.XtraGrid.Columns.GridColumn();
            repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            grdcolAttr = new DevExpress.XtraGrid.Columns.GridColumn();
            grdcolDataType = new DevExpress.XtraGrid.Columns.GridColumn();
            pnlButtonNewSchema = new DevExpress.XtraEditors.PanelControl();
            btnCancel = new DevExpress.XtraEditors.SimpleButton();
            btnSave = new DevExpress.XtraEditors.SimpleButton();
            grpSchemaName = new DevExpress.XtraEditors.GroupControl();
            txtSchemaName = new DevExpress.XtraEditors.TextEdit();
            lblSchemaName = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)pnlNewSchema).BeginInit();
            pnlNewSchema.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grpSchemaAttribute).BeginInit();
            grpSchemaAttribute.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdcSchemaAttribute).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grdvSchemaAttribute).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemCheckEdit1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pnlButtonNewSchema).BeginInit();
            pnlButtonNewSchema.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grpSchemaName).BeginInit();
            grpSchemaName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtSchemaName.Properties).BeginInit();
            SuspendLayout();
            // 
            // pnlNewSchema
            // 
            pnlNewSchema.Controls.Add(grpSchemaAttribute);
            pnlNewSchema.Controls.Add(pnlButtonNewSchema);
            pnlNewSchema.Controls.Add(grpSchemaName);
            pnlNewSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlNewSchema.Location = new System.Drawing.Point(0, 0);
            pnlNewSchema.Name = "pnlNewSchema";
            pnlNewSchema.Size = new System.Drawing.Size(738, 523);
            pnlNewSchema.TabIndex = 0;
            // 
            // grpSchemaAttribute
            // 
            grpSchemaAttribute.Controls.Add(grdcSchemaAttribute);
            grpSchemaAttribute.Dock = System.Windows.Forms.DockStyle.Fill;
            grpSchemaAttribute.Location = new System.Drawing.Point(2, 102);
            grpSchemaAttribute.Name = "grpSchemaAttribute";
            grpSchemaAttribute.Size = new System.Drawing.Size(734, 364);
            grpSchemaAttribute.TabIndex = 2;
            grpSchemaAttribute.Text = "Schema Attribute";
            // 
            // grdcSchemaAttribute
            // 
            grdcSchemaAttribute.Dock = System.Windows.Forms.DockStyle.Fill;
            grdcSchemaAttribute.Location = new System.Drawing.Point(2, 28);
            grdcSchemaAttribute.MainView = grdvSchemaAttribute;
            grdcSchemaAttribute.Name = "grdcSchemaAttribute";
            grdcSchemaAttribute.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemCheckEdit1 });
            grdcSchemaAttribute.Size = new System.Drawing.Size(730, 334);
            grdcSchemaAttribute.TabIndex = 0;
            grdcSchemaAttribute.UseEmbeddedNavigator = true;
            grdcSchemaAttribute.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grdvSchemaAttribute });
            // 
            // grdvSchemaAttribute
            // 
            grdvSchemaAttribute.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { grdcolPK, grdcolAttr, grdcolDataType });
            grdvSchemaAttribute.GridControl = grdcSchemaAttribute;
            grdvSchemaAttribute.Name = "grdvSchemaAttribute";
            grdvSchemaAttribute.OptionsView.ShowGroupPanel = false;
            // 
            // grdcolPK
            // 
            grdcolPK.Caption = "Primary Key";
            grdcolPK.ColumnEdit = repositoryItemCheckEdit1;
            grdcolPK.MinWidth = 25;
            grdcolPK.Name = "grdcolPK";
            grdcolPK.Visible = true;
            grdcolPK.VisibleIndex = 0;
            grdcolPK.Width = 94;
            // 
            // repositoryItemCheckEdit1
            // 
            repositoryItemCheckEdit1.AutoHeight = false;
            repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // grdcolAttr
            // 
            grdcolAttr.Caption = "Attribute Name";
            grdcolAttr.MinWidth = 25;
            grdcolAttr.Name = "grdcolAttr";
            grdcolAttr.Visible = true;
            grdcolAttr.VisibleIndex = 1;
            grdcolAttr.Width = 94;
            // 
            // grdcolDataType
            // 
            grdcolDataType.Caption = "Data Type";
            grdcolDataType.MinWidth = 25;
            grdcolDataType.Name = "grdcolDataType";
            grdcolDataType.Visible = true;
            grdcolDataType.VisibleIndex = 2;
            grdcolDataType.Width = 94;
            // 
            // pnlButtonNewSchema
            // 
            pnlButtonNewSchema.Controls.Add(btnCancel);
            pnlButtonNewSchema.Controls.Add(btnSave);
            pnlButtonNewSchema.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlButtonNewSchema.Location = new System.Drawing.Point(2, 466);
            pnlButtonNewSchema.Name = "pnlButtonNewSchema";
            pnlButtonNewSchema.Size = new System.Drawing.Size(734, 55);
            pnlButtonNewSchema.TabIndex = 1;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(610, 16);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(94, 29);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "&Cancel";
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(475, 16);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(94, 29);
            btnSave.TabIndex = 0;
            btnSave.Text = "&Save";
            // 
            // grpSchemaName
            // 
            grpSchemaName.Controls.Add(txtSchemaName);
            grpSchemaName.Controls.Add(lblSchemaName);
            grpSchemaName.Dock = System.Windows.Forms.DockStyle.Top;
            grpSchemaName.Location = new System.Drawing.Point(2, 2);
            grpSchemaName.Name = "grpSchemaName";
            grpSchemaName.Size = new System.Drawing.Size(734, 100);
            grpSchemaName.TabIndex = 0;
            grpSchemaName.Text = "Schema Name";
            // 
            // txtSchemaName
            // 
            txtSchemaName.Location = new System.Drawing.Point(174, 51);
            txtSchemaName.Name = "txtSchemaName";
            txtSchemaName.Size = new System.Drawing.Size(395, 22);
            txtSchemaName.TabIndex = 1;
            // 
            // lblSchemaName
            // 
            lblSchemaName.Location = new System.Drawing.Point(58, 54);
            lblSchemaName.Name = "lblSchemaName";
            lblSchemaName.Size = new System.Drawing.Size(88, 16);
            lblSchemaName.TabIndex = 0;
            lblSchemaName.Text = "Schema Name:";
            // 
            // frmNewSchema
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(738, 523);
            Controls.Add(pnlNewSchema);
            IconOptions.Image = (System.Drawing.Image)resources.GetObject("frmNewSchema.IconOptions.Image");
            Name = "frmNewSchema";
            Text = "New Schema";
            ((System.ComponentModel.ISupportInitialize)pnlNewSchema).EndInit();
            pnlNewSchema.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grpSchemaAttribute).EndInit();
            grpSchemaAttribute.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grdcSchemaAttribute).EndInit();
            ((System.ComponentModel.ISupportInitialize)grdvSchemaAttribute).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemCheckEdit1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pnlButtonNewSchema).EndInit();
            pnlButtonNewSchema.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grpSchemaName).EndInit();
            grpSchemaName.ResumeLayout(false);
            grpSchemaName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtSchemaName.Properties).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlNewSchema;
        private DevExpress.XtraEditors.GroupControl grpSchemaAttribute;
        private DevExpress.XtraGrid.GridControl grdcSchemaAttribute;
        private DevExpress.XtraGrid.Views.Grid.GridView grdvSchemaAttribute;
        private DevExpress.XtraEditors.PanelControl pnlButtonNewSchema;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.GroupControl grpSchemaName;
        private DevExpress.XtraEditors.TextEdit txtSchemaName;
        private DevExpress.XtraEditors.LabelControl lblSchemaName;
        private DevExpress.XtraGrid.Columns.GridColumn grdcolPK;
        private DevExpress.XtraGrid.Columns.GridColumn grdcolAttr;
        private DevExpress.XtraGrid.Columns.GridColumn grdcolDataType;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
    }
}