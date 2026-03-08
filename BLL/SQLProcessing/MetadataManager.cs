using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BLL.DomainObject;
using BLL.Exceptions;

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
                ORDER BY 'rel.oid';
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
        public int getFuzzySetOID(string name)
        {
            IDataReader reader = this.databaseMgr.executeQuery($"SELECT OID FROM fprdb_FuzzySet WHERE fuzzset_name='{name}'");
            int oid;
            using (reader)
            {
                if (!reader.Read())
                    throw new QueryDataNotExistException($"Fuzzy set {name} doesn't exist");
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

            sql += $" {primaryKey[index]}='{value[index]}'";
            if (totalLength != 1)
            {
                do
                {
                    ++index;
                    sql += $" AND {primaryKey[index]}='{value[index]}";
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

    }
}
