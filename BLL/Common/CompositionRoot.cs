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
        //private RecursiveDescentParser parser, parser1, parser2;
        private Preprocessor preprocessor;
        private MetadataManager metadataMgr;
        private BasicUpdatePlanner updatePlanner;
        private SQLProcessor sqlProcessor;
        private ConstraintService constraintService;
        private FPRDBSchemaService fprdbSchemaService;
        private FPRDBSchemaDAO fprdbSchemaDAO;
        private QueryPlanner queryPlanner;
        private FPRDBRelationDAO fprdbRelationDAO;
        private FPRDBRelationService fprdbRelationService;
        private ConstraintDAO constraintDAO;

        public CompositionRoot()
        {
            Initialize();
        }

        public void Initialize()
        {
            //sql processing
            this.dbMgr = new DatabaseManager();
            this.lexer = new Lexer();
            this.metadataMgr = new MetadataManager(this.dbMgr);

            this.constraintDAO = new ConstraintDAOSQLite(this.dbMgr);
            this.constraintService = new ConstraintService(this.metadataMgr, this.constraintDAO);

            this.preprocessor = new Preprocessor(this.metadataMgr, this.constraintService);
            this.updatePlanner = new BasicUpdatePlanner(this.dbMgr, this.metadataMgr, this.getParser(), this.constraintService);
            this.queryPlanner = new BasicQueryPlanner(this.metadataMgr, this.dbMgr, this.getParser(), this.constraintService);

            this.sqlProcessor = new SQLProcessor(this.getParser(), this.updatePlanner, this.preprocessor, this.queryPlanner, this.lexer);

            

            //dao
            this.fuzzySetDAO = new FuzzySetDAOSQLite(this.dbMgr, this.metadataMgr);
            this.fprdbSchemaDAO = new FPRDBSchemaDAOSQLProcessor(this.sqlProcessor);
            this.fprdbRelationDAO = new FPRDBRelationDAOSQLProcessor(this.sqlProcessor);

            //service
            this.fuzzySetService = new FuzzySetService(this.fuzzySetDAO);
            this.databaseService = new DatabaseService(this.dbMgr);
            
            
            this.fprdbSchemaService = new FPRDBSchemaService(this.fprdbSchemaDAO, this.constraintService);
            this.fprdbRelationService = new FPRDBRelationService(this.fprdbRelationDAO);
        }
        private Lexer getLexer()
        {
            return new Lexer();
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
        public FPRDBSchemaService getFPRDBSchemaService() => this.fprdbSchemaService;
        public FPRDBRelationService getFPRDBRelationService() => this.fprdbRelationService;

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
        public RecursiveDescentParser getParser() => new RecursiveDescentParser(this.getLexer(), this.metadataMgr);
        //delete: for testing
        public Preprocessor getPreprocessor() => this.preprocessor;
        //delete: for testing
        public UpdatePlanner getUpdatePlanner() => this.updatePlanner;
        //delete: for testing
        public MetadataManager getMetaDataManger() => this.metadataMgr;
    }
}
