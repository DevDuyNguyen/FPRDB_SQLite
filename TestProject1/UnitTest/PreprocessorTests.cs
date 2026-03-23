using BLL;
using BLL.Common;
using BLL.DomainObject;
using BLL.Enums;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestProject1.UnitTest
{
    public class PreprocessorTests
    {
        private CompositionRoot compRoot;
        private Preprocessor preprocessor;
        public PreprocessorTests()
        {
            this.compRoot = new CompositionRoot();
            this.compRoot.getDBMgr().loadDB("C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestSqlite\\db1.db");
            this.preprocessor = this.compRoot.getPreprocessor();
        }
        //not done: no systematic test case generation, weak foundation

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
        //[Theory]
        //[ClassData(typeof(checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression_positive_testdata))]
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
        //[Theory]
        //[ClassData(typeof(checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression_negative_testdata))]
        public void checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression_negative_test(List<FPRDBRelation> relations, List<string> attributesInSelect, List<string> attributesInSelectionCondition, Exception expected)
        {
            //arrange
            //act
            //assert
            SemanticException actual = Assert.Throws<SemanticException>(() => this.preprocessor.checkAttributeExistAndAmbiguityInSelectClauseAndAtomicExpression(relations, attributesInSelect, attributesInSelectionCondition));
            Assert.Equal(expected.Message, actual.Message);
        }
        //not done: mocking
        public class checkComparisonOperatorOnFieldConstant_positive_testdata:TheoryData<Field, Constant, CompareOperation, bool>
        {
            
            public checkComparisonOperatorOnFieldConstant_positive_testdata()
            {
                //not done: wrong equivalence class

                //Field is INT, constant is int, =
                Add(new Field("attr1", new FieldInfo(FieldType.INT, 0)), new IntConstant(12), CompareOperation.EQUAL, true);
                //Field is INT, constant is float, =
                Add(new Field("attr1", new FieldInfo(FieldType.INT, 0)), new FloatConstant(12), CompareOperation.EQUAL, true);
                //Field is float, constant is int, =
                Add(new Field("attr1", new FieldInfo(FieldType.FLOAT, 0)), new IntConstant(12), CompareOperation.EQUAL, true);
                //Field is float, constant is float, =
                Add(new Field("attr1", new FieldInfo(FieldType.FLOAT, 0)), new FloatConstant(12), CompareOperation.EQUAL, true);

                //Field is dist_INT, constant is float, =
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new FloatConstant(12.1f), CompareOperation.EQUAL, true);
                //Field is dist_INT, constant is int, =
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new IntConstant(12), CompareOperation.EQUAL, true);
                //Field is dist_FLOAT, constant is float, =
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_FLOAT, 0)), new FloatConstant(12.1f), CompareOperation.EQUAL, true);
                //Field is dist_FLOAT, constant is int, =
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_FLOAT, 0)), new IntConstant(12), CompareOperation.EQUAL, true);

                //Field is dist_FLOAT, constant is dist_INT fuzzy set, ->
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_FLOAT, 0)), new FuzzySetConstant("distFS1"), CompareOperation.ALSO, true);
                //Field is dist_FLOAT, constant is dist_INT fuzzy set, ->
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_FLOAT, 0)), new FuzzySetConstant("young"), CompareOperation.ALSO, true);
                //Field is dist_INT, constant is dist_INT fuzzy set, ->
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new FuzzySetConstant("distFS1"), CompareOperation.ALSO, true);
                //Field is dist_INT, constant is dist_INT fuzzy set, ->
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new FuzzySetConstant("young"), CompareOperation.ALSO, true);


                //Field is varchar, constant is string, =
                Add(new Field("attr1", new FieldInfo(FieldType.VARCHAR, 10)), new StringConstant("a"), CompareOperation.EQUAL, true);
                //Field is varchar, constant is string fuzzy set, =
                Add(new Field("attr1", new FieldInfo(FieldType.VARCHAR, 10)), new FuzzySetConstant("distFS2"), CompareOperation.EQUAL, true);

            }
        }
        //[Theory]
        //[ClassData(typeof(checkComparisonOperatorOnFieldConstant_positive_testdata))]
        public void checkComparisonOperatorOnFieldConstant_positive_test(Field field, Constant c, CompareOperation op, bool expected)
        {
            //arrange
            //act
            bool actual = this.preprocessor.checkComparisonOperatorOnFieldConstant(field, c, op, this.compRoot.getMetaDataManger());
            //assert
            Assert.Equal(expected, actual);

        }
        public class checkComparisonOperatorOnFieldConstant_negative_testdata : TheoryData<Field, Constant, CompareOperation, Exception>
        {

            public checkComparisonOperatorOnFieldConstant_negative_testdata()
            {
                //not done: wrong equivalence class

                //Field is INT, constant is string, =
                Add(new Field("attr1", new FieldInfo(FieldType.INT, 0)), new StringConstant("aa"), CompareOperation.EQUAL, new SemanticException($"attr1 {CompareOperation.EQUAL.ToString()} aa is invalid"));
                //Field is INT, constant is bool, =
                Add(new Field("attr1", new FieldInfo(FieldType.INT, 0)), new BooleanConstant(true), CompareOperation.EQUAL, new SemanticException($"attr1 {CompareOperation.EQUAL.ToString()} {true.ToString()} is invalid"));
                //Field is float, constant is string, =
                Add(new Field("attr1", new FieldInfo(FieldType.FLOAT, 0)), new StringConstant("aa"), CompareOperation.EQUAL, new SemanticException($"attr1 {CompareOperation.EQUAL.ToString()} aa is invalid"));
                //Field is float, constant is bool, =
                Add(new Field("attr1", new FieldInfo(FieldType.FLOAT, 0)), new BooleanConstant(true), CompareOperation.EQUAL, new SemanticException($"attr1 {CompareOperation.EQUAL.ToString()} {true.ToString()} is invalid"));

                //Field is dist_INT, constant is string, =
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new StringConstant("aa"), CompareOperation.EQUAL, new SemanticException($"attr1 {CompareOperation.EQUAL.ToString()} aa is invalid"));
                //Field is dist_INT, constant is bool, =
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new BooleanConstant(true), CompareOperation.EQUAL, new SemanticException($"attr1 {CompareOperation.EQUAL.ToString()} {true.ToString()} is invalid"));
                //Field is dist_FLOAT, constant is string, =
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_FLOAT, 0)), new StringConstant("aa"), CompareOperation.EQUAL, new SemanticException($"attr1 {CompareOperation.EQUAL.ToString()} aa is invalid"));
                //Field is dist_FLOAT, constant is bool, =
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_FLOAT, 0)), new BooleanConstant(true), CompareOperation.EQUAL, new SemanticException($"attr1 {CompareOperation.EQUAL.ToString()} {true.ToString()} is invalid"));

                //Field is dist_FLOAT, constant is dist_TEXT fuzzy set, ->
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_FLOAT, 0)), new FuzzySetConstant("distFS2"), CompareOperation.ALSO, new SemanticException($"attr1 {CompareOperation.ALSO.ToString()} distFS2 is invalid"));
                //Field is dist_INT, constant is dist_TEXT fuzzy set, ->
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new FuzzySetConstant("distFS2"), CompareOperation.ALSO, new SemanticException($"attr1 {CompareOperation.ALSO.ToString()} distFS2 is invalid"));

                //Fuzzy set doesn't exist
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new FuzzySetConstant("not exist"), CompareOperation.ALSO, new SemanticException($"Fuzzy set not exist doesn't exist"));
                

            }
        }
        //[Theory]
        //[ClassData(typeof(checkComparisonOperatorOnFieldConstant_negative_testdata))]
        public void checkComparisonOperatorOnFieldConstant_negative_test(Field field, Constant c, CompareOperation op, Exception expected)
        {
            //arrange
            //act
            //assert
            SemanticException actual = Assert.Throws<SemanticException>(() => this.preprocessor.checkComparisonOperatorOnFieldConstant(field, c, op, this.compRoot.getMetaDataManger()));
            Assert.Equal(expected.Message, actual.Message);

        }
        //not done: mocking
        public class checkCompatibleFieldEqualField_positive_testdata : TheoryData<Field, Field, bool>
        {

            public checkCompatibleFieldEqualField_positive_testdata()
            {
                //not done: wrong equivalence class

                Add(new Field("attr1", new FieldInfo(FieldType.INT, 0)), new Field("attr1", new FieldInfo(FieldType.INT, 0)), true);
                Add(new Field("attr1", new FieldInfo(FieldType.INT, 0)), new Field("attr1", new FieldInfo(FieldType.FLOAT, 0)), true);
                Add(new Field("attr1", new FieldInfo(FieldType.FLOAT, 0)), new Field("attr1", new FieldInfo(FieldType.INT, 0)), true);
                Add(new Field("attr1", new FieldInfo(FieldType.FLOAT, 0)), new Field("attr1", new FieldInfo(FieldType.FLOAT, 0)), true);

                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new Field("attr1", new FieldInfo(FieldType.INT, 0)), true);
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new Field("attr1", new FieldInfo(FieldType.FLOAT, 0)), true);
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_FLOAT, 0)), new Field("attr1", new FieldInfo(FieldType.INT, 0)), true);
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_FLOAT, 0)), new Field("attr1", new FieldInfo(FieldType.FLOAT, 0)), true);

                Add(new Field("attr1", new FieldInfo(FieldType.distFS_FLOAT, 0)), new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), true);
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), true);
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new Field("attr1", new FieldInfo(FieldType.contFS, 0)), true);


                Add(new Field("attr1", new FieldInfo(FieldType.VARCHAR, 10)), new Field("attr1", new FieldInfo(FieldType.distFS_TEXT, 10)), true);

            }
        }
        //[Theory]
        //[ClassData(typeof(checkCompatibleFieldEqualField_positive_testdata))]
        public void checkCompatibleFieldEqualField_positive_test(Field f1, Field f2, bool expected)
        {
            //arrange
            //act
            bool actual = this.preprocessor.checkCompatibleFieldEqualField(f1,f2);
            //assert
            Assert.Equal(expected, actual);

        }
        public class checkCompatibleFieldEqualField_negative_testdata : TheoryData<Field, Field, Exception>
        {

            public checkCompatibleFieldEqualField_negative_testdata()
            {
                //not done: wrong equivalence class

                Add(new Field("attr1", new FieldInfo(FieldType.INT, 0)), new Field("attr2", new FieldInfo(FieldType.VARCHAR, 0)), new SemanticException($"attr1 = attr2 is invalid"));
                Add(new Field("attr1", new FieldInfo(FieldType.INT, 0)), new Field("attr2", new FieldInfo(FieldType.BOOLEAN, 0)), new SemanticException($"attr1 = attr2 is invalid"));
                Add(new Field("attr1", new FieldInfo(FieldType.INT, 0)), new Field("attr2", new FieldInfo(FieldType.VARCHAR, 0)), new SemanticException($"attr1 = attr2 is invalid"));
                Add(new Field("attr1", new FieldInfo(FieldType.FLOAT, 0)), new Field("attr2", new FieldInfo(FieldType.VARCHAR, 0)), new SemanticException($"attr1 = attr2 is invalid"));
                Add(new Field("attr1", new FieldInfo(FieldType.FLOAT, 0)), new Field("attr2", new FieldInfo(FieldType.BOOLEAN, 0)), new SemanticException($"attr1 = attr2 is invalid"));
                
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new Field("attr2", new FieldInfo(FieldType.VARCHAR, 0)), new SemanticException($"attr1 = attr2 is invalid"));
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new Field("attr2", new FieldInfo(FieldType.BOOLEAN, 0)), new SemanticException($"attr1 = attr2 is invalid"));
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_FLOAT, 0)), new Field("attr2", new FieldInfo(FieldType.VARCHAR, 0)), new SemanticException($"attr1 = attr2 is invalid"));
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_FLOAT, 0)), new Field("attr2", new FieldInfo(FieldType.BOOLEAN, 0)), new SemanticException($"attr1 = attr2 is invalid"));
                
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_FLOAT, 0)), new Field("attr2", new FieldInfo(FieldType.distFS_TEXT, 0)), new SemanticException($"attr1 = attr2 is invalid"));
                Add(new Field("attr1", new FieldInfo(FieldType.distFS_INT, 0)), new Field("attr2", new FieldInfo(FieldType.distFS_TEXT, 0)), new SemanticException($"attr1 = attr2 is invalid"));

            }
        }
        //[Theory]
        //[ClassData(typeof(checkCompatibleFieldEqualField_negative_testdata))]
        public void checkCompatibleFieldEqualField_negative_test(Field f1, Field f2, Exception expected)
        {
            //arrange
            //act
            //assert
            SemanticException actual = Assert.Throws<SemanticException>(() => this.preprocessor.checkCompatibleFieldEqualField(f1, f2));
            Assert.Equal(expected.Message, actual.Message);

        }

        class checkCartesianProductCompatibility_positive_testdata:TheoryData<List<FPRDBSchema>, FPRDBSchema, bool>
        {
            public checkCartesianProductCompatibility_positive_testdata()
            {
                FPRDBSchema sch1 = new FPRDBSchema(
                    "sch1",
                    new List<Field> {
                        new Field("person_id", new FieldInfo(FieldType.INT,0)),
                        new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                FPRDBSchema sch2 = new FPRDBSchema(
                    "sch2",
                    new List<Field> {
                        new Field("dept_id", new FieldInfo(FieldType.INT,0)),
                        new Field("location", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                FPRDBSchema sch3 = new FPRDBSchema(
                    "sch3",
                    new List<Field> {
                        new Field("person_id", new FieldInfo(FieldType.INT,0)),
                        new Field("name", new FieldInfo(FieldType.VARCHAR, 50)),
                        new Field("dept_id", new FieldInfo(FieldType.INT,0)),
                        new Field("location", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                Add(new List<FPRDBSchema> { sch1, sch2 }, sch3, true);
            }
        }
        //[Theory]
        //[ClassData(typeof(checkCartesianProductCompatibility_positive_testdata))]
        public void checkCartesianProductCompatibility_positive(List<FPRDBSchema> schemas, FPRDBSchema expectedResSchema, bool expected)
        {
            //arrange
            FPRDBSchema actualResSchema;
            //act
            bool actual = this.preprocessor.checkCartesianProductCompatibility(schemas, out actualResSchema);
            //assert
            Assert.Equal(expected, actual);
            Assert.Equivalent(expectedResSchema.getFields(), actualResSchema.getFields());
        }
        class checkCartesianProductCompatibility_negative_testdata : TheoryData<List<FPRDBSchema>, Exception>
        {
            public checkCartesianProductCompatibility_negative_testdata()
            {
                FPRDBSchema sch1 = new FPRDBSchema(
                    "sch1",
                    new List<Field> {
                        new Field("id", new FieldInfo(FieldType.INT,0)),
                        new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                FPRDBSchema sch2 = new FPRDBSchema(
                    "sch2",
                    new List<Field> {
                        new Field("id", new FieldInfo(FieldType.INT,0)),
                        new Field("location", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                Add(new List<FPRDBSchema> { sch1, sch2 }, new SemanticException($"Not cartesian product compatible because of common field id"));
            }
        }
        //[Theory]
        //[ClassData(typeof(checkCartesianProductCompatibility_negative_testdata))]
        public void checkCartesianProductCompatibility_negative(List<FPRDBSchema> schemas, SemanticException expected)
        {
            //arrange
            FPRDBSchema actualResSchema;
            //act;
            //assert
            SemanticException actual = Assert.Throws<SemanticException>(() => this.preprocessor.checkCartesianProductCompatibility(schemas, out actualResSchema));
            Assert.Equal(expected.Message, actual.Message);
        }

        class checkNaturalJoinCompatibility_positive_testdata : TheoryData<List<FPRDBSchema>, FPRDBSchema, bool>
        {
            public checkNaturalJoinCompatibility_positive_testdata()
            {
                FPRDBSchema sch1 = new FPRDBSchema(
                    "sch1",
                    new List<Field> {
                        new Field("person_id", new FieldInfo(FieldType.INT,0)),
                        new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                FPRDBSchema sch2 = new FPRDBSchema(
                    "sch1",
                    new List<Field> {
                        new Field("person_id", new FieldInfo(FieldType.INT,0)),
                        new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                FPRDBSchema sch3 = new FPRDBSchema(
                    null,
                    new List<Field> {
                        new Field("person_id", new FieldInfo(FieldType.INT,0)),
                        new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                Add(new List<FPRDBSchema> { sch1, sch2 }, sch3, true);
            }
        }
        //[Theory]
        //[ClassData(typeof(checkCartesianProductCompatibility_positive_testdata))]
        public void checkNaturalJoinCompatibility_positive(List<FPRDBSchema> schemas, FPRDBSchema expectedResSchema, bool expected)
        {
            //arrange
            FPRDBSchema actualResSchema;
            //act
            bool actual = this.preprocessor.checkCartesianProductCompatibility(schemas, out actualResSchema);
            //assert
            Assert.Equal(expected, actual);
            Assert.Equivalent(expectedResSchema.getFields(), actualResSchema.getFields());
        }
        class checkNaturalJoinCompatibility_negative_testdata : TheoryData<List<FPRDBSchema>, SemanticException>
        {
            public checkNaturalJoinCompatibility_negative_testdata()
            {
                FPRDBSchema sch1 = new FPRDBSchema(
                    "sch1",
                    new List<Field> {
                        new Field("person_id", new FieldInfo(FieldType.INT,0)),
                        new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                FPRDBSchema sch2 = new FPRDBSchema(
                    "sch1",
                    new List<Field> {
                        new Field("person_id", new FieldInfo(FieldType.distFS_INT,0)),
                        new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                Add(new List<FPRDBSchema> { sch1, sch2 }, new SemanticException($"Common field person_id doesn't have same value domain"));
            }
        }
        //[Theory]
        //[ClassData(typeof(checkNaturalJoinCompatibility_negative_testdata))]
        public void checkNaturalJoinCompatibility_negative(List<FPRDBSchema> schemas, SemanticException expected)
        {
            //arrange
            FPRDBSchema actualResSchema;
            //act
            //assert
            SemanticException actual = Assert.Throws<SemanticException>(() => this.preprocessor.checkNaturalJoinCompatibility(schemas, out actualResSchema));
            Assert.Equal(expected.Message, actual.Message);
        }
        class checkSetOperationCompatibility_positive_testdata : TheoryData<FPRDBSchema, FPRDBSchema, bool>
        {
            public checkSetOperationCompatibility_positive_testdata()
            {
                FPRDBSchema sch1 = new FPRDBSchema(
                    "sch1",
                    new List<Field> {
                        new Field("person_id", new FieldInfo(FieldType.INT,0)),
                        new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                FPRDBSchema sch2 = new FPRDBSchema(
                    "sch1",
                    new List<Field> {
                        new Field("person_id", new FieldInfo(FieldType.INT,0)),
                        new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                Add(sch1, sch2, true);
            }
        }
        //[Theory]
        //[ClassData(typeof(checkSetOperationCompatibility_positive_testdata))]
        public void checkSetOperationCompatibility_positive(FPRDBSchema sch1, FPRDBSchema sch2, bool expected)
        {
            //arrange
            //act
            bool actual = this.preprocessor.checkSetOperationCompatibility(sch1, sch2);
            //assert
            Assert.Equal(expected, actual);
        }
        class checkSetOperationCompatibility_negative_testdata : TheoryData<FPRDBSchema, FPRDBSchema, SemanticException>
        {
            public checkSetOperationCompatibility_negative_testdata()
            {
                FPRDBSchema sch1 = new FPRDBSchema(
                    "sch1",
                    new List<Field> {
                        new Field("person_id", new FieldInfo(FieldType.INT,0)),
                        new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                FPRDBSchema sch2 = new FPRDBSchema(
                    "sch1",
                    new List<Field> {
                        new Field("person", new FieldInfo(FieldType.INT,0)),
                        new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                Add(sch1, sch2, new SemanticException($"Set opeation is incompatible because field person_id and field person"));
                 sch2 = new FPRDBSchema(
                    "sch1",
                    new List<Field> {
                        new Field("person_id", new FieldInfo(FieldType.VARCHAR,50)),
                        new Field("name", new FieldInfo(FieldType.VARCHAR, 50))
                        },
                    null
                    );
                Add(sch1, sch2, new SemanticException($"Set opeation is incompatible because field person_id and field person_id"));
            }
        }
        //[Theory]
        //[ClassData(typeof(checkSetOperationCompatibility_negative_testdata))]
        public void checkSetOperationCompatibility_negative(FPRDBSchema sch1, FPRDBSchema sch2, SemanticException expected)
        {
            //arrange
            //act
            //assert
            SemanticException actual = Assert.Throws<SemanticException>(() => this.preprocessor.checkSetOperationCompatibility(sch1, sch2));
            Assert.Equal(expected.Message, actual.Message);
        }

        class checkSemanticQuery_positive_testdata:TheoryData<QueryData, bool>
        {
            //not done: wrong equivalence class
            public checkSemanticQuery_positive_testdata()
            {
                CompositionRoot comp = new CompositionRoot();

                //BaseCartesianProductQueryData dL1 = new BaseCartesianProductQueryData(
                //    new List<SelectField> { new SelectField(null, "DOCTOR_ID"), new SelectField(null, "DOCTOR_NAME") },
                //    new List<string> { "DOCTOR1", "DOCTOR2" },
                //    new AtomicSelectionCondition(new AtomicSelectionExpressionFieldConstant("DOCTOR_ID", new IntConstant(1), CompareOperation.EQUAL,comp.getMetaDataManger()), 1,1)
                //    );
                //Add(dL1, true);

                BaseNaturalJoinQueryData dL1 = new BaseNaturalJoinQueryData(
                    new List<SelectField> { new SelectField(null, "DOCTOR_ID"), new SelectField(null, "DOCTOR_NAME") },
                    new NaturalJoinList(new List<string>{ "DOCTOR1", "DOCTOR2"}, null),
                    new AtomicSelectionCondition(new AtomicSelectionExpressionFieldConstant("DOCTOR_ID", new StringConstant("abc"), CompareOperation.EQUAL, comp.getMetaDataManger()), 1, 1)
                    );
                Add(dL1, true);

                CompoundQueryData dL2 = new CompoundQueryData(
                    new BaseCartesianProductQueryData(new List<SelectField> { new SelectField(null, "*")}, new List<string> { "DIAGNOSE1"}, null),
                    new BaseCartesianProductQueryData(new List<SelectField> { new SelectField(null, "*") }, new List<string> { "DIAGNOSE2" }, null),
                    SetConnective.INTERSECT,
                    ProbabilisticCombinationStrategy.CONJUNCTION_IGNORANCE
                    );
                Add(dL2, true);

            }
        }
        //[Theory]
        //[ClassData(typeof(checkSemanticQuery_positive_testdata))]
        public void checkSemanticQuery_positive(QueryData data, bool expected)
        {
            //arrange
            //act
            bool actual = this.preprocessor.checkSemanticQuery(data);
            //assert
            Assert.Equal(expected, actual);
        }
        class checkSemanticQuery_negative_testdata : TheoryData<QueryData, SemanticException>
        {
            //not done: wrong equivalence class
            public checkSemanticQuery_negative_testdata()
            {
                CompositionRoot comp = new CompositionRoot();


                BaseCartesianProductQueryData dL1 = new BaseCartesianProductQueryData(
                    new List<SelectField> { new SelectField(null, "DOCTOR_ID"), new SelectField(null, "DOCTOR_NAME") },
                    new List<string> { "DOCTOR1"},
                    null
                    );
                Add(dL1, new SemanticException($"field DOCTOR_NAME doesn't appears in any mentioned relation"));

                dL1 = new BaseCartesianProductQueryData(
                    new List<SelectField> { new SelectField(null, "DOCTOR_ID")},
                    new List<string> { "DOCTOR1" },
                    new AtomicSelectionCondition(new AtomicSelectionExpressionFieldConstant("DOCTOR_NAME", new StringConstant("abc"), CompareOperation.EQUAL, comp.getMetaDataManger()), 1, 1)
                    );
                Add(dL1, new SemanticException($"field DOCTOR_NAME doesn't appears in any mentioned relation"));

                dL1 = new BaseCartesianProductQueryData(
                    new List<SelectField> { new SelectField(null, "DOCTOR_ID"), new SelectField(null, "DOCTOR_NAME") },
                    new List<string> { "DOCTOR1", "DOCTOR2" },
                    new AtomicSelectionCondition(new AtomicSelectionExpressionFieldConstant("DOCTOR_ID", new StringConstant("abc"), CompareOperation.EQUAL, comp.getMetaDataManger()), 1, 1)
                    );
                Add(dL1, new SemanticException($"Not cartesian product compatible because of common field D_AGE"));

                dL1 = new BaseCartesianProductQueryData(
                    new List<SelectField> { new SelectField(null, "DOCTOR_ID")},
                    new List<string> { "DOCTOR1" },
                    new AtomicSelectionCondition(new AtomicSelectionExpressionFieldConstant("DOCTOR_ID", new IntConstant(1), CompareOperation.EQUAL, comp.getMetaDataManger()), 1, 1)
                    );
                Add(dL1, new SemanticException($"DOCTOR_ID EQUAL 1 is invalid"));

                dL1 = new BaseCartesianProductQueryData(
                    new List<SelectField> { new SelectField(null, "DOCTOR_ID")},
                    new List<string> { "DOCTOR1" },
                    new AtomicSelectionCondition(new AtomicSelectionExpressionFieldField("DOCTOR_ID", "D_AGE", ProbabilisticCombinationStrategy.CONJUNCTION_IGNORANCE), 1, 1)
                    );
                Add(dL1, new SemanticException($"DOCTOR_ID = D_AGE is invalid"));


                CompoundQueryData dL2 = new CompoundQueryData(
                    new BaseCartesianProductQueryData(new List<SelectField> { new SelectField(null, "*") }, new List<string> { "DIAGNOSE1" }, null),
                    new BaseCartesianProductQueryData(new List<SelectField> { new SelectField(null, "*") }, new List<string> { "DOCTOR1" }, null),
                    SetConnective.INTERSECT,
                    ProbabilisticCombinationStrategy.CONJUNCTION_IGNORANCE
                    );
                Add(dL2, new SemanticException($"Set opeation is incompatible because field P_ID and field DOCTOR_ID"));

            }
        }
        //[Theory]
        //[ClassData(typeof(checkSemanticQuery_negative_testdata))]
        public void checkSemanticQuery_negative(QueryData data, SemanticException expected)
        {
            //arrange
            //act;
            //assert
            SemanticException actual = Assert.Throws<SemanticException>(() => this.preprocessor.checkSemanticQuery(data));
            Assert.Equal(expected.Message, actual.Message);
        }

        class checkSemanticModify_positive_testdata : TheoryData<ModifyData, bool>
        {
            public checkSemanticModify_positive_testdata()
            {
                CompositionRoot compRoot = new CompositionRoot();
                FieldFuzzProbValueModifyData data1 = new FieldFuzzProbValueModifyData(
                    new FuzzyProbabilisticValueParsingData(
                        new List<Constant> { new StringConstant("Test101") },
                        new List<float> { 1 },
                        new List<float> { 1 }
                        ),
                    "DOCTOR1",
                    "DOCTOR_ID",
                    null
                    );
                Add(data1, true);

                data1 = new FieldFuzzProbValueModifyData(
                    new FuzzyProbabilisticValueParsingData(
                        new List<Constant> { new StringConstant("Test101") },
                        new List<float> { 1 },
                        new List<float> { 1 }
                        ),
                    "DOCTOR1",
                    "DOCTOR_ID",
                    new AtomicSelectionCondition(
                        new AtomicSelectionExpressionFieldConstant("DOCTOR_ID", new StringConstant("DT122"), CompareOperation.EQUAL, compRoot.getMetaDataManger()),
                        1,
                        1
                        )
                    );
                Add(data1, true);

                FieldFieldModifyData data2 = new FieldFieldModifyData(
                    "DOCTOR_ID",
                    "DOCTOR1",
                    "DOCTOR_ID",
                    new AtomicSelectionCondition(
                        new AtomicSelectionExpressionFieldConstant("DOCTOR_ID", new StringConstant("DT122"), CompareOperation.EQUAL, compRoot.getMetaDataManger()),
                        1,
                        1
                        )
                    );
                Add(data2, true);
            }
        }
        //[Theory]
        //[ClassData(typeof(checkSemanticModify_positive_testdata))]
        public void checkSemanticModify_success(ModifyData data, bool expected)
        {
            //arrange
            //act
            bool actual = this.preprocessor.checkSemanticModify(data);
            //assert
            Assert.Equal(expected, actual);
        }
        class checkSemanticModify_negative_testdata : TheoryData<ModifyData, SemanticException>
        {
            public checkSemanticModify_negative_testdata()
            {
                CompositionRoot compRoot = new CompositionRoot();
                FieldFuzzProbValueModifyData data1;
                FieldFieldModifyData data2;

                data1 = new FieldFuzzProbValueModifyData(
                    new FuzzyProbabilisticValueParsingData(
                        new List<Constant> { new StringConstant("Test101") },
                        new List<float> { 1 },
                        new List<float> { 1 }
                        ),
                    "nonExistintRelation",
                    "DOCTOR_ID",
                    null
                    );
                Add(data1, new SemanticException("Relation nonExistintRelation doesn't exist"));

                data1 = new FieldFuzzProbValueModifyData(
                    new FuzzyProbabilisticValueParsingData(
                        new List<Constant> { new StringConstant("Test101") },
                        new List<float> { 1 },
                        new List<float> { 1 }
                        ),
                    "DOCTOR1",
                    "nonExisitngInsertField",
                    null
                    );
                Add(data1, new SemanticException($"Field nonExisitngInsertField doesn't exist in relation DOCTOR1"));

                data2 = new FieldFieldModifyData(
                    "DOCTOR_ID",
                    "DOCTOR1",
                    "nonExistAssigningField",
                    new AtomicSelectionCondition(
                        new AtomicSelectionExpressionFieldConstant("DOCTOR_ID", new StringConstant("DT122"), CompareOperation.EQUAL, compRoot.getMetaDataManger()),
                        1,
                        1
                        )
                    );
                Add(data2, new SemanticException($"Field nonExistAssigningField doesn't exist in relation DOCTOR1"));

                data1 = new FieldFuzzProbValueModifyData(
                    new FuzzyProbabilisticValueParsingData(
                        new List<Constant> { new StringConstant("Test101") },
                        new List<float> { 1 },
                        new List<float> { 1 }
                        ),
                    "DOCTOR1",
                    "DOCTOR_ID",
                    new AtomicSelectionCondition(
                        new AtomicSelectionExpressionFieldConstant("nonExistingField", new StringConstant("DT122"), CompareOperation.EQUAL, compRoot.getMetaDataManger()),
                        1,
                        1
                        )
                    );
                Add(data1, new SemanticException("Field nonExistingField doesn't exist in relation DOCTOR1"));

                data2 = new FieldFieldModifyData(
                    "DOCTOR_ID",
                    "DOCTOR1",
                    "D_AGE",
                    new AtomicSelectionCondition(
                        new AtomicSelectionExpressionFieldConstant("DOCTOR_ID", new StringConstant("DT122"), CompareOperation.EQUAL, compRoot.getMetaDataManger()),
                        1,
                        1
                        )
                    );
                Add(data2, new SemanticException($"Can't assign the content of contFS field to VARCHAR field"));

                data1 = new FieldFuzzProbValueModifyData(
                    new FuzzyProbabilisticValueParsingData(
                        new List<Constant> { new IntConstant(12) },
                        new List<float> { 1 },
                        new List<float> { 1 }
                        ),
                    "DOCTOR1",
                    "DOCTOR_ID",
                    null
                    );
                Add(data1, new SemanticException("Inserted data and field type of DOCTOR_ID aren't compatible"));

                data1 = new FieldFuzzProbValueModifyData(
                    new FuzzyProbabilisticValueParsingData(
                        new List<Constant> { new FuzzySetConstant("distFS1") },
                        new List<float> { 1 },
                        new List<float> { 1 }
                        ),
                    "DOCTOR1",
                    "DOCTOR_ID",
                    null
                    );
                Add(data1, new SemanticException("Inserted data and field type of DOCTOR_ID aren't compatible"));

                data1 = new FieldFuzzProbValueModifyData(
                    new FuzzyProbabilisticValueParsingData(
                        new List<Constant> { new FuzzySetConstant("distFS1") },
                        new List<float> { 1 },
                        new List<float> { 1 }
                        ),
                    "DOCTOR1",
                    "D_AGE",
                    null
                    );
                Add(data1, new SemanticException("Inserted data and field type of D_AGE aren't compatible"));


            }
        }
        //[Theory]
        //[ClassData(typeof(checkSemanticModify_negative_testdata))]
        public void checkSemanticModify_negative(ModifyData data, SemanticException expected)
        {
            //arrange
            //act
            SemanticException actual = Assert.Throws<SemanticException>(()=> this.preprocessor.checkSemanticModify(data));
            //assert
            Assert.Equal(expected.Message, actual.Message);
        }
        //[Fact]
        public void checkSemanticModify_integrityConstraintViolation()
        {
            //arrange
            //integrity constraint violation
            string sql = "update DOCTOR1 set DOCTOR_ID={('DT093',[1,1])} where (DOCTOR_ID='DT005')[1,1]";
            SemanticException expected = new SemanticException($"Key value value already exist");

            //act
            SemanticException actual = Assert.Throws<SemanticException>(() =>
            {
                this.compRoot.getSQLProcessor().executeUpdate(sql);
            });
            //assert
            Assert.Equal(expected.Message, actual.Message);
        }


    }
}
