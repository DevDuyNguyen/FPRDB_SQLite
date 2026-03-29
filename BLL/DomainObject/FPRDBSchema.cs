using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL.DomainObject
{
    public class FPRDBSchema
    {
        private String schemaName;
        private List<Field> fields;
        public List<string> primarykey;
        public string primaryConstraintName;
        private int oid;

        public FPRDBSchema(string schemaName, List<Field> fields, List<string> primarykey)
        {
            this.schemaName = schemaName;
            this.fields = fields;
            this.primarykey = primarykey;
            this.oid = -1;
        }
        public FPRDBSchema(string schemaName, List<Field> fields, List<string> primarykey, string primaryConstraintName)
        {
            this.schemaName = schemaName;
            this.fields = fields;
            this.primarykey = primarykey;
            this.primaryConstraintName = primaryConstraintName;
            this.oid = -1;
        }
        public FPRDBSchema(string schemaName, List<Field> fields, List<string> primarykey, string primaryConstraintName, int oid)
        {
            this.schemaName = schemaName;
            this.fields = fields;
            this.primarykey = primarykey;
            this.primaryConstraintName = primaryConstraintName;
            this.oid = oid;
        }
        public string getPrimaryConstraintName() => this.primaryConstraintName;

        public String getSchemaName()
        {
            return schemaName;
        }
        public List<Field> getFields()
        {
            return fields;
        }
        public void setSchemaName(String schemaName)
        {
            this.schemaName = schemaName;
        }
        public void setFields(List<Field> fields)
        {
            this.fields = fields;
        }
        public List<string> getPrimarykey()
        {
            return this.primarykey;
        }
        public Field getFieldByName(string name)
        {
            foreach(Field field in this.fields)
            {
                if (field.getFieldName() == name)
                {
                    return field;
                }
            }
            return null;
        }
        public void addFieldsFromSchema(FPRDBSchema schema)
        {
            //add fields
            foreach(Field f in schema.getFields())
            {
                this.fields.Add(f);
            }
            //add primary key
            this.primarykey = new List<string>();
            foreach(string keyName in schema.getPrimarykey())
            {
                this.primarykey.Add(keyName);
            }
        }
        public bool hasField(string fldName)
        {
            foreach(Field f in this.fields)
            {
                if (f.getFieldName() == fldName)
                    return true;
            }
            return false;
        }
        public FPRDBSchemaDTO toDTO()
        {
            return new FPRDBSchemaDTO(
                this.schemaName,
                this.fields,
                this.primarykey,
                this.oid
                );
        }
    }
}
