namespace FPRDB_SQLite
{
    partial class FrmNewRelation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNewRelation));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.comboBox_Schema = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtRelName = new DevExpress.XtraEditors.TextEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBox_Schema.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRelName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnSave);
            this.panelControl1.Controls.Add(this.txtRelName);
            this.panelControl1.Controls.Add(this.comboBox_Schema);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(557, 236);
            this.panelControl1.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(132, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(295, 40);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Create New Relation";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(73, 77);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(88, 16);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Relation Name:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(71, 123);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(90, 16);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "Select Schema:";
            // 
            // comboBox_Schema
            // 
            this.comboBox_Schema.Location = new System.Drawing.Point(167, 120);
            this.comboBox_Schema.Name = "comboBox_Schema";
            this.comboBox_Schema.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBox_Schema.Size = new System.Drawing.Size(361, 22);
            this.comboBox_Schema.TabIndex = 3;
            // 
            // txtRelName
            // 
            this.txtRelName.Location = new System.Drawing.Point(167, 74);
            this.txtRelName.Name = "txtRelName";
            this.txtRelName.Size = new System.Drawing.Size(361, 22);
            this.txtRelName.TabIndex = 4;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(315, 185);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 29);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "&Save";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(434, 185);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 29);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            // 
            // FrmNewRelation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 236);
            this.Controls.Add(this.panelControl1);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmNewRelation.IconOptions.Image")));
            this.Name = "FrmNewRelation";
            this.Text = "Create New Relation";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBox_Schema.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRelName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBox_Schema;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.TextEdit txtRelName;
    }
}