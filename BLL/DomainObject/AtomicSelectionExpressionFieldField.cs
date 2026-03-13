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

            throw new NotImplementedException();
            //List<float> ans = new List<float>(2);
            //for(int i=0; i<v1.valueList.Count; ++i)
            //{
            //    List<float> tmp = new List<float>(2);
            //    for (int j = 0; j < v1.valueList.Count; ++j)
            //    {
            //        List<float> tmp1 = ProbabilisticCombinationStrategyUltilities.combine(v1.intervalProbLowerBoundList[i], v1.intervalProbUpperBoundList[i], v2.intervalProbLowerBoundList[j], v2.intervalProbUpperBoundList[j], this.probCombinationStrategy);
            //        float tmp2 = ProbabilisticInterpretationOfRelationOnFuzzySets.compare<T>(v1.valueList[i], v2.valueList[j], CompareOperation.EQUAL);
            //        if (j == 0)
            //        {
            //            tmp.Add(tmp1[0] * tmp2);
            //            tmp.Add(tmp1[1] * tmp2);
            //        }
            //        else
            //        {
            //            tmp = ProbabilisticCombinationStrategyUltilities.combine(tmp[0], tmp[1], tmp1[0] * tmp2, tmp1[1] * tmp2, ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION);
            //        }
            //    }
            //    if (i == 0)
            //    {
            //        ans.Add(tmp[0]);
            //        ans.Add(tmp[1]);
            //    }
            //    else
            //    {
            //        tmp = ProbabilisticCombinationStrategyUltilities.combine(ans[0], ans[1], tmp[0], tmp[1], ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION);
            //    }
            //}
            //return ans;

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
