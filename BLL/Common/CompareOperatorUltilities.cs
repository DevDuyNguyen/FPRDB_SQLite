using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Enums;

namespace BLL.Common
{
    public static class CompareOperatorUltilities
    {
        public static CompareOperation convertStringToEnum(string str)
        {
            switch (str)
            {
                case "=":
                    return CompareOperation.EQUAL;
                case "!=":
                    return CompareOperation.NOT_EQUAL;
                case "<":
                    return CompareOperation.LESS_THAN;
                case "<=":
                    return CompareOperation.LESS_EQUAL;
                case ">":
                    return CompareOperation.GREATER_THAN;
                case ">=":
                    return CompareOperation.GREATER_EQUAL;
                case "⊆":
                    return CompareOperation.SUBSET;
                case "∈":
                    return CompareOperation.IN;
                //case "⇒":
                //    return CompareOperation.ALSO;
                default:
                    return CompareOperation.ALSO;
            }
        }
    }
}
