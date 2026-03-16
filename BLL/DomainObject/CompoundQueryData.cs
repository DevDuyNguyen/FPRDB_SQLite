using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Enums;

namespace BLL.DomainObject
{
    public class CompoundQueryData:QueryData
    {
        public QueryData leftQuery;
        public QueryData rightQuery;
        public SetConnective setConnective;
        public ProbabilisticCombinationStrategy probCombinationStrategy;

        public CompoundQueryData(QueryData leftQuery, QueryData rightQuery, SetConnective setConnective, ProbabilisticCombinationStrategy probCombinationStrategy)
        {
            this.leftQuery = leftQuery;
            this.rightQuery = rightQuery;
            this.setConnective = setConnective;
            this.probCombinationStrategy = probCombinationStrategy;
        }
    }
}
