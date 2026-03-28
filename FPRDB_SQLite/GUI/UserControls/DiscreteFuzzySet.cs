using BLL;
using BLL.DomainObject;
using BLL.DTO;
using DevExpress.CodeParser;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FPRDB_SQLite.GUI.frmManageFuzzySet;

namespace FPRDB_SQLite.GUI.UserControls
{
    public partial class DiscreteFuzzySet : DevExpress.XtraEditors.XtraUserControl
    {
        private FuzzySetDTO selectedFuzzySet;
        private readonly BaseEdit[] textFields;
        private class FuzzySetRow
        {
            public string Value { get; set; }
            public float MembershipDegree { get; set; }
        }
        public DiscreteFuzzySet()
        {
            InitializeComponent();
            //cboDataType.Properties.Items.AddRange(new object[] { FieldType.INT, FieldType.FLOAT, FieldType.VARCHAR });
            textFields = new BaseEdit[] { txtNameDiscFuzzy, cboDataType };
            setValidationRules();
            InitGrid();
        }
        public void setDefineDomain4FuzzySet(List<FieldType> domains)
        {
            cboDataType.Properties.Items.AddRange(domains);
        }
        // VALIDATE INPUT
        // Hàm set rules cho các control
        private void setValidationRules()
        {
            var notEmptyRule = new ConditionValidationRule
            {
                ConditionOperator = ConditionOperator.IsNotBlank,
                ErrorText = "Required Field!!"
            };

            foreach (var control in textFields)
            {
                dxValidationProvider1.SetValidationRule(control, notEmptyRule);
            }
        }
        // Hàm validate toàn bộ control, được gọi từ form cha
        public bool ValidateControls()
        {
            return dxValidationProvider1.Validate();
        }
        // Hàm validate từng ô khi người dùng edit trong GridControl
        private void grdvDiscFuzzy_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            GridView view = sender as GridView;
            string fieldName = view.FocusedColumn.FieldName;
            string input = e.Value?.ToString()?.Trim();
            switch (fieldName)
            {
                case "Value":
                    validateValueColumn(e, input);
                    break;

                case "MembershipDegree":
                    validateMembershipColumn(e, input);
                    break;
            }
        }
        // Hàm validate cột Value
        private void validateValueColumn(BaseContainerValidateEditorEventArgs e, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                e.Valid = false;
                e.ErrorText = "Value cannot be empty!";
                return;
            }

            var type = getFuzzySetType();
            if (type == FieldType.INT && !int.TryParse(input, out _))
                SetError(e, "Value must be an integer!");
            else if (type == FieldType.FLOAT && !float.TryParse(input, out _))
                SetError(e, "Value must be a floating-point number!");
        }
        // Hàm validate cột MembershipDegree
        private void validateMembershipColumn(BaseContainerValidateEditorEventArgs e, string input)
        {
            if (!float.TryParse(input, out float degree) || degree < 0 || degree > 1)
                SetError(e, "Membership Degree must be a number between 0 and 1!");
        }
        // Hàm helper để set lỗi cho ô đang edit
        private void SetError(BaseContainerValidateEditorEventArgs e, string message)
        {
            e.Valid = false;
            e.ErrorText = message;
        }
        // Hàm khởi tạo GridControl với một BindingList rỗng
        public void InitGrid()
        {
            grdcDiscFuzzy.Enabled = true;
            BindingList<FuzzySetRow> rows = new BindingList<FuzzySetRow>();
            grdcDiscFuzzy.DataSource = rows;

            grdvDiscFuzzy.PopulateColumns();
            grdvDiscFuzzy.BestFitColumns();
        }
        // Sự kiện khi người dùng chuyển dòng hoặc cột trong GridControl, đảm bảo commit dữ liệu đang edit vào DataSource
        private void grdvDiscFuzzy_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            CommitCurrentEdit();
        }
        private void grdvDiscFuzzy_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            CommitCurrentEdit();
        }
        // Hàm helper để commit editor đang edit vào DataSource
        private void CommitCurrentEdit()
        {
            if (grdvDiscFuzzy.IsEditing || grdvDiscFuzzy.IsEditorFocused)
            {
                grdvDiscFuzzy.PostEditor();          // Commit editor đang edit
                grdvDiscFuzzy.UpdateCurrentRow();    // Cập nhật dòng hiện tại vào DataSource
            }
        }
        // Hàm lấy thông tin loại DiscreteFuzzySet từ UserControl để truyền ra form UI
        public FieldType getFuzzySetType()
        {
            return Enum.Parse<FieldType>(cboDataType.Text);
        }
        // Helper method để parse string từ GridControl về đúng kiểu dữ liệu
        private static T ParseTo<T>(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return default;

            if (typeof(T) == typeof(string)) return (T)(object)input;

            if (typeof(T) == typeof(int) && int.TryParse(input, out int i)) return (T)(object)i;
            if (typeof(T) == typeof(float) && float.TryParse(input, out float f)) return (T)(object)f;
            // Thêm double, decimal nếu cần sau này

            throw new FormatException($"Cannot parse '{input}' to {typeof(T).Name}");
        }
        // Hàm lấy dữ liệu từ UserControl để tạo một DiscreteFuzzySetDTO
        public DiscreteFuzzySetDTO<T> getDiscreteFuzzySet<T>()
        {
            var fuzzySetName = txtNameDiscFuzzy.Text;
            FieldType fuzzySetType = Enum.Parse<FieldType>(cboDataType.Text);
            var rows = grdvDiscFuzzy.DataSource as BindingList<FuzzySetRow>;

            var values = rows.Select(r => ParseTo<T>(r.Value?.ToString())).ToList();
            var degrees = rows.Select(r => (float)r.MembershipDegree).ToList();

            if(this.selectedFuzzySet!=null)
                return new DiscreteFuzzySetDTO<T>(values, degrees, this.selectedFuzzySet.oid, fuzzySetName, fuzzySetType);
            else
                return new DiscreteFuzzySetDTO<T>(values, degrees, fuzzySetName, fuzzySetType);

        }
        // Phương thức này sẽ được gọi khi người dùng chọn một FuzzySet
        public void LoadFuzzySet(FuzzySetDTO fuzzySet)
        {
            this.selectedFuzzySet = fuzzySet;
            txtNameDiscFuzzy.Text = fuzzySet.fuzzySetName;
            cboDataType.Text = fuzzySet.fuzzySetType.ToString();

            if (fuzzySet is DiscreteFuzzySetDTO<int> discreteInt)
            {
                BindDiscrete(discreteInt.valueSet, discreteInt.membershipDegreeSet);
            }
            else if (fuzzySet is DiscreteFuzzySetDTO<float> discreteFloat)
            {
                BindDiscrete(discreteFloat.valueSet, discreteFloat.membershipDegreeSet);
            }
            else if (fuzzySet is DiscreteFuzzySetDTO<string> discreteText)
            {
                BindDiscrete(discreteText.valueSet, discreteText.membershipDegreeSet);
            }
        }

        // Bind dữ liệu cho GridControl, sử dụng kiểu generic để tái sử dụng cho cả 3 loại dữ liệu
        private void BindDiscrete<TDomain>(List<TDomain> valueSet, List<float> membershipDegreeSet)
        {
            // Ghép dữ liệu theo index: mỗi phần tử values[i] đi với memberships[i]
            var rowData = Enumerable.Range(0, valueSet.Count)
                .Select(i => new FuzzySetRow
                {
                    Value = valueSet[i].ToString(),
                    MembershipDegree = membershipDegreeSet[i]
                });

            BindingList<FuzzySetRow> rows = new BindingList<FuzzySetRow>(rowData.ToList());
            // Gán danh sách rows vào GridControl
            grdcDiscFuzzy.DataSource = rows;

            grdvDiscFuzzy.PopulateColumns();
            grdvDiscFuzzy.BestFitColumns();
        }

    }
}
