using BLL.Enums;
using BLL.Interfaces;
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

        public override bool isSatisfied(Scan currentTuple, FPRDBSchema schema)
        {
            if (this.connective == LogicalConnective.NOT)
            {
                return !(this.lSelectionCondition.isSatisfied(currentTuple, schema));
            }
            else if (this.connective == LogicalConnective.AND)
            {
                return this.lSelectionCondition.isSatisfied(currentTuple, schema) && this.rSelectionCondition.isSatisfied(currentTuple, schema);
            }
            else //if (this.connective == LogicalConnective.OR)
            {
                return this.lSelectionCondition.isSatisfied(currentTuple, schema) || this.rSelectionCondition.isSatisfied(currentTuple, schema);
            }
        }
        public override List<SelectionExpression> getAtomicSelectionExpressions()
        {
            return this.lSelectionCondition.getAtomicSelectionExpressions().Concat(this.rSelectionCondition.getAtomicSelectionExpressions()).ToList();
        }
        public override List<string> getMentionedAttributes()
        {
            List<string> ans = new List<string>(this.lSelectionCondition.getMentionedAttributes());
            ans.AddRange(this.rSelectionCondition.getMentionedAttributes());
            return ans;
        }
    }
}
