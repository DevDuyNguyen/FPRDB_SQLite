namespace FPRDB_SQLite
{
    partial class FrmNewQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNewQuery));
            this.panelControl_NewQuery = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtQueryName = new DevExpress.XtraEditors.TextEdit();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_NewQuery)).BeginInit();
            this.panelControl_NewQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl_NewQuery
            // 
            this.panelControl_NewQuery.Controls.Add(this.btnCancel);
            this.panelControl_NewQuery.Controls.Add(this.btnOk);
            this.panelControl_NewQuery.Controls.Add(this.txtQueryName);
            this.panelControl_NewQuery.Controls.Add(this.labelControl1);
            this.panelControl_NewQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl_NewQuery.Location = new System.Drawing.Point(0, 0);
            this.panelControl_NewQuery.Name = "panelControl_NewQuery";
            this.panelControl_NewQuery.Size = new System.Drawing.Size(559, 126);
            this.panelControl_NewQuery.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(41, 29);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(107, 16);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Enter query name:";
            // 
            // txtQueryName
            // 
            this.txtQueryName.Location = new System.Drawing.Point(154, 26);
            this.txtQueryName.Name = "txtQueryName";
            this.txtQueryName.Size = new System.Drawing.Size(371, 22);
            this.txtQueryName.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(309, 81);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(94, 29);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&OK";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(431, 81);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 29);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            // 
            // FrmNewQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 126);
            this.Controls.Add(this.panelControl_NewQuery);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmNewQuery.IconOptions.Image")));
            this.Name = "FrmNewQuery";
            this.Text = "New Query";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_NewQuery)).EndInit();
            this.panelControl_NewQuery.ResumeLayout(false);
            this.panelControl_NewQuery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQueryName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl_NewQuery;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.TextEdit txtQueryName;
    }
}