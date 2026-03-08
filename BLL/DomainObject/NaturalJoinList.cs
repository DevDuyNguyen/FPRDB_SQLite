using BLL.Enums;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class NaturalJoinList
    {
        public List<string> relationList;
        public List<ProbabilisticCombinationStrategy> probCombinationStrategyList;

        public NaturalJoinList(List<string> relationList, List<ProbabilisticCombinationStrategy> probCombinationStrategyList)
        {
            this.relationList = relationList;
            this.probCombinationStrategyList = probCombinationStrategyList;
        }
    }
}
