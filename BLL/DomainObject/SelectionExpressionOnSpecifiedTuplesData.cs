using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class SelectionExpressionOnSpecifiedTuplesData
    {
        public string relation;
        public SelectionExpression selectionExpression;
        public int from;
        public int take;
        public bool isHasSpecifiedTuples;

        public SelectionExpressionOnSpecifiedTuplesData(string relation, SelectionExpression selectionExpression, int from, int take)
        {
            this.relation = relation;
            this.selectionExpression = selectionExpression;
            this.from = from;
            this.take = take;
            this.isHasSpecifiedTuples = true;
        }
        public SelectionExpressionOnSpecifiedTuplesData(string relation, SelectionExpression selectionExpression)
        {
            this.relation = relation;
            this.selectionExpression = selectionExpression;
            this.isHasSpecifiedTuples = false;
        }
    }
}
