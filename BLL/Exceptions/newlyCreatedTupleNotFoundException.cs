using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class newlyCreatedTupleNotFoundException:Exception
    {
        public newlyCreatedTupleNotFoundException() { }
        public newlyCreatedTupleNotFoundException(string mess) : base(mess)
        {

        }
    }
}
