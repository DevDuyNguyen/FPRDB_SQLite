using BLL.DomainObject;
using BLL.Interfaces;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Common
{
    public abstract class FuzzyProbabilisticValueUtilities
    {
        //not done:mocking for private
        public static FuzzyProbabilisticValue<T> turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<T>(FuzzyProbabilisticValueParsingData data, FieldType fieldType, MetadataManager metaDataMgr)
        {
            FuzzyProbabilisticValue<T> ans;
            //extract FieldType domain
            FieldType domain;
            FieldType fuzzSetType;
            Type t = typeof(T);
            if (
                ((fieldType == FieldType.INT || fieldType == FieldType.distFS_INT) && t != typeof(int))
                || ((fieldType == FieldType.FLOAT || fieldType == FieldType.contFS || fieldType == FieldType.distFS_FLOAT) && t != typeof(float))
                || ((fieldType == FieldType.VARCHAR || fieldType == FieldType.CHAR) && t != typeof(string))
                || ((fieldType == FieldType.BOOLEAN) && t != typeof(bool))
            )
            {
                throw new NotSupportedException($"Field type {fieldType.ToString()} isn't compatible with fuzzy probabilistic values of domain {t.Name}");
            }
            if (t == typeof(int))
            {
                domain = FieldType.INT;
            }
            else if (t == typeof(float))
            {
                domain = FieldType.FLOAT;
            }
            else if (t == typeof(string))
            {
                domain = FieldType.VARCHAR;
            }
            else if (t == typeof(bool))
            {
                domain = FieldType.BOOLEAN;
            }
            else
            {
                throw new NotSupportedException($"{typeof(T)} isn't supported");
            }
            //extract List<FuzzySet<T>> valueList
            List<FuzzySet<T>> valueList = new List<FuzzySet<T>>();
            foreach (Constant c in data.valueList)
            {
                valueList.Add(FuzzySetUltilities.turnConstantToFuzzySet<T>(c, metaDataMgr));
            }
            //extract lower bound, upper boud
            List<float> lowerBounds = new List<float>();
            List<float> upperBounds = new List<float>();
            for (int i = 0; i < data.intervalProbUpperBoundList.Count; ++i)
            {
                lowerBounds.Add(data.intervalProbLowerBoundList[i]);
                upperBounds.Add(data.intervalProbUpperBoundList[i]);
            }
            return new FuzzyProbabilisticValue<T>(domain, valueList, lowerBounds, upperBounds);

        }
        public static List<(string, string, string)> extractValuesIntervalProbabilitiesAsString(string strFProbValue)
        {
            List<(string, string, string)> ans = new List<(string, string, string)>();
            // Pattern: Look for '(', then grab everything until ')', non-greedily
            string possibleValuePattern = @"\((.*?)\)";
            string valuePattern = @"^(.*?),";
            string intervalProbPattern = @"\[([+-]?\d+(?:\.\d+)?),([+-]?\d+(?:\.\d+)?)\]";
            string possibleValue, value, upperProb, lowerProb;
            Match valueMatch, intervalProbMatch;

            MatchCollection matches = Regex.Matches(strFProbValue, possibleValuePattern);

            foreach (Match match in matches)
            {
                possibleValue = match.Groups[1].Value;
                valueMatch = Regex.Match(possibleValue, valuePattern);
                intervalProbMatch = Regex.Match(possibleValue, intervalProbPattern);
                value = valueMatch.Groups[1].Value;
                lowerProb = intervalProbMatch.Groups[1].Value;
                upperProb = intervalProbMatch.Groups[2].Value;
                ans.Add((value, lowerProb, upperProb));
            }
            return ans;
        }


    }
}
