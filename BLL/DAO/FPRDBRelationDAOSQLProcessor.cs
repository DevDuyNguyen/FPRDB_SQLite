using BLL.DomainObject;
using BLL.DTO;
using BLL.SQLProcessing;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DAO
{
    public class FPRDBRelationDAOSQLProcessor:FPRDBRelationDAO
    {
        private SQLProcessor sqlProcessor;

        public FPRDBRelationDAOSQLProcessor(SQLProcessor sqlProcessor)
        {
            this.sqlProcessor = sqlProcessor;
        }

        public bool createFPRDBRelation(FPRDBRelationDTO rel)
        {
            string fprdbSQL = $"CREATE RELATION {rel.relName} on {rel.schemaName}";
            return this.sqlProcessor.executeDataDefinition(fprdbSQL);
        }
        public bool removeFPRDBRelation(FPRDBRelationDTO rel) => throw new NotImplementedException();
        public List<FPRDBRelation> findRelation(string name)=>throw new NotImplementedException();

    }
}
