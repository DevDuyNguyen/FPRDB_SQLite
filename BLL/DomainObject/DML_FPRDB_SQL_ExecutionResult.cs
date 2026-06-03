using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class DML_FPRDB_SQL_ExecutionResult : FPRDBSQLExecutionResult
    {
        public int numberTuplesAffected;

        public DML_FPRDB_SQL_ExecutionResult(int numberTuplesAffected)
        {
            this.numberTuplesAffected = numberTuplesAffected;
        }
    }
}
