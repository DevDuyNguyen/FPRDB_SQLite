using BLL.DTO;
using BLL.Interfaces;
using System.IO;

namespace BLL.Services
{
    public class FPRDBRelationService
    {
        private FPRDBRelationDAO fprdbRelationDAO;

        public FPRDBRelationService(FPRDBRelationDAO fprdbRelationDAO)
        {
            this.fprdbRelationDAO = fprdbRelationDAO;
        }

        public bool createFPRDBRelation(FPRDBRelationDTO rel)
        {
            if (rel.relName == "" || rel.relName == null)
                throw new InvalidDataException("Relation name is empty");
            if (rel.schemaName == "" || rel.schemaName == null)
                throw new InvalidDataException("Schema name is empty");
            return this.fprdbRelationDAO.createFPRDBRelation(rel);
        }
        public void removeFPRDBRelation(FPRDBRelationDTO rel)
        {
            this.fprdbRelationDAO.removeFPRDBRelation(rel);
        }

    }
}
