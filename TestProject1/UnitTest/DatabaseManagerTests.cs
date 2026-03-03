using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using BLL;

namespace TestProject1.UnitTest
{
    public class DatabaseManagerTests: IDisposable
    {
        private DatabaseManager dbMgr;
        private string directory;
        private string filePath;
        public DatabaseManagerTests()
        {
            this.dbMgr= new DatabaseManager();
            this.directory = "C:\\Users\\Phung\\Downloads\\DatabaseManagerTest";
            this.filePath = directory + "\\dbTest.db";
        }

        [Fact]
        public void DatabaseManager_createDB_DirectoryNotExist()
        {
            //arrange
            //action
            //assert
            Assert.Throws<FileNotFoundException>(() => this.dbMgr.createDB(this.filePath));

        }
        [Fact]
        public void DatabaseManager_createDB_FileAlreadyExist()
        {
            //arrange
            if(!Directory.Exists(this.directory))
                Directory.CreateDirectory(directory);
            if (!File.Exists(this.filePath))
            {
                FileStream fs = File.Create(this.filePath);
                fs.Close();
            }
            
            //action
            Exception e= Assert.Throws<IOException>(()=>this.dbMgr.createDB(this.filePath));
            //assert
            Assert.Equal("File already exists", e.Message);
        }
        [Fact]
        public void DatabaseManager_createDB_Success()
        {
            //arrange
            string directory = "C:\\Users\\Phung\\Downloads\\DatabaseManagerTest";
            string filePath = directory + "\\dbTest.db";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            //act
            this.dbMgr.createDB(filePath);
            //assert
            Assert.Equal(true, File.Exists(filePath));
        }

        //not done:check if database has correct system catalog
        //not done: check if file is FPRDB database

        public void Dispose()
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
        }
    }
}
