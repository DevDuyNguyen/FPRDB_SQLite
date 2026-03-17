using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;

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
    }
}
