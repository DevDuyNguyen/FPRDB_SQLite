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
            btnCancel = new DevExpress.XtraEditors.SimpleButton();
            btnDelete = new DevExpress.XtraEditors.SimpleButton();
            btnView = new DevExpress.XtraEditors.SimpleButton();
            btnUpdate = new DevExpress.XtraEditors.SimpleButton();
            discreteFuzzySetInfo = new FPRDB_SQLite.GUI.UserControls.DiscreteFuzzySet();
            ((System.ComponentModel.ISupportInitialize)pnlManageDiscreteFuzzySet).BeginInit();
            pnlManageDiscreteFuzzySet.SuspendLayout();
            SuspendLayout();
            // 
            // pnlManageDiscreteFuzzySet
            // 
            pnlManageDiscreteFuzzySet.Controls.Add(btnCancel);
            pnlManageDiscreteFuzzySet.Controls.Add(btnDelete);
            pnlManageDiscreteFuzzySet.Controls.Add(btnView);
            pnlManageDiscreteFuzzySet.Controls.Add(btnUpdate);
            pnlManageDiscreteFuzzySet.Controls.Add(discreteFuzzySetInfo);
            pnlManageDiscreteFuzzySet.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlManageDiscreteFuzzySet.Location = new System.Drawing.Point(0, 0);
            pnlManageDiscreteFuzzySet.Name = "pnlManageDiscreteFuzzySet";
            pnlManageDiscreteFuzzySet.Size = new System.Drawing.Size(545, 369);
            pnlManageDiscreteFuzzySet.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(458, 339);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 25);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "&Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(379, 339);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(75, 25);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "&Delete";
            btnDelete.Click += btnDelete_Click;
            // 
            // btnView
            // 
            btnView.Location = new System.Drawing.Point(298, 339);
            btnView.Name = "btnView";
            btnView.Size = new System.Drawing.Size(75, 25);
            btnView.TabIndex = 2;
            btnView.Text = "&View";
            btnView.Click += btnView_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new System.Drawing.Point(217, 339);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new System.Drawing.Size(75, 25);
            btnUpdate.TabIndex = 1;
            btnUpdate.Text = "&Update";
            btnUpdate.Click += btnUpdate_Click;
            // 
            // discreteFuzzySetInfo
            // 
            discreteFuzzySetInfo.Location = new System.Drawing.Point(0, 5);
            discreteFuzzySetInfo.Name = "discreteFuzzySetInfo";
            discreteFuzzySetInfo.Size = new System.Drawing.Size(545, 327);
            discreteFuzzySetInfo.TabIndex = 0;
            // 
            // frmManageDiscreteFuzzySet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(545, 369);
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
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}