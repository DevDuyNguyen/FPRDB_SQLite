using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace BLL.DAO
{
    public class InDataBaseSQLFileSQLiteDAO:InDataBaseSQLFileDAO
    {
        private MetadataManager metaMgr;
        private DatabaseManager dbMgr;

        public InDataBaseSQLFileSQLiteDAO(MetadataManager metaMgr, DatabaseManager dbMgr)
        {
            this.metaMgr = metaMgr;
            this.dbMgr = dbMgr;
        }

        public void createFile(string fileName, string content)
        {
            if (this.metaMgr.isInDatabaseSQLFileExist(fileName))
                throw new InvalidOperationException($"In database sql file {fileName} already existed");
            string createSQLFile = $"INSERT INTO fprdb_inDatabaseSQLFile (fileName, fileContent)" +
                $"VALUES ('{fileName}','{content}')";
            this.dbMgr.executeNonQuery(createSQLFile);
        }
        public string getFileContent(string fileName)
        {
            if (!this.metaMgr.isInDatabaseSQLFileExist(fileName))
                throw new InvalidOperationException($"In database sql file {fileName} doesn't exist");
            using(IDataReader r=this.dbMgr.executeQuery($"SELECT fileContent FROM fprdb_inDatabaseSQLFile WHERE fileName='{fileName}'"))
            {
                r.Read() ;
                return r["fileContent"] as string;
            }
        }
        public void save(string fileName, String content)
        {
            if (!this.metaMgr.isInDatabaseSQLFileExist(fileName))
                throw new InvalidOperationException($"In database sql file {fileName} doesn't exist");
            string updateSQLFileContent =
                $"UPDATE fprdb_inDatabaseSQLFile " +
                $"SET fileContent='{content}'" +
                $"WHERE fileName='{fileName}'";
            this.dbMgr.executeNonQuery(updateSQLFileContent);
        }
        public void deleteFile(string fileName)
        {
            if (!this.metaMgr.isInDatabaseSQLFileExist(fileName))
                throw new InvalidOperationException($"In database sql file {fileName} doesn't exist");
            string deleteSQLFileContent =
                $"DELETE FROM fprdb_inDatabaseSQLFile " +
                $"WHERE fileName='{fileName}'";
            this.dbMgr.executeNonQuery(deleteSQLFileContent);
        }

    }
}
