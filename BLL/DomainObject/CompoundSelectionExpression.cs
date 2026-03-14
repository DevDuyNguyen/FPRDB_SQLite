using BLL.Common;
using BLL.Enums;
using BLL.Interfaces;
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

        public override List<float> calculateProbabilisticInterpretation(Scan currentTuple, FPRDBSchema schema)
        {
            List<float> interval1 = this.lSelectionExpresion.calculateProbabilisticInterpretation(currentTuple, schema);
            List<float> interval2 = this.rSelectionExpresion.calculateProbabilisticInterpretation(currentTuple, schema);
            return ProbabilisticCombinationStrategyUtilities.combine(interval1[0], interval1[1], interval2[0], interval2[1], this.probCombStrategy);
        }
        public override List<SelectionExpression> getAtomicSelectionExpression()
        {
            return this.lSelectionExpresion.getAtomicSelectionExpression().Concat(this.rSelectionExpresion.getAtomicSelectionExpression()).ToList();
        }
    }
}
