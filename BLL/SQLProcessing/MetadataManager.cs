using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BLL.DomainObject;

namespace BLL.SQLProcessing
{
    public class MetadataManager
    {
        public DatabaseManager databaseMgr;

        public MetadataManager(DatabaseManager databaseMgr)
        {
            this.databaseMgr = databaseMgr;
        }

        public bool isSchemaExist(string name)
        {
            try
            {
                string sql = $"select 1 from fprdb_RelationSchema where relschema_name='{name}'";
                IDataReader reader = this.databaseMgr.executeQuery(sql);
                return reader.Read();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool isConstraintExist(string name)
        {
            try
            {
                string sql = $"select 1 from fprdb_Constraint where con_name='{name}'";
                IDataReader reader = this.databaseMgr.executeQuery(sql);
                return reader.Read();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public FPRDBRelation getRelation(string name)
        //{

        //}
    }
}
