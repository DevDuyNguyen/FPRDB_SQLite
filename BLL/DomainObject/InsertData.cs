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
        public FuzzyProbabilisticValueParsingData getInsertDataByFieldName(string fldName)
        {
            for(int i=0; i<fieldList.Count; ++i)
            {
                if (this.fieldList[i] == fldName)
                    return this.fuzzyProbabilisticValues[i];
            }
            return null;
        }
    }
}
