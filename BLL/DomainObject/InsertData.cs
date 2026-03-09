using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class InsertData
    {
        public string relation;
        public List<string> fieldList;
        public List<FuzzyProbabilisticValueParsingData> fuzzyProbabilisticValues;

        public InsertData(string relation, List<string> fieldList, List<FuzzyProbabilisticValueParsingData> fuzzyProbabilisticValues)
        {
            this.relation = relation;
            this.fieldList = fieldList;
            this.fuzzyProbabilisticValues = fuzzyProbabilisticValues;
        }
    }
}
