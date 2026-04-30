using System.Drawing;
using System.Windows.Forms;
namespace FPRDB_SQLite.GUI.GUI
{
    partial class frmNewQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNewQuery));
            lblQueryName = new DevExpress.XtraEditors.LabelControl();
            txtQueryName = new DevExpress.XtraEditors.TextEdit();
            btnOK = new DevExpress.XtraEditors.SimpleButton();
            btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)txtQueryName.Properties).BeginInit();
            SuspendLayout();
            // 
            // lblQueryName
            // 
            lblQueryName.Location = new Point(30, 24);
            lblQueryName.Name = "lblQueryName";
            lblQueryName.Size = new Size(110, 16);
            lblQueryName.TabIndex = 0;
            lblQueryName.Text = "Enter Query Name:";
            // 
            // txtQueryName
            // 
            txtQueryName.Location = new Point(146, 21);
            txtQueryName.Name = "txtQueryName";
            txtQueryName.Size = new Size(323, 22);
            txtQueryName.TabIndex = 1;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(313, 61);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 25);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(394, 61);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 25);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // frmNewQuery
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(497, 100);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(txtQueryName);
            Controls.Add(lblQueryName);
            IconOptions.Image = (Image)resources.GetObject("frmNewQuery.IconOptions.Image");
            Name = "frmNewQuery";
            Text = "New Query";
            ((System.ComponentModel.ISupportInitialize)txtQueryName.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblQueryName;
        private DevExpress.XtraEditors.TextEdit txtQueryName;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}