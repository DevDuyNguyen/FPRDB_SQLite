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
    public class ProjectScan:Scan
    {
        private Scan s;
        private FPRDBSchema selectFields;
        private List<AbstractFuzzyProbabilisticValue> currentTuple;

        public ProjectScan(Scan s, FPRDBSchema selectFields)
        {
            this.s = s;
            this.selectFields = selectFields;
        }
        public void beforeFirst() => this.s.beforeFirst();
        public bool next()
        {
            if (this.s.next())
            {
                List<AbstractFuzzyProbabilisticValue> tmp1 = new List<AbstractFuzzyProbabilisticValue>();
                foreach(Field field in this.selectFields.getFields())
                {
                    FieldType fieldType = field.getFieldInfo().getType();
                    if (fieldType == FieldType.INT || fieldType == FieldType.distFS_INT)
                        tmp1.Add(this.s.getFieldContent<int>(field.getFieldName()));
                    else if (fieldType == FieldType.FLOAT || fieldType == FieldType.distFS_FLOAT || fieldType == FieldType.contFS)
                        tmp1.Add(this.s.getFieldContent<float>(field.getFieldName()));
                    else if (fieldType == FieldType.VARCHAR || fieldType == FieldType.CHAR || fieldType == FieldType.distFS_TEXT)
                        tmp1.Add(this.s.getFieldContent<string>(field.getFieldName()));
                    else if (fieldType == FieldType.BOOLEAN)
                        tmp1.Add(this.s.getFieldContent<string>(field.getFieldName()));
                }
                this.currentTuple = tmp1;
                return true;
            }
            else
            {
                this.currentTuple = null;
                return false;
            }
        }
        public void close() { }
        private int getFieldIndexInTuple(string fldName)
        {
            List<Field> fields = this.selectFields.getFields();
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
        public bool hasField(string fldname)
        {
            foreach(Field field in this.selectFields.getFields())
            {
                if (field.getFieldName() == fldname)
                    return true;
            }
            return false;
        }
        public List<AbstractFuzzyProbabilisticValue> getCurrentTuple() => this.currentTuple;
    }
}
