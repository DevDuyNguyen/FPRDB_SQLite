namespace FPRDB_SQLite.GUI
{
    partial class frmSearchFuzzySet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchFuzzySet));
            panelControl1 = new DevExpress.XtraEditors.PanelControl();
            lstFuzzySets = new DevExpress.XtraEditors.ListBoxControl();
            txtNameFuzzySet = new DevExpress.XtraEditors.TextEdit();
            lblNameFuzzySet = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)panelControl1).BeginInit();
            panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lstFuzzySets).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtNameFuzzySet.Properties).BeginInit();
            SuspendLayout();
            // 
            // panelControl1
            // 
            panelControl1.Controls.Add(lstFuzzySets);
            panelControl1.Controls.Add(txtNameFuzzySet);
            panelControl1.Controls.Add(lblNameFuzzySet);
            panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            panelControl1.Location = new System.Drawing.Point(0, 0);
            panelControl1.Name = "panelControl1";
            panelControl1.Size = new System.Drawing.Size(477, 277);
            panelControl1.TabIndex = 0;
            // 
            // lstFuzzySets
            // 
            lstFuzzySets.Location = new System.Drawing.Point(45, 61);
            lstFuzzySets.Name = "lstFuzzySets";
            lstFuzzySets.Size = new System.Drawing.Size(377, 190);
            lstFuzzySets.TabIndex = 2;
            lstFuzzySets.MouseDoubleClick += lstFuzzySets_MouseDoubleClick;
            // 
            // txtNameFuzzySet
            // 
            txtNameFuzzySet.Location = new System.Drawing.Point(142, 18);
            txtNameFuzzySet.Name = "txtNameFuzzySet";
            txtNameFuzzySet.Size = new System.Drawing.Size(280, 22);
            txtNameFuzzySet.TabIndex = 1;
            // 
            // lblNameFuzzySet
            // 
            lblNameFuzzySet.Location = new System.Drawing.Point(45, 21);
            lblNameFuzzySet.Name = "lblNameFuzzySet";
            lblNameFuzzySet.Size = new System.Drawing.Size(91, 16);
            lblNameFuzzySet.TabIndex = 0;
            lblNameFuzzySet.Text = "Linguistic Label:";
            // 
            // frmSearchFuzzySet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(477, 277);
            Controls.Add(panelControl1);
            IconOptions.Image = (System.Drawing.Image)resources.GetObject("frmSearchFuzzySet.IconOptions.Image");
            Name = "frmSearchFuzzySet";
            Text = "Search FuzzySet";
            ((System.ComponentModel.ISupportInitialize)panelControl1).EndInit();
            panelControl1.ResumeLayout(false);
            panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)lstFuzzySets).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtNameFuzzySet.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.TextEdit txtNameFuzzySet;
        private DevExpress.XtraEditors.LabelControl lblNameFuzzySet;
        private DevExpress.XtraEditors.ListBoxControl lstFuzzySets;
    }
}