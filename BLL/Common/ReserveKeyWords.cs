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
            "select", "from", "where", "and", "or", "not", "natural", "join",
            "union", "intersect", "except", "not", "and", "or",
            "create", "schema", "int", "float", "char", "varchar", "boolean",
            "dist_fuzzyset_int", "dist_fuzzyset_float", "dist_fuzzyset_text",
            "cont_fuzzyset", "constraint", "primary", "key",
            "relation", "on", "insert", "into", "values", "update", "set", "delete", "drop",
             "true", "false", "on", "from", "take", "null"
        };
    }
}
