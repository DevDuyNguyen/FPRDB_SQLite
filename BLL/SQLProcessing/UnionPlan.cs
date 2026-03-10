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
            this.probCombinationStrategy = probCombinationStrategy;
            this.schema = this.p1.getSchema();
        }

        public Scan open() => throw new NotImplementedException();
        public FPRDBSchema getSchema() => this.schema;

    }
}
