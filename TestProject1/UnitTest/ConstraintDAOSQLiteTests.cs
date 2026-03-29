using BLL;
using BLL.Common;
using BLL.DAO;
using BLL.DomainObject;
using BLL.DTO;
using BLL.Enums;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            this.dao = new ConstraintDAOSQLite(compRoot.getDBMgr(), compRoot.getMetaDataManger());
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
                        new Field("id", new FieldInfo(FieldType.contFS, 0))
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

    }


}
