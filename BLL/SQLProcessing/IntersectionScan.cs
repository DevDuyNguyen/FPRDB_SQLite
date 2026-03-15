using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Exceptions;
using BLL.Interfaces;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class IntersectionScan:Scan
    {
        private Scan s1;
        private Scan s2;
        private List<AbstractFuzzyProbabilisticValue> currentTuple;
        private ProbabilisticCombinationStrategy probCombinationStrategy;
        private FPRDBSchema schema;

        public IntersectionScan(Scan s1, Scan s2, ProbabilisticCombinationStrategy probCombinationStrategy, FPRDBSchema schema)
        {
            this.s1 = s1;
            this.s2 = s2;
            if (!ProbabilisticCombinationStrategyUtilities.isConjunctionStategy(probCombinationStrategy))
                throw new InvalidDataException("Intersection must be paired with probabilistic conjunction strategy");
            this.probCombinationStrategy = probCombinationStrategy;
            this.schema = schema;
            this.s1.next();
        }
        

        public void beforeFirst()
        {
            s1.beforeFirst();
            s1.next();
            s2.beforeFirst();
        }
        public bool nextPair()
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
            List<string> primaryKey = this.schema.primarykey;
            AbstractFuzzyProbabilisticValue keyAttrS1;
            AbstractFuzzyProbabilisticValue keyAttrS2;
            FieldType fieldType;
            bool isSameKeyValue;
            while (nextPair())
            {
                isSameKeyValue = true;
                //check if two tuples t1 and t2 has same key value
                foreach(string keyAttrName in primaryKey)
                {
                    fieldType = this.schema.getFieldByName(keyAttrName).getFieldInfo().getType();
                    if (fieldType == FieldType.INT || fieldType == FieldType.distFS_INT)
                    {
                        keyAttrS1 = this.s1.getFieldContent<int>(keyAttrName);
                        keyAttrS2 = this.s2.getFieldContent<int>(keyAttrName);
                    }
                    else if (fieldType == FieldType.FLOAT || fieldType == FieldType.distFS_FLOAT || fieldType == FieldType.contFS)
                    {
                        keyAttrS1 = this.s1.getFieldContent<float>(keyAttrName);
                        keyAttrS2 = this.s2.getFieldContent<float>(keyAttrName);
                    }
                    else if (fieldType == FieldType.CHAR || fieldType == FieldType.VARCHAR || fieldType == FieldType.distFS_TEXT)
                    {
                        keyAttrS1 = this.s1.getFieldContent<string>(keyAttrName);
                        keyAttrS2 = this.s2.getFieldContent<string>(keyAttrName);
                    }
                    else //if (fieldType == FieldType.BOOLEAN)
                    {
                        keyAttrS1 = this.s1.getFieldContent<bool>(keyAttrName);
                        keyAttrS2 = this.s2.getFieldContent<bool>(keyAttrName);
                    }

                    if (!keyAttrS1.hasSameKeyValue(keyAttrS2))
                    {
                        isSameKeyValue = false;
                        break;
                    }
                }
                if (isSameKeyValue)
                {
                    //intersection on t1 and t2 to produce the next tuple for the intersection
                    List<AbstractFuzzyProbabilisticValue> ans = intersectionOnFuzzySet();
                    bool isValueSetEmpty = false;
                    //check if any attribute has empty probabilistic value
                    foreach (AbstractFuzzyProbabilisticValue v in ans)
                    {
                        if (v.isValueSetEmpty())
                        {
                            isValueSetEmpty = true;
                            break;
                        }

                    }
                    if (!isValueSetEmpty)
                    {
                        this.currentTuple = ans;
                        return true;
                    }

                }

            }
            this.currentTuple = null;
            return false;
        }
        private List<AbstractFuzzyProbabilisticValue> intersectionOnFuzzySet()
        {
            FieldType fieldType;
            List<AbstractFuzzyProbabilisticValue> ans = new List<AbstractFuzzyProbabilisticValue>();
            
            foreach (Field field in this.schema.getFields())
            {
                fieldType = field.getFieldInfo().getType();
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
            return ans;
            
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
            return this.schema.getFieldByName(fldname) != null;
        }
        //public FPRDBSchema getSchema();
        public List<AbstractFuzzyProbabilisticValue> getCurrentTuple() => this.currentTuple;
    }
}
