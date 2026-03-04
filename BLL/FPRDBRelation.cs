using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class FPRDBRelation
    {
        private String relName;
        private FPRDBSchema schema;
        public FPRDBRelation(String relName, FPRDBSchema schema)
        {
            this.relName = relName;
            this.schema = schema;
        }
        public String getRelName()
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
    }
}
