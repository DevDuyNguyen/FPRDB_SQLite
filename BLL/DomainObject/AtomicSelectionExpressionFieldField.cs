using BLL.SQLProcessing;
using BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class AtomicSelectionExpressionFieldField:SelectionExpression
    {
        public string lField;
        public string rField;
        public ProbabilisticCombinationStrategy probCombinationStrategy;
public AtomicSelectionExpressionFieldField(string lField, string rField, ProbabilisticCombinationStrategy probCombinationStrategy)
        {
            this.lField = lField;
            this.rField = rField;
            this.probCombinationStrategy = probCombinationStrategy;
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
