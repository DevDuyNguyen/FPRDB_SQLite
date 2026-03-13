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
            //act
            FuzzySet<int> actual1 = FuzzySetUltilities.turnConstantToFuzzySet<int>(new IntConstant(12), compRoot.getMetaDataManger());
            FuzzySet<float> actual2 = FuzzySetUltilities.turnConstantToFuzzySet<float>(new FloatConstant(12.1f), compRoot.getMetaDataManger());
            FuzzySet<string> actual3 = FuzzySetUltilities.turnConstantToFuzzySet<string>(new StringConstant("haha"), compRoot.getMetaDataManger());
            FuzzySet<bool> actual4 = FuzzySetUltilities.turnConstantToFuzzySet<bool>(new BooleanConstant(false), compRoot.getMetaDataManger());
            FuzzySet<int> actual5 = FuzzySetUltilities.turnConstantToFuzzySet<int>(new FuzzySetConstant("distFS1"), compRoot.getMetaDataManger());
            FuzzySet<string> actual6 = FuzzySetUltilities.turnConstantToFuzzySet<string>(new FuzzySetConstant("distFS2"), compRoot.getMetaDataManger());
            FuzzySet<float> actual7 = FuzzySetUltilities.turnConstantToFuzzySet<float>(new FuzzySetConstant("contFS1"), compRoot.getMetaDataManger());
        }
        //[Fact]
        public void RelationScan_turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue_success()
        {
            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            MetadataManager metaMgr = compRoot.getMetaDataManger();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            dbMgr.loadDB(this.dbFile);
            FPRDBRelation relInfo = metaMgr.getRelation("student23");
            RelationScan scan = new RelationScan(relInfo, dbMgr, metaMgr, compRoot.getParser());
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
            FPRDBRelation relInfo = metaMgr.getRelation("student23");
            RelationScan scan = new RelationScan(relInfo, dbMgr, metaMgr, compRoot.getParser());
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
            FPRDBRelation relInfo = metaMgr.getRelation("student23");
            RelationScan scan = new RelationScan(relInfo, dbMgr, metaMgr, compRoot.getParser());
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
