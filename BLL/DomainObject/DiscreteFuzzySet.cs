using BLL.Common;
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
        //discrete fuzzy set from the underlying database
        public DiscreteFuzzySet(List<T> valueSet, List<float> membershipDegreeSet, string fuzzySetName, FieldType fuzzySetType, int oid) :base(fuzzySetName, fuzzySetType, oid)
        {
            this.valueSet = valueSet;
            this.membershipDegreeSet = membershipDegreeSet;
        }
        //in-memory discrete fuzzy set
        public DiscreteFuzzySet(List<T> valueSet, List<float> membershipDegreeSet, FieldType fuzzySetType) : base(null, fuzzySetType, -1)
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
                this.getOID(),
                this.getName(),
                this.getFuzzysetType()
                );
        }
        public override FuzzySet<T> StandardIntersection(FuzzySet<T> fs)
        {
            if (this.Equals(fs))
                return this;
            else if (this.isSubsetOf(fs))
                return this;
            else if (fs.isSubsetOf(this))
                return fs;

            List<T> values = new List<T>();
            List<float> memberships = new List<float>();
            string fuzzSetName = this.getName();
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
                //if (this.getName() != fs.getName())
                //    fuzzSetName = this.getName() + "⋂" + fs.getName();
                fuzzSetName = this.getName() + "⋂" + fs.getName();
            }
            
            return new DiscreteFuzzySet<T>(values, memberships, fuzzSetName, this.getFuzzysetType(), -1);

        }
        public override bool isNormal()
        {
            //float maxHeight = this.membershipDegreeSet[0];
            //for(int i=1; i < this.membershipDegreeSet.Count; ++i)
            //{
            //    if (this.membershipDegreeSet[i] > maxHeight)
            //    {
            //        maxHeight = this.membershipDegreeSet[i];
            //    }
            //}
            //return maxHeight;
            for (int i = 0; i < this.membershipDegreeSet.Count; ++i)
            {
                if (this.membershipDegreeSet[i]==1)
                {
                    return true;
                }
            }
            return false;
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
        public override bool isSubsetOf(FuzzySet<T> fs) {
            float tmp_degree;
            for(int i=0; i<this.valueSet.Count; ++i)
            {
                //var t = fs.getMembershipDegree((T)Convert.ChangeType(this.valueSet[i], typeof(T)));
                tmp_degree = fs.getMembershipDegree((T)Convert.ChangeType(this.valueSet[i], typeof(T)));
                if (CompareOperatorUltilities.CompareFloats(this.membershipDegreeSet[i], tmp_degree)==1)
                    return false;
            }
            return true;
        }

        public override bool Equal(object fs)
        {
            if (!(fs is DiscreteFuzzySet<T>))
                return false;
            DiscreteFuzzySet<T> dist_fs = (DiscreteFuzzySet<T>)fs;
            if (this.getName() != dist_fs.getName())
                return false;
            else if (!this.isEqualTo(dist_fs))
                return false;
            return true;
        }

    }
}
