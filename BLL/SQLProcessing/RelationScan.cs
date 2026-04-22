using BLL.Common;
using BLL.DomainObject;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BLL.SQLProcessing
{
    public class RelationScan:UpdateScan
    {
        private FPRDBRelation relationInfo;
        private int currentTupleIndex;
        private List<AbstractFuzzyProbabilisticValue> currentTuple;
        private DatabaseManager dbMgr;
        private MetadataManager metaDataMgr;
        private RecursiveDescentParser parser;
        private ConstraintService constraintService;
        public RelationScan(FPRDBRelation relationInfo, DatabaseManager dbMgr, MetadataManager metaDataMgr, RecursiveDescentParser parser, ConstraintService constraintService)
        {
            CompositionRoot compRoot = new CompositionRoot();
            this.parser = compRoot.getParser();
            this.dbMgr = dbMgr;
            this.metaDataMgr = metaDataMgr;
            this.relationInfo = relationInfo;
            this.currentTupleIndex = 0;
            this.parser = parser;
            this.constraintService = constraintService;
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
        public FuzzyProbabilisticValue<T> getFieldContent<T>(string fldName)
        {
            int index = getFieldIndexInTuple(fldName);
            if (index == -1)
                throw new QueryDataNotExistException($"Relation {this.relationInfo.getRelName()} doesn't have attribute {fldName}");
            var fprobValue = this.currentTuple[index];
            if (!(fprobValue is FuzzyProbabilisticValue<T>))
                throw new InvalidCastException($"Fuzzy probabilistic value of field {fldName} doesn't contain fuzzy sets defined on domain of {typeof(T).Name}");
            return (FuzzyProbabilisticValue<T>)fprobValue;

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
                            FuzzyProbabilisticValue<int> fprobValue = FuzzyProbabilisticValueUtilities.turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<int>(parsingData, fieldType, this.metaDataMgr);
                            tmp.Add(fprobValue);
                        }
                        else if (fieldType == FieldType.FLOAT || fieldType == FieldType.distFS_FLOAT || fieldType == FieldType.contFS)
                        {
                            FuzzyProbabilisticValue<float> fprobValue = FuzzyProbabilisticValueUtilities.turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<float>(parsingData, fieldType, this.metaDataMgr);
                            tmp.Add(fprobValue);
                        }
                        else if (fieldType == FieldType.CHAR || fieldType == FieldType.VARCHAR || fieldType == FieldType.distFS_TEXT)
                        {
                            FuzzyProbabilisticValue<string> fprobValue = FuzzyProbabilisticValueUtilities.turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<string>(parsingData, fieldType, this.metaDataMgr);
                            tmp.Add(fprobValue);
                        }
                        else //if (fieldType == FieldType.BOOLEAN)
                        {
                            FuzzyProbabilisticValue<bool> fprobValue = FuzzyProbabilisticValueUtilities.turnFuzzyProbabilisticValueParsingDataToFuzzyProbabilisticValue<bool>(parsingData, fieldType, this.metaDataMgr);
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
            FieldType fieldType = field.getFieldInfo().getType();
            //List<Field> schemaFields = this.relationInfo.getSchema().getFields();
            FPRDBSchema schema = this.relationInfo.getSchema();

            //if a key attribute is updated, check if integrity constraint is violated
            if (this.relationInfo.getSchema().getPrimarykey().Contains(fldname)){
                List<AbstractFuzzyProbabilisticValue> oldKeyValue = new List<AbstractFuzzyProbabilisticValue>();
                List<AbstractFuzzyProbabilisticValue> newKeyValue = new List<AbstractFuzzyProbabilisticValue>();
                Type type;
                AbstractFuzzyProbabilisticValue tmp;
                foreach (string keyName in schema.getPrimarykey())
                {
                    if (FieldTypeUtilities.getDomainType(schema.getFieldByName(keyName).getFieldInfo().getType()) == typeof(int))
                    {
                        tmp = this.getFieldContent<int>(keyName);
                    }
                    else if (FieldTypeUtilities.getDomainType(schema.getFieldByName(keyName).getFieldInfo().getType()) == typeof(float))
                    {
                        tmp = this.getFieldContent<float>(keyName);
                    }
                    else if (FieldTypeUtilities.getDomainType(schema.getFieldByName(keyName).getFieldInfo().getType()) == typeof(string))
                    {
                        tmp = this.getFieldContent<string>(keyName);
                    }
                    else
                        tmp = this.getFieldContent<bool>(keyName);
                    oldKeyValue.Add(tmp);
                    if (keyName == fldname)
                        newKeyValue.Add(content);
                    else
                        newKeyValue.Add(tmp);
                }
                this.constraintService.checkUpdateIntegrityConstraintViolation(this.relationInfo, schema.getPrimarykey(), oldKeyValue, newKeyValue);
            }

            //check if domain of content matches the domain of Field named fldname
            if (typeof(T) == typeof(int))
            {
                if (fieldType != FieldType.INT && fieldType != FieldType.distFS_INT && fieldType != FieldType.FLOAT && fieldType != FieldType.distFS_FLOAT && fieldType != FieldType.contFS)
                    throw new InvalidCastException($"Fuzzy probabilistic value of {fldname} doesn't contain possible value defined on domain of int");
            }
            else if (typeof(T) == typeof(float))
            {
                if (fieldType != FieldType.FLOAT && fieldType != FieldType.distFS_FLOAT && fieldType != FieldType.contFS)
                    throw new InvalidCastException($"Fuzzy probabilistic value of {fldname} doesn't contain possible value defined on domain of float");
            }
            else if (typeof(T) == typeof(string))
            {
                if (fieldType != FieldType.CHAR && fieldType != FieldType.VARCHAR && fieldType != FieldType.distFS_TEXT)
                    throw new InvalidCastException($"Fuzzy probabilistic value of {fldname} doesn't contain possible value defined on domain of string");
            }
            else if (typeof(T) == typeof(bool))
            {
                if (fieldType != FieldType.BOOLEAN)
                    throw new InvalidCastException($"Fuzzy probabilistic value of {fldname} doesn't contain possible value defined on domain of boolean");
            }
            else
            {
                throw new InvalidCastException($"Type {typeof(T).Name} isn't supported");
            }


            //foreach fuzzy set in the current interested tuple's attribute, decrease the ammount of time this fuzzy set is stored in the current relation by 1
            int relOid = this.metaDataMgr.getRelationOID(this.relationInfo.getRelName());
            this.decreaseNoFuzzySetRelatedToCurrentRelationBaseOn_FProbValue<T>((FuzzyProbabilisticValue<T>)this.currentTuple[index], relOid);
            //update stored current tuple attribute:
            string updateSQL = $"UPDATE {this.relationInfo.getRelName()} SET {fldname}='{content.ToString()}' WHERE";
            List<Field> fields = this.relationInfo.getSchema().getFields();
            int keyIndex = 0;
            foreach (string key in this.relationInfo.getSchema().primarykey)
            {
                for (int i = 0; i < fields.Count; ++i)
                {
                    if (fields[i].getFieldName() == key)
                    {
                        keyIndex = i;
                        break;
                    }
                }
                updateSQL += $" {key}='{this.currentTuple[keyIndex].ToString()}' AND";
            }
            int trailingANDIndex = updateSQL.LastIndexOf("AND");
            updateSQL = updateSQL.Substring(0, trailingANDIndex);
            this.dbMgr.executeNonQuery(updateSQL);
            //foreach fuzzy set in the assigning fuzzy probabilistic value content, increase the ammount of time this fuzzy set is stored in the current relation by 1
            this.increaseNoFuzzySetRelatedToCurrentRelationBaseOn_FProbValue<T>(content);

            //update in-memory current tuple attribute
            //this.currentTuple[index] = content;

        }
        public FPRDBSchema getSchema()
        {
            return this.relationInfo.getSchema();
        }
        public List<AbstractFuzzyProbabilisticValue> getCurrentTuple()
        {
            return this.currentTuple;
        }
        public void insert() => throw new NotImplementedException();
        private void decreaseNoFuzzySetRelatedToCurrentRelationBaseOn_FProbValue<T1>(FuzzyProbabilisticValue<T1> fprobValue, int relOid)
        {
            string fuzzySetName;
            string decreaseNoInFPRDB_Rel_FuzzSet;
            int fsOid;
            foreach (FuzzySet<T1> fs in fprobValue.valueList)
            {
                fuzzySetName = fs.getName();
                fsOid = this.metaDataMgr.getFuzzySetOID(fuzzySetName);
                if (fsOid != -1)
                {
                    decreaseNoInFPRDB_Rel_FuzzSet = $"UPDATE FPRDB_Rel_FuzzSet SET no=no-1 WHERE rel_oid={relOid} and fuzzset_oid={fsOid}";
                    this.dbMgr.executeNonQuery(decreaseNoInFPRDB_Rel_FuzzSet);
                }
            }
        }
        private void increaseNoFuzzySetRelatedToCurrentRelationBaseOn_FProbValue<T1>(FuzzyProbabilisticValue<T1> fprobValue)
        {
            int relOID;
            IDataReader reader = this.dbMgr.executeQuery($"SELECT oid FROM fprdb_Relation WHERE rel_name='{this.relationInfo.getRelName()}'");
            using (reader)
            {
                if (!reader.Read())
                    throw new QueryDataNotExistException($"Relation {this.relationInfo.getRelName()} doesn't exist");
                relOID = Convert.ToInt32(reader["oid"]);
            }
            //if inserted data is a fuzzy set, then increase the field "no" of fprdb_Relation_Fuzzyset by 1
            foreach (FuzzySet<T1> c in fprobValue.valueList)
            {
                if (c.getOID() != -1)
                {
                    reader = this.dbMgr.executeQuery($"SELECT 1 FROM FPRDB_Rel_FuzzSet WHERE rel_oid={relOID} AND fuzzset_oid={c.getOID()}");
                    bool relHasFuzzySet;
                    using (reader)
                    {
                        relHasFuzzySet = reader.Read();
                    }
                    if (!relHasFuzzySet)
                        this.dbMgr.executeNonQuery($"INSERT INTO FPRDB_Rel_FuzzSet (rel_oid, fuzzset_oid, no) VALUES ({relOID},{c.getOID()},1)");
                    else
                    {
                        this.dbMgr.executeNonQuery($"UPDATE FPRDB_Rel_FuzzSet SET no=no+1 WHERE rel_oid={relOID} AND fuzzset_oid={c.getOID()}");
                    }
                }
                
            }
            
        }
        public void delete()
        {
            int relOid=this.metaDataMgr.getRelationOID(this.relationInfo.getRelName());

            //decrease the no in FPRDB_Rel_FuzzSet for each fuzzy set in the current tuple
            
            Field field;
            List<Field> fields = this.relationInfo.getSchema().getFields();
            for(int i=0; i<fields.Count; ++i)
            {
                field = fields[i];
                switch (field.getFieldInfo().getType())
                {
                    case FieldType.INT:
                    case FieldType.distFS_INT:
                        decreaseNoFuzzySetRelatedToCurrentRelationBaseOn_FProbValue<int>((FuzzyProbabilisticValue<int>)this.currentTuple[i], relOid);
                        break;
                    case FieldType.FLOAT:
                    case FieldType.distFS_FLOAT:
                    case FieldType.contFS:
                        decreaseNoFuzzySetRelatedToCurrentRelationBaseOn_FProbValue<float>((FuzzyProbabilisticValue<float>)this.currentTuple[i], relOid);
                        break;
                    case FieldType.VARCHAR:
                    case FieldType.CHAR:
                    case FieldType.distFS_TEXT:
                        decreaseNoFuzzySetRelatedToCurrentRelationBaseOn_FProbValue<string>((FuzzyProbabilisticValue<string>)this.currentTuple[i], relOid);
                        break;
                    case FieldType.BOOLEAN:
                        decreaseNoFuzzySetRelatedToCurrentRelationBaseOn_FProbValue<bool>((FuzzyProbabilisticValue<bool>)this.currentTuple[i], relOid);
                        break;

                }
            }

            //delete the fprdb tuple from the fprdb relation
            string deleteSQL = $"DELETE FROM {relationInfo.getRelName()} WHERE";
            int keyIndex=0;
            foreach(string key in this.relationInfo.getSchema().primarykey)
            {
                for(int i=0; i<fields.Count; ++i)
                {
                    if (fields[i].getFieldName() == key)
                    {
                        keyIndex = i;
                        break;
                    }
                }
                deleteSQL += $" {key}='{this.currentTuple[keyIndex].ToString()}' AND";
            }
            int trailingANDIndex = deleteSQL.LastIndexOf("AND");
            deleteSQL = deleteSQL.Substring(0, trailingANDIndex);
            this.dbMgr.executeNonQuery(deleteSQL);

        }

    }
}
