using BLL.Common;
using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class MassAssignment
    {
        //static List<(List<T>, float)> createMassAssignment<T>(DiscreteFuzzySet<T> fs)
        //{
        //    DiscreteFuzzySetSorter.MergeSort<T>(fs);
        //    List<T> tmpSubSet = new List<T>();
        //    float currentDegree = fs.membershipDegreeSet[0];
        //    List<(List<T>, float)> ans = new List<(List<T>, float)>();
        //    for (int i = 0; i < fs.valueSet.Count; ++i)
        //    {
        //        if (fs.membershipDegreeSet[i] == currentDegree)
        //        {
        //            tmpSubSet.Add(fs.valueSet[i]);
        //        }
        //        else
        //        {
        //            ans.Add(new(tmpSubSet, currentDegree - fs.membershipDegreeSet[i]));
        //            tmpSubSet = new List<T>(tmpSubSet);
        //            tmpSubSet.Add(fs.valueSet[i]);
        //            currentDegree = fs.membershipDegreeSet[i];
        //        }
        //    }
        //    ans.Add((tmpSubSet, currentDegree));
        //    return ans;
        //}

        public static List<VoteCrispDefinition<T>> createMassAssignment<T>(DiscreteFuzzySet<T> fs)
        {
            DiscreteFuzzySetSorter.MergeSort<T>(fs);
            List<T> tmpSubSet = new List<T>();
            float currentDegree = fs.membershipDegreeSet[0];
            List<VoteCrispDefinition<T>> ans = new List<VoteCrispDefinition<T>>();
            for (int i = 0; i < fs.valueSet.Count; ++i)
            {
                
                if (fs.membershipDegreeSet[i] == currentDegree)
                {
                    tmpSubSet.Add(fs.valueSet[i]);
                }
                else
                {
                    ans.Add(new VoteCrispDefinition<T>(tmpSubSet, currentDegree - fs.membershipDegreeSet[i]));
                    tmpSubSet = new List<T>(tmpSubSet);
                    tmpSubSet.Add(fs.valueSet[i]);
                    currentDegree = fs.membershipDegreeSet[i];
                    if (fs.membershipDegreeSet[i] == 0)
                        break;
                }
            }
            ans.Add(new VoteCrispDefinition<T>(tmpSubSet, currentDegree));
            return ans;

        }

    }
}
