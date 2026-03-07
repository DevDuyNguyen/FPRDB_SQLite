using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class FuzzyProbabilisticValueParsingData
    {
        public List<Constant> valueList;
        public List<float> intervalProbLowerBoundList;
        public List<float> intervalProbUpperBoundList;
        public FuzzyProbabilisticValueParsingData() { }

        public FuzzyProbabilisticValueParsingData(List<Constant> valueList, List<float> intervalProbLowerBoundList, List<float> intervalProbUpperBoundList)
        {
            this.valueList = valueList;
            this.intervalProbLowerBoundList = intervalProbLowerBoundList;
            this.intervalProbUpperBoundList = intervalProbUpperBoundList;
        }
    }
}
