using BLL.DomainObject;
using BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public bool checkSemanticCreateRelation(FPRDBRelation data)
        {
            //relation name must not existed before
            if (this.metadataMgr.isRelationExist(data.getRelName()))
            {
                throw new SemanticException($"Relation with name {data.getRelName()} already exists");
            }
            //schema must existed before
            if (!this.metadataMgr.isSchemaExist(data.getSchemaName()))
            {
                throw new SemanticException($"No schema with name {data.getSchemaName()} exists");
            }
            return true;
        }


    }
}
