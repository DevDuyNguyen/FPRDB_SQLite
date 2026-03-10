using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.SQLProcessing;
using BLL.Interfaces;
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
        //[Fact]
        public void RelationScan_turnConstantToFuzzySet_success()
        {
            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            dbMgr.loadDB(this.dbFile);
            RelationScan scan = new RelationScan("student23", dbMgr, metaMgr);
            //act
            FuzzySet<int> actual1 = scan.turnConstantToFuzzySet<int>(new IntConstant(12));
            FuzzySet<float> actual2 = scan.turnConstantToFuzzySet<float>(new FloatConstant(12.1f));
            FuzzySet<string> actual3 = scan.turnConstantToFuzzySet<string>(new StringConstant("haha"));
            FuzzySet<bool> actual4 = scan.turnConstantToFuzzySet<bool>(new BooleanConstant(false));
            FuzzySet<int> actual5 = scan.turnConstantToFuzzySet<int>(new FuzzySetConstant("distFS1"));
            FuzzySet<string> actual6 = scan.turnConstantToFuzzySet<string>(new FuzzySetConstant("distFS2"));
            FuzzySet<float> actual7 = scan.turnConstantToFuzzySet<float>(new FuzzySetConstant("contFS1"));
        }
        //[Fact]
        public void RelationScan_turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue_success()
        {
            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            dbMgr.loadDB(this.dbFile);
            RelationScan scan = new RelationScan("student23", dbMgr, metaMgr);
            //act
            FuzzyProbabilisticValue<int> actual1 = scan.turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<int>(
                new FuzzyProbabilisticValueParsingData(
                    new List<Constant> { new IntConstant(1), new IntConstant(2) },
                    new List<float> { 1, 0.5f },
                    new List<float> { 1, 0.5f }
                    ),
                FieldType.INT
                );

            FuzzyProbabilisticValue<int> actual2 = scan.turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<int>(
                new FuzzyProbabilisticValueParsingData(
                    new List<Constant> { new IntConstant(1), new FuzzySetConstant("distFS1") },
                    new List<float> { 1, 0.5f },
                    new List<float> { 1, 0.5f }
                    ),
                FieldType.distFS_INT
                );
            FuzzyProbabilisticValue<float> actual3 = scan.turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<float>(
                new FuzzyProbabilisticValueParsingData(
                    new List<Constant> { new FloatConstant(1.1f), new FuzzySetConstant("contFS1") },
                    new List<float> { 1, 0.5f },
                    new List<float> { 1, 0.5f }
                    ),
                FieldType.contFS
                );
        }
        //[Fact]
        public void RelationScan_next_success()
        {
            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            dbMgr.loadDB(this.dbFile);
            RelationScan scan = new RelationScan("student23", dbMgr, metaMgr);
            //act
            while (scan.next())
            {

            }
        }
        //[Fact]
        public void RelationScan_getFieldContent_success()
        {
            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            dbMgr.loadDB(this.dbFile);
            RelationScan scan = new RelationScan("student23" dbMgr, metaMgr);
            //act
            while (scan.next())
            {
                FuzzyProbabilisticValue<int> student_id = scan.getFieldContent<int>("student_id");
                FuzzyProbabilisticValue<string> name = scan.getFieldContent<string>("name");
                FuzzyProbabilisticValue<int> age = scan.getFieldContent<int>("age");
            }
        }

    }
}
