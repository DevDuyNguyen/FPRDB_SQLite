using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DAO;
using BLL.SQLProcessing;
using BLL.DomainObject;

namespace BLL.Services
{
    public class FPRDBSchemaService
    {
        private FPRDBSchemaDAO fprdbSchemaDAO;
        private ConstraintService constraintService;
        //private SQLProcessor sqlProcessor;
        public FPRDBSchemaService(FPRDBSchemaDAO fprdbSchemaDAO, ConstraintService constraintService)//, SQLProcessor sqlProcessor)
        {
            this.fprdbSchemaDAO = fprdbSchemaDAO;
            this.constraintService = constraintService;
            //this.sqlProcessor = sqlProcessor;
        }

        public bool defineFPRDBSchema(FPRDBSchemaDTO fprdbSchemaDTO)
        {
            if (fprdbSchemaDTO.fields.Count == 0)
                throw new InvalidOperationException($"Creation for FPRDB schema {fprdbSchemaDTO.schemaName} doesn't have any attribute");

            string fieldName;
            FieldType fieldType;
            foreach(Field f in fprdbSchemaDTO.fields)
            {
                fieldName = f.getFieldName();
                
                if (fieldName == null || fieldName == "")
                {
                    throw new InvalidOperationException($"Name for an attribute isn't provided");
                }
            }

            return this.fprdbSchemaDAO.defineFPRDBSchema(fprdbSchemaDTO);
        }
        public void removeFPRDBSchema(FPRDBSchemaDTO fprdbSchemaDTO)
        {
            this.fprdbSchemaDAO.removeFPRDBSchema(fprdbSchemaDTO);
        }
    }
}
