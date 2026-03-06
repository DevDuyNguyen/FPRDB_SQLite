using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Exceptions;
using System.Data;

namespace BLL.SQLProcessing
{
    public class Preprocessor
    {
        private MetadataManager metadataMgr;

        public Preprocessor(MetadataManager metadataMgr)
        {
            this.metadataMgr = metadataMgr;
        }
        public bool checkSemanticCreateSchema(FPRDBSchema data)
        {
            string schemaName = data.getSchemaName();
            string constraintName = data.getPrimaryConstraintName();
            try
            {
                if (this.metadataMgr.isSchemaExist(schemaName))
                {
                    throw new SemanticException($"Schema {schemaName} already exists");
                }
                if(data.getPrimaryConstraintName()=="" || data.getPrimaryConstraintName()==null 
                    || data.getPrimaryConstraintName() == null || data.getPrimarykey().Count == 0)
                {
                    throw new SemanticException($"Schema creation must have primary key");
                }
                if (this.metadataMgr.isConstraintExist(constraintName))
                {
                    throw new SemanticException($"Constraint with name {constraintName} already exists");
                }
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


    }
}
