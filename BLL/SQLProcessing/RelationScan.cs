using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Common;
using BLL.DomainObject;
using BLL.Exceptions;
using BLL.Interfaces;
namespace BLL.SQLProcessing
{
    public class RelationScan:Scan
    {
        private FPRDBRelation relationInfo;
        private int currentTupleIndex;
        private List<AbstractFuzzyProbabilisticValue> currentTuple;
        private DatabaseManager dbMgr;
        private MetadataManager metaDataMgr;
        private RecursiveDescentParser parser;
        public RelationScan(FPRDBRelation relationInfo, DatabaseManager dbMgr, MetadataManager metaDataMgr, RecursiveDescentParser parser)
        {
            CompositionRoot compRoot = new CompositionRoot();
            this.parser = compRoot.getParser();
            this.dbMgr = dbMgr;
            this.metaDataMgr = metaDataMgr;
            this.relationInfo = relationInfo;
            this.currentTupleIndex = 0;
            this.parser = parser;
        }
        public void beforeFirst()=> this.currentTupleIndex = 0;
        public bool hasField(string fldname)
        {
            Field field= this.relationInfo.getSchema().getFieldByName(fldname);
            return (field != null) ? true : false;
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
            return (FuzzyProbabilisticValue<T>)fprobValue;

        }
        
        //not done:mocking for private
        public FuzzyProbabilisticValue<T> turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<T>(FuzzyProbabilisticValueParsingData data, FieldType fieldType)
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
            List<FuzzySet<T>> valueList=new List<FuzzySet<T>>();
            foreach(Constant c in data.valueList)
            {
                valueList.Add(FuzzySetUltilities.turnConstantToFuzzySet<T>(c, this.metaDataMgr));
            }
            //extract lower bound, upper boud
            List<float> lowerBounds = new List<float>();
            List<float> upperBounds = new List<float>();
            for(int i=0; i<data.intervalProbUpperBoundList.Count; ++i)
            {
                lowerBounds.Add(data.intervalProbLowerBoundList[i]);
                upperBounds.Add(data.intervalProbUpperBoundList[i]);
            }
            return new FuzzyProbabilisticValue<T>(domain, valueList, lowerBounds, upperBounds);

        }
        public bool next()
        {
            List<string> primaryKey = this.relationInfo.getSchema().getPrimarykey();
            string sql = $"SELECT * FROM {this.relationInfo.getRelName()} ORDER BY";
            foreach(string fieldName in primaryKey)
            {
                sql += " " + fieldName + ",";
            }
            sql = sql.TrimEnd(',');
            sql += $"  LIMIT 1 OFFSET {this.currentTupleIndex}";
            IDataReader reader = this.dbMgr.executeQuery(sql);
            using (reader)
            {
                if (reader.Read())
                {
                    //List<FuzzyProbabilisticValue<object>> tmp = new List<FuzzyProbabilisticValue<object>>();
                    List<AbstractFuzzyProbabilisticValue> tmp = new List<AbstractFuzzyProbabilisticValue>();
                    List<Field> fields = this.relationInfo.getSchema().getFields();
                    string content;
                    for(int i=0;  i<fields.Count; ++i)
                    {
                        string fieldName = fields[i].getFieldName();
                        content = (string)reader[fieldName];
                        this.parser.parse(content);
                        FuzzyProbabilisticValueParsingData parsingData = this.parser.fuzzyProbabilisticValue();
                        FieldType fieldType = fields[i].getFieldInfo().getType();
                        if (fieldType == FieldType.INT || fieldType == FieldType.distFS_INT)
                        {
                            FuzzyProbabilisticValue<int> fprobValue = this.turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<int>(parsingData, fieldType);
                            tmp.Add(fprobValue);
                        }
                        else if (fieldType == FieldType.FLOAT || fieldType == FieldType.distFS_FLOAT || fieldType == FieldType.contFS)
                        {
                            FuzzyProbabilisticValue<float> fprobValue = this.turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<float>(parsingData, fieldType);
                            tmp.Add(fprobValue);
                        }
                        else if (fieldType == FieldType.CHAR || fieldType == FieldType.VARCHAR || fieldType == FieldType.distFS_TEXT)
                        {
                            FuzzyProbabilisticValue<string> fprobValue = this.turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<string>(parsingData, fieldType);
                            tmp.Add(fprobValue);
                        }
                        else //if (fieldType == FieldType.BOOLEAN)
                        {
                            FuzzyProbabilisticValue<bool> fprobValue = this.turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<bool>(parsingData, fieldType);
                            tmp.Add(fprobValue);
                        }
                        
                    }
                    this.currentTuple = tmp;
                    this.currentTupleIndex++;
                    return true;
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
        public FPRDBSchema getSchema()
        {
            return this.relationInfo.getSchema();
        }
        public List<AbstractFuzzyProbabilisticValue> getCurrentTuple()
        {
            return this.currentTuple;
        }

    }
}
