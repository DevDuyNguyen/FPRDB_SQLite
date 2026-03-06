using BLL.Common;
using BLL.DomainObject;
using BLL.DTO;
using BLL.Interfaces;
using BLL.Services;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using BLL.Exceptions;

namespace BLL
{
    public class Program
    {
        static string dbFile = "C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestSqlite\\db1.db";
        //not done: Moq for mocking
        static void test_createDiscreteFuzzySet()
        {
            

            CompositionRoot compRoot = new CompositionRoot();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            FuzzySetDAO fuzzySetDAO = compRoot.getFuzzySetDAO();

            dbMgr.loadDB(dbFile);
            DiscreteFuzzySetDTO<int> fuzzySet = new DiscreteFuzzySetDTO<int>(
                new List<int>() { 21, 22, 23 },
                new List<float>() { 0.5f, 1, 0.5f },
                "about_22",
                FieldType.INT
                );

            fuzzySetDAO.createDiscreteFuzzySet<int>(fuzzySet);
        }
        static void test_createContinuousFuzzySet()
        {
            CompositionRoot compRoot = new CompositionRoot();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            FuzzySetDAO fuzzySetDAO = compRoot.getFuzzySetDAO();

            dbMgr.loadDB(dbFile);
            ContinuousFuzzySetDTO fuzzySet = new ContinuousFuzzySetDTO(10,20,30,40,"random1" );

            fuzzySetDAO.createContinuousFuzzySet(fuzzySet);
        }
        static void test_createFuzzySet()
        {
            CompositionRoot compRoot = new CompositionRoot();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            FuzzySetDAO fuzzySetDAO = compRoot.getFuzzySetDAO();
            FuzzySetService service = compRoot.getFuzzySetService();

            dbMgr.loadDB(dbFile);
            DiscreteFuzzySetDTO<int> fuzzySet = new DiscreteFuzzySetDTO<int>(
                new List<int>() { 22, 23, 24 },
                new List<float>() { 0.5f, 1, 0.5f },
                "about_23",
                FieldType.INT
                );

            ContinuousFuzzySetDTO fuzzySet1 = new ContinuousFuzzySetDTO(10, 20, 30, 40, "random2");
            service.createFuzzySet<int>(fuzzySet);
            service.createFuzzySet<float>(fuzzySet1);
        }
        static void test_checkSemanticCreateSchema()
        {
            CompositionRoot compRoot = new CompositionRoot();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            dbMgr.loadDB(dbFile);
            Preprocessor preprocessor = compRoot.getPreprocessor();

            //positive test
            FPRDBSchema data1 = new FPRDBSchema("thisschemaneverexist", null, 
                new List<string>() { "attr1", "attr2", "attr3" }, 
                "pk_1");
            Debug.WriteLine($"Expected: true, Actual:{preprocessor.checkSemanticCreateSchema(data1)}");

            //negative test: name already belong to an existed schema
            FPRDBSchema data2 = new FPRDBSchema("EmployeeSchema", null,
                new List<string>() { "attr1", "attr2", "attr3" },
                "pk_1");
            try
            {
                Debug.WriteLine($"Expected: true,");
                preprocessor.checkSemanticCreateSchema(data2);
            }
            catch(SemanticException ex)
            {
                Debug.WriteLine($"Actual: exception: {ex.Message}");
            }

            //negative test: Schema creation must have primary key
            FPRDBSchema data3 = new FPRDBSchema("thisschemaneverexist", null,
                new List<string>() { "attr1", "attr2", "attr3" },
                "");
            try
            {
                Debug.WriteLine($"Expected: true,");
                preprocessor.checkSemanticCreateSchema(data3);
            }
            catch (SemanticException ex)
            {
                Debug.WriteLine($"Actual: exception: {ex.Message}");
            }
            //negative test: Schema creation must have primary key
            FPRDBSchema data4 = new FPRDBSchema("thisschemaneverexist", null,
                new List<string>(),
                "pk1");
            try
            {
                Debug.WriteLine($"Expected: true,");
                preprocessor.checkSemanticCreateSchema(data4);
            }
            catch (SemanticException ex)
            {
                Debug.WriteLine($"Actual: exception: {ex.Message}");
            }
            //negative test: Constraint name already exists
            FPRDBSchema data5 = new FPRDBSchema("thisschemaneverexist", null,
                new List<string>() { "attr1"},
                "PK_Products");
            try
            {
                Debug.WriteLine($"Expected: true,");
                preprocessor.checkSemanticCreateSchema(data5);
            }
            catch (SemanticException ex)
            {
                Debug.WriteLine($"Actual: exception: {ex.Message}");
            }

        }
        static void BasicUpdatePlanner_executeCreateSchema_success()
        {
            CompositionRoot root = new CompositionRoot();
            root.getDBMgr().loadDB(dbFile);
            UpdatePlanner updatePlanner = root.getUpdatePlanner();

            List<Field> fieldDefs = new List<Field>()
            {
                new Field("id", new FieldInfo(FieldType.INT, 0)),
                new Field("name", new FieldInfo(FieldType.VARCHAR, 50)),
                new Field("age", new FieldInfo(FieldType.INT, 0)),
            };

            FPRDBSchema data1 = new FPRDBSchema("schema15", fieldDefs,
                new List<string>() { "id", "name"},
                "pk_15");
            updatePlanner.executeCreateSchema(data1);

        }
        static void SQLProcessor_executeDataDefinition_success()
        {

            CompositionRoot root = new CompositionRoot();
            root.getDBMgr().loadDB(dbFile);
            SQLProcessor sqlProcessor = root.getSQLProcessor();
            string sql = @"CREATE SCHEMA student (
                student_id int, 
                name varchar(100), 
                age DIST_FUZZYSET_INT,
                CONSTRAINT pk_student primary key(student_id)
                )";
            sqlProcessor.executeDataDefinition(sql);
        }
        static void Main()
        {
            //CompositionRoot root = new CompositionRoot();
            //root.getDatabaseService().createDB("C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestSqlite\\db1.db");
            SQLProcessor_executeDataDefinition_success();
        }
    }
}
