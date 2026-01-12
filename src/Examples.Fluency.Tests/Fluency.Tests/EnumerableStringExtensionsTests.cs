namespace Examples.Fluency.Tests;

public class EnumerableStringExtensionsTests
{

    [Theory]
    [MemberData(nameof(DataForToSeparatedString))]
    public void When_CallingToSeparatedString_Then_ReturnsConcatenatedString(
        IEnumerable<string> inputs,
        string separator,
        string expected)
    {
        Assert.Equal(expected, inputs.ToSeparatedString(separator));
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
    public void When_CallingToSeparatedStringWithNullElements_Then_ReturnsConcatenatedString()
    {
        Assert.Equal("", (new string?[] { null, null, null, null, null }).ToSeparatedString(""));
    }

    [Fact]
    public void When_CallingToSeparatedString_WithEmpty_Then_ReturnsEmptyString()
    {
        Assert.Equal("", Array.Empty<string>().ToSeparatedString(","));
    }

}
