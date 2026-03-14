namespace FPRDB_SQLite.GUI
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
            cboSchemaName = new DevExpress.XtraEditors.ComboBoxEdit();
            txtRelName = new DevExpress.XtraEditors.TextEdit();
            lblSchemaName = new DevExpress.XtraEditors.LabelControl();
            lblRelName = new DevExpress.XtraEditors.LabelControl();
            lblHeading = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)pnlNewRelation).BeginInit();
            pnlNewRelation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cboSchemaName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtRelName.Properties).BeginInit();
            SuspendLayout();
            // 
            // pnlNewRelation
            // 
            pnlNewRelation.Controls.Add(btnCancel);
            pnlNewRelation.Controls.Add(btnSave);
            pnlNewRelation.Controls.Add(cboSchemaName);
            pnlNewRelation.Controls.Add(txtRelName);
            pnlNewRelation.Controls.Add(lblSchemaName);
            pnlNewRelation.Controls.Add(lblRelName);
            pnlNewRelation.Controls.Add(lblHeading);
            pnlNewRelation.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlNewRelation.Location = new System.Drawing.Point(0, 0);
            pnlNewRelation.Name = "pnlNewRelation";
            pnlNewRelation.Size = new System.Drawing.Size(542, 176);
            pnlNewRelation.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(455, 138);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 25);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "&Cancel";
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(374, 138);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(75, 25);
            btnSave.TabIndex = 5;
            btnSave.Text = "&Save";
            btnSave.Click += btnSave_Click;
            // 
            // cboSchemaName
            // 
            cboSchemaName.Location = new System.Drawing.Point(135, 107);
            cboSchemaName.Name = "cboSchemaName";
            cboSchemaName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cboSchemaName.Size = new System.Drawing.Size(356, 22);
            cboSchemaName.TabIndex = 4;
            cboSchemaName.SelectedIndexChanged += cboSchemaName_SelectedIndexChanged;
            // 
            // txtRelName
            // 
            txtRelName.Location = new System.Drawing.Point(135, 52);
            txtRelName.Name = "txtRelName";
            txtRelName.Size = new System.Drawing.Size(356, 22);
            txtRelName.TabIndex = 3;
            // 
            // lblSchemaName
            // 
            lblSchemaName.Location = new System.Drawing.Point(32, 110);
            lblSchemaName.Name = "lblSchemaName";
            lblSchemaName.Size = new System.Drawing.Size(97, 16);
            lblSchemaName.TabIndex = 2;
            lblSchemaName.Text = "Choose Schema:";
            // 
            // lblRelName
            // 
            lblRelName.Location = new System.Drawing.Point(32, 55);
            lblRelName.Name = "lblRelName";
            lblRelName.Size = new System.Drawing.Size(88, 16);
            lblRelName.TabIndex = 1;
            lblRelName.Text = "Relation Name:";
            // 
            // lblHeading
            // 
            lblHeading.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
            lblHeading.Appearance.Options.UseFont = true;
            lblHeading.Location = new System.Drawing.Point(135, 5);
            lblHeading.Name = "lblHeading";
            lblHeading.Size = new System.Drawing.Size(271, 36);
            lblHeading.TabIndex = 0;
            lblHeading.Text = "Create New Relation";
            // 
            // frmNewRelation
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(542, 176);
            Controls.Add(pnlNewRelation);
            IconOptions.Image = (System.Drawing.Image)resources.GetObject("frmNewRelation.IconOptions.Image");
            Name = "frmNewRelation";
            Text = "Create New Relation";
            ((System.ComponentModel.ISupportInitialize)pnlNewRelation).EndInit();
            pnlNewRelation.ResumeLayout(false);
            pnlNewRelation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cboSchemaName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtRelName.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlNewRelation;
        private DevExpress.XtraEditors.LabelControl lblSchemaName;
        private DevExpress.XtraEditors.LabelControl lblRelName;
        private DevExpress.XtraEditors.LabelControl lblHeading;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.ComboBoxEdit cboSchemaName;
        private DevExpress.XtraEditors.TextEdit txtRelName;
    }
}