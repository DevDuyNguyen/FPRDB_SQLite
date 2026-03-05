
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //create database structure
            string create_fprdb_RelationSchema = "CREATE TABLE fprdb_RelationSchema(" +
                    "oid INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "relschema_name TEXT NOT NULL UNIQUE" +
                    ");";
            string create_fprdb_Relation = "CREATE TABLE fprdb_Relation(" +
                "oid INTEGER PRIMARY KEY AUTOINCREMENT," +
                "rel_name TEXT NOT NULL UNIQUE," +
                "rel_relation_schema INTEGER NOT NULL," +
                "FOREIGN KEY (rel_relation_schema) REFERENCES fprdb_RelationSchema (oid)" +
                ");";
            string create_fprdb_Type = "CREATE TABLE fprdb_Type(" +
                "oid INTEGER PRIMARY KEY AUTOINCREMENT," +
                "type_name TEXT NOT NULL UNIQUE," +
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
                "fuzzset_name TEXT NOT NULL UNIQUE," +
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
                "con_name TEXT NOT NULL UNIQUE," +
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

            //fill initial database content
            string insert_fprdb_Type = @"INSERT INTO fprdb_Type (type_name, type_type)
                VALUES 
                ('INT', 'b'),
                ('FLOAT', 'b'),
                ('CHAR', 'b'),
                ('VARCHAR', 'b'),
                ('BOOLEAN', 'b'),
                ('distFS_INT', 'fs'),
                ('distFS_FLOAT', 'fs'),
                ('distFS_TEXT', 'fs'),
                ('contFS', 'fs');";

            string statemt = create_fprdb_RelationSchema + create_fprdb_Relation + create_fprdb_Type +
                create_fprdb_Attribute + create_fprdb_FuzzySet + create_fprdb_DiscreteFuzzySet
                + create_fprdb_ContinousFuzzySet + create_fprdb_Relation_Fuzzyset + create_fprdb_Constraint
                +insert_fprdb_Type;

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
            finally
            {
                this.connection.Close();
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
            finally
            {
                this.connection.Close();
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
        public T executeScalar<T>(string sql)
        {
            try
            {
                if (this.connection.State != ConnectionState.Open)
                    this.connection.Open();
                IDbCommand command = new SQLiteCommand(sql, (SQLiteConnection)this.connection);
                return (T)command.ExecuteScalar();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.connection.Close();
            }
        }



    }
}
