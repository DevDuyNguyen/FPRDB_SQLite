using BLL.DTO;
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
using BLL;

namespace FPRDB_SQLite.GUI.UserControls
{
    public partial class ContinuosFuzzySet : DevExpress.XtraEditors.XtraUserControl
    {
        public ContinuosFuzzySet()
        {
            InitializeComponent();
        }
        // Hàm lấy dữ liệu từ UserControl để tạo một ContinuousFuzzySetDTO
        public ContinuousFuzzySetDTO GetContinuousFuzzySet()
        {
            string name = txtNameConsFuzzy.Text;
            float leftBottom = float.Parse(txtBotLeft.Text);
            float leftTop = float.Parse(txtTopLeft.Text);
            float rightTop = float.Parse(txtTopRight.Text);
            float rightBottom = float.Parse(txtBotRight.Text);
            return new ContinuousFuzzySetDTO(leftBottom, leftTop, rightTop, rightBottom, name);
        }
        // Phương thức này sẽ được gọi khi người dùng chọn một FuzzySet
        public void LoadFuzzySet(FuzzySetDTO fuzzySet)
        {
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
