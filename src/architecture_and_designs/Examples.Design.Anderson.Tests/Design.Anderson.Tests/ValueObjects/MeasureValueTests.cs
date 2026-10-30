namespace Examples.Design.Anderson.ValueObjects.Tests;

public class MeasureValueTests
{
    [Theory]
    [InlineData(0.0, "0m/s")]
    [InlineData(0.0050, "0m/s")]
    [InlineData(0.0051, "0.01m/s")]
    [InlineData(-13.005, "-13m/s")]
    [InlineData(-13.0051, "-13.01m/s")]
    public void When_GettingDisplayValue_Then_ReturnsExpectedFormat(double value, string expected)
    {
        // Add units by rounding up to three decimal places.
        var measure = new MeasureValue(value);
        Assert.Equal(expected, measure.DisplayValue);
    }


    [Theory]
    [MemberData(nameof(DataOfEquals))]
    public void When_CallingEquals_Then_WorkAsExpected(double value1, double value2, bool expected)
    {
        var measure1 = new MeasureValue(value1);
        var measure2 = new MeasureValue(value2);

        Assert.Equal(expected, measure1.Equals(measure2));
        Assert.Equal(expected, measure1 == measure2);
        Assert.Equal(!expected, measure1 != measure2);
    }

    public static readonly TheoryData<double, double, bool> DataOfEquals = new()
    {
        { 0.0, 0.0, true},
        { 0.0001, 0.0001, true},
        { 0.0000001, 0.00000011, false}
    };

}
