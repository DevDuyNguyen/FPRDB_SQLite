using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ConstraintDAO
    {
        public bool isTupleWithFuzzyProbabilisticValuesExist(string relation, List<string> attributeList, List<AbstractFuzzyProbabilisticValue> contentList);
    }
}
