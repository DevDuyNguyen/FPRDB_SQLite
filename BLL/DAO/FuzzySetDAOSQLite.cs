using BLL.DTO;
using BLL.Interfaces;
using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BLL.Exceptions;

namespace BLL.DAO
{
    public class FuzzySetDAOSQLite:FuzzySetDAO
    {
        private DatabaseManager databaseManager;
        private DatabaseManager databseExportImport;

        public FuzzySetDAOSQLite(DatabaseManager databaseManager)
        {
            this.databaseManager = databaseManager;
        }
        public FuzzySetDAOSQLite() { }
        public List<T> convertStringToListOfT<T>(string str)
        {
            List<T> ans;
            var type = typeof(T);
            if (type== typeof(int) || type == typeof(float) || type==typeof(string))
            {
                ans = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(item => (T)Convert.ChangeType(item.Trim(), typeof(T)))
                    .ToList();
                return ans;
            }
            else
            {
                throw new NotSupportedException($"{typeof(T).Name} isn't supported");
            }
        }
        //not done: Moq for mocking
        public DiscreteFuzzySet<T> createDiscreteFuzzySet<T>(DiscreteFuzzySetDTO<T> fuzzySet)
        {
            try
            {
                string fieldTypeName = fuzzySet.fuzzySetType.ToString();
                long fieldTypeID = this.databaseManager.executeScalar<long>($"SELECT oid FROM fprdb_Type WHERE type_name='{fieldTypeName}'");
                string insert_fprdb_FuzzySet = $"INSERT INTO fprdb_FuzzySet (fuzzset_name, fuzzset_type_id) VALUES ('{fuzzySet.fuzzySetName}',{fieldTypeID})";
                this.databaseManager.executeNonQuery(insert_fprdb_FuzzySet);

                long fuzzySetID = this.databaseManager.executeScalar<long>($"SELECT oid FROM fprdb_FuzzySet WHERE fuzzset_name='{fuzzySet.fuzzySetName}'");
                string valueSetStr = string.Join(",", fuzzySet.valueSet);
                string membershipDegreesStr = string.Join(",", fuzzySet.membershipDegreeSet);
                string insert_fprdb_DiscreteFuzzySet = $"INSERT INTO fprdb_DiscreteFuzzySet (oid,fuzzset_x, fuzzset_membership_degree) VALUES ({fuzzySetID},'{valueSetStr}','{membershipDegreesStr}');";

                this.databaseManager.executeNonQuery(insert_fprdb_DiscreteFuzzySet);

                string selectNewlyCreatedFuzzSet = $@"
                    SELECT 
                        FS.oid,
                        FS.fuzzset_name,
                        type.type_name,
                        distFS.fuzzset_x,
                        distFS.fuzzset_membership_degree
                    FROM fprdb_FuzzySet AS FS
                    JOIN fprdb_DiscreteFuzzySet AS distFS ON FS.oid = distFS.oid
                    JOIN fprdb_Type AS type ON type.oid = FS.fuzzset_type_id
                    WHERE FS.fuzzset_name = '{fuzzySet.fuzzySetName}';
                    ";

                IDataReader reader = this.databaseManager.executeQuery(selectNewlyCreatedFuzzSet);
                DiscreteFuzzySet<T> res;
                if (reader.Read())
                {
                    List<T> valueList = convertStringToListOfT<T>((string)reader["fuzzset_x"]);
                    List<float> membershipDegreeList = convertStringToListOfT<float>((string)reader["fuzzset_membership_degree"]);
                    res = new DiscreteFuzzySet<T>(valueList, membershipDegreeList,
                        (string)reader["fuzzset_name"], Enum.Parse<FieldType>((string)reader["type_name"]));
                    return res;
                }
                else
                    throw new newlyCreatedTupleNotFoundException();

            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
