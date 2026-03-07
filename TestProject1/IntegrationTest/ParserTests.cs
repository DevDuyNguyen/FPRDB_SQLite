using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.IntegrationTest
{
    //not done: lack negative test
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

        [Theory]
        [InlineData("id, name, age", "id", "name", "age")]
        public void Parser_primaryAttributes_success(string str, string attr1,
            string attr2, string attr3)
        {
            //arrange
            this.parser.parse(str);
            //act
            List<string> primaryAttributeList = this.parser.primaryAttributes();
            //assert
            Assert.Equal(attr1, primaryAttributeList[0]);
            Assert.Equal(attr2, primaryAttributeList[1]);
            Assert.Equal(attr3, primaryAttributeList[2]);

        }
        [Theory]
        [InlineData("constraint pk_test primary KEY (attr1, attr2, attr3)",
            "pk_test", ConstraintType.IDENTITY, "attr1", "attr2", "attr3")]
        public void Parser_constraintDef_success(string str, string name,
            ConstraintType type, string attr1, string attr2, string attr3)
        {
            //arrange
            this.parser.parse(str);
            //act
            ConstraintData data = this.parser.constraintDef();
            List<string> fields = data.getFields();
            //assert
            Assert.Equal(name, data.getName());
            Assert.Equal(type, data.getType());
            Assert.Equal(attr1, fields[0]);
            Assert.Equal(attr2, fields[1]);
            Assert.Equal(attr3, fields[2]);
        }
        //not done: how to transfer test data into Theory more efficient
        //hints: Record in c#
        [Theory]
        [InlineData(
            @"create schema schema1 (
                id int,
                name varchar(50),
                income CONT_FUZZYSET,
                constraint pk_test primary key (id)
            )",
            "schema1",
            "id", FieldType.INT, 0,
            "name", FieldType.VARCHAR, 50,
            "income", FieldType.contFS, 0,
            new object[] { "id" }
        )]
        public void Parser_createSchema_success(string str,
            string name,
            string fieldName1, FieldType type1, int txtLength1,
            string fieldName2, FieldType type2, int txtLength2,
            string fieldName3, FieldType type3, int txtLength3,
            object[] primaryAttributeList)
        {
            //arrange
            this.parser.parse(str);
            //act
            FPRDBSchema data = this.parser.createSchema();
            List<Field> fields = data.getFields();
            List<string> primarykey = data.getPrimarykey();
            //assert
            Assert.Equal(name, data.getSchemaName());

            Assert.Equal(fieldName1, fields[0].getFieldName());
            Assert.Equal(type1, fields[0].getFieldInfo().getType());
            Assert.Equal(txtLength1, fields[0].getFieldInfo().getTXTLength());

            Assert.Equal(fieldName2, fields[1].getFieldName());
            Assert.Equal(type2, fields[1].getFieldInfo().getType());
            Assert.Equal(txtLength2, fields[1].getFieldInfo().getTXTLength());

            Assert.Equal(fieldName3, fields[2].getFieldName());
            Assert.Equal(type3, fields[2].getFieldInfo().getType());
            Assert.Equal(txtLength3, fields[2].getFieldInfo().getTXTLength());

            Assert.Equal(primaryAttributeList.Count(), primarykey.Count);
            for(int i=0; i< primaryAttributeList.Count(); ++i)
            {
                Assert.Equal(primaryAttributeList[i], primarykey[i]);
            }

        }

        [Theory]
        [InlineData("CREATE RELATION rel1 ON schema1", "rel1", "schema1")]
        public void Parser_createRelation_success(string str, string relName, string schemaName)
        {
            //arrange
            this.parser.parse(str);
            //act
            FPRDBRelation data = this.parser.createRelation();
            //assert
            Assert.Equal(relName, data.getRelName());
            Assert.Equal(schemaName, data.getSchemaName());

        }


    }
}
