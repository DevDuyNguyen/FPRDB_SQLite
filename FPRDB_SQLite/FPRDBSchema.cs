using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPRDB_SQLite
{
    public class FPRDBSchema
    {
        private String schemaName;
        private List<Field> fields;
        public List<string> PrimaryKeys { get; set; } = new List<string>();
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
    }
}
