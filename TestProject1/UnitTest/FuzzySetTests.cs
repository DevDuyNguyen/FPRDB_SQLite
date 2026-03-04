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
            this.ContFuzzSet = new ContinuousFuzzySet(10,20,30,40);
        }

        [Theory]
        [InlineData("a", 1)]
        [InlineData("b", 0.5f)]
        [InlineData("", 0)]
        [InlineData("123", 0)]
        //testcase techinques: equivalence class
        public void DiscreteFuzzySet_getMembershipDegree_returnRightMembershipDegree(string element, float membershipDegree)
        {
            //arrange
            //action
            //assert
            Assert.Equal(membershipDegree, this.stringDistFuzzSet.getMembershipDegree(element), 10);
        }

        [Theory]
        [InlineData(9, 0)]
        [InlineData(10,0)]
        [InlineData(11, 0.1)]
        [InlineData(20, 1)]
        [InlineData(21, 1)]
        [InlineData(30, 1)]
        [InlineData(31, 0.9)]
        [InlineData(40, 0)]
        [InlineData(41, 0)]
        //testcase technique: boundary
        public void ContinousFuzzySet_getMembershipDegree_returnRightMembershipDegree(double element, float membershipDegree)
        {
            //arrange
            //action
            //assert
            Assert.Equal(membershipDegree, this.ContFuzzSet.getMembershipDegree(element), 10);
        }

    }
}
