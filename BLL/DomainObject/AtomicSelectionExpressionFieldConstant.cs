using BLL.Common;
using BLL.Enums;
using BLL.Interfaces;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class AtomicSelectionExpressionFieldConstant:SelectionExpression
    {
        public string field;
        public Constant constant;
        public CompareOperation compareOperator;
        private MetadataManager metaDataMgr;

        public AtomicSelectionExpressionFieldConstant(string field, Constant constant, CompareOperation compareOperator, MetadataManager metaMgr)
        {
            this.field = field;
            this.constant = constant;
            this.compareOperator = compareOperator;
            this.metaDataMgr = metaMgr;
        }
        public FuzzySet<T> turnConstantToFuzzySet<T>(Constant c)
        {
            Type t = typeof(T);
            if (
                (c is IntConstant && (t != typeof(int)))
                || (c is FloatConstant && (t != typeof(float)))
                || (c is StringConstant && (t != typeof(string)))
                || (c is BooleanConstant && (t != typeof(bool)))
            )
            {
                throw new InvalidCastException($"Can't turn constant of type {c.GetType().Name} to fuzzy set with defining domain of {t.Name}");
            }
            if (ConstantUltilities.isPrimitiveConstant(c))
            {
                T value = (T)c.getVal();
                List<T> valueSet = new List<T> { value };
                List<float> membershipDegreeSet = new List<float> { 1.0f };
                string fuzzySetName = value.ToString();
                FieldType fuzzSetType;
                if (c is IntConstant)
                    fuzzSetType = FieldType.distFS_INT;
                else if (c is FloatConstant)
                    fuzzSetType = FieldType.distFS_FLOAT;
                else if (c is StringConstant)
                    fuzzSetType = FieldType.distFS_TEXT;
                else
                    fuzzSetType = FieldType.BOOLEAN;
                return new DiscreteFuzzySet<T>(valueSet, membershipDegreeSet, fuzzySetName, fuzzSetType);
            }
            else
            {
                FuzzySetConstant fuzz_c = (FuzzySetConstant)c;
                FieldType fuzzSetType = this.metaDataMgr.getFuzzySetType((string)fuzz_c.getVal());
                if (FieldTypeUltilities.isContinuousFuzzySet(fuzzSetType))
                    return this.metaDataMgr.getFuzzySet<T>((string)c.getVal(), FieldType.contFS);
                else
                {
                    return this.metaDataMgr.getFuzzySet<T>((string)c.getVal(), fuzzSetType);
                }

            }

        }
        private List<float> genericCalculateProbabilisticInterpretation<T>(FuzzyProbabilisticValue<T> fprobValue, FuzzySet<T> constant) where T : IComparable<T>
        {
            List<float> intervalProb;
            List<float> ans=new List<float>();
            float probInterpretationRelationOnFuzzSet;

            //intervalProb = new List<float> { fprobValue.intervalProbLowerBoundList[0], fprobValue.intervalProbUpperBoundList[0] };
            //probInterpretationRelationOnFuzzSet = ProbabilisticInterpretationOfRelationOnFuzzySet.compare<T>(fprobValue.valueList[0], constant, this.compareOperator);
            //intervalProb[0] *= probInterpretationRelationOnFuzzSet;
            //intervalProb[1] *= probInterpretationRelationOnFuzzSet;
            //ans.Add(intervalProb[0], intervalProb[1]);

            for (int i = 0; i < fprobValue.valueList.Count; ++i)
            {
                intervalProb = new List<float> { fprobValue.intervalProbLowerBoundList[i], fprobValue.intervalProbUpperBoundList[i] };
                if (this.compareOperator == CompareOperation.ALSO)
                    probInterpretationRelationOnFuzzSet = ProbabilisticInterpretationOfRelationOnFuzzySet.also<T>(fprobValue.valueList[i], constant);
                else
                    probInterpretationRelationOnFuzzSet = ProbabilisticInterpretationOfRelationOnFuzzySet.compare<T>(fprobValue.valueList[i], constant, this.compareOperator);
                intervalProb[0] *= probInterpretationRelationOnFuzzSet;
                intervalProb[1] *= probInterpretationRelationOnFuzzSet;
                if (i == 0)
                {
                    ans.Add(intervalProb[0]);
                    ans.Add(intervalProb[1]);
                }
                else
                    ans = ProbabilisticCombinationStrategyUltilities.combine(ans[0], ans[1], intervalProb[0], intervalProb[1], ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION);

            }
            return ans;
        }
        public override List<float> calculateProbabilisticInterpretation(Scan currentTuple, FPRDBSchema schema)
        {
            FieldType fieldType = schema.getFieldByName(field).getFieldInfo().getType();
            
            if(fieldType == FieldType.INT || fieldType == FieldType.distFS_INT)
            {
                //if (!(this.constant is IntConstant))
                //    throw new InvalidCastException($"Can't compare field of type {fieldType.ToString()} with constant type {typeof(Constant).Name}");
                return genericCalculateProbabilisticInterpretation<int>(currentTuple.getFieldContent<int>(field), turnConstantToFuzzySet<int>(this.constant));
            }
            else if (fieldType == FieldType.FLOAT || fieldType == FieldType.distFS_FLOAT || fieldType==FieldType.contFS)
            {
                    
                    return genericCalculateProbabilisticInterpretation<float>(currentTuple.getFieldContent<float>(field), turnConstantToFuzzySet<float>(this.constant));
            }
            else if (fieldType == FieldType.CHAR || fieldType == FieldType.VARCHAR || fieldType == FieldType.distFS_TEXT)
            {
                    return genericCalculateProbabilisticInterpretation<string>(currentTuple.getFieldContent<string>(field), turnConstantToFuzzySet<string>(this.constant));
                }
            else //if (fieldType == FieldType.BOOLEAN)
            {
                    return genericCalculateProbabilisticInterpretation<bool>(currentTuple.getFieldContent<bool>(field), turnConstantToFuzzySet<bool>(this.constant));
            }
        }
        public override List<SelectionExpression> getAtomicSelectionExpression()
        {
            return new List<SelectionExpression> { this };
        }

    }
}
