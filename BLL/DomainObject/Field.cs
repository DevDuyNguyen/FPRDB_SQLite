using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class Field
    {
        private String fieldName;
        private FieldInfo fieldInfo;

        public Field(string fieldName, FieldInfo fieldInfo)
        {
            this.fieldName = fieldName;
            this.fieldInfo = fieldInfo;
        }
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
