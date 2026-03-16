using BLL.DomainObject;
using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL.DAO
{
    public interface FPRDBSchemaDAO
    {
        public bool defineFPRDBSchema(FPRDBSchemaDTO fprdbSchemaDTO);
        public bool removeFPRDBSchema(FPRDBSchemaDTO fprdbSchemaDTO);
        public List<FPRDBSchema> findSchema(string name);
        public List<FPRDBRelation> findRelationsOfSchema(FPRDBSchemaDTO schema);

    }
}
