using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public abstract class AbstractFuzzyProbabilisticValue
    {
        public abstract bool isValueSetEmpty();
        public abstract bool hasSameValueList(AbstractFuzzyProbabilisticValue v);
        public abstract bool equals(AbstractFuzzyProbabilisticValue v);
        //public abstract bool isPrecise();
        //public abstract bool isExact();
    }
}
