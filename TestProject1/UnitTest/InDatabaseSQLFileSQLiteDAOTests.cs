using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Common;
using BLL.DAO;
using System.Diagnostics;

namespace TestProject1.UnitTest
{
    public class InDatabaseSQLFileSQLiteDAOTests
    {

        //not done: database mocking
        private CompositionRoot compRoot;
        private InDataBaseSQLFileSQLiteDAO dao;
        private DatabaseManager dbMgr;
        private string dbFile = "C:\\Users\\Phung\\Downloads\\DatabaseManagerTest\\test1.fprdb";
        public InDatabaseSQLFileSQLiteDAOTests()
        {
            this.compRoot = new CompositionRoot();
            this.dbMgr = this.compRoot.getDBMgr();
            this.dao = dao = new InDataBaseSQLFileSQLiteDAO(this.compRoot.getMetaDataManger(), this.compRoot.getDBMgr());
        }
        //[Fact]
        public void nonStandardCreateInDatabaseSQLFileTestMethod()
        {
            dbMgr.createDB(this.dbFile);
            this.dao.createFile("sql1", "SELECT * FROM patient1");

            //File.Delete(this.dbFile);
        }
        //[Fact]
        public void nonStandardLoadInDatabaseSQLFileTestMethod()
        {
            dbMgr.loadDB(this.dbFile);
            string actual=this.dao.getFileContent("sql1");

            //File.Delete(this.dbFile);
        }
        //[Fact]
        public void nonStandardSaveInDatabaseSQLFileTestMethod()
        {
            dbMgr.loadDB(this.dbFile);
           this.dao.save("sql1", "hadsfhaha");

            //File.Delete(this.dbFile);
        }
        //[Fact]
        public void nonStandardDeleteInDatabaseSQLFileTestMethod()
        {
            dbMgr.loadDB(this.dbFile);
            this.dao.deleteFile("sql1");

            //File.Delete(this.dbFile);
        }
    }
}
