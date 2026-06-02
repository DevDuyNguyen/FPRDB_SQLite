using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class IntFPRDBSQLExecutionResult:FPRDBSQLExecutionResult
    {
        public int numberTuplesAffected;

        public IntFPRDBSQLExecutionResult(int numberTuplesAffected)
        {
            this.numberTuplesAffected = numberTuplesAffected;
        }
    }
}
