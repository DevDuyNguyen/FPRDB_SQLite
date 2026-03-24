using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;


namespace BLL.DAO
{
    public class ConstraintDAOSQLite:ConstraintDAO
    {
        private DatabaseManager databaseMgr;

        public ConstraintDAOSQLite(DatabaseManager databaseMgr)
        {
            this.databaseMgr = databaseMgr;
        }

        public bool isTupleWithFuzzyProbabilisticValuesExist(string relation, List<string> attributeList, List<AbstractFuzzyProbabilisticValue> contentList)
        {
            string sql = $"SELECT 1 FROM {relation} WHERE";
            for (int i = 0; i < attributeList.Count; ++i)
            {
                sql += $" {attributeList[i]}='{contentList[i].ToString()}' AND";
            }
            int trailingANDIndex = sql.LastIndexOf("AND");
            sql = sql.Substring(0, trailingANDIndex);
            using (IDataReader r = this.databaseMgr.executeQuery(sql))
            {
                return r.Read();
            }

        }

    }
}
