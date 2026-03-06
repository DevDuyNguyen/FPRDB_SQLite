using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.SQLProcessing;

namespace TestProject1.UnitTest
{
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

            Assert.Equal("*", lexer.eatIdentifier());

            Assert.Equal(true, lexer.matchKeyword("from"));
            lexer.eatKeyword("from");

            Assert.Equal("student", lexer.eatIdentifier());
        }

    }
}
