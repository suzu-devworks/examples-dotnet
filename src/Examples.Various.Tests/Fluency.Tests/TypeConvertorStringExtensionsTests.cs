namespace Examples.Fluency.Tests;

public class TypeConvertorStringExtensionsTests
{
    [Fact]
    public void WhenConvertString_ReturnsAsExpected()
    {
        "123".With(s => Convert.ToInt32(s)).Is(123);
        "2000-02-29".With(s => Convert.ToDateTime(s)).Is(DateTime.Parse("2000-02-29"));
    }

    [Fact]
    public void WhenConvertString_WithFailureData_ReturnsDependsOnConverter()
    {
        "ABC".With(s => int.TryParse(s, out int @value) ? @value : 0).Is(0);
        "ABC".With(s => int.TryParse(s, out int @value) ? @value : (int?)null).IsNull();
    }
}
