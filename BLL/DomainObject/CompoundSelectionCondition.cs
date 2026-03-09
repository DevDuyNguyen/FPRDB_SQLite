using BLL.Enums;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class CompoundSelectionCondition:SelectionCondition
    {
        public SelectionCondition lSelectionCondition;
        public SelectionCondition rSelectionCondition;
        public LogicalConnective connective;

        public CompoundSelectionCondition(SelectionCondition lSelectionCondition, SelectionCondition rSelectionCondition, LogicalConnective connective)
        {
            this.lSelectionCondition = lSelectionCondition;
            this.rSelectionCondition = rSelectionCondition;
            this.connective = connective;
        }

        public override bool isSatisfied(Scan currentTuple)
        {
            throw new NotImplementedException();
        }
        public override List<SelectionExpression> getSelectionExpressions()
        {
            throw new NotImplementedException();
        }

    }
}
