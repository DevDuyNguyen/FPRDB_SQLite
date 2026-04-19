using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Exceptions;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace BLL.SQLProcessing
{
    public class UnionScan:Scan
    {
        private Scan s1;
        private Scan s2;
        private List<AbstractFuzzyProbabilisticValue> currentTuple;
        private ProbabilisticCombinationStrategy probCombinationStrategy;
        private FPRDBSchema schema;
        private bool isReverse = false;

        public UnionScan(Scan s1, Scan s2, ProbabilisticCombinationStrategy probCombinationStrategy, FPRDBSchema schema)
        {
            this.s1 = s1;
            this.s2 = s2;
            if (!ProbabilisticCombinationStrategyUtilities.isDisjunctionStategy(probCombinationStrategy))
                throw new InvalidDataException("Intersection must be paired with probabilistic conjunction strategy");
            this.probCombinationStrategy = probCombinationStrategy;
            this.schema = schema;
            //this.s1.next();
        }

        public void beforeFirst()
        {
            s1.beforeFirst();
            s2.beforeFirst();
            if (this.isReverse)
                this.isReverse = false;
            //if (!isReverse)
            //{
            //    s1.beforeFirst();
            //    s1.next();
            //    s2.beforeFirst();
            //}
            //else
            //{
            //    var tmp = s1;
            //    s1 = s2;
            //    s2 = s1;

            //    s1.beforeFirst();
            //    s1.next();
            //    s2.beforeFirst();

            //    this.isReverse = false;
            //}
        }
        //private int nextPair()
        //{
            
        //    if (s2.next())
        //        return 1;//s2 next
        //    else
        //    {
        //        s2.beforeFirst();
        //        bool hasNext= s2.next() && s1.next();
        //        if (hasNext)
        //            return 2;//s1 next
        //        else
        //            return 0;//run out of next

        //    }
        //}
        public bool next()
        {
            bool isMatched;
            List<string> primaryKey = this.schema.primarykey;
            AbstractFuzzyProbabilisticValue keyAttrS1;
            AbstractFuzzyProbabilisticValue keyAttrS2;
            FieldType fieldType;
            bool isSameKeyValue;

            while (!this.isReverse && s1.next())
            {
                //isMatched = false;
                while (s2.next())
                {
                    isSameKeyValue = true;
                    //check if two tuples t1 and t2 has same key value
                    foreach (string keyAttrName in primaryKey)
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

                        if (!keyAttrS1.hasSameValueList(keyAttrS2))
                        {
                            isSameKeyValue = false;
                            break;
                        }
                    }
                    if (isSameKeyValue)
                    {
                        //union on t1 and t2 to produce the next tuple for the intersection
                        this.currentTuple = this.unionOnTuples();
                        isMatched = true;
                        return true;

                    }
                }
                s2.beforeFirst();
                this.currentTuple = s1.getCurrentTuple();
                return true;
                //if(!isMatched)
                //{
                //    this.currentTuple = s1.getCurrentTuple();
                //    return true;
                //}
            }

            
            if(!this.isReverse)
            {
                this.s1.beforeFirst();
                this.s2.beforeFirst();
                this.isReverse = true;
            }
            //var tmp = this.s1;
            //this.s1 = this.s2;
            //this.s2 = tmp;

            while (this.isReverse && s2.next())
            {
                isMatched = false;
                while (s1.next())
                {
                    isSameKeyValue = true;
                    //check if two tuples t1 and t2 has same key value
                    foreach (string keyAttrName in primaryKey)
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

                        if (!keyAttrS1.hasSameValueList(keyAttrS2))
                        {
                            isSameKeyValue = false;
                            break;
                        }
                    }
                    if (isSameKeyValue)
                    {
                        isMatched = true;
                        break;
                    }
                }
                s1.beforeFirst();
                if (!isMatched)
                {
                    this.currentTuple = s2.getCurrentTuple();
                    return true;
                }
            }
            this.currentTuple = null;
            return false;

        }
        private List<AbstractFuzzyProbabilisticValue> unionOnTuples()
        {
            FieldType fieldType;
            List<AbstractFuzzyProbabilisticValue> ans = new List<AbstractFuzzyProbabilisticValue>();
            List<string> primaryKey = this.schema.getPrimarykey();

            foreach (Field field in this.schema.getFields())
            {
                fieldType = field.getFieldInfo().getType();
                if (primaryKey.Contains(field.getFieldName()))
                {
                    if (fieldType == FieldType.INT || fieldType == FieldType.distFS_INT)
                    {
                        ans.Add(this.s1.getFieldContent<int>(field.getFieldName()));
                    }
                    else if (fieldType == FieldType.FLOAT || fieldType == FieldType.distFS_FLOAT || fieldType == FieldType.contFS)
                    {
                        ans.Add(this.s1.getFieldContent<float>(field.getFieldName()));
                    }
                    else if (fieldType == FieldType.CHAR || fieldType == FieldType.VARCHAR || fieldType == FieldType.distFS_TEXT)
                    {
                        ans.Add(this.s1.getFieldContent<string>(field.getFieldName()));
                    }
                    else //if (fieldType == FieldType.BOOLEAN)
                    {
                        ans.Add(this.s1.getFieldContent<bool>(field.getFieldName()));
                    }
                }
                else
                {

                    if (fieldType == FieldType.INT || fieldType == FieldType.distFS_INT)
                    {
                        FuzzyProbabilisticValue<int> fprobValue1 = this.s1.getFieldContent<int>(field.getFieldName());
                        FuzzyProbabilisticValue<int> fprobValue2 = this.s2.getFieldContent<int>(field.getFieldName());
                        ans.Add(FProbValueCombinationStategy.disjunction<int>(fprobValue1, fprobValue2, this.probCombinationStrategy));
                    }
                    else if (fieldType == FieldType.FLOAT || fieldType == FieldType.distFS_FLOAT || fieldType == FieldType.contFS)
                    {
                        FuzzyProbabilisticValue<float> fprobValue1 = this.s1.getFieldContent<float>(field.getFieldName());
                        FuzzyProbabilisticValue<float> fprobValue2 = this.s2.getFieldContent<float>(field.getFieldName());
                        ans.Add(FProbValueCombinationStategy.disjunction<float>(fprobValue1, fprobValue2, this.probCombinationStrategy));
                    }
                    else if (fieldType == FieldType.CHAR || fieldType == FieldType.VARCHAR || fieldType == FieldType.distFS_TEXT)
                    {
                        FuzzyProbabilisticValue<string> fprobValue1 = this.s1.getFieldContent<string>(field.getFieldName());
                        FuzzyProbabilisticValue<string> fprobValue2 = this.s2.getFieldContent<string>(field.getFieldName());
                        ans.Add(FProbValueCombinationStategy.disjunction<string>(fprobValue1, fprobValue2, this.probCombinationStrategy));
                    }
                    else //if (fieldType == FieldType.BOOLEAN)
                    {
                        FuzzyProbabilisticValue<bool> fprobValue1 = this.s1.getFieldContent<bool>(field.getFieldName());
                        FuzzyProbabilisticValue<bool> fprobValue2 = this.s2.getFieldContent<bool>(field.getFieldName());
                        ans.Add(FProbValueCombinationStategy.disjunction<bool>(fprobValue1, fprobValue2, this.probCombinationStrategy));
                    }
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
