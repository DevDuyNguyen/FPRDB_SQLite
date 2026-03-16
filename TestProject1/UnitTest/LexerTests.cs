using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.SQLProcessing;

namespace TestProject1.UnitTest
{
    //not done: lacking testing knowledge
    //not done: lack applying test case technique
    //not done: lacking the true essence of what testing is
    //not done: lacking test coverage, low test coverage
    public class LexerTests
    {
        private Lexer lexer;

        [Fact]
        public void Lexer_simpleSQL_success()
        {
            //arrange
            string sql = "select * from student";
            this.lexer = new Lexer(sql);
            //action
            //assert
            Assert.Equal(true, lexer.matchKeyword("select"));
            lexer.eatKeyword("select");

            Assert.Equal(true, lexer.matchDelimiter("*"));
            lexer.eatDelimiter("*");

            Assert.Equal(true, lexer.matchKeyword("from"));
            lexer.eatKeyword("from");

            Assert.Equal("student", lexer.eatIdentifier());
        }

    }
}
