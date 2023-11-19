namespace Examples.Fluency.Tests;

public class EnumerableStringExtensionsTests
{

    [Theory]
    [MemberData(nameof(DataForToSeparatedString))]
    public void WhenCallingToSeparatedString_ReturnsConcatenatedString(
        IEnumerable<string> inputs,
        string separator,
        string expected)
    {
        inputs.ToSeparatedString(separator).Is(expected);
    }

    public static IEnumerable<object[]> DataForToSeparatedString()
    {
        yield return new object[] {
                Enumerable.Range(1, 10).Select(x => x.ToString()),",",
                "1,2,3,4,5,6,7,8,9,10"};
        yield return new object[] {
                new[] { "A", "B", "C", "D", "E" } , "=>",
                "A=>B=>C=>D=>E"};
    }

    [Fact]
    public void WhenCallingToSeparatedString_WithNullElements_ReturnsConcatenatedString()
    {
        (new string?[] { null, null, null, null, null }).ToSeparatedString("").Is("");
    }

    [Fact]
    public void WhenCallingToSeparatedString_WithEmpty_ReturnsEmptyString()
    {
        Array.Empty<string>().ToSeparatedString(",").Is("");
    }

}
