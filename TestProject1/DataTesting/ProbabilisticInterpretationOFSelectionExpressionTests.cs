using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Interfaces;
using BLL.SQLProcessing;

namespace TestProject1.DataTesting
{
    public class ProbabilisticInterpretationOFSelectionExpressionTests
    {
        //not done: can't only run in local machine, not on github ci/cd
        //not done: not suitable when the FPRDB is implemented from scratch with its own file system


        private string testDataPath = "C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestData\\TestData02\\TestData02.db";
        private CompositionRoot compRoot;
        public ProbabilisticInterpretationOFSelectionExpressionTests()
        {
            this.compRoot = new CompositionRoot();
            this.compRoot.getDBMgr().loadDB(this.testDataPath);
        }

        [Fact]
        public void attributeCompareToConstant()
        {
            //arrange
            FPRDBSchema schema = new FPRDBSchema(
                "sche1",
                new List<Field> {
                    new Field("id", new FieldInfo(FieldType.INT, 0)),
                    new Field("att1", new FieldInfo(FieldType.distFS_INT, 0)),
                    new Field("att2", new FieldInfo(FieldType.distFS_FLOAT, 0)),
                    new Field("att3", new FieldInfo(FieldType.contFS, 0))
                },
                new List<string> { "id" }
                );
            Plan p = new RelationPlan("rel1", this.compRoot.getMetaDataManger(), this.compRoot.getDBMgr(), this.compRoot.getParser());
            Scan s = p.open();
            AtomicSelectionExpressionFieldConstant selectionExpression = new AtomicSelectionExpressionFieldConstant("att3", new FuzzySetConstant("approx_15"), CompareOperation.ALSO, this.compRoot.getMetaDataManger());
            //act
            //assert

            //t1
            s.next();
            List<float> expected = new List<float> { 0.38f, 0.57f };
            List<float> actual = selectionExpression.calculateProbabilisticInterpretation(s, schema);
            Assert.Equal(expected[0], actual[0], 0.02);

            //t2
            s.next();
            expected = new List<float> { 0.08f, 0.12f };
            actual = selectionExpression.calculateProbabilisticInterpretation(s, schema);
            Assert.Equal(expected[0], actual[0], 0.02);
        }

        [Fact]
        public void attributeCompareToAttribute()
        {
            //arrange
            FPRDBSchema schema = new FPRDBSchema(
                "sche1",
                new List<Field> { 
                    new Field("id", new FieldInfo(FieldType.INT, 0)),
                    new Field("att1", new FieldInfo(FieldType.distFS_INT, 0)),
                    new Field("att2", new FieldInfo(FieldType.distFS_FLOAT, 0)),
                    new Field("att3", new FieldInfo(FieldType.contFS, 0))
                },
                new List<string> { "id"}
                );
            Plan p = new RelationPlan("rel1", this.compRoot.getMetaDataManger(), this.compRoot.getDBMgr(), this.compRoot.getParser());
            Scan s = p.open();
            AtomicSelectionExpressionFieldField selectionExpression = new AtomicSelectionExpressionFieldField("att2","att1", ProbabilisticCombinationStrategy.CONJUNCTION_INDEPENDANCE);
            //act
            //assert

            //t1
            s.next();
            List<float> expected = new List<float> { 0.02667f, 0.06f };
            List<float> actual = selectionExpression.calculateProbabilisticInterpretation(s, schema);
            Assert.Equal(expected[0], actual[0], 0.02);

            //t2
            s.next();
            expected = new List<float> { 0.13334f, 0.3f };
            actual = selectionExpression.calculateProbabilisticInterpretation(s, schema);
            Assert.Equal(expected[0], actual[0], 0.02);
        }

        [Fact]
        public void selectionExpression_Conjunction_SelectionExpression()
        {
            //arrange
            FPRDBSchema schema = new FPRDBSchema(
                "sche1",
                new List<Field> {
                    new Field("id", new FieldInfo(FieldType.INT, 0)),
                    new Field("att1", new FieldInfo(FieldType.distFS_INT, 0)),
                    new Field("att2", new FieldInfo(FieldType.distFS_FLOAT, 0)),
                    new Field("att3", new FieldInfo(FieldType.contFS, 0))
                },
                new List<string> { "id" }
                );
            Plan p = new RelationPlan("rel1", this.compRoot.getMetaDataManger(), this.compRoot.getDBMgr(), this.compRoot.getParser());
            Scan s = p.open();
            AtomicSelectionExpressionFieldConstant lSelectionExpression = new AtomicSelectionExpressionFieldConstant("att3", new FuzzySetConstant("approx_15"), CompareOperation.ALSO, this.compRoot.getMetaDataManger());
            AtomicSelectionExpressionFieldField rSelectionExpression = new AtomicSelectionExpressionFieldField("att2", "att1", ProbabilisticCombinationStrategy.CONJUNCTION_INDEPENDANCE);
            CompoundSelectionExpression selectionExpression = new CompoundSelectionExpression(lSelectionExpression, rSelectionExpression, ProbabilisticCombinationStrategy.CONJUNCTION_INDEPENDANCE);
            //act
            //assert

            //t1
            s.next();
            List<float> expected = new List<float> { 0.01f, 0.0342f };
            List<float> actual = selectionExpression.calculateProbabilisticInterpretation(s, schema);
            Assert.Equal(expected[0], actual[0], 0.02);

            //t2
            s.next();
            expected = new List<float> { 0.0106f, 0.036f };
            actual = selectionExpression.calculateProbabilisticInterpretation(s, schema);
            Assert.Equal(expected[0], actual[0], 0.02);
        }

        [Fact]
        public void selectionExpression_Disjunction_SelectionExpression()
        {
            //arrange
            FPRDBSchema schema = new FPRDBSchema(
                "sche1",
                new List<Field> {
                    new Field("id", new FieldInfo(FieldType.INT, 0)),
                    new Field("att1", new FieldInfo(FieldType.distFS_INT, 0)),
                    new Field("att2", new FieldInfo(FieldType.distFS_FLOAT, 0)),
                    new Field("att3", new FieldInfo(FieldType.contFS, 0))
                },
                new List<string> { "id" }
                );
            Plan p = new RelationPlan("rel1", this.compRoot.getMetaDataManger(), this.compRoot.getDBMgr(), this.compRoot.getParser());
            Scan s = p.open();
            AtomicSelectionExpressionFieldConstant lSelectionExpression = new AtomicSelectionExpressionFieldConstant("att3", new FuzzySetConstant("approx_15"), CompareOperation.ALSO, this.compRoot.getMetaDataManger());
            AtomicSelectionExpressionFieldField rSelectionExpression = new AtomicSelectionExpressionFieldField("att2", "att1", ProbabilisticCombinationStrategy.CONJUNCTION_INDEPENDANCE);
            CompoundSelectionExpression selectionExpression = new CompoundSelectionExpression(lSelectionExpression, rSelectionExpression, ProbabilisticCombinationStrategy.DISJUNCTION_INDEPENDANCE);
            //act
            //assert

            //t1
            s.next();
            List<float> expected = new List<float> { 0.3965f, 0.5958f };
            List<float> actual = selectionExpression.calculateProbabilisticInterpretation(s, schema);
            Assert.Equal(expected[0], actual[0], 0.02);

            //t2
            s.next();
            expected = new List<float> { 0.20267f, 0.384f };
            actual = selectionExpression.calculateProbabilisticInterpretation(s, schema);
            Assert.Equal(expected[0], actual[0], 0.02);
        }




    }
}
