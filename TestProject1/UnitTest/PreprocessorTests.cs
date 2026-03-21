using BLL.DomainObject;
using BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.UnitTest
{
    public class PreprocessorTests
    {
        class checkComparisonOperatorOnFieldConstant_positive_testdata:TheoryData<Field, Constant, CompareOperation>
        {
            public checkComparisonOperatorOnFieldConstant_positive_testdata()
            {
                Add()
            }
        }


    }
}
