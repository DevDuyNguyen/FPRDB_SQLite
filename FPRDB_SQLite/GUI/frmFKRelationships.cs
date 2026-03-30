using BLL.Common;
using BLL.DTO;
using BLL.Enums;
using BLL.Services;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using GUI.GlobalStates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPRDB_SQLite.GUI
{
    public partial class frmFKRelationships : DevExpress.XtraEditors.XtraForm
    {
        private CompositionRoot compRoot;
        private ConstraintService service;
        private DatabaseService db;
        private FPRDBRelationDTO rel;
        private List<ConstraintDTO> relationships = new List<ConstraintDTO>();
        bool isAddNew = false;
        private class AttrRow
        {
            public string PKAttr { get; set; }
            public string FKAttr { get; set; }
        }
        //public class ConstraintDTO
        //{
        //    public string conName { get; set; }
        //    public string refRel { get; set; }
        //    public List<string> attrs { get; set; }
        //    public List<string> refAttrs { get; set; }
        //    public ConstraintDTO(string conName, string refRel, List<string> attrs, List<string> refAttrs)
        //    {
        //        this.conName = conName;
        //        this.refRel = refRel;
        //        this.attrs = attrs;
        //        this.refAttrs = refAttrs;
        //    }
        //}
        public frmFKRelationships(CompositionRoot compRoot, FPRDBRelationDTO rel)
        {
            InitializeComponent();
            this.compRoot = compRoot;
            this.service = compRoot.getConstraintService();
            this.db = compRoot.getDatabaseService();
            this.rel = rel;
            txtFKRelName.Text = rel.relName;
            txtFKRelName.ReadOnly = true;
            txtFKRelName.BackColor = Color.LightGray;
            grdcolPKAttr.FieldName = "PKAttr";
            grdcolFKAttr.FieldName = "FKAttr";
            // Load các relationships của relation được chọn vào ListBox
            loadRelationships();
        }
        // Hàm load các relationships của relation được chọn vào ListBox
        private void loadRelationships()
        {
            lstFKSelected.Items.Clear();
            //this.relationships = service.getReferenrialConstraints(rel);
            //foreach (var relationship in relationships)
            //{
            //    lstFKSelected.Items.Add(relationship.conName);
            //}
            //ConstraintDTO con1 = new ConstraintDTO("rel1", "student",
            //    new List<string> { "id", "name" },
            //    new List<string> { "id", "name" });
            //relationships.Add(con1);
            //lstFKSelected.Items.Add(con1.conName);
            this.relationships.AddRange(this.service.getReferenrialConstraints(this.rel));
            foreach(ConstraintDTO referentialConstr in this.relationships)
            {
                lstFKSelected.Items.Add(referentialConstr.conName);
            }
        }
        // Hàm set trạng thái (form không cho edit nếu không phải hành động thêm mới)
        private void SetUIState(bool isAdding)
        {
            txtFKName.ReadOnly = !isAdding;
            cboPKRelName.ReadOnly = !isAdding;
            grdviewMappingAttr.OptionsBehavior.Editable = isAdding;
            btnSave.Enabled = isAdding;

            txtFKName.BackColor = isAdding ? Color.White : Color.LightGray;
            cboPKRelName.BackColor = isAdding ? Color.White : Color.LightGray;
        }
        // Hàm load chi tiết của một relationship đã tồn tại
        public void LoadExistingRelation(ConstraintDTO constraint)
        {
            repositoryItemLookUpEditPK.DataSource = constraint.referencedAttributes;
            repositoryItemLookUpEditFK.DataSource = constraint.attributes;

            txtFKName.Text = constraint.conName;
            cboPKRelName.EditValue = constraint.referencedRelation;

            var mapping = constraint.referencedAttributes.Zip(constraint.attributes,
                          (pk, fk) => new AttrRow { PKAttr = pk, FKAttr = fk }).ToList();
            grdMappingAttr.DataSource = new BindingList<AttrRow>(mapping);
        }
        // Hàm load các control trống (được gọi khi bấm Add)
        public void LoadNewRelation()
        {
            isAddNew = true;
            cboPKRelName.EditValue = null;
            cboPKRelName.Properties.Items.Clear();
            grdMappingAttr.DataSource = new BindingList<AttrRow>();

            var otherRelNames = db.getFPRDBRelations()
                                  .Where(r => r.relName != rel.relName)
                                  .Select(r => r.relName).ToList();
            cboPKRelName.Properties.Items.AddRange(otherRelNames);

            List<string> currentRelFields = rel.fprdbSchema.fields.Select(f => f.getFieldName()).ToList();
            repositoryItemLookUpEditFK.DataSource = currentRelFields;
            repositoryItemLookUpEditFK.ValueMember = "";
            repositoryItemLookUpEditFK.DisplayMember = "";

            string timestamp = DateTime.Now.ToString("HHmmss");
            string newFKName = $"FK_{rel.relName}_{timestamp}";
            txtFKName.Text = newFKName;

            if (!lstFKSelected.Items.Contains(newFKName))
            {
                lstFKSelected.Items.Add(newFKName);
                lstFKSelected.SelectedItem = newFKName;
            }

            cboPKRelName.Focus();
        }
        // Hàm xử lý khi người dùng chọn một relationship trong ListBox
        private void lstFKSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFKSelected.SelectedItem == null)
            {
                grpFKDetail.Visible = false;
                return;
            }
            grpFKDetail.Visible = true;
            SetUIState(isAddNew);
            if (isAddNew)
            {
                lstFKSelected.SelectedItem = txtFKName.Text;
                return;
            }
            ConstraintDTO selectedRelationship = relationships.FirstOrDefault(r => r.conName == lstFKSelected.SelectedItem.ToString());
            LoadExistingRelation(selectedRelationship);
        }
        // Hàm xử lý khi click "Add" button
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (isAddNew) 
            {
                MessageBox.Show("Vui lòng lưu thông tin trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            LoadNewRelation();
        }
        // Hàm xử lý khi click "Delete" button
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstFKSelected.SelectedItem == null) return;

            int currentIndex = lstFKSelected.SelectedIndex;
            string selectedFK = lstFKSelected.SelectedItem.ToString();

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa Foreign Key '{selectedFK}' không?",
                                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {

                    ConstraintDTO itemToRemove = relationships.FirstOrDefault(r => r.conName == selectedFK);
                    this.service.removeConstraint(itemToRemove.oid);

                    if (itemToRemove != null) relationships.Remove(itemToRemove);

                    lstFKSelected.Items.RemoveAt(currentIndex);

                    if (lstFKSelected.Items.Count > 0)
                    {
                        int nextIndex = Math.Max(0, currentIndex - 1);
                        lstFKSelected.SelectedIndex = nextIndex;
                    }
                    else
                    {
                        lstFKSelected.SelectedIndex = -1;
                    }

                    MessageBox.Show("Đã xóa thành công!");
                    isAddNew = false;
                }
                catch (InvalidOperationException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Invalid Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // Hàm xử lý khi click "Save" button
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;

            grdviewMappingAttr.CloseEditor();
            grdviewMappingAttr.UpdateCurrentRow();

            string fkName = txtFKName.Text.Trim();
            string pkRelName = cboPKRelName.Text;
            FPRDBRelationDTO referencedRelation = AppStates.loadFPRDBSchemaRelations.FirstOrDefault(rel=>rel.relName==pkRelName);
            var rows = grdMappingAttr.DataSource as BindingList<AttrRow>;
            List<string> refAttrs = new List<string>();
            List<string> attrs = new List<string>();

            foreach (var row in rows)
            {
                refAttrs.Add(row.PKAttr);
                attrs.Add(row.FKAttr);
            }

            //demo
            //ConstraintDTO newCon = new ConstraintDTO(fkName,pkRelName,attrs, refAttrs);
            //relationships.Add(newCon);
            //MessageBox.Show("Lưu thành công!");
            //lstFKSelected.Items[lstFKSelected.SelectedIndex] = newCon.conName;
            //isAddNew = false;
            //SetUIState(false);

            try
            {
                ConstraintDTO newReferentialConstraint= this.service.createReferentialConstraint(fkName, this.rel, referencedRelation, attrs, refAttrs);
                MessageBox.Show("Lưu thành công!");
                lstFKSelected.Items[lstFKSelected.SelectedIndex] = newReferentialConstraint.conName;
                isAddNew = false;
                SetUIState(false);
            }
            catch (InvalidOperationException ex)
            {
                XtraMessageBox.Show(ex.Message, "Invalid Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Hàm xử lý khi click "Close" button
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Hàm load thuộc tính đối với tương ứng với relation được tham chiếu
        private void cboPKRelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRelName = cboPKRelName.EditValue?.ToString();
            if (string.IsNullOrEmpty(selectedRelName)) return;

            var allRelations = AppStates.loadFPRDBSchemaRelations;
            var selectedPKRel = allRelations.FirstOrDefault(r => r.relName == selectedRelName);

            if (selectedPKRel != null)
            {
                List<string> refFields = selectedPKRel.fprdbSchema.primarykey;

                repositoryItemLookUpEditPK.DataSource = refFields;
                repositoryItemLookUpEditPK.ValueMember = "";
                repositoryItemLookUpEditPK.DisplayMember = "";

                BindingList<AttrRow> mappingList = new BindingList<AttrRow>();
                foreach (var pkName in refFields)
                {
                    mappingList.Add(new AttrRow { PKAttr = null, FKAttr = null });
                }

                grdMappingAttr.DataSource = mappingList;

                grdviewMappingAttr.RefreshData();
            }
        }

        // Hàm validate nếu người dùng chọn thuộc tính đã có ở dòng khác
        private void grdviewMappingAttr_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            GridView view = sender as GridView;
            if (view == null) return;

            if (view.FocusedColumn.FieldName == "PKAttr" || view.FocusedColumn.FieldName == "FKAttr")
            {
                string currentValue = e.Value?.ToString();
                if (string.IsNullOrEmpty(currentValue)) return;

                for (int i = 0; i < view.RowCount; i++)
                {
                    if (i == view.FocusedRowHandle) continue;

                    object cellValue = view.GetRowCellValue(i, view.FocusedColumn);
                    if (cellValue != null && cellValue.ToString() == currentValue)
                    {
                        e.Valid = false;
                        e.ErrorText = "Giá trị này đã được chọn ở dòng khác!";
                        return;
                    }
                }
            }
        }

        // Hàm ẩn thuộc tính được chọn trong combo box nếu đã được chọn ở dòng khác
        private void grdviewMappingAttr_ShownEditor(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            if ((view.FocusedColumn.FieldName == "PKAttr" || view.FocusedColumn.FieldName == "FKAttr")
                && view.ActiveEditor is LookUpEdit edit)
            {
                var repositoryItem = view.FocusedColumn.RealColumnEdit as RepositoryItemLookUpEdit;
                List<string> originalList = repositoryItem.DataSource as List<string>;
                if (originalList == null) return;

                List<string> usedValues = new List<string>();
                for (int i = 0; i < view.RowCount; i++)
                {
                    if (i == view.FocusedRowHandle) continue;

                    object val = view.GetRowCellValue(i, view.FocusedColumn);
                    if (val != null && !string.IsNullOrEmpty(val.ToString()))
                    {
                        usedValues.Add(val.ToString());
                    }
                }

                var filteredList = originalList
                    .Where(x => !usedValues.Contains(x))
                    .ToList();

                edit.Properties.DataSource = filteredList;
            }
        }

        // Hàm validate dữ liệu (được gọi khi ấn Save hoặc ấn Add)
        private bool ValidateData()
        {
            // 1. Kiểm tra tên FK
            if (string.IsNullOrWhiteSpace(txtFKName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên Foreign Key!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 2. Kiểm tra bảng cha
            if (cboPKRelName.EditValue == null)
            {
                MessageBox.Show("Vui lòng chọn bảng tham chiếu (Primary Key Relation)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 3. Kiểm tra Grid Mapping
            var list = grdMappingAttr.DataSource as BindingList<AttrRow>;

            bool hasIncompleteRow = list.Any(row =>
                (string.IsNullOrEmpty(row.PKAttr) && !string.IsNullOrEmpty(row.FKAttr)) ||
                (!string.IsNullOrEmpty(row.PKAttr) && string.IsNullOrEmpty(row.FKAttr))
            );

            if (hasIncompleteRow)
            {
                MessageBox.Show("Tất cả các dòng mapping phải được chọn đầy đủ cả 2 cột!", "Thông báo");
                return false;
            }

            // Phải có ít nhất một dòng map đủ cả 2 cột
            bool hasValidPair = list.Any(x => !string.IsNullOrEmpty(x.PKAttr) && !string.IsNullOrEmpty(x.FKAttr));
            if (!hasValidPair)
            {
                MessageBox.Show("Bạn chưa cấu hình cặp thuộc tính tham chiếu nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}