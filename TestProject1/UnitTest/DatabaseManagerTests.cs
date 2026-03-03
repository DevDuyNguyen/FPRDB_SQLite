using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPRDB_SQLite;
using Xunit;

namespace TestProject1.UnitTest
{
    public class DatabaseManagerTests
    {
        [Fact]
        public void DatabaseManager_createDB_DirectoryNotExist()
        {
            //arrange
            string filePath = "C://not_exist_directory/dbTest.db";
            DatabaseManager dbMgr = new DatabaseManager();
            //action
            //assert
            Assert.Throws<FileNotFoundException>(() => dbMgr.createDB(filetPath));

        }
    }
}
