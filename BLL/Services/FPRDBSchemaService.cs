using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DAO;
using BLL.SQLProcessing;

namespace BLL.Services
{
    public class FPRDBSchemaService
    {
        private FPRDBSchemaDAO fprdbSchemaDAO;
        private ConstraintService constraintService;
        private SQLProcessor sqlProcessor;
        public FPRDBSchemaService(FPRDBSchemaDAO fprdbSchemaDAO, ConstraintService constraintService, SQLProcessor sqlProcessor)
        {
            this.fprdbSchemaDAO = fprdbSchemaDAO;
            this.constraintService = constraintService;
            this.sqlProcessor = sqlProcessor;
        }

        public bool defineFPRDBSchema(FPRDBSchemaDTO fprdbSchemaDTO)
        {
            return this.fprdbSchemaDAO.defineFPRDBSchema(fprdbSchemaDTO);
        }
        public void removeFPRDBSchema(FPRDBSchemaDTO fprdbSchemaDTO)
        {
            this.sqlProcessor.executeUpdate($"DROP SCHEMA {fprdbSchemaDTO.schemaName}");
        }
    }
}
