using BLL.DomainObject;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class ProductPlan:Plan
    {
        private Plan p1;
        private Plan p2;
        private MetadataManager metaDataMgr;
        private DatabaseManager dbMgr;
        private FPRDBSchema schema;

        public ProductPlan(Plan p1, Plan p2, MetadataManager metaDataMgr, DatabaseManager dbMgr)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.metaDataMgr = metaDataMgr;
            this.dbMgr = dbMgr;
            this.schema = new FPRDBSchema("", new List<Field>(), null);

            this.schema.addFieldsFromSchema(this.p1.getSchema());
            this.schema.addFieldsFromSchema(this.p2.getSchema());
        }

        public Scan open()
        {
            return new ProductScan(this.p1.open(), this.p2.open(), this.schema);
        }
        public FPRDBSchema getSchema() => this.schema;
    }
}
