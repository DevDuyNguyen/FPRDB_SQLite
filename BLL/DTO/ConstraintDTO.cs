using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Enums;

namespace BLL.DTO
{
    public class ConstraintDTO
    {
        public int oid;
        public string conName;
        public ConstraintType conType;
        public FPRDBRelationDTO relation;
        public FPRDBRelationDTO referencedRelation;
        public List<string> attributes;
        public List<string> referencedAttributes;
        public FPRDBSchemaDTO fprdbSchema;
        //retrieve ConsrtaintDTO from the underlying database
        public ConstraintDTO(int oid, string conName, ConstraintType conType, FPRDBRelationDTO relation, FPRDBRelationDTO referencedRelation, List<string> attributes, List<string> referencedAttributes, FPRDBSchemaDTO fprdbSchema)
        {
            this.oid = oid;
            this.conName = conName;
            this.conType = conType;
            this.relation = relation;
            this.referencedRelation = referencedRelation;
            this.attributes = attributes;
            this.referencedAttributes = referencedAttributes;
            this.fprdbSchema = fprdbSchema;
        }
        //create in-memory constraintDTO
        public ConstraintDTO(string conName, ConstraintType conType, FPRDBRelationDTO relation, FPRDBRelationDTO referencedRelation, List<string> attributes, List<string> referencedAttributes, FPRDBSchemaDTO fprdbSchema)
        {
            this.conName = conName;
            this.conType = conType;
            this.relation = relation;
            this.referencedRelation = referencedRelation;
            this.attributes = attributes;
            this.referencedAttributes = referencedAttributes;
            this.fprdbSchema = fprdbSchema;
        }
    }
}
