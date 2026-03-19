using BLL.Common;
using BLL.Interfaces;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.IntegrationTest
{
    public class UpdatingTests
    {
        private string dbFile;
        private CompositionRoot compRoot;
        private SQLProcessor sqlProcessor;
        public UpdatingTests()
        {
            this.dbFile = "C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestSqlite\\db1.db";
            this.compRoot = new CompositionRoot();
            compRoot.getDBMgr().loadDB(this.dbFile);
            this.sqlProcessor = this.compRoot.getSQLProcessor();
        }

        //[Fact]
        public void deleteTupleSuccess()
        {
            //arrange
            //string sql = "DELETE FROM DOCTOR1 WHERE (DOCTOR_ID='DT103')[1,1]";
            string sql = "DELETE FROM DOCTOR1 WHERE (D_AGE⇒middle_aged)[1,1]";
            //act
            int affectedNoRows = this.sqlProcessor.executeUpdate(sql);
            Debug.WriteLine("Delete result:" + affectedNoRows);

        }
        //[Fact]
        public void dropRelationSuccess()
        {
            //arrange
            string relName = "rel1";
            string schemaName = "rel";
            //act
            //this.sqlProcessor.executeDataDefinition($"CREATE SCHEMA {schemaName} (id int, age CONT_FUZZYSET, CONSTRAINT pk_rel primary key (id))");
            this.sqlProcessor.executeDataDefinition($"create relation {relName} on {schemaName}");
            this.sqlProcessor.executeUpdate($"insert into {relName} (id, age) values ({{(1, [1,1])}}, {{(young,[1,1])}})");
            this.sqlProcessor.executeUpdate($"insert into {relName} (id, age) values ({{(2, [1,1])}}, {{(40, [1,1])}})");
            this.sqlProcessor.executeUpdate($"insert into {relName} (id, age) values ({{(3, [1,1])}}, {{(middle_aged, [1,1])}})");
            this.sqlProcessor.executeUpdate($"drop relation {relName}");

        }
        //[Fact]
        public void dropSchemaSuccess()
        {
            //arrange
            string schemaName = "schema1";
            //act
            this.sqlProcessor.executeDataDefinition($"CREATE SCHEMA {schemaName} (id int, age CONT_FUZZYSET, CONSTRAINT pk_schema primary key (id))");
            this.sqlProcessor.executeUpdate($"drop schema {schemaName}");

        }


    }
}
