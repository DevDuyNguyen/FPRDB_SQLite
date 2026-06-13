using System;
using System.Collections.Generic;
using System.IO;

namespace BLL.Common
{
    public static class FieldTypeUtilities
    {
        //static public bool isPrimitive(FieldType type)
        //{
        //    return !(type == FieldType.DIST_FUZZYSET_INT || type == FieldType.DIST_FUZZYSET_FLOAT || type == FieldType.DIST_FUZZYSET_TEXT || type == FieldType.CONT_FUZZYSET);
        //}
        //static public bool isContinuousFuzzySet(FieldType type)
        //{
        //    return type == FieldType.CONT_FUZZYSET;
        //}
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
            else if (type == FieldType.DIST_FUZZYSET_INT)
                return "DIST_FUZZYSET_INT";
            else if (type == FieldType.DIST_FUZZYSET_FLOAT)
                return "DIST_FUZZYSET_FLOAT";
            else if (type == FieldType.DIST_FUZZYSET_TEXT)
                return "DIST_FUZZYSET_TEXT";
            else
                return "CONT_FUZZYSET";
        }
        public static FieldType turnSQLFieldTypeToEnumFieldType(string str)
        {
            if(str==null)
                throw new InvalidDataException($"Field type isn't provided");
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
                    return FieldType.DIST_FUZZYSET_INT;
                case "DIST_FUZZYSET_FLOAT":
                    return FieldType.DIST_FUZZYSET_FLOAT;
                case "DIST_FUZZYSET_TEXT":
                    return FieldType.DIST_FUZZYSET_TEXT;
                case "CONT_FUZZYSET":
                    return FieldType.CONT_FUZZYSET;
                default:
                    throw new InvalidDataException($"{str} isn't supported");
            }
        }
        static public bool isPrimitive(FieldType type)
        {
            return !(type == FieldType.DIST_FUZZYSET_INT || type == FieldType.DIST_FUZZYSET_FLOAT || type == FieldType.DIST_FUZZYSET_TEXT || type == FieldType.CONT_FUZZYSET);
        }
        static public bool isContinuousFuzzySet(FieldType type)
        {
            return type == FieldType.CONT_FUZZYSET;
        }
        static public List<FieldType> getDefineDomainForFuzzySet()
        {
            return new List<FieldType> { FieldType.INT, FieldType.FLOAT, FieldType.VARCHAR};
        }
        //static public genericActionByFieldType(FieldType, Action<string, FuzzyProbabilisticValue>)
        static public bool isPrimitiveNumberFieldType(FieldType fType)
        {
            return fType == FieldType.INT || fType == FieldType.FLOAT;
        }
        static public Type getDomainType(FieldType fType)
        {
            if (fType == FieldType.INT || fType == FieldType.DIST_FUZZYSET_INT)
                return typeof(int);
            else if (fType == FieldType.FLOAT || fType == FieldType.DIST_FUZZYSET_FLOAT || fType==FieldType.CONT_FUZZYSET)
                return typeof(float);
            else if (fType == FieldType.CHAR || fType == FieldType.VARCHAR || fType == FieldType.DIST_FUZZYSET_TEXT)
                return typeof(string);
            else //if (fType == FieldType.BOOLEAN)
                return typeof(bool);
        }

    }
}
