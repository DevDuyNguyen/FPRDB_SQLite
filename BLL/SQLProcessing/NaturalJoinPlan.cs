using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class NaturalJoinPlan:Plan
    {
        private Plan p1,p2;
        private ProbabilisticCombinationStrategy probCombinationStrategy;
        private FPRDBSchema schema;
        private List<string> commonFields;

        public NaturalJoinPlan(Plan p1, Plan p2, ProbabilisticCombinationStrategy probCombinationStrategy)
        {
            if (!ProbabilisticCombinationStrategyUtilities.isConjunctionStategy(probCombinationStrategy))
            {
                throw new InvalidDataException("Probabilistic combination strategy must be conjunction");
            }
            this.p1 = p1;
            this.p2 = p2;
            this.probCombinationStrategy = probCombinationStrategy;
            List<Field> p1Fields = this.p1.getSchema().getFields();
            List<Field> p2Fields = this.p2.getSchema().getFields();
            List<Field> pfields = new List<Field>();
            List<string> commonFields = new List<string>();
            foreach (Field field in p1Fields)
            {
                pfields.Add(field);
            }
            bool isCommon;
            foreach (Field field1 in p2Fields)
            {
                isCommon = false;
                foreach(Field field2 in pfields)
                {
                    if (field1.getFieldName() == field2.getFieldName())
                    {
                        commonFields.Add(field1.getFieldName());
                        isCommon = true;
                        break;

                    }   
                }
                if (!isCommon)
                    pfields.Add(field1);
            }
            this.commonFields = commonFields;
            this.schema = new FPRDBSchema(null, pfields, null);
        }

        public Scan open()
        {
            return new NaturalJoinScan(this.p1.open(), this.p2.open(), this.commonFields, this.schema, this.probCombinationStrategy);
        }
        public FPRDBSchema getSchema() => this.schema;
    }
}
