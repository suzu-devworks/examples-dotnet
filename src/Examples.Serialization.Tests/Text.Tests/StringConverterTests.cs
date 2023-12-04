namespace Examples.Text.Tests;

/// <summary>
/// Tests <see cref="StringConverter" /> methods.
/// </summary>
public class StringConverterTests
{

    [Theory]
    [MemberData(nameof(ValidDataForToIndex))]
    public void WhenCallingToIndex_WithValidValue(string input, Index expected)
    {
        StringConverter.ToIndex(input).Is(expected);
    }

    public static readonly TheoryData<string, Index> ValidDataForToIndex = new()
    {
        { "123", new Index(123) },
        { "^5", new Index(5, true) },
    };

    [Fact]
    public void WhenCallingToIndex_WithInvalidValue_ThrowAnException()
    {
        Assert.Throws<ArgumentNullException>(() => StringConverter.ToIndex(null))
            .Message.Is("Value cannot be null. (Parameter 'value')");
        Assert.Throws<ArgumentException>(() => StringConverter.ToIndex(""))
            .Message.Is("Illegal value is [].");
    }

    [Theory]
    [MemberData(nameof(ValidDataForToRange))]
    public void WhenCallingToRange_WithValidValue(string input, Range expected)
    {
        StringConverter.ToRange(input).Is(expected);
    }

    public static readonly TheoryData<string, Range> ValidDataForToRange = new()
    {
        { "1..2", 1..2 },
        { "^5..^3", ^5..^3 },
    };

    [Fact]
    public void WhenCallingToRange_WithInvalidValue_ThrowAnException()
    {
        Assert.Throws<ArgumentNullException>(() => StringConverter.ToRange(null))
            .Message.Is("Value cannot be null. (Parameter 'value')");
        Assert.Throws<ArgumentException>(() => StringConverter.ToRange(""))
            .Message.Is("Illegal value is [].");
    }

}
