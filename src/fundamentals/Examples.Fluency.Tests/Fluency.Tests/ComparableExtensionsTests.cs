namespace Examples.Fluency.Tests;

/// <summary>
/// Tests <see cref="ComparableExtensions" /> methods.
/// </summary>
public class ComparableExtensionsTests
{
    public class BetweenMethod
    {
        [Fact]
        public void When_IntValidValues_Then_ReturnsAsExpected()
        {
            var target = 100;

            var atTheTime1 = target - 2;
            var atTheTime2 = target - 1;
            var atTheTime3 = target;
            var atTheTime4 = target + 1;
            var atTheTime5 = target + 2;

            // a1, a2 << X
            Assert.False(target.Between(atTheTime1, atTheTime2));
            // a2 < a3 == X
            Assert.True(target.Between(atTheTime2, atTheTime3));
            // X == a3 < a4
            Assert.True(target.Between(atTheTime3, atTheTime4));
            // X << a4, a5
            Assert.False(target.Between(atTheTime4, atTheTime5));
        }
    }
}
