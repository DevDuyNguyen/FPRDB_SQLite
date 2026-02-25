namespace FPRDB_SQLite.GUI
{
    partial class frmManageDiscreteFuzzySet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManageDiscreteFuzzySet));
            pnlManageDiscreteFuzzySet = new DevExpress.XtraEditors.PanelControl();
            discreteFuzzySetInfo = new FPRDB_SQLite.GUI.UserControls.DiscreteFuzzySet();
            btnUpdate = new DevExpress.XtraEditors.SimpleButton();
            btnView = new DevExpress.XtraEditors.SimpleButton();
            btnDelete = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)pnlManageDiscreteFuzzySet).BeginInit();
            pnlManageDiscreteFuzzySet.SuspendLayout();
            SuspendLayout();
            // 
            // pnlManageDiscreteFuzzySet
            // 
            pnlManageDiscreteFuzzySet.Controls.Add(btnDelete);
            pnlManageDiscreteFuzzySet.Controls.Add(btnView);
            pnlManageDiscreteFuzzySet.Controls.Add(btnUpdate);
            pnlManageDiscreteFuzzySet.Controls.Add(discreteFuzzySetInfo);
            pnlManageDiscreteFuzzySet.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlManageDiscreteFuzzySet.Location = new System.Drawing.Point(0, 0);
            pnlManageDiscreteFuzzySet.Name = "pnlManageDiscreteFuzzySet";
            pnlManageDiscreteFuzzySet.Size = new System.Drawing.Size(545, 348);
            pnlManageDiscreteFuzzySet.TabIndex = 0;
            // 
            // discreteFuzzySetInfo
            // 
            discreteFuzzySetInfo.Location = new System.Drawing.Point(0, 5);
            discreteFuzzySetInfo.Name = "discreteFuzzySetInfo";
            discreteFuzzySetInfo.Size = new System.Drawing.Size(545, 295);
            discreteFuzzySetInfo.TabIndex = 0;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new System.Drawing.Point(296, 307);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new System.Drawing.Size(75, 25);
            btnUpdate.TabIndex = 1;
            btnUpdate.Text = "&Update";
            // 
            // btnView
            // 
            btnView.Location = new System.Drawing.Point(377, 307);
            btnView.Name = "btnView";
            btnView.Size = new System.Drawing.Size(75, 25);
            btnView.TabIndex = 2;
            btnView.Text = "&View";
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(458, 307);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(75, 25);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "&Delete";
            // 
            // frmManageDiscreteFuzzySet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(545, 348);
            Controls.Add(pnlManageDiscreteFuzzySet);
            IconOptions.Image = (System.Drawing.Image)resources.GetObject("frmManageDiscreteFuzzySet.IconOptions.Image");
            Name = "frmManageDiscreteFuzzySet";
            Text = "Manage Discrete FuzzySet";
            ((System.ComponentModel.ISupportInitialize)pnlManageDiscreteFuzzySet).EndInit();
            pnlManageDiscreteFuzzySet.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlManageDiscreteFuzzySet;
        private UserControls.DiscreteFuzzySet discreteFuzzySetInfo;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnView;
        private DevExpress.XtraEditors.SimpleButton btnUpdate;
    }
}