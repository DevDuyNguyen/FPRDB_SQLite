using BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    public class CompositionRoot
    {
        private DatabaseManager dbMgr;
        public DatabaseService databaseService;

        public CompositionRoot()
        {
            Initialize();
        }

        public void Initialize()
        {
            this.dbMgr = new DatabaseManager();
            this.databaseService = new DatabaseService(this.dbMgr);
        }

        public DatabaseService getDatabaseService()
        {
            return this.databaseService;
        }

    }
}
