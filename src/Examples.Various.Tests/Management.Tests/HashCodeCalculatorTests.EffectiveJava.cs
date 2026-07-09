namespace Examples.Management.Tests;

public partial class HashCodeCalculatorTests
{
    public class BasedOnEffectiveJavaGetHashCodeMethod
    {
        [Theory]
        [InlineData(123, "Andy", "2020-02-29")]
        [InlineData(0, null, null)]
        public void When_DifferentObjectsWithSameValue_Then_ValidFields_Then_HashCodesMatch(int field1, string? field2, string? field3)
        {
            RunEqualTest(
                Management.HashCodeCalculator.BasedOnEffectiveJava,
                new DataClass(field1, field2, field3 is null ? null : global::System.DateTime.Parse(field3)),
                new DataClass(field1, field2, field3 is null ? null : global::System.DateTime.Parse(field3))
            );
        }

        [Fact]
        public void When_DifferentObjectsWithDifferentValue_Then_ValidFields_Then_HashCodesDoNotMatch()
        {
            RunNotEqualTest(
                Management.HashCodeCalculator.BasedOnEffectiveJava,
                new DataClass(123, "Andy", DateTime.Parse("2020-02-29")),
                new DataClass(123, "Bob", DateTime.Parse("2020-02-29"))
            );
        }

        [Fact]
        public void When_EmptyFields_Then_HashCodesMatch()
        {
            RunEmptyTest(Management.HashCodeCalculator.BasedOnEffectiveJava);
        }
    }
}

