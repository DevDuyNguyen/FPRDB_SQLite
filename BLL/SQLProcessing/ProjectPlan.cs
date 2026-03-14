using BLL.DomainObject;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class ProjectPlan:Plan
    {
        private Plan p;
        private FPRDBSchema schema;
        public ProjectPlan(Plan p, List<string> fieldList)
        {
            this.p = p;
            FPRDBSchema schma = this.p.getSchema();
            List<Field> fields=new List<Field>();
            foreach(Field field in schma.getFields())
            {
                if (fieldList.Contains(field.getFieldName()))
                    fields.Add(field);
            }
            if (fieldList.Count > fields.Count)
                throw new Exception("Some selected fields aren't included");
            this.schema = new FPRDBSchema(null, fields, null);
        }
        public Scan open()
        {
            return new ProjectScan(this.p.open(), this.schema); 
        }
        public FPRDBSchema getSchema() => this.schema;


    }
}
