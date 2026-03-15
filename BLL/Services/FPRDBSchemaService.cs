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
        private ConstaintService constaintService;

        public FPRDBSchemaService(FPRDBSchemaDAO fprdbSchemaDAO, ConstaintService constaintService)
        {
            this.fprdbSchemaDAO = fprdbSchemaDAO;
            this.constaintService = constaintService;
        }

        public bool defineFPRDBSchema(FPRDBSchemaDTO fprdbSchemaDTO)
        {
            return this.fprdbSchemaDAO.defineFPRDBSchema(fprdbSchemaDTO);
        }

    }
}
