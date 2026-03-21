using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Exceptions;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.UnitTest
{
    public class PreprocessorTests
    {
        private CompositionRoot compRoot;
        private Preprocessor preprocessor;
        public PreprocessorTests()
        {
            this.compRoot = new CompositionRoot();
            this.preprocessor = this.compRoot.getPreprocessor();
        }

        class checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression_positive_testdata : TheoryData<List<FPRDBRelation>, List<string>, List<string>, bool>
        {
            public checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression_positive_testdata()
            {
                FPRDBRelation rel1 = new FPRDBRelation(
                    "rel1",
                    new FPRDBSchema(
                        "sch1", 
                        new List<Field> {
                            new Field("person_id", new FieldInfo(FieldType.INT, 0)),
                            new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                            },
                        null
                        )
                    );
                FPRDBRelation rel2 = new FPRDBRelation(
                    "rel2",
                    new FPRDBSchema(
                        "sch2",
                        new List<Field> {
                            new Field("department_id", new FieldInfo(FieldType.INT, 0)),
                            new Field("location", new FieldInfo(FieldType.INT, 0))
                            },
                        null
                        )
                    );

                Add(new List<FPRDBRelation> { rel1, rel2 }, new List<string> { "person_id", "name" }, new List<string> { "location" }, true);

            }
        }
        [Theory]
        [ClassData(typeof(checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression_positive_testdata))]
        public void checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression_positive_test(List<FPRDBRelation> relations, List<string> attributesInSelect, List<string> attributesInSelectionCondition, bool expected)
        {
            //arrange
            //act
            bool actual = this.preprocessor.checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression(relations, attributesInSelect, attributesInSelectionCondition);
            //assert
            Assert.Equal(expected, actual);
        }

        class checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression_negative_testdata : TheoryData<List<FPRDBRelation>, List<string>, List<string>, Exception>
        {
            public checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression_negative_testdata()
            {
                FPRDBRelation rel1 = new FPRDBRelation(
                    "rel1",
                    new FPRDBSchema(
                        "sch1",
                        new List<Field> {
                            new Field("person_id", new FieldInfo(FieldType.INT, 0)),
                            new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                            },
                        null
                        )
                    );
                FPRDBRelation rel2 = new FPRDBRelation(
                    "rel2",
                    new FPRDBSchema(
                        "sch2",
                        new List<Field> {
                            new Field("department_id", new FieldInfo(FieldType.INT, 0)),
                            new Field("location", new FieldInfo(FieldType.INT, 0))
                            },
                        null
                        )
                    );
                //an attribute in select clause doesn't exist in any relation
                Add(new List<FPRDBRelation> { rel1, rel2 }, new List<string> { "not exist", "name" }, new List<string> { "location" }, new SemanticException($"field not exist doesn't appears in any mentioned relation"));
                //an attribute in selection condition clause doesn't exist in any relation
                Add(new List<FPRDBRelation> { rel1, rel2 }, new List<string> { "person_id", "name" }, new List<string> { "not exist" }, new SemanticException($"field not exist doesn't appears in any mentioned relation"));

                //attribute ammbiguity, a mentioned attribute exist in more than 1 relations
                rel1 = new FPRDBRelation(
                    "rel1",
                    new FPRDBSchema(
                        "sch1",
                        new List<Field> {
                            new Field("id", new FieldInfo(FieldType.INT, 0)),
                            new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                            },
                        null
                        )
                    );
                rel2 = new FPRDBRelation(
                    "rel2",
                    new FPRDBSchema(
                        "sch2",
                        new List<Field> {
                            new Field("id", new FieldInfo(FieldType.INT, 0)),
                            new Field("location", new FieldInfo(FieldType.INT, 0))
                            },
                        null
                        )
                    );
                Add(new List<FPRDBRelation> { rel1, rel2 }, new List<string> { "id", "name" }, new List<string> { "location" }, new SemanticException($"Ambiguity: field id appears in more than on relations"));

            }
        }
        [Theory]
        [ClassData(typeof(checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression_negative_testdata))]
        public void checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression_negative_test(List<FPRDBRelation> relations, List<string> attributesInSelect, List<string> attributesInSelectionCondition, Exception expected)
        {
            //arrange
            //act
            //assert
            SemanticException actual = Assert.Throws<SemanticException>(() => this.preprocessor.checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression(relations, attributesInSelect, attributesInSelectionCondition));
            Assert.Equal(expected.Message, actual.Message);

        }


    }
}
