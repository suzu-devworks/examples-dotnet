using System;
using System.Collections.Generic;
using Examples.Design.Enumerating.JavaStyle;

namespace Examples.Design.Tests.Enumerating.JavaStyle;

/// <summary>
/// This class contains tests for the Enumeration pattern implementation in the Examples.Design.Enumerating.JavaStyle namespace.
/// </summary>
public partial class EnumerationTests
{

    [Theory]
    [MemberData(nameof(DatesOfEqualObjects))]
    public void When_CallingEqualsWithEqualObjects_Then_ReturnsTrue(CardType? instanceA, CardType? instanceB, string reason)
    {
        Assert.True(EqualityComparer<CardType>.Default.Equals(instanceA, instanceB), reason);
        Assert.True(object.Equals(instanceA, instanceB), reason);
        Assert.True(instanceA == instanceB, reason);
    }

    public static readonly TheoryData<CardType?, CardType?, string> DatesOfEqualObjects = new()
    {
        {
            null,
            null,
            "they should be equal because they are both null"
        },
        {
            CardType.Amex,
            CardType.Amex,
            "they should be equal because they are the same object"
        },
    };

    [Theory]
    [MemberData(nameof(DataOfNotEqualObjects))]
    public void When_CallingEqualsWithNotEqualObjects_Then_ReturnsFalse(CardType? instanceA, CardType? instanceB, string reason)
    {
        Assert.False(EqualityComparer<CardType>.Default.Equals(instanceA, instanceB), reason);
        Assert.False(object.Equals(instanceA, instanceB), reason);
        Assert.True(instanceA != instanceB, reason);
    }

    public static readonly TheoryData<CardType?, CardType?, string> DataOfNotEqualObjects = new()
    {
        {
            null,
            CardType.Amex,
            "they should not be equal because the left side is null"
        },
        {
            CardType.Amex,
            null,
            "they should not be equal because the right side is null"
        },
        {
            CardType.Amex,
            CardType.Visa,
            "they should not be equal because the right side is null"
        },
    };

    [Fact]
    public void When_CallingCompareTo_Then_ComparisonResultsAreCorrect()
    {
        Assert.True(CardType.Amex.CompareTo(CardType.ValueOf("Amex")) == 0);
        Assert.False(CardType.Amex.CompareTo(CardType.ValueOf("amex")) == 0);
        Assert.False(CardType.Amex.CompareTo(CardType.ValueOf("Visa")) == 0);
    }

    [Fact]
    public void When_CallingValueOf_Then_ReturnsExpectedValue()
    {
        Assert.True(CardType.Amex == Enumeration<CardType>.ValueOf("Amex"));
        Assert.True(CardType.Amex == CardType.ValueOf("Amex"));
        Assert.True(CardType.Amex == CardType.ValueOf("amex", ignoreCase: true));
        Assert.False(CardType.Amex == CardType.ValueOf("amex"));
        Assert.False(CardType.Amex == CardType.ValueOf("Visa"));
    }

    [Fact]
    public void When_UsedInSwitchStatement_Then_ResultIsNotNull()
    {
        var value1 = CardType.ValueOf("Visa");

        var value2 = value1 switch
        {
            var x when x == CardType.Amex => null,
            var x when x == CardType.Visa => x,
            var x when x == CardType.MasterCard => null,
            _ => throw new NotSupportedException($"{value1}"),
        };
        Assert.NotNull(value2);
    }

}
