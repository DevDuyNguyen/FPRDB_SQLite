using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.SQLProcessing;

namespace TestProject1.IntegrationTest
{
    public class MetadataTest
    {
        private string dbFile;
        public MetadataTest()
        {
            this.dbFile = "C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestSqlite\\db1.db";
        }
        //[Fact]
        public void MetaDataManager_getFuzzySet_success()
        {
            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            compRoot.getDBMgr().loadDB(this.dbFile);
            //act
            FuzzySet<int> actual1 = metaMgr.getFuzzySet<int>("distFS1", FieldType.distFS_INT);
            FuzzySet<string> actual2 = metaMgr.getFuzzySet<string>("distFS2", FieldType.distFS_TEXT);
            FuzzySet<float> actual3 = metaMgr.getFuzzySet<float>("contFS1", FieldType.contFS);
        }
    }
}
