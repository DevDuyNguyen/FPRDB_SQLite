using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Exceptions;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class NaturalJoinScan:Scan
    {
        private Scan s1,s2;
        private List<string> commonFields;
        private List<AbstractFuzzyProbabilisticValue> currentTuple;
        private FPRDBSchema schema;
        private ProbabilisticCombinationStrategy probCombinationStrategy;
        public NaturalJoinScan(Scan s1, Scan s2, List<string> commonFields, FPRDBSchema schema, ProbabilisticCombinationStrategy probCombinationStrategy)
        {
            this.s1 = s1;
            this.s2 = s2;
            this.commonFields = commonFields;
            this.schema = schema;
            if (ProbabilisticCombinationStrategyUtilities.isConjunctionStategy(probCombinationStrategy))
                throw new InvalidDataException("Intersection must be paired with probabilistic conjunction strategy");
            this.probCombinationStrategy = probCombinationStrategy;
            this.s1.next();
        }

        public void beforeFirst()
        {
            this.s1.beforeFirst();
            this.s1.next();
            this.s2.beforeFirst();
        }
        private bool nextPair()
        {
            if (s2.next())
                return true;
            else
            {
                s2.beforeFirst();
                return s2.next() && s1.next();
            }
        }
        public bool next()
        {
            bool isValueSetEmpty;
            while (this.nextPair())
            {
                List<AbstractFuzzyProbabilisticValue> ans = this.naturalJoinOnTuples();
                isValueSetEmpty = false;
                foreach (AbstractFuzzyProbabilisticValue v in ans)
                {
                    if (v.isValueSetEmpty())
                    {
                        isValueSetEmpty = true;
                        break;
                    }
                        
                }
                if(!isValueSetEmpty)
                {
                    this.currentTuple = ans;
                    return true;
                }
            }
            return false;
            
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

        public bool hasField(string fldname)
        {
            if (this.schema.getFieldByName(fldname) == null)
                return false;
            return true;
        }
        //public FPRDBSchema getSchema();
        public List<AbstractFuzzyProbabilisticValue> getCurrentTuple() => this.currentTuple;
        private List<AbstractFuzzyProbabilisticValue> naturalJoinOnTuples()
        {
            List<Field> fields = this.schema.getFields();
            Field field;
            List<AbstractFuzzyProbabilisticValue> ans = new List<AbstractFuzzyProbabilisticValue>();
            FieldType fieldType;
            for (int i=0; i< fields.Count; ++i)
            {
                field = fields[i];
                fieldType = field.getFieldInfo().getType();
                if (this.commonFields.Contains(field.getFieldName())) {
                    if (fieldType == FieldType.INT || fieldType == FieldType.distFS_INT)
                    {
                        FuzzyProbabilisticValue<int> fprobValue1 = this.s1.getFieldContent<int>(field.getFieldName());
                        FuzzyProbabilisticValue<int> fprobValue2 = this.s2.getFieldContent<int>(field.getFieldName());
                        ans.Add(FProbValueCombinationStategy.conjunction<int>(fprobValue1, fprobValue2, this.probCombinationStrategy));
                    }
                    else if (fieldType == FieldType.FLOAT || fieldType == FieldType.distFS_FLOAT || fieldType == FieldType.contFS)
                    {
                        FuzzyProbabilisticValue<float> fprobValue1 = this.s1.getFieldContent<float>(field.getFieldName());
                        FuzzyProbabilisticValue<float> fprobValue2 = this.s2.getFieldContent<float>(field.getFieldName());
                        ans.Add(FProbValueCombinationStategy.conjunction<float>(fprobValue1, fprobValue2, this.probCombinationStrategy));
                    }
                    else if (fieldType == FieldType.CHAR || fieldType == FieldType.VARCHAR || fieldType == FieldType.distFS_TEXT)
                    {
                        FuzzyProbabilisticValue<string> fprobValue1 = this.s1.getFieldContent<string>(field.getFieldName());
                        FuzzyProbabilisticValue<string> fprobValue2 = this.s2.getFieldContent<string>(field.getFieldName());
                        ans.Add(FProbValueCombinationStategy.conjunction<string>(fprobValue1, fprobValue2, this.probCombinationStrategy));
                    }
                    else //if (fieldType == FieldType.BOOLEAN)
                    {
                        FuzzyProbabilisticValue<bool> fprobValue1 = this.s1.getFieldContent<bool>(field.getFieldName());
                        FuzzyProbabilisticValue<bool> fprobValue2 = this.s2.getFieldContent<bool>(field.getFieldName());
                        ans.Add(FProbValueCombinationStategy.conjunction<bool>(fprobValue1, fprobValue2, this.probCombinationStrategy));
                    }
                }
                else
                {
                    if (this.s1.hasField(field.getFieldName()))
                    {
                        if (fieldType == FieldType.INT || fieldType == FieldType.distFS_INT)
                            ans.Add(this.s1.getFieldContent<int>(field.getFieldName()));
                        else if (fieldType == FieldType.FLOAT || fieldType == FieldType.distFS_FLOAT || fieldType==FieldType.contFS)
                            ans.Add(this.s1.getFieldContent<float>(field.getFieldName()));
                        else if (fieldType == FieldType.CHAR || fieldType == FieldType.VARCHAR || fieldType==FieldType.distFS_TEXT)
                            ans.Add(this.s1.getFieldContent<string>(field.getFieldName()));
                        else //if (fieldType == FieldType.BOOLEAN)
                            ans.Add(this.s1.getFieldContent<int>(field.getFieldName()));
                    }
                    else
                    {
                        if (fieldType == FieldType.INT || fieldType == FieldType.distFS_INT)
                            ans.Add(this.s2.getFieldContent<int>(field.getFieldName()));
                        else if (fieldType == FieldType.FLOAT || fieldType == FieldType.distFS_FLOAT || fieldType == FieldType.contFS)
                            ans.Add(this.s2.getFieldContent<float>(field.getFieldName()));
                        else if (fieldType == FieldType.CHAR || fieldType == FieldType.VARCHAR || fieldType == FieldType.distFS_TEXT)
                            ans.Add(this.s2.getFieldContent<string>(field.getFieldName()));
                        else //if (fieldType == FieldType.BOOLEAN)
                            ans.Add(this.s2.getFieldContent<int>(field.getFieldName()));
                    }
                }

            }
            return ans;
        }

    }
}
