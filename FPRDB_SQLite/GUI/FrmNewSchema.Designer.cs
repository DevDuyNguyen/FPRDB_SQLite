namespace FPRDB_SQLite
{
    partial class FrmNewSchema
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNewSchema));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl_NameSchema = new DevExpress.XtraEditors.GroupControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtSchemaName = new DevExpress.XtraEditors.TextEdit();
            this.panelControl_BtnNewSchema = new DevExpress.XtraEditors.PanelControl();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl_SchemaAttr = new DevExpress.XtraEditors.GroupControl();
            this.gridControl_NewSchema = new DevExpress.XtraGrid.GridControl();
            this.gridView_NewSchema = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_PK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Attr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_DataType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Desc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_Domain = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl_NameSchema)).BeginInit();
            this.groupControl_NameSchema.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSchemaName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_BtnNewSchema)).BeginInit();
            this.panelControl_BtnNewSchema.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl_SchemaAttr)).BeginInit();
            this.groupControl_SchemaAttr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_NewSchema)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_NewSchema)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControl_SchemaAttr);
            this.panelControl1.Controls.Add(this.panelControl_BtnNewSchema);
            this.panelControl1.Controls.Add(this.groupControl_NameSchema);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(738, 523);
            this.panelControl1.TabIndex = 0;
            // 
            // groupControl_NameSchema
            // 
            this.groupControl_NameSchema.Controls.Add(this.txtSchemaName);
            this.groupControl_NameSchema.Controls.Add(this.labelControl1);
            this.groupControl_NameSchema.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl_NameSchema.Location = new System.Drawing.Point(2, 2);
            this.groupControl_NameSchema.Name = "groupControl_NameSchema";
            this.groupControl_NameSchema.Size = new System.Drawing.Size(734, 100);
            this.groupControl_NameSchema.TabIndex = 0;
            this.groupControl_NameSchema.Text = "Table Name";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(58, 54);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(74, 16);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Table Name:";
            // 
            // txtSchemaName
            // 
            this.txtSchemaName.Location = new System.Drawing.Point(174, 51);
            this.txtSchemaName.Name = "txtSchemaName";
            this.txtSchemaName.Size = new System.Drawing.Size(395, 22);
            this.txtSchemaName.TabIndex = 1;
            // 
            // panelControl_BtnNewSchema
            // 
            this.panelControl_BtnNewSchema.Controls.Add(this.btnCancel);
            this.panelControl_BtnNewSchema.Controls.Add(this.btnSave);
            this.panelControl_BtnNewSchema.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl_BtnNewSchema.Location = new System.Drawing.Point(2, 466);
            this.panelControl_BtnNewSchema.Name = "panelControl_BtnNewSchema";
            this.panelControl_BtnNewSchema.Size = new System.Drawing.Size(734, 55);
            this.panelControl_BtnNewSchema.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(475, 16);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 29);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(610, 16);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 29);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            // 
            // groupControl_SchemaAttr
            // 
            this.groupControl_SchemaAttr.Controls.Add(this.gridControl_NewSchema);
            this.groupControl_SchemaAttr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl_SchemaAttr.Location = new System.Drawing.Point(2, 102);
            this.groupControl_SchemaAttr.Name = "groupControl_SchemaAttr";
            this.groupControl_SchemaAttr.Size = new System.Drawing.Size(734, 364);
            this.groupControl_SchemaAttr.TabIndex = 2;
            this.groupControl_SchemaAttr.Text = "Table Attribute";
            // 
            // gridControl_NewSchema
            // 
            this.gridControl_NewSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_NewSchema.Location = new System.Drawing.Point(2, 28);
            this.gridControl_NewSchema.MainView = this.gridView_NewSchema;
            this.gridControl_NewSchema.Name = "gridControl_NewSchema";
            this.gridControl_NewSchema.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gridControl_NewSchema.Size = new System.Drawing.Size(730, 334);
            this.gridControl_NewSchema.TabIndex = 0;
            this.gridControl_NewSchema.UseEmbeddedNavigator = true;
            this.gridControl_NewSchema.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_NewSchema});
            // 
            // gridView_NewSchema
            // 
            this.gridView_NewSchema.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_PK,
            this.gridColumn_Attr,
            this.gridColumn_DataType,
            this.gridColumn_Desc,
            this.gridColumn_Domain});
            this.gridView_NewSchema.GridControl = this.gridControl_NewSchema;
            this.gridView_NewSchema.Name = "gridView_NewSchema";
            this.gridView_NewSchema.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn_PK
            // 
            this.gridColumn_PK.Caption = "Primary Key";
            this.gridColumn_PK.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn_PK.MinWidth = 25;
            this.gridColumn_PK.Name = "gridColumn_PK";
            this.gridColumn_PK.Visible = true;
            this.gridColumn_PK.VisibleIndex = 0;
            this.gridColumn_PK.Width = 94;
            // 
            // gridColumn_Attr
            // 
            this.gridColumn_Attr.Caption = "Attribute Name";
            this.gridColumn_Attr.MinWidth = 25;
            this.gridColumn_Attr.Name = "gridColumn_Attr";
            this.gridColumn_Attr.Visible = true;
            this.gridColumn_Attr.VisibleIndex = 1;
            this.gridColumn_Attr.Width = 94;
            // 
            // gridColumn_DataType
            // 
            this.gridColumn_DataType.Caption = "Data Type";
            this.gridColumn_DataType.MinWidth = 25;
            this.gridColumn_DataType.Name = "gridColumn_DataType";
            this.gridColumn_DataType.Visible = true;
            this.gridColumn_DataType.VisibleIndex = 2;
            this.gridColumn_DataType.Width = 94;
            // 
            // gridColumn_Desc
            // 
            this.gridColumn_Desc.Caption = "Description";
            this.gridColumn_Desc.MinWidth = 25;
            this.gridColumn_Desc.Name = "gridColumn_Desc";
            this.gridColumn_Desc.Visible = true;
            this.gridColumn_Desc.VisibleIndex = 3;
            this.gridColumn_Desc.Width = 94;
            // 
            // gridColumn_Domain
            // 
            this.gridColumn_Domain.Caption = "Domain";
            this.gridColumn_Domain.MinWidth = 25;
            this.gridColumn_Domain.Name = "gridColumn_Domain";
            this.gridColumn_Domain.Visible = true;
            this.gridColumn_Domain.VisibleIndex = 4;
            this.gridColumn_Domain.Width = 94;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // FrmNewSchema
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 523);
            this.Controls.Add(this.panelControl1);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmNewSchema.IconOptions.Image")));
            this.Name = "FrmNewSchema";
            this.Text = "New Table";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl_NameSchema)).EndInit();
            this.groupControl_NameSchema.ResumeLayout(false);
            this.groupControl_NameSchema.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSchemaName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_BtnNewSchema)).EndInit();
            this.panelControl_BtnNewSchema.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl_SchemaAttr)).EndInit();
            this.groupControl_SchemaAttr.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_NewSchema)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_NewSchema)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl_SchemaAttr;
        private DevExpress.XtraGrid.GridControl gridControl_NewSchema;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_NewSchema;
        private DevExpress.XtraEditors.PanelControl panelControl_BtnNewSchema;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.GroupControl groupControl_NameSchema;
        private DevExpress.XtraEditors.TextEdit txtSchemaName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_PK;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Attr;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_DataType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Desc;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_Domain;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
    }
}