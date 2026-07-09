
namespace Examples.Management.Tests;

public partial class HashCodeCalculatorTests
{
    public class ValueTupleGetHashCodeMethod
    {
        private class MyCalculator : HashCodeCalculator.IHashCodeCalculator
        {
            public int GetHashCode(params object?[] fields)
            {
                if (fields is [var field1, var field2, var field3, .. var _])
                {
                    return (field1, field2, field3).GetHashCode();
                }

                if (fields is { Length: 0 })
                {
                    return 17; //dummy
                }

                throw new InvalidOperationException("This test only supports 3 fields.");
            }
        }

        private static readonly MyCalculator Calculator = new();

        [Theory]
        [InlineData(123, "Andy", "2020-02-29")]
        [InlineData(0, null, null)]
        public void When_DifferentObjectsWithSameValue_Then_ValidFields_Then_HashCodesMatch(int field1, string? field2, string? field3)
        {
            RunEqualTest(
                Calculator,
                new DataClass(field1, field2, field3 is null ? null : DateTime.Parse(field3)),
                new DataClass(field1, field2, field3 is null ? null : DateTime.Parse(field3))
            );
        }

        [Fact]
        public void When_DifferentObjectsWithDifferentValue_Then_ValidFields_Then_HashCodesDoNotMatch()
        {
            RunNotEqualTest(
                Calculator,
                new DataClass(123, "Andy", DateTime.Parse("2020-02-29")),
                new DataClass(123, "Bob", DateTime.Parse("2020-02-29"))
            );
        }

        [Fact]
        public void When_EmptyFields_Then_HashCodesMatch()
        {
            RunEmptyTest(Calculator);
        }
    }
}

