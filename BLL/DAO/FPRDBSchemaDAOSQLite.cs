using BLL.DomainObject;
using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DAO
{
    public class FPRDBSchemaDAOSQLite:FPRDBSchemaDAO
    {
        private DatabaseManager databaseMgr;

        public FPRDBSchemaDAOSQLite(DatabaseManager databaseMgr)
        {
            this.databaseMgr = databaseMgr;
        }

        public FPRDBSchema defineFPRDBSchema(FPRDBSchemaDTO fprdbSchemaDTO)
        {
            string fprdSQL = $"CREATE SCHEMA {fprdbSchemaDTO.schemaName} (";
            foreach(Field field in fprdbSchemaDTO.fields)
            {

            }

        }
        public bool removeFPRDBSchema(FPRDBSchemaDTO fprdbSchemaDTO)=>throw new NotImplementedException();
        public List<FPRDBSchema> findSchema(string name) => throw new NotImplementedException();;
        public List<FPRDBRelation> findRelationsOfSchema(FPRDBSchemaDTO schema) => throw new NotImplementedException();;
    }
}
