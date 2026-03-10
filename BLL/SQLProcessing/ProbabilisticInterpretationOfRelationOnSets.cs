using BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public static class ProbabilisticInterpretationOfRelationOnSets
    {
        public static float compare<T>(List<T> s1, List<T> s2, CompareOperation compOperator) where T : IComparable<T>
        {
            if (s1.Count == 0 || s2.Count == 0) return 0;

            float epsilon = 0.00001f;
            int count = 0;
            int compareRes;
            foreach (T v1 in s1)
            {
                foreach (T v2 in s2)
                {
                    // Use CompareTo since T is IComparable
                    if (typeof(T) == typeof(float))
                    {
                        float a = Convert.ToSingle(v1);
                        float b = Convert.ToSingle(v2);
                        if (Math.Abs(a - b) < epsilon)
                        {
                            if (compOperator == CompareOperation.EQUAL || compOperator == CompareOperation.LESS_EQUAL || compOperator == CompareOperation.GREATER_EQUAL)
                                ++count;
                        }
                        else {
                            if (a < b)
                            {
                                if (compOperator == CompareOperation.LESS_THAN || compOperator == CompareOperation.LESS_EQUAL)
                                    ++count;
                            }
                            else
                            {
                                if(compOperator == CompareOperation.GREATER_THAN || compOperator == CompareOperation.GREATER_EQUAL)
                                ++count;
                            }

                        }
                    }
                    else
                    {
                        compareRes = v1.CompareTo(v2);
                        if (compareRes == 0)
                        {
                            if (compOperator == CompareOperation.EQUAL || compOperator == CompareOperation.LESS_EQUAL || compOperator == CompareOperation.GREATER_EQUAL)
                                ++count;
                        }
                        else if (compareRes < 0)
                        {
                            if (compOperator == CompareOperation.LESS_THAN || compOperator == CompareOperation.LESS_EQUAL)
                                ++count;
                        }
                        else if (compareRes > 0)
                        {
                            if (compOperator == CompareOperation.GREATER_THAN || compOperator == CompareOperation.GREATER_EQUAL)
                                ++count;
                        }
                    }
                }
            }

            return (float)count / (s1.Count * s2.Count);
        }
        public static float subset<T>(List<T> s1, List<T> s2) where T : IComparable<T>
        {
            int count = 0;
            foreach(T v in s1)
            {
                if (s2.Contains(v))
                    ++count;
            }
            return (float)count / (s1.Count);
        }

    }
}
