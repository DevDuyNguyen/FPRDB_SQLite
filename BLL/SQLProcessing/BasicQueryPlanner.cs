using BLL.DomainObject;
using BLL.Interfaces;
using BLL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Enums;

namespace BLL.SQLProcessing
{
    public class BasicQueryPlanner:QueryPlanner
    {
        private MetadataManager metaDataMgr;
        private DatabaseManager dbMgr;
        RecursiveDescentParser parser;

        public BasicQueryPlanner(MetadataManager metaDataMgr, DatabaseManager dbMgr, RecursiveDescentParser parser)
        {
            this.metaDataMgr = metaDataMgr;
            this.dbMgr = dbMgr;
            this.parser = parser;
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
                relPlans.Add(new RelationPlan(relName, this.metaDataMgr, this.dbMgr, this.parser));
            }
            //create product plans 
            Plan plan = relPlans[0];
            for(int i=1; i<relPlans.Count; ++i)
            {
                plan = new ProductPlan(plan, relPlans[i], this.metaDataMgr, this.dbMgr);
            }
            //create selection plan
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
                relPlans.Add(new RelationPlan(relName, this.metaDataMgr, this.dbMgr, this.parser));
            }
            //create natural join plans
            Plan plan = relPlans[0];
            for (int i = 1; i < relPlans.Count; ++i)
            {
                plan = new NaturalJoinPlan(plan, relPlans[i], data.naturalJoinList.probCombinationStrategyList[i - 1]);
            }
            //create selection plan
            plan = new SelectPlan(plan, data.selectionCondition);
            //creat projection plan
            plan = new ProjectPlan(plan, data.selectList.Select(x => x.field).ToList());
            return plan;
        }
        
    }
}
