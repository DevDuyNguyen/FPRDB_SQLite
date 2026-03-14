using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public abstract class FuzzySet<T>
    {
        private string fuzzySetName;
        private FieldType fuzzySetType;

        protected FuzzySet(string fuzzySetName, FieldType fuzzySetType)
        {
            this.fuzzySetName = fuzzySetName;
            this.fuzzySetType = fuzzySetType;
        }

        public abstract float getMembershipDegree(T value);

        public string getName() {
            return this.fuzzySetName;
        }
        public FieldType getFuzzysetType()
        {
            return this.fuzzySetType;
        }
        public abstract FuzzySetDTO toDTO();
        public abstract FuzzySet<T> StandardIntersection(FuzzySet<T> fs);
        public abstract float getHeight();
        public abstract bool isEqualTo(FuzzySet<T> fs);

    }
}
