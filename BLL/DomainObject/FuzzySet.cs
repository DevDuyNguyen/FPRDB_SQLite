using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public abstract class FuzzySet<T>:BaseFuzzySet
    {
        private string fuzzySetName;
        private FieldType fuzzySetType;
        private int oid;

        protected FuzzySet(string fuzzySetName, FieldType fuzzySetType, int oid)
        {
            this.fuzzySetName = fuzzySetName;
            this.fuzzySetType = fuzzySetType;
            this.oid = oid;
        }

        public abstract float getMembershipDegree(T value);
        public int getOID() => this.oid;
        public string getName() {
            return this.fuzzySetName;
        }
        public FieldType getFuzzysetType()
        {
            return this.fuzzySetType;
        }
        public abstract FuzzySetDTO toDTO();
        public abstract FuzzySet<T> StandardIntersection(FuzzySet<T> fs);
        public abstract bool isNormal();
        public abstract bool isEqualTo(FuzzySet<T> fs);
        public abstract bool isSubsetOf(FuzzySet<T> fs);

        public abstract bool Equal(object fs);

    }
}
