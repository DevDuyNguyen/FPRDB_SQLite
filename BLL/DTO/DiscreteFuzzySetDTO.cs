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
        public DiscreteFuzzySetDTO(List<TDomain> valueSet, List<float> membershipDegreeSet,
            int oid, string fuzzySetName, FieldType fuzzySetType) : base(oid, fuzzySetName, fuzzySetType)
        {
            this.valueSet = valueSet;
            this.membershipDegreeSet = membershipDegreeSet;
        }
        

        public override bool isValid()
        {
            if (this.valueSet.Count != this.membershipDegreeSet.Count)
                throw new InvalidDataException($"Fuzzy set {this.fuzzySetName} has the number of values x ({this.valueSet.Count}) that isn't equal to the number of membership degree ({this.membershipDegreeSet.Count})");
            foreach(float value in this.membershipDegreeSet)
            {
                if (value < 0 || value > 1)
                    throw new InvalidDataException($"Membership degree {value} is out of range [0,1]");
            }
            return true;
        }

    }
}
