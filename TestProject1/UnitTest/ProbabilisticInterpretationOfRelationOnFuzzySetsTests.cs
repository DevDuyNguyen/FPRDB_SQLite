using BLL;
using BLL.DomainObject;
using BLL.Enums;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.UnitTest
{
    public class ProbabilisticInterpretationOfRelationOnFuzzySetsTests
    {
        [Fact]
        public void discretize2ContinuousFS_sucess()
        {
            //arrange
            ContinuousFuzzySet fs1 = new ContinuousFuzzySet(1, 2, 3, 4, null);
            ContinuousFuzzySet fs2 = new ContinuousFuzzySet(0, 2, 3, 4, null);

            DiscreteFuzzySet<float> rfs1, rfs2;
            //act
            (rfs1, rfs2) = ProbabilisticInterpretationOfRelationOnFuzzySets.discretize2ContinuousFS(fs1, fs2);
            //assert
        }
        //not done: how should I test in such situation, create the test data by hands for use code to generate
        //class discretizeContinuousFSFromDiscreteFS_positive_testdata:TheoryData<ContinuousFuzzySet, DiscreteFuzzySet<float>, DiscreteFuzzySet<float>>
        //{
        //    public discretizeContinuousFSFromDiscreteFS_positive_testdata()
        //    {
        //        ContinuousFuzzySet fs1= new ContinuousFuzzySet(0, 30, 60, 100, null);
        //        List<float> values = new List<float>();
        //        List<float> memberships = new List<float>();
        //        float delta = (fs1.getRightBottom() - fs1.getLeftBottom()) / 100;
        //        for(float i=fs1.getLeftBottom(); i<=fs1.getRightBottom(); ++i)
        //        {
        //            values.Add(i);
        //            memberships.Add(fs1.getMembershipDegree(i));
        //        }

        //        Add(
        //            fs1,
        //            new DiscreteFuzzySet<float>(new List<float> { 1,2,3,4}, new List<float> { 0.1f, 0.2f, 0.3f, 0.4f}, null, FieldType.distFS_FLOAT),
        //            new DiscreteFuzzySet<float>(values, memberships, null, FieldType.distFS_FLOAT)
        //            );
        //    }

        //}
        [Fact]
        //[Theory]
        //[ClassData(typeof(discretizeContinuousFSFromDiscreteFS_positive_testdata))]
        public void discretizeContinuousFSFromDiscreteFS_success()//ContinuousFuzzySet fs1, DiscreteFuzzySet<float> fs2, DiscreteFuzzySet<float>)
        {
            //arrange
            ContinuousFuzzySet fs1 = new ContinuousFuzzySet(10, 30, 60, 100, null);
            DiscreteFuzzySet<float> fs2 = new DiscreteFuzzySet<float>(new List<float> { -1, 121, 30, 4 }, new List<float> { 0.1f, 0.2f, 0.3f, 0.4f }, null, FieldType.distFS_FLOAT);
            //act
            DiscreteFuzzySet<float> actual = ProbabilisticInterpretationOfRelationOnFuzzySets.discretizeContinuousFSFromDiscreteFS<float>(fs1, fs2);
            //assert
        }
        class equalDistcreteFuzzySets_positive_testdata:TheoryData<DiscreteFuzzySet<float>, DiscreteFuzzySet<float>, float>
        {
            public equalDistcreteFuzzySets_positive_testdata()
            {
                Add(
                new DiscreteFuzzySet<float>(new List<float> { 3, 4, 5, 6 }, new List<float> { 0.2f, 0.5f, 0.9f, 1f }, null, FieldType.distFS_FLOAT),
                new DiscreteFuzzySet<float>(new List<float> { 6, 5, 4}, new List<float> { 0.3f, 1f, 0.3f}, null, FieldType.distFS_FLOAT),
                0.34f
                );

            }
        }
        [Theory]
        [ClassData(typeof(equalDistcreteFuzzySets_positive_testdata))]
        public void equalDistcreteFuzzySets_success(DiscreteFuzzySet<float> fs1, DiscreteFuzzySet<float> fs2, float expected)
        {
            //arrange
            //act
            float actual = ProbabilisticInterpretationOfRelationOnFuzzySets.equalDistcreteFuzzySets<float>(fs1, fs2);
            //assert
            Assert.Equal(expected, actual, 5);
        }

        class noEqualDistcreteFuzzySets_positive_testdata : TheoryData<DiscreteFuzzySet<float>, DiscreteFuzzySet<float>, float>
        {
            public noEqualDistcreteFuzzySets_positive_testdata()
            {
                Add(
                new DiscreteFuzzySet<float>(new List<float> { 3, 4, 5, 6 }, new List<float> { 0.2f, 0.5f, 0.9f, 1f }, null, FieldType.distFS_FLOAT),
                new DiscreteFuzzySet<float>(new List<float> { 6, 5, 4 }, new List<float> { 0.3f, 1f, 0.3f }, null, FieldType.distFS_FLOAT),
                0.66f
                );

            }
        }
        [Theory]
        [ClassData(typeof(noEqualDistcreteFuzzySets_positive_testdata))]
        public void notEqualDistcreteFuzzySets_success(DiscreteFuzzySet<float> fs1, DiscreteFuzzySet<float> fs2, float expected)
        {
            //arrange
            //act
            float actual = ProbabilisticInterpretationOfRelationOnFuzzySets.noEqualDistcreteFuzzySets<float>(fs1, fs2);
            //assert
            Assert.Equal(expected, actual, 5);
        }

        class lessThanDistcreteFuzzySets_positive_testdata : TheoryData<DiscreteFuzzySet<float>, DiscreteFuzzySet<float>, float>
        {
            public lessThanDistcreteFuzzySets_positive_testdata()
            {
                Add(
                new DiscreteFuzzySet<float>(new List<float> { 3, 4, 5, 6 }, new List<float> { 0.2f, 0.5f, 0.9f, 1f }, null, FieldType.distFS_FLOAT),
                new DiscreteFuzzySet<float>(new List<float> { 6, 5, 4 }, new List<float> { 0.3f, 1f, 0.3f }, null, FieldType.distFS_FLOAT),
                0.22f
                );

            }
        }
        [Theory]
        [ClassData(typeof(lessThanDistcreteFuzzySets_positive_testdata))]
        public void lessThanDistcreteFuzzySets_success(DiscreteFuzzySet<float> fs1, DiscreteFuzzySet<float> fs2, float expected)
        {
            //arrange
            //act
            float actual = ProbabilisticInterpretationOfRelationOnFuzzySets.lessThanDistcreteFuzzySets<float>(fs1, fs2);
            //assert
            Assert.Equal(expected, actual, 5);
        }

        class lessThanEqualDistcreteFuzzySets_positive_testdata : TheoryData<DiscreteFuzzySet<float>, DiscreteFuzzySet<float>, float>
        {
            public lessThanEqualDistcreteFuzzySets_positive_testdata()
            {
                Add(
                new DiscreteFuzzySet<float>(new List<float> { 3, 4, 5, 6 }, new List<float> { 0.2f, 0.5f, 0.9f, 1f }, null, FieldType.distFS_FLOAT),
                new DiscreteFuzzySet<float>(new List<float> { 6, 5, 4 }, new List<float> { 0.3f, 1f, 0.3f }, null, FieldType.distFS_FLOAT),
                0.56f
                );

            }
        }
        [Theory]
        [ClassData(typeof(lessThanEqualDistcreteFuzzySets_positive_testdata))]
        public void lessThanEqualDistcreteFuzzySets_success(DiscreteFuzzySet<float> fs1, DiscreteFuzzySet<float> fs2, float expected)
        {
            //arrange
            //act
            float actual = ProbabilisticInterpretationOfRelationOnFuzzySets.lessThanEqualDistcreteFuzzySets<float>(fs1, fs2);
            //assert
            Assert.Equal(expected, actual, 5);
        }
        //here here
        class alsoFLOAT_positive_testdata : TheoryData<DiscreteFuzzySet<float>, DiscreteFuzzySet<float>, float>
        {
            public alsoFLOAT_positive_testdata()
            {
                Add(
                    new DiscreteFuzzySet<float>(new List<float> { 3.0f, 4.0f, 5.0f, 6.0f }, new List<float> { 0.2f, 0.5f, 0.9f, 1 }, "high", FieldType.distFS_INT),
                    new DiscreteFuzzySet<float>(new List<float> { 6.0f, 5.0f, 4.0f }, new List<float> { 0.3f, 1.0f, 0.3f }, "about_5", FieldType.distFS_INT),
                    0.53f
                    );
            }
        }
        [Theory]
        [ClassData(typeof(alsoFLOAT_positive_testdata))]
        public void alsoFLOAT_sucess(DiscreteFuzzySet<float> fs1, DiscreteFuzzySet<float> fs2, float expected)
        {
            //arrange
            //act
            float actual = ProbabilisticInterpretationOfRelationOnFuzzySets.alsoDistcreteFuzzySets<float>(fs1, fs2);
            //assert
            Assert.Equal(expected, actual, 1);
        }
        class compareFLOAT_positive_testdata : TheoryData<DiscreteFuzzySet<float>, DiscreteFuzzySet<float>, CompareOperation, float>
        {
            public compareFLOAT_positive_testdata()
            {
                foreach(var row in new equalDistcreteFuzzySets_positive_testdata())
                {
                    Add((DiscreteFuzzySet<float>)row[0], (DiscreteFuzzySet<float>)row[1], CompareOperation.EQUAL, (float)row[2]);
                }
                foreach (var row in new noEqualDistcreteFuzzySets_positive_testdata())
                {
                    Add((DiscreteFuzzySet<float>)row[0], (DiscreteFuzzySet<float>)row[1], CompareOperation.NOT_EQUAL, (float)row[2]);
                }
                foreach (var row in new lessThanDistcreteFuzzySets_positive_testdata())
                {
                    Add((DiscreteFuzzySet<float>)row[0], (DiscreteFuzzySet<float>)row[1], CompareOperation.LESS_THAN, (float)row[2]);

                    Add((DiscreteFuzzySet<float>)row[1], (DiscreteFuzzySet<float>)row[0], CompareOperation.GREATER_THAN, (float)row[2]);
                }
                foreach (var row in new lessThanEqualDistcreteFuzzySets_positive_testdata())
                {
                    Add((DiscreteFuzzySet<float>)row[0], (DiscreteFuzzySet<float>)row[1], CompareOperation.LESS_EQUAL, (float)row[2]);
                    Add((DiscreteFuzzySet<float>)row[1], (DiscreteFuzzySet<float>)row[0], CompareOperation.GREATER_EQUAL, (float)row[2]);
                }
                foreach (var row in new alsoFLOAT_positive_testdata())
                {
                    Add((DiscreteFuzzySet<float>)row[0], (DiscreteFuzzySet<float>)row[1], CompareOperation.ALSO, (float)row[2]);
                }
                //true content:
                Add(
                    new DiscreteFuzzySet<float>(new List<float> { 6.0f, 5.0f, 4.0f }, new List<float> { 0.3f, 1.0f, 0.3f }, "about_5", FieldType.distFS_INT),
                    new DiscreteFuzzySet<float>(new List<float> { 3.0f, 4.0f, 5.0f, 6.0f }, new List<float> { 0.2f, 0.5f, 0.9f, 1 }, "high", FieldType.distFS_INT),
                    CompareOperation.EQUAL,
                    0.34f
                    );



            }
        }
        [Theory]
        [ClassData(typeof(compareFLOAT_positive_testdata))]
        public void compareDistcreteFLOAT_sucess(DiscreteFuzzySet<float> fs1, DiscreteFuzzySet<float> fs2, CompareOperation compOp, float expected)
        {
            //arrange
            //act
            float actual = ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<float>(fs1, fs2, compOp);
            //assert
            Assert.Equal(expected, actual, 1);
        }
        

    }
}
