using Xunit;
using DivisorApp.Infrastructure.Math;

namespace DivisorApp.Tests
{
    public class DivisorCalculatorTests
    {
        private readonly DivisorCalculator _calculator = new();

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 2)]
        [InlineData(4, 3)]
        [InlineData(5, 2)]
        [InlineData(6, 4)]
        [InlineData(8, 4)]
        [InlineData(9, 3)]
        [InlineData(10, 4)]
        [InlineData(12, 6)]
        [InlineData(14, 4)]
        [InlineData(16, 5)]
        [InlineData(18, 6)]
        [InlineData(28, 6)]
        [InlineData(int.MaxValue, 2)]
        public void Should_Return_Expected_Divisor_Count(int number, int expected)
        {
            Assert.Equal(expected, _calculator.GetDivisorCount(number));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-12)]
        [InlineData(int.MinValue)]
        public void Should_Throw_For_Invalid_Input(int number)
        {
            Assert.Throws<System.ArgumentException>(() => _calculator.GetDivisorCount(number));
        }
    }
}
