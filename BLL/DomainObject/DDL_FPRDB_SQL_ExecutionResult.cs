using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class DDL_FPRDB_SQL_ExecutionResult:FPRDBSQLExecutionResult
    {
        public bool isSucess;

        public DDL_FPRDB_SQL_ExecutionResult(bool isSucess)
        {
            this.isSucess = isSucess;
        }
    }
}
