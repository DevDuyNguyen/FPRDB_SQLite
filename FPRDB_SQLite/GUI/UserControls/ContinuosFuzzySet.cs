using BLL;
using BLL.DTO;
using DevExpress.Mvvm.POCO;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPRDB_SQLite.GUI.UserControls
{
    public partial class ContinuosFuzzySet : DevExpress.XtraEditors.XtraUserControl
    {
        private readonly BaseEdit[] textFields;
        private FuzzySetDTO selectedFuzzySet;
        public ContinuosFuzzySet()
        {
            InitializeComponent();
            textFields = new BaseEdit[]
            {
                txtNameConsFuzzy,
                txtBotLeft, 
                txtTopLeft, 
                txtTopRight, 
                txtBotRight
            };
            setValidationRules();
        }

        // Hàm set rules cho các control
        private void setValidationRules()
        {
            // Cho phép chuyển focus sang nút khác dù đang có lỗi
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            dxValidationProvider1.ValidationMode = DevExpress.XtraEditors.DXErrorProvider.ValidationMode.Auto;

            // Rule dùng chung cho ô đầu tiên (Name)
            var notEmptyRule = new ConditionValidationRule
            {
                ConditionOperator = ConditionOperator.IsNotBlank,
                ErrorText = "Trường này không được để trống!",
                ErrorType = ErrorType.Critical
            };
            dxValidationProvider1.SetValidationRule(textFields[0], notEmptyRule);

            // Xử lý các ô nhập số
            foreach (var control in textFields.Skip(1))
            {
                control.Validating += (s, e) =>
                {
                    if (s is DevExpress.XtraEditors.TextEdit edit)
                    {
                        string input = edit.Text.Trim();
                        ConditionValidationRule customRule = new ConditionValidationRule();
                        customRule.ErrorType = ErrorType.Critical;

                        if (string.IsNullOrEmpty(input))
                        {
                            customRule.ConditionOperator = ConditionOperator.IsNotBlank;
                            customRule.ErrorText = "Required Field!!";
                        }
                        else if (!double.TryParse(input, out _))
                        {
                            customRule.ConditionOperator = ConditionOperator.Equals;
                            customRule.Value1 = "Invalid@";
                            customRule.ErrorText = "Vui lòng chỉ nhập số.";
                        }
                        else
                        {
                            dxValidationProvider1.SetValidationRule(edit, null);
                            dxValidationProvider1.RemoveControlError(edit);
                            return;
                        }

                        dxValidationProvider1.SetValidationRule(edit, customRule);
                        dxValidationProvider1.Validate(edit);
                    }
                };
            }
        }

        // Ham validate toàn bộ control, được gọi từ form cha
        public bool ValidateControls()
        {
            bool isValid = this.ValidateChildren();
            return isValid && dxValidationProvider1.Validate();
        }
        // Hàm lấy dữ liệu từ UserControl để tạo một ContinuousFuzzySetDTO
        public ContinuousFuzzySetDTO getContinuousFuzzySet()
        {
            string name = txtNameConsFuzzy.Text;
            float leftBottom = float.Parse(txtBotLeft.Text);
            float leftTop = float.Parse(txtTopLeft.Text);
            float rightTop = float.Parse(txtTopRight.Text);
            float rightBottom = float.Parse(txtBotRight.Text);
            if(this.selectedFuzzySet!=null)
                return new ContinuousFuzzySetDTO(leftBottom, leftTop, rightTop, rightBottom, this.selectedFuzzySet.oid,name);
            else
                return new ContinuousFuzzySetDTO(leftBottom, leftTop, rightTop, rightBottom, name);
        }
        // Hàm này sẽ được gọi khi người dùng chọn một FuzzySet
        public void LoadFuzzySet(FuzzySetDTO fuzzySet)
        {
            this.selectedFuzzySet = fuzzySet;
            txtNameConsFuzzy.Text = fuzzySet.fuzzySetName;
            // Bind dữ liệu của FuzzySet vào các TextBox tương ứng
            if (fuzzySet is ContinuousFuzzySetDTO continuousFuzzySet)
            {
                txtBotLeft.Text = continuousFuzzySet.leftBottom.ToString();
                txtTopLeft.Text = continuousFuzzySet.leftTop.ToString();
                txtTopRight.Text = continuousFuzzySet.rightTop.ToString();
                txtBotRight.Text = continuousFuzzySet.rightBottom.ToString();
            }
        }
    }
}
