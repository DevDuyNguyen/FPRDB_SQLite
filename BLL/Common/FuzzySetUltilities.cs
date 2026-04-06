using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.SQLProcessing;

namespace BLL.Common
{
    public class FuzzySetUltilities
    {
        public static FuzzySet<T> turnConstantToFuzzySet<T>(Constant c, MetadataManager metaDataMgr)
        {
            Type t = typeof(T);
            if (
                (c is IntConstant && (t != typeof(int) && t != typeof(float)))
                || (c is FloatConstant && (t != typeof(float)))
                || (c is StringConstant && (t != typeof(string)))
                || (c is BooleanConstant && (t != typeof(bool)))
            )
            {
                throw new InvalidCastException($"Can't turn constant of type {c.GetType().Name} to fuzzy set with defining domain of {t.Name}");
            }
            if (ConstantUltilities.isPrimitiveConstant(c))
            {
                T value = (T)Convert.ChangeType(c.getVal(), typeof(T));
                //if (typeof(T) == typeof(float))
                //    value=(T)(Convert.ToSingle(c.getVal()));
                //else
                //    value = (T)c.getVal();
                List<T> valueSet = new List<T> { value };
                List<float> membershipDegreeSet = new List<float> { 1.0f };
                string fuzzySetName = value.ToString();
                FieldType fuzzSetType;
                if (c is IntConstant)
                    fuzzSetType = FieldType.distFS_INT;
                else if (c is FloatConstant)
                    fuzzSetType = FieldType.distFS_FLOAT;
                else if (c is StringConstant)
                {
                    fuzzSetType = FieldType.distFS_TEXT;
                    fuzzySetName = $"\"{fuzzySetName}\"";
                    //valueSet[0] = (T)Convert.ChangeType($"\"{valueSet[0]}\"",typeof(T));
                }
                else
                    fuzzSetType = FieldType.BOOLEAN;
                return new DiscreteFuzzySet<T>(valueSet, membershipDegreeSet, fuzzySetName, fuzzSetType, -1);
            }
            else
            {
                //string fsName = (string)(c.getVal());
                //int fsOID = metaDataMgr.getFuzzySetOID(fsName);
                //if (fsOID == -1)
                //{
                //    return new DiscreteFuzzySet<string>(valueSet, membershipDegreeSet, fuzzySetName, fuzzSetType);
                //}
                //else
                //{
                //    FuzzySetConstant fuzz_c = (FuzzySetConstant)c;
                //    FieldType fuzzSetType = metaDataMgr.getFuzzySetType((string)fuzz_c.getVal());
                //    if (FieldTypeUltilities.isContinuousFuzzySet(fuzzSetType))
                //        return metaDataMgr.getFuzzySet<T>((string)c.getVal(), FieldType.contFS);
                //    else
                //    {
                //        return metaDataMgr.getFuzzySet<T>((string)c.getVal(), fuzzSetType);
                //    }
                //}
                FuzzySetConstant fuzz_c = (FuzzySetConstant)c;
                FieldType fuzzSetType = metaDataMgr.getFuzzySetType((string)fuzz_c.getVal());
                if (FieldTypeUtilities.isContinuousFuzzySet(fuzzSetType))
                    return metaDataMgr.getFuzzySet<T>((string)c.getVal(), FieldType.contFS);
                else
                {
                    return metaDataMgr.getFuzzySet<T>((string)c.getVal(), fuzzSetType);
                }


            }

        }
        //public static BaseFuzzySet turnConstantToBaseFuzzySet(Constant c, MetadataManager metaDataMgr)
        //{
        //    if (c is IntConstant)
        //        return turnConstantToFuzzySet<int>(c, metaDataMgr);
        //    else if (c is FloatConstant)
        //        return turnConstantToFuzzySet<float>(c, metaDataMgr);
        //    else if (c is StringConstant)
        //        return turnConstantToFuzzySet<string>(c, metaDataMgr);
        //    else if (c is BooleanConstant)
        //        return turnConstantToFuzzySet<bool>(c, metaDataMgr);
        //    else if (c is FuzzySetConstant)
        //        return turnConstantToFuzzySet<bool>(c, metaDataMgr);
        //}
        public static DiscreteFuzzySet<float> turnINTDistcreteFuzzySetToFLOATDistcreteFuzzySet(DiscreteFuzzySet<int> fs)
        {
            List<float> values = new List<float>();
            foreach(int v in fs.valueSet)
            {
                values.Add(Convert.ToSingle(v));
            }
            return new DiscreteFuzzySet<float>(values, fs.membershipDegreeSet, fs.getName(), FieldType.distFS_FLOAT, fs.getOID());
        }
        public static Type getDefiningDomain(BaseFuzzySet fs)
        {
            if (fs is FuzzySet<int>)
                return typeof(int);
            else if (fs is FuzzySet<float>)
                return typeof(float);
            else if (fs is FuzzySet<string>)
                return typeof(string);
            else //if (fs is FuzzySet<bool>)
                return typeof(bool);

        }
    }
}
