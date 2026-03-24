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

        public FPRDBRelation(string relName, FPRDBSchema schema, string schemaName)
        {
            this.relName = relName;
            this.schema = schema;
            this.schemaName = schemaName;
        }

        public FPRDBRelation(string relName, string schemaName)
        {
            this.relName = relName;
            this.schemaName = schemaName;
        }

        public FPRDBRelation(string relName, FPRDBSchema schema)
        {
            this.relName = relName;
            this.schema = schema;
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
    }
}
