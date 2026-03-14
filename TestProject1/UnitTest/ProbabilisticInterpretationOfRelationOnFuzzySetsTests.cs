using BLL;
using BLL.DomainObject;
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

    }
}
