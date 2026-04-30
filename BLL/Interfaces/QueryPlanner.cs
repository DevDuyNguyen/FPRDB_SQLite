using BLL.DomainObject;

namespace BLL.Interfaces
{
    public interface QueryPlanner
    {
        public Plan createPlan(QueryData data);
        public float calculateProbabilisticInterpretationForRelationOnFuzzySetsExpression(RelationOnFuzzySetExpressionData data);
    }
}
