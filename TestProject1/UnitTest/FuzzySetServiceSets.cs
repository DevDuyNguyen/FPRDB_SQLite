using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services;
using BLL.DTO;
using BLL;

namespace TestProject1.UnitTest
{
    public class FuzzySetServiceSets
    {
        private FuzzySetService service;

        public FuzzySetServiceSets()
        {
            //not done: Moq for mocking
            this.service = new FuzzySetService();
        }

        [Fact]
        //test case technique: positive test, negative test
        public void FuzzySetService_checkIfFuzzySetValid_succes()
        {
            //arrange
            FuzzySetDTO fuzz1 = new DiscreteFuzzySetDTO<int>(
                new List<int>() { 1 },
                new List<float>() { -1 },
                "",
                FieldType.INT
                );
            FuzzySetDTO fuzz2 = new DiscreteFuzzySetDTO<int>(
                new List<int>() { 1 },
                new List<float>() { 1 },
                "",
                FieldType.INT
                );
            FuzzySetDTO fuzz3 = new ContinuousFuzzySetDTO(1,2,3,4,"");
            FuzzySetDTO fuzz4 = new ContinuousFuzzySetDTO(12, 2, 3, 4, "");
            //act
            //assert
            Assert.Equal(false, this.service.checkIfFuzzySetValid(fuzz1));
            Assert.Equal(true, this.service.checkIfFuzzySetValid(fuzz2));
            Assert.Equal(true, this.service.checkIfFuzzySetValid(fuzz3));
            Assert.Equal(false, this.service.checkIfFuzzySetValid(fuzz4));

        }

    }
}
