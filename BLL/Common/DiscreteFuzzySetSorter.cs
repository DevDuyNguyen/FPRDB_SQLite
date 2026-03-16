using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    public static class DiscreteFuzzySetSorter
    {
        public static void MergeSort<T>(DiscreteFuzzySet<T> fuzzySet)
        {
            if (fuzzySet.valueSet == null || fuzzySet.membershipDegreeSet == null)
                throw new InvalidOperationException("Sets cannot be null.");

            if (fuzzySet.valueSet.Count != fuzzySet.membershipDegreeSet.Count)
                throw new InvalidOperationException("valueSet and membershipDegreeSet must have the same length.");

            var (sortedValues, sortedDegrees) = MergeSortRecursive<T>(
                fuzzySet.valueSet,
                fuzzySet.membershipDegreeSet);

            fuzzySet.valueSet = sortedValues;
            fuzzySet.membershipDegreeSet = sortedDegrees;
        }

        private static (List<T> values, List<float> degrees) MergeSortRecursive<T>(
            List<T> values,
            List<float> degrees)
        {
            if (values.Count <= 1)
                return (values, degrees);

            int mid = values.Count / 2;

            var leftValues = values.GetRange(0, mid);
            var leftDegrees = degrees.GetRange(0, mid);
            var rightValues = values.GetRange(mid, values.Count - mid);
            var rightDegrees = degrees.GetRange(mid, degrees.Count - mid);

            var (sortedLeftValues, sortedLeftDegrees) = MergeSortRecursive<T>(leftValues, leftDegrees);
            var (sortedRightValues, sortedRightDegrees) = MergeSortRecursive<T>(rightValues, rightDegrees);

            return Merge(
                sortedLeftValues, sortedLeftDegrees,
                sortedRightValues, sortedRightDegrees);
        }

        private static (List<T> values, List<float> degrees) Merge<T>(
            List<T> leftValues, List<float> leftDegrees,
            List<T> rightValues, List<float> rightDegrees)
        {
            var mergedValues = new List<T>(leftValues.Count + rightValues.Count);
            var mergedDegrees = new List<float>(leftDegrees.Count + rightDegrees.Count);

            int i = 0, j = 0;

            while (i < leftDegrees.Count && j < rightDegrees.Count)
            {
                if (leftDegrees[i] > rightDegrees[j])
                {
                    mergedValues.Add(leftValues[i]);
                    mergedDegrees.Add(leftDegrees[i]);
                    i++;
                }
                else
                {
                    mergedValues.Add(rightValues[j]);
                    mergedDegrees.Add(rightDegrees[j]);
                    j++;
                }
            }

            while (i < leftDegrees.Count)
            {
                mergedValues.Add(leftValues[i]);
                mergedDegrees.Add(leftDegrees[i]);
                i++;
            }

            while (j < rightDegrees.Count)
            {
                mergedValues.Add(rightValues[j]);
                mergedDegrees.Add(rightDegrees[j]);
                j++;
            }

            return (mergedValues, mergedDegrees);
        }
    }
}
