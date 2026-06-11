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
            if (rel == null)
                throw new InvalidOperationException("Parameter rel isn't provided");

            return this.fprdbRelationDAO.createFPRDBRelation(rel);
        }
        public void removeFPRDBRelation(FPRDBRelationDTO rel)
        {
            if (rel == null)
                throw new InvalidOperationException("Parameter rel isn't provided");

            this.fprdbRelationDAO.removeFPRDBRelation(rel);
        }

    }
}
