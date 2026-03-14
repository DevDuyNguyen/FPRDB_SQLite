using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Enums;
using BLL.SQLProcessing;

namespace TestProject1.UnitTest
{
    public class ProbabilisticInterpretationOfRelationOnSetsTests
    {
        //not done: not enough test coverage
        class compareINT_positive_testdata:TheoryData<List<int>, List<int>, CompareOperation, float>
        {
            public compareINT_positive_testdata()
            {
                Add(new List<int> { 1, 2, 3 }, new List<int> { 1, 2, 3 }, CompareOperation.EQUAL, 1/3f);
                Add(new List<int> { 1, 2, 3 }, new List<int> { 1, 2}, CompareOperation.LESS_THAN, 1 / 6f);
            }
        }
        [Theory]
        [ClassData(typeof(compareINT_positive_testdata))]
        public void compareINT_success(List<int> s1, List<int> s2, CompareOperation compOperator,float expected)
        {
            //arrange
            //act
            float actual = ProbabilisticInterpretationOfRelationOnSets.compare<int>(s1, s2, compOperator);
            //assert
            Assert.Equal(expected, actual, 10);
        }

        class compareFLOAT_positive_testdata : TheoryData<List<float>, List<float>, CompareOperation, float>
        {
            public compareFLOAT_positive_testdata()
            {
                Add(new List<float> { 1.0f, 2.1f, 3 }, new List<float> { 1.0f, 2.1f, 3 }, CompareOperation.EQUAL, 1 / 3f);
                Add(new List<float> { 1.0f, 2.1f, 3 }, new List<float> { 1.0f, 2.1f }, CompareOperation.LESS_THAN, 1 / 6f);
            }
        }
        [Theory]
        [ClassData(typeof(compareFLOAT_positive_testdata))]
        public void compareFLOAT_success(List<float> s1, List<float> s2, CompareOperation compOperator, float expected)
        {
            //arrange
            //act
            float actual = ProbabilisticInterpretationOfRelationOnSets.compare<float>(s1, s2, compOperator);
            //assert
            Assert.Equal(expected, actual, 10);
        }

    }
}
