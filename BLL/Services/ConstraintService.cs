using BLL.Common;
using BLL.DomainObject;
using BLL.Exceptions;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.Services
{
    public class ConstraintService
    {
        private MetadataManager metadataMgr;

        public ConstraintService(MetadataManager metadataMgr)
        {
            this.metadataMgr = metadataMgr;
        }

        public bool checkIntegrityConstraint(FPRDBRelation rel, InsertData data)
        {
            FPRDBRelation relation;
            try
            {
                relation = this.metadataMgr.getRelation(data.relation);
            }
            catch (QueryDataNotExistException ex)
            {
                throw new SemanticException($"Relation {data.relation} doesn't exist");
            }
            FPRDBSchema schema = relation.getSchema();
            List<string> value = new List<string>();
            foreach (string keyAttr in schema.getPrimarykey())
            {
                if (!data.fieldList.Contains(keyAttr))
                    throw new SemanticException($"Must provide value for key attribute {keyAttr}");
                int index = data.fieldList.IndexOf(keyAttr);
                if (data.fuzzyProbabilisticValues[index].valueList.Count != 1
                    || !ConstantUltilities.isPrimitiveConstant(data.fuzzyProbabilisticValues[index].valueList[0]))
                    throw new SemanticException($"Invalid insert value for key attribute {keyAttr}");
                value.Add(data.fuzzyProbabilisticValues[index].valueList[0].getVal().ToString());
            }
            if (this.metadataMgr.isTupleExist(schema.getPrimarykey(), value, data.relation))
                throw new SemanticException("IDENTITY constraint violation");
            return true;
        }
    }
}
