namespace FPRDB_SQLite
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            NewDB = new DevExpress.XtraBars.BarButtonItem();
            OpenDB = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            gridControl2 = new DevExpress.XtraGrid.GridControl();
            gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            dataGridView2 = new System.Windows.Forms.DataGridView();
            ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            imageList1 = new System.Windows.Forms.ImageList(components);
            treeView1 = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)repositoryItemPictureEdit1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabControl1).BeginInit();
            xtraTabControl1.SuspendLayout();
            xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControl2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView2).BeginInit();
            xtraTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // repositoryItemPictureEdit1
            // 
            repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            // 
            // ribbonControl1
            // 
            ribbonControl1.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 37, 35, 37);
            ribbonControl1.ExpandCollapseItem.Id = 0;
            ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { ribbonControl1.ExpandCollapseItem, barEditItem1, NewDB, OpenDB, barButtonItem3, barButtonItem4, barButtonItem5 });
            ribbonControl1.Location = new System.Drawing.Point(0, 0);
            ribbonControl1.Margin = new System.Windows.Forms.Padding(4);
            ribbonControl1.MaxItemId = 20;
            ribbonControl1.Name = "ribbonControl1";
            ribbonControl1.OptionsMenuMinWidth = 385;
            ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { ribbonPage1 });
            ribbonControl1.Size = new System.Drawing.Size(1158, 193);
            // 
            // barEditItem1
            // 
            barEditItem1.Caption = "barEditItem1";
            barEditItem1.Edit = repositoryItemPictureEdit1;
            barEditItem1.Id = 2;
            barEditItem1.Name = "barEditItem1";
            // 
            // NewDB
            // 
            NewDB.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            NewDB.Caption = "new";
            NewDB.Id = 10;
            NewDB.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("NewDB.ImageOptions.Image");
            NewDB.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("NewDB.ImageOptions.LargeImage");
            NewDB.Name = "NewDB";
            NewDB.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            NewDB.ItemClick += newDB_ItemClick;
            // 
            // OpenDB
            // 
            OpenDB.Caption = "open";
            OpenDB.Id = 16;
            OpenDB.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("OpenDB.ImageOptions.Image");
            OpenDB.Name = "OpenDB";
            OpenDB.ItemClick += OpenDB_ItemClick;
            // 
            // barButtonItem3
            // 
            barButtonItem3.Caption = "close";
            barButtonItem3.Id = 17;
            barButtonItem3.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("barButtonItem3.ImageOptions.Image");
            barButtonItem3.Name = "barButtonItem3";
            // 
            // barButtonItem4
            // 
            barButtonItem4.Caption = "save";
            barButtonItem4.Id = 18;
            barButtonItem4.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("barButtonItem4.ImageOptions.LargeImage");
            barButtonItem4.Name = "barButtonItem4";
            barButtonItem4.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // barButtonItem5
            // 
            barButtonItem5.Caption = "save as";
            barButtonItem5.Id = 19;
            barButtonItem5.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("barButtonItem5.ImageOptions.LargeImage");
            barButtonItem5.Name = "barButtonItem5";
            // 
            // ribbonPage1
            // 
            ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { ribbonPageGroup2, ribbonPageGroup3 });
            ribbonPage1.Name = "ribbonPage1";
            ribbonPage1.Text = "ribbonPage1";
            // 
            // ribbonPageGroup2
            // 
            ribbonPageGroup2.ItemLinks.Add(NewDB);
            ribbonPageGroup2.ItemLinks.Add(OpenDB);
            ribbonPageGroup2.ItemLinks.Add(barButtonItem3);
            ribbonPageGroup2.Name = "ribbonPageGroup2";
            ribbonPageGroup2.Text = "file";
            // 
            // ribbonPageGroup3
            // 
            ribbonPageGroup3.ItemLinks.Add(barButtonItem4);
            ribbonPageGroup3.ItemLinks.Add(barButtonItem5);
            ribbonPageGroup3.Name = "ribbonPageGroup3";
            ribbonPageGroup3.Text = "ribbonPageGroup3";
            // 
            // xtraTabControl1
            // 
            xtraTabControl1.Location = new System.Drawing.Point(284, 221);
            xtraTabControl1.Name = "xtraTabControl1";
            xtraTabControl1.SelectedTabPage = xtraTabPage1;
            xtraTabControl1.Size = new System.Drawing.Size(862, 364);
            xtraTabControl1.TabIndex = 1;
            xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] { xtraTabPage1, xtraTabPage2, xtraTabPage3 });
            // 
            // xtraTabPage1
            // 
            xtraTabPage1.Controls.Add(gridControl1);
            xtraTabPage1.Name = "xtraTabPage1";
            xtraTabPage1.Size = new System.Drawing.Size(860, 334);
            xtraTabPage1.Text = "xtraTabPage1";
            // 
            // gridControl1
            // 
            gridControl1.Location = new System.Drawing.Point(-1, 0);
            gridControl1.MainView = gridView1;
            gridControl1.MenuManager = ribbonControl1;
            gridControl1.Name = "gridControl1";
            gridControl1.Size = new System.Drawing.Size(862, 331);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView1 });
            // 
            // gridView1
            // 
            gridView1.GridControl = gridControl1;
            gridView1.Name = "gridView1";
            // 
            // xtraTabPage2
            // 
            xtraTabPage2.Controls.Add(gridControl2);
            xtraTabPage2.Name = "xtraTabPage2";
            xtraTabPage2.Size = new System.Drawing.Size(860, 334);
            xtraTabPage2.Text = "xtraTabPage2";
            // 
            // gridControl2
            // 
            gridControl2.Location = new System.Drawing.Point(-1, 0);
            gridControl2.MainView = gridView2;
            gridControl2.MenuManager = ribbonControl1;
            gridControl2.Name = "gridControl2";
            gridControl2.Size = new System.Drawing.Size(552, 198);
            gridControl2.TabIndex = 2;
            gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView2 });
            // 
            // gridView2
            // 
            gridView2.GridControl = gridControl2;
            gridView2.Name = "gridView2";
            // 
            // xtraTabPage3
            // 
            xtraTabPage3.Controls.Add(dataGridView1);
            xtraTabPage3.Controls.Add(dataGridView2);
            xtraTabPage3.Name = "xtraTabPage3";
            xtraTabPage3.Size = new System.Drawing.Size(860, 334);
            xtraTabPage3.Text = "xtraTabPage3";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new System.Drawing.Point(0, 111);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new System.Drawing.Size(551, 90);
            dataGridView1.TabIndex = 3;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new System.Drawing.Point(0, 3);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new System.Drawing.Size(551, 88);
            dataGridView2.TabIndex = 4;
            // 
            // ribbonPageGroup1
            // 
            ribbonPageGroup1.ItemLinks.Add(barEditItem1);
            ribbonPageGroup1.Name = "ribbonPageGroup1";
            ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "open.jpg");
            imageList1.Images.SetKeyName(1, "close.png");
            imageList1.Images.SetKeyName(2, "attribute.png");
            imageList1.Images.SetKeyName(3, "database.png");
            imageList1.Images.SetKeyName(4, "folder.png");
            imageList1.Images.SetKeyName(5, "key.png");
            imageList1.Images.SetKeyName(6, "open.png");
            imageList1.Images.SetKeyName(7, "relation.jpg");
            // 
            // treeView1
            // 
            treeView1.ImageIndex = 0;
            treeView1.ImageList = imageList1;
            treeView1.Location = new System.Drawing.Point(7, 225);
            treeView1.Name = "treeView1";
            treeView1.SelectedImageIndex = 0;
            treeView1.Size = new System.Drawing.Size(257, 356);
            treeView1.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1158, 597);
            Controls.Add(treeView1);
            Controls.Add(xtraTabControl1);
            Controls.Add(ribbonControl1);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "Form1";
            Ribbon = ribbonControl1;
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)repositoryItemPictureEdit1).EndInit();
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabControl1).EndInit();
            xtraTabControl1.ResumeLayout(false);
            xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControl2).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView2).EndInit();
            xtraTabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private DevExpress.XtraBars.BarButtonItem NewDB;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraBars.BarButtonItem OpenDB;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private System.Windows.Forms.TreeView treeView1;
    }
}

