namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.VirtualProxy;

/// <summary>
/// Pattern 2: Virtual Proxy.
/// </summary>
/// <remarks>
/// <para>Generated with Google Gemini Pro 2.5.</para>
/// </remarks>
public class VirtualProxyTests
{
    [Fact]
    public void When_CallingMethodOnProxy_Then_LoadsRealObjectOnlyOnFirstCall()
    {
        // Arrange
        ICustomer customerProxy = new CustomerProxy(2);

        // Assert: No DB access occurs immediately after creating a proxy object.
        Assert.Equal(0, customerProxy.GetRepositoryCallCount());

        // Act: First, access the Orders property.
        var orders = customerProxy.Orders;

        // Assert: The first access creates a real object, which results in one DB access.
        Assert.Equal(1, customerProxy.GetRepositoryCallCount());
        Assert.Equal(2, orders.Count);

        // Act: The second time you access the Name property.
        var name = customerProxy.Name;

        // Assert: The second access does not increase DB access.
        Assert.Equal(1, customerProxy.GetRepositoryCallCount());
        Assert.Equal("Real Customer 2", name);
    }
}
