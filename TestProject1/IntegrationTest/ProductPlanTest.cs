using BLL;
using BLL.Common;
using BLL.SQLProcessing;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services;
using BLL.DomainObject;

namespace TestProject1.IntegrationTest
{
    public class ProductPlanTest
    {
        private string dbFile;
        public ProductPlanTest()
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
            //    CREATE SCHEMA DOCTOR1 (
            //        DOCTOR_ID VARCHAR(100),
            //        D_AGE DIST_FUZZYSET_INT,
            //        CONSTRAINT pk_doctor1 PRIMARY KEY (DOCTOR_ID)
            //    )
            //");
            //processor.executeDataDefinition(@"
            //    CREATE SCHEMA DOCTOR2 (
            //        DOCTOR_NAME VARCHAR(100),
            //        D_AGE DIST_FUZZYSET_INT,
            //        CONSTRAINT pk_doctor2 PRIMARY KEY (DOCTOR_NAME)
            //    )
            //");
            //processor.executeDataDefinition(@"
            //    CREATE RELATION DOCTOR1 ON DOCTOR1
            //");
            //processor.executeDataDefinition(@"
            //    CREATE RELATION DOCTOR2 ON DOCTOR2
            //");

            processor.executeUpdate(@"
                INSERT INTO DOCTOR1 (DOCTOR_ID, D_AGE)
                VALUES ( {('DT005',[1,1])}, {(distFS1,[1,1])} )
            ");
            processor.executeUpdate(@"
                INSERT INTO DOCTOR1 (DOCTOR_ID, D_AGE)
                VALUES ( {('DT093',[1,1])}, {(distFS1,[1,1])} )
            ");
            processor.executeUpdate(@"
                INSERT INTO DOCTOR1 (DOCTOR_ID, D_AGE)
                VALUES ( {('DT102',[1,1])}, {(distFS1,[1,1])} )
            ");

            processor.executeUpdate(@"
                INSERT INTO DOCTOR2 (DOCTOR_NAME, D_AGE)
                VALUES ( {('L.V Cuong',[1,1])}, {(30,[0.4,0.6]),(31,[0.4,0.6])} )
            ");
            processor.executeUpdate(@"
                INSERT INTO DOCTOR2 (DOCTOR_NAME, D_AGE)
                VALUES ( {('N.V. Hung',[1,1])}, {(distFS1,[1,1])} )
            ");
            processor.executeUpdate(@"
                INSERT INTO DOCTOR2 (DOCTOR_NAME, D_AGE)
                VALUES ( {('N.T.Dat',[1,1])}, {(54,[0.5,0.5]),(54,[0.5,0.5])} )
            ");

        }
        [Fact]
        public void ProductPlan_getSchema_success()
        {
            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            dbMgr.loadDB(this.dbFile);
            RelationPlan p1 = new RelationPlan("DOCTOR1", metaMgr, dbMgr);
            RelationPlan p2 = new RelationPlan("DOCTOR2", metaMgr, dbMgr);
            ProductPlan p3 = new ProductPlan(p1, p2, metaMgr, dbMgr);
            Scan res = p3.open();
            while (res.next())
            {
                FuzzyProbabilisticValue<string> id = res.getFieldContent<string>("DOCTOR_ID");
                FuzzyProbabilisticValue<int> age = res.getFieldContent<int>("D_AGE");
                FuzzyProbabilisticValue<string> name = res.getFieldContent<string>("DOCTOR_NAME");
                FuzzyProbabilisticValue<int> age1 = res.getFieldContent<int>("D_AGE");
            }
        }


    }
}
