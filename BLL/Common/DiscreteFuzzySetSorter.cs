using BLL.DomainObject;
using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    public static class DiscreteFuzzySetSorter
    {
        public static void MergeSortByMembershipDegree<T>(DiscreteFuzzySet<T> fuzzySet)
        {
            if (fuzzySet.valueSet == null || fuzzySet.membershipDegreeSet == null)
                throw new InvalidOperationException("Sets cannot be null.");

            if (fuzzySet.valueSet.Count != fuzzySet.membershipDegreeSet.Count)
                throw new InvalidOperationException("valueSet and membershipDegreeSet must have the same length.");

            var (sortedValues, sortedDegrees) = MergeSortByMembershipDegreeRecursive<T>(
                fuzzySet.valueSet,
                fuzzySet.membershipDegreeSet);

            fuzzySet.valueSet = sortedValues;
            fuzzySet.membershipDegreeSet = sortedDegrees;
        }
        private static (List<T> values, List<float> degrees) MergeSortByMembershipDegreeRecursive<T>(
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

            var (sortedLeftValues, sortedLeftDegrees) = MergeSortByMembershipDegreeRecursive<T>(leftValues, leftDegrees);
            var (sortedRightValues, sortedRightDegrees) = MergeSortByMembershipDegreeRecursive<T>(rightValues, rightDegrees);

            return MergeByMembershipDegree(
                sortedLeftValues, sortedLeftDegrees,
                sortedRightValues, sortedRightDegrees);
        }


        private static (List<T> values, List<float> degrees) MergeByMembershipDegree<T>(
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

        public static void MergeSortByValue<T>(DiscreteFuzzySetDTO<T> fuzzySet)
            where T : IComparable<T>
        {
            if (fuzzySet.valueSet == null || fuzzySet.membershipDegreeSet == null)
                throw new InvalidOperationException("Sets cannot be null.");

            if (fuzzySet.valueSet.Count != fuzzySet.membershipDegreeSet.Count)
                throw new InvalidOperationException("valueSet and membershipDegreeSet must have the same length.");

            var (sortedValues, sortedDegrees) = MergeSortByValueRecursive<T>(
                fuzzySet.valueSet,
                fuzzySet.membershipDegreeSet);

            fuzzySet.valueSet = sortedValues;
            fuzzySet.membershipDegreeSet = sortedDegrees;
        }
        private static (List<T> values, List<float> degrees) MergeSortByValueRecursive<T>(
            List<T> values,
            List<float> degrees)
            where T : IComparable<T>
        {
            if (values.Count <= 1)
                return (values, degrees);

            int mid = values.Count / 2;

            var leftValues = values.GetRange(0, mid);
            var leftDegrees = degrees.GetRange(0, mid);
            var rightValues = values.GetRange(mid, values.Count - mid);
            var rightDegrees = degrees.GetRange(mid, degrees.Count - mid);

            var (sortedLeftValues, sortedLeftDegrees) = MergeSortByValueRecursive<T>(leftValues, leftDegrees);
            var (sortedRightValues, sortedRightDegrees) = MergeSortByValueRecursive<T>(rightValues, rightDegrees);

            return MergeByValue(
                sortedLeftValues, sortedLeftDegrees,
                sortedRightValues, sortedRightDegrees);
        }


        private static (List<T> values, List<float> degrees) MergeByValue<T>(
            List<T> leftValues, List<float> leftDegrees,
            List<T> rightValues, List<float> rightDegrees)
            where T:IComparable<T>
        {
            var mergedValues = new List<T>(leftValues.Count + rightValues.Count);
            var mergedDegrees = new List<float>(leftDegrees.Count + rightDegrees.Count);

            int i = 0, j = 0;

            while (i < leftValues.Count && j < rightValues.Count)
            {
                //if (leftValues[i] == null || rightValues[j] == null)
                //    throw new InvalidOperationException("An element x of fuzzy set membership degree function can't be null");
                if (leftValues[i].CompareTo(rightValues[j])==1)
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

            while (i < leftValues.Count)
            {
                //if (leftValues[i]==null)
                //    throw new InvalidOperationException("An element x of fuzzy set membership degree function can't be null");
                mergedValues.Add(leftValues[i]);
                mergedDegrees.Add(leftDegrees[i]);
                i++;
            }

            while (j < rightValues.Count)
            {
                //if (rightValues[j]==null)
                //    throw new InvalidOperationException("An element x of fuzzy set membership degree function can't be null");
                mergedValues.Add(rightValues[j]);
                mergedDegrees.Add(rightDegrees[j]);
                j++;
            }

            return (mergedValues, mergedDegrees);
        }

    }
}
