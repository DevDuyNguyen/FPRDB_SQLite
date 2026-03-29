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
        public bool checkIfDropRelationViolateReferentialConstraint(DropRelationData data, ConstraintService)
        {
            FPRDBRelationDTO referencedRelationDTO = this.metaDataMgr.getRelation(data.relation).toDTO();
            FPRDBRelationDTO referencingRelationDTO;
            List<ConstraintDTO> referentialConstraints = this.getReferenrialConstraintsTo(referencedRelationDTO);
            string sql;
            AbstractFuzzyProbabilisticValue fprobValue;
            Field tmpField;
            FieldType tmpFieldType;
            List<string> strPrimaryKeyValue = new List<string>(referencedRelationDTO.fprdbSchema.primarykey.Count);

            RelationPlan plan = new RelationPlan(data.relation, this.metaDataMgr, this.databaseMgr, this.parser);
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
                        if (!r.Read())
                        {
                            //return false;
                            throw new InvalidOperationException($"Delete a tuple in {data.relation} will violate referential constraint {constr.conName}");
                        }
                    }
                }
            }
            return true;
        }
            
            
    }
}
