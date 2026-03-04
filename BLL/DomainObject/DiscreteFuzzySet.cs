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
        private List<T> valueSet;
        private List<float> membershipDegreeSet;

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

    }
}
