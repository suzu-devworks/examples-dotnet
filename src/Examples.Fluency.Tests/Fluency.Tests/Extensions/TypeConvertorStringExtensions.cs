
using Examples.Fluency.Extensions;

namespace Examples.Serialization.Tests.Text;

/// <summary>
/// Tests <see cref="TypeConvertorStringExtensions" /> methods.
/// </summary>
public class TypeConvertorStringExtensionsTests
{
    public class ToIndexMethod
    {
        [Theory]
        [MemberData(nameof(ValidDataForToIndex))]
        public void When_ValidString_Then_ReturnsExpected(string input, Index expected)
        {
            var actual = input.ToIndex();

            Assert.Equal(expected, actual);
        }

        public static readonly TheoryData<string, Index> ValidDataForToIndex = new()
        {
            { "123", new Index(123) },
            { "^5", new Index(5, true) },
        };

        [Fact]
        public void When_Null_Then_ThrowsException()
        {
            string value = default!; // is null.
            var exception = Assert.Throws<ArgumentNullException>(() => value.ToIndex());

            Assert.Equal("Value cannot be null. (Parameter 'value')", exception.Message);
        }

        [Fact]
        public void When_Empty_Then_ThrowsException()
        {
            string value = string.Empty;
            var exception = Assert.Throws<ArgumentException>(() => value.ToIndex());

            Assert.Equal("Illegal value is [].", exception.Message);
        }
    }

    public class ToRangeMethod
    {
        [Theory]
        [MemberData(nameof(ValidDataForToRange))]
        public void When_ValidString_Then_ReturnsExpected(string input, Range expected)
        {
            var actual = input.ToRange();

            Assert.Equal(expected, actual);
        }

        public static readonly TheoryData<string, Range> ValidDataForToRange = new()
        {
            { "1..2", 1..2 },
            { "^5..^3", ^5..^3 },
        };

        [Fact]
        public void When_Null_Then_ThrowsException()
        {
            string value = default!; // is null.
            var exception = Assert.Throws<ArgumentNullException>(() => value.ToRange());

            Assert.Equal("Value cannot be null. (Parameter 'value')", exception.Message);
        }

        [Fact]
        public void When_Empty_Then_ThrowsException()
        {
            var value = string.Empty;
            var exception = Assert.Throws<ArgumentException>(() => value.ToRange());

            Assert.Equal("Illegal value is [].", exception.Message);
        }
    }
}
