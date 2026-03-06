using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.SQLProcessing;
using DevExpress.Office.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.UnitTest
{
    public class ParserTests
    {
        private RecursiveDescentParser parser;
        public ParserTests()
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

        [Theory]
        [InlineData("ID INT, name VARCHAR (50), about22.2 DIST_FUZZYSET_FLOAT",
            "ID", FieldType.INT, 0,
            "name", FieldType.VARCHAR, 50,
            "about22.2", FieldType.distFS_FLOAT, 0)]
        public void Parser_fieldDefs_success(string str,
            string fieldName1, FieldType type1, int txtLength1,
            string fieldName2, FieldType type2, int txtLength2,
            string fieldName3, FieldType type3, int txtLength3)
        {
            //arrange
            this.parser.parse(str);
            //act
            List < Field > res= this.parser.fieldDefs();
            //assert
            Assert.Equal(fieldName1, res[0].getFieldName());
            Assert.Equal(type1, res[0].getFieldInfo().getType());
            Assert.Equal(txtLength1, res[0].getFieldInfo().getTXTLength());

            Assert.Equal(fieldName2, res[1].getFieldName());
            Assert.Equal(type2, res[1].getFieldInfo().getType());
            Assert.Equal(txtLength2, res[1].getFieldInfo().getTXTLength());

            Assert.Equal(fieldName3, res[2].getFieldName());
            Assert.Equal(type3, res[2].getFieldInfo().getType());
            Assert.Equal(txtLength3, res[2].getFieldInfo().getTXTLength());
        }

    }
}
