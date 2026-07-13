using System;
using System.Collections.Generic;
using Examples.Design.Enumerating.Generic;

namespace Examples.Design.Tests.Enumerating.Generic;

/// <summary>
/// Tests <see cref="Enumeration{T}" /> class.
/// </summary>
public partial class EnumerationTests
{

    [Theory]
    [MemberData(nameof(DataOfEqualObjects))]
    public void When_CallingEqualsWithEqualObjects_Then_ReturnsTrue(CardType? instanceA, CardType? instanceB, string reason)
    {
        Assert.True(EqualityComparer<CardType>.Default.Equals(instanceA, instanceB), reason);
        Assert.True(object.Equals(instanceA, instanceB), reason);
        Assert.True(instanceA == instanceB, reason);
    }

    public static readonly TheoryData<CardType?, CardType?, string> DataOfEqualObjects = new()
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

    //[Fact]
    //public void WhenCallingCompareTo()
    //{
    //    // Enumeration<CardType> is not IComparable<CardType>
    //    return;
    //}

    [Fact]
    public void When_CallingValueOf_Then_WorkAsExpected()
    {
        Assert.True(CardType.Amex == Enumeration<CardType>.Parse("Amex"));

        Assert.True(CardType.Amex == CardType.Parse("Amex"));
        Assert.True(CardType.Amex == CardType.Parse("amex"));
        Assert.True(CardType.Amex == CardType.Parse("1"));
        Assert.False(CardType.Amex == CardType.Parse("ameX"));

        Assert.True(CardType.Visa == CardType.Parse("Visa"));
        Assert.True(CardType.Visa == CardType.Parse("visa"));
        Assert.True(CardType.Visa == CardType.Parse("2"));
        Assert.False(CardType.Visa == CardType.Parse("vIsa"));

        Assert.True(CardType.MasterCard == CardType.Parse("MasterCard"));
        Assert.True(CardType.MasterCard == CardType.Parse("mastercard"));
        Assert.True(CardType.MasterCard == CardType.Parse("3"));
        Assert.False(CardType.MasterCard == CardType.Parse("mAsterCard"));

        // spell-checker:: disable-next-line
        Assert.Null(CardType.Parse("hogehoge"));
        Assert.Throws<ArgumentNullException>(() => CardType.Parse(null));
    }

    [Fact]
    public void When_UsedInSwitchStatement_Then_ResultIsNotNull()
    {
        var value1 = CardType.Parse("Visa");

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
