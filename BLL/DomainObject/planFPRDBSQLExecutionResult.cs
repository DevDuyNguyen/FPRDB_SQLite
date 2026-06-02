using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class planFPRDBSQLExecutionResult:FPRDBSQLExecutionResult
    {
        public Plan plan;

        public planFPRDBSQLExecutionResult(Plan plan)
        {
            this.plan=plan;
        }
    }
}
