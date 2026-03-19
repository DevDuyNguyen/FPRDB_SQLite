using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public abstract class ModifyData
    {
        private string relation;
        private string field;
        private SelectionCondition selectionCondition;

        protected ModifyData(string relation, string field, SelectionCondition selectionCondition)
        {
            this.relation = relation;
            this.field = field;
            this.selectionCondition = selectionCondition;
        }

        public string getRelation() => this.relation;
        public string getField() => this.field;
        public SelectionCondition getSelectionCondition() => this.selectionCondition;
        public abstract object getAssignValue();

    }
}
