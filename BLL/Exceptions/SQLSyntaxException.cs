using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class SQLSyntaxException:Exception
    {
        public string nearToken;//the vinicity of syntax violation
        public int line, column;
        public SQLSyntaxException(string nearToken,
            int line, int column,
            string message) : base(message)
        {
            this.nearToken = nearToken;
            this.line = line;
            this.column = column;
        }
    }
}
