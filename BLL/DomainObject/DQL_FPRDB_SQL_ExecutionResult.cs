using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class DQL_FPRDB_SQL_ExecutionResult:FPRDBSQLExecutionResult
    {
        public Plan plan;

        public DQL_FPRDB_SQL_ExecutionResult(Plan plan)
        {
            this.plan=plan;
        }
    }
}
