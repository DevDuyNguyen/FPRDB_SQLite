using BLL.DTO;
using BLL.Interfaces;
using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DAO
{
    public class FuzzySetDAOSQLite:FuzzySetDAO
    {
        private DatabaseManager databaseManager;
        private DatabaseManager databseExportImport;

        public FuzzySetDAOSQLite(DatabaseManager databaseManager)
        {
            this.databaseManager = databaseManager;
        }

        public DiscreteFuzzySet<T> createDiscreteFuzzySet<T>(DiscreteFuzzySetDTO<T> fuzzySet)
        {

        }
    }
}
