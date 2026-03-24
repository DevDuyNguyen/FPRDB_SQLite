using BLL.Common;
using BLL.DTO;
using BLL.Services;
using DevExpress.XtraEditors;
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
        //private ConstraintDTO[] relationships;
        private class AttrRow
        {
            public string PKAttr { get; set; }
            public string FKAttr { get; set; }
        }
        public frmFKRelationships(CompositionRoot compRoot, FPRDBRelationDTO rel)
        {
            InitializeComponent();
            this.compRoot = compRoot;
            this.service = compRoot.getConstraintService();
            this.db = compRoot.getDatabaseService();
            this.rel = rel;
            grdcolFKAttr.FieldName = "FKAttr";
            grdcolPKAttr.FieldName = "PKAttr";
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
            lstFKSelected.Items.Add("testFK");
        }
        // Hàm load chi tiết của một relationship vào các control tương ứng
        //private void loadRelationshipDetail(ConstraintDTO relationship)
        //{
        //    txtFKName = relationship.conName;

        //    txtFKRelName = relationship.relation.relName;

        //    txtFKRelName.ReadOnly = true;

        //    List<FPRDBRelationDTO> pkRel = db.getFPRDBRelations();

        //    cboPKRelName.Properties.Items.AddRange(pkRel.Select(r => r.relName).ToArray());
        //    cboPKRelName.EditValue = relationship.referencedRelation.relName;

        //    List<string> pkAttrs = relationship.referencedAttributes;
        //    List<string> fkAttrs = relationship.attributes;
        //    var result = pkAttrs.Zip(fkAttrs, (pk, fk) => new AttrRow { PKAttr = pk, FKAttr = fk }).ToList();
        //    grdMappingAttr.DataSource = result;
        //}
        private void loadRelationshipDetailDemo()
        {
            //txtFKName = relationship.conName;

            //txtFKRelName = relationship.relation.relName;

            txtFKRelName.ReadOnly = true;

            List<FPRDBRelationDTO> pkRel = db.getFPRDBRelations();

            cboPKRelName.Properties.Items.AddRange(pkRel.Select(r => r.relName).ToArray());
            //cboPKRelName.EditValue = relationship.referencedRelation.relName;

            List<string> pkAttrs = new List<string>
            {
                "id", "name"
            };
            List<string> fkAttrs = new List<string> { "id", "name" };
            var result = pkAttrs.Zip(fkAttrs, (pk, fk) => new AttrRow { PKAttr = pk, FKAttr = fk }).ToList();
            repositoryItemLookUpEdit1.DataSource = pkAttrs; // pkAttrs nên là List của một Object hoặc string
            repositoryItemLookUpEdit1.ValueMember = ""; // Nếu là List<string> thì để trống hoặc dùng chính nó
            repositoryItemLookUpEdit1.DisplayMember = "";
            repositoryItemLookUpEdit2.DataSource = fkAttrs; // pkAttrs nên là List của một Object hoặc string
            repositoryItemLookUpEdit2.ValueMember = ""; // Nếu là List<string> thì để trống hoặc dùng chính nó
            repositoryItemLookUpEdit2.DisplayMember = "";
            grdMappingAttr.DataSource = result;
        }
        // Hàm xử lý khi người dùng chọn một relationship trong ListBox
        private void lstFKSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFKSelected.SelectedItems == null)
            {
                grpFKDetail.Visible = false;
                return;
            }
            grpFKDetail.Visible = true;
            //ConstraintDTO selectedRelationship = relationships.Select(r => r.conName == lstFKSelected.SelectedItems);
            loadRelationshipDetailDemo();

        }
        // Hàm xử lý khi click "Add" button
        private void btnAdd_Click(object sender, EventArgs e)
        {

        }
        // Hàm xử lý khi click "Delete" button
        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
        // Hàm xử lý khi click "Save" button
        private void btnSave_Click(object sender, EventArgs e)
        {

        }
        // Hàm xử lý khi click "Close" button
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}