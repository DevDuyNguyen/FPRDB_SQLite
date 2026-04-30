using BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class RelationOnFuzzySetExpressionData
    {
        private string leftFuzzySet;
        private CompareOperation compareOp;
        private string rightFuzzySet;

        public RelationOnFuzzySetExpressionData(string leftFuzzySet, CompareOperation compareOp, string rightFuzzySet)
        {
            this.leftFuzzySet = leftFuzzySet;
            this.compareOp = compareOp;
            this.rightFuzzySet = rightFuzzySet;
        }

        public string getLeftFuzzySet()
        {
            return leftFuzzySet;
        }

        public CompareOperation getCompareOp()
        {
            return compareOp;
        }

        public string getRightFuzzySet()
        {
            return rightFuzzySet;
        }
    }
}
