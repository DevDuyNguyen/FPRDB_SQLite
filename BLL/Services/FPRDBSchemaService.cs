using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DAO;

namespace BLL.Services
{
    public class FPRDBSchemaService
    {
        private FPRDBSchemaDAO fprdbSchemaDAO;
        private ConstraintService constraintService;

        public FPRDBSchemaService(FPRDBSchemaDAO fprdbSchemaDAO, ConstraintService constraintService)
        {
            this.fprdbSchemaDAO = fprdbSchemaDAO;
            this.constraintService = constraintService;
        }

        public bool defineFPRDBSchema(FPRDBSchemaDTO fprdbSchemaDTO)
        {
            return this.fprdbSchemaDAO.defineFPRDBSchema(fprdbSchemaDTO);
        }

    }
}
