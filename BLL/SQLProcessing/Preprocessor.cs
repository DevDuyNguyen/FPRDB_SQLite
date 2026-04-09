using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.SQLProcessing
{
    public class Preprocessor
    {
        private MetadataManager metadataMgr;
        private ConstraintService constraintService;

        public Preprocessor(MetadataManager metadataMgr, ConstraintService constraintService)
        {
            this.metadataMgr = metadataMgr;
            this.constraintService = constraintService;
        }
        private bool checkIfSelectionConditionIsValid(SelectionCondition condition, List<Field> mentionedFields)
        {
            //Every attribute that is mentioned in the WHERE-clause must be an attribute of some relation in the current scope
            List<string> attributesInSelectionCondition = condition.getMentionedAttributes();
            foreach(string attrName in attributesInSelectionCondition)
            {
                if (mentionedFields.FirstOrDefault(el => el.getFieldName() == attrName) == null)
                {
                    throw new SemanticException($"Field {attrName} doesn't exist");
                }
            }

            //Check operator is applicable on attribute:
            AtomicSelectionExpressionFieldConstant fieldConstantEx;
            AtomicSelectionExpressionFieldField fieldfieldEx;
            Field tmpField1 = null;
            Field tmpField2 = null;
            List<SelectionExpression> atomicSelectionExpressions = condition.getAtomicSelectionExpressions();

            foreach (SelectionExpression ex in atomicSelectionExpressions)
            {

                //check on AtomicSelectionExpressionFieldConstant
                if (ex is AtomicSelectionExpressionFieldConstant)
                {
                    fieldConstantEx = ex as AtomicSelectionExpressionFieldConstant;
                    tmpField1 = mentionedFields.FirstOrDefault(f => f.getFieldName() == fieldConstantEx.field);
                    this.checkComparisonOperatorOnFieldConstant(tmpField1, fieldConstantEx.constant, fieldConstantEx.compareOperator, this.metadataMgr);
                }

                //check on AtomicSelectionExpressionFieldField
                if (ex is AtomicSelectionExpressionFieldField)
                {
                    fieldfieldEx = ex as AtomicSelectionExpressionFieldField;
                    tmpField1 = mentionedFields.FirstOrDefault(f => f.getFieldName() == fieldfieldEx.lField);
                    tmpField2 = mentionedFields.FirstOrDefault(f => f.getFieldName() == fieldfieldEx.rField);
                    this.checkCompatibleFieldEqualField(tmpField1, tmpField2);
                }
            }
            return true;
            
        }

        public bool checkSemanticCreateSchema(FPRDBSchema data)
        {
            string schemaName = data.getSchemaName();
            string constraintName = data.getPrimaryConstraintName();
            if (this.metadataMgr.isSchemaExist(schemaName))
            {
                throw new SemanticException($"Schema {schemaName} already exists");
            }
            if(data.getPrimaryConstraintName()=="" || data.getPrimaryConstraintName()==null 
                || data.getPrimaryConstraintName() == null || data.getPrimarykey().Count == 0)
            {
                throw new SemanticException($"Schema creation must have primary key");
            }
            if (this.metadataMgr.isConstraintExist(constraintName))
            {
                throw new SemanticException($"Constraint with name {constraintName} already exists");
            }
            foreach(string fldName in data.getPrimarykey())
            {
                if (!FieldTypeUtilities.isPrimitive(data.getFieldByName(fldName).getFieldInfo().getType()))
                    throw new SemanticException("Primary key must be of non fuzzy set type");
            }
            return true;

        }
        public bool checkSemanticCreateRelation(FPRDBRelation data)
        {
            //relation name must not existed before
            if (this.metadataMgr.isRelationExist(data.getRelName()))
            {
                throw new SemanticException($"Relation with name {data.getRelName()} already exists");
            }
            //schema must existed before
            if (!this.metadataMgr.isSchemaExist(data.getSchemaName()))
            {
                throw new SemanticException($"No schema with name {data.getSchemaName()} exists");
            }
            return true;
        }
        public bool checkCompatibleInsertTypeAndFillFuzzySetConstant(List<string> fields, List<FuzzyProbabilisticValueParsingData> data, FPRDBSchema schema)
        {
            /* For each field:
             * -loop through the constant values in its FuzzyProbabilisticValueParsingData:
             * +if field is not fuzzy set type, check if that constant is of the field type
             * +if the field is fuzzy set type, get the information of the fuzzy set, check if the fuzzy set type is compatible with the field type, fill the fs constant. Ex: integer discrete fuzzy set ~= int
             */

            for (int i = 0; i < fields.Count; ++i)
            {
                Field field = schema.getFieldByName(fields[i]);
                FieldInfo fieldInfo = field.getFieldInfo();
                FieldType fieldType = fieldInfo.getType();
                string generalExceptionMessage = $"Inserted data and field type of {field.getFieldName()} aren't compatible";
                

                foreach (Constant constant in data[i].valueList)
                {
                    if(constant is IntConstant)
                    {
                        if(fieldInfo.getType()!=FieldType.INT && fieldInfo.getType() != FieldType.distFS_INT && fieldInfo.getType() != FieldType.distFS_FLOAT && fieldInfo.getType() != FieldType.contFS)
                            throw new SemanticException(generalExceptionMessage);
                    }
                    else if (constant is FloatConstant)
                    {
                        if (fieldInfo.getType() != FieldType.FLOAT && fieldInfo.getType() != FieldType.distFS_FLOAT && fieldInfo.getType()!=FieldType.contFS)
                            throw new SemanticException(generalExceptionMessage);
                    }
                    else if (constant is StringConstant)
                    {
                        if (fieldInfo.getType() != FieldType.VARCHAR && fieldInfo.getType() != FieldType.CHAR && fieldInfo.getType() != FieldType.distFS_TEXT)
                            throw new SemanticException(generalExceptionMessage);
                    }
                    else if (constant is BooleanConstant)
                    {
                        if (fieldInfo.getType() != FieldType.BOOLEAN)
                            throw new SemanticException(generalExceptionMessage);
                    }
                    else if (constant is FuzzySetConstant)
                    {
                        FuzzySetConstant fsContant = (FuzzySetConstant)constant;
                        string fsName = (string)fsContant.getVal();
                        FieldType type;
                        int fuzzySetOID;
                        //check fuzzy set use in insert data really exist
                        try
                        {
                            type=this.metadataMgr.getFuzzySetType(fsName);
                            fuzzySetOID = this.metadataMgr.getFuzzySetOID(fsName);
                        }
                        catch (QueryDataNotExistException ex)
                        {
                            throw new SemanticException(ex.Message);
                        }
                        
                        fsContant.setFuzzySetOID(fuzzySetOID);
                        fsContant.setType(type);
                        if (fieldInfo.getType() == FieldType.distFS_FLOAT)
                        {
                            if(type!=FieldType.distFS_FLOAT && type != FieldType.distFS_INT)
                                throw new SemanticException(generalExceptionMessage);
                        }
                        else
                        {
                            if (fieldInfo.getType() != type)
                                throw new SemanticException(generalExceptionMessage);
                        }
                    }

                }
            }
            return true;
        }
        public bool checkSemanticInsert(InsertData data)
        {
            //Check if relation exist
            FPRDBRelation relation;
            try
            {
                relation = this.metadataMgr.getRelation(data.relation);
            }
            catch(QueryDataNotExistException ex)
            {
                throw new SemanticException($"Relation {data.relation} doesn't exist");
            }
            //Check insert attribute
            FPRDBSchema schema = relation.getSchema();
            foreach(string fieldName in data.fieldList)
            {
                bool exist = false;
                foreach(Field field in schema.getFields())
                {
                    if (fieldName == field.getFieldName())
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                    throw new SemanticException($"Attribute {fieldName} doesn't exist in relation {relation.getRelName()} on schema {schema.getSchemaName()}");
            }
            //Check compatible attribute list and insert data list sizes
            if (data.fieldList.Count != data.fuzzyProbabilisticValues.Count)
            {
                throw new SemanticException("Number of attributes must equal the number of inserted data");
            }
            //Check compatible insert value 
            checkCompatibleInsertTypeAndFillFuzzySetConstant(data.fieldList, data.fuzzyProbabilisticValues, schema);
            //check if [a,b] is within [0,1]
            foreach (FuzzyProbabilisticValueParsingData d in data.fuzzyProbabilisticValues)
            {
                for(int j=0;  j<d.intervalProbLowerBoundList.Count; ++j)
                {
                    if (d.intervalProbLowerBoundList[j] < 0 || d.intervalProbLowerBoundList[j] > 1
                        || d.intervalProbUpperBoundList[j] < 0 || d.intervalProbUpperBoundList[j] > 1)
                        throw new SemanticException("[a,b] must be within the range of [0,1]");
                }
                
            }
            //
            //The insert fuzzy probabilistic value for a key attribute must be primitive, it is the only possible value in the fuzzy probabilistic value and its interval probability is [1,1]
            //check identity constraint
            this.constraintService.checkIntegrityConstraint(relation, data);


            //not done: referential constraint

            return true;

        }
        public bool checkSemanticDelete(DeleteData data)
        {
            //[not done] check selection condition:
            if (!this.metadataMgr.isRelationExist(data.relation))
            {
                throw new SemanticException($"Relation {data.relation} doesn't exist");
            }

            FPRDBRelation relation = this.metadataMgr.getRelation(data.relation);
            /*
             * Selection condition:
             * -Mentioned field exist
             * -Field is compared with data type compatible constant or other field
             */
            if (data.selectionCondition != null)
            {

                this.checkIfSelectionConditionIsValid(data.selectionCondition, relation.getSchema().getFields());
            }

            return true;
        }
        public bool checkSemanticDropRelation(DropRelationData data)
        {
            if (!this.metadataMgr.isRelationExist(data.relation))
                throw new SemanticException($"Relation {data.relation} doesn't exist");
            return true;
        }
        public bool checkSemanticDropSchema(DropSchemaData data)
        {
            if (!this.metadataMgr.isSchemaExist(data.schema))
                throw new SemanticException($"Can't delete schema {data.schema}, because it doesn't exist");
            if (this.metadataMgr.isRelationOnSchemaExist(data.schema))
                throw new SemanticException($"Can't delete schema {data.schema}, because there are still relation defined on it exist");
            return true;
        }
        public bool checkSemanticModify(ModifyData data)
        {
            //relation exist
            FPRDBRelation relation=null;
            FPRDBSchema schema = null;
            List<Field> fieldsInSchema = null;
            try
            {
                relation = this.metadataMgr.getRelation(data.getRelation());
                schema = relation.getSchema();
                fieldsInSchema = schema.getFields();
            }
            catch (QueryDataNotExistException ex)
            {
                throw new SemanticException(ex.Message);
            }
            //Mentiond fields exists
            if (!relation.getSchema().hasField(data.getAssignedField())){
                throw new SemanticException($"Field {data.getAssignedField()} doesn't exist in relation {relation.getRelName()}");
            }
            if(data is FieldFieldModifyData)
            {
                FieldFieldModifyData data1 = (FieldFieldModifyData)data;
                if (!relation.getSchema().hasField(data1.getAssignValue() as string))
                {
                    throw new SemanticException($"Field {data1.getAssignValue() as string} doesn't exist in relation {relation.getRelName()}");
                }
            }
            if (data.getSelectionCondition() != null)
            {
                List<string> fieldsInSelectionCondition = data.getSelectionCondition().getMentionedAttributes();
                foreach (string fieldName in fieldsInSelectionCondition)
                {
                    if (!schema.hasField(fieldName))
                        throw new SemanticException($"Field {fieldName} doesn't exist in relation {relation.getRelName()}");
                }
            }

            //Compatible update value
            if(data is FieldFieldModifyData)
            {
                FieldFieldModifyData data1 = (FieldFieldModifyData)data;
                Field assignedField = relation.getSchema().getFieldByName(data1.getAssignedField());
                Field assigningField = relation.getSchema().getFieldByName(data1.getAssignValue() as string);
                if (assignedField.getFieldInfo().getType() != assigningField.getFieldInfo().getType() || assignedField.getFieldInfo().getTXTLength() != assigningField.getFieldInfo().getTXTLength())
                    throw new SemanticException($"Can't assign the content of {assigningField.getFieldInfo().getType().ToString()} field to {assignedField.getFieldInfo().getType()} field");
            }
            else if(data is FieldFuzzProbValueModifyData)
            {
                FieldFuzzProbValueModifyData data1 = (FieldFuzzProbValueModifyData)data;
                checkCompatibleInsertTypeAndFillFuzzySetConstant(
                    new List<string> { data1.getAssignedField()}, 
                    new List<FuzzyProbabilisticValueParsingData>{ data1.getAssignValue() as FuzzyProbabilisticValueParsingData},
                    relation.getSchema()
                    );
            }
            /*
             * Selection condition:
             * -Mentioned field exist
             * -Field is compared with data type compatible constant or other field
             */
            if (data.getSelectionCondition() != null)
            {
                this.checkIfSelectionConditionIsValid(data.getSelectionCondition(), fieldsInSchema);
            }

            return true;
        }
        //not done: mocking for private
        public bool checkComparisonOperatorOnFieldConstant(Field field1, Constant c, CompareOperation op, MetadataManager metaDataMgr)
        {
            //FieldType fieldType1 = field1.getFieldInfo().getType();
            //string errorMess = $"{field1.getFieldName()} {op.ToString()} {c.getVal() as string} is invalid";
            //if (FieldTypeUtilities.isPrimitive(fieldType1))
            //{
            //    if (CompareOperatorUltilities.isScalarComparison(op))
            //    {
            //        if (!(ConstantUltilities.isPrimitiveConstant(c)))
            //            throw new SemanticException(errorMess);
            //        if(fieldType1==FieldType.INT && !(c is IntConstant) && !(c is FloatConstant))
            //            throw new SemanticException(errorMess);
            //        else if (fieldType1 == FieldType.FLOAT && !(c is IntConstant) && !(c is FloatConstant))
            //            throw new SemanticException(errorMess);
            //        else if(FieldTypeUtilities.getDomainType(fieldType1)!=ConstantUltilities.getDomainType(c, metaDataMgr))
            //            throw new SemanticException(errorMess);
            //    }
            //    else if (op == CompareOperation.ALSO)
            //    {
            //        if(!(c is FuzzySetConstant))
            //            throw new SemanticException(errorMess);
            //        string fsName = (c as FuzzySetConstant).getVal() as string;
            //        FieldType fsType = metadataMgr.getFuzzySetType(fsName);
            //        if (fieldType1 == FieldType.INT && !(fsType!=FieldType.distFS_INT) && !(fsType != FieldType.distFS_FLOAT))
            //            throw new SemanticException(errorMess);
            //        else if (fieldType1 == FieldType.FLOAT && !(fsType != FieldType.distFS_INT) && !(fsType != FieldType.distFS_FLOAT))
            //            throw new SemanticException(errorMess);
            //        else if (FieldTypeUtilities.getDomainType(fieldType1) != ConstantUltilities.getDomainType(c, metaDataMgr))
            //            throw new SemanticException(errorMess);
            //    }
            //}
            //else
            //{

            //}
            FieldType fieldType1 = field1.getFieldInfo().getType();
            string errorMess = $"{field1.getFieldName()} {op.ToString()} {c.getVal().ToString()} is invalid";
            if (CompareOperatorUltilities.isScalarComparison(op) || op == CompareOperation.ALSO)
            {
                try
                {
                    if (FieldTypeUtilities.getDomainType(fieldType1) == typeof(int))
                    {
                        if (ConstantUltilities.getDomainType(c, metaDataMgr) != typeof(int)
                        && ConstantUltilities.getDomainType(c, metaDataMgr) != typeof(float))
                            throw new SemanticException(errorMess);
                        return true;
                    }
                    else if (FieldTypeUtilities.getDomainType(fieldType1) == typeof(float))
                    {
                        if (ConstantUltilities.getDomainType(c, metaDataMgr) != typeof(int)
                        && ConstantUltilities.getDomainType(c, metaDataMgr) != typeof(float))
                            throw new SemanticException(errorMess);
                        return true;
                    }
                    else if (FieldTypeUtilities.getDomainType(fieldType1) != ConstantUltilities.getDomainType(c, metaDataMgr))
                        throw new SemanticException(errorMess);
                }
                catch(QueryDataNotExistException ex)
                {
                    throw new SemanticException(ex.Message);
                }
            }
            else
            {
                throw new SemanticException($"Comparison operator {op.ToString()} isn't supported");
            }

            return true;
        }
        //not done: mocking for private
        public bool checkCompatibleFieldEqualField(Field f1, Field f2)
        {
            FieldType fieldType1 = f1.getFieldInfo().getType();
            FieldType fieldType2 = f2.getFieldInfo().getType();
            string errorMess = $"{f1.getFieldName()} = {f2.getFieldName()} is invalid";
            if (FieldTypeUtilities.getDomainType(fieldType1) == typeof(int))
            {
                if(FieldTypeUtilities.getDomainType(fieldType2) != typeof(int)
                    && FieldTypeUtilities.getDomainType(fieldType2) != typeof(float))
                    throw new SemanticException(errorMess);
                return true;
            }
            else if (FieldTypeUtilities.getDomainType(fieldType1) == typeof(float))
            {
                if(FieldTypeUtilities.getDomainType(fieldType2) != typeof(int)
                    && FieldTypeUtilities.getDomainType(fieldType2) != typeof(float))
                    throw new SemanticException(errorMess);
                return true;
            }
            else if (FieldTypeUtilities.getDomainType(fieldType1) != FieldTypeUtilities.getDomainType(fieldType2))
                throw new SemanticException(errorMess);
            return true;
        }
        //not done:mocking for private
        public bool checkCartesianProductCompatibility(List<FPRDBSchema> cartesianRelationSchemas, out FPRDBSchema res)
        {
            List<Field> fields = new List<Field>();
            Dictionary<string, bool> meetFields = new Dictionary<string, bool>();

            foreach(FPRDBSchema sch in cartesianRelationSchemas)
            {
                foreach(Field f in sch.getFields())
                {
                    if (meetFields.ContainsKey(f.getFieldName()))
                    {
                        throw new SemanticException($"Not cartesian product compatible because of common field {f.getFieldName()}");
                    }
                    else
                    {
                        fields.Add(f);
                        meetFields.Add(f.getFieldName(), true);
                    }
                }

            }
            res = new FPRDBSchema(null, fields, null);
            return true;

        }
        public bool checkNaturalJoinCompatibility(List<FPRDBSchema> naturalJoinRelationSchemas, out FPRDBSchema res)
        {
            List<Field> fields = new List<Field>();
            Dictionary<string, Field> meetFields = new Dictionary<string, Field>();

            foreach (FPRDBSchema sch in naturalJoinRelationSchemas)
            {
                foreach (Field f in sch.getFields())
                {
                    if (meetFields.ContainsKey(f.getFieldName()))
                    {
                        if (meetFields[f.getFieldName()].getFieldInfo().getType() != f.getFieldInfo().getType()
                            || meetFields[f.getFieldName()].getFieldInfo().getTXTLength()!= f.getFieldInfo().getTXTLength())
                            throw new SemanticException($"Common field {f.getFieldName()} doesn't have same value domain");
                    }
                    else
                    {
                        fields.Add(f);
                        meetFields.Add(f.getFieldName(), f);
                    }
                }

            }
            res = new FPRDBSchema(null, fields, null);
            return true;

        }
        public bool checkSetOperationCompatibility(FPRDBSchema sch1, FPRDBSchema sch2)
        {
            List<Field> fields1 = sch1.getFields();
            List<Field> fields2 = sch2.getFields();
            for (int i=0; i<fields1.Count; ++i)
            {
                if (fields1[i].getFieldName() != fields2[i].getFieldName()
                    || fields1[i].getFieldInfo().getType() != fields2[i].getFieldInfo().getType())
                    throw new SemanticException($"Set opeation is incompatible because field {fields1[i].getFieldName()} and field {fields2[i].getFieldName()}");
            }
            return true;
        }
        /*Every attribute that is mentioned in the SELECT- or WHERE-clause must be an attribute 
         * of some relation in the current scope. It also checks ambiguity, signaling an error 
         * if the attribute is in the scope of two or more relations with that attribute*/
        public bool checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression(List<FPRDBRelation> relations, List<string> attributesInSelect, List<string> attributesInAtomicExpression)
        {
            Dictionary<string, bool> attributeList = new Dictionary<string, bool>();
            foreach(string attrName in attributesInSelect)
            {
                if (!attributeList.ContainsKey(attrName))
                {
                    attributeList[attrName] = true;
                }
            }
            if (attributesInAtomicExpression == null)
                attributesInAtomicExpression = new List<string>();
            foreach (string attrName in attributesInAtomicExpression)
            {
                if (!attributeList.ContainsKey(attrName))
                {
                    attributeList[attrName] = true;
                    attributesInSelect.Add(attrName);
                }
            }

            int matchCount;
            foreach(string attrName in attributesInSelect)
            {
                matchCount = 0;
                if (attrName == "*")
                    continue;
                foreach (FPRDBRelation rel in relations)
                {
                    if (rel.getSchema().hasField(attrName))
                    {
                        ++matchCount;
                        if (matchCount == 2)
                        {
                            throw new SemanticException($"Ambiguity: field {attrName} appears in more than on relations");
                        }
                    }
                }
                if (matchCount == 0)
                    throw new SemanticException($"field {attrName} doesn't appears in any mentioned relation");
            }
            return true;

        }
        public bool checkSemanticQuery(QueryData data)
        {

            if(!(data is CompoundQueryData))
            {
                List<FPRDBRelation> relations = new List<FPRDBRelation>();
                List<SelectField> selectList=null;
                List<string> attributesInSelectionCondition = null;
                List<SelectionExpression> atomicSelectionExpressions=null;
                FPRDBSchema atomicQuerySchema;

                if (data is BaseCartesianProductQueryData)
                {
                    BaseCartesianProductQueryData data1 = (BaseCartesianProductQueryData)data;

                    selectList = new List<SelectField>();
                    foreach (SelectField f in data1.selectList)
                        selectList.Add(f);
                    if(data1.selectionCondition!=null)
                    {
                        atomicSelectionExpressions = data1.selectionCondition.getAtomicSelectionExpressions();
                        attributesInSelectionCondition = data1.selectionCondition.getMentionedAttributes();
                    }

                    //Every relation mentioned in a FROM-clause must be a relation in the current database.
                    foreach (string relName in data1.relationList)
                    {
                        if (!this.metadataMgr.isRelationExist(relName))
                            throw new SemanticException($"Relation {relName} doesn't exist");
                        relations.Add(this.metadataMgr.getRelation(relName));
                    }
                    //Check Cartesian Product compatibility 
                    checkCartesianProductCompatibility(relations.Select(rel => rel.getSchema()).ToList(), out atomicQuerySchema);

                    //create schema for BaseCartesianProductQueryData for later set opeartion semantic check
                    List<Field> tmpField;
                    if (data1.selectList.Count == 1 && data1.selectList[0].field == "*")
                        tmpField = atomicQuerySchema.getFields();
                    else
                    {
                        tmpField = new List<Field>();
                        foreach (Field f in atomicQuerySchema.getFields())
                        {
                            foreach (SelectField sf in data1.selectList)
                            {
                                if (sf.field == f.getFieldName())
                                {
                                    tmpField.Add(f);
                                    break;
                                }
                            }
                        }
                    }
                    
                    data1.schema = new FPRDBSchema(null, tmpField, null);

                    /*Checking: Every attribute that is mentioned in the SELECT- or WHERE-clause 
                     * must be an attribute of some relation in the current scope. It also checks ambiguity, 
                     * signaling an error if the attribute is in the scope of two or more relations with that attribute.
                     */
                    checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression(relations, selectList.Select(f => f.field).ToList(), attributesInSelectionCondition);
                }
                else if (data is BaseNaturalJoinQueryData)
                {
                    BaseNaturalJoinQueryData data1 = (BaseNaturalJoinQueryData)data;

                    selectList = new List<SelectField>();
                    foreach (SelectField f in data1.selectList)
                        selectList.Add(f);
                    if (data1.selectionCondition != null)
                    {
                        atomicSelectionExpressions = data1.selectionCondition.getAtomicSelectionExpressions();
                        attributesInSelectionCondition = data1.selectionCondition.getMentionedAttributes();
                    }

                    //Every relation mentioned in a FROM-clause must be a relation in the current database.
                    foreach (string relName in data1.naturalJoinList.relationList)
                    {
                        if (!this.metadataMgr.isRelationExist(relName))
                            throw new SemanticException($"Relation {relName} doesn't exist");
                        relations.Add(this.metadataMgr.getRelation(relName));
                    }
                    //Check natural join compatibility
                    this.checkNaturalJoinCompatibility(relations.Select(rel => rel.getSchema()).ToList(), out atomicQuerySchema);

                    //create schema for BaseCartesianProductQueryData for later set opeartion semantic check
                    List<Field> tmpField;
                    if (data1.selectList.Count == 1 && data1.selectList[0].field == "*")
                        tmpField = atomicQuerySchema.getFields();
                    else
                    {
                        tmpField = new List<Field>();
                        foreach (Field f in atomicQuerySchema.getFields())
                        {
                            foreach (SelectField sf in data1.selectList)
                            {
                                if (sf.field == f.getFieldName())
                                {
                                    tmpField.Add(f);
                                    break;
                                }
                            }
                        }
                    }

                    data1.schema = new FPRDBSchema(null, tmpField, null);
                }
                

                if (atomicSelectionExpressions!=null)
                {
                    //Check operator is applicable on attribute:
                    AtomicSelectionExpressionFieldConstant fieldConstantEx;
                    AtomicSelectionExpressionFieldField fieldfieldEx;
                    Field tmpField1 = null;
                    Field tmpField2 = null;

                    List<Field> allFieldsFromMentionedRelations = new List<Field>();
                    foreach (FPRDBRelation rel in relations)
                    {
                        allFieldsFromMentionedRelations.AddRange(rel.getSchema().getFields());
                    }
                    foreach (SelectionExpression ex in atomicSelectionExpressions)
                    {

                        //check on AtomicSelectionExpressionFieldConstant
                        if (ex is AtomicSelectionExpressionFieldConstant)
                        {
                            fieldConstantEx = ex as AtomicSelectionExpressionFieldConstant;
                            tmpField1 = allFieldsFromMentionedRelations.FirstOrDefault(f => f.getFieldName() == fieldConstantEx.field);
                            this.checkComparisonOperatorOnFieldConstant(tmpField1, fieldConstantEx.constant, fieldConstantEx.compareOperator, this.metadataMgr);
                        }

                        //check on AtomicSelectionExpressionFieldField
                        if (ex is AtomicSelectionExpressionFieldField)
                        {
                            fieldfieldEx = ex as AtomicSelectionExpressionFieldField;
                            tmpField1 = allFieldsFromMentionedRelations.FirstOrDefault(f => f.getFieldName() == fieldfieldEx.lField);
                            tmpField2 = allFieldsFromMentionedRelations.FirstOrDefault(f => f.getFieldName() == fieldfieldEx.rField);
                            this.checkCompatibleFieldEqualField(tmpField1, tmpField2);
                        }
                    }
                }

                return true;
            }
            else //if(data is CompoundQueryData)
            {
                //Check set operation compatibility
                CompoundQueryData data1 = data as CompoundQueryData;
                this.checkSemanticQuery(data1.leftQuery);
                this.checkSemanticQuery(data1.rightQuery);
                this.checkSetOperationCompatibility(data1.leftQuery.getSchema(), data1.rightQuery.getSchema());
                
                return true;
            }
        }

        private bool checkFieldsMentionedInSelectionConditionExist(FPRDBSchema schema, SelectionCondition condition)
        {
            List<string> fieldsInSelectCondition = condition.getMentionedAttributes();
            foreach(string fieldName in fieldsInSelectCondition)
            {
                if (!schema.hasField(fieldName))
                    throw new SemanticException($"Field {fieldName} doesn't exist");
            }
            return true;
        }



    }
}
