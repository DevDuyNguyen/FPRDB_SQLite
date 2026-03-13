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


    }
}
