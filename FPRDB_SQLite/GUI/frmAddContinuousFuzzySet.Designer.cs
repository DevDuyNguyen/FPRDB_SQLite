namespace FPRDB_SQLite.GUI
{
    partial class frmAddContinuousFuzzySet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddContinuousFuzzySet));
            panelControl1 = new DevExpress.XtraEditors.PanelControl();
            lblNameConsFuzzy = new DevExpress.XtraEditors.LabelControl();
            lblXCoordinates = new DevExpress.XtraEditors.LabelControl();
            lblBotLeft = new DevExpress.XtraEditors.LabelControl();
            lblTopLeft = new DevExpress.XtraEditors.LabelControl();
            lblTopRight = new DevExpress.XtraEditors.LabelControl();
            txtNameConsFuzzy = new DevExpress.XtraEditors.TextEdit();
            txtBotLeft = new DevExpress.XtraEditors.TextEdit();
            lblBotRight = new DevExpress.XtraEditors.LabelControl();
            txtTopLeft = new DevExpress.XtraEditors.TextEdit();
            txtTopRight = new DevExpress.XtraEditors.TextEdit();
            txtBotRight = new DevExpress.XtraEditors.TextEdit();
            btnSave = new DevExpress.XtraEditors.SimpleButton();
            btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)panelControl1).BeginInit();
            panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtNameConsFuzzy.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtBotLeft.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtTopLeft.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtTopRight.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtBotRight.Properties).BeginInit();
            SuspendLayout();
            // 
            // panelControl1
            // 
            panelControl1.Controls.Add(btnCancel);
            panelControl1.Controls.Add(btnSave);
            panelControl1.Controls.Add(txtBotRight);
            panelControl1.Controls.Add(txtTopRight);
            panelControl1.Controls.Add(txtTopLeft);
            panelControl1.Controls.Add(lblBotRight);
            panelControl1.Controls.Add(txtBotLeft);
            panelControl1.Controls.Add(txtNameConsFuzzy);
            panelControl1.Controls.Add(lblTopRight);
            panelControl1.Controls.Add(lblTopLeft);
            panelControl1.Controls.Add(lblBotLeft);
            panelControl1.Controls.Add(lblXCoordinates);
            panelControl1.Controls.Add(lblNameConsFuzzy);
            panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            panelControl1.Location = new System.Drawing.Point(0, 0);
            panelControl1.Name = "panelControl1";
            panelControl1.Size = new System.Drawing.Size(436, 298);
            panelControl1.TabIndex = 0;
            // 
            // lblNameConsFuzzy
            // 
            lblNameConsFuzzy.Location = new System.Drawing.Point(36, 26);
            lblNameConsFuzzy.Name = "lblNameConsFuzzy";
            lblNameConsFuzzy.Size = new System.Drawing.Size(91, 16);
            lblNameConsFuzzy.TabIndex = 0;
            lblNameConsFuzzy.Text = "Linguistic Label:";
            // 
            // lblXCoordinates
            // 
            lblXCoordinates.Location = new System.Drawing.Point(36, 66);
            lblXCoordinates.Name = "lblXCoordinates";
            lblXCoordinates.Size = new System.Drawing.Size(183, 16);
            lblXCoordinates.TabIndex = 1;
            lblXCoordinates.Text = "X-Coordinates for the FuzzySet:";
            // 
            // lblBotLeft
            // 
            lblBotLeft.Location = new System.Drawing.Point(56, 100);
            lblBotLeft.Name = "lblBotLeft";
            lblBotLeft.Size = new System.Drawing.Size(71, 16);
            lblBotLeft.TabIndex = 2;
            lblBotLeft.Text = "Bottom-Left:";
            // 
            // lblTopLeft
            // 
            lblTopLeft.Location = new System.Drawing.Point(56, 136);
            lblTopLeft.Name = "lblTopLeft";
            lblTopLeft.Size = new System.Drawing.Size(53, 16);
            lblTopLeft.TabIndex = 3;
            lblTopLeft.Text = "Top-Left:";
            // 
            // lblTopRight
            // 
            lblTopRight.Location = new System.Drawing.Point(56, 171);
            lblTopRight.Name = "lblTopRight";
            lblTopRight.Size = new System.Drawing.Size(61, 16);
            lblTopRight.TabIndex = 4;
            lblTopRight.Text = "Top-Right:";
            // 
            // txtNameConsFuzzy
            // 
            txtNameConsFuzzy.Location = new System.Drawing.Point(133, 23);
            txtNameConsFuzzy.Name = "txtNameConsFuzzy";
            txtNameConsFuzzy.Size = new System.Drawing.Size(282, 22);
            txtNameConsFuzzy.TabIndex = 5;
            // 
            // txtBotLeft
            // 
            txtBotLeft.Location = new System.Drawing.Point(171, 94);
            txtBotLeft.Name = "txtBotLeft";
            txtBotLeft.Size = new System.Drawing.Size(216, 22);
            txtBotLeft.TabIndex = 6;
            // 
            // lblBotRight
            // 
            lblBotRight.Location = new System.Drawing.Point(56, 207);
            lblBotRight.Name = "lblBotRight";
            lblBotRight.Size = new System.Drawing.Size(79, 16);
            lblBotRight.TabIndex = 7;
            lblBotRight.Text = "Bottom-Right:";
            // 
            // txtTopLeft
            // 
            txtTopLeft.Location = new System.Drawing.Point(171, 133);
            txtTopLeft.Name = "txtTopLeft";
            txtTopLeft.Size = new System.Drawing.Size(216, 22);
            txtTopLeft.TabIndex = 8;
            // 
            // txtTopRight
            // 
            txtTopRight.Location = new System.Drawing.Point(171, 168);
            txtTopRight.Name = "txtTopRight";
            txtTopRight.Size = new System.Drawing.Size(216, 22);
            txtTopRight.TabIndex = 9;
            // 
            // txtBotRight
            // 
            txtBotRight.Location = new System.Drawing.Point(171, 204);
            txtBotRight.Name = "txtBotRight";
            txtBotRight.Size = new System.Drawing.Size(216, 22);
            txtBotRight.TabIndex = 10;
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(259, 249);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(75, 25);
            btnSave.TabIndex = 11;
            btnSave.Text = "&Save";
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(340, 249);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 25);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "&Cancel";
            // 
            // frmAddContinuousFuzzySet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(436, 298);
            Controls.Add(panelControl1);
            IconOptions.Image = (System.Drawing.Image)resources.GetObject("frmAddContinuousFuzzySet.IconOptions.Image");
            Name = "frmAddContinuousFuzzySet";
            Text = "Continuous FuzzySet";
            ((System.ComponentModel.ISupportInitialize)panelControl1).EndInit();
            panelControl1.ResumeLayout(false);
            panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtNameConsFuzzy.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtBotLeft.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtTopLeft.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtTopRight.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtBotRight.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl lblTopRight;
        private DevExpress.XtraEditors.LabelControl lblTopLeft;
        private DevExpress.XtraEditors.LabelControl lblBotLeft;
        private DevExpress.XtraEditors.LabelControl lblXCoordinates;
        private DevExpress.XtraEditors.LabelControl lblNameConsFuzzy;
        private DevExpress.XtraEditors.TextEdit txtNameConsFuzzy;
        private DevExpress.XtraEditors.TextEdit txtBotLeft;
        private DevExpress.XtraEditors.TextEdit txtBotRight;
        private DevExpress.XtraEditors.TextEdit txtTopRight;
        private DevExpress.XtraEditors.TextEdit txtTopLeft;
        private DevExpress.XtraEditors.LabelControl lblBotRight;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
    }
}