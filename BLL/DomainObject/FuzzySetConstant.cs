using BLL.Enums;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class FuzzySetConstant:Constant
    {
        private string val;

        public FuzzySetConstant(string val)
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
