using FluentAssertions;
using FPRDB_SQLite;
namespace TestProject1
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void Add_TwoNumbers_ReturnsCorrectSum()
        {
            // 1. Arrange
            var calc = new Calculator();
            int a = 5, b = 3;

            // 2. Act
            int result = calc.Add(a, b);

            // 3. Assert (Sử dụng FluentAssertions)
            result.Should().Be(8);
        }
    }
}