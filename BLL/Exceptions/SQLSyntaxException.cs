using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class SQLSyntaxException:Exception
    {
        string nearToken;//the vinicity of syntax violation
        public SQLSyntaxException(string nearToken, string message) : base(message)
        {
            this.nearToken = nearToken;
        }
    }
}
