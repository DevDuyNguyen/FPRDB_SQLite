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
        private BasicUpdatePlanner updatePlanner;
        private SQLProcessor sqlProcessor;
        private ConstraintService constraintService;
        private QueryPlanner queryPlanner;
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
            this.metadataMgr = new MetadataManager(this.dbMgr);
            this.parser = new RecursiveDescentParser(this.lexer, this.metadataMgr);
            this.constraintService = new ConstraintService(this.metadataMgr);
            this.preprocessor = new Preprocessor(this.metadataMgr, this.constraintService);
            this.updatePlanner=new BasicUpdatePlanner(this.dbMgr);
            this.queryPlanner = new BasicQueryPlanner(this.metadataMgr, this.dbMgr);
            this.sqlProcessor = new SQLProcessor(this.parser, this.updatePlanner, this.preprocessor, ,this.lexer);

        }

        public DatabaseService getDatabaseService()
        {
            return this.databaseService;
        }
        public FuzzySetService getFuzzySetService()
        {
            return this.fuzzySetService;
        }
        public SQLProcessor getSQLProcessor() => this.sqlProcessor;
        public ConstraintService getConstraintService() => this.constraintService;

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
        //delete: for testing
        public UpdatePlanner getUpdatePlanner() => this.updatePlanner;
        //delete: for testing
        public MetadataManager getMetaDataManger() => this.metadataMgr;
    }
}
