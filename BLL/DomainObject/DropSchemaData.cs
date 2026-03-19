using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class DropSchemaData
    {
        public string schema;

        public DropSchemaData(string schema)
        {
            this.schema = schema;
        }
    }
}
