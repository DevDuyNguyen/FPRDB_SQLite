using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class AtomicSelectionCondition:SelectionCondition
    {
        public SelectionExpression selectionExpression;
        public float lowerBound;
        public float upperBound;

        public AtomicSelectionCondition(SelectionExpression selectionExpression, float lowerBound, float upperBound)
        {
            this.selectionExpression = selectionExpression;
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
        }

        public override bool isSatisfied(Scan currentTuple, FPRDBSchema schema)
        {
            List<float> intervalProb = this.selectionExpression.calculateProbabilisticInterpretation(currentTuple, schema);
            if (intervalProb[0] >= this.lowerBound && intervalProb[1] <= this.upperBound)
                return true;
            else
                return false;
        }
        public override List<SelectionExpression> getAtomicSelectionExpressions()
        {
            return this.selectionExpression.getAtomicSelectionExpression();
        }
        public override List<string> getMentionedAttributes() => this.selectionExpression.getMentionedAttributes();

    }
}
