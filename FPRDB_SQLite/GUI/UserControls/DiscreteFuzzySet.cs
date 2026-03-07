using BLL;
using BLL.DomainObject;
using BLL.DTO;
using DevExpress.CodeParser;
using DevExpress.XtraEditors;
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
        public DiscreteFuzzySet()
        {
            InitializeComponent();
            cboDataType.Properties.Items.AddRange(new object[] { FieldType.distFS_INT, FieldType.distFS_FLOAT, FieldType.distFS_TEXT });
        }
        // Hàm lấy thông tin loại DiscreteFuzzySet từ UserControl để truyền ra form UI
        public FieldType getFuzzySetType()
        {
            return (FieldType)cboDataType.SelectedItem;
        }
        // Hàm lấy dữ liệu từ UserControl để tạo một DiscreteFuzzySetDTO
        public DiscreteFuzzySetDTO<T> getDiscreteFuzzySet<T>()
        {
            var fuzzySetName = txtNameDiscFuzzy.Text;
            FieldType fuzzySetType = (FieldType)cboDataType.SelectedItem;
            var rows = grdvDiscFuzzy.DataSource as IEnumerable<dynamic>;

            var values = rows.Select(r => (T)r.value).ToList();
            var degrees = rows.Select(r => (float)r.membershipDegree).ToList();

            return new DiscreteFuzzySetDTO<T>(values, degrees, fuzzySetName, fuzzySetType);
        }
        // Phương thức này sẽ được gọi khi người dùng chọn một FuzzySet
        public void LoadFuzzySet(FuzzySetDTO fuzzySet)
        {
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
            var rows = Enumerable.Range(0, valueSet.Count)
                .Select(i => new
                {
                    value = valueSet[i],
                    membershipDegree = membershipDegreeSet[i]
                })
                .ToList();

            // Gán danh sách rows vào GridControl
            grdcDiscFuzzy.DataSource = rows;

            // Map cột với thuộc tính trong object ẩn danh
            grdcolValue.FieldName = "value";
            grdcolMembership.FieldName = "membershipDegree";
        }

    }
}
