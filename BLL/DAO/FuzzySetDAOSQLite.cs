using BLL.DomainObject;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    .Select(item => (T)Convert.ChangeType(item.Trim(), typeof(T), CultureInfo.InvariantCulture))
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
            if (fuzzySet.fuzzySetName == "" || fuzzySet.fuzzySetName == null)
                throw new InvalidDataException("Fuzzy set name is empty");
            if ( fuzzySet.valueSet.Count == 0)
                throw new InvalidDataException("Fuzzy set name's universe of discourse empty");
            if (fuzzySet.valueSet.Count!=fuzzySet.membershipDegreeSet.Count)
                throw new InvalidDataException("Fuzzy set number of elements in the universe of discourse doesn't match number of membership degrees");
            foreach(float degree in fuzzySet.membershipDegreeSet)
            {
                if (degree < 0 || degree > 1)
                    throw new InvalidDataException("Membership degree must be within [0,1]");
            }

            try
            {
                string checkIfFuzzSetExist = $" SELECT 1 FROM fprdb_FuzzySet AS FS  WHERE FS.fuzzset_name = '{fuzzySet.fuzzySetName}';";
                using(IDataReader r = this.databaseManager.executeQuery(checkIfFuzzSetExist))
                {
                    if (r.Read())
                        throw new InvalidDataException($"Fuzz set {fuzzySet.fuzzySetName} already exists");
                }

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
            catch(SQLiteException ex)
            {
                throw new SQLExecutionException("Something when wrong with sqlite");
            }

        }

        public ContinuousFuzzySet createContinuousFuzzySet(ContinuousFuzzySetDTO fuzzySet)
        {
            if (fuzzySet.fuzzySetName == "" || fuzzySet.fuzzySetName == null)
                throw new InvalidDataException("Fuzzy set name is empty");
            if (!(fuzzySet.leftBottom <= fuzzySet.leftTop && fuzzySet.leftTop <= fuzzySet.rightTop && fuzzySet.rightTop <= fuzzySet.rightBottom))
            {
                throw new InvalidDataException("Continuous fuzzy set's memberhsip degree function must be a trapazoid or triangle");
            }
            try
            {
                string checkIfFuzzSetExist = $" SELECT 1 FROM fprdb_FuzzySet AS FS  WHERE FS.fuzzset_name = '{fuzzySet.fuzzySetName}';";
                using (IDataReader r = this.databaseManager.executeQuery(checkIfFuzzSetExist))
                {
                    if (r.Read())
                        throw new InvalidDataException($"Fuzz set {fuzzySet.fuzzySetName} already exists");
                }

                string fieldTypeName = fuzzySet.fuzzySetType.ToString();
                long fieldTypeID = this.databaseManager.executeScalar<long>($"SELECT oid FROM fprdb_Type WHERE type_name='{fieldTypeName}'");
                string insert_fprdb_FuzzySet = $"INSERT INTO fprdb_FuzzySet (fuzzset_name, fuzzset_type_id) " +
                    $"VALUES ('{fuzzySet.fuzzySetName}',{fieldTypeID})";
                this.databaseManager.executeNonQuery(insert_fprdb_FuzzySet);

                long fuzzySetID = this.databaseManager.executeScalar<long>($"SELECT oid FROM fprdb_FuzzySet WHERE fuzzset_name='{fuzzySet.fuzzySetName}'");
                string insert_fprdb_ContinousFuzzySet = $"INSERT INTO fprdb_ContinousFuzzySet (oid,fuzzset_bottom_left, fuzzset_top_left, fuzzset_top_right, fuzzset_bottom_right) " +
                    $"VALUES ({fuzzySetID},{fuzzySet.leftBottom},{fuzzySet.leftTop},{fuzzySet.rightTop},{fuzzySet.rightBottom});";

                this.databaseManager.executeNonQuery(insert_fprdb_ContinousFuzzySet);

                string selectNewlyCreatedFuzzSet = $@"
                    SELECT 
                        FS.oid,
                        FS.fuzzset_name,
                        contFS.fuzzset_bottom_left,
                        contFS.fuzzset_top_left,
                        contFS.fuzzset_top_right,
                        contFS.fuzzset_bottom_right
                    FROM fprdb_FuzzySet AS FS
                    JOIN fprdb_ContinousFuzzySet AS contFS 
                        ON FS.oid = contFS.oid
                    WHERE FS.fuzzset_name = '{fuzzySet.fuzzySetName}';
                    ";

                IDataReader reader = this.databaseManager.executeQuery(selectNewlyCreatedFuzzSet);
                ContinuousFuzzySet res;
                if (reader.Read())
                {
                    
                    res = new ContinuousFuzzySet(
                        Convert.ToSingle(reader["fuzzset_bottom_left"]),
                        Convert.ToSingle(reader["fuzzset_top_left"]),
                        Convert.ToSingle(reader["fuzzset_top_right"]),
                        Convert.ToSingle(reader["fuzzset_bottom_right"]),
                        (string)reader["fuzzset_name"]);
                    return res;
                }
                else
                    throw new newlyCreatedTupleNotFoundException();

            }
            catch (SQLiteException ex)
            {
                throw new SQLExecutionException("Something went wrong with sqlite");
            }

        }
    }
}
