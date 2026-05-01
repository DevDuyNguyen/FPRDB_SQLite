using BLL.DomainObject;
using BLL.SQLProcessing;

namespace BLL.Interfaces
{
    public interface QueryPlanner
    {
        public Plan createPlan(QueryData data);
        public float calculateProbabilisticInterpretationForRelationOnFuzzySetsExpression(RelationOnFuzzySetExpressionData data);
        public SelectPlan createPlanForCalculatingProbabilisticInterpretationForSelectionOnSpeficifiedTuple(SelectionExpressionOnSpecifiedTuplesData data);
    }
}
