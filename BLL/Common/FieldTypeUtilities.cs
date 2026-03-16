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
        public static FieldType turnSQLFieldTypeToEnumFieldType(string str)
        {
            switch (str.ToUpper())
            {
                case "INT":
                    return FieldType.INT;
                case "FLOAT":
                    return FieldType.FLOAT;
                case "CHAR":
                    return FieldType.CHAR;
                case "VARCHAR":
                    return FieldType.VARCHAR;
                case "BOOLEAN":
                    return FieldType.BOOLEAN;
                case "DIST_FUZZYSET_INT":
                    return FieldType.distFS_INT;
                case "DIST_FUZZYSET_FLOAT":
                    return FieldType.distFS_FLOAT;
                case "DIST_FUZZYSET_TEXT":
                    return FieldType.distFS_TEXT;
                case "CONT_FUZZYSET":
                    return FieldType.contFS;
                default:
                    throw new InvalidDataException($"{str} isn't supported");
            }
        }
        static public bool isPrimitive(FieldType type)
        {
            return !(type == FieldType.distFS_INT || type == FieldType.distFS_FLOAT || type == FieldType.distFS_TEXT || type == FieldType.contFS);
        }
        static public bool isContinuousFuzzySet(FieldType type)
        {
            return type == FieldType.contFS;
        }
        static public List<FieldType> getDefineDomainForFuzzySet()
        {
            return new List<FieldType> { FieldType.INT, FieldType.FLOAT, FieldType.VARCHAR};
        }
    }
}
