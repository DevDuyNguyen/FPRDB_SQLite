using BLL.DomainObject;
using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL.Interfaces
{
    public interface FPRDBRelationDAO
    {
        public bool createFPRDBRelation(FPRDBRelationDTO rel);
        public void removeFPRDBRelation(FPRDBRelationDTO rel);
        public List<FPRDBRelation> findRelation(string name);

    }
}
