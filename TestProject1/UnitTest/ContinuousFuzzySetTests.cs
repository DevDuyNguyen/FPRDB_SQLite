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
                new ContinuousFuzzySet(19, 20, 30, 40, null, -1),
                new DiscreteFuzzySet<float>(new List<float> { 2.1f, 3.2f, 1.3f }, new List<float> { 0.2f, 0.3f, 0.3f, 1f }, "fs1", FieldType.distFS_FLOAT, -1),
                false
                );

                Add(
                new ContinuousFuzzySet(19, 20, 30, 40, null, -1),
                new ContinuousFuzzySet(19, 22, 30, 40, null, -1),
                false
                );

                Add(
                new ContinuousFuzzySet(19, 20, 30, 40, null, -1),
                new ContinuousFuzzySet(19, 18, 30, 40, null, -1),
                true
                );

                Add(
                new ContinuousFuzzySet(19, 20, 30, 40, null, -1),
                new ContinuousFuzzySet(10, 20, 30, 40, null, -1),
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

        class StandardIntersection_postive_negative_testdata : TheoryData<FuzzySet<float>, FuzzySet<float>, FuzzySet<float>>
        {
            public StandardIntersection_postive_negative_testdata()
            {
                var young = new ContinuousFuzzySet(19, 20, 30, 40, "young", -1);
                var fs1 = new DiscreteFuzzySet<float>(new List<float> { 2.1f, 3.2f, 1.3f }, new List<float> { 0.2f, 0.3f, 0.3f}, "fs1", FieldType.distFS_FLOAT, -1);
                var upperYoung= new ContinuousFuzzySet(18, 19, 25, 40, "young1", -1);
                var lowerYoung = new ContinuousFuzzySet(20, 25, 30, 40, "young1", -1);
                var approx_25= new ContinuousFuzzySet(19, 25, 25, 40, "approx_25", -1);
                var middle_aged= new ContinuousFuzzySet(35, 50, 50, 60, "middle_aged", -1);

                Add(
                young,
                fs1,
                 new DiscreteFuzzySet<float>(new List<float> { 2.1f, 3.2f, 1.3f }, new List<float> { young.getMembershipDegree(0.2f), young.getMembershipDegree(0.3f), young.getMembershipDegree(0.3f) }, "fs1", FieldType.distFS_FLOAT, -1)
                );

                Add(
                young,
                upperYoung,
                young
                );

                Add(
                young,
                lowerYoung,
                lowerYoung
                );

                Add(
                young,
                approx_25,
                new ContinuousFuzzySet(10, 0, 0, 40, young, approx_25, young.getName() + "⋂" + approx_25.getName())
                );

                Add(
                young,
                middle_aged,
                new ContinuousFuzzySet(10, 0, 0, 60, young, middle_aged, young.getName() + "⋂" + middle_aged.getName())
                );
            }
        }
        [Theory]
        [ClassData(typeof(StandardIntersection_postive_negative_testdata))]
        public void StandardIntersection_sucess_fail(FuzzySet<float> fs1, FuzzySet<float> fs2, FuzzySet<float> expected)
        {
            //arrange
            //act
            var actual = fs1.StandardIntersection(fs2);
            //assert
            Assert.Equivalent(expected, actual);
        }




    }
}
