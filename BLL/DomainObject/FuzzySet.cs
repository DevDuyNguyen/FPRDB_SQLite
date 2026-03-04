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

        public abstract float getMembershipDegree(T value);

        public string getName() {
            return this.fuzzySetName;
        }
        public FieldType getFuzzysetType()
        {
            return this.fuzzySetType;
        }

    }
}
