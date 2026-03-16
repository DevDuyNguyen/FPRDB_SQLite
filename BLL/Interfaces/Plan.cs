using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainObject;

namespace BLL.Interfaces
{
    public interface Plan
    {
        public Scan open();
        public FPRDBSchema getSchema();
    }
}
