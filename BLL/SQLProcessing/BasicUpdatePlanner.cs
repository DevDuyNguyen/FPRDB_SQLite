using BLL.DomainObject;
using BLL.Enums;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string createTableForSchemaSQL = $@"CREATE TABLE {data.getSchemaName()} (";
            
            foreach(Field field in data.getFields())
            {
                FieldInfo fieldInfo = field.getFieldInfo();


                string fieldDef = $"{field.getFieldName()} {fieldInfo.getType().ToString()}";
                if (fieldInfo.getType() == FieldType.VARCHAR)
                {
                    fieldDef += $"({fieldInfo.getTXTLength()})";
                }
                fieldDef += ",";
                createTableForSchemaSQL += fieldDef;
            }

            string primaryKeyConstraint = $"PRIMARY KEY (";
            foreach(string keyAttribute in data.getPrimarykey())
            {
                primaryKeyConstraint+=keyAttribute+",";
            }
            primaryKeyConstraint=primaryKeyConstraint.TrimEnd(',');
            primaryKeyConstraint += ")";
            createTableForSchemaSQL += primaryKeyConstraint + ")";


            this.dbMgr.executeNonQuery(createTableForSchemaSQL);
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
                    VALUES ({schemaID}, '{field.getFieldName()}', {typeID}, {fieldInfo.getTXTLength()}, {primaryKeyConstraint.Contains(field.getFieldName())})
                ";
                this.dbMgr.executeNonQuery(insertAttributeSQL);
            }

            string primaryKeyConstraintSQL = $@"INSERT INTO fprdb_Constraint (con_name, con_type, con_attributes, con_relschema_id)
                VALUES ('{data.primaryConstraintName}','{ConstraintType.IDENTITY.ToString()}','{string.Join(",", data.getPrimarykey())}',{schemaID})
            ";
            this.dbMgr.executeNonQuery(primaryKeyConstraintSQL);

            return true;
            
  

        }


    }
}
