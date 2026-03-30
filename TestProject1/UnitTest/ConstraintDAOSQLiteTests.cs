using BLL;
using BLL.Common;
using BLL.DAO;
using BLL.DomainObject;
using BLL.DTO;
using BLL.Enums;
using BLL.Interfaces;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestProject1.UnitTest
{
    public class ConstraintDAOSQLiteTests
    {
        private ConstraintDAO dao;
        private CompositionRoot compRoot;
        private string dbPath = "C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestSqlite\\db1.db";
        private DatabaseManager dbMgr;
        public ConstraintDAOSQLiteTests()
        {
            //not done: Moq for mocking
            this.compRoot = new CompositionRoot();
            this.compRoot.getDBMgr().loadDB(this.dbPath);
            this.dao = new ConstraintDAOSQLite(compRoot.getDBMgr(), compRoot.getMetaDataManger(), compRoot.getParser());
            this.dbMgr = this.compRoot.getDBMgr();
        }
        public class createReferentialConstraint_positive_test_data:TheoryData<string, FPRDBRelationDTO, FPRDBRelationDTO, List<string>, List<string>, ConstraintDTO>
        {
            public createReferentialConstraint_positive_test_data()
            {
                FPRDBSchemaDTO baseSchema = new FPRDBSchemaDTO(
                    "rel",
                    new List<Field> { 
                        new Field("id", new FieldInfo(FieldType.INT, 0)),
                        new Field("age", new FieldInfo(FieldType.contFS, 0))
                    },
                    new List<string> { "id" },
                    11
                    );
                FPRDBRelationDTO rel3 = new FPRDBRelationDTO("rel3", baseSchema, "rel", 15);
                FPRDBRelationDTO rel4 = new FPRDBRelationDTO("rel4", baseSchema, "rel", 16);
                ConstraintDTO consDTO = new ConstraintDTO(-1, "fk_rel3_rel4", ConstraintType.REFERENTIAL, rel3, rel4, new List<string> { "id" }, new List<string> { "id" }, null);
                Add("fk_rel3_rel4", rel3, rel4, new List<string> { "id" }, new List<string> { "id" }, consDTO);
            }
        }
        //not done: not reproduceable
        //[Theory]
        //[ClassData(typeof(createReferentialConstraint_positive_test_data))]
        public void createReferentialConstraint_success(string conName, FPRDBRelationDTO fprdbRelation, FPRDBRelationDTO referencedFPRDBRelation, List<string> attributes, List<string> referencedAttributes, ConstraintDTO expected)
        {
            //arrange
            //act
            ConstraintDTO actual = this.dao.createReferentialConstraint(conName, fprdbRelation, referencedFPRDBRelation, attributes, referencedAttributes);
            //assert
            Assert.Equal(expected.conName, actual.conName);
            Assert.Equal(expected.conType, actual.conType);
            Assert.Equivalent(expected.relation, actual.relation);
            Assert.Equivalent(expected.referencedRelation, actual.referencedRelation);
            Assert.Equivalent(expected.attributes, actual.attributes);
            Assert.Equivalent(expected.referencedAttributes, actual.referencedAttributes);
            //cleaning
        }

        public class createReferentialConstraint_negative_test_data : TheoryData<string, FPRDBRelationDTO, FPRDBRelationDTO, List<string>, List<string>, Exception>
        {
            public createReferentialConstraint_negative_test_data()
            {
                FPRDBSchemaDTO baseSchema = new FPRDBSchemaDTO(
                    "rel",
                    new List<Field> {
                        new Field("id", new FieldInfo(FieldType.INT, 0)),
                        new Field("age", new FieldInfo(FieldType.contFS, 0))
                    },
                    new List<string> { "id" },
                    11
                    );
                FPRDBRelationDTO rel3 = new FPRDBRelationDTO("rel3", baseSchema, "rel", 15);
                FPRDBRelationDTO rel4 = new FPRDBRelationDTO("rel4", baseSchema, "rel", 16);
                FPRDBRelationDTO rel11 = new FPRDBRelationDTO("rel11", baseSchema, "rel", 17);
                //ConstraintDTO consDTO = new ConstraintDTO(-1, "fk_rel3_rel4", ConstraintType.REFERENTIAL, rel3, rel4, new List<string> { "ID" }, new List<string> { "ID" }, null);
                Add("fk_rel3_rel4", rel3, rel4, new List<string> { "ID" }, new List<string> { "ID" }, new InvalidOperationException($"Constraint name fk_rel3_rel4 already exists"));
                Add("fk_rel3_rel11", rel3, rel11, new List<string> { "age" }, new List<string> { "age" }, new InvalidOperationException($"Field age isn't a key attribute in relation {rel11.relName}"));
                Add("fk_rel3_rel11", rel3, rel11, new List<string> { "age" }, new List<string> { "id","id" }, new InvalidOperationException($"Invalid mapping from foreigh key of {rel3.relName} to primary key of {rel11.relName}"));
                Add("fk_rel3_rel11", rel3, rel11, new List<string> { "age" }, new List<string> { "id" }, new InvalidOperationException($"Field age can't map to field id"));


            }
        }
        //not done: not reproduceable
        //[Theory]
        //[ClassData(typeof(createReferentialConstraint_negative_test_data))]
        public void createReferentialConstraint_fail(string conName, FPRDBRelationDTO fprdbRelation, FPRDBRelationDTO referencedFPRDBRelation, List<string> attributes, List<string> referencedAttributes, Exception expected)
        {
            //arrange
            Exception actual=null;
            //act
            if (expected is InvalidOperationException)
                actual = Assert.Throws<InvalidOperationException>(() => this.dao.createReferentialConstraint(conName, fprdbRelation, referencedFPRDBRelation, attributes, referencedAttributes));
            //assert
            Assert.Equal(expected.GetType().Name, actual.GetType().Name);
            Assert.Equal(expected.Message, actual.Message);
            //cleaning
        }
        class getReferenrialConstraints_positive_testData:TheoryData<FPRDBRelationDTO, List<ConstraintDTO>>
        {
            public getReferenrialConstraints_positive_testData()
            {
                FPRDBSchemaDTO baseSchema = new FPRDBSchemaDTO(
                    "rel",
                    new List<Field> {
                        new Field("id", new FieldInfo(FieldType.INT, 0)),
                        new Field("id", new FieldInfo(FieldType.contFS, 0))
                    },
                    new List<string> { "id" },
                    11
                    );
                FPRDBRelationDTO rel3 = new FPRDBRelationDTO("rel3", baseSchema, "rel", 15);
                FPRDBRelationDTO rel4 = new FPRDBRelationDTO("rel4", baseSchema, "rel", 16);
                ConstraintDTO consDTO = new ConstraintDTO(-1, "fk_rel3_rel4", ConstraintType.REFERENTIAL, rel3, rel4, new List<string> { "id" }, new List<string> { "id" }, null);
                Add(rel3, new List<ConstraintDTO> { consDTO });
            }
        }
        //[Theory]
        //[ClassData(typeof(getReferenrialConstraints_positive_testData))]
        public void getReferenrialConstraints_success(FPRDBRelationDTO rel, List<ConstraintDTO> expected)
        {
            //arrange
            //act
            List<ConstraintDTO> actual = this.dao.getReferenrialConstraints(rel);
            //assert
            Assert.Equal(expected.Count, actual.Count);
            for(int i=0; i<expected.Count; ++i)
            {
                Assert.Equal(expected[i].conName, actual[i].conName);
                Assert.Equal(expected[i].conType, actual[i].conType);
                Assert.Equivalent(expected[i].relation, actual[i].relation);
                Assert.Equivalent(expected[i].referencedRelation, actual[i].referencedRelation);
                Assert.Equivalent(expected[i].attributes, actual[i].attributes);
                Assert.Equivalent(expected[i].referencedAttributes, actual[i].referencedAttributes);
            }
        }
        //[Theory]
        //[InlineData(32)]
        public void removeConstraint_success(int oid)
        {
            //arrange
            string sqlCheckIfConstraintExist = $"SELECT 1 FROM fprdb_Constraint WHERE oid={oid}";
            //act
            this.dao.removeConstraint(oid);
            //assert
            using(IDataReader r = this.dbMgr.executeQuery(sqlCheckIfConstraintExist))
            {
                Assert.Equal(false, r.Read());
            }
        }
        class checkIfInsertTupleViolateReferentialConstraint_testData:TheoryData<InsertData, bool>
        {
            public checkIfInsertTupleViolateReferentialConstraint_testData()
            {
                //success
                Add(
                    new InsertData(
                        "rel3",
                        new List<string> { "id", "age" },
                        new List<FuzzyProbabilisticValueParsingData> {
                            new FuzzyProbabilisticValueParsingData(
                                new List<Constant>{new IntConstant(1)},
                                new List<float>{1},
                                new List<float>{1}
                                ),
                            new FuzzyProbabilisticValueParsingData(
                                new List<Constant>{new IntConstant(22)},
                                new List<float>{1},
                                new List<float>{1}
                                )
                        }
                    ),
                    true
                );
                //a key attribute is null
                Add(
                    new InsertData(
                        "rel3",
                        new List<string> {"age" },
                        new List<FuzzyProbabilisticValueParsingData> {
                            new FuzzyProbabilisticValueParsingData(
                                new List<Constant>{new IntConstant(22)},
                                new List<float>{1},
                                new List<float>{1}
                                )
                        }
                    ),
                    true
                );
                //fail
                Add(
                    new InsertData(
                        "rel3",
                        new List<string> { "id", "age" },
                        new List<FuzzyProbabilisticValueParsingData> {
                            new FuzzyProbabilisticValueParsingData(
                                new List<Constant>{new IntConstant(111)},
                                new List<float>{1},
                                new List<float>{1}
                                ),
                            new FuzzyProbabilisticValueParsingData(
                                new List<Constant>{new IntConstant(22)},
                                new List<float>{1},
                                new List<float>{1}
                                )
                        }
                    ),
                    false
                );


            }
        }
        //[Theory]
        //[ClassData(typeof(checkIfInsertTupleViolateReferentialConstraint_testData))]
        public void checkIfInsertTupleViolateReferentialConstraint_test(InsertData data, bool expected)
        {
            ConstraintDTO constr=null;
            try
            {
                //arrange
                FPRDBSchemaDTO baseSchema = new FPRDBSchemaDTO(
                        "rel",
                        new List<Field> {
                        new Field("id", new FieldInfo(FieldType.INT, 0)),
                        new Field("id", new FieldInfo(FieldType.contFS, 0))
                        },
                        new List<string> { "id" },
                        11
                        );
                FPRDBRelationDTO rel3 = new FPRDBRelationDTO("rel3", baseSchema, "rel", 15);
                FPRDBRelationDTO rel4 = new FPRDBRelationDTO("rel4", baseSchema, "rel", 16);
                constr = this.compRoot.getConstraintDAO().createReferentialConstraint("fk_rel3_rel4", rel3, rel4, new List<string> { "id" }, new List<string> { "id" });

                //act
                bool actual = this.dao.checkIfInsertTupleViolateReferentialConstraint(data);
                //assert
                Assert.Equal(expected, actual);
            }
            finally
            {
                if(constr!=null)
                    this.compRoot.getConstraintDAO().removeConstraint(constr.oid);
            }

        }
        class checkIfDropRelationViolateReferentialConstraint_positive_testdata : TheoryData<DropRelationData, bool>
        {
            public checkIfDropRelationViolateReferentialConstraint_positive_testdata()
            {
                Add(new DropRelationData("rel12"), true);
            }
        }
        //[Theory]
        //[ClassData(typeof(checkIfDropRelationViolateReferentialConstraint_positive_testdata))]
        public void checkIfDropRelationViolateReferentialConstraint_success(DropRelationData data, bool expected)
        {
            //arrange
            //act
            bool actual = this.dao.checkIfDropRelationViolateReferentialConstraint(data);
            //assert
            Assert.Equal(expected, actual);

        }
        class checkIfDropRelationViolateReferentialConstraint_negative_testdata : TheoryData<DropRelationData, Exception>
        {
            public checkIfDropRelationViolateReferentialConstraint_negative_testdata()
            {
                Add(new DropRelationData("rel4"), new InvalidOperationException("Relation rel4 is referenced by rel3"));
            }
        }
        //[Theory]
        //[ClassData(typeof(checkIfDropRelationViolateReferentialConstraint_negative_testdata))]
        public void checkIfDropRelationViolateReferentialConstraint_fail(DropRelationData data, Exception expected)
        {
            ConstraintDTO constr = null;
            try
            {
                //arrange
                FPRDBSchemaDTO baseSchema = new FPRDBSchemaDTO(
                        "rel",
                        new List<Field> {
                        new Field("id", new FieldInfo(FieldType.INT, 0)),
                        new Field("id", new FieldInfo(FieldType.contFS, 0))
                        },
                        new List<string> { "id" },
                        11
                        );
                FPRDBRelationDTO rel3 = new FPRDBRelationDTO("rel3", baseSchema, "rel", 15);
                FPRDBRelationDTO rel4 = new FPRDBRelationDTO("rel4", baseSchema, "rel", 16);
                constr = this.compRoot.getConstraintDAO().createReferentialConstraint("fk_rel3_rel4", rel3, rel4, new List<string> { "id" }, new List<string> { "id" });

                //act
                Exception actual = Assert.Throws<InvalidOperationException>(() => this.dao.checkIfDropRelationViolateReferentialConstraint(data));
                //assert
                Assert.Equal(expected.GetType().Name, actual.GetType().Name);
                Assert.Equal(expected.Message, actual.Message);
            }
            finally
            {
                if (constr != null)
                    this.compRoot.getConstraintDAO().removeConstraint(constr.oid);
            }
        }
        class checkIfDeleteTupleViolateReferentialConstraint_positive_testdata : TheoryData<DeleteData, bool>
        {
            public checkIfDeleteTupleViolateReferentialConstraint_positive_testdata()
            {
                CompositionRoot compRoot = new CompositionRoot();
                Add(new DeleteData("rel3", null), true);
                Add(
                    new DeleteData(
                        "rel3",
                        new AtomicSelectionCondition(
                            new AtomicSelectionExpressionFieldConstant("id", new IntConstant(1), CompareOperation.EQUAL, compRoot.getMetaDataManger()),
                            1,
                            1
                        )
                    ),
                    true
                );
            }
        }
        //[Theory]
        //[ClassData(typeof(checkIfDeleteTupleViolateReferentialConstraint_positive_testdata))]
        public void checkIfDeleteTupleViolateReferentialConstraint_success(DeleteData data, bool expected)
        {
            //arrage
            //act
            bool actual = this.dao.checkIfDeleteTupleViolateReferentialConstraint(data);
            //assert
            Assert.Equal(expected, actual);
        }
        class checkIfDeleteTupleViolateReferentialConstraint_negative_testdata : TheoryData<DeleteData, Exception>
        {
            public checkIfDeleteTupleViolateReferentialConstraint_negative_testdata()
            {
                CompositionRoot compRoot = new CompositionRoot();
                Add(
                    new DeleteData(
                        "rel4",
                        new AtomicSelectionCondition(
                            new AtomicSelectionExpressionFieldConstant("id", new IntConstant(1), CompareOperation.EQUAL, compRoot.getMetaDataManger()),
                            1,
                            1
                        )
                    ),
                    new InvalidOperationException("Delete a tuple in rel4 will violate referential constraint fk_rel3_rel4")
                );
            }
        }
        //[Theory]
        //[ClassData(typeof(checkIfDeleteTupleViolateReferentialConstraint_negative_testdata))]
        public void checkIfDeleteTupleViolateReferentialConstraint_fail(DeleteData data, Exception expected)
        {
            ConstraintDTO constr = null;
            try
            {
                //arrange
                FPRDBSchemaDTO baseSchema = new FPRDBSchemaDTO(
                        "rel",
                        new List<Field> {
                        new Field("id", new FieldInfo(FieldType.INT, 0)),
                        new Field("id", new FieldInfo(FieldType.contFS, 0))
                        },
                        new List<string> { "id" },
                        11
                        );
                FPRDBRelationDTO rel3 = new FPRDBRelationDTO("rel3", baseSchema, "rel", 15);
                FPRDBRelationDTO rel4 = new FPRDBRelationDTO("rel4", baseSchema, "rel", 16);
                constr = this.compRoot.getConstraintDAO().createReferentialConstraint("fk_rel3_rel4", rel3, rel4, new List<string> { "id" }, new List<string> { "id" });

                //act
                Exception actual = Assert.Throws<InvalidOperationException>(() => this.dao.checkIfDeleteTupleViolateReferentialConstraint(data));
                //assert
                Assert.Equal(expected.GetType().Name, actual.GetType().Name);
                Assert.Equal(expected.Message, actual.Message);
            }
            finally
            {
                if (constr != null)
                    this.compRoot.getConstraintDAO().removeConstraint(constr.oid);
            }
        }
        class checkIfUpdatingTupleViolateReferentialConstraint_positive_testdata:TheoryData<ModifyData, bool>
        {
            public checkIfUpdatingTupleViolateReferentialConstraint_positive_testdata()
            {
                SelectionCondition condition1;
                AtomicSelectionCondition c1= new AtomicSelectionCondition(
                    new AtomicSelectionExpressionFieldConstant("id1", new IntConstant(1), CompareOperation.EQUAL, null),
                    1,
                    1
                    );
                AtomicSelectionCondition c2 = new AtomicSelectionCondition(
                    new AtomicSelectionExpressionFieldConstant("id2", new IntConstant(3), CompareOperation.EQUAL, null),
                    1,
                    1
                    );
                condition1 = new CompoundSelectionCondition(c1, c2, LogicalConnective.AND);
                AtomicSelectionCondition condition2 = new AtomicSelectionCondition(
                    new AtomicSelectionExpressionFieldConstant("id", new IntConstant(1), CompareOperation.EQUAL, null),
                    1,
                    1
                    );

                //violate no referential constraint to it, update field=fprobvalue, no update on primary key or foreigh key
                Add(
                    new FieldFuzzProbValueModifyData(
                        new FuzzyProbabilisticValueParsingData(
                            new List<Constant> { new IntConstant(11) },
                            new List<float> { 1 },
                            new List<float> { 1 }
                            ),
                        "referenced_rel",
                        "attr",
                        condition1
                        ),
                    true
                    );
                //violate no referential constraint to it, update field=field, no update on primary key or foreigh key
                Add(
                    new FieldFieldModifyData(
                        "attr",
                        "referenced_rel",
                        "attr",
                        condition1
                        ),
                    true
                    );

                //violate no referential constraint to it, update field=fprobvalue, update on primary key
                Add(
                    new FieldFuzzProbValueModifyData(
                        new FuzzyProbabilisticValueParsingData(
                            new List<Constant> { new IntConstant(11) },
                            new List<float> { 1 },
                            new List<float> { 1 }
                            ),
                        "referenced_rel",
                        "id1",
                        condition1
                        ),
                    true
                    );
                //violate no referential constraint to it, update field=field, update on primary key
                Add(
                    new FieldFieldModifyData(
                        "id1",
                        "referenced_rel",
                        "id2",
                        condition1
                        ),
                    true
                    );

                //violate no referential constraint on it, update field=fprobvalue, update on foreign key
                Add(
                    new FieldFuzzProbValueModifyData(
                        new FuzzyProbabilisticValueParsingData(
                            new List<Constant> { new IntConstant(2) },
                            new List<float> { 1 },
                            new List<float> { 1 }
                            ),
                        "referencing_rel",
                        "fk2",
                        condition2
                        ),
                    true
                    );
                //violate no referential constraint on it, update field=field, update on foreign key
                Add(
                    new FieldFieldModifyData(
                        "fk1",
                        "referencing_rel",
                        "fk1",
                        condition2
                        ),
                    true
                    );

            }
        }
        //[Theory]
        //[ClassData(typeof(checkIfUpdatingTupleViolateReferentialConstraint_positive_testdata))]
        public void checkIfUpdatingTupleViolateReferentialConstraint_success(ModifyData data, bool expected)
        {
            //arrange

            //SQLProcessor sqlProcessor = this.compRoot.getSQLProcessor();
            //sqlProcessor.executeDataDefinition("create schema referencing_sche (id INT, fk1 INT, fk2 INT, attr INT, CONSTRAINT pk_referencing_sche PRIMARY KEY (id))");
            //sqlProcessor.executeDataDefinition("create schema referenced_sche (id1 int, id2 INT, attr INT, CONSTRAINT pk_referenced_sche PRIMARY KEY (id1, id2))");
            //sqlProcessor.executeDataDefinition("create relation referencing_rel on referencing_sche");
            //sqlProcessor.executeDataDefinition("create relation referenced_rel on referenced_sche");
            //FPRDBSchemaDTO referencing_sche = new FPRDBSchemaDTO(
            //   "referencing_sche",
            //   new List<Field> {
            //        new Field("id", new FieldInfo(FieldType.INT, 0)),
            //        new Field("fk1", new FieldInfo(FieldType.INT, 0)),
            //        new Field("fk2", new FieldInfo(FieldType.INT, 0)),
            //        new Field("attr", new FieldInfo(FieldType.INT, 0))
            //   },
            //   new List<string> { "id" },
            //   14
            //   );
            //FPRDBSchemaDTO referenced_sche = new FPRDBSchemaDTO(
            //   "referenced_sche",
            //   new List<Field> {
            //        new Field("id1", new FieldInfo(FieldType.INT, 0)),
            //        new Field("id2", new FieldInfo(FieldType.INT, 0)),
            //        new Field("attr", new FieldInfo(FieldType.INT, 0))
            //   },
            //   new List<string> { "id1", "id2" },
            //   15
            //   );
            //FPRDBRelationDTO referencing_rel = new FPRDBRelationDTO("referencing_rel", referencing_sche, "rel", 21);
            //FPRDBRelationDTO referenced_rel = new FPRDBRelationDTO("referenced_rel", referenced_sche, "rel", 22);
            //this.dao.createReferentialConstraint("fk_referencing_rel_referenced_rel", referencing_rel, referenced_rel, new List<string> { "fk1", "fk2"}, new List<string>{ "id1", "id2"});
            //sqlProcessor.executeUpdate("insert into referenced_rel (id1, id2, attr) values ({(1,[1,1])},{(1,[1,1])},{(1,[1,1])})");
            //sqlProcessor.executeUpdate("insert into referenced_rel (id1, id2, attr) values ({(1,[1,1])},{(2,[1,1])},{(2,[1,1])})");
            //sqlProcessor.executeUpdate("insert into referenced_rel (id1, id2, attr) values ({(1,[1,1])},{(3,[1,1])},{(3,[1,1])})");

            //sqlProcessor.executeUpdate("insert into referencing_rel (id, fk1, fk2, attr) values ({(1,[1,1])},{(1,[1,1])},{(1,[1,1])},{(1,[1,1])})");
            //sqlProcessor.executeUpdate("insert into referencing_rel (id, fk1, fk2, attr) values ({(2,[1,1])},{(1,[1,1])},{(2,[1,1])},{(2,[1,1])})");

            //act
            bool actual = this.dao.checkIfUpdatingTupleViolateReferentialConstraint(data);
            //assert
            Assert.Equal(expected, actual);
          
        }
        class checkIfUpdatingTupleViolateReferentialConstraint_negative_testdata : TheoryData<ModifyData, Exception, Type>
        {
            public checkIfUpdatingTupleViolateReferentialConstraint_negative_testdata()
            {
                SelectionCondition condition1;
                AtomicSelectionCondition c1 = new AtomicSelectionCondition(
                    new AtomicSelectionExpressionFieldConstant("id1", new IntConstant(1), CompareOperation.EQUAL, null),
                    1,
                    1
                    );
                AtomicSelectionCondition c2 = new AtomicSelectionCondition(
                    new AtomicSelectionExpressionFieldConstant("id2", new IntConstant(1), CompareOperation.EQUAL, null),
                    1,
                    1
                    );
                condition1 = new CompoundSelectionCondition(c1, c2, LogicalConnective.AND);
                AtomicSelectionCondition condition2 = new AtomicSelectionCondition(
                    new AtomicSelectionExpressionFieldConstant("id", new IntConstant(2), CompareOperation.EQUAL, null),
                    1,
                    1
                    );

                //violate referential constraint to it, update field=fprobvalue, update on primary key
                Add(
                    new FieldFuzzProbValueModifyData(
                        new FuzzyProbabilisticValueParsingData(
                            new List<Constant> { new IntConstant(11) },
                            new List<float> { 1 },
                            new List<float> { 1 }
                            ),
                        "referenced_rel",
                        "id1",
                        condition1
                        ),
                    new InvalidOperationException($"Can't update key attribute of relation referenced_rel, because a tuple in relation referencing_rel is referencing to the updated tuple"),
                    typeof(InvalidOperationException)
                    );
                //violate referential constraint to it, update field=field, update on primary key
                Add(
                    new FieldFieldModifyData(
                        "id1",
                        "referenced_rel",
                        "attr",
                        condition1
                        ),
                    new InvalidOperationException($"Can't update key attribute of relation referenced_rel, because a tuple in relation referencing_rel is referencing to the updated tuple"),
                    typeof(InvalidOperationException)
                    );

                //violate referential constraint on it, update field=fprobvalue, update on foreign key
                Add(
                    new FieldFuzzProbValueModifyData(
                        new FuzzyProbabilisticValueParsingData(
                            new List<Constant> { new IntConstant(233) },
                            new List<float> { 1 },
                            new List<float> { 1 }
                            ),
                        "referencing_rel",
                        "fk2",
                        condition2
                        ),
                    new InvalidOperationException($"Can't update foreign key attribute in relaiton referencing_rel, because the updated tuple references to no tuple in relation referenced_rel"),
                    typeof(InvalidOperationException)
                    );
                //violate referential constraint on it, update field=field, update on foreign key
                Add(
                    new FieldFieldModifyData(
                        "fk1",
                        "referencing_rel",
                        "fk2",
                        condition2
                        ),
                    new InvalidOperationException($"Can't update foreign key attribute in relaiton referencing_rel, because the updated tuple references to no tuple in relation referenced_rel"),
                    typeof(InvalidOperationException)
                    );

            }
        }
        //[Theory]
        //[ClassData(typeof(checkIfUpdatingTupleViolateReferentialConstraint_negative_testdata))]
        public void checkIfUpdatingTupleViolateReferentialConstraint_fail(ModifyData data, Exception expected, Type execptedType)
        {
            //arrange
            //act
            Exception actual = Assert.Throws(execptedType, () => this.dao.checkIfUpdatingTupleViolateReferentialConstraint(data));
            //assert
            Assert.Equal(expected.Message, actual.Message);
        }


    }


}
