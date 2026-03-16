using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class SelectField
    {
        public string relation;
        public string field;

        public SelectField(string relation, string field)
        {
            this.relation = relation;
            this.field = field;
        }
    }
}
