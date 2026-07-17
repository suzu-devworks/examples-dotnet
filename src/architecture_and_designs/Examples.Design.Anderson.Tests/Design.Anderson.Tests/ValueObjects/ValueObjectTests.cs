using System;

namespace Examples.Design.Anderson.ValueObjects.Tests;

public partial class ValueObjectTests
{
    [Fact]
    public void When_CallingEqualsReference_Then_WorkAsExpected()
    {
        Derived instance = new() { Id = 1, Name = "ABC", ExpiredAt = DateTime.Parse("2000-02-29T12:34:56") };

        Assert.True(instance.Equals(instance));

#pragma warning disable CS1718 // Comparison made to same variable
        Assert.True(instance == instance);
        Assert.False(instance != instance);
#pragma warning restore CS1718 // Comparison made to same variable

        Assert.False(instance.Equals(null));
        Assert.False(instance!.Equals(DateTime.Now));
    }

    [Theory]
    [MemberData(nameof(DataOfEquals))]
    public void When_CallingEquals_Then_WorkAsExpected(Derived value1, Derived? value2, bool expected)
    {
        Assert.Equal(expected, value1.Equals(value2));
        Assert.Equal(expected, value1 == value2);
        Assert.Equal(!expected, value1 != value2);
    }

    public static readonly TheoryData<Derived, Derived?, bool> DataOfEquals = new()
    {
        {
            new() { Id = 1, Name = "ABC", ExpiredAt = DateTime.Parse("2000-02-29T12:34:56") },
            new() { Id = 1, Name = "ABC", ExpiredAt = DateTime.Parse("2000-02-29T12:34:56") },
            true
        },
        {
            new() { Id = 1, Name = "ABC", ExpiredAt = DateTime.Parse("2000-02-29T12:34:56") },
            new() { Id = 1, Name = "ABC", ExpiredAt = DateTime.Parse("2000-02-29T12:34:56") },
            true
        },
        {
            new() { Id = 1, Name = "ABC", ExpiredAt = DateTime.Parse("2000-02-29T12:34:56") },
            new() { Id = 2, Name = "ABC", ExpiredAt = DateTime.Parse("2000-02-29T12:34:56") },
            false
        },
        {
            new() { Id = 1, Name = "ABC", ExpiredAt = DateTime.Parse("2000-02-29T12:34:56") },
            new() { Id = 1, Name = "ABCD", ExpiredAt = DateTime.Parse("2000-02-29T12:34:56") },
            false
        },
        {
            new() { Id = 1, Name = "ABC", ExpiredAt = DateTime.Parse("2000-02-29T12:34:56") },
            new() { Id = 1, Name = "ABC", ExpiredAt = DateTime.Parse("2000-02-29T12:34:57") },
            false
        },
        {
            new() { Id = 1, Name = "ABC", ExpiredAt = DateTime.Parse("2000-02-29T12:34:56") },
            null,
            false
        },
    };

}
