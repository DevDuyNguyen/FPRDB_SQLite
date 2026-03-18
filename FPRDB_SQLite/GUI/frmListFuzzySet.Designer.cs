namespace GUI.GUI
{
    partial class frmListFuzzySet
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
            panelControlListFS = new DevExpress.XtraEditors.PanelControl();
            gridControlListFS = new DevExpress.XtraGrid.GridControl();
            gridViewListFS = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridColumnFSName = new DevExpress.XtraGrid.Columns.GridColumn();
            panelControlButton = new DevExpress.XtraEditors.PanelControl();
            simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            simpleButtonOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)panelControlListFS).BeginInit();
            panelControlListFS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControlListFS).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridViewListFS).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelControlButton).BeginInit();
            panelControlButton.SuspendLayout();
            SuspendLayout();
            // 
            // panelControlListFS
            // 
            panelControlListFS.Controls.Add(gridControlListFS);
            panelControlListFS.Dock = System.Windows.Forms.DockStyle.Fill;
            panelControlListFS.Location = new System.Drawing.Point(0, 0);
            panelControlListFS.Name = "panelControlListFS";
            panelControlListFS.Size = new System.Drawing.Size(463, 357);
            panelControlListFS.TabIndex = 0;
            // 
            // gridControlListFS
            // 
            gridControlListFS.Dock = System.Windows.Forms.DockStyle.Fill;
            gridControlListFS.Location = new System.Drawing.Point(2, 2);
            gridControlListFS.MainView = gridViewListFS;
            gridControlListFS.Name = "gridControlListFS";
            gridControlListFS.Size = new System.Drawing.Size(459, 353);
            gridControlListFS.TabIndex = 0;
            gridControlListFS.UseEmbeddedNavigator = true;
            gridControlListFS.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridViewListFS });
            // 
            // gridViewListFS
            // 
            gridViewListFS.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { gridColumnFSName });
            gridViewListFS.GridControl = gridControlListFS;
            gridViewListFS.Name = "gridViewListFS";
            gridViewListFS.OptionsBehavior.Editable = false;
            gridViewListFS.OptionsView.ShowColumnHeaders = false;
            gridViewListFS.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumnFSName
            // 
            gridColumnFSName.FieldName = "Name";
            gridColumnFSName.MinWidth = 25;
            gridColumnFSName.Name = "gridColumnFSName";
            gridColumnFSName.Visible = true;
            gridColumnFSName.VisibleIndex = 0;
            gridColumnFSName.Width = 94;
            // 
            // panelControlButton
            // 
            panelControlButton.Controls.Add(simpleButtonCancel);
            panelControlButton.Controls.Add(simpleButtonOK);
            panelControlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelControlButton.Location = new System.Drawing.Point(0, 357);
            panelControlButton.Name = "panelControlButton";
            panelControlButton.Size = new System.Drawing.Size(463, 50);
            panelControlButton.TabIndex = 0;
            // 
            // simpleButtonCancel
            // 
            simpleButtonCancel.Location = new System.Drawing.Point(376, 13);
            simpleButtonCancel.Name = "simpleButtonCancel";
            simpleButtonCancel.Size = new System.Drawing.Size(75, 25);
            simpleButtonCancel.TabIndex = 1;
            simpleButtonCancel.Text = "&Cancel";
            simpleButtonCancel.Click += simpleButtonCancel_Click;
            // 
            // simpleButtonOK
            // 
            simpleButtonOK.Location = new System.Drawing.Point(278, 13);
            simpleButtonOK.Name = "simpleButtonOK";
            simpleButtonOK.Size = new System.Drawing.Size(75, 25);
            simpleButtonOK.TabIndex = 0;
            simpleButtonOK.Text = "&OK";
            simpleButtonOK.Click += simpleButtonOK_Click;
            // 
            // frmListFuzzySet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(463, 407);
            Controls.Add(panelControlListFS);
            Controls.Add(panelControlButton);
            Name = "frmListFuzzySet";
            Text = "Fuzzy Set";
            ((System.ComponentModel.ISupportInitialize)panelControlListFS).EndInit();
            panelControlListFS.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControlListFS).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridViewListFS).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelControlButton).EndInit();
            panelControlButton.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlListFS;
        private DevExpress.XtraEditors.PanelControl panelControlButton;
        private DevExpress.XtraGrid.GridControl gridControlListFS;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewListFS;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnFSName;
        private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
        private DevExpress.XtraEditors.SimpleButton simpleButtonOK;
    }
}