using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    public class FieldTypeUltilities
    {
        public bool isPrimitive(FieldType type)
        {
            return !(type == FieldType.distFS_INT || type == FieldType.distFS_FLOAT || type == FieldType.distFS_TEXT || type == FieldType.contFS);
        }
    }
}
