using BLL.Common;
using BLL.DTO;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Program
    {
        //not done: Moq for mocking
        static void test_createDiscreteFuzzySet()
        {
            CompositionRoot compRoot = new CompositionRoot();
            DatabaseManager dbMgr = compRoot.getDBMgr();
            FuzzySetDAO fuzzySetDAO = compRoot.getFuzzySetDAO();

            dbMgr.loadDB("C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestSqlite\\db1.db");
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

            dbMgr.loadDB("C:\\Users\\Phung\\Desktop\\nam4\\KLTN\\TestSqlite\\db1.db");
            ContinuousFuzzySetDTO fuzzySet = new ContinuousFuzzySetDTO(10,20,30,40,"random1" );

            fuzzySetDAO.createContinuousFuzzySet(fuzzySet);
        }
        static void Main()
        {
            //test_createContinuousFuzzySet();
        }
    }
}
