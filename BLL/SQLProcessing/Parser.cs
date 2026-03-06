using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Exceptions;
using Irony.Parsing;

namespace BLL.SQLProcessing
{
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
        //mocking for private
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
        //mocking for private
        public Field fieldDef()
        {
            string fieldName = field();
            FieldInfo fieldInfo = typeDef();
            return new Field(fieldName, fieldInfo);
        }
        private List<Field> fieldDefs()
        {
            List<Field> fieldDefList = new List<Field>();
            fieldDefList.Add(fieldDef());
            while (lexer.matchDelimiter(","))
            {
                lexer.eatDelimiter(",");
                //this is where my code differs from Database Design and Implementation Book by Edward Sciore
                //to reduce method invocationmy code doesn't recursively call fieldDefs
                //as it is supposed in Recursive Descent parsing technique
                fieldDefList.Add(fieldDef());
            }
            return fieldDefList;
        }
        private List<string> primaryAttributes()
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
        private ConstraintData constraintDef()
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

                if (lexer.matchDelimiter(","))
                {
                    lexer.eatDelimiter(",");
                    constraintData = constraintDef();
                }
                return new FPRDBSchema(schemaName
                    ,fieldDefList
                    ,(constraintData!=null)?constraintData.getFields():null);

            }
            catch(MismatchTokenType ex)
            {
                throw createSQLSyntaxException(ex.Message);
            }
            catch(SQLSyntaxException ex)
            {
                throw ex;
            }
        }


    }
}
