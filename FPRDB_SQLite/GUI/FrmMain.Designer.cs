using DevExpress.XtraVerticalGrid.Internal;

namespace FPRDB_SQLite.GUI
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            buttonExit_pageHome = new DevExpress.XtraBars.BarButtonItem();
            buttonNew_pageHome = new DevExpress.XtraBars.BarButtonItem();
            buttonOpen_pageHome = new DevExpress.XtraBars.BarButtonItem();
            iAddDiscrete = new DevExpress.XtraBars.BarButtonItem();
            iAddContinuous = new DevExpress.XtraBars.BarButtonItem();
            iNewSchema = new DevExpress.XtraBars.BarButtonItem();
            iDeleteSchema = new DevExpress.XtraBars.BarButtonItem();
            iSearchFuzzySet = new DevExpress.XtraBars.BarButtonItem();
            iNewRelation = new DevExpress.XtraBars.BarButtonItem();
            iDeleteRelation = new DevExpress.XtraBars.BarButtonItem();
            iCloseRelation = new DevExpress.XtraBars.BarButtonItem();
            iNewQuery = new DevExpress.XtraBars.BarButtonItem();
            iOpenQuery = new DevExpress.XtraBars.BarButtonItem();
            iSaveQuery = new DevExpress.XtraBars.BarButtonItem();
            iConjunctionIgnorance = new DevExpress.XtraBars.BarButtonItem();
            iConjunctionIndependence = new DevExpress.XtraBars.BarButtonItem();
            iConjunctionMutual = new DevExpress.XtraBars.BarButtonItem();
            iDisjunctionIgnorance = new DevExpress.XtraBars.BarButtonItem();
            iDisjunctionIndependence = new DevExpress.XtraBars.BarButtonItem();
            iDisjunctionMutual = new DevExpress.XtraBars.BarButtonItem();
            iDifferenceIgnorance = new DevExpress.XtraBars.BarButtonItem();
            iDifferenceIndependence = new DevExpress.XtraBars.BarButtonItem();
            iDiferenceMutual = new DevExpress.XtraBars.BarButtonItem();
            iOperator = new DevExpress.XtraBars.BarButtonItem();
            iExcuteQuery = new DevExpress.XtraBars.BarButtonItem();
            iConjunctionPositive = new DevExpress.XtraBars.BarButtonItem();
            iDisjunctionPositive = new DevExpress.XtraBars.BarButtonItem();
            iDifferencePositive = new DevExpress.XtraBars.BarButtonItem();
            barButtonGroup1 = new DevExpress.XtraBars.BarButtonGroup();
            barButtonGroup2 = new DevExpress.XtraBars.BarButtonGroup();
            barButtonSelectTuples = new DevExpress.XtraBars.BarButtonItem();
            iExportFS = new DevExpress.XtraBars.BarButtonItem();
            iImportFS = new DevExpress.XtraBars.BarButtonItem();
            barButtonRelationships = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            DatabaseRibbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
            groupFile_pageHome = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            groupExit_pageHome = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            SchemaRibbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
            fileSchemaRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            RelationRibbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
            fileRelationRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            QueryRibbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
            fileQueryRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            conjunctionRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            disjunctionRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            differenceRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            operatorRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            excuteQueryribbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            pageFuzzySet = new DevExpress.XtraBars.Ribbon.RibbonPage();
            discreteFuzzySetribbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            continuousFuzzySetRibbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            SearchFuzzySetribbonPageGroup = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            TreeView_imageList = new System.Windows.Forms.ImageList(components);
            RelationsplitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
            treeView = new System.Windows.Forms.TreeView();
            xtraTabControlDatabase = new DevExpress.XtraTab.XtraTabControl();
            SchemaxtraTabPage = new DevExpress.XtraTab.XtraTabPage();
            gridControlScheme = new DevExpress.XtraGrid.GridControl();
            gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridColumnPrimary = new DevExpress.XtraGrid.Columns.GridColumn();
            repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            gridColumnAttribute = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumnDataType = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumnLength = new DevExpress.XtraGrid.Columns.GridColumn();
            repositoryItemCalcEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            repositoryItemHeightType1 = new DevExpress.XtraRichEdit.Design.RepositoryItemHeightType();
            RelationxtraTabPage = new DevExpress.XtraTab.XtraTabPage();
            splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            gridControlRelation = new DevExpress.XtraGrid.GridControl();
            gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridControlValueRelation = new DevExpress.XtraGrid.GridControl();
            gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridColumnValue = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumnMinProb = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumnMaxProb = new DevExpress.XtraGrid.Columns.GridColumn();
            panelControlRelation = new DevExpress.XtraEditors.PanelControl();
            popupMenuTreeView = new DevExpress.XtraBars.PopupMenu(components);
            ContextMenu_RelationNode = new System.Windows.Forms.ContextMenuStrip(components);
            CTMenuRelNode_OpenRelation = new System.Windows.Forms.ToolStripMenuItem();
            CTMenuRelNode_DeleteRelation = new System.Windows.Forms.ToolStripMenuItem();
            CTMenuRelNode_RenameRelation = new System.Windows.Forms.ToolStripMenuItem();
            ContextMenu_SchemaNode = new System.Windows.Forms.ContextMenuStrip(components);
            CTMenuSchNode_EditSchema = new System.Windows.Forms.ToolStripMenuItem();
            CTMenuSchNode_OpenSchema = new System.Windows.Forms.ToolStripMenuItem();
            CTMenuSchNode_DeleteSchema = new System.Windows.Forms.ToolStripMenuItem();
            ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)ribbonControl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RelationsplitContainerControl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RelationsplitContainerControl.Panel1).BeginInit();
            RelationsplitContainerControl.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)RelationsplitContainerControl.Panel2).BeginInit();
            RelationsplitContainerControl.Panel2.SuspendLayout();
            RelationsplitContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)xtraTabControlDatabase).BeginInit();
            xtraTabControlDatabase.SuspendLayout();
            SchemaxtraTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControlScheme).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemCheckEdit1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemCalcEdit1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemButtonEdit1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemHeightType1).BeginInit();
            RelationxtraTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerControl2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerControl2.Panel1).BeginInit();
            splitContainerControl2.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerControl2.Panel2).BeginInit();
            splitContainerControl2.Panel2.SuspendLayout();
            splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControlRelation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridControlValueRelation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelControlRelation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)popupMenuTreeView).BeginInit();
            ContextMenu_RelationNode.SuspendLayout();
            ContextMenu_SchemaNode.SuspendLayout();
            SuspendLayout();
            // 
            // ribbonControl
            // 
            ribbonControl.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(65, 55, 65, 55);
            ribbonControl.ExpandCollapseItem.Id = 0;
            ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] { ribbonControl.ExpandCollapseItem, buttonExit_pageHome, buttonNew_pageHome, buttonOpen_pageHome, iAddDiscrete, iAddContinuous, iNewSchema, iDeleteSchema, iSearchFuzzySet, iNewRelation, iDeleteRelation, iCloseRelation, iNewQuery, iOpenQuery, iSaveQuery, iConjunctionIgnorance, iConjunctionIndependence, iConjunctionMutual, iDisjunctionIgnorance, iDisjunctionIndependence, iDisjunctionMutual, iDifferenceIgnorance, iDifferenceIndependence, iDiferenceMutual, iOperator, iExcuteQuery, iConjunctionPositive, iDisjunctionPositive, iDifferencePositive, barButtonGroup1, barButtonGroup2, barButtonSelectTuples, iExportFS, iImportFS, barButtonRelationships, barButtonItem1, barButtonItem2 });
            ribbonControl.Location = new System.Drawing.Point(0, 0);
            ribbonControl.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            ribbonControl.MaxItemId = 61;
            ribbonControl.Name = "ribbonControl";
            ribbonControl.OptionsMenuMinWidth = 715;
            ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { DatabaseRibbonPage, SchemaRibbonPage, RelationRibbonPage, QueryRibbonPage, pageFuzzySet });
            ribbonControl.Size = new System.Drawing.Size(970, 193);
            ribbonControl.StatusBar = ribbonStatusBar;
            // 
            // buttonExit_pageHome
            // 
            buttonExit_pageHome.Caption = "Exit";
            buttonExit_pageHome.Hint = "Exit program";
            buttonExit_pageHome.Id = 8;
            buttonExit_pageHome.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("buttonExit_pageHome.ImageOptions.LargeImage");
            buttonExit_pageHome.Name = "buttonExit_pageHome";
            buttonExit_pageHome.ItemClick += buttonExit_pageHome_ItemClick;
            // 
            // buttonNew_pageHome
            // 
            buttonNew_pageHome.Caption = "New";
            buttonNew_pageHome.Id = 9;
            buttonNew_pageHome.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("buttonNew_pageHome.ImageOptions.LargeImage");
            buttonNew_pageHome.Name = "buttonNew_pageHome";
            buttonNew_pageHome.ItemClick += buttonNew_pageHome_ItemClick;
            // 
            // buttonOpen_pageHome
            // 
            buttonOpen_pageHome.Caption = "Open";
            buttonOpen_pageHome.Id = 10;
            buttonOpen_pageHome.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("buttonOpen_pageHome.ImageOptions.LargeImage");
            buttonOpen_pageHome.Name = "buttonOpen_pageHome";
            buttonOpen_pageHome.ItemClick += buttonOpen_pageHome_ItemClick;
            // 
            // iAddDiscrete
            // 
            iAddDiscrete.Caption = "Add";
            iAddDiscrete.Id = 15;
            iAddDiscrete.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("iAddDiscrete.ImageOptions.LargeImage");
            iAddDiscrete.Name = "iAddDiscrete";
            iAddDiscrete.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            iAddDiscrete.ItemClick += buttonAdd_groupDis_ItemClick;
            // 
            // iAddContinuous
            // 
            iAddContinuous.Caption = "Add";
            iAddContinuous.Id = 17;
            iAddContinuous.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("iAddContinuous.ImageOptions.LargeImage");
            iAddContinuous.Name = "iAddContinuous";
            iAddContinuous.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            iAddContinuous.ItemClick += buttonAdd_groupCont_ItemClick;
            // 
            // iNewSchema
            // 
            iNewSchema.Caption = "Create Schema";
            iNewSchema.Id = 19;
            iNewSchema.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("iNewSchema.ImageOptions.LargeImage");
            iNewSchema.Name = "iNewSchema";
            iNewSchema.ItemClick += iNewSchema_ItemClick;
            // 
            // iDeleteSchema
            // 
            iDeleteSchema.Caption = "Delete";
            iDeleteSchema.Id = 22;
            iDeleteSchema.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("iDeleteSchema.ImageOptions.LargeImage");
            iDeleteSchema.Name = "iDeleteSchema";
            iDeleteSchema.ItemClick += iDeleteSchema_ItemClick;
            // 
            // iSearchFuzzySet
            // 
            iSearchFuzzySet.Caption = "Search";
            iSearchFuzzySet.Id = 24;
            iSearchFuzzySet.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("iSearchFuzzySet.ImageOptions.LargeImage");
            iSearchFuzzySet.Name = "iSearchFuzzySet";
            iSearchFuzzySet.ItemClick += iSearchFuzzySet_ItemClick;
            // 
            // iNewRelation
            // 
            iNewRelation.Caption = "Create Relation";
            iNewRelation.Id = 25;
            iNewRelation.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("iNewRelation.ImageOptions.LargeImage");
            iNewRelation.Name = "iNewRelation";
            iNewRelation.ItemClick += iNewRelation_ItemClick;
            // 
            // iDeleteRelation
            // 
            iDeleteRelation.Caption = "Delete";
            iDeleteRelation.Id = 27;
            iDeleteRelation.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("iDeleteRelation.ImageOptions.LargeImage");
            iDeleteRelation.Name = "iDeleteRelation";
            iDeleteRelation.ItemClick += iDeleteRelation_ItemClick;
            // 
            // iCloseRelation
            // 
            iCloseRelation.Caption = "Close";
            iCloseRelation.Enabled = false;
            iCloseRelation.Id = 29;
            iCloseRelation.Name = "iCloseRelation";
            // 
            // iNewQuery
            // 
            iNewQuery.Caption = "New";
            iNewQuery.Id = 30;
            iNewQuery.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("iNewQuery.ImageOptions.LargeImage");
            iNewQuery.Name = "iNewQuery";
            iNewQuery.ItemClick += iNewQuery_ItemClick;
            // 
            // iOpenQuery
            // 
            iOpenQuery.Caption = "Open";
            iOpenQuery.Id = 31;
            iOpenQuery.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("iOpenQuery.ImageOptions.LargeImage");
            iOpenQuery.Name = "iOpenQuery";
            iOpenQuery.ItemClick += iOpenQuery_ItemClick;
            // 
            // iSaveQuery
            // 
            iSaveQuery.Caption = "Save";
            iSaveQuery.Id = 32;
            iSaveQuery.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("iSaveQuery.ImageOptions.LargeImage");
            iSaveQuery.Name = "iSaveQuery";
            iSaveQuery.ItemClick += iSaveQuery_ItemClick;
            // 
            // iConjunctionIgnorance
            // 
            iConjunctionIgnorance.Caption = "⨂_ig";
            iConjunctionIgnorance.Id = 34;
            iConjunctionIgnorance.Name = "iConjunctionIgnorance";
            iConjunctionIgnorance.SmallWithTextWidth = 80;
            iConjunctionIgnorance.ItemClick += iConjunctionIgnorance_ItemClick;
            // 
            // iConjunctionIndependence
            // 
            iConjunctionIndependence.Caption = "⨂_in";
            iConjunctionIndependence.Id = 35;
            iConjunctionIndependence.Name = "iConjunctionIndependence";
            iConjunctionIndependence.SmallWithTextWidth = 80;
            iConjunctionIndependence.ItemClick += iConjunctionIndependence_ItemClick;
            // 
            // iConjunctionMutual
            // 
            iConjunctionMutual.Caption = "⨂_me";
            iConjunctionMutual.Id = 36;
            iConjunctionMutual.Name = "iConjunctionMutual";
            iConjunctionMutual.SmallWithTextWidth = 80;
            iConjunctionMutual.ItemClick += iConjunctionMutual_ItemClick;
            // 
            // iDisjunctionIgnorance
            // 
            iDisjunctionIgnorance.Caption = "⨁_ig";
            iDisjunctionIgnorance.Id = 37;
            iDisjunctionIgnorance.Name = "iDisjunctionIgnorance";
            iDisjunctionIgnorance.SmallWithTextWidth = 80;
            iDisjunctionIgnorance.ItemClick += iDisjunctionIgnorance_ItemClick;
            // 
            // iDisjunctionIndependence
            // 
            iDisjunctionIndependence.Caption = "⨁_in";
            iDisjunctionIndependence.Id = 38;
            iDisjunctionIndependence.Name = "iDisjunctionIndependence";
            iDisjunctionIndependence.SmallWithTextWidth = 80;
            iDisjunctionIndependence.ItemClick += iDisjunctionIndependence_ItemClick;
            // 
            // iDisjunctionMutual
            // 
            iDisjunctionMutual.Caption = "⨁_me";
            iDisjunctionMutual.Id = 39;
            iDisjunctionMutual.Name = "iDisjunctionMutual";
            iDisjunctionMutual.SmallWithTextWidth = 80;
            iDisjunctionMutual.ItemClick += iDisjunctionMutual_ItemClick;
            // 
            // iDifferenceIgnorance
            // 
            iDifferenceIgnorance.Caption = "⦵_ig";
            iDifferenceIgnorance.Id = 40;
            iDifferenceIgnorance.Name = "iDifferenceIgnorance";
            iDifferenceIgnorance.SmallWithTextWidth = 80;
            iDifferenceIgnorance.ItemClick += iDifferenceIgnorance_ItemClick;
            // 
            // iDifferenceIndependence
            // 
            iDifferenceIndependence.Caption = "⦵_in";
            iDifferenceIndependence.Id = 42;
            iDifferenceIndependence.Name = "iDifferenceIndependence";
            iDifferenceIndependence.SmallWithTextWidth = 80;
            iDifferenceIndependence.ItemClick += iDifferenceIndependence_ItemClick;
            // 
            // iDiferenceMutual
            // 
            iDiferenceMutual.Caption = "⦵_me";
            iDiferenceMutual.Id = 43;
            iDiferenceMutual.Name = "iDiferenceMutual";
            iDiferenceMutual.SmallWithTextWidth = 80;
            iDiferenceMutual.ItemClick += iDiferenceMutual_ItemClick;
            // 
            // iOperator
            // 
            iOperator.Caption = "⇒";
            iOperator.Id = 46;
            iOperator.ItemAppearance.Hovered.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            iOperator.ItemAppearance.Hovered.FontStyleDelta = System.Drawing.FontStyle.Bold;
            iOperator.ItemAppearance.Hovered.Options.UseFont = true;
            iOperator.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            iOperator.ItemAppearance.Normal.FontStyleDelta = System.Drawing.FontStyle.Bold;
            iOperator.ItemAppearance.Normal.Options.UseFont = true;
            iOperator.ItemAppearance.Pressed.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            iOperator.ItemAppearance.Pressed.FontStyleDelta = System.Drawing.FontStyle.Bold;
            iOperator.ItemAppearance.Pressed.Options.UseFont = true;
            iOperator.Name = "iOperator";
            iOperator.SmallWithTextWidth = 80;
            iOperator.ItemClick += iOperator_ItemClick;
            // 
            // iExcuteQuery
            // 
            iExcuteQuery.Caption = "Excute Query";
            iExcuteQuery.Id = 47;
            iExcuteQuery.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("iExcuteQuery.ImageOptions.LargeImage");
            iExcuteQuery.Name = "iExcuteQuery";
            iExcuteQuery.ItemClick += iExcuteQuery_ItemClick;
            // 
            // iConjunctionPositive
            // 
            iConjunctionPositive.Caption = "⨂_pc";
            iConjunctionPositive.Id = 50;
            iConjunctionPositive.Name = "iConjunctionPositive";
            iConjunctionPositive.SmallWithTextWidth = 80;
            iConjunctionPositive.ItemClick += iConjunctionPositive_ItemClick;
            // 
            // iDisjunctionPositive
            // 
            iDisjunctionPositive.Caption = "⨁_pc";
            iDisjunctionPositive.Id = 51;
            iDisjunctionPositive.Name = "iDisjunctionPositive";
            iDisjunctionPositive.SmallWithTextWidth = 80;
            iDisjunctionPositive.ItemClick += iDisjunctionPositive_ItemClick;
            // 
            // iDifferencePositive
            // 
            iDifferencePositive.Caption = "⦵_pc";
            iDifferencePositive.Id = 52;
            iDifferencePositive.Name = "iDifferencePositive";
            iDifferencePositive.SmallWithTextWidth = 80;
            iDifferencePositive.ItemClick += iDifferencePositive_ItemClick;
            // 
            // barButtonGroup1
            // 
            barButtonGroup1.Caption = "barButtonGroup1";
            barButtonGroup1.Id = 53;
            barButtonGroup1.Name = "barButtonGroup1";
            // 
            // barButtonGroup2
            // 
            barButtonGroup2.Caption = "barButtonGroup2";
            barButtonGroup2.Id = 54;
            barButtonGroup2.Name = "barButtonGroup2";
            // 
            // barButtonSelectTuples
            // 
            barButtonSelectTuples.Caption = "Select top 100 tuples";
            barButtonSelectTuples.Id = 55;
            barButtonSelectTuples.Name = "barButtonSelectTuples";
            barButtonSelectTuples.ItemClick += barButtonSelectTuples_ItemClick;
            // 
            // iExportFS
            // 
            iExportFS.Caption = "Export Fuzzy Set";
            iExportFS.Id = 56;
            iExportFS.Name = "iExportFS";
            iExportFS.ItemClick += iExportFS_ItemClick;
            // 
            // iImportFS
            // 
            iImportFS.Caption = "Import Fuzzy Set";
            iImportFS.Id = 57;
            iImportFS.Name = "iImportFS";
            iImportFS.ItemClick += iImportFS_ItemClick;
            // 
            // barButtonRelationships
            // 
            barButtonRelationships.Caption = "Relationships";
            barButtonRelationships.Id = 58;
            barButtonRelationships.Name = "barButtonRelationships";
            barButtonRelationships.ItemClick += barButtonRelationships_ItemClick;
            // 
            // barButtonItem1
            // 
            barButtonItem1.Caption = "Calculate";
            barButtonItem1.Id = 59;
            barButtonItem1.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("barButtonItem1.ImageOptions.SvgImage");
            barButtonItem1.Name = "barButtonItem1";
            barButtonItem1.ItemClick += barButtonItem1_ItemClick;
            // 
            // DatabaseRibbonPage
            // 
            DatabaseRibbonPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { groupFile_pageHome, groupExit_pageHome });
            DatabaseRibbonPage.Name = "DatabaseRibbonPage";
            DatabaseRibbonPage.Text = "Home";
            // 
            // groupFile_pageHome
            // 
            groupFile_pageHome.ItemLinks.Add(buttonNew_pageHome);
            groupFile_pageHome.ItemLinks.Add(buttonOpen_pageHome);
            groupFile_pageHome.Name = "groupFile_pageHome";
            groupFile_pageHome.Text = "File";
            // 
            // groupExit_pageHome
            // 
            groupExit_pageHome.ItemLinks.Add(buttonExit_pageHome);
            groupExit_pageHome.Name = "groupExit_pageHome";
            groupExit_pageHome.Text = "Exit";
            // 
            // SchemaRibbonPage
            // 
            SchemaRibbonPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { fileSchemaRibbonPageGroup });
            SchemaRibbonPage.Name = "SchemaRibbonPage";
            SchemaRibbonPage.Text = "Schema";
            // 
            // fileSchemaRibbonPageGroup
            // 
            fileSchemaRibbonPageGroup.ItemLinks.Add(iNewSchema);
            fileSchemaRibbonPageGroup.ItemLinks.Add(iDeleteSchema);
            fileSchemaRibbonPageGroup.Name = "fileSchemaRibbonPageGroup";
            fileSchemaRibbonPageGroup.Text = "File";
            // 
            // RelationRibbonPage
            // 
            RelationRibbonPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { fileRelationRibbonPageGroup });
            RelationRibbonPage.Name = "RelationRibbonPage";
            RelationRibbonPage.Text = "Relation";
            // 
            // fileRelationRibbonPageGroup
            // 
            fileRelationRibbonPageGroup.ItemLinks.Add(iNewRelation);
            fileRelationRibbonPageGroup.ItemLinks.Add(iDeleteRelation);
            fileRelationRibbonPageGroup.Name = "fileRelationRibbonPageGroup";
            fileRelationRibbonPageGroup.Text = "File";
            // 
            // QueryRibbonPage
            // 
            QueryRibbonPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { fileQueryRibbonPageGroup, conjunctionRibbonPageGroup, disjunctionRibbonPageGroup, differenceRibbonPageGroup, operatorRibbonPageGroup, excuteQueryribbonPageGroup, ribbonPageGroup1, ribbonPageGroup2 });
            QueryRibbonPage.Name = "QueryRibbonPage";
            QueryRibbonPage.Text = "Query";
            // 
            // fileQueryRibbonPageGroup
            // 
            fileQueryRibbonPageGroup.ItemLinks.Add(iNewQuery);
            fileQueryRibbonPageGroup.ItemLinks.Add(iOpenQuery);
            fileQueryRibbonPageGroup.ItemLinks.Add(iSaveQuery);
            fileQueryRibbonPageGroup.Name = "fileQueryRibbonPageGroup";
            fileQueryRibbonPageGroup.Text = "File";
            // 
            // conjunctionRibbonPageGroup
            // 
            conjunctionRibbonPageGroup.ItemLinks.Add(iConjunctionIgnorance);
            conjunctionRibbonPageGroup.ItemLinks.Add(iConjunctionIndependence);
            conjunctionRibbonPageGroup.ItemLinks.Add(iConjunctionMutual);
            conjunctionRibbonPageGroup.ItemLinks.Add(iConjunctionPositive);
            conjunctionRibbonPageGroup.ItemsLayout = DevExpress.XtraBars.Ribbon.RibbonPageGroupItemsLayout.TwoRows;
            conjunctionRibbonPageGroup.Name = "conjunctionRibbonPageGroup";
            conjunctionRibbonPageGroup.Text = "Conjunction";
            // 
            // disjunctionRibbonPageGroup
            // 
            disjunctionRibbonPageGroup.ItemLinks.Add(iDisjunctionIgnorance);
            disjunctionRibbonPageGroup.ItemLinks.Add(iDisjunctionIndependence);
            disjunctionRibbonPageGroup.ItemLinks.Add(iDisjunctionMutual);
            disjunctionRibbonPageGroup.ItemLinks.Add(iDisjunctionPositive);
            disjunctionRibbonPageGroup.ItemsLayout = DevExpress.XtraBars.Ribbon.RibbonPageGroupItemsLayout.TwoRows;
            disjunctionRibbonPageGroup.Name = "disjunctionRibbonPageGroup";
            disjunctionRibbonPageGroup.Text = "Disjunction";
            // 
            // differenceRibbonPageGroup
            // 
            differenceRibbonPageGroup.ItemLinks.Add(iDifferenceIgnorance);
            differenceRibbonPageGroup.ItemLinks.Add(iDifferenceIndependence);
            differenceRibbonPageGroup.ItemLinks.Add(iDiferenceMutual);
            differenceRibbonPageGroup.ItemLinks.Add(iDifferencePositive);
            differenceRibbonPageGroup.ItemsLayout = DevExpress.XtraBars.Ribbon.RibbonPageGroupItemsLayout.TwoRows;
            differenceRibbonPageGroup.Name = "differenceRibbonPageGroup";
            differenceRibbonPageGroup.Text = "Difference";
            // 
            // operatorRibbonPageGroup
            // 
            operatorRibbonPageGroup.ItemLinks.Add(iOperator);
            operatorRibbonPageGroup.ItemsLayout = DevExpress.XtraBars.Ribbon.RibbonPageGroupItemsLayout.OneRow;
            operatorRibbonPageGroup.Name = "operatorRibbonPageGroup";
            operatorRibbonPageGroup.Text = "Operator";
            // 
            // excuteQueryribbonPageGroup
            // 
            excuteQueryribbonPageGroup.ItemLinks.Add(iExcuteQuery);
            excuteQueryribbonPageGroup.Name = "excuteQueryribbonPageGroup";
            excuteQueryribbonPageGroup.Text = "Excute";
            // 
            // ribbonPageGroup1
            // 
            ribbonPageGroup1.ItemLinks.Add(barButtonItem1);
            ribbonPageGroup1.Name = "ribbonPageGroup1";
            ribbonPageGroup1.Text = "Probabilistic Interpretation For Relation On Fuzzy Sets";
            // 
            // pageFuzzySet
            // 
            pageFuzzySet.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { discreteFuzzySetribbonPageGroup, continuousFuzzySetRibbonPageGroup, SearchFuzzySetribbonPageGroup });
            pageFuzzySet.Name = "pageFuzzySet";
            pageFuzzySet.Text = "Fuzzy Set";
            pageFuzzySet.Visible = false;
            // 
            // discreteFuzzySetribbonPageGroup
            // 
            discreteFuzzySetribbonPageGroup.AllowTextClipping = false;
            discreteFuzzySetribbonPageGroup.ItemLinks.Add(iAddDiscrete);
            discreteFuzzySetribbonPageGroup.Name = "discreteFuzzySetribbonPageGroup";
            discreteFuzzySetribbonPageGroup.State = DevExpress.XtraBars.Ribbon.RibbonPageGroupState.Expanded;
            discreteFuzzySetribbonPageGroup.Text = "Discrete Fuzzy Set";
            // 
            // continuousFuzzySetRibbonPageGroup
            // 
            continuousFuzzySetRibbonPageGroup.AllowTextClipping = false;
            continuousFuzzySetRibbonPageGroup.ItemLinks.Add(iAddContinuous);
            continuousFuzzySetRibbonPageGroup.Name = "continuousFuzzySetRibbonPageGroup";
            continuousFuzzySetRibbonPageGroup.State = DevExpress.XtraBars.Ribbon.RibbonPageGroupState.Expanded;
            continuousFuzzySetRibbonPageGroup.Text = "Continuous Fuzzy Set";
            // 
            // SearchFuzzySetribbonPageGroup
            // 
            SearchFuzzySetribbonPageGroup.ItemLinks.Add(iSearchFuzzySet);
            SearchFuzzySetribbonPageGroup.ItemLinks.Add(iExportFS);
            SearchFuzzySetribbonPageGroup.ItemLinks.Add(iImportFS);
            SearchFuzzySetribbonPageGroup.Name = "SearchFuzzySetribbonPageGroup";
            SearchFuzzySetribbonPageGroup.Text = "Fuzzy Set";
            // 
            // ribbonStatusBar
            // 
            ribbonStatusBar.Location = new System.Drawing.Point(0, 569);
            ribbonStatusBar.Name = "ribbonStatusBar";
            ribbonStatusBar.Ribbon = ribbonControl;
            ribbonStatusBar.Size = new System.Drawing.Size(970, 30);
            // 
            // TreeView_imageList
            // 
            TreeView_imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            TreeView_imageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("TreeView_imageList.ImageStream");
            TreeView_imageList.TransparentColor = System.Drawing.Color.Transparent;
            TreeView_imageList.Images.SetKeyName(0, "open.jpg");
            TreeView_imageList.Images.SetKeyName(1, "close.png");
            TreeView_imageList.Images.SetKeyName(2, "attribute.png");
            TreeView_imageList.Images.SetKeyName(3, "database.png");
            TreeView_imageList.Images.SetKeyName(4, "folder.png");
            TreeView_imageList.Images.SetKeyName(5, "key.png");
            TreeView_imageList.Images.SetKeyName(6, "open.png");
            TreeView_imageList.Images.SetKeyName(7, "relation.jpg");
            TreeView_imageList.Images.SetKeyName(8, "schema.png");
            // 
            // RelationsplitContainerControl
            // 
            RelationsplitContainerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            RelationsplitContainerControl.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            RelationsplitContainerControl.Location = new System.Drawing.Point(0, 193);
            RelationsplitContainerControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RelationsplitContainerControl.Name = "RelationsplitContainerControl";
            RelationsplitContainerControl.Padding = new System.Windows.Forms.Padding(10);
            // 
            // RelationsplitContainerControl.Panel1
            // 
            RelationsplitContainerControl.Panel1.Controls.Add(treeView);
            RelationsplitContainerControl.Panel1.Text = "Panel1";
            // 
            // RelationsplitContainerControl.Panel2
            // 
            RelationsplitContainerControl.Panel2.Controls.Add(xtraTabControlDatabase);
            RelationsplitContainerControl.Panel2.Text = "Panel2";
            RelationsplitContainerControl.Size = new System.Drawing.Size(970, 376);
            RelationsplitContainerControl.SplitterPosition = 250;
            RelationsplitContainerControl.TabIndex = 5;
            // 
            // treeView
            // 
            treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            treeView.ImageIndex = 0;
            treeView.ImageList = TreeView_imageList;
            treeView.ItemHeight = 22;
            treeView.Location = new System.Drawing.Point(0, 0);
            treeView.Name = "treeView";
            treeView.SelectedImageIndex = 0;
            treeView.Size = new System.Drawing.Size(250, 356);
            treeView.TabIndex = 2;
            treeView.MouseDown += treeView_MouseDown;
            // 
            // xtraTabControlDatabase
            // 
            xtraTabControlDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            xtraTabControlDatabase.Location = new System.Drawing.Point(0, 0);
            xtraTabControlDatabase.Name = "xtraTabControlDatabase";
            xtraTabControlDatabase.SelectedTabPage = SchemaxtraTabPage;
            xtraTabControlDatabase.Size = new System.Drawing.Size(688, 356);
            xtraTabControlDatabase.TabIndex = 1;
            xtraTabControlDatabase.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] { SchemaxtraTabPage, RelationxtraTabPage });
            xtraTabControlDatabase.SelectedPageChanged += xtraTabControlDatabase_SelectedPageChanged;
            xtraTabControlDatabase.CloseButtonClick += xtraTabControlDatabase_CloseButtonClick;
            // 
            // SchemaxtraTabPage
            // 
            SchemaxtraTabPage.Controls.Add(gridControlScheme);
            SchemaxtraTabPage.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            SchemaxtraTabPage.Name = "SchemaxtraTabPage";
            SchemaxtraTabPage.Size = new System.Drawing.Size(686, 326);
            SchemaxtraTabPage.Text = "Schema";
            // 
            // gridControlScheme
            // 
            gridControlScheme.Dock = System.Windows.Forms.DockStyle.Fill;
            gridControlScheme.Location = new System.Drawing.Point(0, 0);
            gridControlScheme.MainView = gridView;
            gridControlScheme.MenuManager = ribbonControl;
            gridControlScheme.Name = "gridControlScheme";
            gridControlScheme.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemCalcEdit1, repositoryItemButtonEdit1, repositoryItemHeightType1, repositoryItemCheckEdit1 });
            gridControlScheme.Size = new System.Drawing.Size(686, 326);
            gridControlScheme.TabIndex = 0;
            gridControlScheme.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView });
            // 
            // gridView
            // 
            gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { gridColumnPrimary, gridColumnAttribute, gridColumnDataType, gridColumnLength });
            gridView.GridControl = gridControlScheme;
            gridView.Name = "gridView";
            gridView.OptionsBehavior.Editable = false;
            gridView.OptionsPrint.PrintFilterInfo = true;
            gridView.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumnPrimary
            // 
            gridColumnPrimary.Caption = "Primary Key";
            gridColumnPrimary.ColumnEdit = repositoryItemCheckEdit1;
            gridColumnPrimary.FieldName = "isPrimaryKey";
            gridColumnPrimary.MinWidth = 25;
            gridColumnPrimary.Name = "gridColumnPrimary";
            gridColumnPrimary.Visible = true;
            gridColumnPrimary.VisibleIndex = 0;
            gridColumnPrimary.Width = 249;
            // 
            // repositoryItemCheckEdit1
            // 
            repositoryItemCheckEdit1.AutoHeight = false;
            repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // gridColumnAttribute
            // 
            gridColumnAttribute.Caption = "Attribute name";
            gridColumnAttribute.FieldName = "attributeName";
            gridColumnAttribute.MinWidth = 25;
            gridColumnAttribute.Name = "gridColumnAttribute";
            gridColumnAttribute.Visible = true;
            gridColumnAttribute.VisibleIndex = 1;
            gridColumnAttribute.Width = 245;
            // 
            // gridColumnDataType
            // 
            gridColumnDataType.Caption = "Data Type";
            gridColumnDataType.FieldName = "dataType";
            gridColumnDataType.MinWidth = 25;
            gridColumnDataType.Name = "gridColumnDataType";
            gridColumnDataType.Visible = true;
            gridColumnDataType.VisibleIndex = 2;
            gridColumnDataType.Width = 247;
            // 
            // gridColumnLength
            // 
            gridColumnLength.Caption = "Length";
            gridColumnLength.FieldName = "length";
            gridColumnLength.MinWidth = 25;
            gridColumnLength.Name = "gridColumnLength";
            gridColumnLength.Visible = true;
            gridColumnLength.VisibleIndex = 3;
            gridColumnLength.Width = 94;
            // 
            // repositoryItemCalcEdit1
            // 
            repositoryItemCalcEdit1.AutoHeight = false;
            repositoryItemCalcEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton() });
            repositoryItemCalcEdit1.Name = "repositoryItemCalcEdit1";
            // 
            // repositoryItemButtonEdit1
            // 
            repositoryItemButtonEdit1.AutoHeight = false;
            repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton() });
            repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // repositoryItemHeightType1
            // 
            repositoryItemHeightType1.AutoHeight = false;
            repositoryItemHeightType1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton() });
            repositoryItemHeightType1.Name = "repositoryItemHeightType1";
            // 
            // RelationxtraTabPage
            // 
            RelationxtraTabPage.Controls.Add(splitContainerControl2);
            RelationxtraTabPage.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            RelationxtraTabPage.Name = "RelationxtraTabPage";
            RelationxtraTabPage.Size = new System.Drawing.Size(686, 326);
            RelationxtraTabPage.Text = "Relation";
            // 
            // splitContainerControl2
            // 
            splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerControl2.Horizontal = false;
            splitContainerControl2.Location = new System.Drawing.Point(0, 0);
            splitContainerControl2.Name = "splitContainerControl2";
            // 
            // splitContainerControl2.Panel1
            // 
            splitContainerControl2.Panel1.Controls.Add(gridControlRelation);
            splitContainerControl2.Panel1.Text = "Panel1";
            // 
            // splitContainerControl2.Panel2
            // 
            splitContainerControl2.Panel2.Controls.Add(gridControlValueRelation);
            splitContainerControl2.Panel2.Controls.Add(panelControlRelation);
            splitContainerControl2.Panel2.Text = "Panel2";
            splitContainerControl2.Size = new System.Drawing.Size(686, 326);
            splitContainerControl2.SplitterPosition = 167;
            splitContainerControl2.TabIndex = 0;
            // 
            // gridControlRelation
            // 
            gridControlRelation.Dock = System.Windows.Forms.DockStyle.Fill;
            gridControlRelation.Location = new System.Drawing.Point(0, 0);
            gridControlRelation.MainView = gridView3;
            gridControlRelation.MenuManager = ribbonControl;
            gridControlRelation.Name = "gridControlRelation";
            gridControlRelation.Size = new System.Drawing.Size(686, 167);
            gridControlRelation.TabIndex = 0;
            gridControlRelation.UseEmbeddedNavigator = true;
            gridControlRelation.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView3 });
            gridControlRelation.Click += gridControlRelation_Click;
            // 
            // gridView3
            // 
            gridView3.GridControl = gridControlRelation;
            gridView3.Name = "gridView3";
            gridView3.OptionsView.ShowGroupPanel = false;
            gridView3.FocusedRowChanged += gridView3_FocusedRowChanged;
            gridView3.FocusedColumnChanged += gridView3_FocusedColumnChanged;
            // 
            // gridControlValueRelation
            // 
            gridControlValueRelation.Dock = System.Windows.Forms.DockStyle.Fill;
            gridControlValueRelation.Location = new System.Drawing.Point(0, 45);
            gridControlValueRelation.MainView = gridView4;
            gridControlValueRelation.MenuManager = ribbonControl;
            gridControlValueRelation.Name = "gridControlValueRelation";
            gridControlValueRelation.Size = new System.Drawing.Size(686, 102);
            gridControlValueRelation.TabIndex = 1;
            gridControlValueRelation.UseEmbeddedNavigator = true;
            gridControlValueRelation.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView4 });
            // 
            // gridView4
            // 
            gridView4.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { gridColumnValue, gridColumnMinProb, gridColumnMaxProb });
            gridView4.GridControl = gridControlValueRelation;
            gridView4.Name = "gridView4";
            gridView4.OptionsView.ShowGroupPanel = false;
            gridView4.InvalidRowException += gridView4_InvalidRowException;
            gridView4.RowDeleting += gridView4_RowDeleting;
            gridView4.ValidateRow += gridView4_ValidateRow;
            // 
            // gridColumnValue
            // 
            gridColumnValue.Caption = "Value";
            gridColumnValue.FieldName = "Value";
            gridColumnValue.MinWidth = 25;
            gridColumnValue.Name = "gridColumnValue";
            gridColumnValue.Visible = true;
            gridColumnValue.VisibleIndex = 0;
            gridColumnValue.Width = 94;
            // 
            // gridColumnMinProb
            // 
            gridColumnMinProb.Caption = "MinProb";
            gridColumnMinProb.FieldName = "MinProb";
            gridColumnMinProb.MinWidth = 25;
            gridColumnMinProb.Name = "gridColumnMinProb";
            gridColumnMinProb.Visible = true;
            gridColumnMinProb.VisibleIndex = 1;
            gridColumnMinProb.Width = 94;
            // 
            // gridColumnMaxProb
            // 
            gridColumnMaxProb.AccessibleName = "gridColumnMaxProb";
            gridColumnMaxProb.Caption = "MaxProb";
            gridColumnMaxProb.FieldName = "MaxProb";
            gridColumnMaxProb.MinWidth = 25;
            gridColumnMaxProb.Name = "gridColumnMaxProb";
            gridColumnMaxProb.Visible = true;
            gridColumnMaxProb.VisibleIndex = 2;
            gridColumnMaxProb.Width = 94;
            // 
            // panelControlRelation
            // 
            panelControlRelation.Dock = System.Windows.Forms.DockStyle.Top;
            panelControlRelation.Location = new System.Drawing.Point(0, 0);
            panelControlRelation.Name = "panelControlRelation";
            panelControlRelation.Size = new System.Drawing.Size(686, 45);
            panelControlRelation.TabIndex = 0;
            // 
            // popupMenuTreeView
            // 
            popupMenuTreeView.ItemLinks.Add(barButtonSelectTuples);
            popupMenuTreeView.ItemLinks.Add(barButtonRelationships);
            popupMenuTreeView.Name = "popupMenuTreeView";
            popupMenuTreeView.Ribbon = ribbonControl;
            // 
            // ContextMenu_RelationNode
            // 
            ContextMenu_RelationNode.Font = new System.Drawing.Font("Lucida Sans Unicode", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            ContextMenu_RelationNode.ImageScalingSize = new System.Drawing.Size(20, 20);
            ContextMenu_RelationNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { CTMenuRelNode_OpenRelation, CTMenuRelNode_DeleteRelation, CTMenuRelNode_RenameRelation });
            ContextMenu_RelationNode.Name = "ContextMenu_RelationNode";
            ContextMenu_RelationNode.Size = new System.Drawing.Size(148, 82);
            // 
            // CTMenuRelNode_OpenRelation
            // 
            CTMenuRelNode_OpenRelation.Name = "CTMenuRelNode_OpenRelation";
            CTMenuRelNode_OpenRelation.Size = new System.Drawing.Size(147, 26);
            CTMenuRelNode_OpenRelation.Text = "&Open";
            CTMenuRelNode_OpenRelation.ToolTipText = "Tạo quan hệ mới";
            // 
            // CTMenuRelNode_DeleteRelation
            // 
            CTMenuRelNode_DeleteRelation.Name = "CTMenuRelNode_DeleteRelation";
            CTMenuRelNode_DeleteRelation.Size = new System.Drawing.Size(147, 26);
            CTMenuRelNode_DeleteRelation.Text = "&Delete";
            // 
            // CTMenuRelNode_RenameRelation
            // 
            CTMenuRelNode_RenameRelation.Name = "CTMenuRelNode_RenameRelation";
            CTMenuRelNode_RenameRelation.Size = new System.Drawing.Size(147, 26);
            CTMenuRelNode_RenameRelation.Text = "&Rename";
            // 
            // ContextMenu_SchemaNode
            // 
            ContextMenu_SchemaNode.ImageScalingSize = new System.Drawing.Size(20, 20);
            ContextMenu_SchemaNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { CTMenuSchNode_EditSchema, CTMenuSchNode_OpenSchema, CTMenuSchNode_DeleteSchema });
            ContextMenu_SchemaNode.Name = "ContextMenu_SchemaNode";
            ContextMenu_SchemaNode.Size = new System.Drawing.Size(123, 76);
            // 
            // CTMenuSchNode_EditSchema
            // 
            CTMenuSchNode_EditSchema.Name = "CTMenuSchNode_EditSchema";
            CTMenuSchNode_EditSchema.Size = new System.Drawing.Size(122, 24);
            CTMenuSchNode_EditSchema.Text = "&Edit";
            // 
            // CTMenuSchNode_OpenSchema
            // 
            CTMenuSchNode_OpenSchema.Name = "CTMenuSchNode_OpenSchema";
            CTMenuSchNode_OpenSchema.Size = new System.Drawing.Size(122, 24);
            CTMenuSchNode_OpenSchema.Text = "&Open";
            // 
            // CTMenuSchNode_DeleteSchema
            // 
            CTMenuSchNode_DeleteSchema.Name = "CTMenuSchNode_DeleteSchema";
            CTMenuSchNode_DeleteSchema.Size = new System.Drawing.Size(122, 24);
            CTMenuSchNode_DeleteSchema.Text = "&Delete";
            // 
            // ribbonPageGroup2
            // 
            ribbonPageGroup2.ItemLinks.Add(barButtonItem2);
            ribbonPageGroup2.Name = "ribbonPageGroup2";
            ribbonPageGroup2.Text = "Probabilistic Interpretation For Selection Expression";
            // 
            // barButtonItem2
            // 
            barButtonItem2.Caption = "Calculate";
            barButtonItem2.Id = 60;
            barButtonItem2.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("barButtonItem2.ImageOptions.SvgImage");
            barButtonItem2.Name = "barButtonItem2";
            barButtonItem2.ItemClick += barButtonItem2_ItemClick;
            // 
            // frmMain
            // 
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(970, 599);
            Controls.Add(RelationsplitContainerControl);
            Controls.Add(ribbonStatusBar);
            Controls.Add(ribbonControl);
            Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            IconOptions.Image = (System.Drawing.Image)resources.GetObject("frmMain.IconOptions.Image");
            IsMdiContainer = true;
            Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            Name = "frmMain";
            Ribbon = ribbonControl;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            StatusBar = ribbonStatusBar;
            Text = "FPRDB Visual Management System";
            FormClosing += frmMain_FormClosing;
            ((System.ComponentModel.ISupportInitialize)ribbonControl).EndInit();
            ((System.ComponentModel.ISupportInitialize)RelationsplitContainerControl.Panel1).EndInit();
            RelationsplitContainerControl.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)RelationsplitContainerControl.Panel2).EndInit();
            RelationsplitContainerControl.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)RelationsplitContainerControl).EndInit();
            RelationsplitContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)xtraTabControlDatabase).EndInit();
            xtraTabControlDatabase.ResumeLayout(false);
            SchemaxtraTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControlScheme).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemCheckEdit1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemCalcEdit1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemButtonEdit1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemHeightType1).EndInit();
            RelationxtraTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerControl2.Panel1).EndInit();
            splitContainerControl2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerControl2.Panel2).EndInit();
            splitContainerControl2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerControl2).EndInit();
            splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControlRelation).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView3).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridControlValueRelation).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView4).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelControlRelation).EndInit();
            ((System.ComponentModel.ISupportInitialize)popupMenuTreeView).EndInit();
            ContextMenu_RelationNode.ResumeLayout(false);
            ContextMenu_SchemaNode.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
        private DevExpress.XtraBars.Ribbon.RibbonPage DatabaseRibbonPage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup groupFile_pageHome;
        private DevExpress.XtraBars.Ribbon.RibbonPage pageFuzzySet;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup discreteFuzzySetribbonPageGroup;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup groupExit_pageHome;
        private DevExpress.XtraBars.BarButtonItem buttonExit_pageHome;
        private DevExpress.XtraBars.BarButtonItem buttonNew_pageHome;
        private DevExpress.XtraBars.BarButtonItem buttonOpen_pageHome;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup continuousFuzzySetRibbonPageGroup;
        private DevExpress.XtraBars.BarButtonItem iAddDiscrete;
        private DevExpress.XtraBars.BarButtonItem iAddContinuous;
        private DevExpress.XtraBars.Ribbon.RibbonPage SchemaRibbonPage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup fileSchemaRibbonPageGroup;
        private DevExpress.XtraBars.Ribbon.RibbonPage RelationRibbonPage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup fileRelationRibbonPageGroup;
        private DevExpress.XtraBars.Ribbon.RibbonPage QueryRibbonPage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup fileQueryRibbonPageGroup;
        private System.Windows.Forms.ImageList TreeView_imageList;
        private DevExpress.XtraBars.BarButtonItem iNewSchema;
        private DevExpress.XtraBars.BarButtonItem iDeleteSchema;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup SearchFuzzySetribbonPageGroup;
        private DevExpress.XtraBars.BarButtonItem iSearchFuzzySet;
        private DevExpress.XtraBars.BarButtonItem iNewRelation;
        private DevExpress.XtraBars.BarButtonItem iDeleteRelation;
        private DevExpress.XtraBars.BarButtonItem iCloseRelation;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup conjunctionRibbonPageGroup;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup disjunctionRibbonPageGroup;
        private DevExpress.XtraBars.BarButtonItem iNewQuery;
        private DevExpress.XtraBars.BarButtonItem iOpenQuery;
        private DevExpress.XtraBars.BarButtonItem iSaveQuery;
        private DevExpress.XtraBars.BarButtonItem iConjunctionIgnorance;
        private DevExpress.XtraBars.BarButtonItem iConjunctionIndependence;
        private DevExpress.XtraBars.BarButtonItem iConjunctionMutual;
        private DevExpress.XtraBars.BarButtonItem iDisjunctionIgnorance;
        private DevExpress.XtraBars.BarButtonItem iDisjunctionIndependence;
        private DevExpress.XtraBars.BarButtonItem iDisjunctionMutual;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup differenceRibbonPageGroup;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup operatorRibbonPageGroup;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup excuteQueryribbonPageGroup;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraEditors.SplitContainerControl RelationsplitContainerControl;
        private System.Windows.Forms.TreeView treeView;
        private DevExpress.XtraTab.XtraTabControl xtraTabControlDatabase;
        private DevExpress.XtraTab.XtraTabPage SchemaxtraTabPage;
        private DevExpress.XtraTab.XtraTabPage RelationxtraTabPage;
        private DevExpress.XtraGrid.GridControl gridControlScheme;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraBars.BarButtonItem iDifferenceIgnorance;
        private DevExpress.XtraBars.BarButtonItem iDifferenceIndependence;
        private DevExpress.XtraBars.BarButtonItem iDiferenceMutual;
        private DevExpress.XtraBars.BarButtonItem iOperator;
        private DevExpress.XtraBars.BarButtonItem iExcuteQuery;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private DevExpress.XtraGrid.GridControl gridControlRelation;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraGrid.GridControl gridControlValueRelation;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraEditors.PanelControl panelControlRelation;
        private DevExpress.XtraBars.BarButtonItem iConjunctionPositive;
        private DevExpress.XtraBars.BarButtonItem iDisjunctionPositive;
        private DevExpress.XtraBars.BarButtonItem iDifferencePositive;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnPrimary;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnAttribute;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDataType;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraRichEdit.Design.RepositoryItemHeightType repositoryItemHeightType1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnValue;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnMinProb;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnMaxProb;
        private DevExpress.XtraBars.BarButtonGroup barButtonGroup1;
        private DevExpress.XtraBars.BarButtonGroup barButtonGroup2;
        private DevExpress.XtraBars.BarButtonItem barButtonSelectTuples;
        private DevExpress.XtraBars.PopupMenu popupMenuTreeView;
        private System.Windows.Forms.ContextMenuStrip ContextMenu_RelationNode;
        private System.Windows.Forms.ToolStripMenuItem CTMenuRelNode_OpenRelation;
        private System.Windows.Forms.ToolStripMenuItem CTMenuRelNode_DeleteRelation;
        private System.Windows.Forms.ToolStripMenuItem CTMenuRelNode_RenameRelation;
        private System.Windows.Forms.ContextMenuStrip ContextMenu_SchemaNode;
        private System.Windows.Forms.ToolStripMenuItem CTMenuSchNode_EditSchema;
        private System.Windows.Forms.ToolStripMenuItem CTMenuSchNode_OpenSchema;
        private System.Windows.Forms.ToolStripMenuItem CTMenuSchNode_DeleteSchema;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnLength;
        private DevExpress.XtraBars.BarButtonItem iExportFS;
        private DevExpress.XtraBars.BarButtonItem iImportFS;
        private DevExpress.XtraBars.BarButtonItem barButtonRelationships;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
    }
}

