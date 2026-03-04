
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Data.Common;
using System.Windows.Input;

namespace BLL
{
    public class DatabaseManager
    {
        private string connectionString;
        private IDbConnection connection;

        //not done: Moq for mocking
        public void setConnectionString(string str)
        {
            this.connectionString = str;
        }
        public void createDB(string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
                throw new FileNotFoundException();
            if (File.Exists(filePath))
                throw new IOException("File already exists");
            this.connectionString = $"Data Source={filePath};Version=3;";
            this.connection = new SQLiteConnection(this.connectionString);
            try
            {
                this.connection.Open();
                this.connection.Close();
                createSystemCatalog();
                insertTestData();
            }
            catch (Exception e)
            {
                throw new IOException("Unable to create database");
            }
            finally
            {
                this.connection.Close();
            }
        }
        public void createSystemCatalog()
        {
            string create_fprdb_RelationSchema = "CREATE TABLE fprdb_RelationSchema(" +
                    "oid INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "relschema_name TEXT NOT NULL" +
                    ");";
            string create_fprdb_Relation = "CREATE TABLE fprdb_Relation(" +
                "oid INTEGER PRIMARY KEY AUTOINCREMENT," +
                "rel_name TEXT NOT NULL," +
                "rel_relation_schema INTEGER NOT NULL," +
                "FOREIGN KEY (rel_relation_schema) REFERENCES fprdb_RelationSchema (oid)" +
                ");";
            string create_fprdb_Type = "CREATE TABLE fprdb_Type(" +
                "oid INTEGER PRIMARY KEY AUTOINCREMENT," +
                "type_name TEXT NOT NULL," +
                "type_type TEXT NOT NULL" +
                ");";
            string create_fprdb_Attribute = "CREATE TABLE fprdb_Attribute(" +
                "att_relschema_id INTEGER," +
                "att_number INTEGER," +
                "att_name TEXT NOT NULL," +
                "att_type_id INTEGER NOT NULL," +
                "att_type_mod INTEGER," +
                "att_not_null BOOLEAN," +
                "PRIMARY KEY(att_relschema_id, att_number)," +
                "FOREIGN KEY (att_relschema_id) REFERENCES fprdb_RelationSchema (oid)," +
                "FOREIGN KEY (att_type_id) REFERENCES fprdb_Type (oid)" +
                ");";
            string create_fprdb_FuzzySet = "CREATE TABLE fprdb_FuzzySet(" +
                "oid INTEGER PRIMARY KEY AUTOINCREMENT," +
                "fuzzset_name TEXT NOT NULL," +
                "fuzzset_type_id INTEGER NOT NULL," +
                "FOREIGN KEY (fuzzset_type_id) REFERENCES fprdb_Type (oid)" +
                ");";
            string create_fprdb_DiscreteFuzzySet = "CREATE TABLE fprdb_DiscreteFuzzySet(" +
                "oid INTEGER PRIMARY KEY AUTOINCREMENT," +
                "fuzzset_x TEXT NOT NULL," +
                "fuzzset_membership_degree TEXT NOT NULL," +
                "FOREIGN KEY (oid) REFERENCES fprdb_FuzzySet (oid)" +
                ");";
            string create_fprdb_ContinousFuzzySet = "CREATE TABLE fprdb_ContinousFuzzySet(" +
                "oid INTEGER PRIMARY KEY AUTOINCREMENT," +
                "fuzzset_bottom_left REAL NOT NULL," +
                "fuzzset_top_left REAL NOT NULL," +
                "fuzzset_top_right REAL NOT NULL," +
                "fuzzset_bottom_right REAL NOT NULL," +
                "FOREIGN KEY (oid) REFERENCES fprdb_FuzzySet (oid)" +
                ");";
            string create_fprdb_Relation_Fuzzyset = "CREATE TABLE FPRDB_Rel_FuzzSet (" +
                "rel_oid INTEGER," +
                "fuzzset_oid INTEGER," +
                "no INTEGER NOT NULL," +
                "PRIMARY KEY (rel_oid, fuzzset_oid)," +
                "FOREIGN KEY (rel_oid) REFERENCES fprdb_Relation (oid)," +
                "FOREIGN KEY (fuzzset_oid) REFERENCES fprdb_FuzzySet (oid)" +
                ");";
            string create_fprdb_Constraint = "CREATE TABLE fprdb_Constraint(" +
                "oid INTEGER PRIMARY KEY AUTOINCREMENT," +
                "con_name TEXT NOT NULL," +
                "con_type TEXT NOT NULL," +
                "con_relation_id INTEGER," +
                "con_referenced_relation_id INTEGER," +
                "con_attributes TEXT NOT NULL," +
                "con_referenced_attributes TEXT," +
                "con_relschema_id INTEGER," +
                "FOREIGN KEY (con_relation_id) REFERENCES fprdb_Relation (oid)," +
                "FOREIGN KEY (con_referenced_relation_id) REFERENCES fprdb_Relation (oid)," +
                "FOREIGN KEY (con_relschema_id) REFERENCES fprdb_RelationSchema (oid)" +
                ");";

            string statemt = create_fprdb_RelationSchema + create_fprdb_Relation + create_fprdb_Type +
                create_fprdb_Attribute + create_fprdb_FuzzySet + create_fprdb_DiscreteFuzzySet
                + create_fprdb_ContinousFuzzySet + create_fprdb_Relation_Fuzzyset + create_fprdb_Constraint;

            try
            {
                this.connection.Open();
                IDbCommand command = new SQLiteCommand(statemt, (SQLiteConnection)this.connection);
                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                throw ex;
            }
            finally
            {
                this.connection.Close();
            }
        }
        public string getConnectionString()
        {
            return this.connectionString;
        }
        public void closeConnection()
        {
            if (this.connection != null)
                this.connection.Close();
        }
        public IDataReader executeQuery(string sql)
        {
            //not done: Moq for mocking
            IDbCommand command = new SQLiteCommand(sql, (SQLiteConnection)this.connection);
            try
            {
                if(this.connection.State!=ConnectionState.Open)
                    this.connection.Open();
                IDataReader dbReader = command.ExecuteReader();
                return dbReader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IDataReader executeQuery(IDbCommand sql)
        {
            //not done: Moq for mocking
            sql.Connection = this.connection;
            try
            {
                if (this.connection.State != ConnectionState.Open)
                    this.connection.Open();
                IDataReader dbReader = sql.ExecuteReader();
                return dbReader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int executeNonQuery(string sql)
        {
            //not done: Moq for mocking
            IDbCommand command = new SQLiteCommand(sql, (SQLiteConnection)this.connection);
            try
            {
                if (this.connection.State != ConnectionState.Open)
                    this.connection.Open();
                int noRowsAffected = command.ExecuteNonQuery();
                return noRowsAffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int executeNonQuery(IDbCommand sql)
        {
            //not done: Moq for mocking
            sql.Connection = this.connection;
            try
            {
                if (this.connection.State != ConnectionState.Open)
                    this.connection.Open();
                int noRowsAffected = sql.ExecuteNonQuery();
                return noRowsAffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void loadDB(String filePath)
        {
            //not done: Moq for mocking
            if (!File.Exists(filePath))
                throw new FileNotFoundException();
            this.connectionString = $"Data Source={filePath};Version=3;";
            this.connection = new SQLiteConnection(this.connectionString);
            try
            {
                this.connection.Open();
                this.connection.Close();
            }
            catch (Exception e)
            {
                throw new IOException("Unable to open the database");
            }
            finally
            {
                this.connection.Close();
            }
        }
        public void insertTestData()
        {
            // 1. Thêm Kiểu dữ liệu (Type)
            // Giả định OID tự sinh sẽ là: 1 (INT) và 2 (VARCHAR)
            string insertTypes = @"
                INSERT INTO fprdb_Type (type_name, type_type) VALUES ('INT', 'b');
                INSERT INTO fprdb_Type (type_name, type_type) VALUES ('VARCHAR', 'b');
            ";

            // 2. Thêm Lược đồ (Schema) 
            // Giả định OID tự sinh sẽ là: 1
            string insertSchema = @"
                INSERT INTO fprdb_RelationSchema (relschema_name) VALUES ('PersonSchema');
            ";

            // 3. Thêm Cột (Attribute)
            // Cột 'id': thuộc Lược đồ 1, kiểu INT (type_id = 1)
            // Cột 'name': thuộc Lược đồ 1, kiểu VARCHAR (type_id = 2)
            string insertAttributes = @"
                INSERT INTO fprdb_Attribute (att_relschema_id, att_number, att_name, att_type_id, att_type_mod, att_not_null) 
                VALUES (1, 1, 'id', 1, 0, 1);
                
                INSERT INTO fprdb_Attribute (att_relschema_id, att_number, att_name, att_type_id, att_type_mod, att_not_null) 
                VALUES (1, 2, 'name', 2, 255, 0);
            ";

            // 4. Thêm Khóa chính (Constraint)
            // con_type = 'p' (Primary Key), con_attributes = 'id', thuộc Lược đồ 1
            string insertConstraint = @"
                INSERT INTO fprdb_Constraint (con_name, con_type, con_attributes, con_relschema_id) 
                VALUES ('PK_Person', 'p', 'id', 1);
            ";

            // 5. Thêm Quan hệ (Relation)
            // American và Asian đều trỏ về rel_relation_schema = 1 (Tức là PersonSchema)
            string insertRelations = @"
                INSERT INTO fprdb_Relation (rel_name, rel_relation_schema) VALUES ('American', 1);
                INSERT INTO fprdb_Relation (rel_name, rel_relation_schema) VALUES ('Asian', 1);
            ";

            string fullScript = insertTypes + insertSchema + insertAttributes + insertConstraint + insertRelations;

            try
            {
                this.connection.Open();
                IDbCommand command = new SQLiteCommand(fullScript, (SQLiteConnection)this.connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                throw new IOException("Lỗi khi chèn dữ liệu test: " + ex.Message);
            }
            finally
            {
                this.connection.Close();
            }
        }
    }
}
