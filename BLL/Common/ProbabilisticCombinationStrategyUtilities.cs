using BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    public static class ProbabilisticCombinationStrategyUtilities
    {
        public static List<float> combine(float l1, float u1, float l2, float u2, ProbabilisticCombinationStrategy strategy)
        {
            float a = 0, b = 0;
            if (strategy == ProbabilisticCombinationStrategy.CONJUNCTION_IGNORANCE)
            {
                a = Math.Max(0, l1 + l2 - 1);
                b = Math.Min(u1, u2);
            }
            else if (strategy == ProbabilisticCombinationStrategy.CONJUNCTION_INDEPENDANCE)
            {
                a = l1 * l2;
                b = u1 * u2;
            }
            else if (strategy == ProbabilisticCombinationStrategy.CONJUNCTION_POSITIVE_CORRELATION)
            {
                a = Math.Min(l1, l2);
                b = Math.Min(u1, u2);
            }
            else if (strategy == ProbabilisticCombinationStrategy.CONJUNCTION_MUTUAL_EXCLUSION)
            {
                a = 0;
                b = 0;
            }
            else if (strategy == ProbabilisticCombinationStrategy.DISJUNCTION_IGNORANCE)
            {
                a = Math.Max(l1, l2);
                b = Math.Min(1, u1 + u2);
            }
            else if (strategy == ProbabilisticCombinationStrategy.DISJUNCTION_INDEPENDANCE)
            {
                a = l1 + l2 - l1 * l2;
                b = u1 + u2 - u1 * u2;
            }
            else if (strategy == ProbabilisticCombinationStrategy.DISJUNCTION_POSITIVE_CORRELATION)
            {
                a = Math.Max(l1, l2);
                b = Math.Max(u1, u2);
            }
            else if (strategy == ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION)
            {
                a = Math.Min(1, l1 + l2);
                b = Math.Min(1, u1 + u2);
            }
            else if (strategy == ProbabilisticCombinationStrategy.DIFFERENCE_IGNORANCE)
            {
                a = Math.Max(0, l1 - u2);
                b = Math.Min(u1, 1 - l2);
            }
            else if (strategy == ProbabilisticCombinationStrategy.DIFFERENCE_INDEPENDANCE)
            {
                a = l1 * (1 - u2);
                b = u1 * (1 - l2);
            }
            else if (strategy == ProbabilisticCombinationStrategy.DIFFERENCE_POSITIVE_CORRELATION)
            {
                a = Math.Max(0, l1 - u2);
                b = Math.Max(0, u1 - l2);
            }
            else if (strategy == ProbabilisticCombinationStrategy.DIFFERENCE_MUTUAL_EXCLUSION)
            {
                a = l1;
                b = Math.Min(u1, 1 - l2);
            }
            return new List<float> { a, b };
        }
        public static ProbabilisticCombinationStrategy convertStringToEnum(string str)
        {
            switch (str)
            {
                case "⨂_ig":
                    return ProbabilisticCombinationStrategy.CONJUNCTION_IGNORANCE;
                case "⨂_in":
                    return ProbabilisticCombinationStrategy.CONJUNCTION_INDEPENDANCE;
                case "⨂_pc":
                    return ProbabilisticCombinationStrategy.CONJUNCTION_POSITIVE_CORRELATION;
                case "⨂_me":
                    return ProbabilisticCombinationStrategy.CONJUNCTION_MUTUAL_EXCLUSION;
                case "⦵_ig":
                    return ProbabilisticCombinationStrategy.DIFFERENCE_IGNORANCE;
                case "⦵_in":
                    return ProbabilisticCombinationStrategy.DIFFERENCE_INDEPENDANCE;
                case "⦵_pc":
                    return ProbabilisticCombinationStrategy.DIFFERENCE_POSITIVE_CORRELATION;
                case "⦵_me":
                    return ProbabilisticCombinationStrategy.DIFFERENCE_MUTUAL_EXCLUSION;
                case "⨁_ig":
                    return ProbabilisticCombinationStrategy.DISJUNCTION_IGNORANCE;
                case "⨁_in":
                    return ProbabilisticCombinationStrategy.DISJUNCTION_INDEPENDANCE;
                case "⨁_pc":
                    return ProbabilisticCombinationStrategy.DISJUNCTION_POSITIVE_CORRELATION;
                case "⨁_me":
                    return ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION;
                default:
                    throw new Exception($"{str} is not a probabilistic combination strategy");

            }
        }
        static public string convertEnumToString(ProbabilisticCombinationStrategy enumValue)
        {
            switch (enumValue)
            {
                case ProbabilisticCombinationStrategy.CONJUNCTION_IGNORANCE:
                    return "⨂_ig";
                case ProbabilisticCombinationStrategy.CONJUNCTION_INDEPENDANCE:
                    return "⨂_in";
                case ProbabilisticCombinationStrategy.CONJUNCTION_POSITIVE_CORRELATION:
                    return "⨂_pc";
                case ProbabilisticCombinationStrategy.CONJUNCTION_MUTUAL_EXCLUSION:
                    return "⨂_me";
                case ProbabilisticCombinationStrategy.DIFFERENCE_IGNORANCE:
                    return "⦵_ig";
                case ProbabilisticCombinationStrategy.DIFFERENCE_INDEPENDANCE:
                    return "⦵_in";
                case ProbabilisticCombinationStrategy.DIFFERENCE_POSITIVE_CORRELATION:
                    return "⦵_pc";
                case ProbabilisticCombinationStrategy.DIFFERENCE_MUTUAL_EXCLUSION:
                    return "⦵_me";
                case ProbabilisticCombinationStrategy.DISJUNCTION_IGNORANCE:
                    return "⨁_ig";
                case ProbabilisticCombinationStrategy.DISJUNCTION_INDEPENDANCE:
                    return "⨁_in";
                case ProbabilisticCombinationStrategy.DISJUNCTION_POSITIVE_CORRELATION:
                    return "⨁_pc";
                //case ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION:
                //    return "⨁_me";
                default:
                    return "⨁_me";

            }
        }
        static public bool isDisjunctionStategy(ProbabilisticCombinationStrategy value)
        {
            switch (value)
            {
                case ProbabilisticCombinationStrategy.DISJUNCTION_IGNORANCE:
                case ProbabilisticCombinationStrategy.DISJUNCTION_INDEPENDANCE:
                case ProbabilisticCombinationStrategy.DISJUNCTION_POSITIVE_CORRELATION:
                case ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION:
                    return true;
                default:
                    return false;
            }
        }
        static public bool isDifferenceStategy(ProbabilisticCombinationStrategy value)
        {
            switch (value)
            {
                case ProbabilisticCombinationStrategy.DIFFERENCE_IGNORANCE:
                case ProbabilisticCombinationStrategy.DIFFERENCE_INDEPENDANCE:
                case ProbabilisticCombinationStrategy.DIFFERENCE_POSITIVE_CORRELATION:
                case ProbabilisticCombinationStrategy.DIFFERENCE_MUTUAL_EXCLUSION:
                    return true;
                default:
                    return false;
            }
        }
        static public bool isConjunctionStategy(ProbabilisticCombinationStrategy value)
        {
            switch (value)
            {
                case ProbabilisticCombinationStrategy.CONJUNCTION_IGNORANCE:
                case ProbabilisticCombinationStrategy.CONJUNCTION_INDEPENDANCE:
                case ProbabilisticCombinationStrategy.CONJUNCTION_POSITIVE_CORRELATION:
                case ProbabilisticCombinationStrategy.CONJUNCTION_MUTUAL_EXCLUSION:
                    return true;
                default:
                    return false;
            }
        }
    }
}
