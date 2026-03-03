using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPRDB_SQLite
{
    public static class CompositionRoot
    {
        public static DatabaseManager dbMgr;
        public static DatabseService databaseService;
        static CompositionRoot()
        {
            dbMgr = new DatabaseManager();
            databaseService = new DatabseService();
        }
    }
}
