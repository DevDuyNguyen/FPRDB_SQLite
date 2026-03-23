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
    public class IntersectionPlanTests
    {
        private string dbFile;
        public IntersectionPlanTests()
        {
            this.dbFile = "C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestSqlite\\db1.db";
        }
        //[Fact]
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
            //    VALUES ( {('PT234',[1,1])}, {('DT102',[1,1])}, {(approx_15,[1,1])}, {('hepatitis',[0.5, 0.5]),('cirrhosis',[0.5, 0.5])} )
            //");

            //processor.executeUpdate(@"
            //    INSERT INTO DIAGNOSE2 (P_ID, D_ID, P_AGE, DISEASE)
            //    VALUES ( {('PT383',[1,1])}, {('DT102',[1,1])}, {(69,[0.5,0.5]),(70,[0.5,0.5])}, {('lung cancer',[1, 1])} )
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
        //[Fact]
        public void IntersectionPlan_getSchema_success()
        {
            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            dbMgr.loadDB(this.dbFile);
            Plan p1 = new RelationPlan("DIAGNOSE1", metaMgr, dbMgr, compRoot.getParser(), compRoot.getConstraintService());
            Plan p2 = new RelationPlan("DIAGNOSE2", metaMgr, dbMgr, compRoot.getParser(), compRoot.getConstraintService());

            Plan p3 = new IntersectionPlan(p1, p2, ProbabilisticCombinationStrategy.CONJUNCTION_INDEPENDANCE);
            Scan res = p3.open();
            while (res.next())
            {
                FuzzyProbabilisticValue<string> p_id = res.getFieldContent<string>("P_ID");
                FuzzyProbabilisticValue<string> d_id = res.getFieldContent<string>("D_ID");
                FuzzyProbabilisticValue<float> p_age = res.getFieldContent<float>("P_AGE");
                FuzzyProbabilisticValue<string> disease = res.getFieldContent<string>("DISEASE");
            }
        }

    }
}
