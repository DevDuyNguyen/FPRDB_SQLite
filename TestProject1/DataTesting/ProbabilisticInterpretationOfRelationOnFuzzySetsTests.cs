using BLL;
using BLL.DomainObject;
using BLL.Enums;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.DataTesting
{
    public class ProbabilisticInterpretationOfRelationOnFuzzySetsTests
    {
        class constantAndConstantTestData:TheoryData<Type, BaseFuzzySet, BaseFuzzySet, CompareOperation, float>
        {
            public constantAndConstantTestData()
            {
                //“abc”=”abc”->1
                Add(
                    typeof(string),
                    new DiscreteFuzzySet<string>(new List<string> { "abc" }, new List<float> { 1}, FieldType.VARCHAR),
                    new DiscreteFuzzySet<string>(new List<string> { "abc" }, new List<float> { 1 }, FieldType.VARCHAR),
                    CompareOperation.EQUAL,
                    1
                    );
                //True!=false->1
                Add(
                    typeof(bool),
                    new DiscreteFuzzySet<bool>(new List<bool> { true }, new List<float> { 1 }, FieldType.BOOLEAN),
                    new DiscreteFuzzySet<bool>(new List<bool> { false }, new List<float> { 1 }, FieldType.BOOLEAN),
                    CompareOperation.NOT_EQUAL,
                    1
                    );
                //1.1=1.1->1
                Add(
                    typeof(float),
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f }, new List<float> { 1 }, FieldType.FLOAT),
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f }, new List<float> { 1 }, FieldType.FLOAT),
                    CompareOperation.EQUAL,
                    1
                    );
                //2<3->1
                Add(
                    typeof(int),
                    new DiscreteFuzzySet<int>(new List<int> { 2 }, new List<float> { 1 }, FieldType.INT),
                    new DiscreteFuzzySet<int>(new List<int> { 3 }, new List<float> { 1 }, FieldType.INT),
                    CompareOperation.LESS_THAN,
                    1
                    );
                //1.1=>1.1->1
                Add(
                    typeof(float),
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f }, new List<float> { 1 }, FieldType.FLOAT),
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f }, new List<float> { 1 }, FieldType.FLOAT),
                    CompareOperation.GREATER_EQUAL,
                    1
                    );

            }
        }
        [Theory]
        [ClassData(typeof(constantAndConstantTestData))]
        public void constantAndConstantTesting(Type t, BaseFuzzySet fs1, BaseFuzzySet fs2, CompareOperation op, float expected)
        {
            //arrange
            //act
            float actual;
            if (t == typeof(int))
                actual = ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<int>(fs1 as FuzzySet<int>, fs2 as FuzzySet<int>, op);
            else if (t == typeof(float))
                actual = ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<float>(fs1 as FuzzySet<float>, fs2 as FuzzySet<float>, op);
            else if (t == typeof(string))
                actual = ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<string>(fs1 as FuzzySet<string>, fs2 as FuzzySet<string>, op);
            else //if (t == typeof(string))
                actual = ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<bool>(fs1 as FuzzySet<bool>, fs2 as FuzzySet<bool>, op);
            //assert
            Assert.Equal(expected, actual);
        }



    }
}
