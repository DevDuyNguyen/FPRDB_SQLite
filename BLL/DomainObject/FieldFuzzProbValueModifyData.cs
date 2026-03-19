using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class FieldFuzzProbValueModifyData:ModifyData
    {
        public FuzzyProbabilisticValueParsingData fuzzyProbabilisticValue;

        public FieldFuzzProbValueModifyData(FuzzyProbabilisticValueParsingData fuzzyProbabilisticValue, string relation, string assignedField, SelectionCondition selectionCondition):base(relation, assignedField, selectionCondition)
        {
            this.fuzzyProbabilisticValue = fuzzyProbabilisticValue;
        }

        public override object getAssignValue() => this.fuzzyProbabilisticValue;
    }
}
