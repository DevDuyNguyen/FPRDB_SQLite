using BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainObject;
using BLL.Interfaces;

namespace BLL.DomainObject
{
    public class RelationOnFuzzySetExpressionData
    {
        private Constant leftFuzzySet;
        private CompareOperation compareOp;
        private Constant rightFuzzySet;

        public RelationOnFuzzySetExpressionData(Constant leftFuzzySet, CompareOperation compareOp, Constant rightFuzzySet)
        {
            this.leftFuzzySet = leftFuzzySet;
            this.compareOp = compareOp;
            this.rightFuzzySet = rightFuzzySet;
        }

        public Constant getLeftFuzzySet()
        {
            return leftFuzzySet;
        }

        public CompareOperation getCompareOp()
        {
            return compareOp;
        }

        public Constant getRightFuzzySet()
        {
            return rightFuzzySet;
        }
    }
}
