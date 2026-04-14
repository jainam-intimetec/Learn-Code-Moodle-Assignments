using Xunit;
using DivisorApp.Application.Services;
using DivisorApp.Infrastructure.Math;

namespace DivisorApp.Tests
{
    public class ConsecutiveDivisorServiceTests
    {
        private readonly ConsecutiveDivisorService _service = new(new DivisorCalculator());

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(3, 1)]
        [InlineData(4, 1)]
        [InlineData(5, 1)]
        [InlineData(6, 1)]
        [InlineData(14, 1)]
        [InlineData(15, 2)]
        [InlineData(16, 2)]
        [InlineData(17, 2)]
        [InlineData(22, 3)]
        [InlineData(27, 4)]
        public void Should_Return_Expected_Count_For_Limit(int limit, int expected)
        {
            int result = _service.CountValidNumbers(limit);

            Assert.Equal(expected, result);
        }
    }
}
