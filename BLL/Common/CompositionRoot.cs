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
        private RecursiveDescentParser parser;
        private Preprocessor preprocessor;
        private MetadataManager metadataMgr;

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
            this.lexer = new Lexer();
            this.parser = new RecursiveDescentParser(this.lexer);
            this.metadataMgr = new MetadataManager(this.dbMgr);
            this.preprocessor = new Preprocessor(this.metadataMgr);

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
        //delete: for testing
        public RecursiveDescentParser getParser() => this.parser;
        //delete: for testing
        public Preprocessor getPreprocessor() => this.preprocessor;

    }
}
