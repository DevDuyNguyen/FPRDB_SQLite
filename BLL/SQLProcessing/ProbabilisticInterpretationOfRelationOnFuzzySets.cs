using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public static class ProbabilisticInterpretationOfRelationOnFuzzySets
    {
        //not done: mocking for private
        public static (DiscreteFuzzySet<float>, DiscreteFuzzySet<float>) discretize2ContinuousFS(ContinuousFuzzySet fs1, ContinuousFuzzySet fs2)
        {
            int maxDiscretePoint = 100;
            int variance = 10;

            float fs1_left_bottom = fs1.getLeftBottom();
            float fs1_left_top = fs1.getLeftTop();
            float fs1_right_bottom = fs1.getRightBottom();
            float fs1_right_top = fs1.getRightTop();


            float fs2_left_bottom = fs2.getLeftBottom();
            float fs2_left_top = fs2.getLeftTop();
            float fs2_right_bottom = fs2.getRightBottom();
            float fs2_right_top = fs2.getRightTop();

            List<float> rfs1_values = new List<float>();
            List<float> rfs1_memberships = new List<float>();
            List<float> rfs2_values = new List<float>();
            List<float> rfs2_memberships = new List<float>();


            if (fs1.isDomainOverlapedWith(fs2)){
                float delta;
                if (CompareOperatorUltilities.floatNearlyEquals(fs1_left_bottom, fs2_left_bottom)){
                    delta = (fs1.getRightBottom() - fs1.getLeftBottom()) / (float)(maxDiscretePoint);
                }
                else
                {
                    delta = Math.Abs(fs1_left_bottom - fs2_left_bottom) / 2;
                    //if left_a is very close to left_b 
                    if (((fs1_right_bottom-fs1_left_bottom)/delta>maxDiscretePoint+variance)||((fs2_right_bottom - fs2_left_bottom) / delta > maxDiscretePoint + variance)){
                        if (fs1_left_bottom < fs2_left_bottom)
                        {
                            rfs1_values.Add(fs1_left_bottom);
                            rfs1_memberships.Add(fs1.getMembershipDegree(fs1_left_bottom));
                            rfs1_values.Add(fs2_left_bottom);
                            rfs1_memberships.Add(fs2.getMembershipDegree(fs2_left_bottom));
                            delta = (fs2_right_bottom - fs2_left_bottom) / maxDiscretePoint;
                            fs1_left_bottom = fs2_left_bottom;
                        }
                        else
                        {
                            rfs2_values.Add(fs2_left_bottom);
                            rfs2_memberships.Add(fs2.getMembershipDegree(fs2_left_bottom));
                            rfs2_values.Add(fs1_left_bottom);
                            rfs2_memberships.Add(fs1.getMembershipDegree(fs1_left_bottom));
                            delta = (fs1_right_bottom - fs1_left_bottom) / maxDiscretePoint;
                            fs2_left_bottom = fs1_left_bottom;
                        }
                    }
                    //if A and B has overlapping universe of discourse but distance betwee left_a and left_b is very long
                    if (((fs1_right_bottom - fs1_left_bottom) / delta < maxDiscretePoint - variance) || ((fs2_right_bottom - fs2_left_bottom) / delta < maxDiscretePoint - variance))
                    {
                        int n = 3;
                        while (((fs1_right_bottom - fs1_left_bottom) / delta < maxDiscretePoint - variance) || ((fs2_right_bottom - fs2_left_bottom) / delta < maxDiscretePoint - variance))
                        {
                            delta= Math.Abs(fs1_left_bottom - fs2_left_bottom) / n;
                            ++n;
                        }
                    }
                }
                for (; fs1_left_bottom <= fs1_right_bottom; fs1_left_bottom += delta)
                {
                    rfs1_values.Add(fs1_left_bottom);
                    rfs1_memberships.Add(fs1.getMembershipDegree(fs1_left_bottom));
                }
                for (; fs2_left_bottom <= fs2_right_bottom; fs2_left_bottom += delta)
                {
                    rfs2_values.Add(fs2_left_bottom);
                    rfs2_memberships.Add(fs2.getMembershipDegree(fs2_left_bottom));
                }

            }
            else
            {
                float epsilon1 = (fs1.getRightBottom() - fs1.getLeftBottom()) / (float)(maxDiscretePoint);
                float epsilon2 = (fs2.getRightBottom() - fs2.getLeftBottom()) / (float)(maxDiscretePoint);
                for (; fs1_left_bottom <= fs1_right_bottom; fs1_left_bottom += epsilon1)
                {
                    rfs1_values.Add(fs1_left_bottom);
                    rfs1_memberships.Add(fs1.getMembershipDegree(fs1_left_bottom));
                }
                for (; fs2_left_bottom <= fs2_right_bottom; fs2_left_bottom += epsilon2)
                {
                    rfs2_values.Add(fs2_left_bottom);
                    rfs2_memberships.Add(fs2.getMembershipDegree(fs2_left_bottom));
                }
            }
            return (new DiscreteFuzzySet<float>(rfs1_values, rfs1_memberships, null, FieldType.distFS_FLOAT), new DiscreteFuzzySet<float>(rfs2_values, rfs2_memberships, null, FieldType.distFS_FLOAT));
        }
        public static DiscreteFuzzySet<float> discretizeContinuousFSFromDiscreteFS<T>(ContinuousFuzzySet fs1, DiscreteFuzzySet<T> fs2) where T : INumber<T>{
            //discretization for continuous fuzzy set
            float fs1_left_bottom = fs1.getLeftBottom();
            float fs1_right_bottom = fs1.getRightBottom();
            int maxDiscretePoint = 100;
            float delta = (fs1_right_bottom - fs1_left_bottom) / maxDiscretePoint;
            List<float> fs1_Values = new List<float>();
            List<float> fs1_Memberships = new List<float>();
            for (; fs1_left_bottom <= fs1_right_bottom; fs1_left_bottom += delta)
            {
                fs1_Values.Add(fs1_left_bottom);
                fs1_Memberships.Add(fs1.getMembershipDegree(fs1_left_bottom));
            }
            //add the discrete set's universe of discourse to the discretized continuous fuzzy set
            float tmp_v;
            foreach (T v in fs2.valueSet)
            {
                tmp_v = Convert.ToSingle(v);
                if (fs1_Values.BinarySearch(tmp_v)<0){
                    fs1_Values.Add(tmp_v);
                    fs1_Memberships.Add(fs1.getMembershipDegree(tmp_v));
                }
            }
            return new DiscreteFuzzySet<float>(fs1_Values, fs1_Memberships, null, FieldType.distFS_FLOAT);

        }
        //not done: mocking for private
        public static float equalDistcreteFuzzySets<T>(DiscreteFuzzySet<T> fs1, DiscreteFuzzySet<T>  fs2) where T : IComparable<T>
        {

            List<VoteCrispDefinition<T>> massAssignMentsFS1 = MassAssignment.createMassAssignment<T>(fs1);
            List<VoteCrispDefinition<T>> massAssignMentsFS2 = MassAssignment.createMassAssignment<T>(fs2);
            float ans = 0.0f;

            for (int i = 0; i < massAssignMentsFS1.Count; ++i)
            {
                for (int j = 0; j < massAssignMentsFS2.Count; ++j)
                {
                    ans += ProbabilisticInterpretationOfRelationOnSets.compare<T>(massAssignMentsFS1[i].subSet, massAssignMentsFS2[j].subSet, CompareOperation.EQUAL) * massAssignMentsFS1[i].mass * massAssignMentsFS2[j].mass;
                    //float tmp1 = ProbabilisticInterpretationOfRelationOnSets.compare<T>(massAssignMentsFS1[i].subSet, massAssignMentsFS2[j].subSet, CompareOperation.EQUAL);
                    //float tmp2 = massAssignMentsFS1[i].mass;
                    //float tmp3 = massAssignMentsFS2[j].mass;
                    //ans += tmp1 * tmp2 * tmp3;
                }
            }
            return ans;
        }
        public static float noEqualDistcreteFuzzySets<T>(DiscreteFuzzySet<T> fs1, DiscreteFuzzySet<T>  fs2) where T : IComparable<T>
        {

            List<VoteCrispDefinition<T>> massAssignMentsFS1 = MassAssignment.createMassAssignment<T>(fs1);
            List<VoteCrispDefinition<T>> massAssignMentsFS2 = MassAssignment.createMassAssignment<T>(fs2);
            float ans = 0.0f;

            for (int i = 0; i < massAssignMentsFS1.Count; ++i)
            {
                for (int j = 0; j < massAssignMentsFS2.Count; ++j)
                {
                    //ans += ProbabilisticInterpretationOfRelationOnSets.compare<T>(massAssignMentsFS1[i].subSet, massAssignMentsFS2[j].subSet, CompareOperation.NOT_EQUAL) * massAssignMentsFS1[i].mass * massAssignMentsFS2[j].mass;
                    float tmp1 = ProbabilisticInterpretationOfRelationOnSets.compare<T>(massAssignMentsFS1[i].subSet, massAssignMentsFS2[j].subSet, CompareOperation.NOT_EQUAL);
                    float tmp2 = massAssignMentsFS1[i].mass;
                    float tmp3 = massAssignMentsFS2[j].mass;
                    ans += tmp1 * tmp2 * tmp3;
                }
            }
            return ans;
        }
        public static float compareFuzzySet<T>(FuzzySet<T> fs1, FuzzySet<T> fs2, CompareOperation operation) where T : IComparable<T>
        {
            throw new NotImplementedException();

            //DiscreteFuzzySet<T> fs1 = nfs1.ToDiscreteFuzzySet();
            //DiscreteFuzzySet<T> fs2 = nfs2.ToDiscreteFuzzySet();

            //List<(List<T>, float)> massAssignMentsFS1 = MassAssignment.createMassAssignment<T>(fs1);
            //List<(List<T>, float)> massAssignMentsFS2 = createMassAssignment<T>(fs2);
            //float ans = 0.0f;

            //for (int i = 0; i < massAssignMentsFS1.Count; ++i)
            //{
            //    for (int j = 0; j < massAssignMentsFS2.Count; ++j)
            //    {
            //        ans += ProbabilisticInterpretationOfRelationOnSets.compare<T>(massAssignMentsFS1[i].Item1, massAssignMentsFS2[j].Item1, compOperator) * massAssignMentsFS1[i].Item2 * massAssignMentsFS2[j].Item2; ;
            //        //float tmp1 = ProbabilisticInterpretationOfRelationOnSets.compare<T>(massAssignMentsFS1[i].Item1, massAssignMentsFS2[j].Item1, compOperator);
            //        //float tmp2 = massAssignMentsFS1[i].Item2;
            //        //float tmp3 = massAssignMentsFS2[j].Item2;
            //        //float tmp = tmp1 * tmp2 * tmp3;
            //    }
            //}
            //return ans;

        }

        //public static float compare<T>(FuzzySet<T> nfs1, FuzzySet<T> nfs2, CompareOperation compOperator) where T : IComparable<T>
        //{
        //    throw new NotImplementedException();
        //    DiscreteFuzzySet<T> fs1 = nfs1.ToDiscreteFuzzySet();
        //    DiscreteFuzzySet<T> fs2 = nfs2.ToDiscreteFuzzySet();

        //    List<(List<T>, float)> massAssignMentsFS1 = MassAssignment.createMassAssignment<T>(fs1);
        //    List<(List<T>, float)> massAssignMentsFS2 = createMassAssignment<T>(fs2);
        //    float ans = 0.0f;

        //    for (int i = 0; i < massAssignMentsFS1.Count; ++i)
        //    {
        //        for (int j = 0; j < massAssignMentsFS2.Count; ++j)
        //        {
        //            ans += ProbabilisticInterpretationOfRelationOnSets.compare<T>(massAssignMentsFS1[i].Item1, massAssignMentsFS2[j].Item1, compOperator) * massAssignMentsFS1[i].Item2 * massAssignMentsFS2[j].Item2; ;
        //            //float tmp1 = ProbabilisticInterpretationOfRelationOnSets.compare<T>(massAssignMentsFS1[i].Item1, massAssignMentsFS2[j].Item1, compOperator);
        //            //float tmp2 = massAssignMentsFS1[i].Item2;
        //            //float tmp3 = massAssignMentsFS2[j].Item2;
        //            //float tmp = tmp1 * tmp2 * tmp3;
        //        }
        //    }
        //    return ans;
        //}
        public static float also<T>(FuzzySet<T> nfs1, FuzzySet<T> nfs2) where T : IComparable<T>
        {

            throw new NotImplementedException();
            //DiscreteFuzzySet<T> fs1 = nfs1.ToDiscreteFuzzySet();
            //DiscreteFuzzySet<T> fs2 = nfs2.ToDiscreteFuzzySet();

            //List<(List<T>, float)> massAssignMentsFS1 = getMassAssignments<T>(fs1);
            //List<(List<T>, float)> massAssignMentsFS2 = getMassAssignments<T>(fs2);
            //float ans = 0.0f;

            //for (int i = 0; i < massAssignMentsFS1.Count; ++i)
            //{
            //    for (int j = 0; j < massAssignMentsFS2.Count; ++j)
            //    {
            //        //ans += ProbabilisticInterpretationOfRelationOnSets.subset<T>(massAssignMentsFS1[i].Item1, massAssignMentsFS2[j].Item1) * massAssignMentsFS1[i].Item2 * massAssignMentsFS2[j].Item2; ;
            //        float tmp1 = ProbabilisticInterpretationOfRelationOnSets.subset<T>(massAssignMentsFS1[i].Item1, massAssignMentsFS2[j].Item1);
            //        float tmp2 = massAssignMentsFS1[i].Item2;
            //        float tmp3 = massAssignMentsFS2[j].Item2;
            //        ans += tmp1 * tmp2 * tmp3;
            //    }
            //}
            //return ans;
        }

    }
}
