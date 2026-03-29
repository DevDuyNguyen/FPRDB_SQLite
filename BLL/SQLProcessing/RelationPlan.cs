using BLL.DomainObject;
using BLL.Interfaces;
using BLL.Services;
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
        private RecursiveDescentParser parser;
        private ConstraintService constraintService;

        public RelationPlan(string relName, MetadataManager metaDataMgr, DatabaseManager dbMgr, RecursiveDescentParser parser, ConstraintService constraintService)
        {
            this.relationInfo = relationInfo;
            this.metaDataMgr = metaDataMgr;
            this.relationInfo = this.metaDataMgr.getRelation(relName);
            this.dbMgr = dbMgr;
            this.parser = parser;
            this.constraintService = constraintService;
        }
        public RelationPlan(string relName, MetadataManager metaDataMgr, DatabaseManager dbMgr, RecursiveDescentParser parser)
        {
            this.relationInfo = relationInfo;
            this.metaDataMgr = metaDataMgr;
            this.relationInfo = this.metaDataMgr.getRelation(relName);
            this.dbMgr = dbMgr;
            this.parser = parser;
        }

        public Scan open()
        {
            return new RelationScan(this.relationInfo, this.dbMgr, this.metaDataMgr, this.parser, this.constraintService);
        }
        public FPRDBSchema getSchema()
        {
            return this.relationInfo.getSchema();
        }
    }
}
