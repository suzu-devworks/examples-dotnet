namespace Examples.Management.Tests;

public partial class HashCodeCalculatorTests
{

    public class HashCodeToHashCodeMethod
    {
        private class MyCalculator : HashCodeCalculator.IHashCodeCalculator
        {
            public int GetHashCode(params object?[] fields)
            {
                System.HashCode hash = new();
                foreach (var field in fields)
                {
                    hash.Add(field?.GetHashCode() ?? 0);
                }

                return hash.ToHashCode();
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

    public class HashCodeCombineCodeMethod
    {
        private class MyCalculator : HashCodeCalculator.IHashCodeCalculator
        {
            public int GetHashCode(params object?[] fields)
            {
                if (fields is [var field1, var field2, var field3, .. var _])
                {
                    return System.HashCode.Combine(field1, field2, field3);
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

