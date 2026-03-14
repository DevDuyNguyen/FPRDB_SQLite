using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class ContinuousFuzzySet : FuzzySet<float>
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
        public float getLeftBottom() => this.leftBottom;
        public float getLeftTop() => this.leftTop;
        public float getRightBottom() => this.rightBottom;
        public float getRightTop() => this.rightTop;

        public override float getMembershipDegree(float value)
        {
            if (leftBottom == leftTop && leftTop == rightBottom && rightBottom == rightTop)
            {
                if (value == leftBottom)
                    return 1;
                else
                    return 0;
            }
            else if (leftBottom == leftTop)
            {
                if (value == leftBottom)
                    return 1;
                else if (value < leftBottom || value > rightBottom)
                    return 0;
                else if (value >= leftTop && value <= rightTop)
                    return 1;
                else //value> rightTop && value < rightBottom
                {
                    double m = -1.0 / (rightBottom - rightTop);
                    double b = -m * rightBottom;
                    return Convert.ToSingle(m * value + b);
                }
            }
            else if (leftTop == rightTop)
            {
                if (value < leftBottom || value > rightBottom)
                    return 0;
                else if (value == leftTop)
                    return 1;
                else if (value >= leftBottom && value <= leftTop)
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
            else if (rightTop == rightBottom)
            {
                if (value == rightTop)
                    return 1;
                else if (value < leftBottom || value > rightBottom)
                    return 0;
                else if (value >= leftTop && value <= rightTop)
                    return 1;
                else //if (value >= leftBottom && value <= leftTop)
                {
                    double m = 1.0 / (leftTop - leftBottom);
                    double b = -m * leftBottom;
                    return Convert.ToSingle(m * value + b);
                }

            }
            else
            {
                if (value < leftBottom || value > rightBottom)
                    return 0;
                else if (value >= leftTop && value <= rightTop)
                    return 1;
                else if (value >= leftBottom && value <= leftTop)
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
            int noPoints = 15;
            float interval = (this.rightBottom - this.leftBottom) / noPoints;
            List<float> valueSet = new List<float>();
            List<float> membershipDegrees = new List<float>();
            float currentValue = this.leftBottom;
            valueSet.Add(currentValue);
            membershipDegrees.Add(this.getMembershipDegree(currentValue));
            for (int i = 2; i <= noPoints; ++i)
            {
                currentValue += interval;
                valueSet.Add(currentValue);
                membershipDegrees.Add(this.getMembershipDegree(currentValue));
            }
            return new DiscreteFuzzySet<float>(valueSet, membershipDegrees, "", FieldType.distFS_FLOAT);
        }
        public bool isDomainOverlapedWith(ContinuousFuzzySet fs)
        {
            return !(this.rightBottom < fs.leftBottom || fs.rightBottom < this.leftBottom);
        }
        public override FuzzySet<float> StandardIntersection(FuzzySet<float> fs) => throw new NotImplementedException();
        public override float getHeight() => throw new NotImplementedException();
        public override bool isEqualTo(FuzzySet<float> fs) => throw new NotImplementedException();
    }
}
