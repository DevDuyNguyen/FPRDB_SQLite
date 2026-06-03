using BLL.Interfaces;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class DQL_FPRDB_SQL_ExecutionResult:FPRDBSQLExecutionResult
    {
        public InMemoryScan resultedRelation;

        public DQL_FPRDB_SQL_ExecutionResult(InMemoryScan resultedRelation)
        {
            this.resultedRelation= resultedRelation;
        }
    }
}
