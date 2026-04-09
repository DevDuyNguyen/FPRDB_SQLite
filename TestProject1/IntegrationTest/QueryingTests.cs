using BLL;
using BLL.Common;
using BLL.Interfaces;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.IntegrationTest
{
    public class QueryingTests
    {
        private string dbFile;
        private CompositionRoot compRoot;
        private SQLProcessor sqlProcessor;
        public QueryingTests()
        {
            this.dbFile = "C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestData\\TestData03\\TestData03.db";
            this.compRoot = new CompositionRoot();
            compRoot.getDBMgr().loadDB(this.dbFile);
            this.sqlProcessor = this.compRoot.getSQLProcessor();
        }

        //[Fact]
        public void selectTest()
        {
            //arrange
            string sql = "Select * from patient1,doctor1";
            //act
            Plan plan = this.sqlProcessor.createQueryPlan(sql);
            Scan scan = plan.open();
            //assert
            while (scan.next())
            {

            }
        }

        //[Fact]
        public void selectAllTest()
        {
            //arrange
            string sql = "select * from DOCTOR1";
            //act
            Plan plan = this.sqlProcessor.createQueryPlan(sql);
            Scan scan = plan.open();
            //assert
            while (scan.next())
            {

            }
        }

        //[Fact]
        public void selectAttributeTest()
        {
            //arrange
            string sql = "select P_ID, P_AGE from DIAGNOSE1";
            //act
            Plan plan = this.sqlProcessor.createQueryPlan(sql);
            Scan scan = plan.open();
            //assert
            while (scan.next())
            {

            }
        }
        //[Fact]
        public void selectAllCartesianProductTest()
        {
            //arrange
            string sql = "select * from DOCTOR1,DOCTOR2";
            //act
            Plan plan = this.sqlProcessor.createQueryPlan(sql);
            Scan scan = plan.open();
            //assert
            while (scan.next())
            {

            }
        }
        //[Fact]
        public void selectAllNatualJoin()
        {
            //arrange
            string sql = "select * from DOCTOR1 NATURAL JOIN ⨂_in DOCTOR2";
            //act
            Plan plan = this.sqlProcessor.createQueryPlan(sql);
            Scan scan = plan.open();
            //assert
            while (scan.next())
            {

            }
        }
        //[Fact]
        public void selectAllNatualJoinWhereID()
        {
            //arrange
            string sql = "select * from DOCTOR1 NATURAL JOIN ⨂_in DOCTOR2 where (D_AGE ⇒ middle_aged)[0.7,1]";
            //act
            Plan plan = this.sqlProcessor.createQueryPlan(sql);
            Scan scan = plan.open();
            //assert
            while (scan.next())
            {

            }
        }
        //[Fact]
        public void SelectAll_INTERSECTION_SelectionAll()
        {
            //arrange
            string sql = "select * from DIAGNOSE1 INTERSECT ⨂_in select * from DIAGNOSE2";
            //act
            Plan plan = this.sqlProcessor.createQueryPlan(sql);
            Scan scan = plan.open();
            //assert
            while (scan.next())
            {

            }
        }
        //[Fact]
        public void SelectAll_UNION_SelectionAll()
        {
            //arrange
            string sql = "select * from DIAGNOSE1 union ⨁_in select * from DIAGNOSE2";
            //act
            Plan plan = this.sqlProcessor.createQueryPlan(sql);
            Scan scan = plan.open();
            //assert
            while (scan.next())
            {

            }
        }
        //[Fact]
        public void SelectAll_DIFFERENECE_SelectionAll()
        {
            //arrange
            string sql = "select * from DIAGNOSE1 EXCEPT ⦵_in select * from DIAGNOSE2";
            //act
            Plan plan = this.sqlProcessor.createQueryPlan(sql);
            Scan scan = plan.open();
            //assert
            while (scan.next())
            {

            }
        }
        //[Fact]
        public void UNION_DIFFERENECE_INTERSECTION()
        {
            //arrange
            string sql = "select * from DIAGNOSE1 union ⨁_in select * from DIAGNOSE2 EXCEPT ⦵_in (select * from DIAGNOSE1 INTERSECT ⨂_in select * from DIAGNOSE2)";
            //act
            Plan plan = this.sqlProcessor.createQueryPlan(sql);
            Scan scan = plan.open();
            //assert
            while (scan.next())
            {

            }
        }
    }
}
