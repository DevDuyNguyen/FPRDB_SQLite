using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainObject;
using BLL.Exceptions;
using BLL.Interfaces;
namespace BLL.SQLProcessing
{
    public class RelationScan:Scan
    {
        private FPRDBRelation relationInfo;
        private int currentTupleIndex;
        private List<FuzzyProbabilisticValue<object>> currentTuple;
        private DatabaseManager dbMgr;
        private MetadataManager metaDataMgr;
        private RecursiveDescentParser parser;
        public RelationScan(string relName, RecursiveDescentParser parser)
        {
            this.relationInfo = this.metaDataMgr.getRelation(relName);
            this.parser = parser;
            this.currentTupleIndex = 0;
        }
        public void beforeFirst()=> this.currentTupleIndex = 0;
        public bool hasField(string fldname)
        {
            List<Field> fields= this.relationInfo.getSchema().getFields();
            foreach(Field field in fields)
            {
                if (field.getFieldName() == fldname)
                    return true;
            }
            return false;
        }
        private int getFieldIndexInTuple(string fldName)
        {
            List<Field> fields = this.relationInfo.getSchema().getFields();
            for (int i=0; i<fields.Count; ++i)
            {
                if (fields[i].getFieldName() == fldName)
                    return i;
            }
            return -1;
        }
        public FuzzyProbabilisticValue<T> getFieldContent<T>(String fldName)
        {
            int index = getFieldIndexInTuple(fldName);
            if (index == -1)
                throw new QueryDataNotExistException($"Relation {this.relationInfo.getRelName()} doesn't have attribute {fldName}");
            var fprobValue = this.currentTuple[index];
            if (!(fprobValue is FuzzyProbabilisticValue<T>))
                throw new InvalidCastException($"Fuzzy probabilistic value of {fldName} doesn't contain fuzzy sets defined on domain of {typeof(T).Name}");
            return (FuzzyProbabilisticValue<T>)(object)fprobValue;

        }
        private FuzzySet<T> turnConstantToFuzzySet<T>(Constant c, FieldType fuzzSetType)
        {
            if(c is IntConstant || c is FloatConstant || c is StringConstant)
            {
                T value = (T)c.getVal();
                List<T> valueSet = new List<T> { value };
                List<float> membershipDegreeSet = new List<float> { 1.0f };
                string fuzzySetName = value.ToString();
                if (fuzzSetType == FieldType.distFS_INT || fuzzSetType == FieldType.distFS_FLOAT || fuzzSetType == FieldType.distFS_TEXT)
                {
                    return new DiscreteFuzzySet<T>(valueSet, membershipDegreeSet, fuzzySetName, fuzzSetType);
                }
                else
                    throw new InvalidCastException("Can't turn a primitive constant value into non-discrete fuzzy set value");
            }
            else
            {
                throw new NotImplementedException();
            }
            
        }
        private FuzzyProbabilisticValue<T> turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<T>(FuzzyProbabilisticValueParsingData data)
        {
            throw new NotImplementedException();
            FuzzyProbabilisticValue<T> ans;
            //extract FieldType domain
            FieldType domain;
            Type t = typeof(T);
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
            else
            {
                throw new NotSupportedException($"{typeof(T)} isn't supported");
            }
            //extract List<FuzzySet<T>> valueList
            List<FuzzySet<object>> valueList=new List<FuzzySet<object>>();
            foreach(Constant c in data.valueList)
            {
                if(c is IntConstant)
                {
                    if (t != typeof(int))
                        throw new InvalidCastException($"The fuzzy set's defining domain is supposed to be {t.Name}, but get value of int");
                    int value = (int)c.getVal();
                    List<int> valueSet = new List<int> { value};
                    List<float> membershipDegreeSet=new List<float> { 1.0f};
                    string fuzzySetName = value.ToString();
                    FieldType fuzzySetType = FieldType.distFS_INT;
                    DiscreteFuzzySet<int> fuzzset = new DiscreteFuzzySet<int>(valueSet, membershipDegreeSet, fuzzySetName, fuzzySetType);
                    valueList.Add((FuzzySet<object>)(object)fuzzset);
                }
                
            }

        }
        public bool next()
        {
            throw new NotImplementedException();
            List<string> primaryKey = this.relationInfo.getSchema().getPrimarykey();
            string sql = $"SELECT * FROM {this.relationInfo.getRelName()} ORDER BY";
            foreach(string fieldName in primaryKey)
            {
                sql += " " + fieldName + ",";
            }
            sql = sql.TrimEnd(',');
            sql += $"  LIMIT {this.currentTupleIndex} OFFSET 1";
            IDataReader reader = this.dbMgr.executeQuery(sql);
            using (reader)
            {
                if (reader.Read())
                {
                    List<Field> fields = this.relationInfo.getSchema().getFields();
                    string content;
                    for(int i=0;  i<fields.Count; ++i)
                    {
                        content = (string)reader[fields[i].getFieldName()];
                        this.parser.parse(content);
                        FuzzyProbabilisticValueParsingData parsingData = this.parser.fuzzyProbabilisticValue();

                    }

                }
                else
                    return false;
            }
        }
        public void close() { }
        public void setFieldContent<T>(string fldname, FuzzyProbabilisticValue<T> content)
        {
            int index = getFieldIndexInTuple(fldname);
            if (index == -1)
                throw new QueryDataNotExistException($"Relation {this.relationInfo.getRelName()} doesn't have attribute {fldname}");
            Field field=this.relationInfo.getSchema().getFields()[index];
            if(typeof(T)==typeof(int))
            {
                if (field.getFieldInfo().getType() != FieldType.INT && field.getFieldInfo().getType() != FieldType.distFS_INT)
                    throw new InvalidCastException($"Fuzzy probabilistic value of {fldname} doesn't contain fuzzy sets defined on domain of int");

            }
               
        }


    }
}
