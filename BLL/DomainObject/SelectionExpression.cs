using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public abstract class SelectionExpression
    {
        public abstract List<SelectionExpression> getAtomicSelectionExpression();
        public abstract List<float> calculateProbabilisticInterpretation(Scan currentTuple, FPRDBSchema schema);
    }
}
