using BLL.DomainObject;
using BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL.SQLProcessing
{
    public class MetadataManager
    {
        public DatabaseManager databaseMgr;

        public MetadataManager(DatabaseManager databaseMgr)
        {
            this.databaseMgr = databaseMgr;
        }

        public bool isSchemaExist(string name)
        {
            try
            {
                string sql = $"select 1 from fprdb_RelationSchema where relschema_name='{name}'";
                IDataReader reader = this.databaseMgr.executeQuery(sql);
                bool ans = reader.Read();
                reader.Close();
                return ans;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool isFuzzySetWithNameExist(string name)
        {
            try
            {
                string sql = $"select 1 from fprdb_FuzzySet where fuzzset_name='{name}'";
                IDataReader reader = this.databaseMgr.executeQuery(sql);
                bool ans = reader.Read();
                reader.Close();
                return ans;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool isConstraintExist(string name)
        {
            try
            {
                string sql = $"select 1 from fprdb_Constraint where con_name='{name}'";
                IDataReader reader = this.databaseMgr.executeQuery(sql);
                bool ans= reader.Read();
                reader.Close();
                return ans;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool isRelationExist(string name)
        {
            IDataReader reader = this.databaseMgr.executeQuery($"SELECT 1 FROM fprdb_Relation WHERE rel_name='{name}'");
            bool ans;
            using (reader)
            {
                ans = reader.Read();
            }
            return ans;
        }
        public FPRDBRelation getRelation(string name)
        {
            string getRelationSQL = $@"
                SELECT
	                rel.oid as 'rel.oid',
	                relSch.oid as 'relSch.oid',
	                relSch.relschema_name,
	                attr.att_name,
	                attr.att_not_null,
                    attr.att_type_mod,
	                type.oid as 'type.oid',
	                type.type_name,
                    cons.con_attributes,
                    cons.con_name
                FROM fprdb_Relation as rel
                JOIN fprdb_RelationSchema as relSch ON rel.rel_relation_schema=relSch.oid
                JOIN fprdb_Attribute as attr on relSch.oid=attr.att_relschema_id
                JOIN fprdb_Type as type on attr.att_type_id=type.oid
                JOIN fprdb_Constraint AS cons ON cons.con_relschema_id = relSch.oid
                WHERE rel.rel_name='{name}' AND cons.con_type = 'IDENTITY'
                ORDER BY rel.oid;
            ";
            IDataReader reader = this.databaseMgr.executeQuery(getRelationSQL);

            string schemaName;
            List<Field> fields=new List<Field>();
            List<string> primarykey=new List<string>();
            string primaryConstraintName;

            using (reader)
            {
                
                if(!reader.Read())
                {
                    throw new QueryDataNotExistException($"Relation {name} doesn't exist");
                }
                schemaName = (string)reader["relschema_name"];
                primarykey = ((string)reader["con_attributes"]).Split(",").ToList();
                primaryConstraintName = (string)reader["con_name"];
                do
                {
                    fields.Add(
                        new Field(
                            (string)reader["att_name"],
                            new FieldInfo(Enum.Parse<FieldType>((string)reader["type_name"]), Convert.ToInt32(reader["att_type_mod"]))
                        )
                    );

                } while (reader.Read());
            }
            return new FPRDBRelation(
                name,
                new FPRDBSchema(schemaName, fields, primarykey, primaryConstraintName),
                schemaName
            );


        }
        public FPRDBRelation getRelationByID(int oid)
        {
            string getRelationSQL = $@"
                SELECT
	                rel.oid as 'rel.oid',
                    rel.rel_name as 'rel_name',
	                relSch.oid as 'relSch.oid',
	                relSch.relschema_name,
	                attr.att_name,
	                attr.att_not_null,
                    attr.att_type_mod,
	                type.oid as 'type.oid',
	                type.type_name,
                    cons.con_attributes,
                    cons.con_name
                FROM fprdb_Relation as rel
                JOIN fprdb_RelationSchema as relSch ON rel.rel_relation_schema=relSch.oid
                JOIN fprdb_Attribute as attr on relSch.oid=attr.att_relschema_id
                JOIN fprdb_Type as type on attr.att_type_id=type.oid
                JOIN fprdb_Constraint AS cons ON cons.con_relschema_id = relSch.oid
                WHERE rel.oid={oid} AND cons.con_type = 'IDENTITY'
                ORDER BY rel.oid;
            ";
            IDataReader reader = this.databaseMgr.executeQuery(getRelationSQL);

            string schemaName;
            List<Field> fields = new List<Field>();
            List<string> primarykey = new List<string>();
            string primaryConstraintName, relName;

            using (reader)
            {

                if (!reader.Read())
                {
                    throw new QueryDataNotExistException($"Relation with {oid} doesn't exist");
                }
                schemaName = (string)reader["relschema_name"];
                primarykey = ((string)reader["con_attributes"]).Split(",").ToList();
                relName = (string)reader["rel_name"];
                primaryConstraintName = (string)reader["con_name"];
                do
                {
                    fields.Add(
                        new Field(
                            (string)reader["att_name"],
                            new FieldInfo(Enum.Parse<FieldType>((string)reader["type_name"]), Convert.ToInt32(reader["att_type_mod"]))
                        )
                    );

                } while (reader.Read());
            }
            return new FPRDBRelation(
                relName,
                new FPRDBSchema(schemaName, fields, primarykey, primaryConstraintName),
                schemaName
            );
        }
        public FieldType getFuzzySetType(string name)
        {
            IDataReader reader = this.databaseMgr.executeQuery($@"
                SELECT 
	                'DISCRETE' AS 'fuzzy_set_category',
	                type.type_name
                FROM fprdb_DiscreteFuzzySet as distFS
                JOIN fprdb_FuzzySet as FS ON distFS.oid=FS.oid
                JOIN fprdb_Type as type ON type.oid=FS.fuzzset_type_id
                WHERE fuzzset_name='{name}'

                UNION 

                SELECT 
	                'CONTINUOUS' AS 'fuzzy_set_category',
	                type.type_name
                FROM fprdb_ContinousFuzzySet as contFS
                JOIN fprdb_FuzzySet as FS ON contFS.oid=FS.oid
                JOIN fprdb_Type as type ON type.oid=FS.fuzzset_type_id
                WHERE fuzzset_name='{name}';
            ");
            FieldType type;
            using (reader)
            {
                if (!reader.Read())
                    throw new QueryDataNotExistException($"Fuzzy set {name} doesn't exist");
                if ((string)reader["fuzzy_set_category"]== "DISCRETE")
                {
                    if ((string)reader["type_name"] == FieldType.INT.ToString())
                        type = FieldType.distFS_INT;
                    else if ((string)reader["type_name"] == FieldType.FLOAT.ToString())
                        type = FieldType.distFS_FLOAT;
                    else if ((string)reader["type_name"] == FieldType.VARCHAR.ToString())
                        type = FieldType.distFS_TEXT;
                    else {
                        throw new Exception($"Fuzzy set {name} has supported domain type");
                    }
                }
                else
                {
                    if ((string)reader["type_name"] != FieldType.FLOAT.ToString())
                        throw new Exception($"Fuzzy set {name} has supported domain type");
                    type = FieldType.contFS;
                }
            }
            return type;
        }
        public FieldType getFuzzySetTypeByID(int oid)
        {
            IDataReader reader = this.databaseMgr.executeQuery($@"
                SELECT 
	                'DISCRETE' AS 'fuzzy_set_category',
	                type.type_name
                FROM fprdb_DiscreteFuzzySet as distFS
                JOIN fprdb_FuzzySet as FS ON distFS.oid=FS.oid
                JOIN fprdb_Type as type ON type.oid=FS.fuzzset_type_id
                WHERE distFS.oid={oid}

                UNION 

                SELECT 
	                'CONTINUOUS' AS 'fuzzy_set_category',
	                type.type_name
                FROM fprdb_ContinousFuzzySet as contFS
                JOIN fprdb_FuzzySet as FS ON contFS.oid=FS.oid
                JOIN fprdb_Type as type ON type.oid=FS.fuzzset_type_id
                WHERE contFS.oid={oid};
            ");
            FieldType type;
            using (reader)
            {
                if (!reader.Read())
                    throw new QueryDataNotExistException($"Fuzzy set with id {oid} doesn't exist");
                if ((string)reader["fuzzy_set_category"] == "DISCRETE")
                {
                    if ((string)reader["type_name"] == FieldType.INT.ToString())
                        type = FieldType.distFS_INT;
                    else if ((string)reader["type_name"] == FieldType.FLOAT.ToString())
                        type = FieldType.distFS_FLOAT;
                    else if ((string)reader["type_name"] == FieldType.VARCHAR.ToString())
                        type = FieldType.distFS_TEXT;
                    else
                    {
                        throw new Exception($"Fuzzy set with id {oid} doesn't supported domain type");
                    }
                }
                else
                {
                    if ((string)reader["type_name"] != FieldType.FLOAT.ToString())
                        throw new Exception($"Fuzzy set with id {oid} has supported domain type");
                    type = FieldType.contFS;
                }
            }
            return type;
        }
        public int getFuzzySetOID(string name)
        {
            IDataReader reader = this.databaseMgr.executeQuery($"SELECT OID FROM fprdb_FuzzySet WHERE fuzzset_name='{name}'");
            int oid=-1;
            using (reader)
            {
                if (reader.Read())
                    oid = Convert.ToInt32(reader["oid"]);
                
            }
            return oid;
        }
        public bool isTupleExist(List<string> primaryKey, List<string> value, string relationName)
        {
            string sql = $@"
                SELECT 1 FROM {relationName}
                WHERE
            ";

            int index = 0;
            int totalLength = primaryKey.Count;

            sql += $" {primaryKey[index]}='{{({value[index]},[1,1])}}'";
            if (totalLength != 1)
            {
                do
                {
                    ++index;
                    sql += $" AND {primaryKey[index]}='{{({value[index]},[1,1])}}'";
                } while (index < totalLength - 1);
            }

            IDataReader reader = this.databaseMgr.executeQuery(sql);
            bool ans = false;
            using (reader)
            {
                ans = reader.Read();
            }
            return ans;
        }
        public FuzzySet<T> getFuzzySet<T>(string name, FieldType fuzzSetType)
        {
            string sql;
            Type t = typeof(T);
            if (
                (fuzzSetType == FieldType.distFS_INT && t != typeof(int))
                || ((fuzzSetType == FieldType.distFS_FLOAT && t != typeof(float)))
                || (fuzzSetType == FieldType.distFS_TEXT && t != typeof(string))
                || (fuzzSetType == FieldType.contFS && t != typeof(float))
                )
                throw new InvalidCastException($"Fuzzy set type {fuzzSetType.ToString()} isn't compatible with defininng domain {t.Name}");
            if(fuzzSetType != FieldType.contFS)
            {
                sql = $@"
                    SELECT fuzzset_x,fuzzset_membership_degree, fs.oid as ""fs_oid""
                    FROM fprdb_DiscreteFuzzySet as distFS
                    JOIN fprdb_FuzzySet as fs on fs.oid=distFS.oid
                    where fs.fuzzset_name='{name}';
                ";
                IDataReader reader = this.databaseMgr.executeQuery(sql);
                using (reader)
                {
                    if (!reader.Read())
                        throw new QueryDataNotExistException($"Fuzzy set {name} doesn't exist");
                    List<T> values = ((string)(reader["fuzzset_x"]))
                                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(s => (T)Convert.ChangeType(
                                                s.Trim(),
                                                typeof(T),
                                                CultureInfo.InvariantCulture))
                                            .ToList();
                    string str = (string)reader["fuzzset_membership_degree"];
                    List<float> memberships = (str).Split(",").Select(float.Parse).ToList();
                    int oid = Convert.ToInt32(reader["fs_oid"]);

                    return new DiscreteFuzzySet<T>(values, memberships, name, fuzzSetType, oid);

                }
            }
            else
            {
                sql = $@"
                    SELECT fuzzset_bottom_left,fuzzset_top_left,fuzzset_top_right,fuzzset_bottom_right, fs.oid as ""fs_oid""
                    FROM fprdb_ContinousFuzzySet as contFS
                    JOIN fprdb_FuzzySet as fs on fs.oid=contFS.oid
                    where fs.fuzzset_name='{name}';
                ";
                IDataReader reader = this.databaseMgr.executeQuery(sql);
                using (reader)
                {
                    if (!reader.Read())
                        throw new QueryDataNotExistException($"Fuzzy set {name} doesn't exist");
                    float p1 = Convert.ToSingle(reader["fuzzset_bottom_left"]);
                    float p2 = Convert.ToSingle(reader["fuzzset_top_left"]);
                    float p3 = Convert.ToSingle(reader["fuzzset_top_right"]);
                    float p4 = Convert.ToSingle(reader["fuzzset_bottom_right"]);
                    int oid = Convert.ToInt32(reader["fs_oid"]);
                    return (FuzzySet<T>)(object)new ContinuousFuzzySet(p1, p2, p3, p4, name, oid);
                }
            }
        }
        public FuzzySet<T> getFuzzySetByID<T>(int fs_oid, FieldType fuzzSetType)
        {
            string sql;
            Type t = typeof(T);
            if (
                (fuzzSetType == FieldType.distFS_INT && t != typeof(int))
                || ((fuzzSetType == FieldType.distFS_FLOAT && t != typeof(float)))
                || (fuzzSetType == FieldType.distFS_TEXT && t != typeof(string))
                || (fuzzSetType == FieldType.contFS && t != typeof(float))
                )
                throw new InvalidCastException($"Fuzzy set type {fuzzSetType.ToString()} isn't compatible with defininng domain {t.Name}");
            string fsName;
            using (IDataReader r = this.databaseMgr.executeQuery($"SELECT fuzzset_name FROM fprdb_FuzzySet WHERE oid={fs_oid}"))
            {
                if (!r.Read())
                    throw new QueryDataNotExistException($"Fuzzy set with id {fs_oid} doesn't exist");
                fsName = (string)r["fuzzset_name"];
            }
            if (fuzzSetType != FieldType.contFS)
            {
                sql = $@"
                    SELECT fuzzset_x,fuzzset_membership_degree, fs.oid as ""fs_oid""
                    FROM fprdb_DiscreteFuzzySet as distFS
                    JOIN fprdb_FuzzySet as fs on fs.oid=distFS.oid
                    where fs.oid={fs_oid};
                ";
                IDataReader reader = this.databaseMgr.executeQuery(sql);
                using (reader)
                {
                    if (!reader.Read())
                        throw new QueryDataNotExistException($"Fuzzy set with id {fs_oid} doesn't exist");
                    List<T> values = ((string)(reader["fuzzset_x"]))
                                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(s => (T)Convert.ChangeType(
                                                s.Trim(),
                                                typeof(T),
                                                CultureInfo.InvariantCulture))
                                            .ToList();
                    string str = (string)reader["fuzzset_membership_degree"];
                    List<float> memberships = (str).Split(",").Select(float.Parse).ToList();
                    int oid = Convert.ToInt32(reader["fs_oid"]);

                    return new DiscreteFuzzySet<T>(values, memberships, fsName, fuzzSetType, oid);

                }
            }
            else
            {
                sql = $@"
                    SELECT fuzzset_bottom_left,fuzzset_top_left,fuzzset_top_right,fuzzset_bottom_right, fs.oid as ""fs_oid""
                    FROM fprdb_ContinousFuzzySet as contFS
                    JOIN fprdb_FuzzySet as fs on fs.oid=contFS.oid
                    where fs.oid={fs_oid};
                ";
                IDataReader reader = this.databaseMgr.executeQuery(sql);
                using (reader)
                {
                    if (!reader.Read())
                        throw new QueryDataNotExistException($"Fuzzy set with id {fs_oid} doesn't exist");
                    float p1 = Convert.ToSingle(reader["fuzzset_bottom_left"]);
                    float p2 = Convert.ToSingle(reader["fuzzset_top_left"]);
                    float p3 = Convert.ToSingle(reader["fuzzset_top_right"]);
                    float p4 = Convert.ToSingle(reader["fuzzset_bottom_right"]);
                    int oid = Convert.ToInt32(reader["fs_oid"]);
                    return (FuzzySet<T>)(object)new ContinuousFuzzySet(p1, p2, p3, p4, fsName, oid);
                }
            }
        }
        public int getRelationOID(string name)
        {
            int relOid=-1;
            string getOIDofRelation = $"select oid from fprdb_Relation where rel_name='{name}'";
            using(IDataReader r = this.databaseMgr.executeQuery(getOIDofRelation))
            {
                if (r.Read())
                {
                    relOid = Convert.ToInt32(r["oid"]);
                }
                else
                    throw new QueryDataNotExistException($"Relation {name} doesn't exist");
            }
            return relOid;
        }
        public int getSchemaOID(string name)
        {
            int schemaOID = -1;
            using (IDataReader r = this.databaseMgr.executeQuery($"select oid from fprdb_RelationSchema where relschema_name='{name}'"))
            {
                if (r.Read())
                {
                    schemaOID = Convert.ToInt32(r["oid"]);
                }
            }
            return schemaOID;
        }
        public bool isRelationOnSchemaExist(string schemaName)
        {
            int schemaOID=getSchemaOID(schemaName);
            if(schemaOID==-1)
                throw new QueryDataNotExistException($"Schema {schemaName} doesn't exist");
            using (IDataReader r = this.databaseMgr.executeQuery($"select 1 from fprdb_Relation where rel_relation_schema={schemaOID}"))
            {
                return r.Read();
            }

        }
        public FPRDBSchema getFPRDBSchema(string name)
        {
            string getSchemaSQL = $@"
                SELECT
	                relSch.oid as 'relSch.oid',
	                relSch.relschema_name,
	                attr.att_name,
	                attr.att_not_null,
                    attr.att_type_mod,
	                type.oid as 'type.oid',
	                type.type_name,
                    cons.con_attributes,
                    cons.con_name
                FROM fprdb_RelationSchema as relSch
                JOIN fprdb_Attribute as attr on relSch.oid=attr.att_relschema_id
                JOIN fprdb_Type as type on attr.att_type_id=type.oid
                JOIN fprdb_Constraint AS cons ON cons.con_relschema_id = relSch.oid
                WHERE rel.rel_name='{name}' AND cons.con_type = 'IDENTITY'
                ORDER BY 'rel.oid';
            ";
            IDataReader reader = this.databaseMgr.executeQuery(getSchemaSQL);

            string schemaName;
            List<Field> fields = new List<Field>();
            List<string> primarykey = new List<string>();
            string primaryConstraintName;

            using (reader)
            {

                if (!reader.Read())
                {
                    throw new QueryDataNotExistException($"Relation {name} doesn't exist");
                }
                schemaName = (string)reader["relschema_name"];
                primarykey = ((string)reader["con_attributes"]).Split(",").ToList();
                primaryConstraintName = (string)reader["con_name"];
                do
                {
                    fields.Add(
                        new Field(
                            (string)reader["att_name"],
                            new FieldInfo(Enum.Parse<FieldType>((string)reader["type_name"]), Convert.ToInt32(reader["att_type_mod"]))
                        )
                    );

                } while (reader.Read());
            }
            return new FPRDBSchema(schemaName, fields, primarykey, primaryConstraintName);

        }
    }
}
