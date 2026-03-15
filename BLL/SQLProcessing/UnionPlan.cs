using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class UnionPlan:Plan
    {
        private Plan p1;
        private Plan p2;
        private ProbabilisticCombinationStrategy probCombinationStrategy;
        private FPRDBSchema schema;

        public UnionPlan(Plan p1, Plan p2, ProbabilisticCombinationStrategy probCombinationStrategy)
        {
            this.p1 = p1;
            this.p2 = p2;
            if (!isSameSchema(this.p1.getSchema(), this.p2.getSchema()))
            {
                throw new InvalidDataException("Relations of the union doesn't have the same schema structure");
            }
            if (!ProbabilisticCombinationStrategyUtilities.isDisjunctionStategy(probCombinationStrategy))
                throw new InvalidDataException("Union must be paired with probabilistic disjunction strategy");
            this.probCombinationStrategy = probCombinationStrategy;
            this.schema = this.p1.getSchema();
        }
        private bool isSameSchema(FPRDBSchema sch1, FPRDBSchema sch2)
        {
            bool isSameStructure = true;
            List<Field> sch1Fields = sch1.getFields();
            List<Field> sch2Fields = sch2.getFields();
            isSameStructure = sch1Fields.Count == sch2Fields.Count;
            for (int i = 0; i < sch1Fields.Count; ++i)
            {
                if (!(sch1Fields[i].getFieldName() == sch2Fields[i].getFieldName()
                    && sch1Fields[i].getFieldInfo().getType() == sch2Fields[i].getFieldInfo().getType()
                    && sch1Fields[i].getFieldInfo().getTXTLength() == sch2Fields[i].getFieldInfo().getTXTLength()
                    ))
                {
                    isSameStructure = false;
                    break;
                }
            }
            return isSameStructure;
        }
        public Scan open()
        {
            return new UnionScan(this.p1.open(), this.p2.open(), this.probCombinationStrategy, this.schema);
        }
        public FPRDBSchema getSchema() => this.schema;

    }
}
