using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public abstract class SelectionCondition
    {
        public abstract bool isSatisfied(Scan currentTuple, FPRDBSchema schema);
        public abstract List<SelectionExpression> getSelectionExpressions();
    }
}
