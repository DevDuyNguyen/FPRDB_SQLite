using BLL.DomainObject;
using BLL.DTO;
using BLL.Enums;
using BLL.Interfaces;
using BLL.Services;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace BLL.DAO
{
    public class ConstraintDAOSQLite:ConstraintDAO
    {
        private DatabaseManager databaseMgr;
        private MetadataManager metaDataMgr;
        private RecursiveDescentParser parser;
        public ConstraintDAOSQLite(DatabaseManager databaseMgr, MetadataManager metaDataMgr, RecursiveDescentParser parser)
        {
            this.databaseMgr = databaseMgr;
            this.metaDataMgr = metaDataMgr;
            this.parser = parser;
        }

        public bool isTupleWithFuzzyProbabilisticValuesExist(string relation, List<string> attributeList, List<AbstractFuzzyProbabilisticValue> contentList)
        {
            string sql = $"SELECT 1 FROM {relation} WHERE";
            for (int i = 0; i < attributeList.Count; ++i)
            {
                sql += $" {attributeList[i]}='{contentList[i].ToString()}' AND";
            }
            int trailingANDIndex = sql.LastIndexOf("AND");
            sql = sql.Substring(0, trailingANDIndex);
            using (IDataReader r = this.databaseMgr.executeQuery(sql))
            {
                return r.Read();
            }

        }
        public List<ConstraintDTO> getReferenrialConstraints(FPRDBRelationDTO fprdbRelationDTO)
        {
            if (fprdbRelationDTO.oid == -1)
                throw new InvalidOperationException("oid is not provided");

            string sql =$"SELECT * FROM fprdb_Constraint WHERE con_type='REFERENTIAL' AND con_relation_id={fprdbRelationDTO.oid}";
            int referenced_rel_oid;
            FPRDBRelationDTO referenced_rel=null;

            using (IDataReader r = this.databaseMgr.executeQuery(sql))
            {
                List<ConstraintDTO> ans = new List<ConstraintDTO>();
                while(r.Read())
                {
                    referenced_rel_oid = Convert.ToInt32(r["con_referenced_relation_id"]);
                    referenced_rel = (this.metaDataMgr.getRelationByID(referenced_rel_oid)).toDTO();
                    ans.Add(new ConstraintDTO(
                            Convert.ToInt32(r["oid"]),
                            (string)r["con_name"],
                            ConstraintType.REFERENTIAL,
                            fprdbRelationDTO,
                            referenced_rel,
                            (r["con_attributes"] as string).Split(',').ToList(),
                            (r["con_referenced_attributes"] as string).Split(',').ToList(),
                            null
                            )
                        );
                }

                return ans;
            }
        }

        public ConstraintDTO createReferentialConstraint(string conName, FPRDBRelationDTO fprdbRelation, FPRDBRelationDTO referencedFPRDBRelation, List<string> attributes, List<string> referencedAttributes)
        {
            //check if constraint name existed
            if (this.metaDataMgr.isConstraintNameExist(conName))
                throw new InvalidOperationException($"Constraint name {conName} already exists");
            //check if relations has oids
            if (fprdbRelation.oid == -1 || referencedFPRDBRelation.oid == -1)
                throw new InvalidOperationException("oid of involed relations aren't provided");
            //check if referencedAttributes are primary key in fprdbRelation
            bool isKeyAttribute;
            foreach(string attrName in referencedAttributes)
            {
                isKeyAttribute = false;
                foreach(Field f in referencedFPRDBRelation.fprdbSchema.fields)
                {
                    if (attrName == f.getFieldName())
                    {
                        isKeyAttribute = true;
                        break;
                    }
                }
                if (!isKeyAttribute)
                    throw new InvalidOperationException($"Field {attrName} isn't a key attribute in relation {referencedFPRDBRelation.relName}");
            }
            //check attributes mapping to referencedAttributes
            if (attributes.Count != referencedAttributes.Count || attributes.Count == 0)
                throw new InvalidOperationException($"Invalid mapping from foreigh key of {fprdbRelation.relName} to primary key of {referencedFPRDBRelation.relName}");
            List<Field> referencingRelationFields = fprdbRelation.fprdbSchema.fields;
            List<Field> referencedRelationFields = referencedFPRDBRelation.fprdbSchema.fields;
            Field f1, f2;
            for(int i=0; i<attributes.Count; ++i)
            {
                f1 = referencingRelationFields.FirstOrDefault(el => el.getFieldName() == attributes[i]);
                f2 = referencedRelationFields.FirstOrDefault(el => el.getFieldName() == referencedAttributes[i]);
                if (f1.getFieldInfo().getType() != f2.getFieldInfo().getType() || f1.getFieldInfo().getTXTLength() != f2.getFieldInfo().getTXTLength())
                    throw new InvalidOperationException($"Field {f1.getFieldName()} can't map to field {f2.getFieldName()}");
            }
            //create referential constraint
            string constraintSQL = @$"
                INSERT INTO fprdb_Constraint (con_name, con_type, con_relation_id, con_referenced_relation_id, con_attributes, con_referenced_attributes)
                VALUES  ('{conName}','REFERENTIAL',{fprdbRelation.oid},{referencedFPRDBRelation.oid},'{string.Join(',', attributes)}','{string.Join(',', referencedAttributes)}')
            ";
            this.databaseMgr.executeNonQuery(constraintSQL);
            return this.metaDataMgr.getConstraintByName(conName);
        }
        public void removeConstraint(int oid)
        {
            if (oid == default || oid == -1)
                throw new InvalidOperationException("oid of referential constraint to be deleted isn't provided");
            this.databaseMgr.executeNonQuery($"DELETE FROM fprdb_Constraint WHERE oid={oid}");
        }
        public bool checkIfInsertTupleViolateReferentialConstraint(InsertData data)
        {
            FPRDBRelation referencingRel = this.metaDataMgr.getRelation(data.relation);
            FPRDBRelationDTO referencedRelDTO;
            List<ConstraintDTO> referentialConstraints = this.getReferenrialConstraints(referencingRel.toDTO());
            string sql;
            FuzzyProbabilisticValueParsingData insertValue;
            bool skipReferentialConstraint;
            foreach (ConstraintDTO constr in referentialConstraints)
            {
                skipReferentialConstraint = false;
                referencedRelDTO = constr.referencedRelation;
                sql = $"SELECT 1 FROM {referencedRelDTO.relName} WHERE";
                for (int i = 0; i < constr.attributes.Count; ++i)
                {
                    insertValue = data.getInsertDataByFieldName(constr.attributes[i]);
                    //If any column in the foreign key is NULL, the constraint is simply not checked
                    if (insertValue == null)
                    {
                        skipReferentialConstraint = true;
                        break;
                    }
                    //foreign key point to primary key, hence foreign key attribute value must be exact and precise
                    else if (insertValue.valueList.Count>1 || insertValue.valueList[0] is FuzzySetConstant || insertValue.intervalProbLowerBoundList[0]!=1 || insertValue.intervalProbLowerBoundList[0]!=1)
                    {
                        throw new InvalidOperationException($"Value for foreign key attribute {constr.attributes[i]} isn't exact or precise");
                    }
                    else
                    {
                        sql += $" {constr.referencedAttributes[i]}='{insertValue.ToTextRepresentation()}' AND";
                    }
                }
                if (skipReferentialConstraint)
                    continue;
                int trailingAND = sql.LastIndexOf("AND");
                sql = sql.Substring(0, trailingAND);
                using (IDataReader r = this.databaseMgr.executeQuery(sql))
                {
                    if (!r.Read())
                    {
                        //return false;
                        throw new InvalidOperationException($"A tuple in {data.relation} violate referential constraint {constr.conName}");
                    }
                }
            }
            return true;
        }
        public List<ConstraintDTO> getReferenrialConstraintsTo(FPRDBRelationDTO fprdbRelationDTO)
        {
            if (fprdbRelationDTO.oid == -1)
                throw new InvalidOperationException("oid is not provided");

            string sql = $"SELECT * FROM fprdb_Constraint WHERE con_type='REFERENTIAL' AND con_referenced_relation_id={fprdbRelationDTO.oid}";
            int referencing_rel_oid;
            FPRDBRelationDTO referencing_rel = null;

            using (IDataReader r = this.databaseMgr.executeQuery(sql))
            {
                List<ConstraintDTO> ans = new List<ConstraintDTO>();
                while (r.Read())
                {
                    referencing_rel_oid = Convert.ToInt32(r["con_relation_id"]);
                    referencing_rel = (this.metaDataMgr.getRelationByID(referencing_rel_oid)).toDTO();
                    ans.Add(new ConstraintDTO(
                            Convert.ToInt32(r["oid"]),
                            (string)r["con_name"],
                            ConstraintType.REFERENTIAL,
                            referencing_rel,
                            fprdbRelationDTO,
                            (r["con_attributes"] as string).Split(',').ToList(),
                            (r["con_referenced_attributes"] as string).Split(',').ToList(),
                            null
                            )
                        );
                }

                return ans;
            }
        }
        public bool checkIfDropRelationViolateReferentialConstraint(DropRelationData data)
        {
            FPRDBRelationDTO referencedRelationDTO = this.metaDataMgr.getRelation(data.relation).toDTO();
            List<ConstraintDTO> referentialConstraints = this.getReferenrialConstraintsTo(referencedRelationDTO);
            if (referentialConstraints.Count != 0)
            {
                string errorMessage = $"Relation {data.relation} is referenced by ";
                foreach (ConstraintDTO contr in referentialConstraints)
                    errorMessage += $"{contr.relation.relName},";
                errorMessage = errorMessage.TrimEnd(',');
                throw new InvalidOperationException(errorMessage);
            }
            
            return true;
        }

        public bool checkIfDeleteTupleViolateReferentialConstraint(DeleteData data)
        {
            FPRDBRelationDTO referencedRelationDTO = this.metaDataMgr.getRelation(data.relation).toDTO();
            FPRDBRelationDTO referencingRelationDTO;
            List<ConstraintDTO> referentialConstraints = this.getReferenrialConstraintsTo(referencedRelationDTO);
            string sql;
            AbstractFuzzyProbabilisticValue fprobValue;
            Field tmpField;
            FieldType tmpFieldType;
            //List<string> strPrimaryKeyValue = new List<string>(referencedRelationDTO.fprdbSchema.primarykey.Count);

            Plan plan = new RelationPlan(data.relation, this.metaDataMgr, this.databaseMgr, this.parser);
            if (data.selectionCondition != null)
                plan = new SelectPlan(plan, data.selectionCondition);
            Scan s = plan.open();
            while (s.next())
            {
                foreach (ConstraintDTO constr in referentialConstraints)
                {
                    referencingRelationDTO = constr.relation;
                    sql = $"SELECT 1 FROM {referencingRelationDTO.relName} WHERE";
                    for (int i = 0; i < constr.referencedAttributes.Count; ++i)
                    {
                        tmpField = referencedRelationDTO.getSchemaFieldByName(constr.referencedAttributes[i]);
                        tmpFieldType = tmpField.getFieldInfo().getType();
                        if (tmpFieldType == FieldType.INT)
                            fprobValue = s.getFieldContent<int>(tmpField.getFieldName());
                        else if (tmpFieldType == FieldType.FLOAT)
                            fprobValue = s.getFieldContent<float>(tmpField.getFieldName());
                        else //if (tmpFieldType == FieldType.CHAR || tmpFieldType == FieldType.VARCHAR)
                            fprobValue = s.getFieldContent<string>(tmpField.getFieldName());
                        sql += $" {constr.attributes[i]}='{fprobValue.ToString()}' AND";
                    }
                    int trailingAND = sql.LastIndexOf("AND");
                    sql = sql.Substring(0, trailingAND);
                    using (IDataReader r = this.databaseMgr.executeQuery(sql))
                    {
                        if (r.Read())
                        {
                            //return false;
                            throw new InvalidOperationException($"Delete a tuple in {data.relation} will violate referential constraint {constr.conName}");
                        }
                    }
                }
            }
            return true;
        }
        public bool checkIfUpdatingTupleViolateReferentialConstraint(ModifyData data)
        {
            //check if update invole key attribute
            FPRDBRelation rel = this.metaDataMgr.getRelation(data.getRelation());
            FPRDBSchema schema = rel.getSchema();
            List<Field> fieldsInFPRDBSchema = rel.getSchema().getFields();
            List<string> primaryKey = rel.getSchema().primarykey;
            Field involvingKeyAttribute=null;
            bool isInvolvingKeyAttribute = false;
            string updatedField = data.getAssignedField();
            foreach(string attrName in primaryKey)
            {
                if (attrName == updatedField)
                {
                    involvingKeyAttribute= schema.getFieldByName(attrName);
                    isInvolvingKeyAttribute = true;
                    break;
                }
            }

            /*
             * Loop through each tuple to be updated:
             * -If updation involves key attribute: check if there is any tuple referencing to the current tuple
             * -For each referential constraint of the current relation:
             * +Check if updation involve the constraint's foreign key
             * +If yes, check if the updated foreign key value points to existing tuple
             */
            Plan plan = new RelationPlan(data.getRelation(), this.metaDataMgr, this.databaseMgr, this.parser);
            if (data.getSelectionCondition() != null)
                plan = new SelectPlan(plan, data.getSelectionCondition());
            Scan s = plan.open();
            List<ConstraintDTO> referentialConstraintTo = this.getReferenrialConstraintsTo(rel.toDTO());
            List<ConstraintDTO> referentialConstraintOn = this.getReferenrialConstraints(rel.toDTO());
            string sql;
            AbstractFuzzyProbabilisticValue fprobValue;
            Dictionary<string, (string, string)> referencedFieldToReferencingFieldAndFProbValues;
            Field tmpField;
            FieldType tmpFieldType;
            FPRDBRelationDTO referencingRelationDTO;
            int tmpIndex;

            while (s.next())
            {
                //If updation involves key attribute: check if there is any tuple referencing to the current tuple
                if (isInvolvingKeyAttribute)
                {
                    foreach(ConstraintDTO contr in referentialConstraintTo)
                    {
                        referencingRelationDTO = contr.relation;
                        referencedFieldToReferencingFieldAndFProbValues = new Dictionary<string, (string, string)>();

                        for(int i=0; i<contr.referencedAttributes.Count; ++i)
                        {
                            tmpField = schema.getFieldByName(contr.referencedAttributes[i]);
                            tmpFieldType = tmpField.getFieldInfo().getType();
                            if (tmpFieldType == FieldType.INT)
                                fprobValue = s.getFieldContent<int>(tmpField.getFieldName());
                            else if (tmpFieldType == FieldType.FLOAT)
                                fprobValue = s.getFieldContent<float>(tmpField.getFieldName());
                            else //if (tmpFieldType == FieldType.CHAR || tmpFieldType == FieldType.VARCHAR)
                                fprobValue = s.getFieldContent<string>(tmpField.getFieldName());
                            referencedFieldToReferencingFieldAndFProbValues.Add(contr.referencedAttributes[i], (contr.attributes[i], fprobValue.ToString()));
                        }

                        //if(data is FieldFieldModifyData)
                        //{
                        //    tmpField = schema.getFieldByName(data.getAssignValue() as string);
                        //    tmpFieldType = tmpField.getFieldInfo().getType();
                        //    if (tmpFieldType == FieldType.INT)
                        //        fprobValue = s.getFieldContent<int>(tmpField.getFieldName());
                        //    else if (tmpFieldType == FieldType.FLOAT)
                        //        fprobValue = s.getFieldContent<float>(tmpField.getFieldName());
                        //    else //if (tmpFieldType == FieldType.CHAR || tmpFieldType == FieldType.VARCHAR)
                        //        fprobValue = s.getFieldContent<string>(tmpField.getFieldName());

                        //    string tmpReferencingField = referencedFieldToReferencingFieldAndFProbValues[involvingKeyAttribute.getFieldName()].Item1;
                        //    referencedFieldToReferencingFieldAndFProbValues[involvingKeyAttribute.getFieldName()] = (tmpReferencingField, fprobValue.ToString());
                        //}
                        //else
                        //{
                        //    string tmpReferencingField = referencedFieldToReferencingFieldAndFProbValues[involvingKeyAttribute.getFieldName()].Item1;
                        //    referencedFieldToReferencingFieldAndFProbValues[involvingKeyAttribute.getFieldName()] = (tmpReferencingField, (data.getAssignValue() as FuzzyProbabilisticValueParsingData).ToTextRepresentation());
                        //}

                        sql = $"SELECT 1 FROM {contr.relation.relName} WHERE";
                        foreach(var keyValue in referencedFieldToReferencingFieldAndFProbValues)
                        {
                            sql += $" {keyValue.Value.Item1}='{keyValue.Value.Item2}' AND";
                        }
                        tmpIndex = sql.LastIndexOf("AND");
                        sql = sql.Substring(0, tmpIndex);
                        using(IDataReader r = this.databaseMgr.executeQuery(sql))
                        {
                            if (r.Read())
                                throw new InvalidOperationException($"Can't update key attribute of relation {data.getRelation()}, because a tuple in relation {referencingRelationDTO.relName} is referencing to the updated tuple");
                        }

                    }
                }

                /*-For each referential constraint of the current relation:
                *+Check if updation involve the constraint's foreign key
                *+If yes, check if the updated foreign key value points to existing tuple
                */
                foreach (ConstraintDTO constr in referentialConstraintOn)
                {
                    if (constr.attributes.Contains(data.getAssignedField()))
                    {
                        sql = $"SELECT 1 FROM {constr.referencedRelation.relName} WHERE";
                        for(int i=0; i<constr.attributes.Count; ++i)
                        {
                            tmpField = constr.relation.getSchemaFieldByName(constr.attributes[i]);
                            if (tmpField.getFieldName() == data.getAssignedField())
                            {
                                if (data is FieldFieldModifyData)
                                {
                                    tmpField = schema.getFieldByName(data.getAssignValue() as string);
                                    tmpFieldType = tmpField.getFieldInfo().getType();
                                    if (tmpFieldType == FieldType.INT)
                                        fprobValue = s.getFieldContent<int>(tmpField.getFieldName());
                                    else if (tmpFieldType == FieldType.FLOAT)
                                        fprobValue = s.getFieldContent<float>(tmpField.getFieldName());
                                    else //if (tmpFieldType == FieldType.CHAR || tmpFieldType == FieldType.VARCHAR)
                                        fprobValue = s.getFieldContent<string>(tmpField.getFieldName());
                                    sql += $" {constr.referencedAttributes[i]}='{fprobValue.ToString()}' AND";

                                }
                                else
                                {
                                    sql += $" {constr.referencedAttributes[i]}='{(data.getAssignValue() as FuzzyProbabilisticValueParsingData).ToTextRepresentation()}' AND";
                                }
                            }
                            else
                            {
                                tmpFieldType = tmpField.getFieldInfo().getType();
                                if (tmpFieldType == FieldType.INT)
                                    fprobValue = s.getFieldContent<int>(tmpField.getFieldName());
                                else if (tmpFieldType == FieldType.FLOAT)
                                    fprobValue = s.getFieldContent<float>(tmpField.getFieldName());
                                else //if (tmpFieldType == FieldType.CHAR || tmpFieldType == FieldType.VARCHAR)
                                    fprobValue = s.getFieldContent<string>(tmpField.getFieldName());
                                sql += $" {constr.referencedAttributes[i]}='{fprobValue.ToString()}' AND";
                            }
                            
                        }
                        tmpIndex = sql.LastIndexOf("AND");
                        sql = sql.Substring(0, tmpIndex);
                        using(IDataReader r = this.databaseMgr.executeQuery(sql))
                        {
                            if (!r.Read())
                                throw new InvalidOperationException($"Can't update foreign key attribute in relaiton {data.getRelation()}, because the updated tuple references to no tuple in relation {constr.referencedRelation.relName}");
                        }
                    }
                }


            }
            return true;

        }

    }
}
