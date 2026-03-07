using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Enums;

namespace BLL.Interfaces
{
    public interface Constant
    {
        public Object getVal();
        public float probInterpretationOfRelationsOnFuzzySets(Constant c, CompareOperation compareOperation);

    }
}
