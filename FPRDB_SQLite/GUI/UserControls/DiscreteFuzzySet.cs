using DevExpress.CodeParser;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        }
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
        private void BindDiscrete<TDomain>(TDomain[] values, float[] memberships)
        {
            // Tạo danh sách dữ liệu gồm cả Value và Membership
            var rows = values
                .Select((val, idx) => new
                {
                    Value = val,
                    Membership = memberships[idx]
                })
                .ToList();

            // Bind vào GridControl
            grdcDiscFuzzy.DataSource = rows;

            // Gán FieldName cho từng cột
            grdcolValue.FieldName = "Value";
            grdcolMembership.FieldName = "Membership";

            // Nếu muốn GridView tự tạo lại cột
            grdvDiscFuzzy.PopulateColumns();
        }

    }
}
