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
        static List<(List<T>, float)> createMassAssignment<T>(DiscreteFuzzySet<T> fs)
        {
            DiscreteFuzzySetSorter.MergeSort<T>(fs);
            List<T> tmpSubSet = new List<T>();
            float currentDegree = fs.membershipDegreeSet[0];
            List<(List<T>, float)> ans = new List<(List<T>, float)>();
            for (int i = 0; i < fs.valueSet.Count; ++i)
            {
                if (fs.membershipDegreeSet[i] == currentDegree)
                {
                    tmpSubSet.Add(fs.valueSet[i]);
                }
                else
                {
                    ans.Add(new(tmpSubSet, currentDegree));
                    tmpSubSet = new List<T>(tmpSubSet);
                    tmpSubSet.Add(fs.valueSet[i]);
                    currentDegree = fs.membershipDegreeSet[i];
                }
            }
            ans.Add((tmpSubSet, currentDegree));
            for (int i = 0; i < ans.Count - 1; ++i)
            {
                var tmp = ans[i];
                tmp.Item2 -= ans[i + 1].Item2;
                ans[i] = tmp;
            }
            return ans;


        }

        public static VoteCrispDefinition<T> createMassAssignment<T>(DiscreteFuzzySet<T> fs)
        {

        }

    }
}
