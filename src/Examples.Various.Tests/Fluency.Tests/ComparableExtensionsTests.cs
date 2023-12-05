namespace Examples.Fluency.Tests;

/// <summary>
/// Tests <see cref="ComparableExtensions" /> methods.
/// </summary>
public class ComparableExtensionsTests
{

    [Fact]
    public void WhenCallingBetween_WithDateTime_ReturnsAsExpected()
    {
        var target = DateTime.Parse("2020-02-29");

        var atTheTime1 = target.AddMilliseconds(-2);
        var atTheTime2 = target.AddMilliseconds(-1);
        var atTheTime3 = target;
        var atTheTime4 = target.AddMilliseconds(+1);
        var atTheTime5 = target.AddMilliseconds(+2);

        // a1, a2 << X
        target.Between(atTheTime1, atTheTime2).IsFalse();
        // a2 < a3 == X
        target.Between(atTheTime2, atTheTime3).IsTrue();
        // X == a3 < a4
        target.Between(atTheTime3, atTheTime4).IsTrue();
        // X << a4, a5
        target.Between(atTheTime4, atTheTime5).IsFalse();

        return;
    }

}
