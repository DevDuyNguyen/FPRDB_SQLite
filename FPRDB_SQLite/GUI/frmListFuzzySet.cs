using BLL;
using BLL.Common;
using BLL.DTO;
using BLL.Services;
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
        private DatabaseService dbService;
        private List<FuzzySetDTO> fuzzySets = new List<FuzzySetDTO>();
        public FuzzySetDTO SelectedFuzzySet { get; private set; }
        private List<string> fsNames;
        public string selectedFSName;
        public frmListFuzzySet(CompositionRoot compRoot, FieldType typeFS)
        {
            this.compRoot = compRoot;
            this.dbService = this.compRoot.getDatabaseService();
            this.fsNames = this.dbService.getFuzzySetNameByType(typeFS);

            InitializeComponent();
            //mockingFuzzySets();
            //LoadFuzzySetsByType();
            LoadFuzzySetNames();
        }
        private void mockingFuzzySets()
        {
            FuzzySetDTO continuousFuzzySet = new ContinuousFuzzySetDTO(10, 20, 30, 40, "conFS");
            FuzzySetDTO discreteInt = new DiscreteFuzzySetDTO<int>(
                new List<int>() { 22, 23, 24 },
                new List<float>() { 0.5f, 1, 0.5f },
                "dis_INT",
                FieldType.INT);
            FuzzySetDTO discreteFloat = new DiscreteFuzzySetDTO<float>(
                new List<float>() { 21.2f, 23.5f, 24.8f },
                new List<float>() { 0.5f, 1, 0.5f },
                "dis_FLOAT",
                FieldType.FLOAT);
            FuzzySetDTO discreteText = new DiscreteFuzzySetDTO<string>(
                new List<string>() { "Low", "Medium", "High" },
                new List<float>() { 0.5f, 1, 0.5f },
                "dis_TEXT",
                FieldType.VARCHAR);
            fuzzySets.Add(continuousFuzzySet);
            fuzzySets.Add(discreteInt);
            fuzzySets.Add(discreteFloat);
            fuzzySets.Add(discreteText);
        }
        //private void LoadFuzzySetsByType(string typeFS)
        //{
        //    List<FuzzySetDTO> filtered = new List<FuzzySetDTO>();

        //    switch (typeFS)
        //    {
        //        case "distFS_INT":
        //            filtered = fuzzySets
        //                .OfType<DiscreteFuzzySetDTO<int>>()
        //                .Cast<FuzzySetDTO>()
        //                .ToList();
        //            break;

        //        case "distFS_FLOAT":
        //            filtered = fuzzySets
        //                .OfType<DiscreteFuzzySetDTO<float>>()
        //                .Cast<FuzzySetDTO>()
        //                .ToList();
        //            break;

        //        case "distFS_TEXT":
        //            filtered = fuzzySets
        //                .OfType<DiscreteFuzzySetDTO<string>>()
        //                .Cast<FuzzySetDTO>()
        //                .ToList();
        //            break;

        //        default:
        //            filtered = fuzzySets
        //                .OfType<ContinuousFuzzySetDTO>()
        //                .Cast<FuzzySetDTO>()
        //                .ToList();
        //            break;
        //    }
        //    LoadFuzzySetNames(filtered);
        //}
        private void LoadFuzzySetNames()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));

            foreach (var name in this.fsNames)
            {
                DataRow row = dt.NewRow();
                row["Name"] = name;
                dt.Rows.Add(row);
            }

            gridControlListFS.DataSource = dt;
            gridViewListFS.BestFitColumns();
        }

        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            var cellValue = gridViewListFS.GetFocusedRowCellValue("Name");
            if (cellValue == null) return;

            //SelectedFuzzySet = fuzzySets
            //    .FirstOrDefault(fs => fs.fuzzySetName == cellValue.ToString());
            this.selectedFSName = (string)cellValue;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}