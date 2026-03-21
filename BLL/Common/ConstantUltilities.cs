using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.SQLProcessing;

namespace BLL.Common
{
    public static class ConstantUltilities
    {
        public static bool isPrimitiveConstant(Constant c)
        {
            return c is IntConstant || c is FloatConstant || c is StringConstant || c is BooleanConstant;
        }
        public static Constant turnValueIntoConstant(object v)
        {
            if (v is int)
                return new IntConstant((int)v);
            else if (v is float)
                return new FloatConstant((float)v);
            else if (v is bool)
                return new BooleanConstant(Convert.ToBoolean(v));
            else if (v is string)
            {
                string str = (string)v;
                if (str[0] == '\'' || str[0] == '\"')
                {
                    str = str.TrimStart('\'', '\"');
                    str = str.TrimEnd('\'', '\"');
                    return new StringConstant(str);
                }
                else
                    return new FuzzySetConstant((string)v);
            }
            else
            {
                throw new Exception($"{v.ToString()} isn't a constant");
            }
        }
        static public Type getDomainType(Constant c, MetadataManager metaDataMgr)
        {
            if (c is IntConstant)
                return typeof(int);
            else if (c is FloatConstant)
                return typeof(float);
            else if (c is StringConstant)
                return typeof(string);
            else if (c is BooleanConstant)
                return typeof(bool);
            else
            {
                string fsName = (c as FuzzySetConstant).getVal() as string;
                FieldType fuzzySetType = metaDataMgr.getFuzzySetType(fsName);
                if (fuzzySetType is FieldType.distFS_INT)
                    return typeof(int);
                else if (fuzzySetType is FieldType.distFS_FLOAT|| fuzzySetType is FieldType.contFS)
                    return typeof(float);
                else //if (fuzzySetType is FieldType.distFS_TEXT)
                    return typeof(string);

            }
        }


    }
}
