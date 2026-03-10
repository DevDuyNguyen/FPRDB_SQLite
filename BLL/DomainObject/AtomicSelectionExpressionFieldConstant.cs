using BLL.Enums;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class AtomicSelectionExpressionFieldConstant:SelectionExpression
    {
        public string field;
        public Constant constant;
        public CompareOperation compareOperator;

        public AtomicSelectionExpressionFieldConstant(string field, Constant constant, CompareOperation compareOperator)
        {
            this.field = field;
            this.constant = constant;
            this.compareOperator = compareOperator;
        }

        public override float calculateProbabilisticInterpretation(Scan currentTuple)
        {


        }
        public override List<SelectionExpression> getAtomicSelectionExpression()
        {
            return new List<SelectionExpression> { this };
        }

    }
}
