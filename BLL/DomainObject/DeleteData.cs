using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class DeleteData
    {
        public string relation;
        public SelectionCondition selectionCondition;

        public DeleteData(string relation, SelectionCondition selectionCondition)
        {
            this.relation = relation;
            this.selectionCondition = selectionCondition;
        }
    }
}
