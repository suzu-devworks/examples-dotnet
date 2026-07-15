using System.ComponentModel;

namespace Examples.Types.Tests;

/// <summary>
/// Tests <see cref="EnumExtensions" /> methods.
/// </summary>
public class EnumExtensionsTests
{
    private enum Status
    {
        [Description("Active status")]
        Active,

        [Description("Published status")]
        Published,

        [Description("Canceled status")]
        Canceled,

        Unknown,
    }

    [Fact]
    public void When_UsingRepresentativeMethod_Then_ReturnsAsExpected()
    {
        Assert.Equal("Active status", Status.Active.GetDescription());
        Assert.Equal("Published status", Status.Published.GetDescription());
        Assert.Equal("Canceled status", Status.Canceled.GetDescription());
        Assert.Equal("Unknown", Status.Unknown.GetDescription());
    }

    [Fact]
    public void When_UsingReflection_Then_ReturnsAsExpected()
    {
        Assert.Equal("Active status", Status.Active.GetDescriptionWithReflection());
        Assert.Equal("Published status", Status.Published.GetDescriptionWithReflection());
        Assert.Equal("Canceled status", Status.Canceled.GetDescriptionWithReflection());
        Assert.Equal("Unknown", Status.Unknown.GetDescriptionWithReflection());
    }

    [Fact]
    public void When_UsingValueCached_Then_ReturnsAsExpected()
    {
        Assert.Equal("Active status", Status.Active.GetDescriptionWithValueCached());
        Assert.Equal("Published status", Status.Published.GetDescriptionWithValueCached());
        Assert.Equal("Canceled status", Status.Canceled.GetDescriptionWithValueCached());
        Assert.Equal("Unknown", Status.Unknown.GetDescriptionWithValueCached());
    }

    [Fact]
    public void When_UsingDelegateCached_Then_ReturnsAsExpected()
    {
        Assert.Equal("Active status", Status.Active.GetDescriptionWithDelegateCached());
        Assert.Equal("Published status", Status.Published.GetDescriptionWithDelegateCached());
        Assert.Equal("Canceled status", Status.Canceled.GetDescriptionWithDelegateCached());
        Assert.Equal("Unknown", Status.Unknown.GetDescriptionWithDelegateCached());
    }
}
