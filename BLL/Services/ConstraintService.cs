using BLL.Common;
using BLL.DomainObject;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.SQLProcessing;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ConstraintService
    {
        private MetadataManager metadataMgr;
        private ConstraintDAO constraintDAO;

        public ConstraintService(MetadataManager metadataMgr, ConstraintDAO constraintDAO)
        {
            this.metadataMgr = metadataMgr;
            this.constraintDAO = constraintDAO;
        }

        public bool checkIntegrityConstraintInsert(FPRDBRelation rel, InsertData data)
        {
            FPRDBRelation relation;
            try
            {
                relation = this.metadataMgr.getRelation(data.relation);
            }
            catch (QueryDataNotExistException ex)
            {
                throw new SemanticException($"Relation {data.relation} doesn't exist");
            }
            FPRDBSchema schema = relation.getSchema();
            List<string> value = new List<string>();
            foreach (string keyAttr in schema.getPrimarykey())
            {
                if (!data.fieldList.Contains(keyAttr))
                    throw new SemanticException($"Must provide value for key attribute {keyAttr}");
                int index = data.fieldList.IndexOf(keyAttr);
                if (data.fuzzyProbabilisticValues[index].valueList.Count != 1
                    || !ConstantUltilities.isPrimitiveConstant(data.fuzzyProbabilisticValues[index].valueList[0]))
                    throw new SemanticException($"Invalid insert value for key attribute {keyAttr}");
                if (data.fuzzyProbabilisticValues[index].intervalProbLowerBoundList[0] != 1
                    || data.fuzzyProbabilisticValues[index].intervalProbUpperBoundList[0] != 1)
                    throw new SemanticException($"key attribute {keyAttr} must have interval probability of [1,1]");
                if(data.fuzzyProbabilisticValues[index].valueList[0] is StringConstant)
                    value.Add("\""+data.fuzzyProbabilisticValues[index].valueList[0].getVal().ToString()+"\"");
                else
                    value.Add(data.fuzzyProbabilisticValues[index].valueList[0].getVal().ToString());
            }
            if (this.metadataMgr.isTupleExist(schema.getPrimarykey(), value, data.relation))
                throw new SemanticException("IDENTITY constraint violation");
            return true;
        }
        
        public bool checkUpdateIntegrityConstraintViolation(FPRDBRelation relation, List<string> primarykeyOrder, List<AbstractFuzzyProbabilisticValue> oldKeyValue, List<AbstractFuzzyProbabilisticValue> newKeyValue)
        {
            bool sameKeyValue = true;
            if (oldKeyValue.Count != newKeyValue.Count)
                throw new SemanticException("Old key value has different length in comparison to new key value");
            for(int i=0; i<oldKeyValue.Count; ++i)
            {

                if (!oldKeyValue[i].equals(newKeyValue[i]))
                {
                    sameKeyValue = false;
                    break;
                }
            }
            if (sameKeyValue)
                return true;
            else
            {
                if (this.constraintDAO.isTupleWithFuzzyProbabilisticValuesExist(relation.getRelName(), relation.getSchema().getPrimarykey(), newKeyValue))
                    throw new SemanticException($"Key value already exist");
                else
                    return true;
            }
            

        }
        public ConstraintDTO createReferentialConstraint(string conName, FPRDBRelationDTO fprdbRelation, FPRDBRelationDTO referencedFPRDBRelation, List<string> attributes, List<string>  referencedAttributes)
        {
            return this.constraintDAO.createReferentialConstraint(conName, fprdbRelation, referencedFPRDBRelation, attributes, referencedAttributes);
        }
        public List<ConstraintDTO> getReferenrialConstraints(FPRDBRelationDTO fprdbRelationDTO)
        {
            return this.constraintDAO.getReferenrialConstraints(fprdbRelationDTO);
        }
        public void removeConstraint(int oid)
        {
            this.constraintDAO.removeConstraint(oid);
        }
        public bool checkIfInsertTupleViolateReferentialConstraint(InsertData data)
        {
            return this.constraintDAO.checkIfInsertTupleViolateReferentialConstraint(data);
        }
        public bool checkIfDropRelationViolateReferentialConstraint(DropRelationData data)
        {
            return this.constraintDAO.checkIfDropRelationViolateReferentialConstraint(data);
        }
        public bool checkIfDeleteTupleViolateReferentialConstraint(DeleteData data)
        {
            return this.constraintDAO.checkIfDeleteTupleViolateReferentialConstraint(data);
        }
        public bool checkIfUpdatingTupleViolateReferentialConstraint(ModifyData data)
        {
            return this.constraintDAO.checkIfUpdatingTupleViolateReferentialConstraint(data);
        }

        //not done: mocking for private
        //not done: not implemented
        //public bool checkIfRelationSatisfyReferentialConstraint(ConstraintDTO constr)
        //{
        //    FPRDBRelationDTO referencingRel = constr.relation;
        //    FPRDBRelationDTO referencedRel = constr.referencedRelation;
        //    List<string> foreignKey = constr.attributes;
        //    List<string> primaryKey = constr.referencedAttributes;
        //    using(Idatarea)

        //}
    }
}
