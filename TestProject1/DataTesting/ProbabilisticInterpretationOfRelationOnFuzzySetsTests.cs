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

        class constantAndConstantInvalidOp_TestData : TheoryData<Type, BaseFuzzySet, BaseFuzzySet, CompareOperation, Exception, Type>
        {
            public constantAndConstantInvalidOp_TestData()
            {
                //true<true -> error
                Add(
                    typeof(bool),
                    new DiscreteFuzzySet<bool>(new List<bool> { true }, new List<float> { 1 }, FieldType.BOOLEAN),
                    new DiscreteFuzzySet<bool>(new List<bool> { true }, new List<float> { 1 }, FieldType.BOOLEAN),
                    CompareOperation.LESS_THAN,
                    new InvalidOperationException("Probabilistic interpretation for < can't be applied on boolean special fuzzy set"),
                    typeof(InvalidOperationException)
                    );
            }
        }
        [Theory]
        [ClassData(typeof(constantAndConstantInvalidOp_TestData))]
        public void constantAndConstantInvalidOp_Testing(Type t, BaseFuzzySet fs1, BaseFuzzySet fs2, CompareOperation op, Exception expected, Type exceptionType)
        {
            //arrange
            //act
            Exception actual;
            if (t == typeof(int))
                actual = Assert.Throws(exceptionType, () =>ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<int>(fs1 as FuzzySet<int>, fs2 as FuzzySet<int>, op));
            else if (t == typeof(float))
                actual = Assert.Throws(exceptionType, () =>ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<float>(fs1 as FuzzySet<float>, fs2 as FuzzySet<float>, op));
            else if (t == typeof(string))
                actual = Assert.Throws(exceptionType, () =>ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<string>(fs1 as FuzzySet<string>, fs2 as FuzzySet<string>, op));
            else //if (t == typeof(string))
                actual = Assert.Throws(exceptionType, () =>ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<bool>(fs1 as FuzzySet<bool>, fs2 as FuzzySet<bool>, op));
            //assert
            if (expected == null)
                Assert.Equal(null, actual);
            Assert.Equal(expected.Message, actual.Message);
        }
        class differentDefiningDomainConstantAndConstant_TestData : TheoryData<Type, BaseFuzzySet, Type, BaseFuzzySet, CompareOperation, float>
        {
            public differentDefiningDomainConstantAndConstant_TestData()
            {
                //1<1.1->1
                Add(
                    typeof(int),
                    new DiscreteFuzzySet<int>(new List<int> { 1 }, new List<float> { 1 }, FieldType.INT),
                    typeof(float),
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f }, new List<float> { 1 }, FieldType.FLOAT),
                    CompareOperation.LESS_THAN,
                    1
                    );
                //1!=1.1->1
                Add(
                    typeof(int),
                    new DiscreteFuzzySet<int>(new List<int> { 1 }, new List<float> { 1 }, FieldType.INT),
                    typeof(float),
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f }, new List<float> { 1 }, FieldType.FLOAT),
                    CompareOperation.NOT_EQUAL,
                    1
                    );
                //4>1.1->1
                Add(
                    typeof(int),
                    new DiscreteFuzzySet<int>(new List<int> { 4 }, new List<float> { 1 }, FieldType.INT),
                    typeof(float),
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f }, new List<float> { 1 }, FieldType.FLOAT),
                    CompareOperation.GREATER_THAN,
                    1
                    );
            }
        }
        [Theory]
        [ClassData(typeof(differentDefiningDomainConstantAndConstant_TestData))]
        public void differentDefiningDomainConstantAndConstant(Type t1, BaseFuzzySet fs1, Type t2, BaseFuzzySet fs2, CompareOperation op, float expected)
        {
            //arrange
            //act
            float actual;
            if (t1 == typeof(int) && t2==typeof(float))
                actual = ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<int, float>(fs1 as FuzzySet<int>, fs2 as FuzzySet<float>, op);
            else //if (t1 == typeof(float) && t2 == typeof(int))
                actual = ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<float, int>(fs1 as FuzzySet<float>, fs2 as FuzzySet<int>, op);
            //assert
            Assert.Equal(expected, actual);
        }
        class incompatibleDifferentDefiningDomainConstantAndConstant_TestData : TheoryData<Type, BaseFuzzySet, Type, BaseFuzzySet, CompareOperation, Exception, Type>
        {
            public incompatibleDifferentDefiningDomainConstantAndConstant_TestData()
            {
                //“abc”=1-> error
                Add(
                    typeof(string),
                    new DiscreteFuzzySet<string>(new List<string> { "abc" }, new List<float> { 1 }, FieldType.VARCHAR),
                    typeof(int),
                    new DiscreteFuzzySet<int>(new List<int> { 1 }, new List<float> { 1 }, FieldType.INT),
                    CompareOperation.EQUAL,
                    new InvalidOperationException($"{typeof(string).Name} and {typeof(int).Name} aren't compatible for probabilistic interpretation for relation on fuzzy sets"),
                    typeof(InvalidOperationException)
                    );
                //True>1->error
                Add(
                    typeof(bool),
                    new DiscreteFuzzySet<bool>(new List< bool> { true }, new List<float> { 1 }, FieldType.BOOLEAN),
                    typeof(int),
                    new DiscreteFuzzySet<int>(new List<int> { 1 }, new List<float> { 1 }, FieldType.INT),
                    CompareOperation.GREATER_THAN,
                    new InvalidOperationException($"{typeof(bool).Name} and {typeof(int).Name} aren't compatible for probabilistic interpretation for relation on fuzzy sets"),
                    typeof(InvalidOperationException)
                    );
            }
        }
        [Theory]
        [ClassData(typeof(incompatibleDifferentDefiningDomainConstantAndConstant_TestData))]
        public void incompatibleDifferentDefiningDomainConstantAndConstant_Testing(Type t1, BaseFuzzySet fs1, Type t2, BaseFuzzySet fs2, CompareOperation op, Exception expected, Type expectedExceptionType)
        {
            //arrange
            Exception actual=null;
            //act
            if (t1 == typeof(string) && t2 == typeof(int))
                actual = Assert.Throws(expectedExceptionType, () => ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<string, int>(fs1 as FuzzySet<string>, fs2 as FuzzySet<int>, op));
            else if (t1 == typeof(bool) && t2 == typeof(int))
                actual = Assert.Throws(expectedExceptionType, () => ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<bool, int>(fs1 as FuzzySet<bool>, fs2 as FuzzySet<int>, op));
            //assert
            Assert.Equal(expected.Message, actual.Message);
        }
        class sameDefiningDomainsConstantFuzzySet_testdata : TheoryData<Type, BaseFuzzySet, BaseFuzzySet, CompareOperation, float>
        {
            public sameDefiningDomainsConstantFuzzySet_testdata()
            {
                //1.1=>{1.1:1, 2:0.1, 3:0.5}->1
                Add(
                    typeof(float),
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f }, new List<float> { 1 }, FieldType.FLOAT),
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f, 2, 3 }, new List<float> { 1, 0.1f, 0.5f }, FieldType.VARCHAR),
                    CompareOperation.ALSO,
                    1
                    );
                //1.1={1.1:1, 2:0.1, 3:0.5}->11/15
                Add(
                    typeof(float),
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f }, new List<float> { 1 }, FieldType.FLOAT),
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f, 2, 3 }, new List<float> { 1, 0.1f, 0.5f }, FieldType.VARCHAR),
                    CompareOperation.EQUAL,
                    11.0f / 15.0f
                    );
                //15.0=> Approx_15 (15, 1), (10, 0) (20, 0) -> 1
                Add(
                    typeof(float),
                    new DiscreteFuzzySet<float>(new List<float> { 15 }, new List<float> { 1 }, FieldType.FLOAT),
                    new ContinuousFuzzySet(10, 15, 15, 20),
                    CompareOperation.ALSO,
                    1
                    );
                //15.0 > Approx_15(15, 1), (10, 0)(20, 0)-> 1
                Add(
                    typeof(float),
                    new DiscreteFuzzySet<float>(new List<float> { 15 }, new List<float> { 1 }, FieldType.FLOAT),
                    new ContinuousFuzzySet(10, 15, 15, 20),
                    CompareOperation.GREATER_THAN,
                    0.48963f
                    );

            }
        }
        [Theory]
        [ClassData(typeof(sameDefiningDomainsConstantFuzzySet_testdata))]
        public void sameDefiningDomainsConstantFuzzySet_testing(Type t, BaseFuzzySet fs1, BaseFuzzySet fs2, CompareOperation op, float expected)
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
            Assert.Equal(expected, actual, 0.05);
        }




    }
}
