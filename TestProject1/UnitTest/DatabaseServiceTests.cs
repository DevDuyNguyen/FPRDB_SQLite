using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using BLL;

namespace TestProject1.UnitTest
{
    public class DatabaseServiceTests
    {
        private DatabaseService databaseService;
        //not done: Moq for mocking
        private DatabaseManager dbMgr;
        public DatabaseServiceTests()
        {
            this.dbMgr = new DatabaseManager();
            this.databaseService = new DatabaseService(this.dbMgr);
        }
        //[InlineData("c:\\desktop\\f1.txt")]
        [Fact]
        public void DatabaseService_getDatabaseName_returnEmptyIfNotFile()
        {
            //arrange
            string fp1 = "c:\\hahaha";
            string fpt2 = "c:\\desktop\\f1";
            //act
            //asert
            this.dbMgr.setConnectionString(fp1);
            Assert.Equal("", this.databaseService.getDatabaseName());

            this.dbMgr.setConnectionString(fpt2);
            Assert.Equal("", this.databaseService.getDatabaseName());
            
        }
        [Fact]
        public void DatabaseService_getDatabaseName_success()
        {
            //arrange
            string fp1 = "c:\\desktop\\f1.txt";
            //act
            //asert
            this.dbMgr.setConnectionString(fp1);
            Assert.Equal("f1", this.databaseService.getDatabaseName());
        }
    }
}
