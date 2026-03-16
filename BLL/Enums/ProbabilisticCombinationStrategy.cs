using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Enums
{
    public enum ProbabilisticCombinationStrategy
    {
        CONJUNCTION_IGNORANCE,
        CONJUNCTION_INDEPENDANCE,
        CONJUNCTION_POSITIVE_CORRELATION,
        CONJUNCTION_MUTUAL_EXCLUSION,
        DISJUNCTION_IGNORANCE,
        DISJUNCTION_MUTUAL_EXCLUSION,
        DISJUNCTION_POSITIVE_CORRELATION,
        DISJUNCTION_INDEPENDANCE,
        DIFFERENCE_IGNORANCE,
        DIFFERENCE_MUTUAL_EXCLUSION,
        DIFFERENCE_POSITIVE_CORRELATION,
        DIFFERENCE_INDEPENDANCE
    }
}
