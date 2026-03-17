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
    public class SelectScan : UpdateScan
    {
        private Scan s;
        private SelectionCondition selectionCondition;
        private List<AbstractFuzzyProbabilisticValue> currentTuple;
        private FPRDBSchema schema;

        public SelectScan(Scan s, SelectionCondition selectionCondition, FPRDBSchema schema)
        {
            this.s = s;
            this.selectionCondition = selectionCondition;
            this.schema = schema;
        }

        public void beforeFirst() => this.s.beforeFirst();
        public bool next()
        {
            while (this.s.next())
            {
                if (this.selectionCondition.isSatisfied(s, schema))
                {
                    this.currentTuple = s.getCurrentTuple();
                    return true;
                }
            }
            this.currentTuple = null;
            return false;
        }
        public void close() { }
        public bool hasField(string fldname)
        {
            foreach (Field f in this.schema.getFields())
                if (f.getFieldName() == fldname)
                    return true;
            return false;
        }
        //public FPRDBSchema getSchema();
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
        public List<AbstractFuzzyProbabilisticValue> getCurrentTuple() => this.currentTuple;

        public void setFieldContent<T>(String fldname, FuzzyProbabilisticValue<T> content)=> throw new NotImplementedException();
        public void insert() => throw new NotImplementedException();
        public void delete()
        {
            UpdateScan us = (UpdateScan)s;
            us.delete();
        }

    }
}
