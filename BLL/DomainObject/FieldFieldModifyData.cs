using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class FieldFieldModifyData:ModifyData
    {
        private string field;

        public FieldFieldModifyData(string assignedField, string relation, string assigningField, SelectionCondition selectionCondition):base(relation, assigningField, selectionCondition)
        {
            this.field = assignedField;
        }

        public override object getAssignValue() => this.field;
    }
}
