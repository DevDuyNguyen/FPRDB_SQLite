using BLL;
using BLL.DomainObject;
using BLL.SQLProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.UnitTest
{
    public class MassAssignmentTests
    {
        class createMassAssignment_positive_testdata:TheoryData<DiscreteFuzzySet<float>, List<VoteCrispDefinition<float>>>
        {
            public createMassAssignment_positive_testdata()
            {
                Add(
                    new DiscreteFuzzySet<float>(new List<float> { 3.0f, 4.0f, 5.0f, 6.0f }, new List<float> {0.2f, 0.5f, 0.9f, 1f },null,FieldType.distFS_FLOAT, -1),
                    new List<VoteCrispDefinition<float>> {
                        new VoteCrispDefinition<float>(new List<float>{ 6.0f}, 0.1f),
                        new VoteCrispDefinition<float>(new List<float>{ 6.0f, 5.0f }, 0.4f),
                        new VoteCrispDefinition<float>(new List<float>{ 6.0f,5.0f,4.0f}, 0.3f),
                        new VoteCrispDefinition<float>(new List<float>{ 6.0f, 5.0f, 4.0f,3.0f}, 0.2f)
                        
                    }
                    );

                Add(
                    new DiscreteFuzzySet<float>(new List<float> { 6.0f, 5.0f, 4.0f }, new List<float> { 0.3f, 1f, 0.3f }, null, FieldType.distFS_FLOAT, -1),
                    new List<VoteCrispDefinition<float>> {
                        new VoteCrispDefinition<float>(new List<float>{ 5.0f },0.7f),
                        new VoteCrispDefinition<float>(new List<float>{ 5.0f, 4.0f, 6.0f }, 0.3f)
                    }
                    );
            }
        }
        [Theory]
        [ClassData(typeof(createMassAssignment_positive_testdata))]
        public void MassAssignment_createMassAssignment_success(DiscreteFuzzySet<float> fs, List<VoteCrispDefinition<float>> expected)
        {
            //arrange

            //act
            List<VoteCrispDefinition<float>> actual = MassAssignment.createMassAssignment<float>(fs);
            //assert
            Assert.Equal(expected.Count, actual.Count);
            for(int i=0;  i<expected.Count; ++i)
            {
                Assert.Equal(expected[i].mass, actual[i].mass, 4);
                Assert.Equal(expected[i].subSet, actual[i].subSet);
            }
        }


    }
}
