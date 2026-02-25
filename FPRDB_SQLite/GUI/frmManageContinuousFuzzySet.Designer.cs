namespace FPRDB_SQLite.GUI
{
    partial class frmManageContinuousFuzzySet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManageContinuousFuzzySet));
            pnlManageContinuousFuzzySet = new DevExpress.XtraEditors.PanelControl();
            continuosFuzzySetInfo = new FPRDB_SQLite.GUI.UserControls.ContinuosFuzzySet();
            btnDelete = new DevExpress.XtraEditors.SimpleButton();
            btnView = new DevExpress.XtraEditors.SimpleButton();
            btnUpdate = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)pnlManageContinuousFuzzySet).BeginInit();
            pnlManageContinuousFuzzySet.SuspendLayout();
            SuspendLayout();
            // 
            // pnlManageContinuousFuzzySet
            // 
            pnlManageContinuousFuzzySet.Controls.Add(btnDelete);
            pnlManageContinuousFuzzySet.Controls.Add(btnView);
            pnlManageContinuousFuzzySet.Controls.Add(btnUpdate);
            pnlManageContinuousFuzzySet.Controls.Add(continuosFuzzySetInfo);
            pnlManageContinuousFuzzySet.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlManageContinuousFuzzySet.Location = new System.Drawing.Point(0, 0);
            pnlManageContinuousFuzzySet.Name = "pnlManageContinuousFuzzySet";
            pnlManageContinuousFuzzySet.Size = new System.Drawing.Size(426, 285);
            pnlManageContinuousFuzzySet.TabIndex = 0;
            // 
            // continuosFuzzySetInfo
            // 
            continuosFuzzySetInfo.Location = new System.Drawing.Point(-2, 12);
            continuosFuzzySetInfo.Name = "continuosFuzzySetInfo";
            continuosFuzzySetInfo.Size = new System.Drawing.Size(429, 224);
            continuosFuzzySetInfo.TabIndex = 0;
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(339, 242);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(75, 25);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "&Delete";
            // 
            // btnView
            // 
            btnView.Location = new System.Drawing.Point(258, 242);
            btnView.Name = "btnView";
            btnView.Size = new System.Drawing.Size(75, 25);
            btnView.TabIndex = 5;
            btnView.Text = "&View";
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new System.Drawing.Point(177, 242);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new System.Drawing.Size(75, 25);
            btnUpdate.TabIndex = 4;
            btnUpdate.Text = "&Update";
            // 
            // frmManageContinuousFuzzySet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(426, 285);
            Controls.Add(pnlManageContinuousFuzzySet);
            IconOptions.Image = (System.Drawing.Image)resources.GetObject("frmManageContinuousFuzzySet.IconOptions.Image");
            Name = "frmManageContinuousFuzzySet";
            Text = "Manage Continuous FuzzySet";
            ((System.ComponentModel.ISupportInitialize)pnlManageContinuousFuzzySet).EndInit();
            pnlManageContinuousFuzzySet.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlManageContinuousFuzzySet;
        private UserControls.ContinuosFuzzySet continuosFuzzySetInfo;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnView;
        private DevExpress.XtraEditors.SimpleButton btnUpdate;
    }
}