using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.Interfaces;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.IntegrationTest
{
    public class IntersectionPlanTests
    {
        private string dbFile;
        public IntersectionPlanTests()
        {
            this.dbFile = "C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestSqlite\\db1.db";
        }
        [Fact]
        public void createTeacherRelation()
        {
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            dbMgr.loadDB(this.dbFile);
            SQLProcessor processor = compRoot.getSQLProcessor();
            //processor.executeDataDefinition(@"
            //    CREATE SCHEMA DIAGNOSE1 (
            //        P_ID VARCHAR(100),
            //        D_ID VARCHAR(100),
            //        P_AGE CONT_FUZZYSET,
            //        DISEASE VARCHAR(100)
            //        CONSTRAINT pk_diagnose1 PRIMARY KEY (P_ID,D_ID)
            //    )
            //");
            //processor.executeDataDefinition((@"
            //    CREATE SCHEMA DIAGNOSE2 (
            //        P_ID VARCHAR(100),
            //        D_ID VARCHAR(100),
            //        P_AGE CONT_FUZZYSET,
            //        DISEASE VARCHAR(100)
            //        CONSTRAINT pk_diagnose2 PRIMARY KEY (P_ID,D_ID)
            //    )
            //"));
            //processor.executeDataDefinition(@"
            //    CREATE RELATION DIAGNOSE1 ON DIAGNOSE1
            //");
            //processor.executeDataDefinition(@"
            //    CREATE RELATION DIAGNOSE2 ON DIAGNOSE2
            //");

            //processor.executeUpdate(@"
            //    INSERT INTO DIAGNOSE1 (P_ID, D_ID, P_AGE, DISEASE)
            //    VALUES ( {('PT226',[1,1])}, {('DT093',[1,1])}, {(65,[1,1])}, {('lung cancer',[0.4, 0.6]),('tuberculosis',[0.4, 0.6])} )
            //");
            //processor.executeUpdate(@"
            //    INSERT INTO DIAGNOSE1 (P_ID, D_ID, P_AGE, DISEASE)
            //    VALUES ( {('PT234',[1,1])}, {('DT102',[1,1])}, {(approx_15,[1,1])}, {('lhepatitis',[0.5, 0.5]),('cirrhosis',[0.5, 0.5])} )
            //");

            //processor.executeUpdate(@"
            //    INSERT INTO DIAGNOSE2 (P_ID, D_ID, P_AGE, DISEASE)
            //    VALUES ( {('PT383',[1,1])}, {('DT102',[1,1])}, {(69,[0.5,0.5]),(50,[0.5,0.5])}, {('lung cancer',[1, 1])} )
            //");
            //processor.executeUpdate(@"
            //    INSERT INTO DIAGNOSE2 (P_ID, D_ID, P_AGE, DISEASE)
            //    VALUES ( {('PT234',[1,1])}, {('DT102',[1,1])}, {(young,[1,1])}, {('hepatitis',[0.4, 0.6]),('gall-stone',[0.4, 0.6])} )
            //");
            //processor.executeUpdate(@"
            //    INSERT INTO DIAGNOSE2 (P_ID, D_ID, P_AGE, DISEASE)
            //    VALUES ( {('PT242',[1,1])}, {('DT025',[1,1])}, {(middle_aged,[1,1])}, {('cholecystitis',[1, 1])} )
            //");

        }
        [Fact]
        public void ProductPlan_getSchema_success()
        {
            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            dbMgr.loadDB(this.dbFile);
            RelationPlan p1 = new RelationPlan("DOCTOR1", metaMgr, dbMgr, compRoot.getParser());
            RelationPlan p2 = new RelationPlan("DOCTOR2", metaMgr, dbMgr, compRoot.getParser());


            Plan p3 = new ProductPlan(p1, p2, metaMgr, dbMgr);
            Scan res = p3.open();
            while (res.next())
            {
                FuzzyProbabilisticValue<string> id = res.getFieldContent<string>("DOCTOR_ID");
                FuzzyProbabilisticValue<float> age = res.getFieldContent<float>("D_AGE");
                FuzzyProbabilisticValue<string> name = res.getFieldContent<string>("DOCTOR_NAME");
                FuzzyProbabilisticValue<float> age1 = res.getFieldContent<float>("D_AGE");
            }
        }

    }
}
