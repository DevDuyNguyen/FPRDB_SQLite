using BLL.DomainObject;
using BLL.Enums;
using BLL.Exceptions;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BLL.SQLProcessing
{
    public class BasicUpdatePlanner:UpdatePlanner
    {
        private DatabaseManager dbMgr;

        public BasicUpdatePlanner(DatabaseManager dbMgr)
        {
            this.dbMgr = dbMgr;
        }

        public bool executeCreateSchema(FPRDBSchema data)
        {
            //string createTableForSchemaSQL = $@"CREATE TABLE {data.getSchemaName()} (";
            
            //foreach(Field field in data.getFields())
            //{
            //    FieldInfo fieldInfo = field.getFieldInfo();


            //    string fieldDef = $"{field.getFieldName()} {fieldInfo.getType().ToString()}";
            //    if (fieldInfo.getType() == FieldType.VARCHAR)
            //    {
            //        fieldDef += $"({fieldInfo.getTXTLength()})";
            //    }
            //    fieldDef += ",";
            //    createTableForSchemaSQL += fieldDef;
            //}

            //string primaryKeyConstraint = $"PRIMARY KEY (";
            //foreach(string keyAttribute in data.getPrimarykey())
            //{
            //    primaryKeyConstraint+=keyAttribute+",";
            //}
            //primaryKeyConstraint=primaryKeyConstraint.TrimEnd(',');
            //primaryKeyConstraint += ")";
            //createTableForSchemaSQL += primaryKeyConstraint + ")";


            //this.dbMgr.executeNonQuery(createTableForSchemaSQL);
            long schemaID;

            string insert_fprdb_RelationSchema = $"INSERT INTO fprdb_RelationSchema(relschema_name) VALUES ('{data.getSchemaName()}')";
            this.dbMgr.executeNonQuery(insert_fprdb_RelationSchema);

            IDataReader reader;
            using (reader = this.dbMgr.executeQuery($"SELECT oid from fprdb_RelationSchema where relschema_name='{data.getSchemaName()}'"))
            {
                reader.Read();
                schemaID = (long)reader["oid"];
            }
            //reader.Close();
            //this.dbMgr.closeConnection();

            foreach(Field field in data.getFields())
            {
                FieldInfo fieldInfo = field.getFieldInfo();
                string getTypeID = $"SELECT oid from fprdb_Type where type_name='{fieldInfo.getType().ToString()}'";
                long typeID;
                using (reader = this.dbMgr.executeQuery(getTypeID))
                {
                    reader.Read();
                    typeID = (long)reader["oid"];
                }
                //reader.Close();
                //this.dbMgr.closeConnection();

                string insertAttributeSQL = @$"INSERT INTO fprdb_Attribute(att_relschema_id, att_name, att_type_id, att_type_mod, att_not_null)
                    VALUES ({schemaID}, '{field.getFieldName()}', {typeID}, {fieldInfo.getTXTLength()}, {data.getPrimarykey().Contains(field.getFieldName())})
                ";
                this.dbMgr.executeNonQuery(insertAttributeSQL);
            }

            string primaryKeyConstraintSQL = $@"INSERT INTO fprdb_Constraint (con_name, con_type, con_attributes, con_relschema_id)
                VALUES ('{data.primaryConstraintName}','{ConstraintType.IDENTITY.ToString()}','{string.Join(",", data.getPrimarykey())}',{schemaID})
            ";
            this.dbMgr.executeNonQuery(primaryKeyConstraintSQL);

            return true;



        }
        public bool executeCreateRelation(FPRDBRelation data)
        {
            //create sqlite table for relation
            IDataReader reader;
            long schemaID;
            using (reader = this.dbMgr.executeQuery($"SELECT oid from fprdb_RelationSchema where relschema_name='{data.getSchemaName()}'"))
            {
                reader.Read();
                schemaID = (long)reader["oid"];
            }
            //string getAttributes = $@"SELECT attr.*, type.type_name 
            //    FROM fprdb_Attribute as attr
            //    JOIN fprdb_Type as type on attr.att_type_id=type.oid
            //    WHERE att_relschema_id={schemaID};";
            //reader = this.dbMgr.executeQuery(getAttributes);
            //List<Field> fieldList = new List<Field>();
            //using (reader)
            //{
            //    if (!reader.Read())
            //        throw new SQLExecutionException($"Schema {data.getSchemaName()} has no attribute");
            //    do
            //    {
            //        Field field = new Field(
            //            (string)reader["att_name"],
            //            new FieldInfo(Enum.Parse<FieldType>((string)reader["type_name"]), Convert.ToInt32(reader["att_type_mod"]))
            //        );
            //        fieldList.Add(field);
            //    }
            //    while (reader.Read());
            //}

            //string createTableForRelationSQL = $@"CREATE TABLE {data.getRelName()} (";
            

            //foreach (Field field in fieldList)
            //{
            //    FieldInfo fieldInfo = field.getFieldInfo();


            //    string fieldDef = $"{field.getFieldName()} {fieldInfo.getType().ToString()}";
            //    if (fieldInfo.getType() == FieldType.VARCHAR)
            //    {
            //        fieldDef += $"({fieldInfo.getTXTLength()})";
            //    }
            //    fieldDef += ",";
            //    createTableForRelationSQL += fieldDef;
            //}

            //string primaryKeyConstraint = $"PRIMARY KEY (";
            //foreach (string keyAttribute in data.getPrimarykey())
            //{
            //    primaryKeyConstraint += keyAttribute + ",";
            //}
            //primaryKeyConstraint = primaryKeyConstraint.TrimEnd(',');
            //primaryKeyConstraint += ")";
            //createTableForSchemaSQL += primaryKeyConstraint + ")";


            //this.dbMgr.executeNonQuery(createTableForSchemaSQL);
            //string getAttributes = $@"SELECT attr.*, type.type_name 
            //    FROM fprdb_Attribute as attr
            //    JOIN fprdb_Type as type on attr.att_type_id=type.oid
            //    WHERE att_relschema_id={schemaID};";
            //reader = this.dbMgr.executeQuery(getAttributes);
            //List<Field> fieldList = new List<Field>();
            //using (reader)
            //{
            //    if (!reader.Read())
            //        throw new SQLExecutionException($"Schema {data.getSchemaName()} has no attribute");
            //    do
            //    {
            //        Field field = new Field(
            //            (string)reader["att_name"],
            //            new FieldInfo(Enum.Parse<FieldType>((string)reader["type_name"]), Convert.ToInt32(reader["att_type_mod"]))
            //        );
            //        fieldList.Add(field);
            //    }
            //    while (reader.Read());
            //}

            //string createTableForRelationSQL = $@"CREATE TABLE {data.getRelName()} (";
            

            //foreach (Field field in fieldList)
            //{
            //    FieldInfo fieldInfo = field.getFieldInfo();


            //    string fieldDef = $"{field.getFieldName()} {fieldInfo.getType().ToString()}";
            //    if (fieldInfo.getType() == FieldType.VARCHAR)
            //    {
            //        fieldDef += $"({fieldInfo.getTXTLength()})";
            //    }
            //    fieldDef += ",";
            //    createTableForRelationSQL += fieldDef;
            //}

            //string primaryKeyConstraint = $"PRIMARY KEY (";
            //foreach (string keyAttribute in data.getPrimarykey())
            //{
            //    primaryKeyConstraint += keyAttribute + ",";
            //}
            //primaryKeyConstraint = primaryKeyConstraint.TrimEnd(',');
            //primaryKeyConstraint += ")";
            //createTableForSchemaSQL += primaryKeyConstraint + ")";


            //this.dbMgr.executeNonQuery(createTableForSchemaSQL);

            string getAttributes = $@"SELECT att_name 
                FROM fprdb_Attribute 
                WHERE att_relschema_id={schemaID};";
            reader = this.dbMgr.executeQuery(getAttributes);
            List<string> fieldNames = new List<string>();
            using (reader)
            {
                if (!reader.Read())
                    throw new SQLExecutionException($"Schema {data.getSchemaName()} has no attribute");
                do
                {
                    fieldNames.Add((string)reader["att_name"]);
                }
                while (reader.Read());
            }

            string createTableForRelationSQL = $@"CREATE TABLE {data.getRelName()} (";

            foreach (string name in fieldNames)
            {
                string fieldDef = $"{name} TEXT,";
                createTableForRelationSQL += fieldDef;
            }
            createTableForRelationSQL = createTableForRelationSQL.TrimEnd(',');
            createTableForRelationSQL += ")";

            this.dbMgr.executeNonQuery(createTableForRelationSQL);

            //insert metadata of the relation into fprdb_Relation
            string insertRelationMetadata = $@"INSERT INTO fprdb_Relation(rel_name, rel_relation_schema)
                VALUES ('{data.getRelName()}',{schemaID})";
            this.dbMgr.executeNonQuery(insertRelationMetadata);
            return true;
        }
        public int executeInsert(InsertData data)
        {
            //insert the data in actual table as text value
            string relName = data.relation;
            List<string> fieldList = data.fieldList;
            List<FuzzyProbabilisticValueParsingData> fprobValues = data.fuzzyProbabilisticValues;
            Dictionary<string, int> fuzzySetOIDs = new Dictionary<string, int>();
            string insertSQL = $"INSERT INTO {relName} (";
            foreach(string fieldName in fieldList)
            {
                insertSQL += fieldName + ",";
            }
            insertSQL = insertSQL.TrimEnd(',');
            insertSQL += ") VALUES (";
            for(int i=0; i<fieldList.Count; ++i)
            {
                insertSQL += "'" + fprobValues[i].ToTextRepresentation() + "'" + ",";
            }
            insertSQL = insertSQL.TrimEnd(',');
            insertSQL += ")";
            this.dbMgr.executeNonQuery(insertSQL);

            //if inserted data is a fuzzy set, then increase the field "no" of fprdb_Relation_Fuzzyset by 1
            foreach(FuzzyProbabilisticValueParsingData v in fprobValues)
            {
                foreach (Constant c in v.valueList)
                {
                    if (c is FuzzySetConstant)
                    {
                        FuzzySetConstant fsConstant = (FuzzySetConstant)c;
                        if (fsConstant.getFuzzySetOID() == null || fsConstant.getFuzzySetOID() == default)
                        {
                            IDataReader reader1 = this.dbMgr.executeQuery($"SELECT oid FROM fprdb_FuzzySet WHERE fuzzset_name='{(string)fsConstant.getVal()}'");
                            if (!reader1.Read())
                                throw new QueryDataNotExistException($"Fuzzy set {(string)fsConstant.getVal()} doesn't exist");
                            fuzzySetOIDs.TryAdd((string)fsConstant.getVal(), Convert.ToInt32(reader1["oid"]));
                        }
                        else
                            fuzzySetOIDs.TryAdd((string)fsConstant.getVal(), fsConstant.getFuzzySetOID());
                    }
                }
            }
            int relOID;
            IDataReader reader = this.dbMgr.executeQuery($"SELECT oid FROM fprdb_Relation WHERE rel_name='{data.relation}'");
            using (reader)
            {
                if (!reader.Read())
                    throw new QueryDataNotExistException($"Relation {data.relation} doesn't exist");
                relOID=Convert.ToInt32(reader["oid"]);
            }
            foreach(KeyValuePair<string, int> entry in fuzzySetOIDs)
            {
                reader = this.dbMgr.executeQuery($"SELECT 1 FROM FPRDB_Rel_FuzzSet WHERE rel_oid={relOID} AND fuzzset_oid={fuzzySetOIDs[entry.Key]}");
                bool relHasFuzzySet;
                using (reader)
                {
                    relHasFuzzySet = reader.Read();
                }
                if(!relHasFuzzySet)
                    this.dbMgr.executeNonQuery($"INSERT INTO FPRDB_Rel_FuzzSet (rel_oid, fuzzset_oid, no) VALUES ({relOID},{fuzzySetOIDs[entry.Key]},1)");
                else
                {
                    this.dbMgr.executeNonQuery($"UPDATE FPRDB_Rel_FuzzSet SET no=no+1 WHERE rel_oid={relOID} AND fuzzset_oid={fuzzySetOIDs[entry.Key]}");
                }
            }
            return 1;

        }


    }
}
