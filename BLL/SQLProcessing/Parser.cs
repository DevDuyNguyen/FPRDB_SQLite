using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainObject;

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
        public FPRDBSchema createSchema()
        {
            try
            {
                lexer.eatKeyword("CREATE");
            }
            catch(MismatchTokenType ex)
            {

            }
        }

    }
}
