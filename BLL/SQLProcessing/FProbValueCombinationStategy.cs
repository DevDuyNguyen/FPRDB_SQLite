using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Enums;
using BLL.Common;
using System.Runtime.Intrinsics;

namespace BLL.SQLProcessing
{
    public static class FProbValueCombinationStategy
    {
        public static FuzzyProbabilisticValue<T> conjunction<T>(FuzzyProbabilisticValue<T> v1, FuzzyProbabilisticValue<T> v2, ProbabilisticCombinationStrategy probCombStrategy)
        {
            FuzzySet<T> fs1, fs2, fs;
            List<float> intervalProbabilityConjunction;


            List<FuzzySet<T>> ans_valueList=new List<FuzzySet<T>>();
            List<float> ans_intervalProbLowerBoundList= new List<float>();
            List<float> ans_intervalProbUpperBoundList=new List<float>();

            if (!ProbabilisticCombinationStrategyUtilities.isConjunctionStategy(probCombStrategy))
                throw new InvalidDataException("Probabilistic combination strategy must be conjunction");


            for (int i=0; i<v1.valueList.Count; ++i)
            {
                for(int j=0; j<v2.valueList.Count; ++j)
                {
                    fs1 = v1.valueList[i];
                    fs2 = v2.valueList[j];
                    fs = fs1.StandardIntersection(fs2);
                    intervalProbabilityConjunction = ProbabilisticCombinationStrategyUtilities.combine(v1.intervalProbLowerBoundList[i], v1.intervalProbUpperBoundList[i], v2.intervalProbLowerBoundList[j], v2.intervalProbUpperBoundList[j], probCombStrategy);
                    if(fs.isNormal() && (intervalProbabilityConjunction[0]>0 || intervalProbabilityConjunction[1] > 0))
                    {
                        ans_valueList.Add(fs);
                        ans_intervalProbLowerBoundList.Add(intervalProbabilityConjunction[0]);
                        ans_intervalProbUpperBoundList.Add(intervalProbabilityConjunction[1]);
                    }
                }
            }
            
            for(int i=0; i<ans_valueList.Count-1; ++i)
            {
                for(int j=i+1; j<ans_valueList.Count; ++j)
                {
                    if (ans_valueList[i].isEqualTo(ans_valueList[j]))
                    {
                        intervalProbabilityConjunction = ProbabilisticCombinationStrategyUtilities.combine(ans_intervalProbLowerBoundList[i], ans_intervalProbUpperBoundList[i], ans_intervalProbLowerBoundList[j], ans_intervalProbUpperBoundList[j], ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION);
                        ans_valueList.RemoveAt(j);
                        ans_intervalProbLowerBoundList.RemoveAt(j);
                        ans_intervalProbUpperBoundList.RemoveAt(j);
                        --j;
                        ans_intervalProbLowerBoundList[i] = intervalProbabilityConjunction[0];
                        ans_intervalProbUpperBoundList[i] = intervalProbabilityConjunction[1];

                    }
                }
            }


            return new FuzzyProbabilisticValue<T>(v1.domain, ans_valueList, ans_intervalProbLowerBoundList, ans_intervalProbUpperBoundList);
        }
        public static FuzzyProbabilisticValue<T> disjunction<T>(FuzzyProbabilisticValue<T> v1, FuzzyProbabilisticValue<T> v2,ProbabilisticCombinationStrategy probCombStrategy)
        {
            List<bool> isV1ValueListIntersectedNormalHeight=new List<bool>();
            for (int i = 0; i < v1.valueList.Count; ++i)
                isV1ValueListIntersectedNormalHeight.Add(false);
            List<bool> isV2ValueListIntersectedNormalHeight = new List<bool>();
            for (int i = 0; i < v2.valueList.Count; ++i)
                isV2ValueListIntersectedNormalHeight.Add(false);
            FuzzySet<T> fs1, fs2, fs;
            List<float> intervalProbabilityConjunction;


            List<FuzzySet<T>> ans_valueList = new List<FuzzySet<T>>();
            List<float> ans_intervalProbLowerBoundList = new List<float>();
            List<float> ans_intervalProbUpperBoundList = new List<float>();

            if (!ProbabilisticCombinationStrategyUtilities.isDisjunctionStategy(probCombStrategy))
                throw new InvalidDataException("Probabilistic combination strategy must be disjunction");


            for (int i = 0; i < v1.valueList.Count; ++i)
            {
                for (int j = 0; j < v2.valueList.Count; ++j)
                {
                    fs1 = v1.valueList[i];
                    fs2 = v2.valueList[j];
                    fs = fs1.StandardIntersection(fs2);
                    intervalProbabilityConjunction = ProbabilisticCombinationStrategyUtilities.combine(v1.intervalProbLowerBoundList[i], v1.intervalProbUpperBoundList[i], v2.intervalProbLowerBoundList[j], v2.intervalProbUpperBoundList[j], probCombStrategy);
                    if (fs.isNormal())
                    {
                        isV1ValueListIntersectedNormalHeight[i] = isV2ValueListIntersectedNormalHeight[j] = true;
                        if ((intervalProbabilityConjunction[0] > 0 || intervalProbabilityConjunction[1] > 0))
                        {
                            ans_valueList.Add(fs);
                            ans_intervalProbLowerBoundList.Add(intervalProbabilityConjunction[0]);
                            ans_intervalProbUpperBoundList.Add(intervalProbabilityConjunction[1]);
                        }
                    }
                    
                }
            }
            for (int i = 0; i < ans_valueList.Count - 1; ++i)
            {
                for (int j = i + 1; j < ans_valueList.Count; ++j)
                {
                    if (ans_valueList[i].isEqualTo(ans_valueList[j]))
                    {
                        intervalProbabilityConjunction = ProbabilisticCombinationStrategyUtilities.combine(ans_intervalProbLowerBoundList[i], ans_intervalProbUpperBoundList[i], ans_intervalProbLowerBoundList[j], ans_intervalProbUpperBoundList[j], ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION);
                        ans_valueList.RemoveAt(j);
                        ans_intervalProbLowerBoundList.RemoveAt(j);
                        ans_intervalProbUpperBoundList.RemoveAt(j);
                        --j;
                        ans_intervalProbLowerBoundList[i] = intervalProbabilityConjunction[0];
                        ans_intervalProbUpperBoundList[i] = intervalProbabilityConjunction[1];

                    }
                }
            }
            for(int i=0; i<isV1ValueListIntersectedNormalHeight.Count; ++i)
            {
                if (isV1ValueListIntersectedNormalHeight[i] == false)
                {
                    ans_valueList.Add(v1.valueList[i]);
                    ans_intervalProbLowerBoundList.Add(v1.intervalProbLowerBoundList[i]);
                    ans_intervalProbUpperBoundList.Add(v1.intervalProbUpperBoundList[i]);
                }
            }
            for (int i = 0; i < isV2ValueListIntersectedNormalHeight.Count; ++i)
            {
                if (isV1ValueListIntersectedNormalHeight[i] == false)
                {
                    ans_valueList.Add(v2.valueList[i]);
                    ans_intervalProbLowerBoundList.Add(v2.intervalProbLowerBoundList[i]);
                    ans_intervalProbUpperBoundList.Add(v2.intervalProbUpperBoundList[i]);
                }
            }
            return new FuzzyProbabilisticValue<T>(v1.domain, ans_valueList, ans_intervalProbLowerBoundList, ans_intervalProbUpperBoundList);


        }
        public static FuzzyProbabilisticValue<T> difference<T>(FuzzyProbabilisticValue<T> v1, FuzzyProbabilisticValue<T> v2, ProbabilisticCombinationStrategy probCombStrategy)
        {

            List<bool> isV1ValueListIntersectedNormalHeight = new List<bool>();
            for (int i = 0; i < v1.valueList.Count; ++i)
                isV1ValueListIntersectedNormalHeight.Add(false);
            //List<bool> isV2ValueListIntersectedNormalHeight = new List<bool>();
            //for (int i = 0; i < v2.valueList.Count; ++i)
            //    isV2ValueListIntersectedNormalHeight.Add(false);
            FuzzySet<T> fs1, fs2, fs;
            List<float> intervalProbabilityConjunction;


            List<FuzzySet<T>> ans_valueList = new List<FuzzySet<T>>();
            List<float> ans_intervalProbLowerBoundList = new List<float>();
            List<float> ans_intervalProbUpperBoundList = new List<float>();

            if (!ProbabilisticCombinationStrategyUtilities.isDifferenceStategy(probCombStrategy))
                throw new InvalidDataException("Probabilistic combination strategy must be difference");


            for (int i = 0; i < v1.valueList.Count; ++i)
            {
                for (int j = 0; j < v2.valueList.Count; ++j)
                {
                    fs1 = v1.valueList[i];
                    fs2 = v2.valueList[j];
                    fs = fs1.StandardIntersection(fs2);
                    intervalProbabilityConjunction = ProbabilisticCombinationStrategyUtilities.combine(v1.intervalProbLowerBoundList[i], v1.intervalProbUpperBoundList[i], v2.intervalProbLowerBoundList[j], v2.intervalProbUpperBoundList[j], probCombStrategy);
                    if (fs.isNormal())
                    {
                        //isV1ValueListIntersectedNormalHeight[i] = isV2ValueListIntersectedNormalHeight[j] = true;
                        isV1ValueListIntersectedNormalHeight[i] = true;
                        if (intervalProbabilityConjunction[0] > 0 || intervalProbabilityConjunction[1] > 0)
                        {
                            ans_valueList.Add(fs);
                            ans_intervalProbLowerBoundList.Add(intervalProbabilityConjunction[0]);
                            ans_intervalProbUpperBoundList.Add(intervalProbabilityConjunction[1]);
                        }
                    }
                    
                }
            }
            for (int i = 0; i < ans_valueList.Count - 1; ++i)
            {
                for (int j = i + 1; j < ans_valueList.Count; ++j)
                {
                    if (ans_valueList[i].isEqualTo(ans_valueList[j]))
                    {
                        intervalProbabilityConjunction = ProbabilisticCombinationStrategyUtilities.combine(ans_intervalProbLowerBoundList[i], ans_intervalProbUpperBoundList[i], ans_intervalProbLowerBoundList[j], ans_intervalProbUpperBoundList[j], ProbabilisticCombinationStrategy.DISJUNCTION_MUTUAL_EXCLUSION);
                        ans_valueList.RemoveAt(j);
                        ans_intervalProbLowerBoundList.RemoveAt(j);
                        ans_intervalProbUpperBoundList.RemoveAt(j);
                        --j;
                        ans_intervalProbLowerBoundList[i] = intervalProbabilityConjunction[0];
                        ans_intervalProbUpperBoundList[i] = intervalProbabilityConjunction[1];

                    }
                }
            }
            for (int i = 0; i < isV1ValueListIntersectedNormalHeight.Count; ++i)
            {
                if (isV1ValueListIntersectedNormalHeight[i] == false)
                {
                    ans_valueList.Add(v1.valueList[i]);
                    ans_intervalProbLowerBoundList.Add(v1.intervalProbLowerBoundList[i]);
                    ans_intervalProbUpperBoundList.Add(v1.intervalProbUpperBoundList[i]);
                }
            }
            //for (int i = 0; i < isV2ValueListIntersectedNormalHeight.Count; ++i)
            //{
            //    if (isV1ValueListIntersectedNormalHeight[i] == false)
            //    {
            //        ans_valueList.Add(v2.valueList[i]);
            //        ans_intervalProbLowerBoundList.Add(v2.intervalProbLowerBoundList[i]);
            //        ans_intervalProbUpperBoundList.Add(v2.intervalProbUpperBoundList[i]);
            //    }
            //}
            return new FuzzyProbabilisticValue<T>(v1.domain, ans_valueList, ans_intervalProbLowerBoundList, ans_intervalProbUpperBoundList);
        }


    }
}
