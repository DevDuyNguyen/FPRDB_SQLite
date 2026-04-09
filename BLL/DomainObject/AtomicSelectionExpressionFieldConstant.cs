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
        private List<float> genericCalculateProbabilisticInterpretation<T>(FuzzyProbabilisticValue<T> fprobValue, FuzzySet<T> constant) where T : IComparable<T>
        {
            List<float> tmp_IntervalProbability;
            List<float> resultIntervalProbability=new List<float>();
            float probInterpretationRelationOnFuzzSet;

            tmp_IntervalProbability = new List<float> { fprobValue.intervalProbLowerBoundList[0], fprobValue.intervalProbUpperBoundList[0] };
            probInterpretationRelationOnFuzzSet = ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<T>(fprobValue.valueList[0], constant, this.compareOperator);
            tmp_IntervalProbability[0] *= probInterpretationRelationOnFuzzSet;
            tmp_IntervalProbability[1] *= probInterpretationRelationOnFuzzSet;
            resultIntervalProbability.Add(tmp_IntervalProbability[0]);
            resultIntervalProbability.Add(tmp_IntervalProbability[1]);

            for (int i = 1; i < fprobValue.valueList.Count; ++i)
            {
                tmp_IntervalProbability = new List<float> { fprobValue.intervalProbLowerBoundList[i], fprobValue.intervalProbUpperBoundList[i] };
                probInterpretationRelationOnFuzzSet = ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<T>(fprobValue.valueList[i], constant, this.compareOperator);
                tmp_IntervalProbability[0] *= probInterpretationRelationOnFuzzSet;
                tmp_IntervalProbability[1] *= probInterpretationRelationOnFuzzSet;
                //if (i == 0)
                //{
                //    ans.Add(intervalProb[0]);
                //    ans.Add(intervalProb[1]);
                //}
                //else
                //    ans = ProbabilisticCombinationStrategyUltilities.combine(ans[0], ans[1], intervalProb[0], intervalProb[1], ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION);
                resultIntervalProbability = ProbabilisticCombinationStrategyUtilities.combine(resultIntervalProbability[0], resultIntervalProbability[1], tmp_IntervalProbability[0], tmp_IntervalProbability[1], ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION);

            }
            return resultIntervalProbability;
        }
        //not done: mocking for private
        public List<float> genericCalculateProbabilisticInterpretation<T1, T2>(FuzzyProbabilisticValue<T1> fprobValue, FuzzySet<T2> constant)
            where T1:IComparable<T1>
            where T2 : IComparable<T2>
        {
            //if (!(typeof(T1) == typeof(int) && typeof(T2) == typeof(float)) && !(typeof(T1) == typeof(float) && typeof(T2) == typeof(int)))
            //    throw new InvalidOperationException($"{typeof(T1).Name} and {typeof(T2).Name} aren't compatible for probabilistic interpretation of selection expression");
            List<float> tmp_IntervalProbability;
            List<float> resultIntervalProbability = new List<float>();
            float probInterpretationRelationOnFuzzSet;

            tmp_IntervalProbability = new List<float> { fprobValue.intervalProbLowerBoundList[0], fprobValue.intervalProbUpperBoundList[0] };
            probInterpretationRelationOnFuzzSet = ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<T1, T2>(fprobValue.valueList[0], constant, this.compareOperator);
            tmp_IntervalProbability[0] *= probInterpretationRelationOnFuzzSet;
            tmp_IntervalProbability[1] *= probInterpretationRelationOnFuzzSet;
            resultIntervalProbability.Add(tmp_IntervalProbability[0]);
            resultIntervalProbability.Add(tmp_IntervalProbability[1]);

            for (int i = 1; i < fprobValue.valueList.Count; ++i)
            {
                tmp_IntervalProbability = new List<float> { fprobValue.intervalProbLowerBoundList[i], fprobValue.intervalProbUpperBoundList[i] };
                probInterpretationRelationOnFuzzSet = ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<T1, T2>(fprobValue.valueList[i], constant, this.compareOperator);
                tmp_IntervalProbability[0] *= probInterpretationRelationOnFuzzSet;
                tmp_IntervalProbability[1] *= probInterpretationRelationOnFuzzSet;
                //if (i == 0)
                //{
                //    ans.Add(intervalProb[0]);
                //    ans.Add(intervalProb[1]);
                //}
                //else
                //    ans = ProbabilisticCombinationStrategyUltilities.combine(ans[0], ans[1], intervalProb[0], intervalProb[1], ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION);
                resultIntervalProbability = ProbabilisticCombinationStrategyUtilities.combine(resultIntervalProbability[0], resultIntervalProbability[1], tmp_IntervalProbability[0], tmp_IntervalProbability[1], ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION);

            }
            return resultIntervalProbability;
        }
        public override List<float> calculateProbabilisticInterpretation(Scan currentTuple, FPRDBSchema schema)
        {
            FieldType fieldType = schema.getFieldByName(field).getFieldInfo().getType();
            Type constantType = this.constant.GetType();
            
            if(fieldType == FieldType.INT || fieldType == FieldType.distFS_INT)
            {
                if (!(this.constant is IntConstant || this.constant is FuzzySetConstant || this.constant is FloatConstant))
                    throw new InvalidCastException($"Can't compare field of type {fieldType.ToString()} with constant type {constantType.Name}");
                if(this.constant is FloatConstant)
                    return genericCalculateProbabilisticInterpretation<int, float>(currentTuple.getFieldContent<int>(field), FuzzySetUltilities.turnConstantToFuzzySet<float>(this.constant, this.metaDataMgr));
                return genericCalculateProbabilisticInterpretation<int>(currentTuple.getFieldContent<int>(field),FuzzySetUltilities.turnConstantToFuzzySet<int>(this.constant, this.metaDataMgr));
            }
            else if (fieldType == FieldType.FLOAT || fieldType == FieldType.distFS_FLOAT || fieldType==FieldType.contFS)
            {
                if(!(this.constant is FloatConstant || this.constant is IntConstant || this.constant is FuzzySetConstant))
                    throw new InvalidCastException($"Can't compare field of type {fieldType.ToString()} with constant type {constantType.Name}"); 
                return genericCalculateProbabilisticInterpretation<float>(currentTuple.getFieldContent<float>(field), FuzzySetUltilities.turnConstantToFuzzySet<float>(this.constant, this.metaDataMgr));
            }
            else if (fieldType == FieldType.CHAR || fieldType == FieldType.VARCHAR || fieldType == FieldType.distFS_TEXT)
            {
                if (!(this.constant is StringConstant || this.constant is FuzzySetConstant))
                    throw new InvalidCastException($"Can't compare field of type {fieldType.ToString()} with constant type {constantType.Name}");
                return genericCalculateProbabilisticInterpretation<string>(currentTuple.getFieldContent<string>(field), FuzzySetUltilities.turnConstantToFuzzySet<string>(this.constant, this.metaDataMgr));
            }
            else //if (fieldType == FieldType.BOOLEAN)
            {
                if (!(this.constant is BooleanConstant || this.constant is FuzzySetConstant))
                    throw new InvalidCastException($"Can't compare field of type {fieldType.ToString()} with constant type {constantType.Name}");
                return genericCalculateProbabilisticInterpretation<bool>(currentTuple.getFieldContent<bool>(field), FuzzySetUltilities.turnConstantToFuzzySet<bool>(this.constant, this.metaDataMgr));
            }
        }
        public override List<SelectionExpression> getAtomicSelectionExpression()
        {
            return new List<SelectionExpression> { this };
        }
        public override List<string> getMentionedAttributes() => new List<string> { this.field };


    }
}
