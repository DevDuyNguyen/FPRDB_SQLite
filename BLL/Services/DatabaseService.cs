using BLL.Common;
using BLL.DomainObject;
using BLL.DTO;
using BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace BLL.Services
{
    class SystemCatalogAttribute
    {
        public string name;
        public string type;
        public bool isPrimayKeyAttribute;

        public SystemCatalogAttribute(string name, string type, bool isPrimayKeyAttribute)
        {
            this.name = name;
            this.type = type;
            this.isPrimayKeyAttribute = isPrimayKeyAttribute;
        }
    }
    class SystemCatalogForeignKeyMapping
    {
        public string referencedTable;
        public string fromAttribute;
        public string toAttribute;

        public SystemCatalogForeignKeyMapping(string referencedTable, string fromAttribute, string toAttribute)
        {
            this.referencedTable = referencedTable;
            this.fromAttribute = fromAttribute;
            this.toAttribute = toAttribute;

        }
        public override bool Equals(object obj) 
        {
            var item = obj as SystemCatalogForeignKeyMapping;
            if (this.referencedTable == item.referencedTable
                       && this.fromAttribute == item.fromAttribute
                       && this.toAttribute == item.toAttribute)
                return true;
            else
                return false;
               
        }
    }
    class SystemCatalogTable
    {
        public string name;
        public List<SystemCatalogAttribute> attributes;
        public List<SystemCatalogForeignKeyMapping> foreignKeyMappings;
        //public List<string> primaryKey;

        public SystemCatalogTable(string name, List<SystemCatalogAttribute> attributes, List<SystemCatalogForeignKeyMapping> foreignKeyMappings)//, List<string> primaryKey)
        {
            this.name = name;
            this.attributes = attributes;
            this.foreignKeyMappings = foreignKeyMappings;
            //this.primaryKey = primaryKey;
        }
        public bool matchAttributeDefinition(List<SystemCatalogAttribute> attributeDefs)
        {
            if (this.attributes == null)
            {
                if (attributeDefs == null || attributeDefs.Count == 0)
                    return true;
                else
                    return false;
            }
            if (attributeDefs.Count != this.attributes.Count)
                return false;
            int index;
            foreach(SystemCatalogAttribute att in this.attributes)
            {
                index = -1;
                for(int i=0; i< attributeDefs.Count; ++i)
                {
                    if (att.name == attributeDefs[i].name)
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1 || att.type != attributeDefs[index].type || att.isPrimayKeyAttribute!= attributeDefs[index].isPrimayKeyAttribute)
                    return false;
            }
            return true;
        }
        public bool matchForeignKeys(List<SystemCatalogForeignKeyMapping> foreignKeyMappingDefs)
        {
            if (this.foreignKeyMappings == null)
            {
                if (foreignKeyMappingDefs == null || foreignKeyMappingDefs.Count == 0)
                    return true;
                else
                    return false;
            }
            if (foreignKeyMappingDefs.Count != this.foreignKeyMappings.Count)
                return false;
            int index;
            foreach (SystemCatalogForeignKeyMapping fkMapping in this.foreignKeyMappings)
            {
                index = -1;
                for (int i = 0; i < foreignKeyMappingDefs.Count; ++i)
                {
                    if (fkMapping.Equals(foreignKeyMappingDefs[i]))
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                    return false;
            }
            return true;
        }
    }
    public class DatabaseService
    {
        private DatabaseManager dbMgr;
        public string getFPRDBDotExtenstion() => this.dbMgr.getFPRDBDotExtenstion();

        public DatabaseService(DatabaseManager dbMgr)
        {
            this.dbMgr = dbMgr;
        }
        private bool isSystemCatalogTableExist(string tableName)
        {
            using(IDataReader r=this.dbMgr.executeQuery($"select 1 from sqlite_master where type='table' and name='{tableName}';"))
            {
                return r.Read();
            }
        }
        private bool isMatchSystemCatalogTableAttributeDefinition(SystemCatalogTable table, out List<SystemCatalogAttribute> attributeDefs)
        {
            using (IDataReader reader = this.dbMgr.executeQuery($"PRAGMA table_info('{table.name}')"))
            {
                string name;
                string type;
                bool isPk;
                //List<SystemCatalogAttribute> attributeDefs = new List<SystemCatalogAttribute>();
                attributeDefs = new List<SystemCatalogAttribute>();
                while (reader.Read())
                {
                    name = reader["name"].ToString();
                    type = reader["type"].ToString();
                    isPk = (long)reader["pk"] > 0;

                    attributeDefs.Add(new SystemCatalogAttribute(name, type, isPk));
                }
                return table.matchAttributeDefinition(attributeDefs);
            }
        }
        private bool isMatchSystemCatalogTableForeignKey(SystemCatalogTable table)
        {
            using (IDataReader reader = this.dbMgr.executeQuery($"PRAGMA foreign_key_list('{table.name}')"))
            {
                List<SystemCatalogForeignKeyMapping> foreignKeys = new List<SystemCatalogForeignKeyMapping>();
                string fromColumn;
                string toTable;
                string toColumn;
                while (reader.Read())
                {
                    fromColumn = reader["from"].ToString();
                    toTable = reader["table"].ToString();
                    toColumn = reader["to"].ToString();

                    foreignKeys.Add(new SystemCatalogForeignKeyMapping(toTable, fromColumn, toColumn));
                }
                return table.matchForeignKeys(foreignKeys);
            }
        }

        private bool is_system_catalog_table_definition_exist(SystemCatalogTable table)
        {
            
            //is system catalog table exist
            if (!isSystemCatalogTableExist(table.name))
                throw new InvalidFPRDBDatabaseFile($"System catalog table {table.name} doesn't exist");
            //is the attribute definition (name, type, is the primary key) of table matched
            List<SystemCatalogAttribute> attributeDefs;
            if (!isMatchSystemCatalogTableAttributeDefinition(table, out attributeDefs))
                throw new InvalidFPRDBDatabaseFile($"Attribute definitions for System catalog table {table.name} doesn't match the data design document");
            //is the foreign keys matched
            if (!isMatchSystemCatalogTableForeignKey(table))
                throw new InvalidFPRDBDatabaseFile($"Foreign keys for System catalog table {table.name} doesn't match the data design document");
            return true;
        }

        private bool isValidFPRDBDatabaseFileStructure(string filePath)
        {
            //the structure follows the System catalog
            SystemCatalogTable fprdb_RelationSchema = new SystemCatalogTable(
                "fprdb_RelationSchema",
                new List<SystemCatalogAttribute>
                {
                    new SystemCatalogAttribute("oid", "INTEGER", true),
                    new SystemCatalogAttribute("relschema_name", "TEXT", false),
                },
                null
                );
            is_system_catalog_table_definition_exist(fprdb_RelationSchema);

            SystemCatalogTable fprdb_Type = new SystemCatalogTable(
                "fprdb_Type",
                new List<SystemCatalogAttribute>
                {
                    new SystemCatalogAttribute("oid", "INTEGER", true),
                    new SystemCatalogAttribute("type_name", "TEXT", false),
                    new SystemCatalogAttribute("type_type", "TEXT", false)
                },
                null
                );
            is_system_catalog_table_definition_exist(fprdb_Type);

            SystemCatalogTable fprdb_Attribute = new SystemCatalogTable(
                "fprdb_Attribute",
                new List<SystemCatalogAttribute>
                {
                    new SystemCatalogAttribute("att_relschema_id", "INTEGER", false),
                    new SystemCatalogAttribute("oid", "INTEGER", true),
                    new SystemCatalogAttribute("att_name", "TEXT", false),
                    new SystemCatalogAttribute("att_type_id", "INTEGER", false),
                    new SystemCatalogAttribute("att_type_mod", "INTEGER", false),
                    new SystemCatalogAttribute("att_not_null", "BOOLEAN", false)
                },
                new List<SystemCatalogForeignKeyMapping>
                {
                    new SystemCatalogForeignKeyMapping("fprdb_Type", "att_type_id", "oid"),
                    new SystemCatalogForeignKeyMapping("fprdb_RelationSchema", "att_relschema_id", "oid")
                }
                );
            is_system_catalog_table_definition_exist(fprdb_Attribute);

            SystemCatalogTable fprdb_Constraint = new SystemCatalogTable(
                "fprdb_Constraint",
                new List<SystemCatalogAttribute>
                {
                    new SystemCatalogAttribute("oid", "INTEGER", true),
                    new SystemCatalogAttribute("con_name", "TEXT", false),
                    new SystemCatalogAttribute("con_type", "TEXT", false),
                    new SystemCatalogAttribute("con_relation_id", "INTEGER", false),
                    new SystemCatalogAttribute("con_referenced_relation_id", "INTEGER", false),
                    new SystemCatalogAttribute("con_attributes", "TEXT", false),
                    new SystemCatalogAttribute("con_referenced_attributes", "TEXT", false),
                    new SystemCatalogAttribute("con_relschema_id", "INTEGER", false)
                },
                new List<SystemCatalogForeignKeyMapping>
                {
                    new SystemCatalogForeignKeyMapping("fprdb_RelationSchema", "con_relschema_id", "oid"),
                    new SystemCatalogForeignKeyMapping("fprdb_Relation", "con_referenced_relation_id", "oid"),
                    new SystemCatalogForeignKeyMapping("fprdb_Relation", "con_relation_id", "oid")
                }
                );
            is_system_catalog_table_definition_exist(fprdb_Constraint);

            SystemCatalogTable fprdb_Relation = new SystemCatalogTable(
                "fprdb_Relation",
                new List<SystemCatalogAttribute>
                {
                    new SystemCatalogAttribute("oid", "INTEGER", true),
                    new SystemCatalogAttribute("rel_name", "TEXT", false),
                    new SystemCatalogAttribute("rel_relation_schema", "INTEGER", false)
                },
                new List<SystemCatalogForeignKeyMapping>
                {
                    new SystemCatalogForeignKeyMapping("fprdb_RelationSchema", "rel_relation_schema", "oid")
                }
                );
            is_system_catalog_table_definition_exist(fprdb_Relation);

            SystemCatalogTable fprdb_FuzzySet = new SystemCatalogTable(
                "fprdb_FuzzySet",
                new List<SystemCatalogAttribute>
                {
                    new SystemCatalogAttribute("oid", "INTEGER", true),
                    new SystemCatalogAttribute("fuzzset_name", "TEXT", false),
                    new SystemCatalogAttribute("fuzzset_type_id", "INTEGER", false)
                },
                new List<SystemCatalogForeignKeyMapping>
                {
                    new SystemCatalogForeignKeyMapping("fprdb_Type", "fuzzset_type_id", "oid")
                }
                );
            is_system_catalog_table_definition_exist(fprdb_FuzzySet);

            SystemCatalogTable fprdb_DiscreteFuzzySet = new SystemCatalogTable(
                "fprdb_DiscreteFuzzySet",
                new List<SystemCatalogAttribute>
                {
                    new SystemCatalogAttribute("oid", "INTEGER", true),
                    new SystemCatalogAttribute("fuzzset_x", "TEXT", false),
                    new SystemCatalogAttribute("fuzzset_membership_degree", "TEXT", false)
                },
                new List<SystemCatalogForeignKeyMapping>
                {
                    new SystemCatalogForeignKeyMapping("fprdb_FuzzySet", "oid", "oid")
                }
                );
            is_system_catalog_table_definition_exist(fprdb_DiscreteFuzzySet);

            SystemCatalogTable fprdb_ContinousFuzzySet = new SystemCatalogTable(
                "fprdb_ContinousFuzzySet",
                new List<SystemCatalogAttribute>
                {
                    new SystemCatalogAttribute("oid", "INTEGER", true),
                    new SystemCatalogAttribute("fuzzset_bottom_left", "REAL", false),
                    new SystemCatalogAttribute("fuzzset_top_left", "REAL", false),
                    new SystemCatalogAttribute("fuzzset_top_right", "REAL", false),
                    new SystemCatalogAttribute("fuzzset_bottom_right", "REAL", false)
                },
                new List<SystemCatalogForeignKeyMapping>
                {
                    new SystemCatalogForeignKeyMapping("fprdb_FuzzySet", "oid", "oid")
                }
                );
            is_system_catalog_table_definition_exist(fprdb_ContinousFuzzySet);

            SystemCatalogTable FPRDB_Rel_FuzzSet = new SystemCatalogTable(
                "FPRDB_Rel_FuzzSet",
                new List<SystemCatalogAttribute>
                {
                    new SystemCatalogAttribute("rel_oid", "INTEGER", true),
                    new SystemCatalogAttribute("fuzzset_oid", "INTEGER", true),
                    new SystemCatalogAttribute("no", "INTEGER", false)
                },
                new List<SystemCatalogForeignKeyMapping>
                {
                    new SystemCatalogForeignKeyMapping("fprdb_FuzzySet", "fuzzset_oid", "oid"),
                    new SystemCatalogForeignKeyMapping("fprdb_Relation", "rel_oid", "oid")
                }
                );
            is_system_catalog_table_definition_exist(FPRDB_Rel_FuzzSet);

            return true;
        }
        public void createDB(string filePath)
        {
            this.dbMgr.createDB(filePath);
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

        public List<FPRDBSchemaDTO> getFPRDBSchemas()
        {
            List<FPRDBSchemaDTO> schemas = new List<FPRDBSchemaDTO>();
            string sql = @"
                SELECT 
                    rsch.oid                 AS ""rsch.oid"", 
                    rsch.relschema_name      AS ""rsch.relschema_name"",
                    attr.att_relschema_id    AS ""attr.att_relschema_id"",
                    attr.oid          		AS ""attr.oid"", 
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
                WHERE cons.con_type = 'IDENTITY'
                ORDER BY rsch.oid;";

            IDataReader reader = this.dbMgr.executeQuery(sql);

            if (reader.Read())
            {
                bool hasNext = true;
                while (hasNext)
                {
                    // SỬA LỖI 1: Ép kiểu an toàn bằng Convert.ToInt64
                    int currentRelSchemaId = Convert.ToInt32(reader["rsch.oid"]);
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

                    schemas.Add(new FPRDBSchemaDTO(currentSchemaName, fields, primaryKey, currentRelSchemaId));
                }
            }
            this.dbMgr.closeConnection();
            return schemas;

        }

        public List<FPRDBRelationDTO> getFPRDBRelations()
        {
            List<FPRDBRelationDTO> rels = new List<FPRDBRelationDTO>();
            string sql = @"
                SELECT
                    rel.oid             AS ""rel.oid"",
                    rel.rel_name        AS ""rel.rel_name"",
                    rel.rel_relation_schema AS ""rel.rel_relation_schema"",
                    rsch.oid            AS ""rsch.oid"",
                    rsch.relschema_name AS ""rsch.relschema_name"",
                    attr.att_relschema_id AS ""attr.att_relschema_id"",
                    attr.oid     AS ""attr.oid"",
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
                WHERE cons.con_type = 'IDENTITY'
                ORDER BY rel.oid
            ";

            IDataReader reader = this.dbMgr.executeQuery(sql);
            bool hasNext = reader.Read();
            while (hasNext)
            {
                //FPRDBSchema schema = new FPRDBSchema();
                int currentRelSchemaId = Convert.ToInt32(reader["rsch.oid"]);
                string currentSchemaName = (string)reader["rsch.relschema_name"];
                List<string> primaryKey = ((string)reader["cons.con_attributes"]).Split(',').ToList();
                int currentRelId = Convert.ToInt32(reader["rel.oid"]);
                string currentRelName = (string)reader["rel.rel_name"];
                List<Field> fields = new List<Field>();
                do
                {
                    var a1 = (string)reader["rel.rel_name"];
                    int a3 = reader.IsDBNull(reader.GetOrdinal("attr.att_type_mod"))
                               ? 0 : reader.GetInt32(reader.GetOrdinal("attr.att_type_mod"));
                    Field field = new Field(
                        (string)reader["attr.att_name"],
                        new FieldInfo(
                            (FieldType)Enum.Parse(typeof(FieldType), (string)reader["type.type_name"]),
                            a3
                        )
                    );
                    fields.Add(field);
                } while ((hasNext = reader.Read()) && (long)reader["rel.oid"] == currentRelId);
                rels.Add(new FPRDBRelationDTO(
                    currentRelName,
                    new FPRDBSchemaDTO(currentSchemaName, fields, primaryKey, currentRelSchemaId),
                    currentSchemaName,
                    currentRelId
                    )
                );

            }
            this.dbMgr.closeConnection();
            return rels;

        }
        public void loadDB(String filePath)
        {
            //file dot extension is .fprdb
            int dotExtensionStartIndex = filePath.LastIndexOf(".");
            if (dotExtensionStartIndex == -1)
                throw new InvalidOperationException("The database file to be loaded must ends with .fprdb");
            string dotExtension = filePath.Substring(dotExtensionStartIndex);
            if (dotExtension != this.dbMgr.getFPRDBDotExtenstion())
                throw new InvalidOperationException("The database file to be loaded must ends with .fprdb");
            //is a database file of sqlite
            this.dbMgr.loadDB(filePath);
            //check if the selected file follow the designed system catalog of FPRDB
            isValidFPRDBDatabaseFileStructure(filePath);
        }
        public List<string> getFieldTypes()
        {
            return new List<string>{"INT", "FLOAT", "CHAR", "VARCHAR", "BOOLEAN",
                "DIST_FUZZYSET_INT", "DIST_FUZZYSET_FLOAT", "DIST_FUZZYSET_TEXT", "CONT_FUZZYSET"};
        }
        public List<FieldType> getDefineDomainForDistFuzzSet()
        {
            return FieldTypeUtilities.getDefineDomainForFuzzySet();
        }
        public List<string> getFuzzySetNameByType(FieldType fsType)
        {
            List<string> names = new List<string>();
            string sql;
            if(fsType == FieldType.contFS)
            {
                sql = @"
                    SELECT fs.fuzzset_name
                    FROM fprdb_FuzzySet AS fs
                    INNER JOIN fprdb_ContinousFuzzySet AS conFS on fs.oid=conFS.oid
                ";
            }
            else
            {
                if (fsType == FieldType.distFS_INT)
                {
                    sql = @"
                        SELECT fs.fuzzset_name
                        FROM fprdb_FuzzySet AS fs
                        INNER JOIN fprdb_DiscreteFuzzySet AS distFS on fs.oid=distFS.oid
                        INNER JOIN fprdb_Type AS type ON fs.fuzzset_type_id=type.oid
                        WHERE type.type_name='INT'
                    ";
                }
                else if (fsType == FieldType.distFS_FLOAT)
                {
                    sql = @"
                        SELECT fs.fuzzset_name
                        FROM fprdb_FuzzySet AS fs
                        INNER JOIN fprdb_DiscreteFuzzySet AS distFS on fs.oid=distFS.oid
                        INNER JOIN fprdb_Type AS type ON fs.fuzzset_type_id=type.oid
                        WHERE type.type_name='FLOAT'
                    ";
                }
                else if (fsType == FieldType.distFS_TEXT)
                {
                    sql = @"
                        SELECT fs.fuzzset_name
                        FROM fprdb_FuzzySet AS fs
                        INNER JOIN fprdb_DiscreteFuzzySet AS distFS on fs.oid=distFS.oid
                        INNER JOIN fprdb_Type AS type ON fs.fuzzset_type_id=type.oid
                        WHERE type.type_name='VARCHAR'
                    ";
                }
                else
                {
                    throw new InvalidOperationException($"No fuzzy set has universe of discourse of {fsType}");
                }

            }
            using (IDataReader r = this.dbMgr.executeQuery(sql))
            {
                while (r.Read())
                {
                    names.Add(r["fuzzset_name"] as string);
                }
            }
            return names;
        }

    }
}
