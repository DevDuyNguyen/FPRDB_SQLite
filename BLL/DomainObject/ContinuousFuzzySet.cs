using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class ContinuousFuzzySet:FuzzySet<double>
    {
        private float leftBottom;
        private float leftTop;
        private float rightTop;
        private float rightBottom;

        public ContinuousFuzzySet(float leftBottom, float leftTop, float rightTop, float rightBottom)
        {
            this.leftBottom = leftBottom;
            this.leftTop = leftTop;
            this.rightTop = rightTop;
            this.rightBottom = rightBottom;
        }

        public override float getMembershipDegree(double value)
        {
            if (value <= leftBottom || value >= rightBottom)
                return 0;
            else if (value >= leftTop && value <= rightTop)
                return 1;
            else if(value> leftBottom && value < leftTop)
            {
                double m = 1.0 / (leftTop - leftBottom);
                double b = -m * leftBottom;
                return Convert.ToSingle(m * value + b);
            }
            else //value> rightTop && value < rightBottom
            {
                double m = -1.0 / (rightBottom - rightTop);
                double b = -m * rightBottom;
                return Convert.ToSingle(m * value + b);
            }
        }

    }
}
