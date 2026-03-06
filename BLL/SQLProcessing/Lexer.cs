using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class MismatchTokenType : Exception
    {
        public string Message{
            get;
            private set;
        }
        public MismatchTokenType(string supposedTokenType, Token currentToken):base()
        {
            this.Message = $"The current token {currentToken.Value} isn't {supposedTokenType}";
        }
    }
    internal class FPRDBSQLTerminals : Grammar
    {
        public FPRDBSQLTerminals() : base(false)
        {
            var comma = new KeyTerm(",", "delimiter");
            var dot = new KeyTerm(".", "delimiter");
            var asterisk = new KeyTerm("*", "asterisk");
            var openParenthesis = new KeyTerm("(", "delimiter");
            var closedParenthesis = new KeyTerm(")", "delimiter");
            var openSquareBracket = new KeyTerm("[", "delimiter");
            var closedSquareBracket = new KeyTerm("]", "delimiter");

            //var select = ToTerm("SELECT", "keyword");
            //var from = ToTerm("FROM", "keyword");
            //var where = ToTerm("WHERE", "keyword");
            //var and = ToTerm("AND", "keyword");
            //var or = ToTerm("OR", "keyword");
            //var not = ToTerm("NOT", "keyword");
            //var naturaljoin = ToTerm("NATURAL JOIN", "keyword");
            //var union = ToTerm("UNION", "keyword");
            //var intersect = ToTerm("INTERSECT", "keyword");
            //var except = ToTerm("EXCEPT", "keyword");

            var numberConstant = new NumberLiteral("numberConstant");
            var singleQuoteStringConstant = new StringLiteral("singleQuoteStringConstant", "'");
            var doubleQuoteStringConstant = new StringLiteral("doubleQuoteStringConstant", "\"");
            var booleanConstant = new ConstantTerminal("booleanConstant");
            booleanConstant.Add("TRUE", true);
            booleanConstant.Add("True", true);
            booleanConstant.Add("true", true);
            booleanConstant.Add("FALSE", false);
            booleanConstant.Add("False", false);
            booleanConstant.Add("false", false);

            var identifier = new IdentifierTerminal("identifier");

            var eq = ToTerm("=", "comparison operator");
            var neq = ToTerm("!=", "comparison operator");
            var lt = ToTerm("<", "comparison operator");
            var leq = ToTerm("<=", "comparison operator");
            var gt = ToTerm(">", "comparison operator");
            var geq = ToTerm(">=", "comparison operator");
            var subseteq = ToTerm("⊆", "comparison operator");
            var belongTo = ToTerm("∈", "comparison operator");
            var rightDoubleArrow = ToTerm("⇒", "comparison operator");


            var probConjunctionStrategy = ToTerm("⨂", "probabilistic combination strategy");
            var probDisjunctionStrategy = ToTerm("⨁", "probabilistic combination strategy");
            var probDifferencetionStrategy = ToTerm("⦵", "probabilistic combination strategy");

            var negative = ToTerm("-", "unary operator");
            var positive = ToTerm("+", "unary operator");

            var any = new NonTerminal("any");
            any.Rule = comma | dot | asterisk | openParenthesis | closedParenthesis | openSquareBracket | closedSquareBracket
                | numberConstant | singleQuoteStringConstant| doubleQuoteStringConstant | booleanConstant | identifier
                | eq | neq | lt | leq | gt | geq | subseteq | belongTo | rightDoubleArrow
                | probConjunctionStrategy | probDisjunctionStrategy | probDifferencetionStrategy
                | negative | positive;
            //any.Rule = comma | openParenthesis | closedParenthesis
            //    | openSquareBracket | closedSquareBracket
            //    | select | from | where | and | or | not
            //    | naturaljoin | union | intersect | except
            //    | numberConstant | stringConstant | booleanConstant | identifier
            //    | eq | neq | lt | leq | gt | geq | subseteq | belongTo | rightDoubleArrow
            //    | probConjunctionStrategy | probDisjunctionStrategy | probDifferencetionStrategy;

            var content = new NonTerminal("content");
            content.Rule = any | any + content;

            this.Root = content;

        }
    }
    public class Lexer
    {
        private Parser _parser;
        private FPRDBSQLTerminals terminals;
        private Token currentToken;
        private Scanner _scanner;
        private int currentIndex = -1;
        private List<Token> tokens = new List<Token>();
        private int tokenListLength { get => this.tokens.Count;}
        private List<string> keywords = new List<String>() {"select", "from", "where", "and", "or", "not", "natural", "join",
            "union", "intersect", "except", "not", "and", "or",
            "create", "schema", "int", "float", "char", "varchar", "boolean",
            "dist_fuzzyset_int", "dist_fuzzyset_float", "dist_fuzzyset_text",
            "cont_fuzzyset", "constraint", "primary", "key",
            "relation", "on", "insert", "into", "values", "update", "set", "delete", "drop"};

        public void printAllToken()
        {
            while (this.currentToken != null && this.currentToken.Terminal.Name != "EOF")
            {
                Debug.WriteLine($"Terminal: {this.currentToken.Terminal.Name} | value: {this.currentToken.Value}");
                next();
            }
        }
        public Lexer() { }
        public Lexer(string s)
        {
            analyze(s);
        }

        public void analyze(string s)
        {
            this.terminals = new FPRDBSQLTerminals();
            this._parser = new Parser(this.terminals);
            this._scanner = this._parser.Scanner;

            this._scanner.VsSetSource(s, 0);

            Token token;
            int state = 0;
            //buffered the tokens retrieved from Irony
            do
            {
                token = this._scanner.VsReadToken(ref state);
                if (token != null && token.Category == TokenCategory.Content)
                    this.tokens.Add(token);
            } while (token != null && token.Terminal.Name != "EOF");
            next();
        }

        public void next()
        {
            if (this.currentIndex < this.tokenListLength - 1)
            {
                this.currentIndex += 1;
                this.currentToken = this.tokens[this.currentIndex];
            }
            else
            {
                this.currentToken = null;
            }
        }
        public bool matchDelimiter(string delimiter)
        {
            if (this.currentToken.Terminal.Name == "delimiter"
                && this.currentToken.Text==delimiter)
                return true;
            else
                return false;
        }
        public void eatDelimiter(string delimiter)
        {
            if (!matchDelimiter(delimiter))
                throw new MismatchTokenType("delimiter", this.currentToken);
            next();
        }

        public bool matchKeyword(string w)
        {
            if (this.currentToken.Terminal.Name == "identifier" && this.keywords.Contains(w.ToLower()))
            {
                string tokenStr = (string)this.currentToken.Value;
                tokenStr = tokenStr.ToLower();
                if (tokenStr == w.ToLower())
                    return true;
            }
            return false;
        }
        public void eatKeyword(string w)
        {
            if (!matchKeyword(w))
                throw new MismatchTokenType("keyword", this.currentToken);
            next();
        }
        //non-orphan unary operator: -1, -12
        public bool matchOrphanUnaryOperator()
        {
            if (this.currentToken.Terminal.Name == "unary operator")
            {
                Token peekNexToken = (this.currentIndex<this.tokenListLength)? 
                    this.tokens[this.currentIndex + 1]:null;
                if (peekNexToken == null && peekNexToken.Terminal.Name != "numberConstant")
                    return true;
            }
            return false; 
        }
        //not done: this tokenization shouldn't be at here
        //it should be at when you are creating field Tokens
        public bool matchNumberConstant()
        {
            if (this.currentToken.Terminal.Name == "unary operator")
            {
                if(!matchOrphanUnaryOperator())
                {
                    int number = 1;
                    if ((string)this.currentToken.Value == "-")
                        number = -1;
                    this.tokens.RemoveAt(this.currentIndex);
                    this.currentToken = this.tokens[this.currentIndex];
                    if (this.currentToken.Terminal.Name == "numberConstant")
                    {
                        this.currentToken.Value = number * Convert.ToDouble(this.currentToken.Value);
                        //this.tokens[this.currentIndex].Value = this.currentToken.Value;
                        return true;
                    }
                }
            }
            else if (this.currentToken.Terminal.Name == "numberConstant")
                return true;
            return false;
        }
        public double eatNumberConstant()
        {
            if (!matchNumberConstant())
                throw new MismatchTokenType("number constant", this.currentToken);

            double res = Convert.ToDouble(this.currentToken.Value);
            next();
            return res;
        }

        public bool matchStringConstant()
        {
            if (this.currentToken.Terminal.Name == "singleQuoteStringConstant"
                || this.currentToken.Terminal.Name == "doubleQuoteStringConstant")
            {
                return true;
            }
            return false;
        }
        public string eatStringConstant()
        {
            if (!matchStringConstant())
                throw new MismatchTokenType("string constant", this.currentToken);
            string res= (string)this.currentToken.Value;
            next();
            return res;
        }

        public bool matchBooleanConstant()
        {
            if (this.currentToken.Terminal.Name == "booleanConstant")
                return true;
            return false;
        }
        public bool eatBooleanConstant()
        {
            if (!matchBooleanConstant())
                throw new MismatchTokenType("boolean constant", this.currentToken);
            bool res = (bool)this.currentToken.Value;
            next();
            return res;
        }

        public bool matchOperator()
        {
            if (this.currentToken.Terminal.Name == "comparison operator")
                return true;
            return false;
        }
        public string eatOperator()
        {
            if (!matchOperator())
                throw new MismatchTokenType("comparison operator", this.currentToken);
            string res= (string)this.currentToken.Value;
            next();
            return res;
        }

        public bool matchProbabilisticCombinationStrategy()
        {
            if (this.currentToken.Terminal.Name == "probabilistic combination strategy")
                return true;
            return false;
        }
        public string eatProbabilisticCombinationStrategy()
        {
            if (!matchProbabilisticCombinationStrategy())
                throw new MismatchTokenType("probabilistic combination strategy", this.currentToken);
            string res= (string)this.currentToken.Value;
            next();
            return res;
        }

        public bool matchIdentifier()
        {
            return (this.currentToken.Terminal.Name == "identifier") || (this.currentToken.Terminal.Name == "asterisk");
        }
        //not done: this tokenization shouldn't be at here
        //it should be at when you are creating field Tokens
        public string eatIdentifier()
        {
            if (!matchIdentifier())
                throw new MismatchTokenType("identifier", this.currentToken);
            string res = (string)this.currentToken.Value;

            Token peekNexToken = (this.currentIndex+1 <= this.tokenListLength-1) ?
                    this.tokens[this.currentIndex + 1] : null;
            if(peekNexToken!=null && peekNexToken.Terminal.Name=="delimiter" 
                && peekNexToken.Text == ".")
            {
                Token peekNNextToken= (this.currentIndex+2 <= this.tokenListLength-1) ?
                    this.tokens[this.currentIndex + 2] : null;
                if(peekNNextToken != null && peekNNextToken.Terminal.Name== "numberConstant")
                {
                    res = res + "." + peekNNextToken.Value.ToString();
                    this.currentToken.Value = res;
                    this.tokens.RemoveAt(this.currentIndex+1);
                    this.tokens.RemoveAt(this.currentIndex + 1);
                }
            } 
            next();
            return res;    
        }
        public Token getPrevToken()
        {
            if (this.currentIndex != 0)
                return this.tokens[this.currentIndex - 1];
            else
            {
                throw new IndexOutOfRangeException();
            }
        }
        public Token getCurrentToken()
        {
            if (this.currentIndex <= this.tokens.Count-1)
                return this.tokens[this.currentIndex];
            else
            {
                throw new IndexOutOfRangeException();
            }
        }
    }
}
