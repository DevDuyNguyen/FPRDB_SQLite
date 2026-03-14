using BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Common;
using BLL.SQLProcessing;

namespace BLL.DomainObject
{
    public class AtomicSelectionExpressionFieldField:SelectionExpression
    {
        public string lField;
        public string rField;
        public ProbabilisticCombinationStrategy probCombinationStrategy;
public AtomicSelectionExpressionFieldField(string lField, string rField, ProbabilisticCombinationStrategy probCombinationStrategy)
        {
            this.lField = lField;
            this.rField = rField;
            this.probCombinationStrategy = probCombinationStrategy;
        }
        private List<float> genericCalculateProbabilisticInterpretation<T>(FuzzyProbabilisticValue<T> v1, FuzzyProbabilisticValue<T> v2) where T : IComparable<T>
        {

            List<float> result_IntervalProbabilistic = new List<float>();
            List<float> tmp_IntervalProbabilistic;
            for (int i = 0; i < v1.valueList.Count; ++i)
            {
                tmp_IntervalProbabilistic = new List<float>(2);
                for (int j = 0; j < v2.valueList.Count; ++j)
                {
                    List<float> v1_v2_interval_prob_conjunction = ProbabilisticCombinationStrategyUltilities.combine(v1.intervalProbLowerBoundList[i], v1.intervalProbUpperBoundList[i], v2.intervalProbLowerBoundList[j], v2.intervalProbUpperBoundList[j], this.probCombinationStrategy);
                    float probabilisticIntepretationforRelation = ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet<T>(v1.valueList[i], v2.valueList[j], CompareOperation.EQUAL);
                    v1_v2_interval_prob_conjunction[0] = v1_v2_interval_prob_conjunction[0] * probabilisticIntepretationforRelation;
                    v1_v2_interval_prob_conjunction[1] = v1_v2_interval_prob_conjunction[1] * probabilisticIntepretationforRelation;
                    if (i==0 && j == 0)
                    {
                        result_IntervalProbabilistic.Add(v1_v2_interval_prob_conjunction[0]);
                        result_IntervalProbabilistic.Add(v1_v2_interval_prob_conjunction[1]);
                    }
                    else
                    {
                        result_IntervalProbabilistic = ProbabilisticCombinationStrategyUltilities.combine(result_IntervalProbabilistic[0], result_IntervalProbabilistic[1], v1_v2_interval_prob_conjunction[0], v1_v2_interval_prob_conjunction[1], ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION);
                    }
                }
                
            }
            return result_IntervalProbabilistic;

        }
        public override List<float> calculateProbabilisticInterpretation(Scan currentTuple, FPRDBSchema schema)
        {
            FieldType fieldType = schema.getFieldByName(lField).getFieldInfo().getType();

            if (fieldType == FieldType.INT || fieldType == FieldType.distFS_INT)
            {
                return genericCalculateProbabilisticInterpretation<int>(currentTuple.getFieldContent<int>(lField), currentTuple.getFieldContent<int>(rField));
            }
            else if (fieldType == FieldType.FLOAT || fieldType == FieldType.distFS_FLOAT || fieldType == FieldType.contFS)
            {
                return genericCalculateProbabilisticInterpretation<float>(currentTuple.getFieldContent<float>(lField), currentTuple.getFieldContent<float>(rField));
            }
            else if (fieldType == FieldType.CHAR || fieldType == FieldType.VARCHAR || fieldType == FieldType.distFS_TEXT)
            {
                return genericCalculateProbabilisticInterpretation<string>(currentTuple.getFieldContent<string>(lField), currentTuple.getFieldContent<string>(rField));
            }
            else //if (fieldType == FieldType.BOOLEAN)
            {
                return genericCalculateProbabilisticInterpretation<bool>(currentTuple.getFieldContent<bool>(lField), currentTuple.getFieldContent<bool>(rField));
            }
        }
        public override List<SelectionExpression> getAtomicSelectionExpression()
        {
            return new List<SelectionExpression> { this };
        }
    }
}
