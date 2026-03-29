using BLL.DomainObject;
using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ConstraintDAO
    {
        public bool isTupleWithFuzzyProbabilisticValuesExist(string relation, List<string> attributeList, List<AbstractFuzzyProbabilisticValue> contentList);
        public List<ConstraintDTO> getReferenrialConstraints(FPRDBRelationDTO fprdbRelationDTO);
        public ConstraintDTO createReferentialConstraint(string conName, FPRDBRelationDTO fprdbRelation, FPRDBRelationDTO referencedFPRDBRelation, List<string> attributes, List<string> referencedAttributes);
        public void removeConstraint(int oid);
    }
}
