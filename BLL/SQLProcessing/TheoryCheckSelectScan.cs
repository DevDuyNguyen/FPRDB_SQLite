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
    public class TheoryCheckSelectScan:SelectionTheoryCheckScan
    {
        private Scan s;
        private SelectionCondition selectionCondition;
        private List<AbstractFuzzyProbabilisticValue> currentTuple;
        private FPRDBSchema schema;
        private float currentTupleLowerProb, currentTupleUpperProb;

        public TheoryCheckSelectScan(Scan s, SelectionCondition selectionCondition, FPRDBSchema schema)
        {
            this.s = s;
            this.selectionCondition = selectionCondition;
            this.schema = schema;
        }

        public void beforeFirst() => this.s.beforeFirst();
        public bool next()
        {
            //float tmpLowerProb, tmpUpperProb;
            while (this.s.next())
            {
                if (this.selectionCondition.isSatisfied(s, schema, out this.currentTupleLowerProb, out this.currentTupleUpperProb))
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
        public (float, float) getCurrentTupleProbabilisticInterpretationForSelectionExpression() => (this.currentTupleLowerProb, this.currentTupleUpperProb);

    }
}
