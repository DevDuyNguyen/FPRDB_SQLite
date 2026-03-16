using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.Interfaces
{
    public interface QueryPlanner
    {
        public Plan createPlan(QueryData data);
    }
}
