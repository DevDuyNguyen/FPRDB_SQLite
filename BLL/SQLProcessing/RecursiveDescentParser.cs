using BLL.DomainObject;
using BLL.Enums;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Common;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    //not done: not unit test, cause no mocking yet
    public class RecursiveDescentParser
    {
        private Lexer lexer;
        private MetadataManager metaDataManager;

        public RecursiveDescentParser(Lexer lexer, MetadataManager metaDataManager)
        {
            this.lexer = lexer;
            this.metaDataManager = metaDataManager;
        }

        public void parse(String sqlStatement)
        {
            this.lexer.analyze(sqlStatement);
        }

        private SQLSyntaxException createSQLSyntaxException(string message)
        {
            try
            {
                Token nearToken;
                nearToken = this.lexer.getCurrentToken();
                return new SQLSyntaxException(nearToken.Value.ToString(),
                    nearToken.Location.Line,
                    nearToken.Location.Column,
                    message
                );
            }
            catch(IndexOutOfRangeException ex)
            {
                return new SQLSyntaxException(null, -1, -1, message);
            }
        }

        private string schema()
        {
            try
            {
                return lexer.eatIdentifier();
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At schema position: "+ ex.Message);
            }

        }
        private string field()
        {
            try
            {
                return lexer.eatIdentifier();
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At field position: "+ex.Message);
            }
        }
        //not done:mocking for private
        public FieldInfo typeDef()
        {
            FieldType type;
            int txtLength = 0;

            try
            {
                if (lexer.matchKeyword("INT"))
                {
                    type = FieldType.INT;
                    lexer.eatKeyword("INT");
                }
                else if (lexer.matchKeyword("FLOAT"))
                {
                    type = FieldType.FLOAT;
                    lexer.eatKeyword("FLOAT");
                }
                else if (lexer.matchKeyword("CHAR"))
                {
                    type = FieldType.CHAR;
                    lexer.eatKeyword("CHAR");
                }
                else if (lexer.matchKeyword("VARCHAR"))
                {
                    type = FieldType.VARCHAR;
                    lexer.eatKeyword("VARCHAR");
                    lexer.eatDelimiter("(");
                    txtLength = Convert.ToInt32(lexer.eatNumberConstant());
                    lexer.eatDelimiter(")");

                }
                else if (lexer.matchKeyword("BOOLEAN"))
                {
                    type = FieldType.BOOLEAN;
                    lexer.eatKeyword("BOOLEAN");
                }
                else if (lexer.matchKeyword("DIST_FUZZYSET_INT"))
                {
                    type = FieldType.distFS_INT;
                    lexer.eatKeyword("DIST_FUZZYSET_INT");
                }
                else if (lexer.matchKeyword("DIST_FUZZYSET_FLOAT"))
                {
                    type = FieldType.distFS_FLOAT;
                    lexer.eatKeyword("DIST_FUZZYSET_FLOAT");
                }
                else if (lexer.matchKeyword("DIST_FUZZYSET_TEXT"))
                {
                    type = FieldType.distFS_TEXT;
                    lexer.eatKeyword("DIST_FUZZYSET_TEXT");
                }
                else if (lexer.matchKeyword("CONT_FUZZYSET"))
                {
                    type = FieldType.contFS;
                    lexer.eatKeyword("CONT_FUZZYSET");
                }
                else
                {
                    throw createSQLSyntaxException($"{lexer.getCurrentToken().Value} isnt' a data type");
                }
                return new FieldInfo(type, txtLength);
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At type definition position: "+ex.Message);
            }

        }
        //not done: mocking for private
        public Field fieldDef()
        {
            try
            {
                string fieldName = field();
                FieldInfo fieldInfo = typeDef();
                return new Field(fieldName, fieldInfo);
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At field definition: "+ex.Message);
            }
        }
        //mot done: mocking for private
        public List<Field> fieldDefs()
        {
            try
            {
                List<Field> fieldDefList = new List<Field>();
                fieldDefList.Add(fieldDef());
                while (lexer.matchDelimiter(","))
                {
                    lexer.eatDelimiter(",");
                    if (!lexer.matchAnyKeyword())
                    {
                        //this is where my code differs from Database Design and Implementation Book by Edward Sciore
                        //to reduce method invocationmy code doesn't recursively call fieldDefs
                        //as it is supposed in Recursive Descent parsing technique
                        fieldDefList.Add(fieldDef());
                    }

                }
                return fieldDefList;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At field definitions: "+ex.Message);
            }
        }
        //not done: mocking for private
        public List<string> primaryAttributes()
        {
            try
            {
                List<string> fieldNames = new List<string>();
                fieldNames.Add(field());
                while (lexer.matchDelimiter(","))
                {
                    lexer.eatDelimiter(",");
                    fieldNames.Add(field());
                }
                return fieldNames;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At primary attribute: "+ex.Message);
            }
        }
        //not done: mocking for private
        public ConstraintData constraintDef()
        {
            try
            {
                lexer.eatKeyword("CONSTRAINT");
                string constraintName = lexer.eatIdentifier();
                lexer.eatKeyword("PRIMARY");
                lexer.eatKeyword("KEY");
                lexer.eatDelimiter("(");
                List<string> primaryAttributeList = primaryAttributes();
                lexer.eatDelimiter(")");
                return new ConstraintData(constraintName, ConstraintType.IDENTITY, primaryAttributeList);
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At constraint definition: "+ex.Message);
            }
        }
        public FPRDBSchema createSchema()
        {
            try
            {
                lexer.eatKeyword("CREATE");
                lexer.eatKeyword("SCHEMA");
                string schemaName = schema();
                lexer.eatDelimiter("(");
                List<Field> fieldDefList = fieldDefs();
                ConstraintData constraintData = null;

                if (lexer.matchKeyword("CONSTRAINT"))
                {
                    constraintData = constraintDef();
                }
                lexer.eatDelimiter(")");

                if (!this.lexer.isEndOfToken())
                    throw this.createSQLSyntaxException($"Extraneous input {this.lexer.getCurrentToken().Text}, expecting EOF");

                return new FPRDBSchema(schemaName
                    , fieldDefList
                    , (constraintData != null) ? constraintData.getFields() : null
                    , (constraintData != null) ? constraintData.getName() : null);

            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException(ex.Message);
            }
        }
        public Object create()
        {
            try
            {
                if (!lexer.matchKeyword("CREATE"))
                    throw createSQLSyntaxException("Not a create statement");
                Token peekNextToken = lexer.peekNext();
                if (peekNextToken.Terminal.Name == "identifier" && peekNextToken.Text.ToUpper() == "SCHEMA")
                {
                    return createSchema();
                }
                else
                {
                    return createRelation();
                }
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At schema position: " + ex.Message);
            }
            finally
            {
                this.lexer.clearTokens();
            }
        }
        public string relation()
        {
            try
            {
                return lexer.eatIdentifier();
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At relation position: "+ex.Message);
            }
        }
        public FPRDBRelation createRelation()
        {
            try
            {
                //if (!lexer.matchKeyword("CREATE"))
                //    throw createSQLSyntaxException("Not a create relation statement");
                lexer.eatKeyword("CREATE");
                //if (!lexer.matchKeyword("RELATION"))
                //    throw createSQLSyntaxException("Not a create relation statement");
                lexer.eatKeyword("RELATION");
                string relationName = relation();
                //if (!lexer.matchKeyword("ON"))
                //    throw createSQLSyntaxException("Not a create relation statement");
                lexer.eatKeyword("ON");
                string schemaName = schema();

                if (!this.lexer.isEndOfToken())
                    throw this.createSQLSyntaxException($"Extraneous input {this.lexer.getCurrentToken().Text}, expecting EOF");

                return new FPRDBRelation(relationName, schemaName);
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At schema position: " + ex.Message);
            }
        }
        public List<string> fieldList()
        {
            try
            {
                List<string> fields = new List<string>();
                fields.Add(field());
                while (lexer.matchDelimiter(","))
                {
                    lexer.eatDelimiter(",");
                    fields.Add(field());
                }
                return fields;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At field list position: " + ex.Message);
            }
        }
        public Constant constant()
        {
            try
            {
                if (lexer.matchNumberConstant())
                {
                    object value = lexer.eatNumberConstant();
                    if (value is int)
                        return new IntConstant((int)value);
                    else
                        return new FloatConstant((float)value);
                }
                else if (lexer.matchStringConstant())
                {
                    return new StringConstant(lexer.eatStringConstant());
                }
                else if (lexer.matchBooleanConstant())
                {
                    return new BooleanConstant(lexer.eatBooleanConstant());
                }
                else if (lexer.matchFuzzySetConstant())
                {
                    return new FuzzySetConstant(lexer.eatFuzzySetConstant());
                }
                else
                {
                    throw createSQLSyntaxException("Not a constant value");
                }
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At schema position: " + ex.Message);
            }
        }
        private bool isOfType<TExpected>(object value)
        {
            if (value == null)
                return false;
            else
            {
                Type type = typeof(TExpected);
                return type.IsInstanceOfType(value);
            }
        }

        public PossibleValue possibleValue()
        {
            try
            {
                lexer.eatDelimiter("(");
                Constant constantValue = constant();
                lexer.eatDelimiter(",");
                lexer.eatDelimiter("[");
                float lowerBound = Convert.ToSingle(lexer.eatNumberConstant());
                lexer.eatDelimiter(",");
                float upperBound = Convert.ToSingle(lexer.eatNumberConstant());
                lexer.eatDelimiter("]");
                lexer.eatDelimiter(")");
                return new PossibleValue(constantValue, lowerBound, upperBound);
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At possible value position: " + ex.Message);
            }
        }
        public FuzzyProbabilisticValueParsingData fuzzyProbabilisticValue()
        {
            try
            {
                FuzzyProbabilisticValueParsingData ans = new FuzzyProbabilisticValueParsingData();
                List<Constant> valueList = new List<Constant>();
                List<float> lowerBoundList = new List<float>();
                List<float> upperBoundList = new List<float>();

                //Type constraintType;
                lexer.eatDelimiter("{");
                PossibleValue possibleValueData = possibleValue();
                valueList.Add(possibleValueData.constant);
                lowerBoundList.Add(possibleValueData.lowerBound);
                upperBoundList.Add(possibleValueData.upperBound);

                while (lexer.matchDelimiter(","))
                {
                    lexer.eatDelimiter(",");
                    possibleValueData = possibleValue();
                    //if(valueList.Count!=0 
                    //    && !(possibleValueData.constant is FuzzySetConstant)
                    //    && valueList[0].GetType()!= possibleValueData.constant.GetType())
                    //{
                    //    throw new SemanticException("Values within a fuzzy probabilistic value must come from the same domain");
                    //}
                    valueList.Add(possibleValueData.constant);
                    lowerBoundList.Add(possibleValueData.lowerBound);
                    upperBoundList.Add(possibleValueData.upperBound);
                }
                lexer.eatDelimiter("}");
                ans.valueList = valueList;
                ans.intervalProbLowerBoundList = lowerBoundList;
                ans.intervalProbUpperBoundList = upperBoundList;
                return ans;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At fuzzy probabilistic value position: " + ex.Message);
            }

        }
        public List<FuzzyProbabilisticValueParsingData> fuzzyProbabilisticValueList()
        {
            try
            {
                List<FuzzyProbabilisticValueParsingData> ans = new List<FuzzyProbabilisticValueParsingData>();
                ans.Add(fuzzyProbabilisticValue());
                while (lexer.matchDelimiter(","))
                {
                    lexer.eatDelimiter(",");
                    ans.Add(fuzzyProbabilisticValue());
                }
                return ans;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At fuzzy probabilistic value list position: " + ex.Message);
            }
        }
        public InsertData insert()
        {
            try
            {
                //if(lexer.matchKeyword("INSERT"))
                //    throw createSQLSyntaxException("Not a insert statement");
                lexer.eatKeyword("INSERT");

                //if(lexer.matchKeyword("INTO"))
                //    throw createSQLSyntaxException("Not a insert statement");
                lexer.eatKeyword("INTO");

                string relName = relation();
                //if (lexer.matchKeyword("("))
                //    throw createSQLSyntaxException("Not a insert statement");
                lexer.eatDelimiter("(");

                List<string> fields = fieldList();
                //if (lexer.matchKeyword(")"))
                //    throw createSQLSyntaxException("Not a insert statement");
                lexer.eatDelimiter(")");

                //if (lexer.matchKeyword("VALUES"))
                //    throw createSQLSyntaxException("Not a insert statement");
                lexer.eatKeyword("VALUES");

                //if (lexer.matchKeyword("("))
                //    throw createSQLSyntaxException("Not a insert statement");
                lexer.eatDelimiter("(");

                List<FuzzyProbabilisticValueParsingData> insertValues = fuzzyProbabilisticValueList();

                //if (lexer.matchKeyword(")"))
                //    throw createSQLSyntaxException("Not a insert statement");
                lexer.eatDelimiter(")");

                if (!this.lexer.isEndOfToken())
                    throw this.createSQLSyntaxException($"Extraneous input {this.lexer.getCurrentToken().Text}, expecting EOF");

                return new InsertData(relName, fields, insertValues);
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException(ex.Message);
            }
            finally
            {
                this.lexer.clearTokens();
            }
        }
        public DeleteData delete()
        {
            try
            {
                //if (lexer.matchKeyword("DELETE"))
                //    throw createSQLSyntaxException("Not a delete statement");
                lexer.eatKeyword("DELETE");

                //if (lexer.matchKeyword("FROM"))
                //    throw createSQLSyntaxException("Not a delete statement");
                lexer.eatKeyword("FROM");

                string rel = relation();
                SelectionCondition condition = null;
                if (lexer.matchKeyword("WHERE"))
                {
                    lexer.eatKeyword("WHERE");
                    condition = selectionCondition();
                }

                if (!this.lexer.isEndOfToken())
                    throw this.createSQLSyntaxException($"Extraneous input {this.lexer.getCurrentToken().Text}, expecting EOF");

                return new DeleteData(rel, condition);
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException(ex.Message);
            }
            finally
            {
                this.lexer.clearTokens();
            }
        }
        public DropRelationData dropRelation()
        {
            try
            {
                lexer.eatKeyword("RELATION");
                DropRelationData data = new DropRelationData(relation());

                if (!this.lexer.isEndOfToken())
                    throw this.createSQLSyntaxException($"Extraneous input {this.lexer.getCurrentToken().Text}, expecting EOF");

                return data;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException(ex.Message);
            }
            finally
            {
                this.lexer.clearTokens();
            }
        }
        public DropSchemaData dropSchema()
        {
            try
            {
                lexer.eatKeyword("SCHEMA");
                DropSchemaData data = new DropSchemaData(schema());

                if (!this.lexer.isEndOfToken())
                    throw this.createSQLSyntaxException($"Extraneous input {this.lexer.getCurrentToken().Text}, expecting EOF");

                return data;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException(ex.Message);
            }
            finally
            {
                this.lexer.clearTokens();
            }
        }
        public object drop()
        {
            try
            {
                //if (lexer.matchKeyword("FROM"))
                //    throw createSQLSyntaxException("Not a drop statement");
                lexer.eatKeyword("DROP");

                if (lexer.matchKeyword("RELATION"))
                    return dropRelation();
                else if (lexer.matchKeyword("SCHEMA"))
                    return dropSchema();
                else
                    throw createSQLSyntaxException("After DROP keyword must be either SCHEMA or RELATIOM");
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException( ex.Message);
            }
            finally
            {
                this.lexer.clearTokens();
            }
        }
        public ModifyData modify()
        {
            try
            {
                //if (lexer.matchKeyword("UPDATE"))
                //    throw createSQLSyntaxException("Not a modify statement");
                lexer.eatKeyword("UPDATE");

                string relName = relation();

                //if (lexer.matchKeyword("SET"))
                //    throw createSQLSyntaxException("Not a modify statement");
                lexer.eatKeyword("SET");

                string assignedField = field();
                string assignSymbol = lexer.eatOperator();
                if (assignSymbol != "=")
                    throw createSQLSyntaxException("assign symbol = is expected");
                if (lexer.matchIdentifier())
                {
                    string assigningField = field();
                    SelectionCondition condition = null;
                    if (lexer.matchKeyword("WHERE"))
                    {
                        lexer.eatKeyword("WHERE");
                        condition = this.selectionCondition();
                    }

                    if (!this.lexer.isEndOfToken())
                        throw this.createSQLSyntaxException($"Extraneous input {this.lexer.getCurrentToken().Text}, expecting EOF");

                    return new FieldFieldModifyData(assignedField, relName, assigningField, condition);
                }
                else
                {
                    FuzzyProbabilisticValueParsingData assigningFProbValue = this.fuzzyProbabilisticValue();
                    SelectionCondition condition = null;
                    if (lexer.matchKeyword("WHERE"))
                    {
                        lexer.eatKeyword("WHERE");
                        condition = this.selectionCondition();
                    }
                    //check for extraneous token after parsing
                    if (!this.lexer.isEndOfToken())
                        throw this.createSQLSyntaxException($"Extraneous input {this.lexer.getCurrentToken().Text}, expecting EOF");

                    return new FieldFuzzProbValueModifyData(assigningFProbValue, relName, assignedField, condition);
                }
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException( ex.Message);
            }
            finally
            {
                this.lexer.clearTokens();
            }

        }
        public Object updateCommand()
        {
            try
            {
                if (lexer.matchKeyword("INSERT"))
                {
                    return insert();
                }
                else if (lexer.matchKeyword("DELETE"))
                {
                    return delete();
                }
                else if (lexer.matchKeyword("MODIFY"))
                {
                    throw new NotImplementedException();
                }
                else if (lexer.matchKeyword("DROP"))
                {
                    return drop();
                }
                else if (lexer.matchKeyword("UPDATE"))
                {
                    return modify();
                }
                else
                {
                    throw createSQLSyntaxException("Not a update command");
                }
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException(ex.Message);
            }
            finally
            {
                this.lexer.clearTokens();
            }
        }
        public List<SelectField> selectList()
        {
            try
            {
                List<SelectField> ans = new List<SelectField>();
                if (lexer.matchDelimiter("*"))
                {
                    lexer.eatDelimiter("*");
                    return new List<SelectField> { new SelectField("", "*") };
                }
                else
                {
                    string str1 = lexer.eatIdentifier();
                    string str2;
                    if (lexer.matchDelimiter("."))
                    {
                        lexer.eatDelimiter(".");
                        str2 = lexer.eatIdentifier();
                        ans.Add(new SelectField(str1, str2));
                    }
                    else
                        ans.Add(new SelectField("", str1));
                    while (lexer.matchDelimiter(","))
                    {
                        lexer.eatDelimiter(",");
                        str1 = lexer.eatIdentifier();
                        if (lexer.matchDelimiter("."))
                        {
                            lexer.eatDelimiter(".");
                            str2 = lexer.eatIdentifier();
                            ans.Add(new SelectField(str1, str2));
                        }
                        else
                            ans.Add(new SelectField("", str1));
                    }
                    return ans;
                }
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At select list position: " + ex.Message);
            }
        }
        public object fromList()
        {
            try
            {
                List<string> relNames = new List<string>();
                List<ProbabilisticCombinationStrategy> combinationStrategies = new List<ProbabilisticCombinationStrategy>();
                relNames.Add(relation());

                if (lexer.matchKeyword("NATURAL"))
                {
                    while (lexer.matchKeyword("NATURAL"))
                    {
                        lexer.eatKeyword("NATURAL");
                        lexer.eatKeyword("JOIN");
                        var combinationStrategy = ProbabilisticCombinationStrategyUtilities.convertStringToEnum(lexer.eatProbabilisticCombinationStrategy());
                        if (!ProbabilisticCombinationStrategyUtilities.isConjunctionStategy(combinationStrategy))
                            throw createSQLSyntaxException("NATUAL JOIN can only be paired with probabilistic conjunction strategy");
                        combinationStrategies.Add(combinationStrategy);
                        relNames.Add(relation());
                    }
                    return new NaturalJoinList(relNames, combinationStrategies);
                }
                else //if (lexer.matchDelimiter(","))
                {
                    while (lexer.matchDelimiter(","))
                    {
                        lexer.eatDelimiter(",");
                        relNames.Add(relation());
                    }
                    return relNames;
                }
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At from list position: " + ex.Message);
            }

        }
        public SelectionExpression PrimarySelectionExpression()
        {
            try
            {
                if (!lexer.matchDelimiter("("))
                {
                    string fieldName1 = field();
                    string compareOperator = lexer.eatOperator();
                    if (compareOperator != "=" || !lexer.matchProbabilisticCombinationStrategy())
                    {
                        Constant v;
                        if (this.lexer.matchNumberConstant())
                        {
                            object number = this.lexer.eatNumberConstant();
                            if (number is int)
                                v = new IntConstant((int)number);
                            else
                                v = new FloatConstant((float)number);
                        }
                        else if (this.lexer.matchStringConstant())
                        {
                            v = new StringConstant(this.lexer.eatStringConstant());
                        }
                        else if (this.lexer.matchBooleanConstant())
                        {
                            v = new BooleanConstant(this.lexer.eatBooleanConstant());
                        }
                        else //if (this.lexer.matchFuzzySetConstant())
                        {
                            v = new FuzzySetConstant(this.lexer.eatFuzzySetConstant());
                        }
                        return new AtomicSelectionExpressionFieldConstant(fieldName1, v, CompareOperatorUltilities.convertStringToEnum(compareOperator), this.metaDataManager);
                        //return new AtomicSelectionExpressionFieldConstant(fieldName1, ConstantUltilities.turnValueIntoConstant(v), CompareOperatorUltilities.convertStringToEnum(compareOperator), this.metaDataManager);
                    }
                    else
                    {
                        string strStrategy = lexer.eatProbabilisticCombinationStrategy();
                        ProbabilisticCombinationStrategy enumStrategy = ProbabilisticCombinationStrategyUtilities.convertStringToEnum(strStrategy);
                        if (!ProbabilisticCombinationStrategyUtilities.isConjunctionStategy(enumStrategy))
                            throw createSQLSyntaxException("Comparetor = must be paired with probabilistic conjunection strategy");
                        string fieldName2 = field();
                        return new AtomicSelectionExpressionFieldField(fieldName1, fieldName2, enumStrategy);

                    }
                }
                else
                {
                    lexer.eatDelimiter("(");
                    SelectionExpression res = selectionExpression();
                    lexer.eatDelimiter(")");
                    return res;
                }
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At select expression position: " + ex.Message);
            }

        }
        public SelectionExpression CONJUNCTIONSelectionExpression()
        {
            try
            {
                //not done: this code isn't elegant, look at ANDSelectionCondition
                SelectionExpression leftSExpression = PrimarySelectionExpression();
                SelectionExpression ans = leftSExpression;
                if (!lexer.hasNext() || !(lexer.matchProbabilisticCombinationStrategy()))
                    return ans;
                //else if (!(lexer.matchProbabilisticCombinationStrategy()) && !lexer.matchDelimiter(")"))
                //    throw createSQLSyntaxException("Supposed to be a probabilistic combination strategy");
                bool isEatNotConjunctionStrategy = false;
                while (lexer.matchProbabilisticCombinationStrategy())
                {
                    string strStrategy = lexer.eatProbabilisticCombinationStrategy();
                    ProbabilisticCombinationStrategy enumStrategy = ProbabilisticCombinationStrategyUtilities.convertStringToEnum(strStrategy);
                    if (!ProbabilisticCombinationStrategyUtilities.isConjunctionStategy(enumStrategy))
                    {
                        isEatNotConjunctionStrategy = true;
                        break;

                    }
                    else
                    {
                        SelectionExpression rightSExpression = PrimarySelectionExpression();
                        ans = new CompoundSelectionExpression(ans, rightSExpression, enumStrategy);
                    }
                }
                if (isEatNotConjunctionStrategy)
                    lexer.prev();
                return ans;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At select expression position: " + ex.Message);
            }
        }
        public SelectionExpression DISJUNCTION_DIFFERENCE_SelectionExpresion()
        {
            try
            {
                //not done: this code isn't elegant, look at ANDSelectionCondition
                SelectionExpression leftSExpression = CONJUNCTIONSelectionExpression();
                SelectionExpression ans = leftSExpression;
                if (!lexer.hasNext() || !(lexer.matchProbabilisticCombinationStrategy()))
                    return ans;
                //else if (!(lexer.matchProbabilisticCombinationStrategy()) && !lexer.matchDelimiter(")"))
                //    throw createSQLSyntaxException("Supposed to be a probabilistic combination strategy");
                string strStrategy;
                while (lexer.matchProbabilisticCombinationStrategy())
                {
                    strStrategy = lexer.eatProbabilisticCombinationStrategy();
                    ProbabilisticCombinationStrategy enumStrategy = ProbabilisticCombinationStrategyUtilities.convertStringToEnum(strStrategy);
                    SelectionExpression rightSExpression = CONJUNCTIONSelectionExpression();

                    ans = new CompoundSelectionExpression(ans, rightSExpression, enumStrategy);
                }
                return ans;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At select expression position: " + ex.Message);
            }
        }
        public SelectionExpression selectionExpression()
        {
            try
            {
                var expression = DISJUNCTION_DIFFERENCE_SelectionExpresion();

                return expression;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At select expression position: " + ex.Message);
            }
        }
        public SelectionCondition PrimarySelectionCondition()
        {
            try
            {
                if (lexer.matchDelimiter("{"))
                {
                    lexer.eatDelimiter("{");
                    SelectionCondition selCond = selectionCondition();
                    lexer.eatDelimiter("}");
                    return selCond;
                }
                else
                {
                    lexer.eatDelimiter("(");
                    SelectionExpression selectionEx = selectionExpression();
                    lexer.eatDelimiter(")");
                    lexer.eatDelimiter("[");
                    float lowerBound = Convert.ToSingle(lexer.eatNumberConstant());
                    lexer.eatDelimiter(",");
                    float upperBound = Convert.ToSingle(lexer.eatNumberConstant());
                    lexer.eatDelimiter("]");

                    return new AtomicSelectionCondition(selectionEx, lowerBound, upperBound);
                }
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At select condition position: " + ex.Message);
            }

        }
        public SelectionCondition NOTSelectionCondition()
        {
            try
            {
                if (lexer.matchKeyword("NOT"))
                {
                    lexer.eatKeyword("NOT");
                    SelectionCondition selectionCondt = PrimarySelectionCondition();
                    return new CompoundSelectionCondition(selectionCondt, null, LogicalConnective.NOT);
                }
                else
                {
                    return PrimarySelectionCondition();
                }
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At select condition position: " + ex.Message);
            }
        }
        public SelectionCondition ANDSelectionCondition()
        {
            try
            {
                SelectionCondition ans = NOTSelectionCondition();
                SelectionCondition nextSelectionCondt;
                while (lexer.matchKeyword("AND"))
                {
                    lexer.eatKeyword("AND");
                    nextSelectionCondt = NOTSelectionCondition();
                    ans = new CompoundSelectionCondition(ans, nextSelectionCondt, LogicalConnective.AND);
                }
                return ans;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At selection condition position: " + ex.Message);
            }
        }
        public SelectionCondition ORSelectionCondition()
        {
            try
            {
                SelectionCondition ans = ANDSelectionCondition();
                SelectionCondition nextSelectionCondt;
                while (lexer.matchKeyword("OR"))
                {
                    lexer.eatKeyword("OR");
                    nextSelectionCondt = ANDSelectionCondition();
                    ans = new CompoundSelectionCondition(ans, nextSelectionCondt, LogicalConnective.OR);
                }
                return ans;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At select condition: " + ex.Message);
            }
        }
        public SelectionCondition selectionCondition()
        {
            try
            {
                var condition = ORSelectionCondition();

                return condition;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At selection condition: " + ex.Message);
            }
        }
        public QueryData PrimaryQuery()
        {
            try
            {
                QueryData ans;
                if (lexer.matchDelimiter("("))
                {
                    lexer.eatDelimiter("(");
                    ans = query(false);
                    lexer.eatDelimiter(")");
                    return ans;
                }
                else if (lexer.matchKeyword("SELECT"))
                {
                    lexer.eatKeyword("SELECT");
                    List<SelectField> selectFields = selectList();
                    lexer.eatKeyword("FROM");
                    object from_list = fromList();
                    SelectionCondition selectionCondt = null;
                    if (lexer.matchKeyword("WHERE"))
                    {
                        lexer.eatKeyword("WHERE");
                        selectionCondt = selectionCondition();
                    }
                    if (from_list is List<string>)
                        return new BaseCartesianProductQueryData(selectFields, (List<string>)from_list, selectionCondt);
                    else
                        return new BaseNaturalJoinQueryData(selectFields, (NaturalJoinList)from_list, selectionCondt);

                }
                else
                    throw createSQLSyntaxException("Not a query");
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException(ex.Message);
            }
        }
        public QueryData INTERSECTIONQuery()
        {
            try
            {
                QueryData ans = PrimaryQuery();
                QueryData next;
                while (lexer.matchKeyword("INTERSECT"))
                {
                    lexer.eatKeyword("INTERSECT");
                    string strProbCombStrategy = lexer.eatProbabilisticCombinationStrategy();
                    ProbabilisticCombinationStrategy enumProbCombStrategy = ProbabilisticCombinationStrategyUtilities.convertStringToEnum(strProbCombStrategy);
                    if (!ProbabilisticCombinationStrategyUtilities.isConjunctionStategy(enumProbCombStrategy))
                        throw createSQLSyntaxException("INTERSECTION must be paired with probabilistic conjunction strategy");
                    next = PrimaryQuery();
                    ans = new CompoundQueryData(ans, next, SetConnective.INTERSECT, enumProbCombStrategy);
                }
                return ans;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException(ex.Message);
            }
        }
        public QueryData UNION_EXCEPT_Query()
        {
            try
            {
                QueryData ans = INTERSECTIONQuery();
                QueryData next;
                bool isUNION;
                while (lexer.matchKeyword("UNION") || lexer.matchKeyword("EXCEPT"))
                {
                    if (lexer.matchKeyword("UNION"))
                    {
                        lexer.eatKeyword("UNION");
                        isUNION = true;
                    }
                    else
                    {
                        lexer.eatKeyword("EXCEPT");
                        isUNION = false;
                    }
                    string strProbCombStrategy = lexer.eatProbabilisticCombinationStrategy();
                    ProbabilisticCombinationStrategy enumProbCombStrategy = ProbabilisticCombinationStrategyUtilities.convertStringToEnum(strProbCombStrategy);
                    if (isUNION && !ProbabilisticCombinationStrategyUtilities.isDisjunctionStategy(enumProbCombStrategy))
                        throw createSQLSyntaxException("UNION must be paired with probabilistic disjunction strategy");
                    else if (!isUNION && !ProbabilisticCombinationStrategyUtilities.isDifferenceStategy(enumProbCombStrategy))
                        throw createSQLSyntaxException("EXCEPT must be paired with probabilistic difference strategy");
                    next = INTERSECTIONQuery();
                    ans = (isUNION) ?
                        new CompoundQueryData(ans, next, SetConnective.UNION, enumProbCombStrategy)
                        : new CompoundQueryData(ans, next, SetConnective.EXCEPT, enumProbCombStrategy);
                }
                return ans;
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException(ex.Message);
            }
        }
        public QueryData query(bool isRootParsing)
        {
            //try{
            //    var data = UNION_EXCEPT_Query();

            //    return data;
            //}
            //finally{

            //    this.lexer.clearTokens();
            //}
            try
            {
                var data = UNION_EXCEPT_Query();

                if (isRootParsing && !this.lexer.isEndOfToken())
                    throw this.createSQLSyntaxException($"Extraneous input {this.lexer.getCurrentToken().Text}, expecting EOF");

                return data;
            }
            catch (MismatchTokenType ex)
            {
                this.lexer.clearTokens();
                throw createSQLSyntaxException(ex.Message);
            }
        }
        private Constant getConstantFromLexer()
        {
            try
            {
                if (lexer.matchNumberConstant())
                {
                    object tmpValue = lexer.eatNumberConstant();
                    if (tmpValue is int)
                        return new IntConstant((int)tmpValue);
                    else
                        return new FloatConstant((float)tmpValue);
                }
                else if (lexer.matchStringConstant())
                    return new StringConstant((string)lexer.eatStringConstant());
                else if (lexer.matchBooleanConstant())
                    return new BooleanConstant((bool)lexer.eatBooleanConstant());
                else //if (lexer.matchFuzzySetConstant())
                    return new FuzzySetConstant((string)lexer.eatFuzzySetConstant());
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException("At constant position:"+ex.Message);
            }
        }
        public RelationOnFuzzySetExpressionData relationOnFuzzySetsExpression()
        {
            try
            {
                Constant fs1Name = getConstantFromLexer();

                string strCompareOp = this.lexer.eatOperator();
                CompareOperation compareOp = CompareOperatorUltilities.convertStringToEnum(strCompareOp);

                Constant fs2Name = getConstantFromLexer();

                if (!this.lexer.isEndOfToken())
                    throw this.createSQLSyntaxException($"Extraneous input {this.lexer.getCurrentToken().Text}, expecting EOF");

                return new RelationOnFuzzySetExpressionData(fs1Name, compareOp, fs2Name);
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException(ex.Message);
            }
            finally
            {
                this.lexer.clearTokens();
            }
        }
        public SelectionExpressionOnSpecifiedTuplesData selectionExpressionOnSpecifiedTuples()
        {
            try
            {
                SelectionExpression selectionExpression = this.selectionExpression();
                lexer.eatKeyword("on");
                string relation = lexer.eatIdentifier();
                if (lexer.matchKeyword("from"))
                {
                    lexer.eatKeyword("from");
                    object startIndex = lexer.eatNumberConstant();
                    if (!(startIndex is int))
                        throw new SemanticException("Number after keyword 'from' must be int");

                    lexer.eatKeyword("take");
                    object noNextTuples = lexer.eatNumberConstant();
                    if (!(noNextTuples is int))
                        throw new SemanticException("Number after keyword 'take' must be int");

                    if (!this.lexer.isEndOfToken())
                        throw this.createSQLSyntaxException($"Extraneous input {this.lexer.getCurrentToken().Text}, expecting EOF");

                    return new SelectionExpressionOnSpecifiedTuplesData(relation, selectionExpression, (int)startIndex, (int)noNextTuples);
                }
                else
                {
                    if (!this.lexer.isEndOfToken())
                        throw this.createSQLSyntaxException($"Extraneous input {this.lexer.getCurrentToken().Text}, expecting EOF");
                    return new SelectionExpressionOnSpecifiedTuplesData(relation, selectionExpression);
                }
            }
            catch (MismatchTokenType ex)
            {
                throw createSQLSyntaxException(ex.Message);
            }
            finally
            {
                this.lexer.clearTokens();
            }
        }

    }
}
