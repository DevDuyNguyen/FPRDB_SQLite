using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public abstract class QueryData
    {
        public abstract FPRDBSchema getSchema();
    }
}
