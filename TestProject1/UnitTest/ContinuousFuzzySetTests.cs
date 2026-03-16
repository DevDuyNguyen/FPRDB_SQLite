using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Enums;
using BLL;

namespace TestProject1.UnitTest
{
    public class ContinuousFuzzySetTests
    {
        class isSubsetOf_postive_negative_testdata : TheoryData<FuzzySet<float>, FuzzySet<float>, bool>
        {
            public isSubsetOf_postive_negative_testdata()
            {

                Add(
                new ContinuousFuzzySet(19, 20, 30, 40, null),
                new DiscreteFuzzySet<float>(new List<float> { 2.1f, 3.2f, 1.3f }, new List<float> { 0.2f, 0.3f, 0.3f, 1f }, "fs1", FieldType.distFS_FLOAT),
                false
                );

                Add(
                new ContinuousFuzzySet(19, 20, 30, 40, null),
                new ContinuousFuzzySet(19, 22, 30, 40, null),
                false
                );

                Add(
                new ContinuousFuzzySet(19, 20, 30, 40, null),
                new ContinuousFuzzySet(19, 18, 30, 40, null),
                true
                );

                Add(
                new ContinuousFuzzySet(19, 20, 30, 40, null),
                new ContinuousFuzzySet(10, 20, 30, 40, null),
                true
                );
            }
        }
        [Theory]
        [ClassData(typeof(isSubsetOf_postive_negative_testdata))]
        public void isSubsetOf_sucess_fail(FuzzySet<float> fs1, FuzzySet<float> fs2, bool expected)
        {
            //arrange
            //act
            //assert
            Assert.Equal(expected, fs1.isSubsetOf(fs2));
        }

    }
}
