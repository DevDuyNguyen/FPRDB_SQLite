using BLL.DomainObject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class DatabaseService
    {
        private DatabaseManager dbMgr;

        public DatabaseService(DatabaseManager dbMgr)
        {
            this.dbMgr = dbMgr;
        }

        public void createDB(String filePath)
        {
            try
            {
                this.dbMgr.createDB(filePath);
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
            catch(IOException e)
            {
                throw e;
            }
        }
        public string getDatabaseName()
        {
            string name = this.dbMgr.getConnectionString();
            name = Path.GetFileName(name);
            int index = name.LastIndexOf(".");
            if (index == -1)
                return "";
            name = name.Substring(0, index);
            return name;
        }

        public List<FPRDBSchema> getFPRDBSchemas()
        {
            List<FPRDBSchema> schemas = new List<FPRDBSchema>();
            string sql = @"
                SELECT 
                    rsch.oid                 AS ""rsch.oid"", 
                    rsch.relschema_name      AS ""rsch.relschema_name"",
                    attr.att_relschema_id    AS ""attr.att_relschema_id"",
                    attr.att_number          AS ""attr.att_number"", 
                    attr.att_name            AS ""attr.att_name"", 
                    attr.att_type_id         AS ""attr.att_type_id"", 
                    attr.att_type_mod        AS ""attr.att_type_mod"", 
                    attr.att_not_null        AS ""attr.att_not_null"",
                    type.oid                 AS ""type.oid"", 
                    type.type_name           AS ""type.type_name"", 
                    type.type_type           AS ""type.type_type"",
                    cons.con_attributes      AS ""cons.con_attributes""
                FROM fprdb_RelationSchema AS rsch
                JOIN fprdb_Attribute AS attr      ON rsch.oid = attr.att_relschema_id
                JOIN fprdb_Type AS type           ON attr.att_type_id = type.oid
                JOIN fprdb_Constraint AS cons     ON cons.con_relschema_id = rsch.oid
                WHERE cons.con_type = 'p'
                ORDER BY ""rsch.oid"";";

            try
            {
                IDataReader reader = this.dbMgr.executeQuery(sql);

                if (reader.Read())
                {
                    bool hasNext = true;
                    while (hasNext)
                    {
                        // SỬA LỖI 1: Ép kiểu an toàn bằng Convert.ToInt64
                        long currentRelSchemaId = Convert.ToInt64(reader["rsch.oid"]);
                        string currentSchemaName = reader["rsch.relschema_name"].ToString();
                        List<string> primaryKey = reader["cons.con_attributes"].ToString().Split(',').ToList();

                        List<Field> fields = new List<Field>();
                        do
                        {
                            Field field = new Field(
                                reader["attr.att_name"].ToString(),
                                new FieldInfo(
                                    (FieldType)Enum.Parse(typeof(FieldType), reader["type.type_name"].ToString(), true),
                                    0
                                )
                            );
                            fields.Add(field);
                        }
                        // SỬA LỖI 2: Chỗ này lúc nãy là (int) làm sập code, giờ đã sửa thành Convert.ToInt64
                        while ((hasNext = reader.Read()) && Convert.ToInt64(reader["rsch.oid"]) == currentRelSchemaId);

                        schemas.Add(new FPRDBSchema(currentSchemaName, fields, primaryKey));
                    }
                }
                this.dbMgr.closeConnection();
                return schemas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<FPRDBRelation> getFPRDBRelations()
        {
            List<FPRDBRelation> rels = new List<FPRDBRelation>();
            string sql = @"
                SELECT
                    rel.oid             AS ""rel.oid"",
                    rel.rel_name        AS ""rel.rel_name"",
                    rel.rel_relation_schema AS ""rel.rel_relation_schema"",
                    rsch.oid            AS ""rsch.oid"",
                    rsch.relschema_name AS ""rsch.relschema_name"",
                    attr.att_relschema_id AS ""attr.att_relschema_id"",
                    attr.att_number     AS ""attr.att_number"",
                    attr.att_name       AS ""attr.att_name"",
                    attr.att_type_id    AS ""attr.att_type_id"",
                    attr.att_type_mod   AS ""attr.att_type_mod"",
                    attr.att_not_null   AS ""attr.att_not_null"",
                    type.oid            AS ""type.oid"",
                    type.type_name      AS ""type.type_name"",
                    type.type_type      AS ""type.type_type"",
                    cons.con_attributes AS ""cons.con_attributes""
                FROM fprdb_Relation AS rel
                JOIN fprdb_RelationSchema AS rsch 
                    ON rsch.oid = rel.rel_relation_schema
                JOIN fprdb_Attribute AS attr 
                    ON rsch.oid = attr.att_relschema_id
                JOIN fprdb_Type AS type 
                    ON attr.att_type_id = type.oid
                JOIN fprdb_Constraint AS cons 
                    ON cons.con_relschema_id = rsch.oid
                WHERE cons.con_type = 'p'
                ORDER BY ""rsch.oid""
            ";

            try
            {
                IDataReader reader = this.dbMgr.executeQuery(sql);
                bool hasNext = reader.Read();
                while (hasNext)
                {
                    //FPRDBSchema schema = new FPRDBSchema();
                    long currentRelSchemaId = (long)reader["rsch.oid"];
                    string currentSchemaName = (string)reader["rsch.relschema_name"];
                    List<string> primaryKey = ((string)reader["cons.con_attributes"]).Split(',').ToList();
                    long currentRelId = (long)reader["rel.oid"];
                    string currentRelName = (string)reader["rel.rel_name"];
                    List<Field> fields = new List<Field>();
                    do
                    {
                        var a1=(string)reader["rel.rel_name"];
                        long a3 = reader.IsDBNull(reader.GetOrdinal("attr.att_type_mod"))
                                   ? 0 : reader.GetInt64(reader.GetOrdinal("attr.att_type_mod"));
                        Field field = new Field(
                            (string)reader["attr.att_name"],
                            new FieldInfo(
                                (FieldType)Enum.Parse(typeof(FieldType), (string)reader["type.type_name"]),
                                a3
                            )
                        );
                        fields.Add(field);
                    } while ((hasNext = reader.Read()) && (long)reader["rel.oid"] == currentRelId);
                    rels.Add(new FPRDBRelation(currentRelName, 
                        new FPRDBSchema(currentSchemaName, fields, primaryKey))
                    );

                }
                this.dbMgr.closeConnection();
                return rels;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }
        public void loadDB(String filePath)
        {
            try
            {
                this.dbMgr.loadDB(filePath);
            }
            catch(IOException ex)
            {
                throw ex;
            }
        }

    }
}
