using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Enums;

namespace BLL.Common
{
    public static class ProbabilisticCombinationStrategyUltilities
    {
        public static List<float> combine(float l1, float u1, float l2, float u2, ProbabilisticCombinationStrategy strategy)
        {
            float a=0, b=0;
            if (strategy==ProbabilisticCombinationStrategy.CONJUNCTION_IGNORANCE)
            {
                a = Math.Max(0, l1+l2-1);
                b = Math.Min(u1, u2);
            }
            else if(strategy == ProbabilisticCombinationStrategy.CONJUNCTION_INDEPENDANCE)
            {
                a = l1 * l2;
                b = u1*u2;
            }
            else if (strategy == ProbabilisticCombinationStrategy.CONJUNCTION_POSITIVE_CORRELATION)
            {
                a = Math.Min(l1,l2);
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
                b = Math.Min(1,u1+ u2);
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
                a = Math.Min(1, l1+ l2);
                b = Math.Min(1, u1+u2);
            }
            else if (strategy == ProbabilisticCombinationStrategy.DIFFERENCE_IGNORANCE)
            {
                a = Math.Max(0, l1 - u2);
                b = Math.Min(u1, 1 - l2);
            }
            else if (strategy == ProbabilisticCombinationStrategy.DIFFERENCE_INDEPENDANCE)
            {
                a = l1*(1-u2);
                b = u1*(1-l2);
            }
            else if (strategy == ProbabilisticCombinationStrategy.DIFFERENCE_POSITIVE_CORRELATION)
            {
                a = Math.Max(0, l1 - u2);
                b = Math.Max(0, u1 - l2);
            }
            else if (strategy == ProbabilisticCombinationStrategy.DIFFERENCE_MUTUAL_EXCLUSION)
            {
                a = l1;
                b = Math.Min(u1, 1-l2);
            }
            return new List<float> { a,b };
        }

    }
}
