using DevExpress.XtraGrid.Views.Grid;

namespace FPRDB_SQLite.GUI.UserControls
{
    partial class DiscreteFuzzySet
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
            components = new System.ComponentModel.Container();
            grdcDiscFuzzy = new DevExpress.XtraGrid.GridControl();
            grdvDiscFuzzy = new GridView();
            repositoryItemValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            repositoryItemMembership = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            txtNameDiscFuzzy = new DevExpress.XtraEditors.TextEdit();
            lblNameDiscFuzzy = new DevExpress.XtraEditors.LabelControl();
            lblDataType = new DevExpress.XtraEditors.LabelControl();
            cboDataType = new DevExpress.XtraEditors.ComboBoxEdit();
            dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(components);
            ((System.ComponentModel.ISupportInitialize)grdcDiscFuzzy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grdvDiscFuzzy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemValue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemMembership).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtNameDiscFuzzy.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cboDataType.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dxValidationProvider1).BeginInit();
            SuspendLayout();
            // 
            // grdcDiscFuzzy
            // 
            grdcDiscFuzzy.Enabled = false;
            grdcDiscFuzzy.Location = new System.Drawing.Point(25, 72);
            grdcDiscFuzzy.MainView = grdvDiscFuzzy;
            grdcDiscFuzzy.Name = "grdcDiscFuzzy";
            grdcDiscFuzzy.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemValue, repositoryItemMembership });
            grdcDiscFuzzy.Size = new System.Drawing.Size(500, 250);
            grdcDiscFuzzy.TabIndex = 5;
            grdcDiscFuzzy.UseEmbeddedNavigator = true;
            grdcDiscFuzzy.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grdvDiscFuzzy });
            // 
            // grdvDiscFuzzy
            // 
            grdvDiscFuzzy.GridControl = grdcDiscFuzzy;
            grdvDiscFuzzy.Name = "grdvDiscFuzzy";
            grdvDiscFuzzy.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            grdvDiscFuzzy.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom;
            grdvDiscFuzzy.OptionsView.ShowGroupPanel = false;
            grdvDiscFuzzy.FocusedRowChanged += grdvDiscFuzzy_FocusedRowChanged;
            grdvDiscFuzzy.FocusedColumnChanged += grdvDiscFuzzy_FocusedColumnChanged;
            grdvDiscFuzzy.ValidatingEditor += grdvDiscFuzzy_ValidatingEditor;
            // 
            // repositoryItemValue
            // 
            repositoryItemValue.AutoHeight = false;
            repositoryItemValue.Name = "repositoryItemValue";
            // 
            // repositoryItemMembership
            // 
            repositoryItemMembership.AutoHeight = false;
            repositoryItemMembership.Name = "repositoryItemMembership";
            // 
            // txtNameDiscFuzzy
            // 
            txtNameDiscFuzzy.Location = new System.Drawing.Point(122, 3);
            txtNameDiscFuzzy.Name = "txtNameDiscFuzzy";
            txtNameDiscFuzzy.Size = new System.Drawing.Size(295, 22);
            txtNameDiscFuzzy.TabIndex = 4;
            // 
            // lblNameDiscFuzzy
            // 
            lblNameDiscFuzzy.Location = new System.Drawing.Point(25, 6);
            lblNameDiscFuzzy.Name = "lblNameDiscFuzzy";
            lblNameDiscFuzzy.Size = new System.Drawing.Size(97, 16);
            lblNameDiscFuzzy.TabIndex = 3;
            lblNameDiscFuzzy.Text = "Fuzzy Set Name:";
            // 
            // lblDataType
            // 
            lblDataType.Location = new System.Drawing.Point(25, 41);
            lblDataType.Name = "lblDataType";
            lblDataType.Size = new System.Drawing.Size(63, 16);
            lblDataType.TabIndex = 6;
            lblDataType.Text = "Data Type:";
            // 
            // cboDataType
            // 
            cboDataType.Location = new System.Drawing.Point(122, 38);
            cboDataType.Name = "cboDataType";
            cboDataType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cboDataType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cboDataType.Size = new System.Drawing.Size(171, 22);
            cboDataType.TabIndex = 7;
            // 
            // DiscreteFuzzySet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(cboDataType);
            Controls.Add(lblDataType);
            Controls.Add(grdcDiscFuzzy);
            Controls.Add(txtNameDiscFuzzy);
            Controls.Add(lblNameDiscFuzzy);
            Name = "DiscreteFuzzySet";
            Size = new System.Drawing.Size(549, 342);
            ((System.ComponentModel.ISupportInitialize)grdcDiscFuzzy).EndInit();
            ((System.ComponentModel.ISupportInitialize)grdvDiscFuzzy).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemValue).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemMembership).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtNameDiscFuzzy.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cboDataType.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)dxValidationProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraGrid.GridControl grdcDiscFuzzy;
        private DevExpress.XtraGrid.Views.Grid.GridView grdvDiscFuzzy;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemMembership;
        private DevExpress.XtraEditors.TextEdit txtNameDiscFuzzy;
        private DevExpress.XtraEditors.LabelControl lblNameDiscFuzzy;
        private DevExpress.XtraEditors.LabelControl lblDataType;
        private DevExpress.XtraEditors.ComboBoxEdit cboDataType;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
    }
}
