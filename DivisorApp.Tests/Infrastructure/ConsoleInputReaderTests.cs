using System.IO;
using Xunit;
using DivisorApp.Infrastructure;

namespace DivisorApp.Tests
{
    public class ConsoleInputReaderTests
    {
        [Fact]
        public void Should_Parse_Valid_Integer_Input()
        {
            using var reader = new StringReader("42\r\n");

            int result = ConsoleInputReader.ReadRequiredInt(reader);

            Assert.Equal(42, result);
        }

        [Fact]
        public void Should_Throw_For_Blank_Line_Input()
        {
            using var reader = new StringReader("\r\n");

            Assert.Throws<FormatException>(() => ConsoleInputReader.ReadRequiredInt(reader));
        }

        [Fact]
        public void Should_Throw_For_Whitespace_Only_Input()
        {
            using var reader = new StringReader("   \r\n");

            Assert.Throws<FormatException>(() => ConsoleInputReader.ReadRequiredInt(reader));
        }

        [Fact]
        public void Should_Throw_For_End_Of_Stream()
        {
            using var reader = new StringReader(string.Empty);

            Assert.Throws<ArgumentNullException>(() => ConsoleInputReader.ReadRequiredInt(reader));
        }

        [Fact]
        public void Should_Parse_Zero_Input()
        {
            using var reader = new StringReader("0\r\n");

            int result = ConsoleInputReader.ReadRequiredInt(reader);

            Assert.Equal(0, result);
        }
    }
}
