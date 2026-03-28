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

        // Ham validate toàn bộ control, được gọi từ form cha
        public bool ValidateControls()
        {
            return dxValidationProvider1.Validate();
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
