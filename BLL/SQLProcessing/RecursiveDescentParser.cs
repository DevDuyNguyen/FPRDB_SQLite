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

        public RecursiveDescentParser(Lexer lexer)
        {
            this.lexer = lexer;
        }

        public void parse(String sqlStatement)
        {
            this.lexer.analyze(sqlStatement);
        }

        private SQLSyntaxException createSQLSyntaxException(string message)
        {
            Token nearToken;
            nearToken = this.lexer.getCurrentToken();
            return new SQLSyntaxException(nearToken.Value.ToString(),
                nearToken.Location.Line,
                nearToken.Location.Column,
                message
            );
        }

        private string schema()
        {
            return lexer.eatIdentifier();
        }
        private string field()
        {
            return lexer.eatIdentifier();
        }
        //not done:mocking for private
        public FieldInfo typeDef()
        {
            FieldType type;
            int txtLength=0;
           
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
                txtLength = (int)lexer.eatNumberConstant();
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
                throw createSQLSyntaxException("Not a data type");
            }
            return new FieldInfo(type, txtLength);

        }
        //not done: mocking for private
        public Field fieldDef()
        {
            string fieldName = field();
            FieldInfo fieldInfo = typeDef();
            return new Field(fieldName, fieldInfo);
        }
        //mot done: mocking for private
        public List<Field> fieldDefs()
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
        //not done: mocking for private
        public List<string> primaryAttributes()
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
        //not done: mocking for private
        public ConstraintData constraintDef()
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
        public FPRDBSchema createSchema()
        {
            try
            {
                lexer.eatKeyword("CREATE");
                lexer.eatKeyword("SCHEMA");
                string schemaName = schema();
                lexer.eatDelimiter("(");
                List<Field> fieldDefList = fieldDefs();
                ConstraintData constraintData=null;

                if (lexer.matchKeyword("CONSTRAINT"))
                {
                    constraintData = constraintDef();
                }
                return new FPRDBSchema(schemaName
                    ,fieldDefList
                    ,(constraintData!=null)?constraintData.getFields():null
                    ,(constraintData != null) ? constraintData.getName() : null);

            }
            catch(MismatchTokenType ex)
            {
                throw createSQLSyntaxException(ex.Message);
            }
        }
        public Object create()
        {
            if (!lexer.matchKeyword("CREATE"))
                throw createSQLSyntaxException("Not a create statement");
            Token peekNextToken = lexer.peekNext();
            if (peekNextToken.Terminal.Name=="identifier"&& peekNextToken.Text== "SCHEMA")
            {
                return createSchema();
            }
            else
            {
                return createRelation();
            }
        }
        public string relation()
        {
            return lexer.eatIdentifier();
        }
        public FPRDBRelation createRelation()
        {
            lexer.eatKeyword("CREATE");
            lexer.eatKeyword("RELATION");
            string relationName = relation();
            lexer.eatKeyword("ON");
            string schemaName = schema();
            return new FPRDBRelation(relationName, schemaName);
        }
        public List<string> fieldList()
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
        public Constant constant()
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
        public FuzzyProbabilisticValueParsingData fuzzyProbabilisticValue()
        {
            FuzzyProbabilisticValueParsingData ans = new FuzzyProbabilisticValueParsingData();
            List<Constant> valueList=new List<Constant>();
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
                if(valueList.Count!=0 
                    && !(possibleValueData.constant is FuzzySetConstant)
                    && valueList[0].GetType()!= possibleValueData.constant.GetType())
                {
                    throw new SemanticException("Values within a fuzzy probabilistic value must come from the same domain");
                }
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
        public List<FuzzyProbabilisticValueParsingData> fuzzyProbabilisticValueList()
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
        public InsertData insert()
        {
            lexer.eatKeyword("INSERT");
            lexer.eatKeyword("INTO");
            string relName = relation();
            lexer.eatDelimiter("(");
            List<string> fields = fieldList();
            lexer.eatDelimiter(")");
            lexer.eatKeyword("VALUES");
            lexer.eatDelimiter("(");
            List<FuzzyProbabilisticValueParsingData> insertValues = fuzzyProbabilisticValueList();
            lexer.eatDelimiter(")");
            return new InsertData(relName, fields, insertValues);
        }
        public Object updateCommand()
        {
            if (lexer.matchKeyword("INSERT"))
            {
                return insert();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public List<SelectField> selectList()
        {
            List<SelectField> ans = new List<SelectField>();
            if (lexer.matchDelimiter("*")){
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
        public object fromList()
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
                        throw createSQLSyntaxException("NATUAL JOIN can only be pared with probabilistic conjunction strategy");
                    combinationStrategies.Add(combinationStrategy);
                    relNames.Add(relation());
                }
                return new NaturalJoinList(relNames, combinationStrategies);
            }
            else if (lexer.matchDelimiter(","))
            {
                while (lexer.matchDelimiter(","))
                {
                    lexer.eatDelimiter(",");
                    relNames.Add(relation());
                }
                return relNames;
            }
            else
                throw createSQLSyntaxException("This isn't a FROM clause statement");
        }
        public SelectionExpression PrimarySelectionExpression()
        {
            if (!lexer.matchDelimiter("("))
            {
                string fieldName1 = field();
                string compareOperator = lexer.eatOperator();
                if (compareOperator != "=" || !lexer.matchProbabilisticCombinationStrategy())
                {
                    object v = lexer.eatConstant();
                    return new AtomicSelectionExpressionFieldConstant(fieldName1, ConstantUltilities.turnValueIntoConstant(v), CompareOperatorUltilities.convertStringToEnum(compareOperator));
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
                SelectionExpression res= selectionExpression();
                lexer.eatDelimiter(")");
                return res;
            }

        }
        public SelectionExpression CONJUNCTIONSelectionExpression()
        {
            //not done: this code isn't elegant, look at ANDSelectionCondition
            SelectionExpression leftSExpression = PrimarySelectionExpression();
            SelectionExpression ans= leftSExpression;
            if (!lexer.hasNext())
                return ans;
            else if (!(lexer.matchProbabilisticCombinationStrategy()) && !lexer.matchDelimiter(")"))
                throw createSQLSyntaxException("Supposed to be a probabilistic combination strategy");
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
                    ans= new CompoundSelectionExpression(ans, rightSExpression, enumStrategy);
                }
            }
            if (isEatNotConjunctionStrategy)
                lexer.prev();
            return ans;
        }
        public SelectionExpression DISJUNCTION_DIFFERENCE_SelectionExpresion()
        {
            //not done: this code isn't elegant, look at ANDSelectionCondition
            SelectionExpression leftSExpression = CONJUNCTIONSelectionExpression();
            SelectionExpression ans = leftSExpression;
            if (!lexer.hasNext())
                return ans;
            else if (!(lexer.matchProbabilisticCombinationStrategy()) && !lexer.matchDelimiter(")"))
                throw createSQLSyntaxException("Supposed to be a probabilistic combination strategy");
            string strStrategy;
            while (lexer.matchProbabilisticCombinationStrategy())
            {
                strStrategy=lexer.eatProbabilisticCombinationStrategy();
                ProbabilisticCombinationStrategy enumStrategy = ProbabilisticCombinationStrategyUtilities.convertStringToEnum(strStrategy);
                SelectionExpression rightSExpression = CONJUNCTIONSelectionExpression();

                ans=new CompoundSelectionExpression(ans, rightSExpression, enumStrategy);
            }
            return ans;

        }
        public SelectionExpression selectionExpression()
        {
            return DISJUNCTION_DIFFERENCE_SelectionExpresion();
        }
        public SelectionCondition PrimarySelectionCondition()
        {
            lexer.eatDelimiter("(");
            if (lexer.matchDelimiter("("))
            {
                SelectionCondition selCond = selectionCondition();
                lexer.eatDelimiter(")");
                return selCond;
            }
            else
            {
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
        public SelectionCondition NOTSelectionCondition()
        {
            if (lexer.matchKeyword("NOT")){
                lexer.eatKeyword("NOT");
                SelectionCondition selectionCondt = PrimarySelectionCondition();
                return new CompoundSelectionCondition(selectionCondt, null, LogicalConnective.NOT);
            }
            else
            {
                return PrimarySelectionCondition();
            }
        }
        public SelectionCondition ANDSelectionCondition()
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
        public SelectionCondition ORSelectionCondition()
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
        public SelectionCondition selectionCondition()
        {
            return ORSelectionCondition();
        }
        public QueryData PrimaryQuery()
        {
            if (lexer.matchKeyword("SELECT")){
                lexer.eatKeyword("SELECT");

            }
            else if (lexer.matchDelimiter("("))
            {
                List<SelectField> selectFields = selectList();
                lexer.eatKeyword("FROM");
                object from_list = fromList();
                if (lexer.matchKeyword("WHERE"))
                {
                    lexer.eatKeyword("WHERE");
                }
            }
            else
            {
                throw createSQLSyntaxException("The statement isn't a query");
            }
            throw new NotFiniteNumberException();
        }
        public QueryData INTERSECTIONQuery()
        {

            throw new NotFiniteNumberException();
        }
        public QueryData UNION_EXCEPT_Query()
        {

            throw new NotFiniteNumberException();
        }
        public QueryData query()
        {

            throw new NotFiniteNumberException();
        }
    }
}
