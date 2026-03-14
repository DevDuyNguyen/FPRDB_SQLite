using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class DiscreteFuzzySet<T>:FuzzySet<T>
    {
        public List<T> valueSet;
        public List<float> membershipDegreeSet;

        public DiscreteFuzzySet(List<T> valueSet, List<float> membershipDegreeSet, string fuzzySetName, FieldType fuzzySetType) :base(fuzzySetName, fuzzySetType)
        {
            this.valueSet = valueSet;
            this.membershipDegreeSet = membershipDegreeSet;
        }

        public override float getMembershipDegree(T value)
        {
            int index = this.valueSet.IndexOf(value);
            if (index == -1)
                return 0;
            else
                return this.membershipDegreeSet[index];
        }
        public override FuzzySetDTO toDTO()
        {
            return new DiscreteFuzzySetDTO<T>(
                this.valueSet,
                this.membershipDegreeSet,
                this.getName(),
                this.getFuzzysetType()
                );
        }
        public override FuzzySet<T> StandardIntersection(FuzzySet<T> fs)
        {
            List<T> values = new List<T>();
            List<float> memberships = new List<float>();
            foreach(T v1 in this.valueSet)
            {
                values.Add(v1);
                memberships.Add(Math.Min(this.getMembershipDegree(v1), fs.getMembershipDegree(v1)));
            }
            if(fs is DiscreteFuzzySet<T>)
            {
                DiscreteFuzzySet<T> dfs = (DiscreteFuzzySet<T>)(object)fs;
                foreach (T v2 in dfs.valueSet)
                {
                    if (!values.Contains(v2))
                    {
                        values.Add(v2);
                        memberships.Add(Math.Min(this.getMembershipDegree(v2), fs.getMembershipDegree(v2)));
                    }   
                    
                }
            }
            //else
            //{
            //    ContinuousFuzzySet cfs = (ContinuousFuzzySet)(object)fs;


            //}
            return new DiscreteFuzzySet<T>(values, memberships, this.getName()+ "⋂"+ fs.getName(), this.getFuzzysetType());

        }
        public override float getHeight()
        {
            float maxHeight = this.membershipDegreeSet[0];
            for(int i=1; i < this.membershipDegreeSet.Count; ++i)
            {
                if (this.membershipDegreeSet[i] > maxHeight)
                {
                    maxHeight = this.membershipDegreeSet[i];
                }
            }
            return maxHeight;
        }
        public override bool isEqualTo(FuzzySet<T> fs)
        {
            if (fs is ContinuousFuzzySet)
                return false;
            else
            {
                DiscreteFuzzySet<T> dfs = (DiscreteFuzzySet<T>)(object)fs;
                if (this.valueSet.Count != dfs.valueSet.Count)
                    return false;
                for(int i=0; i < this.valueSet.Count; ++i)
                {
                    if (this.membershipDegreeSet[i] != dfs.getMembershipDegree(this.valueSet[i]))
                        return false;
                    if (dfs.membershipDegreeSet[i] != this.getMembershipDegree(dfs.valueSet[i]))
                        return false;
                }
                return true;

            }
            
        }
    }
}
