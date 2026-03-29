using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class FPRDBSchemaDTO
    {
        public string schemaName;
        public List<Field> fields;
        public List<string> primarykey;
        public int oid;
        public FPRDBSchemaDTO(string schemaName, List<Field> fields, List<string> primarykey)
        {
            this.schemaName = schemaName;
            this.fields = fields;
            this.primarykey = primarykey;
            this.oid = -1;
        }
        public FPRDBSchemaDTO(string schemaName, List<Field> fields, List<string> primarykey, int oid)
        {
            this.schemaName = schemaName;
            this.fields = fields;
            this.primarykey = primarykey;
            this.oid = oid;
        }
    }
}
