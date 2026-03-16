using BLL.DomainObject;
using BLL.Exceptions;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class ProductScan:Scan
    {
        private Scan s1;
        private Scan s2;
        private FPRDBSchema schema;
        private List<AbstractFuzzyProbabilisticValue> currentTuple;
        public ProductScan(Scan s1, Scan s2, FPRDBSchema schema)
        {
            this.s1 = s1;
            this.s2 = s2;
            this.schema = schema;
            s1.next();
        }
        public void beforeFirst()
        {
            s1.beforeFirst();
            s1.next();
            s2.beforeFirst();
        }
        public bool hasField(string fldname)
        {
            return s1.hasField(fldname) || s2.hasField(fldname);
        }
        public bool next()
        {
            bool hasNext;
            if (s2.next())
            {
                
                hasNext=true;
            }
            else
            {
                s2.beforeFirst();
                hasNext = s1.next() && s2.next();
            }

            if (hasNext)
            {
                this.currentTuple = new List<AbstractFuzzyProbabilisticValue>();
                foreach (AbstractFuzzyProbabilisticValue v in s1.getCurrentTuple())
                {
                    this.currentTuple.Add(v);
                }
                foreach (AbstractFuzzyProbabilisticValue v in s2.getCurrentTuple())
                {
                    this.currentTuple.Add(v);
                }
            }
            else
            {
                this.currentTuple = null;
            }
            return hasNext;
        }

        public void close() { }
        private int getFieldIndexInTuple(string fldName)
        {
            List<Field> fields = this.schema.getFields();
            for (int i = 0; i < fields.Count; ++i)
            {
                if (fields[i].getFieldName() == fldName)
                    return i;
            }
            return -1;
        }
        public FuzzyProbabilisticValue<T> getFieldContent<T>(String fldName)
        {
            int index = getFieldIndexInTuple(fldName);
            if (index == -1)
                throw new QueryDataNotExistException($"Schema doesn't have attribute {fldName}");
            var fprobValue = this.currentTuple[index];
            if (!(fprobValue is FuzzyProbabilisticValue<T>))
                throw new InvalidCastException($"Fuzzy probabilistic value of {fldName} doesn't contain fuzzy sets defined on domain of {typeof(T).Name}");
            return (FuzzyProbabilisticValue<T>)(object)fprobValue;
        }
        //public FPRDBSchema getSchema();
        public List<AbstractFuzzyProbabilisticValue> getCurrentTuple() => this.currentTuple;

    }
}
