using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Enums;

namespace BLL.DomainObject
{
    public class ConstraintData
    {
        private string name;
        private ConstraintType type;
        private List<string> fields;

        public ConstraintData(string name, ConstraintType type, List<string> fields)
        {
            this.name = name;
            this.type = type;
            this.fields = fields;
        }

        public string getName() => name;
        public ConstraintType getType() => type;
        public List<string> getFields() => fields;

    }
}
