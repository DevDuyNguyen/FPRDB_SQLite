using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class FuzzyProbabilisticValue<T>:AbstractFuzzyProbabilisticValue
    {
        public FieldType domain;
        public List<FuzzySet<T>> valueList;
        public List<float> intervalProbLowerBoundList;
        public List<float> intervalProbUpperBoundList;

        public FuzzyProbabilisticValue(FieldType domain, List<FuzzySet<T>> valueList, List<float> intervalProbLowerBoundList, List<float> intervalProbUpperBoundList):base()
        {
            this.domain = domain;
            this.valueList = valueList;
            this.intervalProbLowerBoundList = intervalProbLowerBoundList;
            this.intervalProbUpperBoundList = intervalProbUpperBoundList;
        }
        public override bool isValueSetEmpty()
        {
            return this.valueList.Count == 0;
        }
        public override bool hasSameKeyValue(AbstractFuzzyProbabilisticValue v)
        {
            if (v is FuzzyProbabilisticValue<T>)
            {
                bool isSameKeyValue = true;
                FuzzyProbabilisticValue<T> T_v = (FuzzyProbabilisticValue<T>)(object)v;
                isSameKeyValue = this.valueList.Count == T_v.valueList.Count;
                for(int i=0; i < this.valueList.Count; ++i)
                {
                    if (!this.valueList[i].isEqualTo(T_v.valueList[i]))
                    {
                        isSameKeyValue = false;
                        break;
                    }
                }
                return isSameKeyValue;
                
            }
            else
                return false;
        }

        //public FieldType getDomain() => this.domain;
        //public FuzzySet<T> getVal(int index) => this.valueList[index];
        //public float getLowerBoundProb(int index) => this.intervalProbLowerBoundList[index];
        //public float getUpperBoundProb(int index) => this.intervalProbUpperBoundList[index];


    }
}
