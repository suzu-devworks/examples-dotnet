
using Examples.Fluency;

namespace Examples.Serialization.Tests.Text;

/// <summary>
/// Tests <see cref="TypeConvertorStringExtensions" /> methods.
/// </summary>
public partial class TypeConvertorStringExtensionsTests
{
    public class ParseToMethod
    {
        [Fact]
        public void When_TargetTypeIsNull_Then_ThrowsArgumentNullException()
        {
            var input = "1";

            Assert.Throws<ArgumentNullException>(() => input.ParseTo(null!));
        }

        [Fact]
        public void When_ValidInput_Then_ReturnsConvertedValue()
        {
            var input = "123";

            var actual = input.ParseTo(typeof(int));

            Assert.Equal(123, actual);
        }
    }

    public class TryParseToMethod
    {
        [Fact]
        public void When_TargetTypeIsNull_Then_ThrowsArgumentNullException()
        {
            var input = "1";

            Assert.Throws<ArgumentNullException>(() => input.TryParseTo(null!, out _));
        }
    }
}
