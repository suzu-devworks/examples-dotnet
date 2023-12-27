using System.ComponentModel;

namespace Examples.Fluency.Tests;

/// <summary>
/// Tests <see cref="EnumExtensions" /> methods.
/// </summary>
public class EnumExtensionsTests
{

    [Fact]
    public void WhenCallingGetDescription_ReturnsAsExpected()
    {
        Status.Active.GetDescription().Is("有効");
        Status.Published.GetDescription().Is("発行");
        Status.Canceled.GetDescription().Is("キャンセル");
        Status.Unknown.GetDescription().Is("Unknown");
    }

    [Fact]
    public void WhenCallingGetDescriptionUseReflection_ReturnsAsExpected()
    {
        Status.Canceled.GetDescriptionWithReflection().Is("キャンセル");
        Status.Unknown.GetDescriptionWithReflection().Is("Unknown");
    }

    [Fact]
    public void WhenCallingGetDescriptionWithValueCached_ReturnsAsExpected()
    {
        Status.Canceled.GetDescriptionWithValueCached().Is("キャンセル");
        Status.Unknown.GetDescriptionWithValueCached().Is("Unknown");
    }

    [Fact]
    public void WhenCallingGetDescriptionWithDelegateCached()
    {
        Status.Canceled.GetDescriptionWithDelegateCached().Is("キャンセル");
        Status.Unknown.GetDescriptionWithDelegateCached().Is("Unknown");
    }

    private enum Status
    {
        [Description("有効")]
        Active,
        [Description("発行")]
        Published,
        [Description("キャンセル")]
        Canceled,
        Unknown,
    }


}
