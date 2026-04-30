using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainObject;
using BLL.Enums;
using BLL.SQLProcessing;

namespace TestProject1.DataTesting
{
    public class CalculateProbabilisticInterpretationForRelationOnFuzzySetsExpressionTests
    {
        public class CompareBetweenINTAndFloatDefiningDomainFuzzySetsTestData:TheoryData<BaseFuzzySet, BaseFuzzySet, CompareOperation, float>
        {
            public CompareBetweenINTAndFloatDefiningDomainFuzzySetsTestData()
            {
                Add(
                    new DiscreteFuzzySet<int>(new List<int> { 15 }, new List<float>{1}, BLL.FieldType.distFS_INT),
                    new ContinuousFuzzySet(10, 15, 15, 20, "approx_15", -1),
                    CompareOperation.GREATER_THAN,
                    0.48963f
                    );

            }

        }

        [Theory]
        [ClassData(typeof(CompareBetweenINTAndFloatDefiningDomainFuzzySetsTestData))]
        public void compareBetweenINTAndFloatDefiningDomainFuzzySetsTest(BaseFuzzySet fs1, BaseFuzzySet fs2, CompareOperation compOp, float expected)
        {
            //arrange
            //act
            float actual = ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet(fs1, fs2, compOp);
            //assert
            Assert.Equal(expected, actual, 0.02);
        }

        


    }
}
