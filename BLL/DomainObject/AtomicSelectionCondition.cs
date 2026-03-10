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
