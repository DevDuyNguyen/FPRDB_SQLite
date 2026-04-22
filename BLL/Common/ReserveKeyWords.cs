using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    static class ReserveKeyWords
    {
        public static readonly HashSet<string> reservedKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
             "select", "from", "where", "natural", "join", "intersection","union", "except", "true", "false"
        };
    }
}
