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
        private Constant leftFuzzySetConstant;
        private CompareOperation compareOp;
        private Constant rightFuzzySetConstant;

        public RelationOnFuzzySetExpressionData(Constant leftFuzzySetConstant, CompareOperation compareOp, Constant rightFuzzySetConstant)
        {
            this.leftFuzzySetConstant = leftFuzzySetConstant;
            this.compareOp = compareOp;
            this.rightFuzzySetConstant = rightFuzzySetConstant;
        }

        public Constant getLeftFuzzySetConstant()
        {
            return leftFuzzySetConstant;
        }

        public CompareOperation getCompareOp()
        {
            return compareOp;
        }

        public Constant getRightFuzzySetConstant()
        {
            return rightFuzzySetConstant;
        }
    }
}
