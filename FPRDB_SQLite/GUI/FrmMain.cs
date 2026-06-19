using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Services;
using BLL.SQLProcessing;
using DevExpress.DataAccess.UI.Native.Sql;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.Design;
using DevExpress.XtraRichEdit.Import.Html;
using DevExpress.XtraTab;
using FPRDB_SQLite.GUI.GUI;
using FPRDB_SQLite.GUI.GUI.UserControls;
using GUI.GlobalStates;
using GUI.GUI;
//using GUI.GUI.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static FPRDB_SQLite.GUI.frmNewSchema;
using GUI.HandlingException;

namespace FPRDB_SQLite.GUI
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private CompositionRoot compRoot;
        private DatabaseService databaseService;
        private FPRDBSchemaService fprdbSchemaSerivce;
        private FPRDBRelationService fprdbRelationService;
        private InDataBaseSQLFileService inDataBaseSQLFileService;
        private string fprdbDotExtension;
        //private SQLFileService sqlFileService;
        private bool isDatabaseLoaded = false;
        private string currentSQLFilePath = string.Empty;
        private bool isSQLFileModified = false;
        private SQLProcessor sqlProcessor;
        private FPRDBRelationDTO _selectedRelation;
        private FPRDBSchemaDTO _selectedSchema;
        private string _currentEditingColumn;
        private int _currentEditingRow;
        private string _currentEditingColumnType;
        private bool _isOpeningFuzzySet = false;

        private Field selectedField;
        private FieldType selectedFieldType;
        private bool isSelectedFieldText;

        private int sqlQueryCount = 1;
        public frmMain(CompositionRoot compRoot)
        {
            this.compRoot = compRoot;
            this.databaseService = this.compRoot.getDatabaseService();
            this.fprdbDotExtension = this.databaseService.getFPRDBDotExtenstion();
            this.sqlProcessor = this.compRoot.getSQLProcessor();
            this.fprdbSchemaSerivce = compRoot.getFPRDBSchemaService();
            this.fprdbRelationService = compRoot.getFPRDBRelationService();
            this.inDataBaseSQLFileService = compRoot.getInDataBaseSQLFileService();
            //this.sqlFileService = this.compRoot.getSQLFileService();
            InitializeComponent();
            changeStatusTab();
            xtraTabControlDatabase.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InAllTabPageHeaders;

            // Duyệt qua các Tab hiện có (như Tab Schema, Tab Home) và ẩn nút x đi
            foreach (XtraTabPage page in xtraTabControlDatabase.TabPages)
            {
                page.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            }

            // Set fixed panel so navigation tree width remains constant during window resizing/maximizing
            RelationsplitContainerControl.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel1;

            // Move treeView out of layoutControl1 to Panel1 directly so it docks Fill and auto-stretches without leaving blank spaces
            layoutControlItem1.Control = null;
            layoutControl1.Controls.Remove(treeView);
            layoutControl1.Dispose();
            RelationsplitContainerControl.Panel1.Controls.Clear();
            RelationsplitContainerControl.Panel1.Controls.Add(treeView);
            treeView.Dock = DockStyle.Fill;
            treeView.BringToFront();
            RelationsplitContainerControl.Panel1.PerformLayout();
            RelationsplitContainerControl.SplitterPosition = 350;

            // Limit manual splitter dragging to not exceed 600px
            RelationsplitContainerControl.SplitterPositionChanged += RelationsplitContainerControl_SplitterPositionChanged;

            // Set main form window state to maximized to always open in full screen
            this.WindowState = FormWindowState.Maximized;
        }

        private bool _isEnforcingSplitterLimit = false;
        private void RelationsplitContainerControl_SplitterPositionChanged(object sender, EventArgs e)
        {
            if (_isEnforcingSplitterLimit) return;
            const int MaxSplitterPosition = 350; // 350px maximum width
            if (RelationsplitContainerControl.SplitterPosition > MaxSplitterPosition)
            {
                try
                {
                    _isEnforcingSplitterLimit = true;
                    RelationsplitContainerControl.SplitterPosition = MaxSplitterPosition;
                }
                finally
                {
                    _isEnforcingSplitterLimit = false;
                }
            }
        }
        // Hàm để enable/disable tab và các nút khi load database
        private void changeStatusTab()
        {
            if (!isDatabaseLoaded)
            {
                SchemaRibbonPage.Visible = false;
                pageFuzzySet.Visible = false;
                RelationRibbonPage.Visible = false;    // Tab "Relation"
                QueryRibbonPage.Visible = false;    // Tab "Query"
                xtraTabControlDatabase.Visible = false;
            }
            else
            {
                SchemaRibbonPage.Visible = true;
                pageFuzzySet.Visible = true;
                RelationRibbonPage.Visible = true;    // Tab "Relation"
                QueryRibbonPage.Visible = true;    // Tab "Query"
                xtraTabControlDatabase.Visible = true;
                CreateQueryTab();
                SetQueryTabState(true);
            }
        }
        private string GetRootPath(string path)
        {
            // Hàm này tự động hiểu và lấy ra "C:\", "D:\"... một cách an toàn
            return System.IO.Path.GetPathRoot(path);
        }
        // Load dữ liệu từ chuỗi của Fuzzy Probalistic Value thành 1 bảng liệt kê giá trị
        private void LoadFuzzyProbalisticValueDetail(string probValue)
        {


            DataTable dt = new DataTable();
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("MinProb", typeof(string));
            dt.Columns.Add("MaxProb", typeof(string));

            if (probValue != null)
            {
                //// Liệt kê nội dung cần lấy
                //string content = probValue.Trim('{', '}');
                //// Liệt kê các value (1 value -> 1 tuple)
                //string[] tuples = content.Split(new string[] { "), (" }, StringSplitOptions.None);

                //// Kiểm tra từng value
                //foreach (string tuple in tuples)
                //{
                //    string clean = tuple.Trim('(', ')');
                //    int bracketIndex = clean.IndexOf(",[");
                //    if (bracketIndex < 0) continue;

                //    string value = clean.Substring(0, bracketIndex);

                //    //remove string literal delimiter " from the possible value
                //    //if (this.isSelectedFieldText)
                //    //    value = value.Trim('\"');

                //    string bounds = clean.Substring(bracketIndex + 2).Trim('[', ']');
                //    // Mảng: lowerBound -> index 0, upperBound -> index 1
                //    string[] boundParts = bounds.Split(',');
                //    if (boundParts.Length < 2) continue;


                //    dt.Rows.Add(value, boundParts[0].Trim(), boundParts[1].Trim());
                //}
                List<(string, string, string)> processedFProbValue = FuzzyProbabilisticValueUtilities.extractValuesIntervalProbabilitiesAsString(probValue);
                foreach (var (value, lowerProb, upperProb) in processedFProbValue)
                {
                    dt.Rows.Add(value, lowerProb, upperProb);
                }

            }

            dt.RowChanged += BottomGrid_RowChanged;
            dt.RowDeleted += BottomGrid_RowChanged;

            gridControlValueRelation.DataSource = dt;
            gridView4.BestFitColumns();

            GridColumn col = gridView4.Columns["Value"];
            if (_currentEditingColumnType == "DIST_FUZZYSET_INT"
                || _currentEditingColumnType == "DIST_FUZZYSET_FLOAT"
                || _currentEditingColumnType == "DIST_FUZZYSET_TEXT"
                || _currentEditingColumnType == "CONT_FUZZYSET")
            {
                col.OptionsColumn.AllowEdit = true;
                var btnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
                btnEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                btnEdit.Buttons.Clear();
                btnEdit.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(
                    DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis));

                gridView4.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
                gridControlValueRelation.RepositoryItems.Add(btnEdit);

                btnEdit.ButtonClick += (sender, e) =>
                {
                    var frm = new frmListFuzzySet(compRoot, selectedFieldType);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFSName = frm.selectedFSName;
                        if (selectedFSName != null || selectedFSName != default)
                            gridView4.SetRowCellValue(gridView4.FocusedRowHandle, "Value", selectedFSName);
                    }
                };

                col.ColumnEdit = btnEdit;
            }
            else
            {
                col.ColumnEdit = null;
                col.OptionsColumn.AllowEdit = true;
            }
        }
        // Hàm xử lý khi người dùng nhấp ra khỏi hàng ở Grid liệt kê giá trị
        private void BottomGrid_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action != DataRowAction.Add &&
                e.Action != DataRowAction.Change &&
                e.Action != DataRowAction.Delete) return;
            string newFuzzyString = BuildFuzzyStringFromBottomGrid();
            int rowHandle = gridView3.GetRowHandle(_currentEditingRow);

            if (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
            {
                gridView3.SetRowCellValue(rowHandle, gridView3.Columns[_currentEditingColumn], newFuzzyString);

                gridView3.FocusedRowHandle = rowHandle;
                gridView3.ShowEditor();

                if (gridView3.ActiveEditor != null)
                {
                    gridView3.ActiveEditor.IsModified = true;
                }
            }
        }
        // Hàm kiểm tra dữ liệu hàng khi người dùng nhấp ra ngoài
        private void gridView4_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            var view = sender as GridView;
            if (view == null) return;

            foreach (GridColumn col in view.Columns)
            {
                var val = view.GetRowCellValue(e.RowHandle, col);
                if (val == null || val == DBNull.Value ||
                    string.IsNullOrWhiteSpace(val.ToString()))
                {
                    e.Valid = false;
                    e.ErrorText = $"{col.Caption} cannot be empty!";
                    return;
                }
            }
        }
        // Hàm thông báo cho Grid liệt kê giá trị
        private void gridView4_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
            MessageBox.Show(e.ErrorText, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        // Parse giá trị ngược lại cho Grid relation ở trên (khi có cập nhật)
        private string BuildFuzzyStringFromBottomGrid()
        {
            DataTable dt = gridControlValueRelation.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0) return "";

            StringBuilder sb = new StringBuilder("{");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (row.RowState == DataRowState.Deleted) continue;

                string value = row["Value"].ToString();

                //add string literal delimiter
                //if (this.isSelectedFieldText)
                //    value = "\"" + value + "\"";

                string minProb = row["MinProb"].ToString();
                string maxProb = row["MaxProb"].ToString();

                if (string.IsNullOrEmpty(value)) continue;

                sb.Append($"({value},[{minProb},{maxProb}])");
                if (i < dt.Rows.Count - 1)
                    sb.Append(", ");
            }
            sb.Append("}");
            return sb.ToString();
        }
        #region Tab Page Fuzzy Set
        private void buttonAdd_groupDis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                new frmAddDiscreteFuzzySet(compRoot).ShowDialog();
            }
            catch(InvalidOperationException ex)
            {
                HandlingExceptionViaErrorNotification.handlingInvalidOperationException(new InvalidOperationException("Database isn't loaded"));
            }
        }

        private void buttonAdd_groupCont_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                new frmAddContinuousFuzzySet(compRoot).ShowDialog();
            }
            catch(InvalidOperationException ex)
            {
                HandlingExceptionViaErrorNotification.handlingInvalidOperationException(new InvalidOperationException("Database isn't loaded"));
            }
        }
        private void iSearchFuzzySet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                new frmManageFuzzySet(compRoot).ShowDialog();
            }
            catch(InvalidOperationException ex)
            {
                HandlingExceptionViaErrorNotification.handlingInvalidOperationException(new InvalidOperationException("Database isn't loaded"));
            }
        }
        #endregion
        #region Tab Page Home
        //Method to check if schema is existed
        private bool IsSchemaExisted(string schemaName)
        {
            var schemas = AppStates.loadFPRDBSchemas;
            if (schemas == null) return false;
            return schemas.Any(s => s.schemaName.Equals(schemaName, StringComparison.OrdinalIgnoreCase));
        }
        // Hàm hiển thị thông tin chi tiết của Schema
        private void DisplaySchemaDetail(FPRDBSchemaDTO schema)
        {
            if (!IsSchemaExisted(schema.schemaName))
            {
                gridControlScheme.DataSource = null;
                XtraTabPage schemaTab = xtraTabControlDatabase.TabPages[0];
                schemaTab.Text = "Schema";
                return;
            }
            BindingList<SchemaAttribute> list = new BindingList<SchemaAttribute>();
            List<Field> fields = schema.fields;
            List<string> primaryKeys = schema.primarykey;
            var sortedFields = fields
                .OrderByDescending(attr => primaryKeys.Contains(attr.getFieldName()))
                .ThenBy(attr => attr.getFieldName());
            foreach (var attr in sortedFields)
            {
                var fielInfo = attr.getFieldInfo();
                list.Add(new SchemaAttribute
                {
                    isPrimaryKey = primaryKeys.Contains(attr.getFieldName()),
                    attributeName = attr.getFieldName(),
                    dataType = FieldTypeUtilities.fromFieldTypeEnumToSQLFieldType(fielInfo.getType()),
                    length = fielInfo.getTXTLength()
                });
            }
            gridControlScheme.DataSource = null;
            gridView.Columns.Clear();
            gridControlScheme.DataSource = list;
            gridView.BestFitColumns();
        }
        private DataRow getTupleAsDataRow(List<Field> schemaFields, Scan s, DataTable table)
        {
            DataRow row = table.NewRow();
            Type domainType;
            string fieldName;
            foreach (Field f in schemaFields)
            {
                domainType = FieldTypeUtilities.getDomainType(f.getFieldInfo().getType());
                fieldName = f.getFieldName();
                if (domainType == typeof(int))
                {
                    FuzzyProbabilisticValue<int> fprobValue = s.getFieldContent<int>(fieldName);
                    row[fieldName] = fprobValue.ToString();
                }
                else if (domainType == typeof(float))
                {
                    FuzzyProbabilisticValue<float> fprobValue = s.getFieldContent<float>(fieldName);
                    row[fieldName] = fprobValue.ToString();
                }
                else if (domainType == typeof(string))
                {
                    FuzzyProbabilisticValue<string> fprobValue = s.getFieldContent<string>(fieldName);
                    row[fieldName] = fprobValue.ToString();
                }
                else //if (domainType == typeof(bool))
                {
                    FuzzyProbabilisticValue<bool> fprobValue = s.getFieldContent<bool>(fieldName);
                    row[fieldName] = fprobValue.ToString();
                }
            }
            return row;
        }
        // Method to reload the tabs of schema and relation after modifying
        private void ReloadTabs()
        {
            if (_selectedRelation != null)
            {
                var updatedRelation = AppStates.loadFPRDBSchemaRelations?.FirstOrDefault(r => r.relName.Equals(_selectedRelation.relName, StringComparison.OrdinalIgnoreCase));
                if (updatedRelation != null)
                {
                    _selectedRelation = updatedRelation;
                    DisplayRelationDetail(_selectedRelation);
                }
                else
                {
                    DisplayRelationDetail(_selectedRelation);
                    _selectedRelation = null;
                }
            }
            if (_selectedSchema != null)
            {
                var updatedSchema = AppStates.loadFPRDBSchemas?.FirstOrDefault(s => s.schemaName.Equals(_selectedSchema.schemaName, StringComparison.OrdinalIgnoreCase));
                if (updatedSchema != null)
                {
                    _selectedSchema = updatedSchema;
                    DisplaySchemaDetail(_selectedSchema);
                }
                else
                {
                    DisplaySchemaDetail(_selectedSchema);
                    _selectedSchema = null;
                }
            }
        }
        // Method to check if relation is existed
        private bool IsRelationExisted(string relName)
        {
            var relations = AppStates.loadFPRDBSchemaRelations;
            if (relations == null) return false;
            return relations.Any(r => r.relName.Equals(relName, StringComparison.OrdinalIgnoreCase));
        }
        // Hàm hiển thị nội dung của Relation
        private void DisplayRelationDetail(FPRDBRelationDTO relInfo)
        {
            if (!IsRelationExisted(relInfo.relName))
            {
                gridControlRelation.DataSource = null;
                XtraTabPage relationTab = xtraTabControlDatabase.TabPages[1];
                relationTab.Text = "Relation";
                return;
            }
            var schema = relInfo.fprdbSchema;
            List<Field> schemaFields = schema.fields;
            // Sử dụng DataTable để hiện thị thông tin Relation
            DataTable relationContent = new DataTable();

            foreach (var field in schemaFields)
            {
                string fieldName = field.getFieldName();
                DataColumn dataCol = new DataColumn(fieldName, typeof(string));
                relationContent.Columns.Add(dataCol);
            }

            //fake data
            //Dictionary<string, string> row1 = new Dictionary<string, string>
            //{
            //    {"id", "{(ID01,[1,1])}"},
            //    {"name", "{(John,[0.1,0.3]), (Mary,[0.4,0.6]), (Nick,[0.7,0.9])}" }
            //};
            //Dictionary<string, string> row2 = new Dictionary<string, string>
            //{
            //    {"id", "{(ID02,[1,1])}"},
            //    {"name", "{(Tom,[0.1,0.3]), (Anna,[0.4,0.6]), (James,[0.7,0.9])}" }
            //};
            //List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            //result.Add(row1);
            //result.Add(row2);
            // Xét từng dòng trong bảng result (giả dụ)
            //foreach (var row in result)
            //{
            //    DataRow fakeRow = relInfo.NewRow();
            //    // Lấy từng ô theo fielName
            //    foreach (var field in fields)
            //    {
            //        string fieldName = field.getFieldName();
            //        string content = "";
            //        // Lấy giá trị tại 1 ô
            //        if (row.TryGetValue(fieldName, out var value))
            //        {
            //            content = value?.ToString();
            //        }
            //        fakeRow[fieldName] = content;
            //    }
            //    relInfo.Rows.Add(fakeRow);
            //}

            //extract the top 100 tuples from the current selected relation for grid view
            //not done: Doesn’t have LIMIT OFFSET for “select top 100 row”
            Plan relPlan = new RelationPlan(relInfo.relName, this.compRoot.getMetaDataManger(), this.compRoot.getDBMgr(), this.compRoot.getParser(), this.compRoot.getConstraintService());
            Scan relScan = relPlan.open();
            int count = 0;
            while (relScan.next() && count < AppStates.maxSelectTop)
            {
                relationContent.Rows.Add(this.getTupleAsDataRow(schemaFields, relScan, relationContent));
                count++;
            }

            relationContent.AcceptChanges();

            // Unsubscribe to prevent premature focus event triggering when DataSource is set and columns are populated
            gridView3.FocusedRowChanged -= gridView3_FocusedRowChanged;
            gridView3.FocusedColumnChanged -= gridView3_FocusedColumnChanged;

            gridControlRelation.DataSource = relationContent;

            gridView3.Columns.Clear();
            gridView3.PopulateColumns();

            foreach (var field in schemaFields)
            {
                string fieldName = field.getFieldName();

                GridColumn col = gridView3.Columns[fieldName];
                if (col == null) continue;

                col.Caption = fieldName;
                col.OptionsColumn.AllowEdit = true;
                col.OptionsColumn.ReadOnly = true;
                col.Tag = field.getFieldInfo().getType();
            }

            gridView3.BestFitColumns();

            gridControlRelation.EmbeddedNavigator.ButtonClick -= Navigator_ButtonClick;
            gridControlRelation.EmbeddedNavigator.ButtonClick += Navigator_ButtonClick;

            // Re-subscribe to focus events
            gridView3.FocusedRowChanged += gridView3_FocusedRowChanged;
            gridView3.FocusedColumnChanged += gridView3_FocusedColumnChanged;

            // Manually trigger the focus logic once everything is successfully initialized
            gridView3_FocusedColumnChanged(null, null);
        }
        // Hàm xử lý sự kiện khi click "Select top 100 tuples"
        private void barButtonSelectTuples_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraTabPage relationTab = xtraTabControlDatabase.TabPages[1];
            relationTab.Text = _selectedRelation.relName;
            xtraTabControlDatabase.SelectedTabPageIndex = 1;
            DisplayRelationDetail(_selectedRelation);
            return;
        }
        // Hàm show popup menu cho node relation
        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode node = treeView.GetNodeAt(e.X, e.Y);
            if (node == null) return;
            treeView.SelectedNode = node;

            if (e.Button == MouseButtons.Right)
            {

                if (node.Tag is FPRDBRelationDTO relation)
                {
                    _selectedRelation = relation;
                    popupMenuTreeView.ShowPopup(treeView.PointToScreen(e.Location));
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (node.Tag is FPRDBSchemaDTO schema)
                {
                    _selectedSchema = schema;
                    XtraTabPage schemaTab = xtraTabControlDatabase.TabPages[0];
                    schemaTab.Text = schema.schemaName;
                    xtraTabControlDatabase.SelectedTabPageIndex = 0;
                    DisplaySchemaDetail(_selectedSchema);
                    ribbonControl.SelectedPage = SchemaRibbonPage;
                }
                if (node.Tag is FPRDBRelationDTO relation)
                {
                    _selectedRelation = relation;
                    XtraTabPage relationTab = xtraTabControlDatabase.TabPages[1];
                    relationTab.Text = _selectedRelation.relName;
                    xtraTabControlDatabase.SelectedTabPageIndex = 1;
                    DisplayRelationDetail(_selectedRelation);
                    ribbonControl.SelectedPage = RelationRibbonPage;
                }
                if (node.Tag != null && node.Tag.ToString() == "query")
                {
                    foreach (XtraTabPage page in xtraTabControlDatabase.TabPages)
                    {
                        // Bước 1: Kiểm tra đúng loại Tab Query dựa trên Tag
                        if (page.Tag != null && page.Tag.ToString() == "QueryTab")
                        {
                            // Bước 2: Truy cập vào UserControl bên trong Tab
                            var uc = page.Controls.OfType<ucQueryEditor>().FirstOrDefault();

                            if (uc != null && uc.IsDBFile)
                            {
                                // Bước 3: Kiểm tra có trùng không
                                if (node.Text == page.Text.Substring(0, page.Text.LastIndexOf('.')).Trim())
                                {
                                    xtraTabControlDatabase.SelectedTabPage = page;
                                    ribbonControl.SelectedPage = QueryRibbonPage;
                                    return;
                                }
                            }
                        }
                    }
                    CreateQueryTab(node.Text, this.inDataBaseSQLFileService.getFileContent(node.Text), "", true);
                    SetQueryTabState(true);
                }

            }
        }
        private void unLoadDatabaseTree()
        {
            treeView.Nodes.Clear();
        }
        // Hàm load cây Database sau khi mở hoặc tạo mới Database
        private void LoadDatabaseTree()
        {
            try
            {
                changeStatusTab();
                // 1. Xóa cây cũ
                treeView.Nodes.Clear();

                // 2. Lấy tên file Database thông qua databaseService
                string dbName = "DATABASE";
                if (!string.IsNullOrEmpty(this.databaseService.getDatabaseName()))
                {
                    dbName = this.databaseService.getDatabaseName();
                }

                if (string.IsNullOrEmpty(dbName) || dbName == "DATABASE") return;

                // 3. Tạo Node Root (VD: db1)
                TreeNode rootNode = new TreeNode(dbName);
                rootNode.ImageIndex = 3; // Icon Database
                rootNode.SelectedImageIndex = 3;
                treeView.Nodes.Add(rootNode);

                // ====================================================================
                // TẠO 2 THƯ MỤC CHA: Tables VÀ Relation (Ngang hàng nhau)
                // ====================================================================
                TreeNode tablesRootNode = new TreeNode("FPRDB Schemas");
                tablesRootNode.ImageIndex = 4; // Icon Folder màu vàng
                tablesRootNode.SelectedImageIndex = 4;
                rootNode.Nodes.Add(tablesRootNode);

                TreeNode relationRootNode = new TreeNode("Relation");
                relationRootNode.ImageIndex = 4; // Icon Folder màu vàng
                relationRootNode.SelectedImageIndex = 4;
                rootNode.Nodes.Add(relationRootNode);

                TreeNode queryRootNode = new TreeNode("Query");
                queryRootNode.ImageIndex = 4; // Icon Folder màu vàng
                queryRootNode.SelectedImageIndex = 4;
                rootNode.Nodes.Add(queryRootNode);

                // ====================================================================
                // NHÁNH 1: ĐỔ DỮ LIỆU SCHEMAS VÀ LỌC TRÙNG LẶP
                // ====================================================================
                AppStates.loadFPRDBSchemas = this.databaseService.getFPRDBSchemas();
                var schemas = AppStates.loadFPRDBSchemas;

                if (schemas != null && schemas.Count > 0)
                {
                    // Cuốn sổ ghi nhớ các Lược đồ đã vẽ
                    HashSet<string> addedSchemas = new HashSet<string>();

                    foreach (var schema in schemas)
                    {
                        string schemaName = schema.schemaName;

                        // LỌC: Nếu Lược đồ này đã vẽ rồi thì bỏ qua luôn, chuyển sang cái tiếp theo
                        if (addedSchemas.Contains(schemaName)) continue;

                        // Đánh dấu là đã vẽ
                        addedSchemas.Add(schemaName);

                        TreeNode schemaNode = new TreeNode(schemaName);
                        schemaNode.ImageIndex = 8;
                        schemaNode.SelectedImageIndex = 8;
                        schemaNode.Tag = "schema";
                        schemaNode.Tag = schema;
                        tablesRootNode.Nodes.Add(schemaNode);

                        var fields = schema.fields;
                        if (fields != null)
                        {
                            List<string> primaryKeys = schema.primarykey;

                            foreach (var field in fields)
                            {
                                string fieldName = field.getFieldName();
                                bool isPrimaryKey = primaryKeys != null && primaryKeys.Contains(fieldName);

                                TreeNode fieldNode = new TreeNode(fieldName);
                                fieldNode.ImageIndex = isPrimaryKey ? 5 : 2;
                                fieldNode.SelectedImageIndex = isPrimaryKey ? 5 : 2;
                                schemaNode.Nodes.Add(fieldNode);
                            }
                        }
                    }
                }
                else
                {
                    TreeNode emptySchemaNode = new TreeNode("(There're no Schemas)");
                    emptySchemaNode.ImageIndex = 4;
                    emptySchemaNode.SelectedImageIndex = 4;
                    tablesRootNode.Nodes.Add(emptySchemaNode);
                }

                // ====================================================================
                // NHÁNH 2: ĐỔ DỮ LIỆU RELATIONS VÀ LỌC TRÙNG LẶP
                // ====================================================================
                AppStates.loadFPRDBSchemaRelations = this.databaseService.getFPRDBRelations();
                var relations = AppStates.loadFPRDBSchemaRelations;
                if (relations != null && relations.Count > 0)
                {
                    // Cuốn sổ ghi nhớ các Quan hệ đã vẽ
                    HashSet<string> addedRelations = new HashSet<string>();

                    foreach (var relation in relations)
                    {
                        string relName = relation.relName;

                        // LỌC: Nếu Quan hệ này đã vẽ rồi thì bỏ qua luôn
                        if (addedRelations.Contains(relName)) continue;

                        // Đánh dấu là đã vẽ
                        addedRelations.Add(relName);

                        TreeNode instanceNode = new TreeNode(relName);
                        instanceNode.ImageIndex = 7;
                        instanceNode.SelectedImageIndex = 7;
                        instanceNode.Tag = relation;
                        relationRootNode.Nodes.Add(instanceNode);

                        var refSchema = relation.fprdbSchema;
                        if (refSchema != null)
                        {
                            string refSchemaName = refSchema.schemaName;
                            TreeNode refSchemaNode = new TreeNode(refSchemaName);
                            refSchemaNode.ImageIndex = 8;
                            refSchemaNode.SelectedImageIndex = 8;
                            instanceNode.Nodes.Add(refSchemaNode);
                        }
                    }
                }
                else
                {
                    TreeNode emptyRelNode = new TreeNode("(There're no Relations)");
                    emptyRelNode.ImageIndex = 4;
                    emptyRelNode.SelectedImageIndex = 4;
                    relationRootNode.Nodes.Add(emptyRelNode);
                }
                // Nhánh Query
                //TreeNode instanceNode1 = new TreeNode("Query01");
                //instanceNode1.ImageIndex = 7;
                //instanceNode1.SelectedImageIndex = 7;
                //instanceNode1.Tag = "query";
                //queryRootNode.Nodes.Add(instanceNode1);

                AppStates.listOfInDatabaseSQLFiles = this.inDataBaseSQLFileService.getInDatabaseSQLFileNames();
                List<string> queries = AppStates.listOfInDatabaseSQLFiles;
                if (queries != null && queries.Count > 0)
                {
                    // Cuốn sổ ghi nhớ các in database quries hệ đã vẽ
                    HashSet<string> addedQueries = new HashSet<string>();

                    foreach (string query in queries)
                    {
                        string queryName = query;

                        // LỌC: Nếu Query này đã vẽ rồi thì bỏ qua luôn
                        if (addedQueries.Contains(queryName)) continue;

                        // Đánh dấu là đã vẽ
                        addedQueries.Add(queryName);

                        TreeNode instanceNode = new TreeNode(queryName);
                        instanceNode.ImageIndex = 7;
                        instanceNode.SelectedImageIndex = 7;
                        instanceNode.Tag = "query";
                        queryRootNode.Nodes.Add(instanceNode);
                    }
                }
                else
                {
                    TreeNode emptyQueryNode = new TreeNode("(There're no Queries)");
                    emptyQueryNode.ImageIndex = 4;
                    emptyQueryNode.SelectedImageIndex = 4;
                    queryRootNode.Nodes.Add(emptyQueryNode);
                }

                treeView.ExpandAll();
            }
            catch(InvalidOperationException ex)
            {
                HandlingExceptionViaErrorNotification.handlingInvalidOperationException(ex);
            }
        }
        private void reLoadDatabaseTree()
        {
            // 1. Xóa cây cũ
            treeView.Nodes.Clear();
            // 2. Lấy tên file Database thông qua databaseService
            string dbName = "DATABASE";
            if (!string.IsNullOrEmpty(this.databaseService.getDatabaseName()))
            {
                dbName = this.databaseService.getDatabaseName();
            }

            if (string.IsNullOrEmpty(dbName) || dbName == "DATABASE") return;
            // 3. Tạo Node Root (VD: db1)
            TreeNode rootNode = new TreeNode(dbName);
            rootNode.ImageIndex = 3; // Icon Database
            rootNode.SelectedImageIndex = 3;
            treeView.Nodes.Add(rootNode);

            // ====================================================================
            // TẠO 2 THƯ MỤC CHA: Tables VÀ Relation (Ngang hàng nhau)
            // ====================================================================
            TreeNode tablesRootNode = new TreeNode("FPRDB Schemas");
            tablesRootNode.ImageIndex = 4; // Icon Folder màu vàng
            tablesRootNode.SelectedImageIndex = 4;
            rootNode.Nodes.Add(tablesRootNode);

            TreeNode relationRootNode = new TreeNode("Relation");
            relationRootNode.ImageIndex = 4; // Icon Folder màu vàng
            relationRootNode.SelectedImageIndex = 4;
            rootNode.Nodes.Add(relationRootNode);

            TreeNode queryRootNode = new TreeNode("Query");
            queryRootNode.ImageIndex = 4; // Icon Folder màu vàng
            queryRootNode.SelectedImageIndex = 4;
            rootNode.Nodes.Add(queryRootNode);

            // ====================================================================
            // NHÁNH 1: ĐỔ DỮ LIỆU SCHEMAS VÀ LỌC TRÙNG LẶP
            // ====================================================================
            var schemas = AppStates.loadFPRDBSchemas;

            if (schemas != null && schemas.Count > 0)
            {
                // Cuốn sổ ghi nhớ các Lược đồ đã vẽ
                HashSet<string> addedSchemas = new HashSet<string>();

                foreach (var schema in schemas)
                {
                    string schemaName = schema.schemaName;

                    // LỌC: Nếu Lược đồ này đã vẽ rồi thì bỏ qua luôn, chuyển sang cái tiếp theo
                    if (addedSchemas.Contains(schemaName)) continue;

                    // Đánh dấu là đã vẽ
                    addedSchemas.Add(schemaName);

                    TreeNode schemaNode = new TreeNode(schemaName);
                    schemaNode.ImageIndex = 8;
                    schemaNode.SelectedImageIndex = 8;
                    schemaNode.Tag = schema;
                    tablesRootNode.Nodes.Add(schemaNode);

                    var fields = schema.fields;
                    if (fields != null)
                    {
                        List<string> primaryKeys = schema.primarykey;

                        foreach (var field in fields)
                        {
                            string fieldName = field.getFieldName();
                            bool isPrimaryKey = primaryKeys != null && primaryKeys.Contains(fieldName);

                            TreeNode fieldNode = new TreeNode(fieldName);
                            fieldNode.ImageIndex = isPrimaryKey ? 5 : 2;
                            fieldNode.SelectedImageIndex = isPrimaryKey ? 5 : 2;
                            schemaNode.Nodes.Add(fieldNode);
                        }
                    }
                }
            }
            else
            {
                TreeNode emptySchemaNode = new TreeNode("(There're no Schemas)");
                emptySchemaNode.ImageIndex = 4;
                emptySchemaNode.SelectedImageIndex = 4;
                tablesRootNode.Nodes.Add(emptySchemaNode);
            }

            // ====================================================================
            // NHÁNH 2: ĐỔ DỮ LIỆU RELATIONS VÀ LỌC TRÙNG LẶP
            // ====================================================================
            var relations = AppStates.loadFPRDBSchemaRelations;
            if (relations != null && relations.Count > 0)
            {
                // Cuốn sổ ghi nhớ các Quan hệ đã vẽ
                HashSet<string> addedRelations = new HashSet<string>();

                foreach (var relation in relations)
                {
                    string relName = relation.relName;

                    // LỌC: Nếu Quan hệ này đã vẽ rồi thì bỏ qua luôn
                    if (addedRelations.Contains(relName)) continue;

                    // Đánh dấu là đã vẽ
                    addedRelations.Add(relName);

                    TreeNode instanceNode = new TreeNode(relName);
                    instanceNode.ImageIndex = 7;
                    instanceNode.SelectedImageIndex = 7;
                    instanceNode.Tag = "relation";
                    instanceNode.Tag = relation;
                    relationRootNode.Nodes.Add(instanceNode);

                    var refSchema = relation.fprdbSchema;
                    if (refSchema != null)
                    {
                        string refSchemaName = refSchema.schemaName;
                        TreeNode refSchemaNode = new TreeNode(refSchemaName);
                        refSchemaNode.ImageIndex = 8;
                        refSchemaNode.SelectedImageIndex = 8;
                        instanceNode.Nodes.Add(refSchemaNode);
                    }
                }
            }
            else
            {
                TreeNode emptyRelNode = new TreeNode("(There're no Relations)");
                emptyRelNode.ImageIndex = 4;
                emptyRelNode.SelectedImageIndex = 4;
                relationRootNode.Nodes.Add(emptyRelNode);
            }

            // Nhánh Query
            //TreeNode instanceNode1 = new TreeNode("Query01");
            //instanceNode1.ImageIndex = 7;
            //instanceNode1.SelectedImageIndex = 7;
            //instanceNode1.Tag = "query";
            //queryRootNode.Nodes.Add(instanceNode1);

            
            List<string> queries = AppStates.listOfInDatabaseSQLFiles;
            if (queries != null && queries.Count > 0)
            {
                // Cuốn sổ ghi nhớ các in database quries hệ đã vẽ
                HashSet<string> addedQueries = new HashSet<string>();

                foreach (string query in queries)
                {
                    string queryName = query;

                    // LỌC: Nếu Query này đã vẽ rồi thì bỏ qua luôn
                    if (addedQueries.Contains(queryName)) continue;

                    // Đánh dấu là đã vẽ
                    addedQueries.Add(queryName);

                    TreeNode instanceNode = new TreeNode(queryName);
                    instanceNode.ImageIndex = 7;
                    instanceNode.SelectedImageIndex = 7;
                    instanceNode.Tag = "query";
                    queryRootNode.Nodes.Add(instanceNode);
                }
            }
            else
            {
                TreeNode emptyQueryNode = new TreeNode("(There're no Queries)");
                emptyQueryNode.ImageIndex = 4;
                emptyQueryNode.SelectedImageIndex = 4;
                queryRootNode.Nodes.Add(emptyQueryNode);
            }

            treeView.ExpandAll();
        }
        // Hàm xử lý khi click "Open" button
        private void buttonOpen_pageHome_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog DialogOpen = new OpenFileDialog();
            DialogOpen.Title = "Open Fuzzy Probabilistic Relational Database (FPRDB)";
            // Chỉ lọc ra các file có đuôi .pdb hoặc .sqlite
            DialogOpen.Filter = $"Database file (*{this.fprdbDotExtension})|*{this.fprdbDotExtension}";
            DialogOpen.DefaultExt = "db";
            DialogOpen.InitialDirectory = GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());

            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.databaseService.loadDB(DialogOpen.FileName);
                    XtraMessageBox.Show("Open database successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isDatabaseLoaded = true;
                    LoadDatabaseTree();
                    //load field types
                    AppStates.createSChemaFieldTypes = this.databaseService.getFieldTypes();

                    AppStates.ISAppStateFullyLoad = true;
                }
                catch (InvalidFPRDBDatabaseFile ex)
                {
                    XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AppStates.clear();
                    //clear treview of FPRDB schemas and relaions
                    isDatabaseLoaded = false;
                    changeStatusTab();
                    treeView.Nodes.Clear();
                }
                catch (IOException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // Hàm xử lý khi click "New" button
        private void buttonNew_pageHome_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //CompositionRoot.databaseService.SaveDB();
            try
            {
                SaveFileDialog DialogNew = new SaveFileDialog();
                DialogNew.Title = "Create New Fuzzy Probabilistic Relational Database (FPRDB)";
                DialogNew.Filter = $"Database file (*{this.fprdbDotExtension})|*{this.fprdbDotExtension}";
                DialogNew.DefaultExt = "db";
                DialogNew.AddExtension = true;
                DialogNew.RestoreDirectory = true;
                DialogNew.InitialDirectory = GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());
                DialogNew.SupportMultiDottedExtensions = true;

                if (DialogNew.ShowDialog() == DialogResult.OK)
                {
                    this.databaseService.createDB(DialogNew.FileName);
                    XtraMessageBox.Show("Create new database successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isDatabaseLoaded = true;
                    LoadDatabaseTree();
                    //load field types
                    AppStates.createSChemaFieldTypes = this.databaseService.getFieldTypes();

                    AppStates.ISAppStateFullyLoad = true;
                }
            }
            catch (FileNotFoundException ex)
            {
                XtraMessageBox.Show($"Directory doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (IOException ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonExit_pageHome_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DialogResult result = XtraMessageBox.Show("Are you sure want to exit?", "Exit FPRDB Visual Management System", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    Close();
                }
            }
            catch (Exception Ex)
            {
                XtraMessageBox.Show(Ex.Message);
            }
        }
        #endregion
        #region Tab Page Query
        #region Page Group File
        // Hàm enable Query Edtor khi đã load Query thành công
        private void CreateQueryTab(string fileName = "", string content = "", string path = "", bool isStoredDB = false)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"SQLQuery{sqlQueryCount++}.fprdbsql";
            }

            string tabTitle;
            if (!isStoredDB)
                tabTitle = string.IsNullOrEmpty(path) ? fileName + "*" : fileName;
            else
            {
                fileName = fileName + ".fprdbsql" + " [Database]";
                tabTitle = fileName;
            }
            XtraTabPage newPage = new XtraTabPage { Text = tabTitle, Tag = "QueryTab" };
            newPage.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;

            ucQueryEditor editor = new ucQueryEditor();
            editor.Dock = DockStyle.Fill;
            if (isStoredDB)
                editor.Initialize(content, path, isStoredDB);
            else
                editor.Initialize(content, path);

            editor.OnDirtyStateChanged = (isDirty) =>
            {
                string baseName = string.IsNullOrEmpty(editor.FilePath) ? fileName : Path.GetFileName(editor.FilePath);
                if (isDirty)
                {
                    newPage.Text = baseName + "*";
                }
                else
                {
                    newPage.Text = baseName;
                }
            };


            newPage.Controls.Add(editor);

            xtraTabControlDatabase.TabPages.Add(newPage);
            xtraTabControlDatabase.SelectedTabPage = newPage;
        }

        private void SetQueryTabState(bool isEnabled)
        {
            ribbonControl.SelectedPage = QueryRibbonPage;
            conjunctionRibbonPageGroup.Enabled = isEnabled;
            disjunctionRibbonPageGroup.Enabled = isEnabled;
            differenceRibbonPageGroup.Enabled = isEnabled;
            operatorRibbonPageGroup.Enabled = isEnabled;
            excuteQueryribbonPageGroup.Enabled = isEnabled;
            iSaveQuery.Enabled = isEnabled;

        }
        // Hàm tạo mới SQL File
        private void CreateNewQuery()
        {
            CreateQueryTab();
            SetQueryTabState(true);
        }

        // Hàm mở SQL File đã tồn tại
        private void OpenQuery()
        {
            OpenFileDialog DialogOpen = new OpenFileDialog();
            DialogOpen.Title = "Open SQL File";
            DialogOpen.Filter = "SQL File (*.fprdbsql)|*.fprdbsql";
            DialogOpen.DefaultExt = "fprdbsql";
            DialogOpen.InitialDirectory = GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());
            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = DialogOpen.FileName;

                foreach (XtraTabPage page in xtraTabControlDatabase.TabPages)
                {
                    // Bước 1: Kiểm tra đúng loại Tab Query dựa trên Tag
                    if (page.Tag != null && page.Tag.ToString() == "QueryTab")
                    {
                        // Bước 2: Truy cập vào UserControl bên trong Tab
                        var uc = page.Controls.OfType<ucQueryEditor>().FirstOrDefault();

                        if (uc != null)
                        {
                            // Bước 3: Kiểm tra đường dẫn file lưu trong UC có trùng không
                            if (uc.FilePath == selectedFilePath)
                            {
                                xtraTabControlDatabase.SelectedTabPage = page;
                                ribbonControl.SelectedPage = QueryRibbonPage;
                                return;
                            }
                        }
                    }
                }

                try
                {
                    //string sqlContent = sqlFileService.loadFile(DialogOpen.FileName);
                    currentSQLFilePath = DialogOpen.FileName;
                    // Đọc file và lấy nội dung file gán váo Query Editor
                    CreateQueryTab(Path.GetFileName(DialogOpen.FileName)
                        , File.ReadAllText(currentSQLFilePath, Encoding.Unicode)
                        , currentSQLFilePath);
                    // Set trạng thái enable cho tab Query sau khi mở file thành công
                    SetQueryTabState(true);
                    XtraMessageBox.Show("Open SQL file successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (IOException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // Xử lý sự kiện click cho nút New
        private void iNewQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateNewQuery();
        }
        // Xử lý sự kiện click cho nút Open
        private void iOpenQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenQuery();
        }
        // Hàm lưu file SQL
        private bool SaveCurrentFile(bool isDBFile = false)
        {
            //bool isDBFile=true-> save cho in database sql file, false-> save cho file-based sql file 
            XtraTabPage currentTab = xtraTabControlDatabase.SelectedTabPage;

            if (currentTab != null && currentTab.Controls.Count > 0 && currentTab.Controls[0] is ucQueryEditor uc)
            {
                try
                {
                    if (isDBFile)
                    {
                        if (uc.IsDBFile)
                        {
                            //uc.IsDBFile=true-> tab query dang duoc chon la query luu trong database
                            ////uc.IsDBFile=false-> tab query dang duoc chon la query luu duoi dang file

                            try
                            {
                                string inDatabaseSQLFileName = currentTab.Text.Substring(0, currentTab.Text.LastIndexOf('.')).Trim();
                                this.inDataBaseSQLFileService.save(inDatabaseSQLFileName, uc.QueryText);
                                XtraMessageBox.Show("Save SQL file in database successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                uc.MarkAsSaved(uc.QueryText);
                                return true;
                            }
                            catch (InvalidOperationException ex)
                            {
                                HandlingExceptionViaErrorNotification.handlingInvalidOperationException(ex);
                            }
                            catch (UnderlyingStorageEngineCRUDException ex)
                            {
                                XtraMessageBox.Show($"Error: {ex.Message}", "UNDERLYING STORAGE MECHANISM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("This file is an invalid file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    if (uc.IsDBFile)
                    {
                        XtraMessageBox.Show("This file is an invalid file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    // 2. Kiểm tra nếu là file tạm (chưa có đường dẫn) thì phải hiện SaveFileDialog
                    if (uc.IsTemporary)
                    {
                        using (SaveFileDialog sfd = new SaveFileDialog { Filter = "SQL File (*.fprdbsql)|*.fprdbsql", Title = "Save SQL File" })
                        {
                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                uc.FilePath = sfd.FileName; // Cập nhật đường dẫn mới vào UC
                            }
                            else return false;
                        }
                    }

                    // 3. Thực hiện ghi file (Dùng đúng FilePath của riêng Tab đó)
                    File.WriteAllText(uc.FilePath, uc.QueryText, Encoding.Unicode);

                    // 4. Cập nhật trạng thái trong UC để xóa dấu *
                    uc.MarkAsSaved(uc.QueryText);
                    // 5. Cập nhật lại tiêu đề Tab (Bỏ dấu *)
                    string fileNameOnly = Path.GetFileName(uc.FilePath);
                    currentTab.Text = fileNameOnly;

                    XtraMessageBox.Show("Save SQL file successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                catch (InvalidOperationException ex)
                {
                    HandlingExceptionViaErrorNotification.handlingInvalidOperationException(ex);
                    return false;
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return false;
        }
        // Xử lý sự kiện click cho nút Save
        private void iSaveQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveCurrentFile();
        }
        #endregion
        //hàm chèn ký tự
        private void InsertSymbol(string symbol)
        {
            if (xtraTabControlDatabase.SelectedTabPage?.Controls[0] is ucQueryEditor uc)
            {
                uc.InsertTextAtCursor(symbol);
            }
        }
        #region Page Group Conjuntion
        private void iConjunctionIgnorance_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨂_ig ");
        private void iConjunctionIndependence_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨂_in ");
        private void iConjunctionMutual_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨂_me ");
        private void iConjunctionPositive_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨂_pc ");
        #endregion
        #region Page Group Disjunction
        private void iDisjunctionIgnorance_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨁_ig ");
        private void iDisjunctionIndependence_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨁_in ");
        private void iDisjunctionMutual_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨁_me ");
        private void iDisjunctionPositive_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⨁_pc ");
        #endregion
        #region Page Group Difference
        private void iDifferenceIgnorance_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⦵_ig ");
        private void iDifferenceIndependence_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⦵_in ");
        private void iDiferenceMutual_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⦵_me ");
        private void iDifferencePositive_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⦵_pc ");
        #endregion
        private void ExcuteQueryDemo(string sql)
        {
            //try
            //{
            //    // hiển thị 2 panel của SplitContainerControl để show kết quả truy vấn và cây giải thuật
            //    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;

            //    // Hiển thị cả 2 panel (Query Editor và phần Kết quả)
            //    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;

            //    // Xóa dữ liệu cũ trên GridView
            //    gridControlResultQuery.DataSource = null;
            //    gridViewResultQuery.Columns.Clear();
            //    if (string.IsNullOrEmpty(sql.Trim()))
            //    {
            //        XtraMessageBox.Show("Query does not exist!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    // TODO: Truyền đối tượng database hiện tại vào.
            //    //query = new QueryExecutionBLL(sql, null /* thay bằng đối tượng probDatabase của bạn */);


            //    if (query.ExecuteQuery())
            //    {
            //        if (query.relationResult.FproTuples.Count <= 0)
            //        {
            //            MessagextraTabPage.Text = "No tuple satisfies the condition";
            //            // Chuyển sang Tab Message (Index 1)
            //            xtraTabControlResultQuery.SelectedTabPageIndex = 1;
            //        }
            //        else
            //        {
            //            // Chuẩn bị DataTable để chứa dữ liệu
            //            DataTable dtquery = new DataTable();
            //            dtquery.Columns.Add("NoNumber", typeof(int));

            //            // 1. Khởi tạo cột STT (Number)
            //            DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
            //            col.Caption = "Number";
            //            col.Name = "NoNumber";
            //            col.FieldName = "NoNumber";
            //            col.OptionsColumn.ReadOnly = true;
            //            col.OptionsColumn.AllowEdit = false;
            //            col.Visible = true;
            //            col.VisibleIndex = 0;
            //            gridViewResultQuery.Columns.Add(col);

            //            // 2. Khởi tạo các cột dữ liệu dựa trên thuộc tính được chọn
            //            int i = 1;
            //            foreach (var att in query.selectedAttributes)
            //            {
            //                dtquery.Columns.Add("Col" + i, typeof(String));
            //                DevExpress.XtraGrid.Columns.GridColumn columnDiff = new DevExpress.XtraGrid.Columns.GridColumn();
            //                columnDiff.Caption = att.AttributeName;
            //                columnDiff.Name = "Col" + i;
            //                columnDiff.FieldName = "Col" + i; // FieldName phải map đúng với tên cột trong DataTable
            //                columnDiff.OptionsColumn.ReadOnly = true;
            //                columnDiff.OptionsColumn.AllowEdit = false;

            //                // Đổi màu nền cho các tập mờ
            //                if (att.FproDataType.TypeName == "DiscreteFuzzySet" || att.FproDataType.TypeName == "ContinuousFuzzySet")
            //                {
            //                    columnDiff.AppearanceCell.Options.UseBackColor = true;
            //                    columnDiff.AppearanceCell.BackColor = Color.LightCyan; // Màu nền chính
            //                    // columnDiff.AppearanceCell.BackColor2 = Color.DeepSkyBlue; // (Tùy chọn) Màu Gradient
            //                }

            //                columnDiff.Visible = true;
            //                columnDiff.VisibleIndex = i;
            //                gridViewResultQuery.Columns.Add(columnDiff);
            //                i++;
            //            }

            //            // 3. Đổ dữ liệu từ Tuples vào DataTable
            //            int k = -1;
            //            foreach (var tuple in query.relationResult.FproTuples)
            //            {
            //                DataRow row = dtquery.NewRow();
            //                k++;
            //                int j = 1;
            //                row[0] = k + 1; // STT

            //                foreach (var triple in tuple.FproTriples)
            //                {
            //                    row[j] = triple.GetStrValue();
            //                    j++;
            //                }
            //                dtquery.Rows.Add(row);
            //            }

            //            // 4. Gán DataSource và hiển thị Tab kết quả
            //            gridControlResultQuery.DataSource = dtquery;
            //            // Tự động căn chỉnh độ rộng các cột cho đẹp
            //            gridViewResultQuery.BestFitColumns();

            //            xtraTabControlResultQuery.SelectedTabPageIndex = 0; // Tab Query Result
            //        }
            //    }
            //    else
            //    {
            //        // Truy vấn lỗi (sai cú pháp...)
            //        MessagextraTabPage.Text = query.MessageError;
            //        xtraTabControlResultQuery.SelectedTabPageIndex = 1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show($"Error executing query: {ex.Message}", "Error",
            //        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    // Dù có lỗi hay không thì vẫn phải đảm bảo tập trung con trỏ vào ô nhập liệu để người dùng tiện sửa lỗi hoặc tiếp tục nhập query
            //    memoEditTxtQuery.Focus();

            //    // có hàm ClearAll()(nếu có) để xóa sạch dữ liệu cũ trước khi hiển thị kết quả mới, hãy gọi nó ở đây
            //    // ClearAll();
            //}
            // createQueryPlan

            // 

            // dữ liệu giả để test giao diện
            //try
            //{
            //    // Mở rộng giao diện để xem cả query và kết quả
            //    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;

            //    // 1. Tạo một DataTable giả lập cấu trúc
            //    DataTable dtMock = new DataTable();
            //    dtMock.Columns.Add("Number", typeof(int));
            //    dtMock.Columns.Add("doctor1.ID", typeof(string));
            //    dtMock.Columns.Add("doctor1.AGE", typeof(string));
            //    dtMock.Columns.Add("doctor2.NAME", typeof(string));
            //    dtMock.Columns.Add("doctor2.AGE", typeof(string));

            //    // 2. Thêm dữ liệu giả lập (giống hệt hình ảnh bạn cung cấp)
            //    dtMock.Rows.Add(4, "{ DT093 }[ 1, 1 ]", "{ approx_30 }[ 1, 1 ]", "{ L.V. Cuong }[ 1, 1 ]", "{ 30 }[ 0.4, 0.6 ]");
            //    dtMock.Rows.Add(5, "{ DT093 }[ 1, 1 ]", "{ approx_30 }[ 1, 1 ]", "{ N.V. Hung }[ 1, 1 ]", "{ middle_age }[ 0.8, 1 ]");
            //    dtMock.Rows.Add(6, "{ DT093 }[ 1, 1 ]", "{ approx_30 }[ 1, 1 ]", "{ N.T. Dat }[ 1, 1 ]", "{ 54 }[ 0.5, 0.5 ]");
            //    dtMock.Rows.Add(7, "{ DT102 }[ 1, 1 ]", "{ 55 }[ 0.5, 0.5 ]", "{ L.V. Cuong }[ 1, 1 ]", "{ 30 }[ 0.4, 0.6 ]");
            //    dtMock.Rows.Add(8, "{ DT102 }[ 1, 1 ]", "{ 55 }[ 0.5, 0.5 ]", "{ N.V. Hung }[ 1, 1 ]", "{ middle_age }[ 0.8, 1 ]");
            //    dtMock.Rows.Add(9, "{ DT102 }[ 1, 1 ]", "{ 55 }[ 0.5, 0.5 ]", "{ N.T. Dat }[ 1, 1 ]", "{ 54 }[ 0.5, 0.5 ]");
            //    //next()
            //    // 3. Gán dữ liệu giả vào GridControl
            //    gridControlResultQuery.DataSource = dtMock;

            //    // 4. Yêu cầu GridView tự động tạo các cột dựa trên DataTable
            //    gridViewResultQuery.PopulateColumns();

            //    dtMock.Rows.Add(10, "{ DT102 }[ 1, 1 ]", "{ 55 }[ 0.5, 0.5 ]", "{ N.T. Dat }[ 1, 1 ]", "{ 54 }[ 0.5, 0.5 ]");

            //    // 5. Tùy chỉnh hiển thị cột (Frontend)
            //    if (gridViewResultQuery.Columns.Count > 0)
            //    {
            //        // Chỉnh độ rộng và không cho phép sửa trực tiếp trên lưới
            //        gridViewResultQuery.OptionsBehavior.Editable = false;
            //        gridViewResultQuery.BestFitColumns();
            //    }

            //    // Chuyển sang Tab hiển thị kết quả (Giả sử index 0 là Query Result)
            //    xtraTabControlResultQuery.SelectedTabPageIndex = 0;
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show("Lỗi hiển thị dữ liệu giả: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
        private void triggerReloadDatabaseTree()
        {
            try
            {
                AppStates.loadFPRDBSchemas = this.databaseService.getFPRDBSchemas();
                AppStates.loadFPRDBSchemaRelations = this.databaseService.getFPRDBRelations();
                AppStates.listOfInDatabaseSQLFiles = this.inDataBaseSQLFileService.getInDatabaseSQLFileNames();
            }
            catch(InvalidOperationException ex)
            {
                HandlingExceptionViaErrorNotification.handlingInvalidOperationException(ex);
            }
            this.reLoadDatabaseTree();
        }
        private void iExcuteQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (xtraTabControlDatabase.SelectedTabPage?.Controls[0] is ucQueryEditor uc)
            //{
            //    uc.ClearAllGrids();
            //    string sql = uc.GetSelectedQuery();
            //    string firstString = sql.Split(' ')[0];
            //    switch (firstString.ToUpper())
            //    {
            //        case "INSERT":
            //        case "DELETE":
            //        case "UPDATE":
            //            ExecuteUpdate(sql, false, uc);
            //            break;
            //        case "DROP":
            //        case "CREATE":
            //            ExecuteDataDefinition(sql, uc);
            //            break;
            //        default:
            //            ExecuteQuery(sql, uc);
            //            break;
            //    }
            //}
            if (AppStates.ISAppStateFullyLoad == false)
            {
                XtraMessageBox.Show("Database isn't loaded", "Invalid Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool reloadDatabaseTreeFlag = false;
            bool reloadTabsFlag = false;
            bool showMessageTabFlag = true;
            if (xtraTabControlDatabase.SelectedTabPage?.Controls[0] is ucQueryEditor uc)
            {
                uc.ClearAllGrids();
                string fprdbSQLStatements = uc.GetSelectedQuery();

                try
                {
                    List<FPRDBSQLExecutionResult> results = this.sqlProcessor.executeFPRDBSQLStatements(fprdbSQLStatements);
                    if (uc.memoEditMessageUC.Text == null)
                        uc.memoEditMessageUC.Text = "";

                    foreach (FPRDBSQLExecutionResult res in results.AsEnumerable().Reverse())
                    {
                        if (res is DDL_FPRDB_SQL_ExecutionResult)
                        {
                            reloadDatabaseTreeFlag = true;
                            reloadTabsFlag = true;
                            uc.memoEditMessageUC.Text += $"Data Definition Language success.\r\n";
                            showMessageTabFlag = true;
                        }
                        else if (res is DML_FPRDB_SQL_ExecutionResult)
                        {
                            reloadTabsFlag = true;
                            uc.memoEditMessageUC.Text += $"[Number of tuples affected]: {(res as DML_FPRDB_SQL_ExecutionResult).numberTuplesAffected}.\r\n";
                            showMessageTabFlag = true;
                        }
                        else //if(res is DQL_FPRDB_SQL_ExecutionResult)
                        {
                            DataTable resultForGridView = createDataTableForInMemoryScanResultedFromDQLExecution((res as DQL_FPRDB_SQL_ExecutionResult).resultedRelation);
                            uc.CreateNewGridResult(resultForGridView);

                            // Ghi thông báo thành công vào MemoEdit
                            uc.memoEditMessageUC.Text += $"Query executed successfully.\r\n";
                            showMessageTabFlag = false;

                        }

                    }
                    if (reloadDatabaseTreeFlag)
                    {
                        triggerReloadDatabaseTree();
                    }
                    if (reloadTabsFlag)
                    {
                        ReloadTabs();
                    }
                    if (showMessageTabFlag)
                        uc.ViewError();
                    else
                        uc.ViewResult();
                }
                catch (SQLSyntaxException ex)
                {
                    triggerReloadDatabaseTree();

                    uc.memoEditMessageUC.Text = $"[SQL Syntax Error]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (SemanticException ex)
                {
                    triggerReloadDatabaseTree();

                    uc.memoEditMessageUC.Text = $"[SQL Semantic Error]\r\n{ex.Message}";
                    uc.ViewError();
                }

                catch (InvalidOperationException ex)
                {
                    triggerReloadDatabaseTree();

                    uc.memoEditMessageUC.Text = $"[Invalid Operation Error]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (InvalidCastException ex)
                {
                    triggerReloadDatabaseTree();

                    uc.memoEditMessageUC.Text = $"[Invalid Cast Exception Error]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (MismatchTokenType ex)
                {
                    triggerReloadDatabaseTree();

                    uc.memoEditMessageUC.Text = $"[Token Mismatch]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (NotSupportedException ex)
                {
                    triggerReloadDatabaseTree();

                    uc.memoEditMessageUC.Text = $"[Not Supported]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (UnderlyingStorageEngineCRUDException ex)
                {
                    triggerReloadDatabaseTree();

                    XtraMessageBox.Show($"Error: {ex.Message}", "UNDERLYING STORAGE MECHANISM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //this.DialogResult = DialogResult.Abort;
                }
                finally
                {
                    // Đảm bảo splitContainer luôn hiện để xem được kết quả/lỗi
                    uc.splitContainer.PanelVisibility = SplitPanelVisibility.Both;
                }


            }

        }
        // Dummy situation for list objects excutionResults
        //private void executeDemo()
        //{
        //    executionResults[];
        //    foreach (var obj in executionResults)
        //    {
        //        if (obj is Plan)
        //        {
        //            // Excecuting similar to ExecuteQuery() function then provide data table for uc.CreateNewGridResult(dt)
        //            // uc.CreateNewGridResult(dt) is responsible for adding new grid and binding data source of that data table
        //        }
        //        else if (obj is rowAffected)
        //        {
        //            uc.memoEditMessageUC.Text += $"[Number of rows affected]{rowAffected}\r\n";
        //        }
        //    }
        //}
        private void ExecuteDataDefinition(string sql, ucQueryEditor uc)
        {
            try
            {
                this.sqlProcessor.executeDataDefinition(sql);
                uc.memoEditMessageUC.Text = $"Success";
                uc.ViewError();

                AppStates.loadFPRDBSchemas = this.databaseService.getFPRDBSchemas();
                AppStates.loadFPRDBSchemaRelations = this.databaseService.getFPRDBRelations();
                this.reLoadDatabaseTree();
            }
            catch (SQLSyntaxException ex)
            {
                uc.memoEditMessageUC.Text = $"[SQL Syntax Error]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (SemanticException ex)
            {
                uc.memoEditMessageUC.Text = $"[SQL Semantic Error]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (InvalidCastException ex)
            {
                uc.memoEditMessageUC.Text = $"[Invalid Cast Exception Error]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (MismatchTokenType ex)
            {
                uc.memoEditMessageUC.Text = $"[Token Mismatch]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (NotSupportedException ex)
            {
                uc.memoEditMessageUC.Text = $"[Not Supported]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (UnderlyingStorageEngineCRUDException ex)
            {
                XtraMessageBox.Show($"Error: {ex.Message}", "UNDERLYING STORAGE MECHANISM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.DialogResult = DialogResult.Abort;
            }
            finally
            {
                // Đảm bảo splitContainer luôn hiện để xem được kết quả/lỗi
                uc.splitContainer.PanelVisibility = SplitPanelVisibility.Both;
            }

        }
        private void ExecuteUpdate(string sql, bool isReloadGUI, ucQueryEditor uc)
        {
            try
            {
                int noRowsAffected = this.sqlProcessor.executeUpdate(sql);
                uc.memoEditMessageUC.Text += $"[Number of rows affected]{noRowsAffected}\r\n";
                uc.ViewError();
                if (isReloadGUI)
                {
                    AppStates.loadFPRDBSchemas = this.databaseService.getFPRDBSchemas();
                    AppStates.loadFPRDBSchemaRelations = this.databaseService.getFPRDBRelations();
                    this.reLoadDatabaseTree();
                }
            }
            catch (SQLSyntaxException ex)
            {
                uc.memoEditMessageUC.Text = $"[SQL Syntax Error]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (SemanticException ex)
            {
                uc.memoEditMessageUC.Text = $"[SQL Semantic Error]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (InvalidCastException ex)
            {
                uc.memoEditMessageUC.Text = $"[Invalid Cast Exception Error]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (MismatchTokenType ex)
            {
                uc.memoEditMessageUC.Text = $"[Token Mismatch]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (InvalidOperationException ex)
            {
                uc.memoEditMessageUC.Text = $"[Invalid Operation]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (NotSupportedException ex)
            {
                uc.memoEditMessageUC.Text = $"[Not Supported]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (UnderlyingStorageEngineCRUDException ex)
            {
                XtraMessageBox.Show($"Error: {ex.Message}", "UNDERLYING STORAGE MECHANISM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.DialogResult = DialogResult.Abort;
            }
            finally
            {
                // Đảm bảo splitContainer luôn hiện để xem được kết quả/lỗi
                uc.splitContainer.PanelVisibility = SplitPanelVisibility.Both;
            }

        }
        private DataTable createDataTableForInMemoryScanResultedFromDQLExecution(InMemoryScan iscan)
        {
            DataTable resultForGridView = new DataTable();
            FPRDBSchema schema = iscan.getFPRDBSchema();

            //create columns for grid view of query result
            foreach (Field f in schema.getFields())
            {
                resultForGridView.Columns.Add(f.getFieldName(), typeof(string));
            }
            //Extract the result for grid view
            string[] tupleForGridView = new string[schema.getFields().Count];
            Field field;
            List<Field> fields = schema.getFields();
            while (iscan.next())
            {
                for (int i = 0; i < schema.getFields().Count; ++i)
                {
                    field = fields[i];
                    switch (field.getFieldInfo().getType())
                    {
                        case FieldType.INT:
                        case FieldType.DIST_FUZZYSET_INT:
                            tupleForGridView[i] = iscan.getFieldContent<int>(field.getFieldName()).ToString();
                            break;
                        case FieldType.FLOAT:
                        case FieldType.DIST_FUZZYSET_FLOAT:
                        case FieldType.CONT_FUZZYSET:
                            //tupleForGridView.Add((s.getFieldContent<float>(field.getFieldName())).ToString());
                            tupleForGridView[i] = iscan.getFieldContent<float>(field.getFieldName()).ToString();
                            break;
                        case FieldType.CHAR:
                        case FieldType.VARCHAR:
                        case FieldType.DIST_FUZZYSET_TEXT:
                            //tupleForGridView.Add((s.getFieldContent<string>(field.getFieldName())).ToString());
                            tupleForGridView[i] = iscan.getFieldContent<string>(field.getFieldName()).ToString();
                            break;
                        case FieldType.BOOLEAN:
                            //tupleForGridView.Add((s.getFieldContent<bool>(field.getFieldName())).ToString());
                            tupleForGridView[i] = iscan.getFieldContent<bool>(field.getFieldName()).ToString();
                            break;
                    }
                }
                resultForGridView.Rows.Add(tupleForGridView);


            }
            return resultForGridView;
        }
        private DataTable createDataTableForPlanResultedFromDQLExecution(Plan p)
        {
            DataTable resultForGridView = new DataTable();
            FPRDBSchema schema = p.getSchema();
            Scan s = p.open();

            //create columns for grid view of query result
            foreach (Field f in schema.getFields())
            {
                resultForGridView.Columns.Add(f.getFieldName(), typeof(string));
            }
            //Extract the result for grid view
            string[] tupleForGridView = new string[schema.getFields().Count];
            Field field;
            List<Field> fields = schema.getFields();
            while (s.next())
            {
                for (int i = 0; i < schema.getFields().Count; ++i)
                {
                    field = fields[i];
                    switch (field.getFieldInfo().getType())
                    {
                        case FieldType.INT:
                        case FieldType.DIST_FUZZYSET_INT:
                            tupleForGridView[i] = s.getFieldContent<int>(field.getFieldName()).ToString();
                            break;
                        case FieldType.FLOAT:
                        case FieldType.DIST_FUZZYSET_FLOAT:
                        case FieldType.CONT_FUZZYSET:
                            //tupleForGridView.Add((s.getFieldContent<float>(field.getFieldName())).ToString());
                            tupleForGridView[i] = s.getFieldContent<float>(field.getFieldName()).ToString();
                            break;
                        case FieldType.CHAR:
                        case FieldType.VARCHAR:
                        case FieldType.DIST_FUZZYSET_TEXT:
                            //tupleForGridView.Add((s.getFieldContent<string>(field.getFieldName())).ToString());
                            tupleForGridView[i] = s.getFieldContent<string>(field.getFieldName()).ToString();
                            break;
                        case FieldType.BOOLEAN:
                            //tupleForGridView.Add((s.getFieldContent<bool>(field.getFieldName())).ToString());
                            tupleForGridView[i] = s.getFieldContent<bool>(field.getFieldName()).ToString();
                            break;
                    }
                }
                resultForGridView.Rows.Add(tupleForGridView);


            }
            return resultForGridView;
        }
        private void ExecuteQuery(string sql, ucQueryEditor uc)
        {
            try
            {
                DataTable resultForGridView = new DataTable();
                Plan p = this.sqlProcessor.createQueryPlan(sql);
                FPRDBSchema schema = p.getSchema();
                Scan s = p.open();

                //create columns for grid view of query result
                foreach (Field f in schema.getFields())
                {
                    resultForGridView.Columns.Add(f.getFieldName(), typeof(string));
                }
                //Extract the result for grid view
                string[] tupleForGridView = new string[schema.getFields().Count];
                Field field;
                List<Field> fields = schema.getFields();
                while (s.next())
                {
                    for (int i = 0; i < schema.getFields().Count; ++i)
                    {
                        field = fields[i];
                        switch (field.getFieldInfo().getType())
                        {
                            case FieldType.INT:
                            case FieldType.DIST_FUZZYSET_INT:
                                tupleForGridView[i] = s.getFieldContent<int>(field.getFieldName()).ToString();
                                break;
                            case FieldType.FLOAT:
                            case FieldType.DIST_FUZZYSET_FLOAT:
                            case FieldType.CONT_FUZZYSET:
                                //tupleForGridView.Add((s.getFieldContent<float>(field.getFieldName())).ToString());
                                tupleForGridView[i] = s.getFieldContent<float>(field.getFieldName()).ToString();
                                break;
                            case FieldType.CHAR:
                            case FieldType.VARCHAR:
                            case FieldType.DIST_FUZZYSET_TEXT:
                                //tupleForGridView.Add((s.getFieldContent<string>(field.getFieldName())).ToString());
                                tupleForGridView[i] = s.getFieldContent<string>(field.getFieldName()).ToString();
                                break;
                            case FieldType.BOOLEAN:
                                //tupleForGridView.Add((s.getFieldContent<bool>(field.getFieldName())).ToString());
                                tupleForGridView[i] = s.getFieldContent<bool>(field.getFieldName()).ToString();
                                break;
                        }
                    }
                    resultForGridView.Rows.Add(tupleForGridView);


                }
                uc.CreateNewGridResult(resultForGridView);

                // Ghi thông báo thành công vào MemoEdit
                uc.ViewResult();
                uc.memoEditMessageUC.Text = $"Query executed successfully";
            }
            catch (SQLSyntaxException ex)
            {
                uc.memoEditMessageUC.Text = $"[SQL Syntax Error]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (SemanticException ex)
            {
                uc.memoEditMessageUC.Text = $"[SQL Semantic Error]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (InvalidCastException ex)
            {
                uc.memoEditMessageUC.Text = $"[Invalid Cast Exception Error]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (MismatchTokenType ex)
            {
                uc.memoEditMessageUC.Text = $"[Token Mismatch]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (NotSupportedException ex)
            {
                uc.memoEditMessageUC.Text = $"[Not Supported]\r\n{ex.Message}";
                uc.ViewError();
            }
            catch (UnderlyingStorageEngineCRUDException ex)
            {
                XtraMessageBox.Show($"Error: {ex.Message}", "UNDERLYING STORAGE MECHANISM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.DialogResult = DialogResult.Abort;
            }
            finally
            {
                // Đảm bảo splitContainer luôn hiện để xem được kết quả/lỗi
                uc.splitContainer.PanelVisibility = SplitPanelVisibility.Both;
            }
        }
        private void iOperator_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => InsertSymbol(" ⇒ ");

        #endregion
        #region Tab Page Schema
        private void iNewSchema_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (frmNewSchema childform = new frmNewSchema(compRoot))
                {
                    if (childform.ShowDialog() == DialogResult.OK)
                    {
                        reLoadDatabaseTree();
                    }
                }
            }
            catch(InvalidOperationException ex)
            {
                HandlingExceptionViaErrorNotification.handlingInvalidOperationException(new InvalidOperationException("Database isn't loaded"));
            }
        }
        #endregion
        // Hàm xử lý sự kiện click cho nút Xóa lược đồ
        private void iDeleteSchema_ItemClick(object sender, ItemClickEventArgs e)
        {
            TreeNode selectedNode = treeView.SelectedNode;

            // Kiểm tra tính hợp lệ:
            // 1. Node không null
            // 2. Node cha của nó phải có tên là "FPRDB Schemas"
            if (selectedNode == null || selectedNode.Parent == null || selectedNode.Parent.Text != "FPRDB Schemas")
            {
                XtraMessageBox.Show("Please select a schema from the 'FPRDB Schemas' folder to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string schemaName = selectedNode.Text;

            try
            {
                // Truy vấn ngược lại DTO từ AppStates dựa vào Tên (Text) của Node
                FPRDBSchemaDTO schemaToDelete = AppStates.loadFPRDBSchemas.FirstOrDefault(s => s.schemaName == schemaName);

                if (schemaToDelete == null)
                {
                    XtraMessageBox.Show("Schema data not found in system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //// --- Logic kiểm tra ràng buộc ---
                //var allRelations = this.databaseService.getFPRDBRelations();
                //var dependentRelations = allRelations.Where(r => r.fprdbSchema != null && r.fprdbSchema.schemaName == schemaName)
                //                                     .Select(r => r.relName).ToList();

                //if (dependentRelations.Count > 0)
                //{
                //    string rellList = string.Join(", ", dependentRelations);
                //    XtraMessageBox.Show($"Cannot delete schema '{schemaName}' because it is referenced by: {rellList}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                // --- Thực hiện xóa ---
                if (XtraMessageBox.Show($"Are you sure you want to delete schema '{schemaName}'?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        this.fprdbSchemaSerivce.removeFPRDBSchema(schemaToDelete);
                        XtraMessageBox.Show("Schema deleted successfully!");
                        AppStates.loadFPRDBSchemas = this.databaseService.getFPRDBSchemas();
                        reLoadDatabaseTree();
                        ReloadTabs();
                    }
                    catch (SemanticException ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Semantic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (InvalidOperationException ex)
                    {
                        HandlingExceptionViaErrorNotification.handlingInvalidOperationException(ex);
                    }
                    catch (UnderlyingStorageEngineCRUDException ex)
                    {
                        XtraMessageBox.Show($"Error: {ex.Message}", "UNDERLYING STORAGE MECHANISM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //this.DialogResult = DialogResult.Abort;
                    }
                }
            }
            catch (Exception ex) { /* Handle ex */ }
        }
        #region Tab Page Relation
        private void iNewRelation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (frmNewRelation childForm = new frmNewRelation(compRoot))
                {
                    if (childForm.ShowDialog() == DialogResult.OK)
                        reLoadDatabaseTree();
                }
            }
            catch(InvalidOperationException ex)
            {
                HandlingExceptionViaErrorNotification.handlingInvalidOperationException(new InvalidOperationException("Database isn't loaded"));
            }

        }

        private void iDeleteRelation_ItemClick(object sender, ItemClickEventArgs e)
        {
            TreeNode selectedNode = treeView.SelectedNode;

            // Kiểm tra: Node cha phải là "Relation"
            if (selectedNode == null || selectedNode.Parent == null || selectedNode.Parent.Text != "Relation")
            {
                XtraMessageBox.Show("Please select a relation from the 'Relation' folder to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string relName = selectedNode.Text;

            try
            {
                // Tìm DTO tương ứng trong danh sách Relations
                FPRDBRelationDTO relToDelete = AppStates.loadFPRDBSchemaRelations.FirstOrDefault(r => r.relName == relName);

                if (relToDelete == null) return;

                if (XtraMessageBox.Show($"Are you sure you want to delete relation '{relName}'?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Giả sử bạn dùng executeUpdate hoặc hàm remove trong service
                    // bool isDeleted = this.databaseService.removeFPRDBRelation(relToDelete); 

                    try
                    {
                        this.fprdbRelationService.removeFPRDBRelation(relToDelete);
                        // Cập nhật lại UI
                        XtraMessageBox.Show("Relation deleted successfully!");
                        AppStates.loadFPRDBSchemaRelations = this.databaseService.getFPRDBRelations();
                        reLoadDatabaseTree();
                        ReloadTabs();
                    }
                    catch (SemanticException ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Semantic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (InvalidOperationException ex)
                    {
                        HandlingExceptionViaErrorNotification.handlingInvalidOperationException(ex);
                    }
                    catch (UnderlyingStorageEngineCRUDException ex)
                    {
                        XtraMessageBox.Show($"Error: {ex.Message}", "UNDERLYING STORAGE MECHANISM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex) { /* Handle ex */ }
        }
        #endregion
        // 2 hàm parse dữ liệu xuống dưới Grid liệt kê giá trị
        private void gridView3_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridView3.FocusedColumn == null) return;
            var cellValue = gridView3.GetFocusedRowCellValue(gridView3.FocusedColumn);
            string fuzzyProbalisticValue = cellValue?.ToString() ?? string.Empty;
            _currentEditingColumn = gridView3.FocusedColumn.FieldName;
            _currentEditingRow = gridView3.FocusedRowHandle;
            _currentEditingColumnType = gridView3.FocusedColumn.Tag?.ToString() ?? string.Empty;
            LoadFuzzyProbalisticValueDetail(fuzzyProbalisticValue);
        }
        private void gridView3_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (gridView3.FocusedColumn == null) return;
            var cellValue = gridView3.GetFocusedRowCellValue(gridView3.FocusedColumn);
            string fuzzyProbalisticValue = cellValue?.ToString() ?? string.Empty;
            _currentEditingColumn = gridView3.FocusedColumn.FieldName;
            _currentEditingRow = gridView3.FocusedRowHandle;
            _currentEditingColumnType = gridView3.FocusedColumn.Tag?.ToString() ?? string.Empty;

            string fldName = this._currentEditingColumn;
            this.selectedField = this._selectedRelation.fprdbSchema.fields.FirstOrDefault(n => n.getFieldName() == fldName);
            this.selectedFieldType = selectedField.getFieldInfo().getType();
            this.isSelectedFieldText = selectedFieldType == FieldType.CHAR || selectedFieldType == FieldType.VARCHAR || selectedFieldType == FieldType.DIST_FUZZYSET_TEXT;

            LoadFuzzyProbalisticValueDetail(fuzzyProbalisticValue);
        }
        // Hàm ngăn không cho xóa hết dòng (đối với cột khóa chính)
        private void gridView4_RowDeleting(object sender, DevExpress.Data.RowDeletingEventArgs e)
        {
            DataTable dt = gridControlValueRelation.DataSource as DataTable;
            if (dt == null) return;

            var schema = _selectedRelation.fprdbSchema;
            List<string> pks = schema.primarykey;

            if (!pks.Contains(_currentEditingColumn)) return;

            int validRowCount = dt.Rows.Cast<DataRow>()
                .Count(r => r.RowState != DataRowState.Deleted) - 1;

            if (validRowCount <= 0)
            {
                e.Cancel = true;
                MessageBox.Show("Primary key cannot be empty! At least 1 row required.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Navigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType != NavigatorButtonType.Remove
                && e.Button.ButtonType != NavigatorButtonType.EndEdit) return;

            if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                e.Handled = true;
                DataRowView currentRow = gridView3.GetFocusedRow() as DataRowView;
                DataRow row = currentRow.Row;
                var schema = _selectedRelation.fprdbSchema;
                List<string> pks = schema.primarykey;

                string sbRow = $"DELETE FROM {this._selectedRelation.relName} WHERE";
                List<(string, string, string)> procssedFProbValue;

                foreach (var pk in pks)
                {
                    string val = gridView3.GetFocusedRowCellValue(pk) as string;
                    procssedFProbValue = FuzzyProbabilisticValueUtilities.extractValuesIntervalProbabilitiesAsString(val);
                    //[not done] not support null value
                    //if (val == null || val == DBNull.Value)
                    //{
                    //    sbRow.AppendLine($"{pk}: (empty)");
                    //    continue;
                    //}

                    sbRow += $" ({pk}={procssedFProbValue[0].Item1})[1,1] AND";
                }
                int trailingAND = sbRow.LastIndexOf("AND");
                sbRow = sbRow.Substring(0, trailingAND);
                try
                {
                    this.sqlProcessor.executeUpdate(sbRow);
                    row.Delete();
                    row.AcceptChanges();
                }
                catch (SemanticException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Semantic error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (InvalidOperationException ex)
                {
                    HandlingExceptionViaErrorNotification.handlingInvalidOperationException(ex);
                }

            }
            else
            {
                gridView3.CloseEditor();
                gridView3.UpdateCurrentRow();

                DataRowView currentRow = gridView3.GetFocusedRow() as DataRowView;

                if (currentRow != null)
                {
                    DataRow row = currentRow.Row;
                    var schema = _selectedRelation.fprdbSchema;
                    List<Field> fields = schema.fields;
                    string sbRow;
                    if (row.RowState == DataRowState.Added)
                    {
                        sbRow = $"INSERT INTO {this._selectedRelation.relName} ( ";
                        string cellValue;
                        foreach (Field f in fields)
                        {
                            cellValue = currentRow[f.getFieldName()] as string;
                            if (cellValue == null || cellValue == "")
                            {
                                XtraMessageBox.Show($"FPRDB doesn't support NULL attribute value yet. Attribute {f.getFieldName()} is null", "FPRDB SQL syntax error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            sbRow += $" {f.getFieldName()},";
                        }
                        sbRow = sbRow.TrimEnd(',') + ")";
                        sbRow += " VALUES ( ";
                        foreach (Field field in fields)
                        {
                            sbRow += $"{currentRow[field.getFieldName()]},";
                        }
                        sbRow = sbRow.TrimEnd(',') + " )";
                        try
                        {
                            this.sqlProcessor.executeUpdate(sbRow);
                            row.AcceptChanges();
                        }
                        catch (MismatchTokenType ex)
                        {
                            row.RejectChanges();
                            gridView3.RefreshData();
                            SyncDetailGridWithMaster();
                            XtraMessageBox.Show(ex.Message, "FPRDB SQL syntax error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (SQLSyntaxException ex)
                        {
                            row.RejectChanges();
                            gridView3.RefreshData();
                            SyncDetailGridWithMaster();
                            XtraMessageBox.Show($"Near token {ex.nearToken} at column line {ex.line}, {ex.column}: {ex.Message}", "FPRDB SQL syntax error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (SemanticException ex)
                        {
                            row.RejectChanges();
                            gridView3.RefreshData();
                            SyncDetailGridWithMaster();
                            XtraMessageBox.Show(ex.Message, "Semantic error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (InvalidOperationException ex)
                        {
                            row.RejectChanges();
                            gridView3.RefreshData();
                            SyncDetailGridWithMaster();
                            HandlingExceptionViaErrorNotification.handlingInvalidOperationException(ex);
                        }
                    }
                    else if (row.RowState == DataRowState.Modified)
                    {
                        List<string> pks = schema.primarykey;
                        List<string> setClauses = new List<string>();
                        foreach (var field in fields)
                        {
                            string fName = field.getFieldName();
                            object newVal = row[fName, DataRowVersion.Current];

                            //[not done] not supported null yet
                            //string formattedVal = (newVal == DBNull.Value) ? "NULL" : $"'{newVal.ToString().Replace("'", "''")}'";
                            string formattedVal = newVal as string;
                            setClauses.Add($"{fName} = {formattedVal}");
                        }

                        // 2. Xây dựng phần WHERE (Dữ liệu ĐỊNH DANH CŨ)
                        string whereClause = "WHERE";
                        foreach (string pkName in pks)
                        {
                            string oldPkVal = row[pkName, DataRowVersion.Original] as string;

                            //[not done] not supported null yet
                            //string formattedOldVal = (oldPkVal == DBNull.Value) ? "NULL" : $"'{oldPkVal.ToString().Replace("'", "''")}'";
                            oldPkVal = this.extractValueFromTrueExactFuzzyProbabilisitcValue(oldPkVal);
                            whereClause += $" ({pkName} = {oldPkVal})[1,1] AND";
                        }
                        int trailingAND = whereClause.LastIndexOf("AND");
                        whereClause = whereClause.Substring(0, trailingAND);

                        string fprdbUpdateSQL;
                        try
                        {
                            for (int i = 0; i < setClauses.Count; ++i)
                            {
                                fprdbUpdateSQL = $"UPDATE {this._selectedRelation.relName} SET {setClauses[i]} " + whereClause;
                                this.sqlProcessor.executeUpdate(fprdbUpdateSQL);
                            }
                            row.AcceptChanges();
                        }
                        catch (MismatchTokenType ex)
                        {
                            row.RejectChanges();
                            gridView3.RefreshData();
                            SyncDetailGridWithMaster();
                            XtraMessageBox.Show(ex.Message, "FPRDB SQL syntax error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (SQLSyntaxException ex)
                        {
                            row.RejectChanges();
                            gridView3.RefreshData();
                            SyncDetailGridWithMaster();
                            XtraMessageBox.Show($"Near token {ex.nearToken} at column line {ex.line}, {ex.column}: {ex.Message}", "FPRDB SQL syntax error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (SemanticException ex)
                        {
                            row.RejectChanges();
                            gridView3.RefreshData();
                            SyncDetailGridWithMaster();
                            XtraMessageBox.Show(ex.Message, "Semantic error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (InvalidOperationException ex)
                        {
                            row.RejectChanges();
                            gridView3.RefreshData();
                            SyncDetailGridWithMaster();
                            HandlingExceptionViaErrorNotification.handlingInvalidOperationException(ex);
                            
                        }

                    }
                }
            }
        }
        private void SyncDetailGridWithMaster()
        {
            // 1. Kiểm tra an toàn để tránh lỗi NullReference
            if (gridView3.FocusedColumn == null || gridView3.IsFilterRow(gridView3.FocusedRowHandle))
                return;

            // 2. Lấy giá trị hiện tại của ô đang chọn (sau khi đã Reject hoặc thay đổi)
            var cellValue = gridView3.GetFocusedRowCellValue(gridView3.FocusedColumn);
            string fuzzyString = cellValue?.ToString() ?? string.Empty;

            // 3. Cập nhật các biến trạng thái nếu cần thiết để đồng bộ logic
            _currentEditingColumn = gridView3.FocusedColumn.FieldName;
            _currentEditingRow = gridView3.FocusedRowHandle;
            _currentEditingColumnType = gridView3.FocusedColumn.Tag?.ToString() ?? string.Empty;

            // 4. Gọi hàm load dữ liệu xuống bảng dưới
            LoadFuzzyProbalisticValueDetail(fuzzyString);
        }
        //{("DT201",[1,1])}->DT201
        private string extractValueFromTrueExactFuzzyProbabilisitcValue(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            int startOfPossibleValue = input.IndexOf('(');//( in (1,[1,1])
            if (startOfPossibleValue == -1)
                throw new InvalidOperationException("invalid fuzzy probabilistic value");
            int commaDelimitPossibleValueAndIntervalProbbability = input.IndexOf(',');//first , in (1,[1,1])
            if (commaDelimitPossibleValueAndIntervalProbbability == -1)
                throw new InvalidOperationException("invalid fuzzy probabilistic value");
            string possibleValue = input.Substring(startOfPossibleValue + 1, commaDelimitPossibleValueAndIntervalProbbability - 1 - startOfPossibleValue);
            return possibleValue;
        }
        private void gridControlRelation_Click(object sender, EventArgs e)
        {

        }

        private void iExportFS_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraMessageBox.Show("This feature is not available in the current version.",
                        "Feature Unavailable",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
            return;

            try
            {
                SaveFileDialog DialogNew = new SaveFileDialog();
                DialogNew.Title = "Export Fuzzy Set File";
                DialogNew.Filter = "Fuzzy Set Files (*.fset)|*.fset";
                DialogNew.DefaultExt = "fset";
                DialogNew.AddExtension = true;
                DialogNew.RestoreDirectory = true;
                DialogNew.InitialDirectory = GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());
                DialogNew.SupportMultiDottedExtensions = true;

                if (DialogNew.ShowDialog() == DialogResult.OK)
                {
                    //this.databaseService.createDB(DialogNew.FileName);
                    XtraMessageBox.Show($"Export fuzzy set successfully!\n{DialogNew.FileName}", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FileNotFoundException ex)
            {
                XtraMessageBox.Show($"Directory doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (IOException ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void iImportFS_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraMessageBox.Show("This feature is not available in the current version.",
                        "Feature Unavailable",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
            return;

            OpenFileDialog DialogOpen = new OpenFileDialog();
            DialogOpen.Title = "Import Fuzzy Set File";
            // Chỉ lọc ra các file có đuôi .pdb hoặc .sqlite
            DialogOpen.Filter = "Fuzzy Set Files (*.fset)|*.fset";
            DialogOpen.DefaultExt = "fset";
            DialogOpen.InitialDirectory = GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());

            if (DialogOpen.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //this.databaseService.loadDB(DialogOpen.FileName);
                    XtraMessageBox.Show($"Import fuzzy set successfully!\n{DialogOpen.FileName}", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (IOException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void barButtonRelationships_ItemClick(object sender, ItemClickEventArgs e)
        {
            new frmFKRelationships(compRoot, _selectedRelation).ShowDialog();
        }

        private void xtraTabControlDatabase_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            if (e.Page?.Tag?.ToString() == "QueryTab")
            {
                ribbonControl.SelectedPage = QueryRibbonPage;
            }
            else if (e.Page == SchemaxtraTabPage)
            {
                ribbonControl.SelectedPage = SchemaRibbonPage;
            }
            else
            {
                ribbonControl.SelectedPage = RelationRibbonPage;
            }
        }
        private bool CloseTabPage(XtraTabPage page)
        {
            if (page != null && page.Controls.Count > 0 && page.Controls[0] is ucQueryEditor uc)
            {
                if (uc.IsTemporary && string.IsNullOrWhiteSpace(uc.QueryText))
                {
                    return true;
                }
                if (page.Text.EndsWith("*"))
                {
                    xtraTabControlDatabase.SelectedTabPage = page;
                    DialogResult result = XtraMessageBox.Show(
                        $"Do you want to save changes for {page.Text.TrimEnd('*')}?",
                        "Confirm Close",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        if (uc.IsDBFile)
                            return SaveCurrentFile(true);
                        return SaveCurrentFile(); // Trả về true nếu lưu thành công, false nếu Cancel diaog
                    }
                    else if (result == DialogResult.No)
                    {
                        return true; // Người dùng chấp nhận mất dữ liệu
                    }
                    else
                    {
                        return false; // Người dùng nhấn Cancel
                    }
                }
                return true;
            }
            return false;
        }
        private void xtraTabControlDatabase_CloseButtonClick(object sender, EventArgs e)
        {
            var arg = e as DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs;
            XtraTabPage page = arg.Page as XtraTabPage;

            if (CloseTabPage(page))
            {
                xtraTabControlDatabase.TabPages.Remove(page);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = xtraTabControlDatabase.TabPages.Count - 1; i >= 0; i--)
            {
                XtraTabPage page = xtraTabControlDatabase.TabPages[i];
                if (page?.Tag?.ToString() == "QueryTab")
                {
                    if (CloseTabPage(page))
                    {
                        xtraTabControlDatabase.TabPages.Remove(page);
                    }
                    else
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }

        }

        private void barButtonCreateNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (frmNewQuery childForm = new frmNewQuery(this.compRoot.getInDataBaseSQLFileService()))
                {
                    if (childForm.ShowDialog() == DialogResult.OK)
                    {
                        CreateQueryTab(childForm.QueryName, "", "", true);
                        SetQueryTabState(true);
                        this.reLoadDatabaseTree();
                    }
                }
            }
            catch(InvalidOperationException ex)
            {
                HandlingExceptionViaErrorNotification.handlingInvalidOperationException(ex);
            }
        }

        private void barButtonSaveInDB_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveCurrentFile(true);
        }

        private void barButtonDeleteInDB_ItemClick(object sender, ItemClickEventArgs e)
        {
            TreeNode selectedNode = treeView.SelectedNode;

            // Kiểm tra tính hợp lệ:
            // 1. Node không null
            // 2. Node cha của nó phải có tên là "Query"
            if (selectedNode == null || selectedNode.Parent == null || selectedNode.Parent.Text != "Query")
            {
                XtraMessageBox.Show("Please select a query from the 'Query' folder to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string queryName = selectedNode.Text;
            try
            {
                // Truy vấn ngược lại DTO từ AppStates dựa vào Tên (Text) của Node
                //FPRDBSchemaDTO schemaToDelete = AppStates.loadFPRDBSchemas.FirstOrDefault(s => s.schemaName == schemaName);

                //if (schemaToDelete == null)
                //{
                //    XtraMessageBox.Show("Schema data not found in system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                if (XtraMessageBox.Show($"Are you sure you want to delete query '{queryName}'?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        this.inDataBaseSQLFileService.deleteFile(queryName);
                        XtraMessageBox.Show("Query deleted successfully!");
                        string tabNameToRemove = queryName + ".fprdbsql" + " [Database]";
                        XtraTabPage pageToRemove = xtraTabControlDatabase.TabPages.FirstOrDefault(p => p.Text.Contains(tabNameToRemove));

                        if (pageToRemove != null)
                        {
                            xtraTabControlDatabase.TabPages.Remove(pageToRemove);
                        }
                        AppStates.listOfInDatabaseSQLFiles = this.inDataBaseSQLFileService.getInDatabaseSQLFileNames();
                        reLoadDatabaseTree();
                    }
                    catch (InvalidOperationException ex)
                    {
                        HandlingExceptionViaErrorNotification.handlingInvalidOperationException(ex);
                    }
                    catch (UnderlyingStorageEngineCRUDException ex)
                    {
                        XtraMessageBox.Show($"Error: {ex.Message}", "UNDERLYING STORAGE MECHANISM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AppStates.ISAppStateFullyLoad == false)
            {
                XtraMessageBox.Show("Database isn't loaded", "Invalid Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (xtraTabControlDatabase.SelectedTabPage?.Controls[0] is ucQueryEditor uc)
            {
                try
                {
                    string expression = uc.GetSelectedQuery();
                    float probabilisticInterpretation = this.sqlProcessor.calculateProbabilisticInterpretationForRelationOnFuzzySetsExpression(expression);
                    uc.memoEditMessageUC.Text = probabilisticInterpretation.ToString();
                    uc.xtraTabControlResult.SelectedTabPage = uc.MessageTabPage;
                }
                catch (InvalidOperationException ex)
                {
                    uc.memoEditMessageUC.Text = $"[Invalid operation]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (SemanticException ex)
                {
                    uc.memoEditMessageUC.Text = $"[SQL Semantic Error]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (InvalidCastException ex)
                {
                    uc.memoEditMessageUC.Text = $"[Invalid Cast Exception Error]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (MismatchTokenType ex)
                {
                    uc.memoEditMessageUC.Text = $"[Token Mismatch]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (NotSupportedException ex)
                {
                    uc.memoEditMessageUC.Text = $"[Not Supported]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (SQLSyntaxException ex)
                {
                    uc.memoEditMessageUC.Text = $"[SQL Syntax Error]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (UnderlyingStorageEngineCRUDException ex)
                {
                    XtraMessageBox.Show($"Error: {ex.Message}", "UNDERLYING STORAGE MECHANISM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //this.DialogResult = DialogResult.Abort;
                }
                finally
                {
                    // Đảm bảo splitContainer luôn hiện để xem được kết quả/lỗi
                    uc.splitContainer.PanelVisibility = SplitPanelVisibility.Both;
                }
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AppStates.ISAppStateFullyLoad == false)
            {
                XtraMessageBox.Show("Database isn't loaded", "Invalid Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (xtraTabControlDatabase.SelectedTabPage?.Controls[0] is ucQueryEditor uc)
            {
                try
                {
                    string expression = uc.GetSelectedQuery();
                    TheoryCheckSelectPlan p = this.sqlProcessor.calculateProbabilisticInterpretationForSelectionExpressionOnSpecifiedTuples(expression);
                    TheoryCheckSelectScan s = (TheoryCheckSelectScan)p.open();

                    DataTable resultForGridView = new DataTable();
                    FPRDBSchema schema = p.getSchema();

                    //create columns for grid view of query result
                    foreach (Field f in schema.getFields())
                    {
                        resultForGridView.Columns.Add(f.getFieldName(), typeof(string));
                    }
                    resultForGridView.Columns.Add("Probabilistic Interpretation", typeof(string));

                    //Extract the result for grid view
                    string[] tupleForGridView = new string[schema.getFields().Count + 1];
                    Field field;
                    List<Field> fields = schema.getFields();
                    while (s.next())
                    {
                        for (int i = 0; i < schema.getFields().Count; ++i)
                        {
                            field = fields[i];
                            switch (field.getFieldInfo().getType())
                            {
                                case FieldType.INT:
                                case FieldType.DIST_FUZZYSET_INT:
                                    tupleForGridView[i] = s.getFieldContent<int>(field.getFieldName()).ToString();
                                    break;
                                case FieldType.FLOAT:
                                case FieldType.DIST_FUZZYSET_FLOAT:
                                case FieldType.CONT_FUZZYSET:
                                    //tupleForGridView.Add((s.getFieldContent<float>(field.getFieldName())).ToString());
                                    tupleForGridView[i] = s.getFieldContent<float>(field.getFieldName()).ToString();
                                    break;
                                case FieldType.CHAR:
                                case FieldType.VARCHAR:
                                case FieldType.DIST_FUZZYSET_TEXT:
                                    //tupleForGridView.Add((s.getFieldContent<string>(field.getFieldName())).ToString());
                                    tupleForGridView[i] = s.getFieldContent<string>(field.getFieldName()).ToString();
                                    break;
                                case FieldType.BOOLEAN:
                                    //tupleForGridView.Add((s.getFieldContent<bool>(field.getFieldName())).ToString());
                                    tupleForGridView[i] = s.getFieldContent<bool>(field.getFieldName()).ToString();
                                    break;
                            }

                        }
                        (float lowerProb, float upperProb) = s.getCurrentTupleProbabilisticInterpretationForSelectionExpression();
                        tupleForGridView[schema.getFields().Count] = $"[{lowerProb},{upperProb}]";
                        resultForGridView.Rows.Add(tupleForGridView);


                    }
                    //bind the result to the grid control
                    uc.GridResult.DataSource = resultForGridView;
                    //Yêu cầu GridView tự động tạo các cột dựa trên DataTable
                    uc.GridViewResult.PopulateColumns();

                    // Ghi thông báo thành công vào MemoEdit
                    uc.ViewResult();
                    uc.memoEditMessageUC.Text = $"Query executed successfully";
                }
                catch (SQLSyntaxException ex)
                {
                    uc.memoEditMessageUC.Text = $"[SQL Syntax Error]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (SemanticException ex)
                {
                    uc.memoEditMessageUC.Text = $"[SQL Semantic Error]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (InvalidCastException ex)
                {
                    uc.memoEditMessageUC.Text = $"[Invalid Cast Exception Error]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (MismatchTokenType ex)
                {
                    uc.memoEditMessageUC.Text = $"[Token Mismatch]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (NotSupportedException ex)
                {
                    uc.memoEditMessageUC.Text = $"[Not Supported]\r\n{ex.Message}";
                    uc.ViewError();
                }
                catch (UnderlyingStorageEngineCRUDException ex)
                {
                    XtraMessageBox.Show($"Error: {ex.Message}", "UNDERLYING STORAGE MECHANISM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //this.DialogResult = DialogResult.Abort;
                }
                finally
                {
                    // Đảm bảo splitContainer luôn hiện để xem được kết quả/lỗi
                    uc.splitContainer.PanelVisibility = SplitPanelVisibility.Both;
                }

            }
        }

        private void closdeDatabaseButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AppStates.ISAppStateFullyLoad == false)
                return;

            DialogResult result = XtraMessageBox.Show($"Are you sure want to close database {this.databaseService.getDatabaseName()}?", "Close The Current Database", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                this.databaseService.closeDB();
                AppStates.clear();
                this.unLoadDatabaseTree();
                for (int i = xtraTabControlDatabase.TabPages.Count - 1; i >= 0; i--)
                {
                    XtraTabPage page = xtraTabControlDatabase.TabPages[i];
                    if (page?.Tag?.ToString() == "QueryTab")
                    {
                        CloseTabPage(page);
                        xtraTabControlDatabase.TabPages.Remove(page);
                    }
                }
                isDatabaseLoaded = false;
                changeStatusTab();
                AppStates.ISAppStateFullyLoad = false;
            }

            //them code cua cau Dong Quan voi cac file FPRDB-SQL

        }
    }
}
