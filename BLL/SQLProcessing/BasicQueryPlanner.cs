using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Interfaces;
using BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class BasicQueryPlanner:QueryPlanner
    {
        private MetadataManager metaDataMgr;
        private DatabaseManager dbMgr;
        private RecursiveDescentParser parser;
        private ConstraintService constraintService;
        public BasicQueryPlanner(MetadataManager metaDataMgr, DatabaseManager dbMgr, RecursiveDescentParser parser, ConstraintService constraintService)
        {
            this.metaDataMgr = metaDataMgr;
            this.dbMgr = dbMgr;
            this.parser = parser;
            this.constraintService= constraintService;
        }

        public Plan createPlan(QueryData data)
        {
            if (data is BaseCartesianProductQueryData)
                return createPlanForBaseCartesianProductQueryData((BaseCartesianProductQueryData)data);
            else if (data is BaseNaturalJoinQueryData)
                return createPlanForBaseNaturalJoinQueryData((BaseNaturalJoinQueryData)data);
            
            //the query data is compound: union, intersection, difference
            CompoundQueryData compndData = (CompoundQueryData)data;
            Plan lPlan = createPlan(compndData.leftQuery);
            Plan rPlan = createPlan(compndData.rightQuery);
            if (compndData.setConnective == SetConnective.INTERSECT)
            {
                return new IntersectionPlan(lPlan, rPlan, compndData.probCombinationStrategy);
            }
            else if (compndData.setConnective == SetConnective.UNION)
            {
                return new UnionPlan(lPlan, rPlan, compndData.probCombinationStrategy);
            }
            else //if (compndData.setConnective == SetConnective.EXCEPT)
            {
                return new DifferencePlan(lPlan, rPlan, compndData.probCombinationStrategy);
            }

        }
        private Plan createPlanForBaseCartesianProductQueryData(BaseCartesianProductQueryData data)
        {
            //create relation plans for each mentioned relations
            List<RelationPlan> relPlans = new List<RelationPlan>();
            foreach(string relName in data.relationList)
            {
                relPlans.Add(new RelationPlan(relName, this.metaDataMgr, this.dbMgr, this.parser, this.constraintService));
            }
            //create product plans 
            Plan plan = relPlans[0];
            for(int i=1; i<relPlans.Count; ++i)
            {
                plan = new ProductPlan(plan, relPlans[i], this.metaDataMgr, this.dbMgr);
            }
            //create selection plan
            if(data.selectionCondition!=null)
                plan = new SelectPlan(plan, data.selectionCondition);
            //creat projection plan
            plan = new ProjectPlan(plan, data.selectList.Select(x => x.field).ToList());
            return plan;
        }
        private Plan createPlanForBaseNaturalJoinQueryData(BaseNaturalJoinQueryData data)
        {
            //create relation plans for each mentioned relations
            List<RelationPlan> relPlans = new List<RelationPlan>();
            foreach (string relName in data.naturalJoinList.relationList)
            {
                relPlans.Add(new RelationPlan(relName, this.metaDataMgr, this.dbMgr, this.parser, this.constraintService));
            }
            //create natural join plans
            Plan plan = relPlans[0];
            for (int i = 1; i < relPlans.Count; ++i)
            {
                plan = new NaturalJoinPlan(plan, relPlans[i], data.naturalJoinList.probCombinationStrategyList[i - 1]);
            }
            //create selection plan
            if (data.selectionCondition != null)
                plan = new SelectPlan(plan, data.selectionCondition);
            //creat projection plan
            plan = new ProjectPlan(plan, data.selectList.Select(x => x.field).ToList());
            return plan;
        }
        public float calculateProbabilisticInterpretationForRelationOnFuzzySetsExpression(RelationOnFuzzySetExpressionData data)
        {
            //float probabilisticInterpretation;
            Constant leftFuzzySetConstant = data.getLeftFuzzySetConstant();
            Constant rightFuzzySetConstant = data.getRightFuzzySetConstant();

            BaseFuzzySet fs1, fs2;
            fs1 = FuzzySetUltilities.turnConstantToFuzzySet(leftFuzzySetConstant, this.metaDataMgr);
            fs2 = FuzzySetUltilities.turnConstantToFuzzySet(rightFuzzySetConstant, this.metaDataMgr);

            return ProbabilisticInterpretationOfRelationOnFuzzySets.compareFuzzySet(fs1, fs2, data.getCompareOp());

        }

    }
}
