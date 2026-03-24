using BLL.Common;
using BLL.DAO;
using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.UnitTest
{
    public class FuzzySetDAOSQLiteTests
    {
        private FuzzySetDAOSQLite dao;
        private CompositionRoot compRoot;
        private string dbPath= "C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestSqlite\\db1.db";
        public FuzzySetDAOSQLiteTests()
        {
            //not done: Moq for mocking
            this.compRoot = new CompositionRoot();
            this.compRoot.getDBMgr().loadDB(this.dbPath);
            this.dao = new FuzzySetDAOSQLite(this.compRoot.getDBMgr(), this.compRoot.getMetaDataManger());
        }

        //[Theory]
        //[InlineData("1,2,3", 1,2,3)]
        public void FuzzySetDAOSQLite_convertStringToListOfT_successINT(string str, int exp1, int exp2, int exp3)
        {
            //arrange
            //act
            //assert
            //not done: Moq for mocking
            List<int> ans = this.dao.convertStringToListOfT<int>(str);
            Assert.Contains(exp1, ans);
            Assert.Contains(exp2, ans);
            Assert.Contains(exp3, ans);
        }
        //[Theory]
        //[InlineData("1.12,2.12,-3.12", 1.12, 2.12, -3.12)]
        public void FuzzySetDAOSQLite_convertStringToListOfT_successFLOAT(string str, float exp1, float exp2, float exp3)
        {
            //arrange
            //act
            //assert
            //not done: Moq for mocking
            List<float> ans = this.dao.convertStringToListOfT<float>(str);
            Assert.Equal(exp1, ans[0], 9);
            Assert.Equal(exp2, ans[1], 9);
            Assert.Equal(exp3, ans[2], 9);
        }
        //[Theory]
        //[InlineData("a,b,adsfa","a", "b", "adsfa")]
        public void FuzzySetDAOSQLite_convertStringToListOfT_successSTRING(string str, string exp1, string exp2, string exp3)
        {
            //arrange
            //act
            //assert
            //not done: Moq for mocking
            List<string> ans = this.dao.convertStringToListOfT<string>(str);
            Assert.Equal(exp1, ans[0]);
            Assert.Equal(exp2, ans[1]);
            Assert.Equal(exp3, ans[2]);
        }

        [Theory]
        //[InlineData("dist")]
        [InlineData("con")]
        public void findFuzzySet_success(string name)
        {
            //arrange
            //act
            List<BaseFuzzySet> actual = this.dao.findFuzzySet(name);
            //assert

        }


    }
}
