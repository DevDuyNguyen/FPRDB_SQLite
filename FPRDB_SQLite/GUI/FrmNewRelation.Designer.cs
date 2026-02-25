namespace FPRDB_SQLite
{
    partial class frmNewRelation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNewRelation));
            pnlNewRelation = new DevExpress.XtraEditors.PanelControl();
            btnCancel = new DevExpress.XtraEditors.SimpleButton();
            btnSave = new DevExpress.XtraEditors.SimpleButton();
            txtRelName = new DevExpress.XtraEditors.TextEdit();
            cboSchemaName = new DevExpress.XtraEditors.ComboBoxEdit();
            lblSchemaName = new DevExpress.XtraEditors.LabelControl();
            lblRelName = new DevExpress.XtraEditors.LabelControl();
            lblHeading = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)pnlNewRelation).BeginInit();
            pnlNewRelation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtRelName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cboSchemaName.Properties).BeginInit();
            SuspendLayout();
            // 
            // pnlNewRelation
            // 
            pnlNewRelation.Controls.Add(btnCancel);
            pnlNewRelation.Controls.Add(btnSave);
            pnlNewRelation.Controls.Add(txtRelName);
            pnlNewRelation.Controls.Add(cboSchemaName);
            pnlNewRelation.Controls.Add(lblSchemaName);
            pnlNewRelation.Controls.Add(lblRelName);
            pnlNewRelation.Controls.Add(lblHeading);
            pnlNewRelation.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlNewRelation.Location = new System.Drawing.Point(0, 0);
            pnlNewRelation.Name = "pnlNewRelation";
            pnlNewRelation.Size = new System.Drawing.Size(557, 236);
            pnlNewRelation.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(434, 185);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(94, 29);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "&Cancel";
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(315, 185);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(94, 29);
            btnSave.TabIndex = 5;
            btnSave.Text = "&Save";
            // 
            // txtRelName
            // 
            txtRelName.Location = new System.Drawing.Point(167, 74);
            txtRelName.Name = "txtRelName";
            txtRelName.Size = new System.Drawing.Size(361, 22);
            txtRelName.TabIndex = 4;
            // 
            // cboSchemaName
            // 
            cboSchemaName.Location = new System.Drawing.Point(167, 120);
            cboSchemaName.Name = "cboSchemaName";
            cboSchemaName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cboSchemaName.Size = new System.Drawing.Size(361, 22);
            cboSchemaName.TabIndex = 3;
            // 
            // lblSchemaName
            // 
            lblSchemaName.Location = new System.Drawing.Point(71, 123);
            lblSchemaName.Name = "lblSchemaName";
            lblSchemaName.Size = new System.Drawing.Size(90, 16);
            lblSchemaName.TabIndex = 2;
            lblSchemaName.Text = "Select Schema:";
            // 
            // lblRelName
            // 
            lblRelName.Location = new System.Drawing.Point(73, 77);
            lblRelName.Name = "lblRelName";
            lblRelName.Size = new System.Drawing.Size(88, 16);
            lblRelName.TabIndex = 1;
            lblRelName.Text = "Relation Name:";
            // 
            // lblHeading
            // 
            lblHeading.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            lblHeading.Appearance.Options.UseFont = true;
            lblHeading.Location = new System.Drawing.Point(132, 12);
            lblHeading.Name = "lblHeading";
            lblHeading.Size = new System.Drawing.Size(295, 40);
            lblHeading.TabIndex = 0;
            lblHeading.Text = "Create New Relation";
            // 
            // frmNewRelation
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(557, 236);
            Controls.Add(pnlNewRelation);
            IconOptions.Image = (System.Drawing.Image)resources.GetObject("frmNewRelation.IconOptions.Image");
            Name = "frmNewRelation";
            Text = "Create New Relation";
            ((System.ComponentModel.ISupportInitialize)pnlNewRelation).EndInit();
            pnlNewRelation.ResumeLayout(false);
            pnlNewRelation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtRelName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cboSchemaName.Properties).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlNewRelation;
        private DevExpress.XtraEditors.ComboBoxEdit cboSchemaName;
        private DevExpress.XtraEditors.LabelControl lblSchemaName;
        private DevExpress.XtraEditors.LabelControl lblRelName;
        private DevExpress.XtraEditors.LabelControl lblHeading;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.TextEdit txtRelName;
    }
}