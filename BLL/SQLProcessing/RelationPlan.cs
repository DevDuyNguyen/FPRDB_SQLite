using BLL.DomainObject;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class RelationPlan:Plan
    {
        private FPRDBRelation relationInfo;
        private MetadataManager metaDataMgr;
        private DatabaseManager dbMgr;

        public RelationPlan(string relName, MetadataManager metaDataMgr, DatabaseManager dbMgr)
        {
            this.relationInfo = relationInfo;
            this.metaDataMgr = metaDataMgr;
            this.relationInfo = this.metaDataMgr.getRelation(relName);
            this.dbMgr = dbMgr;
        }

        public Scan open()
        {
            return new RelationScan(this.relationInfo, this.dbMgr, this.metaDataMgr);
        }
        public FPRDBSchema getSchema()
        {
            return this.relationInfo.getSchema();
        }
    }
}
