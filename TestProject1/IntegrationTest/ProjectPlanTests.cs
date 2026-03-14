using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Interfaces;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.IntegrationTest
{
    public class ProjectPlanTests
    {
        private string dbFile;
        public ProjectPlanTests()
        {
            this.dbFile = "C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestSqlite\\db1.db";
        }
        [Fact]
        public void ProjectPlan_open_sucess()
        {

            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            dbMgr.loadDB(this.dbFile);
            Plan p = new RelationPlan("PATIENT", metaMgr, dbMgr, compRoot.getParser());

            SelectionExpression se1 = new AtomicSelectionExpressionFieldConstant("P_AGE", new FuzzySetConstant("approx_15"), CompareOperation.ALSO, metaMgr);
            SelectionExpression se2 = new AtomicSelectionExpressionFieldConstant("P_DISEASE", new StringConstant("hepatitis"), CompareOperation.EQUAL, metaMgr);
            SelectionCondition sc1 = new AtomicSelectionCondition(se1, 0.18f, 0.5f);
            SelectionCondition sc2 = new AtomicSelectionCondition(se2, 0.45f, 0.65f);
            SelectionCondition sc = new CompoundSelectionCondition(sc1, sc2, LogicalConnective.AND);
            p= new SelectPlan(p, sc);
            p = new ProjectPlan(p, new List<string> { "P_ID", "P_NAME", "P_AGE" }); 
            Scan scan = p.open();
            while (scan.next())
            {

            }
        }


    }
}
