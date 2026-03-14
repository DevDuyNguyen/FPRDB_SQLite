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
        private ContinuousFuzzySet lParrent;
        private ContinuousFuzzySet rParrent;
        private bool isNormalFuzzySet = true;

        public ContinuousFuzzySet(float leftBottom, float leftTop, float rightTop, float rightBottom, string fuzzySetName) : base(fuzzySetName, FieldType.FLOAT)
        {
            this.leftBottom = leftBottom;
            this.leftTop = leftTop;
            this.rightTop = rightTop;
            this.rightBottom = rightBottom;
        }

        private ContinuousFuzzySet(float leftBottom, float leftTop, float rightTop, float rightBottom, ContinuousFuzzySet lParrent, ContinuousFuzzySet rParrent, bool isNormalFuzzySet, string fuzzySetName) : base(fuzzySetName, FieldType.FLOAT)
        {
            this.leftBottom = leftBottom;
            this.leftTop = leftTop;
            this.rightTop = rightTop;
            this.rightBottom = rightBottom;
            this.lParrent = lParrent;
            this.rParrent = rParrent;
            this.isNormalFuzzySet = isNormalFuzzySet;
        }

        public float getLeftBottom() => this.leftBottom;
        public float getLeftTop() => this.leftTop;
        public float getRightBottom() => this.rightBottom;
        public float getRightTop() => this.rightTop;

        public override float getMembershipDegree(float value)
        {
            if (this.lParrent == this.rParrent == null)
            {
                return Math.Min(this.lParrent.getMembershipDegree(value), this.rParrent.getMembershipDegree(value));
            }
            else
            {
                if (leftBottom == leftTop)
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

        public bool isDomainOverlapedWith(ContinuousFuzzySet fs)
        {
            return !(this.rightBottom < fs.leftBottom || fs.rightBottom < this.leftBottom);
        }
        public override FuzzySet<float> StandardIntersection(FuzzySet<float> fs)
        {
            string fuzzSetName = this.getName() + "⋂" + fs.getName();
            if (fs is ContinuousFuzzySet)
            {
                ContinuousFuzzySet cfs = (ContinuousFuzzySet)(object)fs;
                if(this.leftBottom>cfs.rightBottom || this.rightBottom < cfs.leftBottom)
                {
                    //not done: the design of continuous fuzzy set is wrongful, it can accommodates its tasks
                    return new ContinuousFuzzySet(0, 0, 0, 0, this, cfs, false, fuzzSetName);
                }
                else
                {
                    float left_bot = (this.leftBottom <= cfs.getLeftBottom()) ? this.leftBottom : cfs.getLeftBottom();
                    float right_bot = (this.rightBottom >= cfs.getRightBottom()) ? this.rightBottom : cfs.getRightBottom();
                    if (this.leftTop > cfs.getRightTop() || this.rightTop <= cfs.getLeftTop())
                        return new ContinuousFuzzySet(left_bot, 0, 0, right_bot, this, cfs, false, fuzzSetName);
                    else
                        return new ContinuousFuzzySet(left_bot, 0, 0, right_bot, this, cfs, true, fuzzSetName);

                } 
                    

            }
            else
            {
                DiscreteFuzzySet<float> dfs = (DiscreteFuzzySet<float>)(object)fs;
                List<float> values = new List<float>();
                List<float> memberships = new List<float>();
                foreach (float v1 in dfs.valueSet)
                {
                    values.Add(v1);
                    memberships.Add(Math.Min(this.getMembershipDegree(v1), fs.getMembershipDegree(v1)));
                }
                return new DiscreteFuzzySet<float>(values, memberships, this.getName() + "⋂" + dfs.getName(), FieldType.distFS_FLOAT);
            }
        }
        public override bool isNormal(){
            return this.isNormalFuzzySet;
        }
        public override bool isEqualTo(FuzzySet<float> fs)
        {
            if (fs is ContinuousFuzzySet)
            {
                ContinuousFuzzySet cfs = (ContinuousFuzzySet)(object)fs;
                //not done: the design of continuous fuzzy set is wrongful, it can accommodates its tasks
                int maxDiscreteTestPoint = 200;
                float delta = (this.rightBottom - this.leftBottom) / 200.0f;
                for(float i=this.leftBottom; i<=this.rightBottom; i += delta)
                {
                    if (this.getMembershipDegree(i) != cfs.getMembershipDegree(i))
                        return false;
                }
                return true;
            }
            else
                return false;
        }
    }
}
