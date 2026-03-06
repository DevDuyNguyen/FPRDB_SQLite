using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.SQLProcessing;
using BLL.Common;
using BLL.DomainObject;

namespace TestProject1.UnitTest
{
    public class typeDefTests
    {
        private RecursiveDescentParser parser;
        public typeDefTests()
        {
            CompositionRoot compRoot = new CompositionRoot();
            this.parser = compRoot.getParser();
        }

        [Theory]
        [InlineData("INT", FieldType.INT, 0)]
        [InlineData("FLOAT", FieldType.FLOAT, 0)]
        [InlineData("CHAR", FieldType.CHAR, 0)]
        [InlineData("VARCHAR (50)", FieldType.VARCHAR, 50)]
        [InlineData("BOOLEAN", FieldType.BOOLEAN, 0)]
        [InlineData("DIST_FUZZYSET_INT", FieldType.distFS_INT, 0)]
        [InlineData("DIST_FUZZYSET_FLOAT", FieldType.distFS_FLOAT, 0)]
        [InlineData("DIST_FUZZYSET_TEXT", FieldType.distFS_TEXT, 0)]
        [InlineData("CONT_FUZZYSET", FieldType.contFS, 0)]
        public void Parser_typeDef_success(string str
            ,FieldType fieldType
            ,int txtLength)
        {
            //arrange
            this.parser.parse(str);
            //act
            FieldInfo res = this.parser.typeDef();
            //assert
            Assert.Equal(fieldType, res.getType());
            Assert.Equal(txtLength, res.getTXTLength());
        }

        [Theory]
        [InlineData("ID INT", "ID", FieldType.INT, 0)]
        [InlineData("money FLOAT", "money", FieldType.FLOAT, 0)]
        [InlineData("gender CHAR", "gender", FieldType.CHAR, 0)]
        [InlineData("name VARCHAR (50)", "name", FieldType.VARCHAR, 50)]
        [InlineData("alive BOOLEAN", "alive", FieldType.BOOLEAN, 0)]
        [InlineData("young DIST_FUZZYSET_INT", "young", FieldType.distFS_INT, 0)]
        [InlineData("about22.2 DIST_FUZZYSET_FLOAT", "about22.2", FieldType.distFS_FLOAT, 0)]
        [InlineData("nothing DIST_FUZZYSET_TEXT", "nothing", FieldType.distFS_TEXT, 0)]
        [InlineData("hot CONT_FUZZYSET", "hot", FieldType.contFS, 0)]
        public void Parser_fieldDef_success(string str, string fieldName, FieldType type, int txtLength)
        {
            //arrange
            this.parser.parse(str);
            //act
            Field res = this.parser.fieldDef();
            //asert
            Assert.Equal(fieldName, res.getFieldName());
            Assert.Equal(type, res.getFieldInfo().getType());
            Assert.Equal(txtLength, res.getFieldInfo().getTXTLength());
        }

    }
}
