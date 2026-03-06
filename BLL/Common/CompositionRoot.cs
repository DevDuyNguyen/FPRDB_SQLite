using BLL.Interfaces;
using BLL.Services;
using BLL.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.SQLProcessing;

namespace BLL.Common
{
    public class CompositionRoot
    {
        private DatabaseManager dbMgr;
        private DatabaseService databaseService;
        private FuzzySetService fuzzySetService;
        private FuzzySetDAO fuzzySetDAO;
        private DatabaseManager databseExportImport;
        private Lexer lexer;
        private Parser parser;

        public CompositionRoot()
        {
            Initialize();
        }

        public void Initialize()
        {
            this.dbMgr = new DatabaseManager();
            this.databaseService = new DatabaseService(this.dbMgr);
            this.fuzzySetDAO = new FuzzySetDAOSQLite(this.dbMgr);
            this.fuzzySetService = new FuzzySetService(this.fuzzySetDAO);


        }

        public DatabaseService getDatabaseService()
        {
            return this.databaseService;
        }
        public FuzzySetService getFuzzySetService()
        {
            return this.fuzzySetService;
        }

        //delete: for testing
        public FuzzySetDAO getFuzzySetDAO()
        {
            return this.fuzzySetDAO;
        }
        //delete: for testing
        public DatabaseManager getDBMgr()
        {
            return this.dbMgr;
        }

    }
}
