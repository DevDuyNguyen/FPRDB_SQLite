using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    static public class FieldTypeUltilities
    {
        static public bool isPrimitive(FieldType type)
        {
            return !(type == FieldType.distFS_INT || type == FieldType.distFS_FLOAT || type == FieldType.distFS_TEXT || type == FieldType.contFS);
        }
        static public bool isContinuousFuzzySet(FieldType type)
        {
            return type == FieldType.contFS;
        }
    }
}
