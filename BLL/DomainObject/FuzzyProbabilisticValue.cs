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
        public FieldType domain;
        public List<FuzzySet<T>> valueList;
        public List<float> intervalProbLowerBoundList;
        public List<float> intervalProbUpperBoundList;

        public FuzzyProbabilisticValue(FieldType domain, List<FuzzySet<T>> valueList, List<float> intervalProbLowerBoundList, List<float> intervalProbUpperBoundList)
        {
            this.domain = domain;
            this.valueList = valueList;
            this.intervalProbLowerBoundList = intervalProbLowerBoundList;
            this.intervalProbUpperBoundList = intervalProbUpperBoundList;
        }

        //public FieldType getDomain() => this.domain;
        //public FuzzySet<T> getVal(int index) => this.valueList[index];
        //public float getLowerBoundProb(int index) => this.intervalProbLowerBoundList[index];
        //public float getUpperBoundProb(int index) => this.intervalProbUpperBoundList[index];


    }
}
