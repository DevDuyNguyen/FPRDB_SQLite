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
    public class NaturalJoinPlanTests
    {
        private string dbFile;
        public NaturalJoinPlanTests()
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
            //    CREATE SCHEMA DOCTOR1 (
            //        DOCTOR_ID VARCHAR(100),
            //        D_AGE CONT_FUZZYSET,
            //        CONSTRAINT pk_doctor1 PRIMARY KEY (DOCTOR_ID)
            //    )
            //");
            //processor.executeDataDefinition(@"
            //    CREATE SCHEMA DOCTOR2 (
            //        DOCTOR_NAME VARCHAR(100),
            //        D_AGE CONT_FUZZYSET,
            //        CONSTRAINT pk_doctor2 PRIMARY KEY (DOCTOR_NAME)
            //    )
            //");
            //processor.executeDataDefinition(@"
            //    CREATE RELATION DOCTOR1 ON DOCTOR1
            //");
            //processor.executeDataDefinition(@"
            //    CREATE RELATION DOCTOR2 ON DOCTOR2
            //");

            //processor.executeUpdate(@"
            //    INSERT INTO DOCTOR1 (DOCTOR_ID, D_AGE)
            //    VALUES ( {('DT005',[1,1])}, {(middle_aged,[1,1])} )
            //");
            //processor.executeUpdate(@"
            //    INSERT INTO DOCTOR1 (DOCTOR_ID, D_AGE)
            //    VALUES ( {('DT093',[1,1])}, {(approx_30,[1,1])} )
            //");
            //processor.executeUpdate(@"
            //    INSERT INTO DOCTOR1 (DOCTOR_ID, D_AGE)
            //    VALUES ( {('DT102',[1,1])}, {(55,[0.5,0.5]), (56,[0.5,0.5])} )
            //");

            //processor.executeUpdate(@"
            //    INSERT INTO DOCTOR2 (DOCTOR_NAME, D_AGE)
            //    VALUES ( {('L.V Cuong',[1,1])}, {(30,[0.4,0.6]),(31,[0.4,0.6])} )
            //");
            //processor.executeUpdate(@"
            //    INSERT INTO DOCTOR2 (DOCTOR_NAME, D_AGE)
            //    VALUES ( {('N.V. Hung',[1,1])}, {(middle_aged,[1,1])} )
            //");
            //processor.executeUpdate(@"
            //    INSERT INTO DOCTOR2 (DOCTOR_NAME, D_AGE)
            //    VALUES ( {('N.T.Dat',[1,1])}, {(54,[0.5,0.5]),(55,[0.5,0.5])} )
            //");

        }
        //[Fact]
        public void NaturalJoinPlan_getSchema_success()
        {
            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            dbMgr.loadDB(this.dbFile);
            RelationPlan p1 = new RelationPlan("DOCTOR1", metaMgr, dbMgr, compRoot.getParser());
            RelationPlan p2 = new RelationPlan("DOCTOR2", metaMgr, dbMgr, compRoot.getParser());

            Plan p3 = new NaturalJoinPlan(p1, p2, ProbabilisticCombinationStrategy.CONJUNCTION_INDEPENDANCE);
            Scan res = p3.open();
            while (res.next())
            {
                FuzzyProbabilisticValue<string> id = res.getFieldContent<string>("DOCTOR_ID");
                FuzzyProbabilisticValue<float> age = res.getFieldContent<float>("D_AGE");
                FuzzyProbabilisticValue<string> name = res.getFieldContent<string>("DOCTOR_NAME");
            }
        }
    }
}
