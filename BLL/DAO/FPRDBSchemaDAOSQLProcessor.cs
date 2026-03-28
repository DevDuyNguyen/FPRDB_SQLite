using BLL.Common;
using BLL.DomainObject;
using BLL.DTO;
using BLL.Exceptions;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DAO
{
    public class FPRDBSchemaDAOSQLProcessor:FPRDBSchemaDAO
    {
        private SQLProcessor sqlProcessor;

        public FPRDBSchemaDAOSQLProcessor(SQLProcessor sqlProcessor)
        {
            this.sqlProcessor = sqlProcessor;
        }

        public bool defineFPRDBSchema(FPRDBSchemaDTO fprdbSchemaDTO)
        {
            if (fprdbSchemaDTO.schemaName == "" || fprdbSchemaDTO.schemaName == null)
                throw new SemanticException("Schema name is empty");
            if (fprdbSchemaDTO.fields.Count==0)
                throw new SemanticException("Attribute is empty");
            if (fprdbSchemaDTO.primarykey.Count == 0)
                throw new SemanticException("Primary key is empty");

            string fprdSQL = $"CREATE SCHEMA {fprdbSchemaDTO.schemaName} (";
            foreach(Field field in fprdbSchemaDTO.fields)
            {
                if(field.getFieldInfo().getType()==FieldType.VARCHAR)
                    fprdSQL += $" {field.getFieldName()} {FieldTypeUtilities.fromFieldTypeEnumToSQLFieldType(field.getFieldInfo().getType())} ({field.getFieldInfo().getTXTLength()}),";
                else 
                    fprdSQL += $" {field.getFieldName()} {FieldTypeUtilities.fromFieldTypeEnumToSQLFieldType(field.getFieldInfo().getType())},";
            }
            if(fprdbSchemaDTO.primarykey.Count>0)
            {
                fprdSQL += $" CONSTRAINT pk_{fprdbSchemaDTO.schemaName} PRIMARY KEY (";
                foreach (string key in fprdbSchemaDTO.primarykey)
                {
                    fprdSQL += " " + key + ",";
                }
                fprdSQL = fprdSQL.TrimEnd(',');
                fprdSQL += "))";
            }
            else
            {
                fprdSQL = fprdSQL.TrimEnd(',');
                fprdSQL += ")";
            }

            return this.sqlProcessor.executeDataDefinition(fprdSQL);
        }
        public void removeFPRDBSchema(FPRDBSchemaDTO fprdbSchemaDTO)
        {
            this.sqlProcessor.executeUpdate($"DROP SCHEMA {fprdbSchemaDTO.schemaName}");
        }
        public List<FPRDBSchema> findSchema(string name) => throw new NotImplementedException();
        public List<FPRDBRelation> findRelationsOfSchema(FPRDBSchemaDTO schema) => throw new NotImplementedException();
    }
}
