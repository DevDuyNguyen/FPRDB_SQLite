using BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace BLL.DomainObject
{
    public class FloatConstant:Constant
    {
        private float val;

        public FloatConstant(float val)
        {
            this.val = val;
        }

        public Object getVal() => this.val;
        public float probInterpretationOfRelationsOnFuzzySets(Constant c, CompareOperation compareOperation)
        {
            throw new NotImplementedException();
        }

    }
}
