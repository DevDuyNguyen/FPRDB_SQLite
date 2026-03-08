using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Interfaces;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        [Theory]
        [InlineData("1", typeof(IntConstant), 1)]
        [InlineData("1.1", typeof(FloatConstant), 1.1)]
        [InlineData("\'haha\'", typeof(StringConstant), "haha")]
        [InlineData("\"haha\"", typeof(StringConstant), "haha")]
        [InlineData("TRUE", typeof(BooleanConstant), true)]
        [InlineData("True", typeof(BooleanConstant), true)]
        [InlineData("true", typeof(BooleanConstant), true)]
        [InlineData("FALSE", typeof(BooleanConstant), false)]
        [InlineData("False", typeof(BooleanConstant), false)]
        [InlineData("false", typeof(BooleanConstant), false)]
        [InlineData("young", typeof(FuzzySetConstant), "young")]

        public void Parser_constant_success(string str, Type type, object value)
        {
            //arrange
            this.parser.parse(str);
            //act
            Constant constant = this.parser.constant();
            //assert
            Assert.Equal(type, constant.GetType());
            if(value is float)
            {
                Assert.Equal(Convert.ToSingle(value), Convert.ToSingle(constant.getVal()), 10);
            }
            else
                Assert.Equivalent(value, constant.getVal());
        }
        [Theory]
        [InlineData("attr1,attr2,attr3","attr1","attr2","attr3")]
        public void Parser_fieldList_success(string str, string exp1, string exp2, string exp3)
        {
            //arrange
            this.parser.parse(str);
            //act
            List<string> ans = this.parser.fieldList();
            //assert
            Assert.Equal(exp1, ans[0]);
            Assert.Equal(exp2, ans[1]);
            Assert.Equal(exp3, ans[2]);
        }
        //[not done: how to carry object as test data into theory]
        class possibleValueTestData : TheoryData<string, PossibleValue>
        {
            public possibleValueTestData()
            {
                Add("(1,[0.5,0.5])", new PossibleValue(new IntConstant(1), 0.5f, 0.5f));
                Add("(1.1,[0.5,0.5])", new PossibleValue(new FloatConstant(1.1f), 0.5f, 0.5f));
                Add("(TRUE,[0.5,0.5])", new PossibleValue(new BooleanConstant(true), 0.5f, 0.5f));
                Add("(True,[0.5,0.5])", new PossibleValue(new BooleanConstant(true), 0.5f, 0.5f));
                Add("(true,[0.5,0.5])", new PossibleValue(new BooleanConstant(true), 0.5f, 0.5f));
                Add("(FALSE,[0.5,0.5])", new PossibleValue(new BooleanConstant(false), 0.5f, 0.5f));
                Add("(False,[0.5,0.5])", new PossibleValue(new BooleanConstant(false), 0.5f, 0.5f));
                Add("(false,[0.5,0.5])", new PossibleValue(new BooleanConstant(false), 0.5f, 0.5f));
                Add("(\'haha\',[0.5,0.5])", new PossibleValue(new StringConstant("haha"), 0.5f, 0.5f));
                Add("(\"haha\",[0.5,0.5])", new PossibleValue(new StringConstant("haha"), 0.5f, 0.5f));
                Add("(young,[0.5,0.5])", new PossibleValue(new FuzzySetConstant("young"), 0.5f, 0.5f));
            }
        }
        [Theory]
        [ClassData(typeof(possibleValueTestData))]
        public void Parser_possibleValue_succes(string str, PossibleValue expected)
        {
            //arrange
            this.parser.parse(str);
            //act
            PossibleValue ans = this.parser.possibleValue();
            //assert
            Assert.Equal(expected.constant.GetType(), ans.constant.GetType());
            if(expected.constant is FloatConstant)
            {
                Assert.Equal((float)expected.constant.getVal(), (float)ans.constant.getVal(), 10);
            }
            else
                Assert.Equal(expected.constant.getVal(), ans.constant.getVal());

            Assert.Equal(expected.lowerBound, ans.lowerBound);
            Assert.Equal(expected.upperBound, ans.upperBound);
        }
        //not done:negative test for fuzzyProbabilisticValue
        class fuzzyProbabilisticValue_negative_TestData :TheoryData<string, FuzzyProbabilisticValueParsingData>
        {
            public fuzzyProbabilisticValue_negative_TestData()
            {
                List<Constant> consList1 = new List<Constant> {
                    new IntConstant(1),
                    new FloatConstant(1.1f),
                    new FuzzySetConstant("young")
                };
                Add("{(1,[0.5, 0.5]),(1.1,[0.5, 0.5]),(young,[0.5, 0.5])}"
                    , new FuzzyProbabilisticValueParsingData(
                            consList1,
                            new List<float> {0.5f, 0.5f, 0.5f },
                            new List<float> { 0.5f, 0.5f, 0.5f }
                        )
                );

                consList1 = new List<Constant> {
                    new BooleanConstant(true),
                    new BooleanConstant(true),
                    new BooleanConstant(true)
                };
                Add("{(TRUE,[0.5,0.5]),(True,[0.5,0.5]),(true,[0.5,0.5])}"
                    , new FuzzyProbabilisticValueParsingData(
                            consList1,
                            new List<float> { 0.5f, 0.5f, 0.5f },
                            new List<float> { 0.5f, 0.5f, 0.5f }
                        )
                );

                consList1 = new List<Constant> {
                    new BooleanConstant(false),
                    new BooleanConstant(false),
                    new BooleanConstant(false)
                };
                Add("{(FALSE,[0.5,0.5]),(False,[0.5,0.5]),(false,[0.5,0.5])}"
                    , new FuzzyProbabilisticValueParsingData(
                            consList1,
                            new List<float> { 0.5f, 0.5f, 0.5f },
                            new List<float> { 0.5f, 0.5f, 0.5f }
                        )
                );

                consList1 = new List<Constant> {
                    new StringConstant("'haha'"),
                    new StringConstant("\"haha\"")
                };
                Add("{('haha',[0.5,0.5]),(\"haha\",[0.5,0.5])}"
                    , new FuzzyProbabilisticValueParsingData(
                            consList1,
                            new List<float> { 0.5f, 0.5f, 0.5f },
                            new List<float> { 0.5f, 0.5f, 0.5f }
                        )
                );

            }
        }
        class fuzzyProbabilisticValue_possive_TestData : TheoryData<string, FuzzyProbabilisticValueParsingData>
        {
            public fuzzyProbabilisticValue_possive_TestData()
            {
                List<Constant> consList1;
                consList1 = new List<Constant> {
                    new IntConstant(1),
                    new IntConstant(2),
                    new IntConstant(3)
                };
                Add("{(1,[0.5, 0.5]),(2,[0.5, 0.5]),(3,[0.5, 0.5])}"
                    , new FuzzyProbabilisticValueParsingData(
                            consList1,
                            new List<float> { 0.5f, 0.5f, 0.5f },
                            new List<float> { 0.5f, 0.5f, 0.5f }
                        )
                );

                consList1 = new List<Constant> {
                    new BooleanConstant(true),
                    new BooleanConstant(true),
                    new BooleanConstant(true)
                };
                Add("{(TRUE,[0.5,0.5]),(True,[0.5,0.5]),(true,[0.5,0.5])}"
                    , new FuzzyProbabilisticValueParsingData(
                            consList1,
                            new List<float> { 0.5f, 0.5f, 0.5f },
                            new List<float> { 0.5f, 0.5f, 0.5f }
                        )
                );

                consList1 = new List<Constant> {
                    new BooleanConstant(false),
                    new BooleanConstant(false),
                    new BooleanConstant(false)
                };
                Add("{(FALSE,[0.5,0.5]),(False,[0.5,0.5]),(false,[0.5,0.5])}"
                    , new FuzzyProbabilisticValueParsingData(
                            consList1,
                            new List<float> { 0.5f, 0.5f, 0.5f },
                            new List<float> { 0.5f, 0.5f, 0.5f }
                        )
                );

                consList1 = new List<Constant> {
                    new StringConstant("haha"),
                    new StringConstant("haha")
                };
                Add("{('haha',[0.5,0.5]),(\"haha\",[0.5,0.5])}"
                    , new FuzzyProbabilisticValueParsingData(
                            consList1,
                            new List<float> { 0.5f, 0.5f},
                            new List<float> { 0.5f, 0.5f}
                        )
                );

            }
        }
        [Theory]
        [ClassData(typeof(fuzzyProbabilisticValue_possive_TestData))]
        public void Parser_fuzzyProbabilisticValue_success(string str
            ,FuzzyProbabilisticValueParsingData expected)
        {
            //arrange
            this.parser.parse(str);
            //act
            FuzzyProbabilisticValueParsingData ans = this.parser.fuzzyProbabilisticValue();
            //assert
            Assert.Equal(expected.valueList[0].GetType(), ans.valueList[0].GetType());
            for(int i=0; i<expected.valueList.Count; ++i){
                if (expected.valueList[i] is FloatConstant)
                {
                    Assert.Equal((float)expected.valueList[i].getVal(), (float)ans.valueList[i].getVal(), 10);
                }
                else
                    Assert.Equal(expected.valueList[i].getVal(), ans.valueList[i].getVal());
            }
            for (int i = 0; i < expected.intervalProbLowerBoundList.Count; ++i)
            {
                Assert.Equal(expected.intervalProbLowerBoundList[i], ans.intervalProbLowerBoundList[i],10);
                Assert.Equal(expected.intervalProbUpperBoundList[i], ans.intervalProbUpperBoundList[i], 10);
            }
        }
        //not done: no test for fuzzyProbabilisticValueList
        class insert_positive_testdata :TheoryData<string, InsertData>
        {
            public insert_positive_testdata()
            {
                InsertData data;
                List<Constant> consList1;
                FuzzyProbabilisticValueParsingData fuzzyProbabilisticValue;
                List<FuzzyProbabilisticValueParsingData> fuzzyProbabilisticValues=new List<FuzzyProbabilisticValueParsingData>();

                consList1 = new List<Constant> {
                    new IntConstant(1)
                };
                fuzzyProbabilisticValue = new FuzzyProbabilisticValueParsingData(
                    consList1,
                    new List<float> { 1},
                    new List<float> { 1}
                );
                fuzzyProbabilisticValues.Add(fuzzyProbabilisticValue);

                consList1 = new List<Constant> {
                    new StringConstant("duy")
                };
                fuzzyProbabilisticValue = new FuzzyProbabilisticValueParsingData(
                    consList1,
                    new List<float> { 1},
                    new List<float> { 1}
                );
                fuzzyProbabilisticValues.Add(fuzzyProbabilisticValue);

                consList1 = new List<Constant> {
                    new FloatConstant(22.2f),
                    new FuzzySetConstant("about30")
                };
                fuzzyProbabilisticValue = new FuzzyProbabilisticValueParsingData(
                    consList1,
                    new List<float> { 0.5f, 0.5f },
                    new List<float> { 0.5f, 0.5f }
                );
                fuzzyProbabilisticValues.Add(fuzzyProbabilisticValue);

                data = new InsertData("rel1",
                    new List<string> { "id", "name", "money" },
                    fuzzyProbabilisticValues
                );
                Add(@"
                    INSERT INTO rel1(id, name, money)
                    VALUES ( {(1,[1,1])}, {('duy',[1,1])}, {(22.2,[0.5,0.5]), (about30,[0.5,0.5])} )"
                , data);
            }
        }
        [Theory]
        [ClassData(typeof(insert_positive_testdata))]
        public void Parser_insert_success(string str, InsertData expected)
        {
            //arrange
            this.parser.parse(str);
            //act
            InsertData ans = this.parser.insert();
            //assert
            Assert.Equal(expected.relation, ans.relation);
            for(int i=0; i<expected.fieldList.Count; ++i)
            {
                Assert.Equal(expected.fieldList[i], ans.fieldList[i]);
            }
            for (int j = 0; j < expected.fuzzyProbabilisticValues.Count; ++j)
            {
                FuzzyProbabilisticValueParsingData expected1 = expected.fuzzyProbabilisticValues[j];
                FuzzyProbabilisticValueParsingData ans1 = ans.fuzzyProbabilisticValues[j];
                Assert.Equal(expected1.valueList[0].GetType(), ans1.valueList[0].GetType());
                for (int i = 0; i < expected1.valueList.Count; ++i)
                {
                    if (expected1.valueList[i] is FloatConstant)
                    {
                        Assert.Equal((float)expected1.valueList[i].getVal(), (float)ans1.valueList[i].getVal(), 10);
                    }
                    else
                        Assert.Equal(expected1.valueList[i].getVal(), ans1.valueList[i].getVal());
                }
                for (int i = 0; i < expected1.intervalProbLowerBoundList.Count; ++i)
                {
                    Assert.Equal(expected1.intervalProbLowerBoundList[i], ans1.intervalProbLowerBoundList[i], 10);
                    Assert.Equal(expected1.intervalProbUpperBoundList[i], ans1.intervalProbUpperBoundList[i], 10);
                }
            }
        }
        //[not done]: not enough coverage
        class selectList_positive_testdata : TheoryData<string, List<SelectField>>
        {
            public selectList_positive_testdata()
            {
                Add("*", new List<SelectField> { new SelectField("", "*") });
                Add("field1,field2,field3", new List<SelectField> { 
                    new SelectField("", "field1"),
                    new SelectField("", "field2"),
                    new SelectField("", "field3"),
                });
                Add("rel1.field1,rel2.field2,field3", new List<SelectField> {
                    new SelectField("rel1", "field1"),
                    new SelectField("rel2", "field2"),
                    new SelectField("", "field3"),
                });
            }
        }
        [Theory]
        [ClassData(typeof(selectList_positive_testdata))]
        public void Parser_selectList_success(string sql, List<SelectField> expected)
        {
            //arrange
            CompositionRoot compRoot = new CompositionRoot();
            RecursiveDescentParser parser = compRoot.getParser();
            parser.parse(sql);
            //act
            List<SelectField> actual = parser.selectList();
            //assert
            for(int i=0; i < expected.Count; ++i)
            {
                Assert.Equal(expected[i].relation, actual[i].relation);
                Assert.Equal(expected[i].field, actual[i].field);
            }

        }


    }
}
