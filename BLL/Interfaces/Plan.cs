using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.SQLProcessing;
using BLL.DomainObject;

namespace BLL.Interfaces
{
    internal interface Plan
    {
        public Scan open();
        public FPRDBSchema getSchema();
    }
}
