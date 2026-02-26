namespace FPRDB_SQLite.GUI
{
    partial class frmAddDiscreteFuzzySet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddDiscreteFuzzySet));
            pnlAddDiscreteFuzzySet = new DevExpress.XtraEditors.PanelControl();
            discreteFuzzySetInfo = new FPRDB_SQLite.GUI.UserControls.DiscreteFuzzySet();
            btnCancel = new DevExpress.XtraEditors.SimpleButton();
            btnSave = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)pnlAddDiscreteFuzzySet).BeginInit();
            pnlAddDiscreteFuzzySet.SuspendLayout();
            SuspendLayout();
            // 
            // pnlAddDiscreteFuzzySet
            // 
            pnlAddDiscreteFuzzySet.Controls.Add(discreteFuzzySetInfo);
            pnlAddDiscreteFuzzySet.Controls.Add(btnCancel);
            pnlAddDiscreteFuzzySet.Controls.Add(btnSave);
            pnlAddDiscreteFuzzySet.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlAddDiscreteFuzzySet.Location = new System.Drawing.Point(0, 0);
            pnlAddDiscreteFuzzySet.Name = "pnlAddDiscreteFuzzySet";
            pnlAddDiscreteFuzzySet.Size = new System.Drawing.Size(541, 374);
            pnlAddDiscreteFuzzySet.TabIndex = 0;
            // 
            // discreteFuzzySetInfo
            // 
            discreteFuzzySetInfo.Location = new System.Drawing.Point(0, 12);
            discreteFuzzySetInfo.Name = "discreteFuzzySetInfo";
            discreteFuzzySetInfo.Size = new System.Drawing.Size(541, 326);
            discreteFuzzySetInfo.TabIndex = 5;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(454, 344);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 25);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "&Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new System.Drawing.Point(358, 344);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(75, 25);
            btnSave.TabIndex = 3;
            btnSave.Text = "&Save";
            btnSave.Click += btnSave_Click;
            // 
            // frmAddDiscreteFuzzySet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(541, 374);
            Controls.Add(pnlAddDiscreteFuzzySet);
            IconOptions.Image = (System.Drawing.Image)resources.GetObject("frmAddDiscreteFuzzySet.IconOptions.Image");
            Name = "frmAddDiscreteFuzzySet";
            Text = "Discrete FuzzySet";
            ((System.ComponentModel.ISupportInitialize)pnlAddDiscreteFuzzySet).EndInit();
            pnlAddDiscreteFuzzySet.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlAddDiscreteFuzzySet;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private UserControls.DiscreteFuzzySet discreteFuzzySetInfo;
    }
}