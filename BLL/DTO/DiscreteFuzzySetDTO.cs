using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class DiscreteFuzzySetDTO<TDomain>:FuzzySetDTO
    {
        public List<TDomain> valueSet;
        public List<float> membershipDegreeSet;

        public DiscreteFuzzySetDTO(List<TDomain> valueSet, List<float> membershipDegreeSet,
            string fuzzySetName, FieldType fuzzySetType):base(fuzzySetName, fuzzySetType)
        {
            this.valueSet = valueSet;
            this.membershipDegreeSet = membershipDegreeSet;
        }

        public override bool isValid()
        {
            foreach(float value in this.membershipDegreeSet)
            {
                if (value < 0 || value > 1)
                    return false;
            }
            return true;
        }

    }
}
