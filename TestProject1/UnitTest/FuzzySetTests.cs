using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainObject;

namespace TestProject1.UnitTest
{
    public class FuzzySetTests
    {
        private FuzzySet<string> stringDistFuzzSet;
        private FuzzySet<double> doubleDistFuzzSet;
        private FuzzySet<double> ContFuzzSet;

        public FuzzySetTests()
        {
            this.stringDistFuzzSet = new DiscreteFuzzySet<string>(
                new List<string>() { "a", "b", "c"},
                new List<float>() { 1, 0.5f, 0.1f}
                );
            this.doubleDistFuzzSet = new DiscreteFuzzySet<double>(
                new List<double>() { 12,23,40 },
                new List<float>() { 1, 0.5f, 0.1f }
                );
            this.ContFuzzSet = new ContinuousFuzzySet(10,20,30,49);
        }



    }
}
