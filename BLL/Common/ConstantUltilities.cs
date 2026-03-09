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
    }
}
