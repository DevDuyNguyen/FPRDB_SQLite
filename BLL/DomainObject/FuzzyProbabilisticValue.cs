using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class FuzzyProbabilisticValue<T>
    {
        private FieldType domain;
        private List<FuzzySet<T>> valueList;
        private List<float> intervalProbLowerBoundList;
        private List<float> intervalProbUpperBoundList;

        public FieldType getDomain() => this.domain;
        public FuzzySet<T> getVal(int index) => this.valueList[index];
        public float getLowerBoundProb(int index) => this.intervalProbLowerBoundList[index];
        public float getUpperBoundProb(int index) => this.intervalProbUpperBoundList[index];


    }
}
