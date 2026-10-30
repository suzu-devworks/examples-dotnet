namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.LazyInitialization;

/// <summary>
/// Pattern 1: Lazy Initialization.
/// </summary>
/// <remarks>
/// <para>Generated with Google Gemini Pro 2.5.</para>
/// </remarks>
public class LazyInitializationTests
{
    [Fact]
    public void When_AccessingOrders_Then_LoadsOrdersOnlyOnFirstAccess()
    {
        var customer = new Customer { Id = 1, Name = "Taro Yamada" };

        // Assert: No DB access occurs immediately after creating a Customer object.
        Assert.Equal(0, customer.GetRepositoryCallCount());

        // Act: First, access the Orders property.
        var orders = customer.Orders;

        // Assert: The first access will result in one DB access.
        Assert.Equal(1, customer.GetRepositoryCallCount());
        Assert.Equal(2, orders.Count);

        // Act: The second time you access the Orders property.
        var ordersAgain = customer.Orders;

        // Assert: The second access does not increase DB access.
        Assert.Equal(1, customer.GetRepositoryCallCount());
        Assert.Equal(2, ordersAgain.Count);
    }
}
