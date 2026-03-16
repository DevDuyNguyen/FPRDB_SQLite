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
        public static int CompareFloats(float a, float b, float epsilon = 0.00001f)
        {
            // Check for equality within precision
            if (Math.Abs(a - b) < epsilon)
            {
                return 0; // Return 0 for equal
            }

            // If not equal, check which is larger
            return a < b ? -1 : 1;
        }

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
        public static bool floatNearlyEquals(float a, float b)
        {
            float epsilon = 1e-6f;

            if (float.IsNaN(a) || float.IsNaN(b))
                return false;
            if (float.IsInfinity(a) || float.IsInfinity(b))
                return a == b;
            if (a == b)
                return true;

            float diff = MathF.Abs(a - b);
            if (diff < epsilon)
                return true;

            return false;
        }
        public static bool floatGreaterThan(float a, float b)
        {
            if (!floatNearlyEquals(a, b))
            {
                return a > b;
            }
            else
                return false;
        }
        public static bool floatSmallerThan(float a, float b)
        {
            if (!floatNearlyEquals(a, b))
            {
                return a < b;
            }
            else
                return false;
        }
    }
    
 }
