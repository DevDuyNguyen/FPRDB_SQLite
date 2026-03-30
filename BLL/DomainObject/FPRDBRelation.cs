using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class FPRDBRelation
    {
        private string relName;
        private FPRDBSchema schema;
        private string schemaName;
        private int oid;

        public FPRDBRelation(string relName, string schemaName)
        {
            this.relName = relName;
            this.schemaName = schemaName;
            this.oid = -1;
        }
        public FPRDBRelation(string relName, FPRDBSchema schema)
        {
            this.relName = relName;
            this.schema = schema;
            this.oid = -1;
        }
        public FPRDBRelation(string relName, FPRDBSchema schema, string schemaName)
        {
            this.relName = relName;
            this.schema = schema;
            this.schemaName = schemaName;
            this.oid = -1;
        }
        public FPRDBRelation(string relName, FPRDBSchema schema, string schemaName, int oid)
        {
            this.relName = relName;
            this.schema = schema;
            this.schemaName = schemaName;
            this.oid = oid;
        }

        public string getRelName()
        {
            return relName;
        }
        public FPRDBSchema getSchema()
        {
            return schema;
        }
        public void setRelName(string name)
        {
            this.relName = name;
        }

        public void setSchema(FPRDBSchema schema)
        {
            this.schema = schema;
        }
        public string getSchemaName() => this.schemaName;
        public FPRDBRelationDTO toDTO()
        {
            return new FPRDBRelationDTO(
                this.relName, 
                this.getSchema().toDTO(),
                this.schemaName,
                this.oid
                );
        }
    }
}
