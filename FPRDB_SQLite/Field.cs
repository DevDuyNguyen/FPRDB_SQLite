using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPRDB_SQLite
{
    public class Field
    {
        private String fieldName;
        private FieldInfo fieldInfo;
        public String getFieldName()
        {
            return fieldName;
        }
        public FieldInfo getFieldInfo()
        {
            return fieldInfo;
        }
        public void setFieldName(String fieldName)
        {
            this.fieldName = fieldName;
        }
    }
}
