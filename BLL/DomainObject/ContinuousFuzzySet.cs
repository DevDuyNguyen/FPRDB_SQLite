using BLL.Common;
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
        private float leftBottom;//also act as left boundary for first non-0 membership degree
        private float leftTop;
        private float rightTop;
        private float rightBottom;//also act as left boundary for first non-0 membership degree
        private ContinuousFuzzySet lParrent;
        private ContinuousFuzzySet rParrent;
        private float leftOf1MembershipDegreeRange, rightOf1MembershipDegreeRange;
        private bool has1MemberShipDegreeRange;

        public ContinuousFuzzySet(float leftBottom, float leftTop, float rightTop, float rightBottom, string fuzzySetName, int oid) : base(fuzzySetName, FieldType.FLOAT, oid)
        {
            this.leftBottom = leftBottom;
            this.leftTop = leftTop;
            this.rightTop = rightTop;
            this.rightBottom = rightBottom;

            this.leftOf1MembershipDegreeRange = this.leftTop;
            this.rightOf1MembershipDegreeRange = this.rightTop;
            this.has1MemberShipDegreeRange = true;
        }

        public ContinuousFuzzySet(float leftBottom, float leftTop, float rightTop, float rightBottom, ContinuousFuzzySet lParrent, ContinuousFuzzySet rParrent, float leftOf1MembershipDegreeRange, float rightOf1MembershipDegreeRangestring,string fuzzySetName) : base(fuzzySetName, FieldType.FLOAT, -1)
        {
            this.leftBottom = leftBottom;
            this.leftTop = leftTop;
            this.rightTop = rightTop;
            this.rightBottom = rightBottom;
            this.lParrent = lParrent;
            this.rParrent = rParrent;
            this.leftOf1MembershipDegreeRange = leftOf1MembershipDegreeRange;
            this.rightOf1MembershipDegreeRange = rightOf1MembershipDegreeRange;
            this.has1MemberShipDegreeRange = true;
          
        }
        //in-memeory fuzzy set
        public ContinuousFuzzySet(float leftBottom, float leftTop, float rightTop, float rightBottom) : base(null, FieldType.FLOAT, -1)
        {
            this.leftBottom = leftBottom;
            this.leftTop = leftTop;
            this.rightTop = rightTop;
            this.rightBottom = rightBottom;

            this.leftOf1MembershipDegreeRange = this.leftTop;
            this.rightOf1MembershipDegreeRange = this.rightTop;
            this.has1MemberShipDegreeRange = true;
        }
        //constructor for continuous created by (fuzzy set and fuzzy set)
        public ContinuousFuzzySet(float leftBottom, float leftTop, float rightTop, float rightBottom, ContinuousFuzzySet lParrent, ContinuousFuzzySet rParrent, string fuzzySetName) : base(fuzzySetName, FieldType.FLOAT, -1)
        {
            this.leftBottom = leftBottom;
            this.leftTop = leftTop;
            this.rightTop = rightTop;
            this.rightBottom = rightBottom;
            this.lParrent = lParrent;
            this.has1MemberShipDegreeRange = false;
        }

        public float getLeftBottom() => this.leftBottom;
        public float getLeftTop() => this.leftTop;
        public float getRightBottom() => this.rightBottom;
        public float getRightTop() => this.rightTop;
        public float getLeftOf1MembershipDegreeRange() => this.leftOf1MembershipDegreeRange;
        public float getRightOf1MembershipDegreeRange() => this.rightOf1MembershipDegreeRange;
        public bool isHas1MemberShipDegreeRange() => this.has1MemberShipDegreeRange;

        public override float getMembershipDegree(float value)
        {
            if (this.lParrent != null && this.rParrent != null)
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
                this.getOID(),
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
                if (fs.isSubsetOf(this))
                    return fs;
                else if (this.isSubsetOf(fs))
                    return this;
                else if (fs.isEqualTo(fs))
                    return this;

                ContinuousFuzzySet cfs = (ContinuousFuzzySet)(object)fs;
                //non-overlapping universe of discourse
                if(this.leftBottom>cfs.getRightBottom() || this.rightBottom < cfs.getLeftBottom())
                {
                    //not done: the design of continuous fuzzy set is wrongful, it can accommodates its tasks
                    return new ContinuousFuzzySet(0, 0, 0, 0, this, cfs, fuzzSetName);
                }
                else
                {
                    float left_bot = (this.leftBottom <= cfs.getLeftBottom()) ? this.leftBottom : cfs.getLeftBottom();
                    float right_bot = (this.rightBottom >= cfs.getRightBottom()) ? this.rightBottom : cfs.getRightBottom();
                    float left_1degree=0, right_1degree=0;
                    bool hasOverLap1DeegreeRange = false;
                    float this_left1DegreeRange = this.getLeftOf1MembershipDegreeRange();
                    float this_rightDegreeRange = this.getRightOf1MembershipDegreeRange();
                    float cfs_left1DegreeRange = cfs.getLeftOf1MembershipDegreeRange();
                    float cfs_rightDegreeRange = cfs.getRightOf1MembershipDegreeRange();
                    if (this.isHas1MemberShipDegreeRange() && cfs.isHas1MemberShipDegreeRange())
                    {
                        if (!(this_left1DegreeRange > cfs_left1DegreeRange || cfs_left1DegreeRange > this_rightDegreeRange))
                            hasOverLap1DeegreeRange = true;
                    }
                    if (hasOverLap1DeegreeRange)
                    {
                        left_1degree = (this_left1DegreeRange <= cfs_left1DegreeRange) ? cfs_left1DegreeRange : this_left1DegreeRange;
                        right_1degree = (this_rightDegreeRange >= cfs_rightDegreeRange) ? cfs_rightDegreeRange : cfs_rightDegreeRange;
                    }
                    //non-overlapping 1 membership degree range
                    if (hasOverLap1DeegreeRange)
                        return new ContinuousFuzzySet(left_bot, 0, 0, right_bot, this, cfs, left_1degree, right_1degree, fuzzSetName);
                    return new ContinuousFuzzySet(left_bot, 0, 0, right_bot, this, cfs, fuzzSetName);
                    
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
                return new DiscreteFuzzySet<float>(values, memberships, dfs.getName(), FieldType.distFS_FLOAT, -1);
            }
        }
        public override bool isNormal(){
            return this.has1MemberShipDegreeRange;
        }
        public override bool isEqualTo(FuzzySet<float> fs)
        {
            if (fs is ContinuousFuzzySet)
            {
                ContinuousFuzzySet cfs = (ContinuousFuzzySet)(object)fs;
                //not done: the design of continuous fuzzy set is wrongful, it can accommodates its tasks
                int maxDiscreteTestPoint = 200;
                float delta = (this.rightBottom - this.leftBottom) / (float)maxDiscreteTestPoint;
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

        public override bool isSubsetOf(FuzzySet<float> fs)
        {
            if (fs is DiscreteFuzzySet<float>)
                return false;
            int maxDiscreteTestPoint = 200;
            float delta = (this.rightBottom - this.leftBottom) / (float)maxDiscreteTestPoint;

            for (float i = this.leftBottom; i <= this.rightBottom; i += delta)
            {
                if (CompareOperatorUltilities.CompareFloats(this.getMembershipDegree(i), fs.getMembershipDegree(i))==1)
                    return false;
            }
            return true;
        }
        public override bool Equal(object fs)
        {
            if (!(fs is ContinuousFuzzySet))
                return false;
            ContinuousFuzzySet con_fs = (ContinuousFuzzySet)fs;
            if (this.getName() != con_fs.getName())
                return false;
            else if (!this.isEqualTo(con_fs))
                return false;
            return true;
        }

    }
}
