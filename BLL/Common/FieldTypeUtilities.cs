using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    public static class FieldTypeUtilities
    {
        public static string fromFieldTypeEnumToSQLFieldType(FieldType type)
        {
            if (type == FieldType.INT)
                return "INT";
            else if (type == FieldType.FLOAT)
                return "FLOAT";
            else if (type == FieldType.CHAR)
                return "CHAR";
            else if (type == FieldType.VARCHAR)
                return "VARCHAR";
            else if (type == FieldType.BOOLEAN)
                return "BOOLEAN";
            else if (type == FieldType.distFS_INT)
                return "DIST_FUZZYSET_INT";
            else if (type == FieldType.distFS_FLOAT)
                return "DIST_FUZZYSET_FLOAT";
            else if (type == FieldType.distFS_TEXT)
                return "DIST_FUZZYSET_TEXT";
            else
                return "CONT_FUZZYSET";
        }

    }
}
