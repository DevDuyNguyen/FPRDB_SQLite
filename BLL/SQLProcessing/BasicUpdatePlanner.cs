using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
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
            string sql = $@"CREATE SCHEMA {data.getSchemaName()} (";
            foreach(Field field in data.getFields())
            {
                FieldInfo fieldInfo = field.getFieldInfo();
                string fieldDef = $"{field.getFieldName()} {fieldInfo.getType().ToString()}";
                if (fieldInfo.getType() == FieldType.VARCHAR)
                {
                    fieldDef += $"({fieldInfo.getTXTLength()})";
                }
                fieldDef += ",";
                sql += fieldDef;
            }
            string primaryKeyConstraint = $"CONSTRAINT {data.getPrimaryConstraintName} PRIMARY KEY (";
            foreach(string keyAttribute in data.getPrimarykey())
            {
                primaryKeyConstraint+=keyAttribute+",";
            }
            primaryKeyConstraint.TrimEnd(',');
            primaryKeyConstraint += ")";
            sql+= primaryKeyConstraint + ")";

            try
            {
                this.dbMgr.executeNonQuery(sql);
                return true;
            }
            catch (Exception ex) 
            {
                throw ex;
            }

        }


    }
}
