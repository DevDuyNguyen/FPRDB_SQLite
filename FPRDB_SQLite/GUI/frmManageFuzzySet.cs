using DevExpress.XtraEditors;
using DevExpress.XtraScheduler.Outlook.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DevExpress.XtraRichEdit.API.Native;

namespace FPRDB_SQLite.GUI
{
    public partial class frmManageFuzzySet : DevExpress.XtraEditors.XtraForm
    {
        public class FuzzySetDTO
        {
            public int oid { get; set; }
            public string fuzzySetName { get; set; }
            public FieldType fuzzySetType { get; set; }
            public override string ToString()
            {
                return fuzzySetName;
            }
            public bool IsDiscrete()
            {
                return fuzzySetType == FieldType.distFS_INT
                    || fuzzySetType == FieldType.distFS_FLOAT
                    || fuzzySetType == FieldType.distFS_TEXT;
            }
            public virtual bool IsValid()
            {
                return !string.IsNullOrEmpty(fuzzySetName) && fuzzySetType != default(FieldType);
            }
        }
        public class DiscreteFuzzySetDTO<TDomain> : FuzzySetDTO
        {
            public TDomain[] valueSet { get; set; }
            public float[] membershipDegreeSet { get; set; }

            public override bool IsValid() => valueSet != null && membershipDegreeSet != null;
        }

        public class ContinuousFuzzySetDTO : FuzzySetDTO
        {
            public float leftBottom { get; set; }
            public float leftTop { get; set; }
            public float rightTop { get; set; }
            public float rightBottom { get; set; }

            public override bool IsValid() => leftBottom < leftTop && rightBottom > rightTop;
        }

        List<FuzzySetDTO> fuzzySets = new List<FuzzySetDTO>
        {
            new DiscreteFuzzySetDTO<int>
            {
                oid = 1,
                fuzzySetName = "discreINT",
                fuzzySetType = FieldType.distFS_INT,
                valueSet = new int[] { 1, 2, 3 },
                membershipDegreeSet = new float[] { 0.2f, 0.5f, 0.8f }
            },
            new DiscreteFuzzySetDTO<float>
            {
                oid = 2,
                fuzzySetName = "discreFLOAT",
                fuzzySetType = FieldType.distFS_FLOAT,
                valueSet = new float[] { 1.1f, 2.2f, 3.3f },
                membershipDegreeSet = new float[] { 0.3f, 0.6f, 0.9f }
            },
            new DiscreteFuzzySetDTO<string>
            {
                oid = 3,
                fuzzySetName = "discreTEXT",
                fuzzySetType = FieldType.distFS_TEXT,
                valueSet = new string[] { "A", "B", "C" },
                membershipDegreeSet = new float[] { 0.4f, 0.7f, 1.0f }
            },
            new ContinuousFuzzySetDTO
            {
                oid = 4,
                fuzzySetName = "continuous",
                fuzzySetType = FieldType.contFS,
                leftBottom = 0.0f,
                leftTop = 0.3f,
                rightTop = 0.7f,
                rightBottom = 1.0f
            }
        };
        public frmManageFuzzySet()
        {
            InitializeComponent();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtFuzzySetName.Text.ToLower();
            // Xóa danh sách trước đó
            lstFuzzySetResults.Items.Clear();
            // Lọc dữ liệu theo keyword
            var results = fuzzySets.Where(x => x.fuzzySetName.ToLower().Contains(keyword)).ToList();
            if (results.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả nào phù hợp.");
                return;
            }
            // Hiển thị kết quả lọc
            lstFuzzySetResults.Items.AddRange(results.ToArray());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFuzzySetName.Clear();
            lstFuzzySetResults.Items.Clear();
            pnlFuzzySetMeaning.Visible = false;
        }

        private void lstFuzzySetResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFuzzySetResults.SelectedItem != null)
            {
                var selectedItem = (FuzzySetDTO)lstFuzzySetResults.SelectedItem;
                MessageBox.Show("Bạn đã chọn: " + selectedItem.ToString());
                pnlFuzzySetMeaning.Visible = true;
                if (selectedItem.IsDiscrete())
                {
                    discreteFuzzySetInfo.LoadFuzzySet(selectedItem);
                    discreteFuzzySetInfo.Visible = true;
                    continuosFuzzySetInfo.Visible = false;
                }
                else
                {
                    discreteFuzzySetInfo.Visible = false;
                    continuosFuzzySetInfo.Visible = true;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }
    }
}