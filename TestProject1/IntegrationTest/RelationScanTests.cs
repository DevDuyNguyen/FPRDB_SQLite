using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.IntegrationTest
{
    public class RelationScanTests
    {
        private string dbFile;
        public RelationScanTests()
        {
            this.dbFile = "C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestSqlite\\db1.db";
        }
        [Fact]
        public void RelationScan_turnConstantToFuzzySet_success()
        {
            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            DatabaseManager dbMgr= compRoot.getDBMgr();
            dbMgr.loadDB(this.dbFile);
            RelationScan scan = new RelationScan("student23", compRoot.getParser(), dbMgr, metaMgr);
            //act
            FuzzySet<int> actual1 = scan.turnConstantToFuzzySet<int>(new IntConstant(12), FieldType.distFS_INT);
            FuzzySet<float> actual2 = scan.turnConstantToFuzzySet<float>(new FloatConstant(12.1f), FieldType.distFS_FLOAT);
            FuzzySet<string> actual3 = scan.turnConstantToFuzzySet<string>(new StringConstant("haha"), FieldType.distFS_TEXT);
            FuzzySet<bool> actual4 = scan.turnConstantToFuzzySet<bool>(new BooleanConstant(false), FieldType.BOOLEAN);
            FuzzySet<int> actual5 = scan.turnConstantToFuzzySet<int>(new FuzzySetConstant("distFS1"), FieldType.distFS_INT);
            FuzzySet<string> actual6 = scan.turnConstantToFuzzySet<string>(new FuzzySetConstant("distFS2"), FieldType.distFS_TEXT);
            FuzzySet<float> actual7 = scan.turnConstantToFuzzySet<float>(new FuzzySetConstant("contFS1"), FieldType.contFS);
        }

    }
}
