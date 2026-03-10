using BLL.DomainObject;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class SelectPlan:Plan
    {
        private Plan p;
        private SelectionCondition selectionCondition;
        public SelectPlan(Plan p, SelectionCondition selectionCondition)
        {
            this.p = p;
            this.selectionCondition = selectionCondition;
        }
        public Scan open(){
            throw new NotImplementedException();
        }
        public FPRDBSchema getSchema() => this.p.getSchema();
    }
}
