using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPRDB_SQLite
{
    public static class CompositionRoot
    {
        public static DatabseManager dbMgr;
        public static DatabseService databaseService;
        static CompositionRoot()
        {
            dbMgr = new DatabseManager();
            databaseService = new DatabseService();
        }
    }
}
