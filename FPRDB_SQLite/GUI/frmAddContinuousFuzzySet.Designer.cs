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
            pnlAddContinuousFuzzySet = new DevExpress.XtraEditors.PanelControl();
            continuosFuzzySetInfo = new FPRDB_SQLite.GUI.UserControls.ContinuosFuzzySet();
            btnCancel = new DevExpress.XtraEditors.SimpleButton();
            btnSave = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)pnlAddContinuousFuzzySet).BeginInit();
            pnlAddContinuousFuzzySet.SuspendLayout();
            SuspendLayout();
            // 
            // pnlAddContinuousFuzzySet
            // 
            pnlAddContinuousFuzzySet.Controls.Add(continuosFuzzySetInfo);
            pnlAddContinuousFuzzySet.Controls.Add(btnCancel);
            pnlAddContinuousFuzzySet.Controls.Add(btnSave);
            pnlAddContinuousFuzzySet.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlAddContinuousFuzzySet.Location = new System.Drawing.Point(0, 0);
            pnlAddContinuousFuzzySet.Name = "pnlAddContinuousFuzzySet";
            pnlAddContinuousFuzzySet.Size = new System.Drawing.Size(436, 298);
            pnlAddContinuousFuzzySet.TabIndex = 0;
            // 
            // continuosFuzzySetInfo
            // 
            continuosFuzzySetInfo.Location = new System.Drawing.Point(0, 12);
            continuosFuzzySetInfo.Name = "continuosFuzzySetInfo";
            continuosFuzzySetInfo.Size = new System.Drawing.Size(436, 231);
            continuosFuzzySetInfo.TabIndex = 13;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(340, 249);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 25);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "&Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(259, 249);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(75, 25);
            btnSave.TabIndex = 11;
            btnSave.Text = "&Save";
            btnSave.Click += btnSave_Click;
            // 
            // frmAddContinuousFuzzySet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(436, 298);
            Controls.Add(pnlAddContinuousFuzzySet);
            IconOptions.Image = (System.Drawing.Image)resources.GetObject("frmAddContinuousFuzzySet.IconOptions.Image");
            Name = "frmAddContinuousFuzzySet";
            Text = "Continuous FuzzySet";
            ((System.ComponentModel.ISupportInitialize)pnlAddContinuousFuzzySet).EndInit();
            pnlAddContinuousFuzzySet.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlAddContinuousFuzzySet;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private UserControls.ContinuosFuzzySet continuosFuzzySetInfo;
    }
}