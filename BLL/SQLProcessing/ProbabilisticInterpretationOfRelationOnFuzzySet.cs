using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Common;
using BLL.Enums;

namespace BLL.SQLProcessing
{
    public static class ProbabilisticInterpretationOfRelationOnFuzzySet
    {
        static List<(List<T>,float)> getMassAssignments<T>(DiscreteFuzzySet<T> fs)
        {
            DiscreteFuzzySetSorter.MergeSort<T>(fs);
            List<T> tmpSubSet = new List<T>();
            float currentDegree = fs.membershipDegreeSet[0];
            List<(List<T>, float)> ans =new List<(List<T>, float)>();
            for(int i=0; i<fs.valueSet.Count; ++i)
            {
                if (fs.membershipDegreeSet[i] == currentDegree)
                {
                    tmpSubSet.Add(fs.valueSet[i]);
                }
                else
                {
                    ans.Add(new (tmpSubSet, currentDegree));
                    tmpSubSet = new List<T>(tmpSubSet);
                    tmpSubSet.Add(fs.valueSet[i]);
                    currentDegree = fs.membershipDegreeSet[i];
                }
            }
            ans.Add((tmpSubSet, currentDegree));
            for(int i=0; i<ans.Count-1; ++i)
            {
                var tmp = ans[i];
                tmp.Item2 -= ans[i + 1].Item2;
                ans[i] = tmp;
            }
            return ans;


        }
        public static float compare<T>(FuzzySet<T> nfs1, FuzzySet<T> nfs2, CompareOperation compOperator) where T : IComparable<T>
        {
            DiscreteFuzzySet<T> fs1 = nfs1.ToDiscreteFuzzySet();
            DiscreteFuzzySet<T> fs2 = nfs2.ToDiscreteFuzzySet();

            List<(List<T>, float)> massAssignMentsFS1 = getMassAssignments<T>(fs1);
            List<(List<T>, float)> massAssignMentsFS2 = getMassAssignments<T>(fs2);
            float ans = 0.0f;
            
            for(int i=0; i<massAssignMentsFS1.Count; ++i)
            {
                for (int j = 0; j < massAssignMentsFS2.Count; ++j)
                {
                    ans += ProbabilisticInterpretationOfRelationOnSets.compare<T>(massAssignMentsFS1[i].Item1, massAssignMentsFS2[j].Item1, compOperator) * massAssignMentsFS1[i].Item2 * massAssignMentsFS2[j].Item2; ;
                    //float tmp1 = ProbabilisticInterpretationOfRelationOnSets.compare<T>(massAssignMentsFS1[i].Item1, massAssignMentsFS2[j].Item1, compOperator);
                    //float tmp2 = massAssignMentsFS1[i].Item2;
                    //float tmp3 = massAssignMentsFS2[j].Item2;
                    //float tmp = tmp1 * tmp2 * tmp3;
                }
            }
            return ans;
        }
        public static float also<T>(FuzzySet<T> nfs1, FuzzySet<T> nfs2) where T : IComparable<T>
        {
            DiscreteFuzzySet<T> fs1 = nfs1.ToDiscreteFuzzySet();
            DiscreteFuzzySet<T> fs2 = nfs2.ToDiscreteFuzzySet();

            List<(List<T>, float)> massAssignMentsFS1 = getMassAssignments<T>(fs1);
            List<(List<T>, float)> massAssignMentsFS2 = getMassAssignments<T>(fs2);
            float ans = 0.0f;

            for (int i = 0; i < massAssignMentsFS1.Count; ++i)
            {
                for (int j = 0; j < massAssignMentsFS2.Count; ++j)
                {
                    //ans += ProbabilisticInterpretationOfRelationOnSets.subset<T>(massAssignMentsFS1[i].Item1, massAssignMentsFS2[j].Item1) * massAssignMentsFS1[i].Item2 * massAssignMentsFS2[j].Item2; ;
                    float tmp1 = ProbabilisticInterpretationOfRelationOnSets.subset<T>(massAssignMentsFS1[i].Item1, massAssignMentsFS2[j].Item1);
                    float tmp2 = massAssignMentsFS1[i].Item2;
                    float tmp3 = massAssignMentsFS2[j].Item2;
                    ans += tmp1 * tmp2 * tmp3;
                }
            }
            return ans;
        }

    }
}
