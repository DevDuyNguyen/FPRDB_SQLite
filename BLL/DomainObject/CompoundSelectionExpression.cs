using BLL.Enums;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class CompoundSelectionExpression:SelectionExpression
    {
        public SelectionExpression lSelectionExpresion;
        public SelectionExpression rSelectionExpresion;
        public ProbabilisticCombinationStrategy probCombStrategy;

        public CompoundSelectionExpression(SelectionExpression lSelectionExpresion, SelectionExpression rSelectionExpresion, ProbabilisticCombinationStrategy probCombStrategy)
        {
            this.lSelectionExpresion = lSelectionExpresion;
            this.rSelectionExpresion = rSelectionExpresion;
            this.probCombStrategy = probCombStrategy;
        }

        public override float calculateProbabilisticInterpretation(Scan currentTuple)
        {
            throw new NotImplementedException();
        }
        public override List<SelectionExpression> getAtomicSelectionExpression()
        {
            throw new NotImplementedException();
        }
    }
}
