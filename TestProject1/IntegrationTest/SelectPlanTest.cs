using BLL;
using BLL.Common;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.IntegrationTest
{
    public class SelectPlanTest
    {
        private string dbFile;
        public SelectPlanTest()
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
            processor.executeDataDefinition(@"
                CREATE SCHEMA PATIENT (
                    P_ID VARCHAR(100),
                    P_NAME VARCHAR(100),
                    P_AGE DIST_FUZZYSET_INT,
                    P_DISEASE VARCHAR(100),
                    P_COST CONT_FUZZYSET
                    CONSTRAINT pk_patient PRIMARY KEY (P_ID)
                )
            ");
            processor.executeDataDefinition(@"
                CREATE RELATION PATIENT ON PATIENT
            ");

            processor.executeUpdate(@"
                INSERT INTO DOCTOR1 (P_ID, P_NAME, P_AGE, P_DISEASE, P_COST)
                VALUES ( {('PT111',[1,1])}, {('N. V. Ha',[1,1])}, {(65, [1, 1])}, {('lung cancer', [0.5, 0.5]), ('tuberculosis', [0.5, 0.5])}, {(300, [0.5, 0.5]), (350, [0.5, 0.5])} )
            ");
            processor.executeUpdate(@"
                INSERT INTO DOCTOR1 (P_ID, P_NAME, P_AGE, P_DISEASE, P_COST)
                VALUES ( {('PT112',[1,1])}, {('T. V. Son',[1,1]}, {(young, [1, 1])}, {('hepatitis', [0.45, 0.65]), ('cirrhosis', [0.45, 0.65]}, {(about_60, [0.4, 0.6]), (about_70, [0.4, 0.6])} )
            ");
            processor.executeUpdate(@"
                INSERT INTO DOCTOR1 (P_ID, P_NAME, P_AGE, P_DISEASE, P_COST)
                VALUES ( {('PT113',[1,1])}, {('L. T. Lan',[1,1])}, {(middle_age, [1, 1])}, {(cholecystitis, [1, 1])}, {(8, [1, 1])} )
            ");

        }
        [Fact]
        public void SelectPlan_open_sucess()
        {
            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            dbMgr.loadDB(this.dbFile);
            RelationPlan p1 = new RelationPlan("DOCTOR1", metaMgr, dbMgr);

            //SelectPlan p2=new SelectPlan(p1, )
            //act

        }
    }
}
