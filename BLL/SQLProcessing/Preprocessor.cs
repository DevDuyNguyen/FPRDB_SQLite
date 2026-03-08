using BLL.DomainObject;
using BLL.Exceptions;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.SQLProcessing
{
    public class Preprocessor
    {
        private MetadataManager metadataMgr;

        public Preprocessor(MetadataManager metadataMgr)
        {
            this.metadataMgr = metadataMgr;
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
             * -loop through the constant values in valueList of its FuzzyProbabilisticValueParsingData:
             * +if field is not fuzzy set type, check if that constant is of the field type
             * +if the field is fuzzy set type, get the information of the fuzzy set, check if the fuzzy set type is compatible with the field type, fill the fs constant. Ex: integer discrete fuzzy set ~= int
             */

            for (int i = 0; i < fields.Count; ++i)
            {
                Field field = schema.getFieldByName(fields[i]);
                FieldInfo fieldInfo = field.getFieldInfo();
                string generalExceptionMessage = $"Inserted data and field type of {field.getFieldName()} aren't compatible";
                foreach (Constant constant in data[i].valueList)
                {
                    if (fieldInfo.getType() == FieldType.INT)
                    {
                        if (!(constant is IntConstant))
                            throw new SemanticException(generalExceptionMessage);
                    }
                    else if (fieldInfo.getType() == FieldType.FLOAT)
                    {
                        if (!(constant is FloatConstant))
                            throw new SemanticException(generalExceptionMessage);
                    }
                    else if (fieldInfo.getType() == FieldType.CHAR)
                    {
                        if (!(constant is StringConstant) || ((string)constant.getVal()).Length != 1)
                            throw new SemanticException(generalExceptionMessage);
                    }
                    else if (fieldInfo.getType() == FieldType.VARCHAR)
                    {
                        if (!(constant is StringConstant) || ((string)constant.getVal()).Length > fieldInfo.getTXTLength())
                            throw new SemanticException(generalExceptionMessage);
                    }
                    else if (fieldInfo.getType() == FieldType.BOOLEAN)
                    {
                        if (!(constant is BooleanConstant))
                            throw new SemanticException(generalExceptionMessage);
                    }
                    else if (fieldInfo.getType() == FieldType.distFS_INT
                        || fieldInfo.getType() == FieldType.distFS_FLOAT
                        || fieldInfo.getType() == FieldType.distFS_TEXT
                        || fieldInfo.getType() == FieldType.contFS) {

                        if(!(constant is FuzzySetConstant))
                        {
                            throw new SemanticException(generalExceptionMessage);
                        }

                        FuzzySetConstant fsContant = (FuzzySetConstant)constant;
                        string fsName = (string)fsContant.getVal();
                        FieldType type = this.metadataMgr.getFuzzySetType(fsName);
                        int fuzzySetOID = this.metadataMgr.getFuzzySetOID(fsName);
                        fsContant.setFuzzySetOID(fuzzySetOID);
                        fsContant.setType(type);

                        if(fieldInfo.getType()!=type)
                            throw new SemanticException(generalExceptionMessage);
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
                    throw new SemanticException($"Attribute {fieldName} doesn't exist in relation {relation.getRelName} on schema {schema.getSchemaName()}");
            }
            //Check compatible attribute list and insert data list sizes
            if (data.fieldList.Count != data.fuzzyProbabilisticValues.Count)
            {
                throw new SemanticException("Number of attributes must equal the number of inserted data");
            }
            //Check compatible insert type 
            try
            {
                checkCompatibleInsertTypeAndFillFuzzySetConstant(data.fieldList, data.fuzzyProbabilisticValues, schema);
            }
            catch(SemanticException ex)
            {
                throw ex;
            }
            //check if [a,b] is within [0,1]
            foreach (FuzzyProbabilisticValueParsingData d in data.fuzzyProbabilisticValues)
            {
                for(int j=0;  j<d.intervalProbLowerBoundList.Count; ++j)
                {
                    if (d.intervalProbLowerBoundList[j] < 0 || d.intervalProbUpperBoundList[j] > 1)
                        throw new SemanticException("[a,b] must be within the range of [0,1]");
                }
                
            }
            //check identity constraint
            List<string> value = new List<string>();
            foreach (string keyAttr in schema.getPrimarykey())
            {
                if (!data.fieldList.Contains(keyAttr))
                    throw new SemanticException($"Must provide value for key attribute {keyAttr}");
                int index = data.fieldList.IndexOf(keyAttr);
                if (data.fuzzyProbabilisticValues[index].valueList.Count != 1 
                    || !isPrimitiveConstant(data.fuzzyProbabilisticValues[index].valueList[0]))
                    throw new SemanticException($"Invalid insert value for key attribute {keyAttr}");
                value.Add(data.fuzzyProbabilisticValues[index].valueList[0].getVal().ToString());
            }
            if (this.metadataMgr.isTupleExist(schema.getPrimarykey(), value, data.relation))
                throw new SemanticException("IDENTITY constraint violation");

            //not done: referential constraint

            return true;

        }
        private bool isPrimitiveConstant(Constant c)
        {
            return c is IntConstant || c is FloatConstant || c is StringConstant || c is BooleanConstant;
        }


    }
}
