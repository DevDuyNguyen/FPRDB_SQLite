using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.DomainObject;
using BLL.Interfaces;

namespace TestProject1.UnitTest
{
    public class DiscreteFuzzySetTests
    {
        //not done: not enough test coverage
        class StandardIntersection_positive_testdata:TheoryData<FuzzySet<float>, FuzzySet<float>, FuzzySet<float>>
        {
            public StandardIntersection_positive_testdata()
            {
                Add(
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f, 3.2f, 1.3f}, new List<float> { 0.2f, 0.2f, 0.3f, 1f}, "fs1", FieldType.distFS_FLOAT),
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f, 1.2f, 4 }, new List<float> { 0.1f, 0.2f, 0.3f, 1f }, "fs2", FieldType.distFS_FLOAT),
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f, 3.2f, 1.2f, 1.3f, 4 }, new List<float> { 0.1f, 0, 0, 0,0 }, "fs1⋂fs2", FieldType.distFS_FLOAT)
                    );
            }

        }
        [Theory]
        [ClassData(typeof(StandardIntersection_positive_testdata))]
        public void StandardIntersection_success(FuzzySet<float> fs1, FuzzySet<float> fs2, FuzzySet<float> expected)
        {
            //arrange
            //act
            FuzzySet<float> actual = fs1.StandardIntersection(fs2);
            //assert
            if(expected is DiscreteFuzzySet<float>)
            {
                DiscreteFuzzySet<float> dExpected = (DiscreteFuzzySet<float>)(object)expected;
                DiscreteFuzzySet<float> dActual = (DiscreteFuzzySet<float>)(object)actual;
                Assert.Equal(dExpected.valueSet.Count, dActual.valueSet.Count);
                Assert.Equal(dExpected.getName(), dActual.getName());
                foreach (float v in dExpected.valueSet)
                {
                    Assert.Equal(true, dActual.valueSet.Contains(v));
                }
            }
        }

        class getHeight_positive_testdata : TheoryData<FuzzySet<float>, float>
        {
            public getHeight_positive_testdata()
            {
                Add(
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f, 3.2f, 1.3f }, new List<float> { 0.2f, 0.2f, 0.3f, 1f }, "fs1", FieldType.distFS_FLOAT),
                    1
                    );
                Add(
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f, 1.2f, 4 }, new List<float> { 0.1f, 0.2f, 0.3f, 0.2f }, "fs2", FieldType.distFS_FLOAT),
                    0.3f
                    );
                Add(
                    new DiscreteFuzzySet<float>(new List<float> { 1.1f, 3.2f, 1.2f, 1.3f, 4 }, new List<float> { 0.1f, 0, 0, 0, 0 }, "fs1⋂fs2", FieldType.distFS_FLOAT),
                    0.1f
                    );
                
                    
            }

        }
        [Theory]
        [ClassData(typeof(getHeight_positive_testdata))]
        public void getHeight_success(DiscreteFuzzySet<float> fs, float expected)
        {
            //arrange
            //act
            //assert
            Assert.Equal(expected, fs.getHeight(), 3);
        }
        class isEqualTo_postive_negative_testdata:TheoryData<FuzzySet<float>, FuzzySet<float>, bool>
        {
            public isEqualTo_postive_negative_testdata()
            {
                Add(
                new DiscreteFuzzySet<float>(new List<float> { 1.1f, 3.2f, 1.3f }, new List<float> { 0.2f, 0.2f, 0.3f, 1f }, "fs1", FieldType.distFS_FLOAT),
                new DiscreteFuzzySet<float>(new List<float> { 1.1f, 1.2f, 4 }, new List<float> { 0.1f, 0.2f, 0.3f, 0.2f }, "fs2", FieldType.distFS_FLOAT),
                false
                );

                Add(
                new DiscreteFuzzySet<float>(new List<float> { 1.1f, 3.2f, 1.3f }, new List<float> { 0.2f, 0.2f, 0.3f, 1f }, "fs1", FieldType.distFS_FLOAT),
                new DiscreteFuzzySet<float>(new List<float> { 1.1f, 3.2f, 1.3f }, new List<float> { 0.2f, 0.2f, 0.3f, 1f }, "fs1", FieldType.distFS_FLOAT),
                true
                );

                Add(
                new DiscreteFuzzySet<float>(new List<float> { 1.1f, 1.2f, 4 }, new List<float> { 0.1f, 0.2f, 0.3f, 0.2f }, "fs2", FieldType.distFS_FLOAT),
                new DiscreteFuzzySet<float>(new List<float> { 1.1f, 1.2f, 4 }, new List<float> { 0.1f, 0.2f, 0.3f, 0.2f }, "fs2", FieldType.distFS_FLOAT),
                true
                );
            }
        }
        [Theory]
        [ClassData(typeof(isEqualTo_postive_negative_testdata))]
        public void isEqualTo_sucess_fail(FuzzySet<float> fs1, FuzzySet<float> fs2, bool expected)
        {
            //arrange
            //act
            //assert
            Assert.Equal(expected, fs1.isEqualTo(fs2));
        }


    }
}
