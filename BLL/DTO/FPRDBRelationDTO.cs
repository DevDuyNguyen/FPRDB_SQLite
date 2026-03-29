using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainObject;

namespace BLL.DTO
{
    public class FPRDBRelationDTO
    {
        public int oid;
        public string relName;
        public FPRDBSchemaDTO fprdbSchema;
        public string schemaName;

        public FPRDBRelationDTO(string relName, FPRDBSchemaDTO fprdbSchema, string schemaName, int oid)
        {
            this.relName = relName;
            this.fprdbSchema = fprdbSchema;
            this.schemaName = schemaName;
            this.oid = oid;
        }
        public FPRDBRelationDTO(string relName, FPRDBSchemaDTO fprdbSchema, string schemaName)
        {
            this.relName = relName;
            this.fprdbSchema = fprdbSchema;
            this.schemaName = schemaName;
            this.oid = -1;
        }
    }
}
