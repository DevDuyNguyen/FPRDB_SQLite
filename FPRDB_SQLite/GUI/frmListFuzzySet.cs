using BLL.Common;
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

namespace GUI.GUI
{
    public partial class frmListFuzzySet : DevExpress.XtraEditors.XtraForm
    {
        private CompositionRoot compRoot;
        private string typeFS;
        public frmListFuzzySet(CompositionRoot compRoot, string typeFS)
        {
            this.compRoot = compRoot;
            this.typeFS = typeFS;
            InitializeComponent();
        }
    }
}