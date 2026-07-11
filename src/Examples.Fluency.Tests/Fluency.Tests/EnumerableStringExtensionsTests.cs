namespace Examples.Fluency.Tests;

public class EnumerableStringExtensionsTests
{
    public class ToSeparatedStringMethod
    {
        [Fact]
        public void When_Null_Then_ThrowsArgumentNullException()
        {
            IEnumerable<string> inputs = null!;
            Assert.Throws<ArgumentNullException>(() => inputs.ToSeparatedString(","));
        }

        [Fact]
        public void When_Empty_Then_ReturnsEmptyString()
        {
            Assert.Equal("", Array.Empty<string>().ToSeparatedString(","));
        }

        [Fact]
        public void When_AllElementsAreNull_Then_ReturnsCommaOnlyString()
        {
            Assert.Equal(",,,,", (new string?[] { null, null, null, null, null }).ToSeparatedString(","));
        }

        [Theory]
        [MemberData(nameof(DataForToSeparatedString))]
        public void When_ValidValues_Then_ReturnsConcatenatedString(
            IEnumerable<string> inputs,
            string separator,
            string expected)
        {
            Assert.Equal(expected, inputs.ToSeparatedString(separator));
        }

        public static readonly TheoryData<string[], string, string> DataForToSeparatedString = new()
        {
            { Enumerable.Range(1, 10).Select(x => x.ToString()).ToArray(), ",", "1,2,3,4,5,6,7,8,9,10" },
            { new[] { "A", "B", "C", "D", "E" }, "=>", "A=>B=>C=>D=>E" },
        };
    }
}
