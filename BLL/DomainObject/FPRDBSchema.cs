using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class FPRDBSchema
    {
        private String schemaName;
        private List<Field> fields;
        public List<string> primarykey;
        public string primaryConstraintName;
        public FPRDBSchema(string schemaName, List<Field> fields, List<string> primarykey)
        {
            this.schemaName = schemaName;
            this.fields = fields;
            this.primarykey = primarykey;
        }

        public FPRDBSchema(string schemaName, List<Field> fields, List<string> primarykey, string primaryConstraintName)
        {
            this.schemaName = schemaName;
            this.fields = fields;
            this.primarykey = primarykey;
            this.primaryConstraintName = primaryConstraintName;
        }
        public string getPrimaryConstraintName() => this.primaryConstraintName;

        public String getSchemaName()
        {
            return schemaName;
        }
        public List<Field> getFields()
        {
            return fields;
        }
        public void setSchemaName(String schemaName)
        {
            this.schemaName = schemaName;
        }
        public void setFields(List<Field> fields)
        {
            this.fields = fields;
        }
        public List<string> getPrimarykey()
        {
            return this.primarykey;
        }
    }
}
