using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class ContinuousFuzzySet:FuzzySet<float>
    {
        private float leftBottom;
        private float leftTop;
        private float rightTop;
        private float rightBottom;

        public ContinuousFuzzySet(float leftBottom, float leftTop, float rightTop, float rightBottom, string fuzzySetName) : base(fuzzySetName, FieldType.FLOAT)
        {
            this.leftBottom = leftBottom;
            this.leftTop = leftTop;
            this.rightTop = rightTop;
            this.rightBottom = rightBottom;
        }

        public override float getMembershipDegree(float value)
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

        public override FuzzySetDTO toDTO()
        {
            return new ContinuousFuzzySetDTO(
                this.leftBottom,
                this.leftTop,
                this.rightTop,
                this.rightBottom,
                this.getName()
                );
        }
        public override DiscreteFuzzySet<float> ToDiscreteFuzzySet()
        {
            int noPoints = 100;
            float interval = (this.rightBottom - this.leftBottom)/100;
            List<float> valueSet = new List<float>();
            List<float> membershipDegrees = new List<float>();
            float currentValue = this.rightBottom;
            valueSet.Add(currentValue);
            membershipDegrees.Add(0);
            for(int i=2; i<=100; ++i)
            {
                currentValue += interval;
                valueSet.Add(currentValue);
                membershipDegrees.Add(this.getMembershipDegree(currentValue));
            }
            return new DiscreteFuzzySet<float>(valueSet, membershipDegrees, "", FieldType.distFS_FLOAT);
        }

    }
}
