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

        [Fact]
        public void deleteSuccess()
        {
            //arrange
            //string sql = "DELETE FROM DOCTOR1 WHERE (DOCTOR_ID='DT103')[1,1]";
            string sql = "DELETE FROM DOCTOR1 WHERE (D_AGE⇒middle_aged)[1,1]";
            //act
            int affectedNoRows = this.sqlProcessor.executeUpdate(sql);
            Debug.WriteLine("Delete result:" + affectedNoRows);

        }

    }
}
