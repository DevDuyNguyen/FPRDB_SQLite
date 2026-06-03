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
    public class InMemoryScan:Scan
    {
        private FPRDBSchema schema;
        private int currentTupleIndex=-1;//index start at 0
        private List<AbstractFuzzyProbabilisticValue> currentTuple;
        private List<List<AbstractFuzzyProbabilisticValue>> tuples;

        public InMemoryScan(Plan p)
        {
            Scan s = p.open();
            this.schema = p.getSchema();
            this.tuples = new List<List<AbstractFuzzyProbabilisticValue>>();

            while (s.next())
            {
                this.tuples.Add(s.getCurrentTuple());
            }
        }

        public FPRDBSchema getFPRDBSchema() => this.schema;
        public void beforeFirst()
        {
            this.currentTupleIndex = 0;
        }
        public bool hasField(string fldname)=> this.schema.hasField(fldname);
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
        public bool next()
        {
            if (this.currentTupleIndex < this.tuples.Count - 1)
            {
                this.currentTupleIndex++;
                this.currentTuple = this.tuples[this.currentTupleIndex];
                return true;
            }
            else
            {
                this.currentTuple = null;
                return false;
            }
        }
        public List<AbstractFuzzyProbabilisticValue> getCurrentTuple() => this.currentTuple;
        public void close() => throw new NotImplementedException();

    }
}
