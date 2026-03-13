using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.DomainObject;
using BLL.Enums;
using BLL.SQLProcessing;

namespace TestProject1.UnitTest
{
    public class ProbabilisticInterpretationOfRelationOnFuzzySetTests
    {
        class compareFLOAT_positive_testdata :TheoryData<DiscreteFuzzySet<float>, DiscreteFuzzySet<float>, CompareOperation, float>
        {
            public compareFLOAT_positive_testdata()
            {
                Add(
                    new DiscreteFuzzySet<float>(new List<float> { 6.0f, 5.0f, 4.0f }, new List<float> { 0.3f, 1.0f, 0.3f }, "about_5", FieldType.distFS_INT), 
                    new DiscreteFuzzySet<float>(new List<float> { 3.0f, 4.0f, 5.0f, 6.0f}, new List<float> { 0.2f, 0.5f, 0.9f, 1}, "high", FieldType.distFS_INT),
                    CompareOperation.EQUAL,
                    0.34f
                    );
            }
        }
        [Theory]
        [ClassData(typeof(compareFLOAT_positive_testdata))]
        public void compareFLOAT_sucess(DiscreteFuzzySet<float> fs1, DiscreteFuzzySet<float> fs2, CompareOperation compOp, float expected)
        {
            //arrange
            //act
            float actual = ProbabilisticInterpretationOfRelationOnFuzzySets.compare<float>(fs1, fs2, compOp);
            //assert
            Assert.Equal(expected, actual, 1);
        }
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
            float actual = ProbabilisticInterpretationOfRelationOnFuzzySets.also<float>(fs1, fs2);
            //assert
            Assert.Equal(expected, actual, 1);
        }

    }
}
